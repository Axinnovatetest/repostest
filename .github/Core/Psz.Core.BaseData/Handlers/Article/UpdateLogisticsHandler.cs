using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateLogisticsHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticleModel _data { get; set; }


		public UpdateLogisticsHandler(Identity.Models.UserModel user, Models.Article.ArticleModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.ArticleEditLock.GetOrAdd(this._data.ArtikelNr, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var logs = LogUpdate();
					var artikelUpdated = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Edit(this._data.ToLogisticsEntity());
					if(artikelUpdated > 0)
					{
						// save update logs
						if(logs.Count > 0)
						{
							Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
						}

						var _artikelExtensionEntity = this._data.ToLogisticsExtension();
						var artikelExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(this._data.ArtikelNr);
						if(artikelExtensionEntity != null)
						{
							_artikelExtensionEntity.Id = artikelExtensionEntity.Id;
							Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.Update(_artikelExtensionEntity);
						}
						else
						{
							Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.Insert(_artikelExtensionEntity);
						}

						// - 2022-03-30
						CreateHandler.generateFileDAT(this._data.ArtikelNr);
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
