using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetPositionsHandler: IHandle<UserModel, ResponseModel<List<Models.Article.BillOfMaterial.BomPosition>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetPositionsHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.BomPosition>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var positionEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);
				var positionAltEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBoms(positionEntities
					?.Select(x => x.Nr)?.ToList());
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(positionEntities?.Select(x => x.Artikel_Nr_des_Bauteils ?? -1)?.ToList());

				return ResponseModel<List<Models.Article.BillOfMaterial.BomPosition>>.SuccessResponse(
					positionEntities?.Select(x => new Models.Article.BillOfMaterial.BomPosition(x,
					positionAltEntities?.FindAll(y => y.OriginalStucklistenNr == x.Nr)?.ToList(),
					articleEntities.FirstOrDefault(z => z.ArtikelNr == (x.Artikel_Nr_des_Bauteils ?? -1))))
					?.ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.BillOfMaterial.BomPosition>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.BomPosition>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.BomPosition>>.FailureResponse(key: "1", value: "BOM article not found");


			return ResponseModel<List<Models.Article.BillOfMaterial.BomPosition>>.SuccessResponse();
		}
	}
}
