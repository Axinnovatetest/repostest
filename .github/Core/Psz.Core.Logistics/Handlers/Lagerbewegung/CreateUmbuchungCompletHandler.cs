using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class CreateUmbuchungCompletHandler: IHandle<Identity.Models.UserModel, ResponseModel<long>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private List<Models.Lagebewegung.LagerbewegungDetailsModel> _data { get; set; }


		public CreateUmbuchungCompletHandler(Identity.Models.UserModel user, List<Models.Lagebewegung.LagerbewegungDetailsModel> listPosition)
		{
			this._user = user;
			this._data = listPosition;

		}

		public ResponseModel<long> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				long idLagerbewegung = -1;
				long response = -1;
				List<int> listIDPosition = new List<int>();
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();
				var insertedNrHeader = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.AddLagebewegungHeaderWithTransaction(new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity
				{
					typ = "Umbuchung",



				}, botransaction.connection, botransaction.transaction
			   );
				idLagerbewegung = (long)insertedNrHeader;
				var deletePosition = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.DeletelistPositionByIdLagerbewegungWithTransaction(idLagerbewegung, botransaction.connection, botransaction.transaction);
				foreach(var item in this._data)
				{
					string bezeichnung1 = null;
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.artikelNr);
					if(articleEntity != null)
					{
						bezeichnung1 = articleEntity.Bezeichnung1;
					}
					var insertedNr = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.AddlistUmbuchungPositionWithTransaction(new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungDetailsEntity
					{

						idLagerbewegung = idLagerbewegung,
						artikelNr = item.artikelNr,
						bezeichnung1 = bezeichnung1,
						einheit = item.einheit,
						anzahl = item.anzahl,
						lagerVon = item.lagerVon,
						artikelNrNach = item.artikelNrNach,
						bezeichnung1Nach = bezeichnung1,
						anzahlNach = item.anzahlNach,
						lagerNach = item.lagerNach,
						gebuchtVon = "Gebucht durch " + this._user.Username + " " + DateTime.Now.ToString("dd-MM-yyyy hh:mm"),

					}, botransaction.connection, botransaction.transaction);
					listIDPosition.Add(insertedNr);
					//idLagerbewegung = item.idLagerbewegung;
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
					response = idLagerbewegung;
				}
				else
				{
					response = -1;
				}
				return ResponseModel<long>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<long> Validate()
		{
			if(this._user == null

			)
			{
				return ResponseModel<long>.AccessDeniedResponse();
			}



			return ResponseModel<long>.SuccessResponse();
		}
	}
}