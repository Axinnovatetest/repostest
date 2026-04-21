using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using System;

namespace Psz.Core.FinanceControl.Handlers.Accounting
{
	public class ZahlungskonditionenKundenUpdateHandler: IHandle<ZahlungskonditionenKundenUpdateModel, ResponseModel<int>>
	{

		private ZahlungskonditionenKundenUpdateModel _data { get; set; }
		private UserModel _user { get; set; }
		public ZahlungskonditionenKundenUpdateHandler(UserModel user, ZahlungskonditionenKundenUpdateModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}
				return Perform(this._user, this._data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<int> Perform(UserModel user, ZahlungskonditionenKundenUpdateModel data)
		{
			lock(Locks.OrderLock)
			{
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

				int response = -1;
				try
				{
					int IdLastItem = 0;
					var dataToCompare = new ZahlungskonditionenKundenEntity();
					//var getItems = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenrahmenAccess.GetNexPossibleId();
					//var item = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenrahmenAccess.GetbyId(data.ID);

					var fetchedkondition = Infrastructure.Data.Access.Tables.FNC.KonditionszuordnungstabelleAccess2.Get(data.KonditionszuordnungstabelleNr);
					var fetchedData = Infrastructure.Data.Access.Tables.FNC.AdressenAccess2.Get(data.adressenNr);

					if(fetchedData is not null)
					{
						dataToCompare = new ZahlungskonditionenKundenEntity()
						{
							Name1 = fetchedData.Name1,
							adressenNr = fetchedData.Nr,
							Kundennummer = fetchedData.Kundennummer ?? 0,
							PLZ_Strabe = fetchedData.PLZ_Strasse,
							Ort = fetchedData.Ort,
							Land = fetchedData.Land,
							Text = fetchedkondition.Text
						};
					}
					var changedItems = data.GetUnMatchingUpdateAttributes(dataToCompare);
					if(changedItems.Count == 0)
					{
						return ResponseModel<int>.SuccessResponse();
					}


					var creatmdl = new ZahlungskonditionenKundenEntity()
					{
						Name1 = data.Name1,
						adressenNr = data.adressenNr,
						Kundennummer = data.Kundennummer,
						KonditionszuordnungstabelleNr = data.KonditionszuordnungstabelleNr,
						PLZ_Strabe = data.PLZ_Strabe,
						Ort = data.Ort,
						Land = data.Land,
						Text = data.Text
					};
					botransaction.beginTransaction();
					var creditDbAfterInsert = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.UpdateZahlungskonditionenKundenWithTransaction(creatmdl, botransaction.connection, botransaction.transaction);
					var update2 = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.UpdateZahlungskonditionenKundenWithTransaction2(creatmdl, botransaction.connection, botransaction.transaction);

					if(botransaction.commit())
					{

						Console.Out.WriteLine("Success");
						//Logging
						foreach(var items in changedItems)
						{
							if(items == "Name1")
							{
								var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
								Infrastructure.Data.Access.Tables.FNC.ZahlungskonditionenKunden_FNC_LogAccess.Insert(logs.LogFNC3(items, dataToCompare.Name1, data.Name1, data.adressenNr));
							}
							if(items == "PLZ_Strabe")
							{
								var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
								Infrastructure.Data.Access.Tables.FNC.ZahlungskonditionenKunden_FNC_LogAccess.Insert(logs.LogFNC3(items, dataToCompare.PLZ_Strabe, data.PLZ_Strabe, data.adressenNr));
							}
							if(items == "Ort")
							{
								var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
								Infrastructure.Data.Access.Tables.FNC.ZahlungskonditionenKunden_FNC_LogAccess.Insert(logs.LogFNC3(items, dataToCompare.Ort, data.Ort, data.adressenNr));
							}
							if(items == "Land")
							{
								var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
								Infrastructure.Data.Access.Tables.FNC.ZahlungskonditionenKunden_FNC_LogAccess.Insert(logs.LogFNC3(items, dataToCompare.Land, data.Land, data.adressenNr));
							}
							if(items == "Kundennummer")
							{
								var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
								Infrastructure.Data.Access.Tables.FNC.ZahlungskonditionenKunden_FNC_LogAccess.Insert(logs.LogFNC3(items, dataToCompare.Kundennummer.ToString(), data.Kundennummer.ToString(), data.adressenNr));
							}
							if(items == "Text")
							{
								var logs = new Psz.Core.FinanceControl.Helpers.LogHelper("FNC", Helpers.LogHelper.LogType.UPDATEOBJECT, _user);
								Infrastructure.Data.Access.Tables.FNC.ZahlungskonditionenKunden_FNC_LogAccess.Insert(logs.LogFNC3(items, dataToCompare.Text, data.Text, data.adressenNr));
							}

						}
						return new ResponseModel<int> { Body = response, Success = true, Errors = null };
					}
					else
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction error");
					}

				} catch(Exception ex)
				{
					botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(ex);
					throw;
				}
			}
		}
		public ResponseModel<int> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
