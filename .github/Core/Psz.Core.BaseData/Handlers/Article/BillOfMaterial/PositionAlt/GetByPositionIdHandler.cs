using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByPositionIdHandler: IHandle<UserModel, ResponseModel<List<Models.Article.BillOfMaterial.BomPositionAlt>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetByPositionIdHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.BomPositionAlt>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<List<Models.Article.BillOfMaterial.BomPositionAlt>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBom(this._data)?.Select(x =>
						new Models.Article.BillOfMaterial.BomPositionAlt(x))?.ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.BillOfMaterial.BomPositionAlt>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.BomPositionAlt>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.BomPositionAlt>>.FailureResponse(key: "1", value: "BOM position not found");


			return ResponseModel<List<Models.Article.BillOfMaterial.BomPositionAlt>>.SuccessResponse();
		}
	}
}
