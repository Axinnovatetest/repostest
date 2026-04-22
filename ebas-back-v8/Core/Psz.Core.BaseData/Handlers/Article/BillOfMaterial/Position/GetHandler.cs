using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.BillOfMaterial.BomPosition>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.BillOfMaterial.BomPosition> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var x = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data);
				return ResponseModel<Models.Article.BillOfMaterial.BomPosition>.SuccessResponse(
						new Models.Article.BillOfMaterial.BomPosition(x,
							Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBom(this._data),
							Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(x.Artikel_Nr_des_Bauteils ?? -1)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.BillOfMaterial.BomPosition> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.BillOfMaterial.BomPosition>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Get(this._data) == null)
				return ResponseModel<Models.Article.BillOfMaterial.BomPosition>.FailureResponse(key: "1", value: "BOM position not found");


			return ResponseModel<Models.Article.BillOfMaterial.BomPosition>.SuccessResponse();
		}
	}
}
