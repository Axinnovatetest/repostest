using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListLSGedruckt: IHandle<Identity.Models.UserModel, ResponseModel<ListeUpdatePacking>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetListLSGedruckt(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<ListeUpdatePacking> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new ListeUpdatePacking();
				var listeLieferschein = new List<long>();


				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.LSDruckerAccess.GetListeLSDruck();
				if(PackingListEntity != null && PackingListEntity.Count > 0)
				{
					PackingListEntity = PackingListEntity.OrderBy(a => a.angeboteNr).ToList();
					listeLieferschein = PackingListEntity.Select(k => k.angeboteNr).Distinct().ToList();
				}



				response.listeLieferscheine = listeLieferschein;
				return ResponseModel<ListeUpdatePacking>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ListeUpdatePacking> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ListeUpdatePacking>.AccessDeniedResponse();
			}

			return ResponseModel<ListeUpdatePacking>.SuccessResponse();
		}
	}
}

