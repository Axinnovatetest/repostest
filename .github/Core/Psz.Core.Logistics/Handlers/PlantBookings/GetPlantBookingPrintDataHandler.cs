using Infrastructure.Data.Access.Joins.Logistics;
using Psz.Core.Logistics.Enums;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetPlantBookingPrintDataHandler
	{

		private Core.Identity.Models.UserModel _user;
		private PrintedDataRequestModel _data;
		public GetPlantBookingPrintDataHandler(Core.Identity.Models.UserModel user, PrintedDataRequestModel data)
		{
			this._user = user;
			this._data=data;
		}
				public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				PrintedDataPlantBookingModel Datamodel = new();

				var fetchedData = _data.LagerId switch
				{
					(int)LagerEnum.Albanien => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetPrintDataAL(this._data.NummerVerpackung),
					(int)LagerEnum.Eigenfertigung => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetPrintDataCZ(this._data.NummerVerpackung),
					_ => Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetPrintDataTN(this._data.NummerVerpackung),
				};

				if(fetchedData is not null)
				{
					Datamodel = new Core.Logistics.Models.PlantBookings.PrintedDataPlantBookingModel(fetchedData);
				}
				else
				{
					throw new InvalidOperationException("Invalid Provided Data in the ticket model !");
				}
				if(fetchedData.Aktiv == 1)
				{
					var ticketlogitem = new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(Datamodel.Nummer_Verpackung)
					{
						CreationDate = DateTime.Now,
						LagerId = _data.LagerId,
						artikelnummer = Datamodel.Artikelnummer,
						ticketscount = 1,
						UserId = _user.Id,
						Userfullname = Datamodel.Inspektor,
						Username = _user.Username,
					};
					// log prinintg operation ...
					PlantBookingsTicketLogsAccess.Insert(ticketlogitem);
				}
				// generating ticket as PDF
				var response = Module.Logistic_ReportingService.GenerateLGTPlantBookingTicket(Enums.ReportingEnums.ReportType.PlantBookingTicket, new List<Core.Logistics.Models.PlantBookings.PrintedDataPlantBookingModel> { Datamodel });
				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
				public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}


