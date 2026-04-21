using Infrastructure.Data.Access.Joins.MGO;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Production.Interfaces;
using Psz.Core.ManagementOverview.Production.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Handlers
{
	public partial class ProductionService: IProductionService
	{
		public ResponseModel<PackGeplanteStundenModel> GetListePlanungStunden(Identity.Models.UserModel user, int lager, int typeLoading)
		{
			try
			{
				var validationResponse = this.ValidationListePlanungStunden(user, lager);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				if(typeLoading == 2)
				{
					string nameAgent = "PlanungStundenByCustomer";
					int? resultExecute = ProductionAccess.ExecuteAgent(nameAgent);
					Thread.Sleep(5000);
				}
				var PackListPlanungStunden = new PackGeplanteStundenModel();
				PackListPlanungStunden.lastUpdate = null;
				PackListPlanungStunden.listeGeplantStunden = new List<Models.GeplantStundenModel>();

				List<Infrastructure.Data.Entities.Tables.MGO.GeplantStundenEntity> listPlanungStundenEntity = ProductionAccess.GetListPlanungStundenByLager(lager);

				var listPlanungStundenModel = new List<Models.GeplantStundenModel>();
				if(listPlanungStundenEntity != null && listPlanungStundenEntity.Count > 0)
					PackListPlanungStunden.lastUpdate = listPlanungStundenEntity[0].datum;
				listPlanungStundenModel = listPlanungStundenEntity.Select(k => new Models.GeplantStundenModel(k)).ToList();

				var result = listPlanungStundenModel
					.GroupBy(x => new { x.jahr, x.KW })
					.Select(g => new
					{
						Kunde = "Total",                // Propriété nommée `Total`
						Jahr = g.Key.jahr,              // Accès à `jahr` via `g.Key`
						KW = g.Key.KW,                  // Accès à `KW` via `g.Key`
						SummeStunden = g.Sum(x => (x.stunden == null ? 0 : x.stunden)),  // Somme des `stunden`, si nullable
						SummeGeschnittenStunden = g.Sum(x => (x.geschnittenStunden == null ? 0 : x.geschnittenStunden))  // Somme des `stunden`, si nullable
					})
					.ToList();


				foreach(var item in result)
				{
					var t = new GeplantStundenModel(item.Kunde, item.Jahr, item.KW, item.SummeStunden);
					listPlanungStundenModel.Add(t);
				}

				//-----------------Add Summ Geschnitten------24-02-2025-------------------
				var result0 = listPlanungStundenModel
					.GroupBy(x => new { x.jahr, x.KW })
					.Select(g => new
					{
						Kunde = "Geschnitten",                // Propriété nommée `Total`
						Jahr = g.Key.jahr,              // Accès à `jahr` via `g.Key`
						KW = g.Key.KW,                  // Accès à `KW` via `g.Key`
						SummeGeschnittenStunden = g.Sum(x => (x.geschnittenStunden == null ? 0 : x.geschnittenStunden))  // Somme des `stunden`, si nullable
					})
					.ToList();
				foreach(var item in result0)
				{
					var t = new GeplantStundenModel(item.Kunde, item.Jahr, item.KW, item.SummeGeschnittenStunden);
					listPlanungStundenModel.Add(t);
				}
				//-----------------End Summ Geschnitten------24-02-2025-------------------
				//-----------------Add Summ Gestatet------07-03-2025-------------------
				var result01 = listPlanungStundenModel
					.GroupBy(x => new { x.jahr, x.KW })
					.Select(g => new
					{
						Kunde = "Gestartet",                // Propriété nommée `Total`
						Jahr = g.Key.jahr,              // Accès à `jahr` via `g.Key`
						KW = g.Key.KW,                  // Accès à `KW` via `g.Key`
						SummeGestartetStunden = g.Sum(x => (x.gestartetStunden == null ? 0 : x.gestartetStunden))  // Somme des `stunden`, si nullable
					})
					.ToList();
				foreach(var item in result01)
				{
					var t = new GeplantStundenModel(item.Kunde, item.Jahr, item.KW, item.SummeGestartetStunden);
					listPlanungStundenModel.Add(t);
				}
				//-----------------End Summ Gestatet------07-03-2025-------------------

				//---------Liste Kunde-----------------------------
				List<string> listeKunden = listPlanungStundenModel.Select(o => o.kunde).Distinct().ToList();
				var listeJahrKW = listPlanungStundenModel.Select(o => new { o.jahr, o.KW }).Distinct().OrderBy(x => x.jahr).ThenBy(p => p.KW).ToList();
				if(listPlanungStundenModel != null)
				{
					PackListPlanungStunden.listeGeplantStunden = listPlanungStundenModel;
				}

				PackListPlanungStunden.listeKunden = listeKunden;
				PackListPlanungStunden.listeJahrKW = listeJahrKW.Select(o => o.jahr + "/" + o.KW).ToList();
				return ResponseModel<PackGeplanteStundenModel>.SuccessResponse(PackListPlanungStunden);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<PackGeplanteStundenModel> ValidationListePlanungStunden(Identity.Models.UserModel user, int lager)
		{
			if(user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<PackGeplanteStundenModel>.AccessDeniedResponse();
			}

			return ResponseModel<PackGeplanteStundenModel>.SuccessResponse();
		}
	}
}
