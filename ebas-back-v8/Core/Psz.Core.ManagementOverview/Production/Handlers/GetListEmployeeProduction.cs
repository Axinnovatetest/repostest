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
	public  partial class ProductionService: IProductionService
	{
		public ResponseModel<PackEmployeeProductionModel> GetListeEmployeeProduction(Identity.Models.UserModel user, int lager,int typeLoading)
		{
			try
			{
				var validationResponse = this.ValidationListeEmployeeProduction(user, lager) ;
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				if(typeLoading == 2)
				{
					string nameAgent = "DaschboardByKW_Update";
					if(lager == 7)
					{
						nameAgent += "_TN";
					}
					else if(lager == 102)
					{
						nameAgent += "_GZTN";
					}
					else if(lager == 42)
					{
						nameAgent += "_WS";
					}
					else if(lager == 26)
					{
						nameAgent += "_AL";
					}
					else if(lager == 6)
					{
						nameAgent += "_CZ";
					}
					else if(lager == 15)
					{
						nameAgent += "_DE";
					}
					if(nameAgent != "DaschboardByKW_Update")
					{
						if(ProductionAccess.verifierJob(nameAgent) != true)
						{
							int? resultExecute = ProductionAccess.ExecuteAgent(nameAgent);
							Thread.Sleep(5000);
						}

						int compteur = 0;
						bool? x = ProductionAccess.verifierJob(nameAgent);
						while((x == true) && compteur < 100)
						{
							compteur++;
							Thread.Sleep(1000);
							x = ProductionAccess.verifierJob(nameAgent);
						}
						compteur = compteur;
						Thread.Sleep(5000);
					}
					
				}
				
				var PackListeEmployeeProduction = new PackEmployeeProductionModel();
				//var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data);

				var ListeEmployeeProductionEntity = ProductionAccess.GetListEmployeeProductionByLager(lager);
                 var ListeEmployeeProductionModel= new List<Models.EmployeeProductionModel>();
				if(ListeEmployeeProductionEntity != null && ListeEmployeeProductionEntity.Count > 0)
				{
					ListeEmployeeProductionModel = ListeEmployeeProductionEntity.Select(k => new Models.EmployeeProductionModel(k)).ToList();
					//PackListeEmployeeProduction.listeEmployeeProduction = ListeEmployeeProductionModel;
					PackListeEmployeeProduction.lastUpdate = ListeEmployeeProductionModel[0].datum;
				}
				PackListeEmployeeProduction.listeEmployeeProduction = ListeEmployeeProductionModel;



				return ResponseModel<PackEmployeeProductionModel>.SuccessResponse(PackListeEmployeeProduction);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<PackEmployeeProductionModel> ValidationListeEmployeeProduction(Identity.Models.UserModel user, int lager)
		{
			if(user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<PackEmployeeProductionModel>.AccessDeniedResponse();
			}

			return ResponseModel<PackEmployeeProductionModel>.SuccessResponse();
		}
	}
}
