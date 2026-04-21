using Psz.Core.Identity.Models;
using Psz.Core.Common.Models;
using Psz.Core.Apps.Purchase.Models;
using iText.StyledXmlParser.Jsoup.Nodes;
using System.Data.SqlClient;
using Infrastructure.Data.Entities.Joins.Logistics;
using Psz.Core.Logistics.Models.Lagebewegung;

namespace Psz.Core.Logistics.Interfaces;

public interface IPlantBookingService
{
	ResponseModel<int> GetUserLager(UserModel user);
}
public interface IPlantBookingStrategy
{
	List<GetPlantBookingsResponseModel> GetPlantBookings(int lagerId, string searchValue, Infrastructure.Data.Access.Settings.SortingModel dataSorting, GetPlantBookingsRequestModel request, out int totalCount);
}
public interface IPlantBookingPostArtikel
{
	int InsertData(CreatePlantBookingRequestModel data, SqlConnection connection, SqlTransaction transaction);
	PSZ_Eingangskontrolle_TNEntity FetchData(int result);

}

//public interface IPlantBookingBookingStrategy
//{
//	Tuple<int, List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity>> UpdatePlantBooking(int? Verpackungsnr, Infrastructure.Data.Entities.Tables.Logistics.PlantBookingUpdateEntity entity, Psz.Core.Logistics.Models.PlantBookings.PlantBookingUpdateModel data, Identity.Models.UserModel user, Infrastructure.Services.Utils.TransactionsManager transaction);
//}
//public class LagerTnStrategy: IPlantBookingBookingStrategy
//{
//	public Tuple<int, List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity>> UpdatePlantBooking(int? Verpackungsnr, Infrastructure.Data.Entities.Tables.Logistics.PlantBookingUpdateEntity entity, Psz.Core.Logistics.Models.PlantBookings.PlantBookingUpdateModel data, Identity.Models.UserModel user, Infrastructure.Services.Utils.TransactionsManager transaction)
//	{
//		var updateResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.UpdateWithTransaction(entity, transaction.connection, transaction.transaction);
//		var currentData = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetByVerpackungNrTn(Verpackungsnr ?? -1);
//		var logs = PlantBookingLogHelper.GenerateLogForUpdates(user, new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(currentData, Infrastructure.Data.Entities.Tables.CTS.LagerAccessEnum.TN), data);
//		return new Tuple<int, List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity>>(updateResult, logs);
//	}
//}