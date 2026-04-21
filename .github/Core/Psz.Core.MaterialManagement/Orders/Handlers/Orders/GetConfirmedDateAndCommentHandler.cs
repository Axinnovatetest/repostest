using Psz.Core.MaterialManagement.Orders.Models.Orders;

namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	public partial class OrderService
	{
		public ResponseModel<int> UpdateConfirmedDateAndComment(UserModel user, ConfirmedDateAndCommentModel data)
		{
			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			try
			{

				var orderPos = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.PositionId);
				var logs = new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
				if(orderPos.Bestatigter_Termin != data.ConfirmedDate)
				{
					logs.Add(new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity()
					{
						BestellungenNr = orderPos.Bestellung_Nr,
						DateTime = DateTime.Now,
						UserId = user.Id,
						LogType = "Position Modification",
						LogObject = "Bestellung",
						Origin = "MTM",
						LogText = $"[Bestellung] Position [{orderPos.Nr}] Confirmed Date changed from [{orderPos.Bestatigter_Termin}] to [{data.ConfirmedDate}]",
						Nr = orderPos.Bestellung_Nr,
						ProjektNr = 0,
						Username = user.Name,
					});
				}
				if(orderPos.Bemerkung_Pos != data.Comment)
				{
					logs.Add(new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity()
					{
						BestellungenNr = orderPos.Bestellung_Nr,
						DateTime = DateTime.Now,
						UserId = user.Id,
						LogType = "Position Modification",
						LogObject = "Bestellung",
						Origin = "MTM",
						LogText = $"[Bestellung] Position [{orderPos.Nr}] Comment changed",
						Nr = orderPos.Bestellung_Nr,
						ProjektNr = 0,
						Username = user.Name,
					});
				}
				if(orderPos.AB_Nr_Lieferant != data.ABNummer)
				{
					logs.Add(new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity()
					{
						BestellungenNr = orderPos.Bestellung_Nr,
						DateTime = DateTime.Now,
						UserId = user.Id,
						LogType = "Position Modification",
						LogObject = "Bestellung",
						Origin = "MTM",
						LogText = $"[Bestellung] Position [{orderPos.Nr}] AB Number changed from [{orderPos.AB_Nr_Lieferant}] to [{data.ABNummer}]",
						Nr = orderPos.Bestellung_Nr,
						ProjektNr = 0,
						Username = user.Name,
					});
				}
				if(logs != null && logs.Count > 0)
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.Insert(logs);
				var order = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(orderPos.Bestellung_Nr ?? -1);
				orderPos.Bestatigter_Termin = data.ConfirmedDate;
				orderPos.Bemerkung_Pos = data.Comment;
				orderPos.AB_Nr_Lieferant = data.ABNummer;
				var response = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Update(orderPos);

				return ResponseModel<int>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}