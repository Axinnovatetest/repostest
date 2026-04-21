using Psz.Core.Identity.Models;
using Psz.Core.Logistics.Interfaces;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public partial class PlantBookingService: IPlantBookingService
	{
		public ResponseModel<int> GetUserLager(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var werke = Infrastructure.Data.Access.Joins.CapitalRequestsJointsAccess.GetWerkeId(user.CompanyId);
			var companyLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetBySingleWerke(werke);
			if(companyLager is null || companyLager.Lagerort_id <= 0)
				return ResponseModel<int>.SuccessResponse(-1);

			return ResponseModel<int>.SuccessResponse(companyLager.Lagerort_id);
		}
	}
}
