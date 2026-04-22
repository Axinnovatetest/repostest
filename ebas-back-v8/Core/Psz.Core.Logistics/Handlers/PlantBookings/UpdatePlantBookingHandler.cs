using Psz.Core.Logistics.Enums;

namespace Psz.Core.Logistics.Handlers.PlantBookings;

public class UpdatePlantBookingHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
{
	private Identity.Models.UserModel _user { get; set; }
	private Psz.Core.Logistics.Models.PlantBookings.PlantBookingUpdateModel _data { get; set; }


	public UpdatePlantBookingHandler(Identity.Models.UserModel user, Psz.Core.Logistics.Models.PlantBookings.PlantBookingUpdateModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<int> Handle()
	{

		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var ReceivedQuantity = Infrastructure.Data.Access.Tables.CTS.Lagerbewegungen_ArtikelAccess.GeReceivedQuantity(this._data.Eingangslieferscheinnr ?? 0,this._data.LagerbewegungenId ?? 0);
			var TransferdQuantity = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetTransferQuantity(this._data.Eingangslieferscheinnr ?? -1);
			//
			
			if(this._data.Choice == false && this._data.ChoiceTransfer == true)
			{
				if(
					this._data.Menge <= 0 ||
				(this._data.Menge > this._data.Gesamtmenge)
			|| (this._data.Menge > (this._data.Gesamtmenge - ReceivedQuantity))
			|| (this._data.Gesamtmenge - this._data.Menge - ReceivedQuantity < 0)
			)
				{
					return ResponseModel<int>.FailureResponse($"Received quantity  {this._data.Menge} exceeded the transfered quantity {this._data.Gesamtmenge - ReceivedQuantity}  !");
				}
			}


			var entity = new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingUpdateEntity()
			{
				Aktiv = _data.Aktiv,
				Akzeptierte_Menge = _data.Akzeptierte_Menge,
				Artikelnummer = _data.Artikelnummer,
				Clock_Number = _data.Clock_Number,
				Geprufte_Prufmenge = _data.Geprufte_Prufmenge,
				Gesamtmenge = _data.Gesamtmenge,
				Inspektor = _data.Inspektor,
				Kunde = _data.Kunde,
				Laufende_Nummer = _data.Laufende_Nummer,
				Menge = _data.Menge,
				Prufentscheid = _data.Prufentscheid,
				Prufmenge = _data.Prufmenge,
				Pruftiefe = _data.Pruftiefe,
				Reklamierte_Menge = _data.Reklamierte_Menge,
				Resultat = _data.Resultat,
				Verpackungsnr = _data.Verpackungsnr,
				WE_Anzahl_VOH = _data.WE_Anzahl_VOH,
				Eingangslieferscheinnr = _data.Eingangslieferscheinnr,
				LagerbewegungenId = _data.LagerbewegungenId,
				Choice = _data.Choice,
				ChoiceTransfer = _data.ChoiceTransfer,
			};

			botransaction.beginTransaction();
			Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity currentData = new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
			List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity> logs = new List<PlantBookingsLogsEntity>();
			int updateResult = 0;
			int updateReceivedQuantity = 0;
			switch(_data.LagerortID)
			{
				case (int)LagerEnum.BETN:
				case (int)LagerEnum.GZTN:
				case (int)LagerEnum.TN:
				case (int)LagerEnum.WS:
					updateResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.UpdateWithTransaction(entity, botransaction.connection, botransaction.transaction);
					if(_data.ChoiceTransfer == true)
						updateReceivedQuantity = Infrastructure.Data.Access.Tables.CTS.Lagerbewegungen_ArtikelAccess.UpdateReceivedQuantityWithTransaction(this._data.LagerbewegungenId ?? -1, this._data.Menge, botransaction.connection, botransaction.transaction);
					currentData = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetByVerpackungNrTn(_data.Verpackungsnr ?? -1);
					logs = PlantBookingLogHelper.GenerateLogForUpdates(_user, new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(currentData, Infrastructure.Data.Entities.Tables.CTS.LagerAccessEnum.TN), _data);
					break;

				case (int)LagerEnum.Eigenfertigung:
					updateResult = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.UpdateData(new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(entity), botransaction.connection, botransaction.transaction);
					if(_data.ChoiceTransfer == true)
						updateReceivedQuantity = Infrastructure.Data.Access.Tables.CTS.Lagerbewegungen_ArtikelAccess.UpdateReceivedQuantityWithTransaction(this._data.LagerbewegungenId ?? -1, this._data.Menge, botransaction.connection, botransaction.transaction);
					currentData = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetByVerpackungNrCz(_data.Verpackungsnr ?? -1);
					logs = PlantBookingLogHelper.GenerateLogForUpdates(_user, new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(currentData, Infrastructure.Data.Entities.Tables.CTS.LagerAccessEnum.Eigenfertigung), _data);
					break;

				case (int)LagerEnum.Albanien:
					updateResult = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.UpdateData(new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_ALEntity(entity), botransaction.connection, botransaction.transaction);
					if(_data.ChoiceTransfer == true)
						updateReceivedQuantity = Infrastructure.Data.Access.Tables.CTS.Lagerbewegungen_ArtikelAccess.UpdateReceivedQuantityWithTransaction(this._data.LagerbewegungenId ?? -1, this._data.Menge, botransaction.connection, botransaction.transaction);
					currentData = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetByVerpackungNrAl(_data.Verpackungsnr ?? -1);
					logs = PlantBookingLogHelper.GenerateLogForUpdates(_user, new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(currentData, Infrastructure.Data.Entities.Tables.CTS.LagerAccessEnum.Albanien), _data);
					break;

				default:
					break;
			}

			var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

			if(botransaction.commit())
			{
				return ResponseModel<int>.SuccessResponse(updateResult * InsertLogsResult);
			}
			else
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
			}
		} catch(Exception e)
		{
			botransaction.rollback();
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<int> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<int>.AccessDeniedResponse();
		}

		return ResponseModel<int>.SuccessResponse();
	}
}
