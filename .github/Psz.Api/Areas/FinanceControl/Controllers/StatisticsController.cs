using System;
using System.Collections.Generic;
using Infrastructure.Data.Entities.Tables.FNC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.FinanceControl.Models.Budget.Order.Statistics;
using Psz.Core.FinanceControl.Models.Budget.Project;
using Psz.Core.FinanceControl.Models.Statistics;
using Swashbuckle.AspNetCore.Annotations;

namespace Psz.Api.Areas.FinanceControl.Controllers
{
	[Authorize]
	[Area("FinanceControl")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StatisticsController: ControllerBase
	{
		private const string MODULE = "Finance Control";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<AmountVsExpensesModel>), 200)]
		public IActionResult GetAllocationVSExpencesByCompany(StatsRequestModel data)
		{
			try
			{
				var response = new Psz.Core.FinanceControl.Handlers.Statistics.GetAllocationVSExpensesByCompanyHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<KeyValuePair<decimal, decimal>>), 200)]
		public IActionResult GetAllocationVSExpencesByDepartment(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetAllocationVSExpensesByDepartmentHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<KeyValuePair<decimal, decimal>>), 200)]
		public IActionResult GetAllocationVSExpencesByUser(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetAllocationVSExpensesByUserHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ProjectsStatsModel>), 200)]
		public IActionResult GetProjectsStatistics(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetProjectsStatisticsHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>>), 200)]
		public IActionResult GetTop10ProjectsByBudget(StatsRequestModelMonth data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetGetTop10ProjectsByBudgetHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<OrdersCountsModel>), 200)]
		public IActionResult GetOrdersStatistics(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetOrdersStatsHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<OrdersMonthlyViewStats>), 200)]
		public IActionResult GetOrdersMonthlyView(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetOrdersMonthlyOverviewHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<PorjectsMonthlyViewStats>), 200)]
		public IActionResult GetProjectsMonthlyView(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetProjectsMonthlyOverviewHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>>), 200)]
		public IActionResult GetTop10ProjectsByAmount(StatsRequestModelMonth data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetGetTop10ProjectsByAmountHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<OrdersHighestOrLowestAmountModel>), 200)]
		public IActionResult GetOrdersHighestAndLowestAmounts(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetOrdersHighestAndLowestAmountsHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ProjectsTop5StatsModel>), 200)]
		public IActionResult GetProjectsTop5Stats(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetTop5ProjetcsStatsHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<OrdersTop5StatsModel>), 200)]
		public IActionResult GetOrdersTop5Stats(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetTop5OrdersStatsHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<WorstSuppliersStatsModel>), 200)]
		public IActionResult GetWorstSuppliersStats(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetWorstSupplierStatsHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<BestSuppliersStatsModel>), 200)]
		public IActionResult GetBestSuppliersStats(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetBestSuppliersStatsHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<BestAndWorstCustomersStatsModel>), 200)]
		public IActionResult GetBestAndWorstCustomers(StatsRequestModel data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetBestAndWorstCustomersHandler(this.GetCurrentUser(), data)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<IEnumerable<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUsersByDepartment(int? departmentId)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Statistics.GetUsersByDepartmentHandler(this.GetCurrentUser(), departmentId)
					.Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Project.StatusResponseModel>>), 200)]
		public IActionResult GetAllProjectsbyStatus()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjetStatusHandler(this.GetCurrentUser()).Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetProjectStatusesDistinct()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectStatusesDistinctHandler(this.GetCurrentUser()).Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectsOverviewModel>>), 200)]
		public IActionResult GetProjectOverviewByStatus(StatsRequestModelStatus data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectsByStatusAndYearHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectsOverviewModel>>), 200)]
		public IActionResult GetProjectOverviewByType(StatsRequestModelType data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectsByTypeAndYearHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectsOverviewModel>>), 200)]
		public IActionResult GetProjectOverviewByCustomer(StatsRequestBestSuppliersOverviewOrdersCount data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectsOverviewByCustomerHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OrdersOverviewModel>>), 200)]
		public IActionResult GetOrdersOverview(StatsRequestModelOrdersOverview data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOrdersOverviewHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectsOverviewModel>>), 200)]
		public IActionResult GetProjectOverviewMonthly(StatsRequestModelOrdersOverviewMonthly data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectsOverviewMonthlyHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OrdersOverviewModel>>), 200)]
		public IActionResult GetOrdersOverviewMonthly(StatsRequestModelOrdersOverviewMonthly data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOrdersMonthlyOverviewHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OrdersOverviewModel>>), 200)]
		public IActionResult GetBestSuppliersOverviwOrdersCount(StatsRequestBestSuppliersOverviewOrdersCount data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetBestSuppliersOverviwOrdersCountHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OrdersOverviewModel>>), 200)]
		public IActionResult GetBestSuppliersOverviewBiggetCountOfOverdueOrders(StatsRequestBestSuppliersOverviewOrdersCount data)
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetBestSuppliersOverviewBiggetCountOfOverdueOrdersHandler(this.GetCurrentUser(), data).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<StatsPropsModel>), 200)]
		public IActionResult GetStatsProps()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetStatsPropsHandler(this.GetCurrentUser()).
					Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//-- adda work
		#region >>Get Project type distinct
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetProjectTypesDistinct()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectTypesDistinctHandler(this.GetCurrentUser()).Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
		#region >> Get project by types
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Project.TypeResponseModel>>), 200)]
		public IActionResult GetProjectByTypes()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectByTypesHandler(this.GetCurrentUser()).Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
		#region >> Allocation vs Order Amount : Orders amounts associating with project

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<AmountOfOrderAssociatedWithProjectResponseModel>>), 200)]
		public IActionResult GetOrderAmount(int? year)
		{
			try
			{

				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOrderAmountHandler(this.GetCurrentUser(), year.Value).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#endregion
		#region >>  All Distinct Status || Type || Payement related to Order

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<StatisticAmounts>>), 200)]
		public IActionResult AllOrderStatusesTypePayementDistinct()
		{
			try
			{

				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOrderPayementHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#endregion
		#region  >> Order type statistical analysis
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<StatisticAmounts>>), 200)]
		public IActionResult GetOrderTypes()
		{
			try
			{

				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOrderTypesHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#endregion
		#region  >> Order Validation Levels statistical analysis
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<StatisticAmounts>>), 200)]
		public IActionResult GetOrderValidationLevels()
		{
			try
			{

				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOrderValidationLevelHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#endregion
		#region  >> Allocation vs Expenses (show Internal orders vs  External orders, direct vs leasing) 

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<AllocationAndExpenseResponseModel>), 200)]
		public IActionResult GetComaparisonBetweenAllocationAndExpenses(int year, int companyId)
		{
			try
			{

				var response = new Core.FinanceControl.Handlers.Budget.Statistics.ComaparisonBetweenAllocationAndExpensesHandler(this.GetCurrentUser(), year, companyId).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// -- Get All Company

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<StatisticAmounts>>), 200)]
		public IActionResult GetAllCompany()
		{
			try
			{

				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetAllCompanyHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}



		#endregion
		#region >>> Monthly view: foreach month : Bar chart
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<ProjectByMonthAllViewResponseModel>), 200)]
		public IActionResult ProjectByMonth(int? year) // Usefull 
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectByMonthFullHandler(this.GetCurrentUser(), year)
					.Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>>), 200)]
		public IActionResult GetNumberOfOrdersPerProjectType(int? year) // 29-02-2024 -- 12-03-2024
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetNumberOfOrdersPerProjectTypeHandler(this.GetCurrentUser(), year)
					.Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<ProjectByMonthAllViewResponseModel>), 200)]
		public IActionResult GetAllProjectStatusesDistinctPerMonth(int? year) // Add year for dropdown list 
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetAllProjectStatusesDistinctPerMonthHandler(this.GetCurrentUser(), year)
					.Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectByMonthResponseModel>>), 200)]
		public IActionResult GetProjectByMonth(int? year) // --29/02/2024
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectByMonthHandler(this.GetCurrentUser(), year)
					.Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectTypeByMonthResponseModel>>), 200)]
		public IActionResult GetProjectTypeByMonth(int? year) //--29/02/2024 
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectTypeByMonthHandler(this.GetCurrentUser(), year)
					.Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, year);
			}
		}

		#region >>> refactoring version ( GetProjectTypeByMonth ) 11-03-2024  << test to see if it works >>
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectTypeByMonthResponseModel>>), 200)]
		public IActionResult GetProjectTypeByMonthV2(int? year) // remade version, with refactoring
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectTypeByMonthV2Handler(this.GetCurrentUser(), year)
					.Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, year);
			}
		}
		#endregion

		#endregion
		#region >> Get Data From last year until yesterday << case highest amount >> 

		// -- 08-02-2024
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<CheapestOrderModel>>), 200)]
		public IActionResult GetHighestOrderAmountFromLastYear()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetDataFromLastYearHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
		#region >> Get Data From last year until today << case cheapest amout >> 

		//--08-02-2024
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<CheapestOrderModel>>), 200)]
		public IActionResult GetCheapestOrderAmountLastyearUntilYeaterday()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetCheapestOrderAmountLastyearUntilYeaterdayHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		#endregion
		#region  >>> Fastest-Smallest first validation to last booking time difference.

		// -- 09-02-2024
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<FastestResponseModel>), 200)]
		public IActionResult GetFastestFirtValidationDiffToLastBookingTime()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetFastestFirtValidationDiffToLastBookingTimeHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}


		#endregion
		#region >> Cheapest first validation to last booking time difference.
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<CheapestResponseModel>), 200)]
		public IActionResult GetCheapestFirstValidationDiffToLastBookingTime()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetCheapestFirtValidationDiffToLastBookingTimeHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}

		#endregion
		#region  >>>  Highest delay - biggest time difference between today and confirmed date for not delilvered orders

		// -- 12/02/2024

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OrderDiffModel>>), 200)]
		public IActionResult BiggestTimeDifferenceBetweenToday()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.BiggestTimeDifferenceBetweenTodayHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}


		#endregion
		#region >>>  biggest allocation (filters: only active or everything)
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<ProjectOverViewModel>), 200)]
		public IActionResult ProjectBiggestAllocation() // All About project --Overview--   // FIXME : remane Action Name
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.ProjectBiggestAllocationHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion
		#region  >>> Section Project


		#region >>> Biggest Allocation Project
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BiggestAllocationResponseModel>>), 200)]
		public IActionResult GetBiggestAllocationProject()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetBiggestAllocationProjectHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion



		#region >>> Biggest Sum of Order
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectBiggestSumOfOrderResponseModel>>), 200)]
		public IActionResult GetProjectBiggestSumOfOrder()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetProjectBiggestSumOfOrderHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion


		#region >>> Oldest First Approval
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BiggestAllocationResponseModel>>), 200)]
		public IActionResult GetOldestFirstApproval()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOldestFirstApprovalHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion


		#region >>> Customer Statistic management 
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BiggestOfferResponseModel>>), 200)]
		public IActionResult GetBiggestProfit()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetBiggestProfitHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion

		#region >>> Customer Statistic management 
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OverbudgetedResponseModel>>), 200)]
		public IActionResult GetOverbudgeted()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetOverbudgetedHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion

		#region >>> Closed Project With Remaining Budget
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BudgetLeakResponseModel>>), 200)]
		public IActionResult GetClosedProjectWithRemainingBudget()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetClosedProjectWithRemainingBudgetHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}

		#endregion

		#endregion
		#region >>> Section Customer


		#region >>>   Best Custommers Biggest Profils in External Project
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BiggestOfferResponseModel>>), 200)]
		public IActionResult GetBestCustommersBiggestProfils()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetBestCustommersBiggestProfilsHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion


		#region >>> Worst Custommers Smallest Profil in external Order

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BiggestOfferResponseModel>>), 200)]
		public IActionResult GetWorstCustommersSmallestProfils()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetWorstCustommersSmallestProfilsHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion


		#region >>>  Biggest customers: Highest PSZOffer in external projects

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BiggestOfferResponseModel>>), 200)]
		public IActionResult BiggestCustomersHighestPSZOffer()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.BiggestCustomersHighestPSZOfferHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion

		#region >>>  Smallest customers lowest PSZOffer
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<BiggestOfferResponseModel>>), 200)]
		public IActionResult SmallestCustomersLowestPSZOffer()
		{
			try
			{
				var response = new Core.FinanceControl.Handlers.Budget.Statistics.SmallestCustomersLowestPSZOfferHandler(this.GetCurrentUser()).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}
		#endregion
		#endregion
		#region >> Do the difference between Allocation and Expense Order
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<StatisticAmounts>>), 200)]
		public IActionResult GetComparisonBetweenAllocationAndExpenseOrder(int year, int companyId, int departmentId)
		{
			try
			{

				var response = new Core.FinanceControl.Handlers.Budget.Statistics.GetComparisonBetweenAllocationAndExpenseOrderHandler(this.GetCurrentUser(), year, companyId, departmentId).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#endregion
		#region >> Allocation vs Expenses - Internal orders  External orders (pi-chart) 

		#endregion
		#region >>> Received Order  and Unreceived order + last receipt 
		#endregion
	}
}