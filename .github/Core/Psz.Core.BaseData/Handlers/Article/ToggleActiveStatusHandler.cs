using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class ToggleActiveStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public ToggleActiveStatusHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var responseBody = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.ToggleActiveStatus(this._data);

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					ObjectLogHelper.getLog(this._user, this._data, "Aktiv", articleEntity.aktiv?.ToString(), (!(articleEntity.aktiv ?? true)).ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data);

				// -
				return ResponseModel<int>.SuccessResponse(responseBody);
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity.aktiv == true)
			{
				// - if, DEACTIVATING, check Parent should not be ACTIVE
				var parentBIds = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetParentIds(this._data);
				var parentBOMEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByArchiveStatus(parentBIds?.Distinct()?.ToList(), true)?.Take(5).ToList();
				if(parentBOMEntities != null && parentBOMEntities.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Article is used in BOM for [{string.Join(", ", parentBOMEntities.Select(x => x.ArtikelNummer))}]");
				}
			}
			else
			{
				// - if ACTIVATING, check BOM should not have DEACTIVATED items
				var childBIds = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetChildrenIds(this._data);
				var childBOMEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByArchiveStatus(childBIds?.Distinct()?.ToList(), false)?.Take(5).ToList();
				if(childBOMEntities != null && childBOMEntities.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Article has deactivated BOM items [{string.Join(", ", childBOMEntities.Select(x => x.ArtikelNummer))}]");
				}
			}
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();

		}
	}

}
