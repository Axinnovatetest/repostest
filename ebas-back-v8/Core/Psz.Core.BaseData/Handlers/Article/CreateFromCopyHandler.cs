using System;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class CreateFromCopyHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.CreateFromCopyRequestModel _data { get; set; }

		public CreateFromCopyHandler(Identity.Models.UserModel user, Models.Article.CreateFromCopyRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				try
				{
					botransaction.beginTransaction();
					var refArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleNr);
					refArticle.ArtikelNummer = this._data.NewArticleNumber;
					refArticle.Bezeichnung1 = this._data.NewArticleDesignation;
					refArticle.Rahmen = null;
					refArticle.Rahmen2 = null;
					refArticle.Rahmenauslauf = null;
					refArticle.Rahmenauslauf2 = null;
					refArticle.Rahmenmenge = null;
					refArticle.Rahmenmenge2 = null;
					refArticle.RahmenNr = null;
					refArticle.RahmenNr = null;
					refArticle.EdiDefault = false;
					var insertedNr = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.AddWithTransaction(
						refArticle, botransaction.connection, botransaction.transaction);

					var insertedExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
						{
							ArtikelNr = insertedNr,
							CreatorID = this._user.Id,
							DateCreation = DateTime.Now
						}, botransaction.connection, botransaction.transaction);

					// - Quality Extension
					var qExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.ArticleNr);
					if(qExtension != null)
					{
						qExtension.ArticleId = insertedNr;
					}
					else
					{
						qExtension = new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity
						{
							ArticleId = insertedNr,
							CreateTime = DateTime.Now,
							CreateUserId = this._user.Id
						};
					}
					Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.InsertWithTransaction(qExtension, botransaction.connection, botransaction.transaction);

					// - Logistics Extension
					var lExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(this._data.ArticleNr);
					if(lExtension != null)
					{
						lExtension.ArticleId = insertedNr;
					}
					else
					{
						lExtension = new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity
						{
							ArticleId = insertedNr,
							CreateTime = DateTime.Now,
							CreateUserId = this._user.Id
						};
					}
					Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.InsertWithTransaction(lExtension, botransaction.connection, botransaction.transaction);

					// - STL
					var sExtension = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleNr);
					if(sExtension != null)
					{
						sExtension.ArticleId = insertedNr;
					}
					else
					{
						sExtension = new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
						{
							ArticleId = insertedNr,
							ArticleNumber = this._data.NewArticleNumber,
							LastUpdateTime = null,
							LastUpdateUserId = null,
							BomVersion = 0,
							BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation,
							BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription()
						};
					}
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.InsertWithTransaction(sExtension, botransaction.connection, botransaction.transaction);
					if(this._data.WithBOM)
					{
						var stl = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data.ArticleNr);
						if(stl != null && stl.Count > 0)
						{
							stl.ForEach(x => { x.Artikel_Nr = insertedNr; });
							Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.InsertWithTransaction(stl, botransaction.connection, botransaction.transaction);
							var altSTL = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBoms(stl.Select(x => x.Nr).ToList());
							if(altSTL != null && altSTL.Count > 0)
							{
								var newSTL = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticleWithTransaction(this._data.ArticleNr, botransaction.connection, botransaction.transaction);
								for(int i = 0; i < altSTL.Count; i++)
								{
									var p = newSTL.FirstOrDefault(x => x.Artikel_Nr_des_Bauteils == altSTL[i].ParentArtikelNr);
									if(p != null)
									{
										altSTL[i].OriginalStucklistenNr = p.Nr;
									}
									altSTL[i].LastUpdateUserId = -1;
									altSTL[i].LastUpdateTime = null;
								}
								// - 
								Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.InsertWithTransaction(altSTL, botransaction.connection, botransaction.transaction);
							}
						}
					}

					// -
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(ObjectLogHelper.getLog(this._user, insertedNr, "ArticleNumber", "", this._data.NewArticleNumber, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Add));


					if(botransaction.commit())
					{
						// - 2022-04-22
						CreateHandler.addToLagers(insertedNr);

						// - 2022-03-30
						CreateHandler.generateFileDAT(insertedNr, isNew: true);
					}

					// -
					return ResponseModel<int>.SuccessResponse(insertedNr);
				} catch(Exception ex)
				{
					botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(ex);
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction error");
					throw;
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleNr) == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found.");
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.NewArticleNumber) != null)
			{
				return ResponseModel<int>.FailureResponse($"Article Number [{this._data.NewArticleNumber}] exists.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
