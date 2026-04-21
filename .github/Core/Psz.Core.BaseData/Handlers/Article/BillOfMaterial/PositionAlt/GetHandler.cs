using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.PositionAlt
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.BillOfMaterial.BomPositionAlt>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.BillOfMaterial.BomPositionAlt> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.BillOfMaterial.BomPositionAlt>.SuccessResponse(
						new Models.Article.BillOfMaterial.BomPositionAlt(
							Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Get(this._data)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.BillOfMaterial.BomPositionAlt> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.BillOfMaterial.BomPositionAlt>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.Get(this._data) == null)
				return ResponseModel<Models.Article.BillOfMaterial.BomPositionAlt>.FailureResponse(key: "1", value: "BOM position not found");


			return ResponseModel<Models.Article.BillOfMaterial.BomPositionAlt>.SuccessResponse();
		}
	}
}
