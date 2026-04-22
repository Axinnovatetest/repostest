using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.FinanceControl.Models.Budget.Order;
using Psz.Core.FinanceControl.Models.Budget.Order.Statistics;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Psz.Api.Areas.FinanceControl.Controllers
{
	[Authorize]
	[Area("FinanceControl")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class BudgetController: ControllerBase
	{
		private const string MODULE = "Finance Control";
		//Lands APIS
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandsNamesModel>>), 200)]
		public IActionResult GetLands()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetLandHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Departements APIS new
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetBudgetUsersModel>>), 200)]
		public IActionResult GetLandWithNotNullBudget(string land_name, int year)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetLandWithNotNullBudgetHandler(this.GetCurrentUser(), land_name, year).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandsModel>>), 200)]
		public IActionResult CreateLand(Core.FinanceControl.Models.Budget.GetLandsModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateLandHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLand(Core.FinanceControl.Models.Budget.GetLandsModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateLandHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteLand(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteLandHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		////Departements APIS new
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetDeptNamesModel>>), 200)]
		//public IActionResult GetDepartements()
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.GetDepartementsHandler(this.GetCurrentUser()).Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}

		////Departements APIS new
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetDepartementAssignementModel>>), 200)]
		//public IActionResult GetDepartementsWithNotNullBudget(string land_name, int year)
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.GetDepartementsWithNotNullBudgetHandler(this.GetCurrentUser(), land_name, year).Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetDepartementAssignementModel>>), 200)]
		//public IActionResult GetDepartementsWithNotNullBudgetUser(string land_name, int year, int id_user)
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.GetDepartementsWithNotNullBudgetUserHandler(this.GetCurrentUser(), land_name, year, id_user).Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}


		//Users APIS
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetBudgetUsersModel>>), 200)]
		public IActionResult GetUsers()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetUsersHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetBudgetUsersModel>>), 200)]
		public IActionResult GetUserWithNotNullBudget(string username, string land_name, string dept_name, int year)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetUserWithNotNullBudgetHandler(this.GetCurrentUser(), username, land_name, dept_name, year).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandsModel>>), 200)]
		public IActionResult CreateUser(Core.FinanceControl.Models.Budget.GetBudgetUsersModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateUserHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateUser(Core.FinanceControl.Models.Budget.GetBudgetUsersModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateUserHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteUser(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteUserHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//userAssignbyuUser
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetBudgetUsersModel>>), 200)]
		public IActionResult GetUserAssignementCurrentList(int id_user)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetUserAssignementCurrentHandler(this.GetCurrentUser(), id_user).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//budget Assignement APIS

		//
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AssignBudgetLand(Core.FinanceControl.Models.Budget.GetLandAssignementModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.AssignLandBudgetHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLandBudgetAssignement(Core.FinanceControl.Models.Budget.GetLandAssignementModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateLandBudgetHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandAssignementModel>>), 200)]
		public IActionResult GetLandsAssignementList()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAssignementLandHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Landuser
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandAssignementModel>>), 200)]
		public IActionResult GetLandAssignementCurrentList(int id_user)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetLandAssignementCurrentHandler(this.GetCurrentUser(), id_user).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}



		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteBudgetLand(Core.FinanceControl.Models.Budget.GetLandAssignementModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteBudgetLandHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AssignBudgetDepartement(Core.FinanceControl.Models.Budget.GetDepartementAssignementModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.AssignBudgetDepartementHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateDepartementBudgetAssignement(Core.FinanceControl.Models.Budget.GetDepartementAssignementModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateDepartementBudgetHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Apps.Settings.Models.Department.GetModel>>), 200)]
		public IActionResult GetDepartementsAssignementList()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetDepartementAssignementHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetDepartementAssignementModel>>), 200)]
		public IActionResult GetDepartementsAssignementCurrentList(int id_user)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetDepartementAssignementCurrentHandler(this.GetCurrentUser(), id_user).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetDepartementAssignementModel>>), 200)]
		public IActionResult GetDepartementsAssignementCurrentListbyLand(int id_user, string land)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetDepartementAssignementCurrentbyLandHandler(this.GetCurrentUser(), id_user, land).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteBudgetDepartement(Core.FinanceControl.Models.Budget.GetDepartementAssignementModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteBudgetDepartementHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.checkDeptBdgModel>>), 200)]
		public IActionResult CheckDeptBDGAPI(string land_name, int year, int ID_assign, string dept_name, float Entredvalue)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CheckDeptBDGHandler(this.GetCurrentUser(), land_name, year, ID_assign, dept_name, Entredvalue).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.checkDeptBdgModel>>), 200)]
		public IActionResult CheckDeptBDGSupplementAPI(string land_name, int year, string dept_name, float Entredvalue)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CheckDeptBDGSupplementHandler(this.GetCurrentUser(), land_name, year, dept_name, Entredvalue).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.checkDeptBdgModel>>), 200)]
		public IActionResult CheckDeptBDGSupplementAPIbyIds(int land, int year, int dept, float Entredvalue)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CheckDeptBDGSupplementbyIdsHandler(this.GetCurrentUser(), land, year, dept, Entredvalue).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.checkDeptBdgModel>>), 200)]
		public IActionResult SommeDeptBDGAPI(string land_name, int year)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.SommeDeptBDGHandler(this.GetCurrentUser(), land_name, year).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.checkUserBdgModel>>), 200)]
		public IActionResult CheckUserBDGAPI(string Land_name, int year, string dept_name, string username, float EntredByear, float EntredBmonth, float EntredBorder)
		{
			try
			{
				//var response = new Core.FinanceControl.Handlers.Budget.CheckUserBDGHandler(this.GetCurrentUser(), Land_name, dept_name, username, year, EntredByear, EntredBmonth, EntredBorder).Handle();

				return Ok(/*response*/-1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.checkUserBdgModel>>), 200)]
		public IActionResult SommeUserBDGAPI(string Land_name, int year, string dept_name)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.SommeUserBDGHandler(this.GetCurrentUser(), Land_name, dept_name, year).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		/**************************************Suupliers APIS******************************************/

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Supplier.SupplierModel>>), 200)]
		public IActionResult Get()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.GetSuppliersHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Supplier.SupplierModel>), 200)]
		public IActionResult GetSingle(int id)
		{
			try
			{
				var requestData = new Core.FinanceControl.Models.Supplier.GetSupplierRequestModel()
				{
					SupplierId = id,
					User = this.GetCurrentUser(),
				};

				var response = new Core.FinanceControl.Handlers.Supplier.GetSupplierHandler(requestData)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Supplier.MinimalSupplierModel>>), 200)]
		public IActionResult GetMinimal()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.GetMinimalSuppliersHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Create(Core.FinanceControl.Models.Supplier.UpdateModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.CreateHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Update(Core.FinanceControl.Models.Supplier.UpdateModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.UpdateHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Supplier.SearchSupplierResponseModel>>), 200)]
		public IActionResult Search(Core.FinanceControl.Models.Supplier.SearchSupplierModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.SearchSupplierHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Delete(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.DeleteHandler(id, this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// <<<<<<<<<<<< ??
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.AddressType.GetModel>>), 200)]
		public IActionResult GetAddressTypes()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.AddressType.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Industry.IndustryModel>>), 200)]
		public IActionResult GetIndustries()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Industry.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Language.LanguageModel>>), 200)]
		public IActionResult GetLanguages()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Language.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.DiscountGroup.DiscountGroupModel>>), 200)]
		public IActionResult GetDiscountGroups()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.DiscountGroup.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Currency.CurrencyModel>>), 200)]
		public IActionResult GetCurrencies()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Currency.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.SuppliersGroup.SuppliersGroupModel>>), 200)]
		public IActionResult GetSuppliersGroups()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.SuppliersGroup.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.SlipCircle.SlipCircleModel>>), 200)]
		public IActionResult GetSlipCircles()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.SlipCircle.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.ConditionAssignment.ConditionAssignementModel>>), 200)]
		public IActionResult GetConditionAssignments()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.ConditionAssignment.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetSupplierNumbers(string searchText)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.GetSupplierNumbersHandler(this.GetCurrentUser(), searchText)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetSupplierNames(string searchText)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Supplier.GetSupplierNamesHandler(this.GetCurrentUser(), searchText)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.SupplementLandModel>>), 200)]
		public IActionResult SupplementLandBDGAPI(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.SupplementLandBDGHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.SupplementLandBdgModel>>), 200)]
		public IActionResult SommeSupplementLandBDGAPI(int ID)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.SommeSupplementLandBDGHandler(this.GetCurrentUser(), ID).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.SupplementLandModel>>), 200)]
		public IActionResult GetsupplementById(int Id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetsupplementByIdHandler(this.GetCurrentUser(), Id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandAssignementModel>>), 200)]
		public IActionResult GetAssignLandByName(string land, int year)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAssignLandByNameHandler(this.GetCurrentUser(), land, year).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandAssignementModel>>), 200)]
		public IActionResult GetAssignLandById(int Id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAssignLandByIdHandler(Id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.SupplementLandModel>>), 200)]
		public IActionResult CreateSupplement(Core.FinanceControl.Models.Budget.SupplementLandModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateSupplementHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteSupplement(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteSupplementHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateSupplement(Core.FinanceControl.Models.Budget.SupplementLandModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateSupplementHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Currency
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetCurrencyModel>>), 200)]
		public IActionResult GetCurrency()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetCurrencyHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//State
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetStateModel>>), 200)]
		public IActionResult GetState()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetStateHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Type
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetTypeProjectModel>>), 200)]
		public IActionResult GetTypeProject()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetTypeProjectHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Customer
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetCustomerProjectModel>>), 200)]
		public IActionResult GetCustomerProject()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetCustomerProjectHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Users
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetUserProjectModel>>), 200)]
		//public IActionResult GetUserProject()
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.GetUserProjectHandler(this.GetCurrentUser()).Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}
		//API Project
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetProjectsModel>>), 200)]
		public IActionResult GetProjects()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetProjectHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}



		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetAllDataProjects()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataProjectHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetAllDataProjectsbyId(int value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetProjectBudgetByIdProjectHandler(this.GetCurrentUser(), value).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetAllDataProjectsbyLand(int value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataProjectbyLandHandler(this.GetCurrentUser(), value).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetAllDataProjectsbyType(int value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataProjectbyTypeHandler(this.GetCurrentUser(), value).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetAllDataProjectsbyState(int value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataProjectbyStateHandler(this.GetCurrentUser(), value).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetAllDataProjectsbyCustomer(int value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataProjectbyCustomerHandler(this.GetCurrentUser(), value).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetAllDataProjectsbyDept(int value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataProjectbyDeptHandler(this.GetCurrentUser(), value).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CreateProject(Core.FinanceControl.Models.Budget.Project.ProjectModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProject(Core.FinanceControl.Models.Budget.Project.ProjectModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.UpdateHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProjectApprovalStatus(Core.FinanceControl.Models.Budget.Project.UpdateStatusModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.UpdateStatusHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProjectStatus(Core.FinanceControl.Models.Budget.Project.UpdateStatusModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.UpdateProjectStatusHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProjectBudgetInternal(Core.FinanceControl.Models.Budget.Project.UpdateBudgetModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.UpdateInternalBudgetHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProjectBudgetCustomer(Core.FinanceControl.Models.Budget.Project.UpdateBudgetModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.UpdateCustomerBudgetHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public async Task<IActionResult> UpdateProjectFile([FromForm] Models.Budget.ProjectFileModel model)
		{
			try
			{
				var response = await new Core.FinanceControl.Handlers.Budget.Project.UpdateProjectFileHandler(this.GetCurrentUser(), model.ToBusinessModel()).Handleasync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProject(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.DeleteHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ArchiveProject(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.ArchiveHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CloseProject(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.CloseHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult OpenProject(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.OpenHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetProjects_Diverse_LogModel>>), 200)]
		public IActionResult CreateProjectLog(Core.FinanceControl.Models.Budget.GetProjects_Diverse_LogModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateProjectLogHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProjectLog(Core.FinanceControl.Models.Budget.GetProjects_LogModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateProjectLogHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProjectLog(Core.FinanceControl.Models.Budget.GetProjects_LogModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteProjectLogHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.checkProjectDeptBdgModel>>), 200)]
		public IActionResult CheckProjectDeptBDGAPI(string land_name, int year, string dept_name, float Entredvalue)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CheckProjectDeptBDGHandler(this.GetCurrentUser(), land_name, year, dept_name, Entredvalue).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetProjectsByName(string value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetProjectBudgetByNameHandler(this.GetCurrentUser(), value)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Supplier List
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetSupplierArticleModel>>), 200)]
		public IActionResult GetSupplierArticle(string search)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetSupplierArticleHandler(this.GetCurrentUser(), search).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//API Article


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllBudgetArticleModel>>), 200)]
		public IActionResult GetAllDataArticles()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataArticleHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}



		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticle(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteArticleHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}



		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetArticlesModel>>), 200)]
		public IActionResult CreateArticle(Core.FinanceControl.Models.Budget.GetArticlesModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateArticleHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticle(Core.FinanceControl.Models.Budget.GetArticlesModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateArticleHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandsModel>>), 200)]
		public IActionResult GetLandbyName(string landName)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetLandbyNameHandler(this.GetCurrentUser(), landName).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetLandsModel>>), 200)]
		public IActionResult GetLandsbyAllowedDepts(int id_user)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetLandsbyAllowedDeptsHandler(this.GetCurrentUser(), id_user).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataLandAssignementModel>>), 200)]
		public IActionResult GetAllDataLandAssignementCurrentList(int id_user)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataLandAssignementCurrentHandler(this.GetCurrentUser(), id_user).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataLandAssignementModel>>), 200)]
		public IActionResult GetAllDataDeptAssignementCurrentList(int id_user)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataDeptAssignementCurrentHandler(this.GetCurrentUser(), id_user).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//Orders API
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetProjectsByType(string value)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetProjectBudgetByTypeHandler(this.GetCurrentUser(), value)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataProjectModel>>), 200)]
		public IActionResult GetProjectsByIdAndType(string TypeProject, int IdProject)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetProjectBudgetByIdAndTypeHandler(this.GetCurrentUser(), TypeProject, IdProject)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.InsertedDataOrderModel>>), 200)]
		public IActionResult CreateOrder(Core.FinanceControl.Models.Budget.InsertedDataOrderModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateOrderHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.InsertedDataOrderModel>>), 200)]
		//public IActionResult UpdateOrder(Core.FinanceControl.Models.Budget.InsertedDataOrderModel data)
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.UpdateOrderHandler(data, this.GetCurrentUser())
		//           .Handle();

		//        return Ok(response);

		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Land_Responsable_JointModel>>), 200)]
		public IActionResult GetResponsableLand(int land)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetResponsableLandHandler(this.GetCurrentUser(), land).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetDeptJointLandResponsableModel>>), 200)]
		public IActionResult GetResponsableDepartement(int dept)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetResponsableDepartementHandler(this.GetCurrentUser(), dept).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderListResponseModel>), 200)]
		public IActionResult GetAllOrders(Core.FinanceControl.Models.Budget.Order.OrderListRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetAllHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetAllLeasingOrders(int year, int? companyId, int? departmentId)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetAllLeasingHandler(this.GetCurrentUser(),
					new Core.FinanceControl.Models.Budget.Order.OrderLeasingRequestModel { Year = year, CompanyId = companyId, DepartmentId = departmentId }).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetLeasingOrders(int year, int? companyId, int? departmentId, int? employeeId)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetLeasingHandler(this.GetCurrentUser(),
					new Core.FinanceControl.Models.Budget.Order.OrderLeasingRequestModel { Year = year, CompanyId = companyId, DepartmentId = departmentId, EmployeeId = employeeId }).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetAllArchivedByUser(OrdersArchivedByUserRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetAllByArchivedUserHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetByUser(bool? showCompletelyBooked)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetByUserHandler(this.GetCurrentUser(), showCompletelyBooked).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetAllByUser(Core.FinanceControl.Models.Budget.Order.OrderRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetAllByUserHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetOrdersToValidater(bool? onlyDirectRequests = false)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetOrdersToValidateHandler(this.GetCurrentUser(), onlyDirectRequests ?? false).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetValidatedHístory()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetValidatedHistoryHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetOrdersByProject(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetByProjectHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderModel>), 200)]
		public IActionResult GetOrder(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetOrderValidationStatus(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetValidationStatusHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteOrder(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.DeleteHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ArchiveOrder(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.ArchiveHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UnArchiveOrder(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.UnArchiveHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public async Task<IActionResult> AddOrder(Psz.Core.FinanceControl.Models.Budget.Order.OrderModel model)
		{
			try
			{
				var response = await new Core.FinanceControl.Handlers.Budget.Order.AddHandler(this.GetCurrentUser(), model).Handleasync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public async Task<IActionResult> UpdateOrderFile([FromForm] Models.Budget.OrderFileModel model)
		{
			try
			{
				var response = await new Core.FinanceControl.Handlers.Budget.Order.UpdateFileHandler(this.GetCurrentUser(), model.OrderToBussModel()).Handleasync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArticlesOrder(Psz.Core.FinanceControl.Models.Budget.Order.OrderModel2 orderModel)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.AddArticlesOrderHandler(this.GetCurrentUser(), orderModel).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateOrder(Psz.Core.FinanceControl.Models.Budget.Order.OrderModel model)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.UpdateHandler(this.GetCurrentUser(), model).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticlesOrder(Psz.Core.FinanceControl.Models.Budget.Order.OrderModel2 orderModel)
		{

			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.UpdateArticlesOrderHandler(this.GetCurrentUser(), orderModel).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticleDeliveryData(Psz.Core.FinanceControl.Models.Budget.Order.Article.UpdateDeliveryModel orderModel)
		{

			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.Article.UpdateDeliveryHandler(this.GetCurrentUser(), orderModel).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		/// <summary>
		/// --------------------- Projects ------------------------------------------------------------------------
		/// </summary>

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Project.ProjectModel>>), 200)]
		public IActionResult GetProjectsInternalForOrders()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetInernalForOrdersHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Project.ProjectModel>>), 200)]
		public IActionResult GetAllProjectsByType(int type)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetByTypeHandler(this.GetCurrentUser(), type).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Project.ProjectModel>>), 200)]
		public IActionResult GetAllProjects()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetAllHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Project.ProjectModel>>), 200)]
		public IActionResult GetByStateProjects(int idState)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetByStateHandler(this.GetCurrentUser(), idState).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetProject(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Project.WorkflowHistoryModel>>), 200)]
		public IActionResult GetProjectWorkflowHistory(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetWorkflowHistoryHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetProjectByDepartment()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetByDepartmentHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetProjectByDepartmentDirector()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetByDepartmentDirectorHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetFinanceProject()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetFinanceProjectsHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetProjectBySiteDirector()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetBySiteDirectorHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetProjectBySite()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetBySiteHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetAllProjectsbyCurrentUser(int Id_current)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetAllByCurrentUserHandler(this.GetCurrentUser(), Id_current).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetAllArchivedProjectsByUser()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetAllArchivedByUserHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetAllProjectsbyCurrentUserAndState(int Id_current, int state)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetAllByCurrentUserAndStateHandler(this.GetCurrentUser(), Id_current, state).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Project.ProjectModel>), 200)]
		public IActionResult GetForUser(Core.FinanceControl.Models.Budget.Project.GetRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.GetForUserHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.AllDataOrderModel>>), 200)]
		public IActionResult GetOrderByIdOrder(int Id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetOrderByIdOrderHandler(Id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderExtensionModel>), 200)]
		public IActionResult GetOrderExtensionByIdOrder(int Id)
		{
			try
			{
				return Ok(new Psz.Core.FinanceControl.Handlers.Budget.Order.GetOrderExtensionByIdHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.Article.ArticleExtensionModel>>), 200)]
		public IActionResult GetExtensionByOrderId(int Id)
		{
			try
			{
				return Ok(new Psz.Core.FinanceControl.Handlers.Budget.Order.Article.GetExtensionByOrderIdHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.OrderModel>>), 200)]
		public IActionResult GetOrderByProject(int Id_project)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetOrdersbyProjectHandler(this.GetCurrentUser(), Id_project).Handle();


				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelOrderModel>>), 200)]
		public IActionResult ArtikelOrderAPI(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.ArtikelOrderHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelOrderConcatNameModel>>), 200)]
		public IActionResult GetListArtikelOrderAPI(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.ArtikelOrder_ConcatNameHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelOrderConcatNameModel>>), 200)]
		public IActionResult GetNameArticle(int id, int id_Article)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.ArtikelOrder_ArticleHandler(this.GetCurrentUser(), id, id_Article).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Account.AccountModel>>), 200)]
		public IActionResult GetAllAccount()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Account.GetAllHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult DeleteOrder(int data)
		//{
		//    try
		//    {
		//        var response = new Core.FinanceControl.Handlers.Budget.DeleteOrderHandler(data, this.GetCurrentUser())
		//           .Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}

		//ArticleOrder List
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetArticleOrderModel>>), 200)]
		public IActionResult GetArticleOrder()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetArticleOrderHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelOrderModel>>), 200)]
		public IActionResult CreateArtikelOrder(Core.FinanceControl.Models.Budget.ArtikelOrderModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateArtikelOrderHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.InsertedArticleOrderModel>>), 200)]
		public IActionResult CreateArtikelVersionOrder(Core.FinanceControl.Models.Budget.InsertedArticleOrderModel data, int Max_Ver_Ord)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateArtikelVersionOrderHandler(data, Max_Ver_Ord, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		/* 
         [HttpPost]
         [SwaggerOperation(Tags = new[] { "FinanceControl" })]
         [ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.InsertedArticleOrderModel>>), 200)]
         public IActionResult CreateListArtikelVersionOrder(List<Core.FinanceControl.Models.Budget.InsertedArticleOrderModel> data, int Max_Ver_Ord)
         {
             try
             {
                 var response = new Core.FinanceControl.Handlers.Budget.CreateListArtikelVersionOrderHandler(data, Max_Ver_Ord, this.GetCurrentUser())
                    .Handle();

                 return Ok(response);
             }
             catch (Exception e)
             {
                 Infrastructure.Services.Logging.Logger.Log(e);
                 return this.HandleException(e);
             }
         }*/
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.InsertedListArticleOrderModel>>), 200)]
		public IActionResult CreateListArtikelVersionOrder(Core.FinanceControl.Models.Budget.InsertedListArticleOrderModel data, int Max_Ver_Ord)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateListArtikelsVersionOrderHandler(data, Max_Ver_Ord, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateListArtikelVersionOrder(Core.FinanceControl.Models.Budget.InsertedListArticleOrderModel data, int Max_Ver_Ord)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateListArtikelVersionOrderHandler(data, Max_Ver_Ord, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteListArtikelVersionOrder(int id_ao, int Max_Ver_Ord, Core.FinanceControl.Models.Budget.InsertedListArticleOrderModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteListArtikelVersionOrderHandler(id_ao, Max_Ver_Ord, data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.GetOrdersModel>>), 200)]
		public IActionResult GetOrdersAkl()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetOrdersAkl(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelOrderParamsModel>>), 200)]
		public IActionResult GetlastVersionOrdersAkl(int Id_Order)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetLastVersionOrdersAklHandler(this.GetCurrentUser(), Id_Order).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelOrderModel>>), 200)]
		public IActionResult GetArtikelOrderById(int Id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetArtikelOrderByIdHandler(this.GetCurrentUser(), Id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Debug.WriteLine("error " + e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult GetFile(int id)
		{
			try
			{
				var file = Psz.Core.Program.FilesManager.GetFile(id);

				switch(file?.FileExtension)
				{
					case ".png":
						return new FileContentResult(file.FileBytes, "application/png")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.png"
						};
					case ".jpg":
					case ".jpeg":
						return new FileContentResult(file.FileBytes, "application/jpeg")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.jpg"
						};
					default:
						return new FileContentResult(file.FileBytes, "application/blob")
						{
							FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMDDHHmmss")}"
						};
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		//[HttpPost]
		//[Route("api/dashboard/UploadImage")]
		//public System.Net.Http.HttpResponseMessage UploadImage()
		//{
		//    string imageName = null;
		//    var httpRequest = HttpContext.Current.Request;
		//    //Upload Image
		//    var postedFile = httpRequest.Files["Image"];
		//    //Create custom filename
		//    if (postedFile != null)
		//    {
		//        imageName = new String(System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
		//        imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
		//        var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
		//        postedFile.SaveAs(filePath);
		//    }
		//}


		//API Artikel-rebuild


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelBudgetModel>>), 200)]
		public IActionResult GetAllDataArtikels()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.GetAllDataArtikelRebuildHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Configuration.Article.GetResponseModel>), 200)]
		public IActionResult GetArtikels(Core.FinanceControl.Models.Budget.Configuration.Article.GetParamsModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Article.GetHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelBudgetModel.Supplier>>), 200)]
		public IActionResult GetArticlePrices(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Article.GetPricesHandler(this.GetCurrentUser(), id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelBudgetModel>>), 200)]
		public IActionResult SearchArticlesByNameHandler(Core.FinanceControl.Models.Budget.Order.Article.SearchByNameModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Article.SearchByNameHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.ArtikelBudgetModel>>), 200)]
		public IActionResult CreateArtikel(Core.FinanceControl.Models.Budget.ArtikelBudgetModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.CreateArtikelHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArtikel(int data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.DeleteArtikelHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArtikel(Core.FinanceControl.Models.Budget.ArtikelBudgetModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateArtikelHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArtikelSuppliers(Core.FinanceControl.Models.Budget.ArtikelBudgetModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.UpdateArtikelSuppliersHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ValidateOrder(Core.FinanceControl.Models.Budget.Order.ValidateModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.ValidateHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ValidateFinanceOrder(Core.FinanceControl.Models.Budget.Order.ValidateModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.ValidateFinanceHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UnvalidateOrder(Core.FinanceControl.Models.Budget.Order.UnvalidateModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.UnvalidateHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult PlaceOrder([FromForm] Models.Budget.OrderPlaceModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.PlaceHandler(data.ToBusinessModel(), this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.PlaceHistoryModel>>), 200)]
		public IActionResult GetOrderPlacementList(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.PlacementByOrderHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.WorkflowModel>), 200)]
		public IActionResult GetOrderWorkflowHistory(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetWorkflowHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Order.WorkflowFullModel>>), 200)]
		public IActionResult GetOrderFullWorkflowHistory(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetFullWorkflowHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RejectOrder(Core.FinanceControl.Models.Budget.Order.RejectModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.RejectHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ReportOrder(int id, string lang)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.ReportHandler(id, lang, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult InvoiceReport(int id, string lang)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.InvoiceHandler(id, lang, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult InvoiceDownloadXLS([FromBody] List<int> ids)
		{
			try
			{
				var results = Core.FinanceControl.Handlers.Budget.Order.InvoiceHandler.GetInvoiceXLS(ids);
				if(results != null && results.Length > 0)
				{
					return new FileContentResult(results, "application/zip")
					{
						FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip"
					};
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult PoDownloadXLS([FromBody] List<int> ids)
		{
			try
			{
				var results = Core.FinanceControl.Handlers.Budget.Order.InvoiceHandler.GetInvoiceXLS(ids);
				if(results != null && results.Length > 0)
				{
					return new FileContentResult(results, "application/zip")
					{
						FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip"
					};
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult InvoiceBookingReport(int id, string lang)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.InvoiceBookingHandler(id, lang, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		public IActionResult InvoiceDownload([FromBody] List<int> ids)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.InvoiceDownloadHandler(ids, "", this.GetCurrentUser())
				   .Handle();

				if(response == null || response.Body == null)
				{
					response.Errors = new List<Core.Common.Models.ResponseModel<byte[]>.ResponseError> { };
					response.Errors.Add(new Core.Common.Models.ResponseModel<byte[]>.ResponseError { Key = "", Value = "No data found" });
					response.Success = false;
					Ok(response);
				}

				return new FileContentResult(response.Body, "application/zip")
				{
					FileDownloadName = $"file-{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip"
				};
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetReportLanguageList()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.GetReportLanguageListHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetOrderValidationStatusList()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.Validation.GetStatusListHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.Leasing.PaymentHistoryModel>), 200)]
		public IActionResult GetLeasingOrderPaymentHistory(int Id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.Leasing.GetPaymentHistoryHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#region >>> Stats <<<<
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderModel>), 200)]
		public IActionResult GetValidatedNonPlaced()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetValidatedNonPlacedHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderModel>), 200)]
		public IActionResult GetPlacedNonSupplierConfirmed()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetPlacedNonSupplierConfirmedHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderModel>), 200)]
		public IActionResult GetSupplierDeliveryOverdue()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetSupplierDeliveryOverdueHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderModel>), 200)]
		public IActionResult GetUpcomingDeliveries()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetUpcomingDeliveriesHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderModel>), 200)]
		public IActionResult GetBooked()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetBookedHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.OrderModel>), 200)]
		public IActionResult GetOpenLeasing()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOpenLeasingHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>), 200)]
		public IActionResult GetOverview()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOverviewHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>), 200)]
		public IActionResult GetOverviewAmount()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOverviewAmountHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>), 200)]
		public IActionResult GetOverviewAmountDistinct(int? year)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOverviewAmountDistinctHandler(this.GetCurrentUser(), year ?? DateTime.Today.Year)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>), 200)]
		public IActionResult GetOverviewLeasingAmount()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOverviewLeasingAmountHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<OrderStatisticsResponseModel>), 200)]
		public IActionResult GetStatisticsOrders(OrderStatisticsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOrdersHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Accounting.CompanyExtOrdersNotFullValidatedModel>), 200)]
		public IActionResult GetExternalOrdersByCompany(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetExternalOrdersByCompanyNotFullValidatedHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetInternalOrdersOverviewByCompany(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetInternalOrdersByCompanyAndLevelHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetInternalOrdersOverviewByDepartment(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetInternalOrdersByDepartmentHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Stats



		/// <summary>
		/// --------------------- User Profiles ------------------------------------------------------------------------
		/// </summary>
		#region User Profiles
		private const string ACCESS_PROFILE = "ADMIN AccessProfile";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAdd(Core.FinanceControl.Models.Administration.AccessProfile.AccessProfileModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEdit(Core.FinanceControl.Models.Administration.AccessProfile.AccessProfileModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.EditHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditName(Core.FinanceControl.Models.Administration.AccessProfile.AccessProfileModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.EditNameHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileDelete(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.DeleteHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Administration.AccessProfile.AccessProfileModel>>), 200)]
		public IActionResult AccessProfileGetAll()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Administration.AccessProfile.AccessProfileModel>>), 200)]
		public IActionResult AccessProfileGetUsers([FromBody] List<int> ids)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.GetUsersHandler(this.GetCurrentUser(), ids)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		// - 
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddUsers(Core.FinanceControl.Models.Administration.AccessProfile.AddUsersModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.AddUsersHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileRemoveUsers(Core.FinanceControl.Models.Administration.AccessProfile.AddUsersModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.RemoveUsersHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddToUser(Core.FinanceControl.Models.Administration.AccessProfile.AddToUserModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.AddToUserHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditForUser(Core.FinanceControl.Models.Administration.AccessProfile.AddToUserModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.EditForUserHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileToggleDefault(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.AccessProfile.ToggleDefaultHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		#endregion User Profiles

		#region Users
		private const string ADMIN_USERS = "ADMIN Users";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.FinanceControl.Models.Administration.Users.GetModel>>), 200)]
		public IActionResult AdminUsersGetAll()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.Users.GetAllHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.FinanceControl.Models.Administration.Users.GetModel>), 200)]
		public IActionResult AdminUsersGet(int id)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Administration.Users.GetHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Users


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult VisibiltyUserAdd(Core.FinanceControl.Models.Budget.Validator.ValidatorModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.VisibilityUserAddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult VisibiltyUserDelete(Core.FinanceControl.Models.Budget.Validator.ValidatorModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Project.VisibilityUserDeleteHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<int>>), 200)]
		//public async Task<IActionResult> ResendPlaceEmailsSentWithoutAttachment()
		//{
		//	try
		//	{
		//		var response = await Task.Run(() => new Core.FinanceControl.Handlers.Budget.Order.ResendPlacementmails_TempHandler(this.GetCurrentUser())
		//		   .HandleAsync());
		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		return this.HandleException(e);
		//	}
		//}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RejectForPastBudget(Core.FinanceControl.Models.Budget.Order.RejectModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Order.RejectForPastBudgetHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
	}
}