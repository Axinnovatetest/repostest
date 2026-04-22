using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdatePartialHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticlePartialModel _data { get; set; }


		public UpdatePartialHandler(Identity.Models.UserModel user, Models.Article.ArticlePartialModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				lock((Locks.ArticleEditLock.GetOrAdd(this._data.ArtikelNr, new object())))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var logs = LogUpdate();

					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
					var articleQuality = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.ArtikelNr)
						?? new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity { };
					var articleLogistics = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(this._data.ArtikelNr)
						?? new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity { };
					var articleProduction = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(this._data.ArtikelNr)
						?? new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity { };

					switch((Enums.ArticleEnums.ArticlePartialData)this._data.PartialData)
					{
						case Enums.ArticleEnums.ArticlePartialData.Overview:
							break;
						case Enums.ArticleEnums.ArticlePartialData.Data:
							break;
						case Enums.ArticleEnums.ArticlePartialData.Sales:
							break;
						case Enums.ArticleEnums.ArticlePartialData.Production:
							this._data.SetProductionEntity(articleEntity, articleProduction);
							break;
						case Enums.ArticleEnums.ArticlePartialData.Quality:
							this._data.SetQualityEntity(articleEntity, articleQuality);
							break;
						case Enums.ArticleEnums.ArticlePartialData.Logstics:
							this._data.SetLogisticsEntity(articleEntity, articleLogistics);
							break;
						default:
							break;
					}

					var artikelUpdated = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Edit(articleEntity);
					if(artikelUpdated > 0)
					{
						// save update logs
						if(logs.Count > 0)
						{
							Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
						}

						switch((Enums.ArticleEnums.ArticlePartialData)this._data.PartialData)
						{
							case Enums.ArticleEnums.ArticlePartialData.Overview:
								break;
							case Enums.ArticleEnums.ArticlePartialData.Data:
								break;
							case Enums.ArticleEnums.ArticlePartialData.Sales:
								break;
							case Enums.ArticleEnums.ArticlePartialData.Production:
								var _articleProduction = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(this._data.ArtikelNr);
								if(_articleProduction == null)
								{
									articleProduction.CreateTime = DateTime.Now;
									articleProduction.CreateUserId = this._user.Id;
									articleProduction.UpdateTime = DateTime.Now;
									articleProduction.UpdateUserId = this._user.Id;
									Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Insert(articleProduction);
								}
								else
								{
									articleProduction.Id = _articleProduction.Id;
									articleProduction.UpdateTime = DateTime.Now;
									articleProduction.UpdateUserId = this._user.Id;
									Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Update(articleProduction);
								}
								break;
							case Enums.ArticleEnums.ArticlePartialData.Quality:
								var _articleQuality = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.ArtikelNr);
								if(_articleQuality == null)
								{
									articleQuality.CreateTime = DateTime.Now;
									articleQuality.CreateUserId = this._user.Id;
									articleQuality.UpdateTime = DateTime.Now;
									articleQuality.UpdateUserId = this._user.Id;
									Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.Insert(articleQuality);
								}
								else
								{
									articleQuality.Id = _articleQuality.Id;
									articleQuality.UpdateTime = DateTime.Now;
									articleQuality.UpdateUserId = this._user.Id;
									Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.Update(articleQuality);
								}
								break;
							case Enums.ArticleEnums.ArticlePartialData.Logstics:
								var _articleLogistics = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(this._data.ArtikelNr);
								if(_articleLogistics == null)
								{
									articleLogistics.CreateTime = DateTime.Now;
									articleLogistics.CreateUserId = this._user.Id;
									articleLogistics.UpdateTime = DateTime.Now;
									articleLogistics.UpdateUserId = this._user.Id;
									Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.Insert(articleLogistics);
								}
								else
								{
									articleLogistics.Id = _articleLogistics.Id;
									articleLogistics.UpdateTime = DateTime.Now;
									articleLogistics.UpdateUserId = this._user.Id;
									Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.Update(articleLogistics);
								}
								break;
							default:
								break;
						}

						return ResponseModel<int>.SuccessResponse(artikelUpdated);
					}

					return ResponseModel<int>.SuccessResponse(0);
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogUpdate()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			var articleExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.ArtikelNr);

			if(articleEntity.ArtikelNummer != this._data.ArtikelNummer)
			{
				addLog(logs, "ArtikelNummer", articleEntity.ArtikelNummer, this._data.ArtikelNummer, Enums.ObjectLogEnums.Objects.Article.GetDescription());
			}

			if(articleEntity.Bezeichnung1 != this._data.Bezeichnung1)
			{
				addLog(logs, "Bezeichnung1", articleEntity.Bezeichnung1, this._data.Bezeichnung1, Enums.ObjectLogEnums.Objects.Article.GetDescription());
			}

			if(articleEntity.Bezeichnung2 != this._data.Bezeichnung2)
			{
				addLog(logs, "Bezeichnung2", articleEntity.Bezeichnung2, this._data.Bezeichnung2, Enums.ObjectLogEnums.Objects.Article.GetDescription());
			}

			// Copper attributes
			if(articleEntity.CuGewicht != this._data.CuGewicht)
			{
				addLog(logs, "CuGewicht", articleEntity.CuGewicht.ToString(), this._data.CuGewicht.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription());
			}
			if(articleEntity.DEL != this._data.DEL)
			{
				addLog(logs, "DEL", articleEntity.DEL.ToString(), this._data.DEL.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription());
			}
			if(articleEntity.VKFestpreis != this._data.VKFestpreis)
			{
				addLog(logs, "VKFestpreis", articleEntity.VKFestpreis.ToString(), this._data.VKFestpreis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription());
			}

			if(articleExtension != null)
			{
				if(articleExtension.CopperCostBasis != this._data.CopperCostBasis)
				{
					addLog(logs, "CopperCostBasis", articleExtension.CopperCostBasis.ToString(), this._data.CopperCostBasis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription());
				}
				if(articleExtension.CopperCostBasis150 != this._data.CopperCostBasis150)
				{
					addLog(logs, "CopperCostBasis150", articleExtension.CopperCostBasis150.ToString(), this._data.CopperCostBasis150.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription());
				}
			}
			else
			{
				if(this._data.CopperCostBasis.HasValue)
				{
					addLog(logs, "CopperCostBasis", "", this._data.CopperCostBasis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription());
				}
				if(this._data.CopperCostBasis150.HasValue)
				{
					addLog(logs, "CopperCostBasis150", "", this._data.CopperCostBasis150.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription());
				}
			}
			//
			return logs;
		}
		internal void addLog(
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> logs,
			string column, string oldValue, string newValue, string description)
		{
			logs.Add(new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity
			{
				Id = -1, //
				LastUpdateUserId = this._user.Id,
				LastUpdateTime = DateTime.Now,
				LastUpdateUsername = this._user.Username,
				LastUpdateUserFullName = this._user.Name,
				LogObject = Enums.ObjectLogEnums.Objects.Article.GetDescription(),
				LogDescription = formatLog(column, oldValue, newValue, description),
				LogObjectId = this._data.ArtikelNr
			});
		}
		internal string formatLog(string column, string oldVal, string newVal, string table)
		{
			return $"Update [{column}] from {{{oldVal}}} to {{{newVal}}} on [{table}]";
		}
	}
}
