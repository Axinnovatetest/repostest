using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetLieferanrantenMitAnzahlHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Statistics.LieferantMitAnzahModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DateTime _d1 { get; set; }
		private DateTime _d2 { get; set; }
		public GetLieferanrantenMitAnzahlHandler(Identity.Models.UserModel user, DateTime d1, DateTime d2)
		{
			this._user = user;
			this._d1 = d1;
			this._d2 = d2;
		}
		public ResponseModel<List<Models.Statistics.LieferantMitAnzahModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<Models.Statistics.LieferantMitAnzahModel>();
				var LieferantenListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetListeLieferantMitAnzahlWareneingang(_d1, _d2);
				if(LieferantenListEntity != null && LieferantenListEntity.Count > 0)
					response = LieferantenListEntity.Select(k => new Models.Statistics.LieferantMitAnzahModel(k)).ToList();

				return ResponseModel<List<Models.Statistics.LieferantMitAnzahModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<List<Models.Statistics.LieferantMitAnzahModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Statistics.LieferantMitAnzahModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Statistics.LieferantMitAnzahModel>>.SuccessResponse();
		}
	}
}