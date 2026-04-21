using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class CreateUmbuchungDetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<int>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private List<Models.Lagebewegung.LagerbewegungDetailsModel> _data { get; set; }


		public CreateUmbuchungDetailsHandler(Identity.Models.UserModel user, List<Models.Lagebewegung.LagerbewegungDetailsModel> listPosition)
		{
			this._user = user;
			this._data = listPosition;

		}

		public ResponseModel<List<int>> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				long idLagerbewegung = -1;
				List<int> response = new List<int>();
				List<int> listIDPosition = new List<int>();
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();
				if(this._data != null && this._data.Count() > 0)
				{
					idLagerbewegung = this._data[0].idLagerbewegung;
				}
				var deletePosition = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.DeletelistPositionByIdLagerbewegungWithTransaction(idLagerbewegung, botransaction.connection, botransaction.transaction);
				foreach(var item in this._data)
				{
					var insertedNr = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.AddlistUmbuchungPositionWithTransaction(new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity
					{
						idLagerbewegung = item.idLagerbewegung,
						artikelNr = item.artikelNr,
						//artikelnummer = item.artikelnummer,
						bezeichnung1 = item.bezeichnung1,
						einheit = item.einheit,
						anzahl = item.anzahl,
						lagerVon = item.lagerVon,
						artikelNrNach = item.artikelNrNach,
						//artikelnummerNach = item.artikelnummerNach,
						bezeichnung1Nach = item.bezeichnung1Nach,
						anzahlNach = item.anzahlNach,
						lagerNach = item.lagerNach,
						bemerkung = item.bemerkung,
						gebuchtVon = "Gebucht durch " + this._user.Username + " " + DateTime.Now.ToString("dd-MM-yyyy hh:mm"),

					}, botransaction.connection, botransaction.transaction);
					listIDPosition.Add(insertedNr);
					idLagerbewegung = item.idLagerbewegung;
					//------------Update Menge
					Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.updateMengeArtikelImLagerWithTransaction(item.artikelNr, item.lagerVon, -item.anzahl, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.updateMengeArtikelImLagerWithTransaction(item.artikelNrNach, item.lagerNach, item.anzahlNach, botransaction.connection, botransaction.transaction);

				}

				//------------Upadte Header Lage bewegung
				var header = new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity
				{
					id = idLagerbewegung,
					gebucht = true,
					gebuchtVon = "Gebucht durch " + this._user.Username + " " + DateTime.Now.ToString("dd-MM-yyyy hh:mm")
				};

				var updateNr = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.updateLagebewegungHeaderWithTransaction(header, botransaction.connection, botransaction.transaction);


				if(botransaction.commit())
				{
					response = listIDPosition;
				}
				else
				{
					listIDPosition.Add(-1);
				}
				return ResponseModel<List<int>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<int>> Validate()
		{
			if(this._user == null

			)
			{
				return ResponseModel<List<int>>.AccessDeniedResponse();
			}



			return ResponseModel<List<int>>.SuccessResponse();
		}
	}
}
