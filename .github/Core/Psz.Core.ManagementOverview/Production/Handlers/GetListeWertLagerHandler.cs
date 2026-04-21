using Infrastructure.Data.Access.Joins.MGO;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Production.Interfaces;
using Psz.Core.ManagementOverview.Production.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Handlers
{
	public partial class ProductionService: IProductionService
	{
		public ResponseModel<PackWertProductionModel> GetListeWertLager(Identity.Models.UserModel user, int lager)
		{
			try
			{
				var validationResponse = this.ValidationListeWertLager(user, lager);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				//	verifierJob(string jobName)
				//	Infrastructure.Data.Access.Tables.MGO.ProductionAccess.ExecuteAgent(lager);
				var ModelLGT = Module.AppSettingsLGT.LGTList.Where(x => x.Lager_Id == lager).ToList();
				int prodLager = ModelLGT.Select(x => x.Lager_P_Id).FirstOrDefault();
				PackWertProductionModel packWertProductionModel = new PackWertProductionModel();
				var ListeEmployeeProductionEntity = ProductionAccess.GetBestandWertLager( lager, prodLager);
				if(ListeEmployeeProductionEntity != null )
				{
					packWertProductionModel.wertROH =(decimal) ListeEmployeeProductionEntity.Where(X => X.warengroup.ToLower()=="roh")
								   .GroupBy(p => p.warengroup)
								   .Select(g => g.Sum(p => p.bestandWert))
								   .FirstOrDefault();
					packWertProductionModel.wertFG = (decimal)ListeEmployeeProductionEntity.Where(X => X.warengroup.ToLower() == "ef")
								   .GroupBy(p => p.warengroup)
								   .Select(g => g.Sum(p => p.bestandWert))
								   .FirstOrDefault();
				}
				DateTime premierJourSemaine = GetPremierJourDeLaSemaine(DateTime.Now);
				DateTime dernierJourSemaine = GetDernierJourDeLaSemaine(DateTime.Now);
				decimal wertROHT = ProductionAccess.GetKostenROH(premierJourSemaine, dernierJourSemaine, lager) ?? 0;
				if(lager==42)
				{
					 wertROHT+= ProductionAccess.GetKostenROH(premierJourSemaine, dernierJourSemaine, 7) ?? 0;
				}
				
				packWertProductionModel.wertScrap = ProductionAccess.GetKostenAusschussROH(premierJourSemaine, dernierJourSemaine, lager, prodLager) ??0;
				packWertProductionModel.wertOrder = ProductionAccess.GetAusgabe(premierJourSemaine, dernierJourSemaine, lager)??0;
				if(packWertProductionModel.wertScrap != null && packWertProductionModel.wertScrap > 0 && wertROHT != null && wertROHT > 0)
				{
					packWertProductionModel.prozentsatzScrap  = packWertProductionModel.wertScrap / wertROHT * 100;
				}
				else
				{
					packWertProductionModel.prozentsatzScrap  = 0;
				}
				




				//var ListeEmployeeProductionModel = new List<Models.EmployeeProductionModel>();
				//if(ListeEmployeeProductionEntity != null && ListeEmployeeProductionEntity.Count > 0)
				//	ListeEmployeeProductionModel = ListeEmployeeProductionEntity.Select(k => new Models.EmployeeProductionModel(k)).ToList();
				//var PackListeEmployeeProduction = new PackEmployeeProductionModel();
				//PackListeEmployeeProduction.listeEmployeeProduction = ListeEmployeeProductionModel;
				//return ResponseModel<PackEmployeeProductionModel>.SuccessResponse(PackListeEmployeeProduction);
				return ResponseModel<PackWertProductionModel>.SuccessResponse(packWertProductionModel);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<PackWertProductionModel> ValidationListeWertLager(Identity.Models.UserModel user, int lager)
		{
			if(user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<PackWertProductionModel>.AccessDeniedResponse();
			}

			return ResponseModel<PackWertProductionModel>.SuccessResponse();
		}
		public static DateTime GetPremierJourDeLaSemaine(DateTime date)
		{
			var culture = CultureInfo.CurrentCulture;
			int diff = date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
			if(diff < 0)
				diff += 7;

			return date.AddDays(-diff).Date;
		}
		public static DateTime GetDernierJourDeLaSemaine(DateTime date)
		{
			var culture = CultureInfo.CurrentCulture;
			DayOfWeek dernierJour = culture.DateTimeFormat.FirstDayOfWeek - 1 < 0
									? DayOfWeek.Saturday
									: culture.DateTimeFormat.FirstDayOfWeek - 1;

			int diff = (7 + (dernierJour - date.DayOfWeek)) % 7;
			return date.AddDays(diff).Date;
		}
	}
}
