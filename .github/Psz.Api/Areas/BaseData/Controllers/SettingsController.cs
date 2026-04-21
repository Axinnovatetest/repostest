using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Models.CustomerSupplierLP;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class SettingsController: ControllerBase
	{
		private const string MODULE = "BaseData | Settings";
		private Psz.Core.BaseData.Interfaces.ISettings _settings;
		public SettingsController(Psz.Core.BaseData.Interfaces.ISettings settings)
		{
			_settings = settings;
		}

		//Discount Groups
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetDiscountGroups()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.DiscountGroup.GetDiscountGroupsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.BaseData.Models.DiscountGroup.DiscountGroupModel>>), 200)]
		public IActionResult GetDiscountGroupList(int Id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Settings.DiscountGroup.GetDiscountGroupsForSetting(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddDiscountGroup(Core.BaseData.Models.DiscountGroup.DiscountGroupModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Settings.DiscountGroup.AddDiscountGroupHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteDiscountGroup(int Id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Settings.DiscountGroup.DeleteDiscountGroupHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.BaseData.Models.DiscountGroup.DiscountGroupModel>), 200)]
		public IActionResult UpdateDiscountGroup(Psz.Core.BaseData.Models.DiscountGroup.DiscountGroupModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Settings.DiscountGroup.UpdateDiscountGroupHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//Conditions assignement

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetConditionAssignments()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ConditionAssignment.GetConditionAssignmentsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.ConditionAssignment.ConditionAssignementModel>>), 200)]
		public IActionResult GetConditionAssignmentsForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ConditionAssignment.GetConditionAsiignementsgForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddConditionAssignement(Core.BaseData.Models.ConditionAssignment.ConditionAssignementModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Settings.ConditionAssignment.AddConditionAssignementHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteConditionAssignement(int Id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Settings.ConditionAssignment.DeleteConditionAssignementHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.BaseData.Models.ConditionAssignment.ConditionAssignementModel>), 200)]
		public IActionResult UpdateConditionAssignement(Psz.Core.BaseData.Models.ConditionAssignment.ConditionAssignementModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Settings.ConditionAssignment.UpdateConditionAssignementHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//Industry
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Industry.IndustryModel>>), 200)]
		public IActionResult GetAllIndustries()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Industry.GetAllIndustries(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Industry.IndustryModel>>), 200)]
		//public IActionResult GetSupplierIndustriesForSettings()
		//{
		//    try
		//    {
		//        var response = new Core.BaseData.Handlers.Settings.Industry.GetSupplierIndustriesForSettingsHandler(this.GetCurrentUser())
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
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCustomerIndustries()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Industry.GetCustomerIndustriesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetSupplierIndustries()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Industry.GetSuppliersIndustriesHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddIndustry(Core.BaseData.Models.Industry.IndustryModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Industry.AddIndustryHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateIndustry(Core.BaseData.Models.Industry.IndustryModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Industry.UpdateIndustryHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteIndustry(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Industry.DeleteIndustryHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Supplier Groups
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetSuppliersGroups()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SuppliersGroup.GetSupplierGroupsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.SuppliersGroup.SuppliersGroupModel>>), 200)]
		public IActionResult GetSuppliersGroupsForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SuppliersGroup.GetSupplierGroupsForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddSupplierGroup(Psz.Core.BaseData.Models.SuppliersGroup.SuppliersGroupModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SuppliersGroup.AddSupplierGroupHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateSupplierGroup(Psz.Core.BaseData.Models.SuppliersGroup.SuppliersGroupModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SuppliersGroup.UpdateSupplierGroupHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteSupplierGroup(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SuppliersGroup.DeleteSupplierGroupHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Customer Groups
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetCustomerGroups()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomersGroup.GetCustomerGroupsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.CustomersGroup.CustomerGroupModel>>), 200)]
		public IActionResult GetCustomersGroupsForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomersGroup.GetCustomerGroupsForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddCustomerGroup(Psz.Core.BaseData.Models.CustomersGroup.CustomerGroupModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomersGroup.AddCustomerGroupHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateCustomerGroup(Psz.Core.BaseData.Models.CustomersGroup.CustomerGroupModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomersGroup.UpdateCustomerGroupHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteCustomerGroup(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomersGroup.DeleteCustomerGroupHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Payement Practices
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetPaymentPractices()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PayementPractice.GetPayementPracticeHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.PayementPractice.PayementPracticeModel>>), 200)]
		public IActionResult GetPaymentPracticesForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PayementPractice.GetPayementPracticeForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddPaymentPractice(Psz.Core.BaseData.Models.PayementPractice.PayementPracticeModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PayementPractice.AddPayementPracticeHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdatePaymentPractice(Psz.Core.BaseData.Models.PayementPractice.PayementPracticeModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PayementPractice.UpdatePayementPracticeHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeletePaymentPractice(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PayementPractice.DeletePayementPracticeHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Terms of payment
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetTermsOfPayement()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.TermsOfPayement.GetTermsOfPaymentHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.TermsOfPayement.TermsOfPayementModel>>), 200)]
		public IActionResult GetTermsOfPayementForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.TermsOfPayement.GetTermsOfPayementForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddTermsOfPayement(Psz.Core.BaseData.Models.TermsOfPayement.TermsOfPayementModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.TermsOfPayement.AddTermsOfPayementHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateTermsOfPayement(Psz.Core.BaseData.Models.TermsOfPayement.TermsOfPayementModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.TermsOfPayement.UpdateTermsOfPayementHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteTermsOfPayement(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.TermsOfPayement.DeleteTermsOfPayementHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCurrencies()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Currency.GeCurrenciestHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Currency.CurrencyModel>>), 200)]
		public IActionResult GetCurrenciesForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Currency.GetCurrenciesForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddCurrency(Psz.Core.BaseData.Models.Currency.CurrencyModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Currency.AddCurrencyHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateCurrency(Psz.Core.BaseData.Models.Currency.CurrencyModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Currency.UpdateCurrencyHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteCurrency(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Currency.DeleteCurrencyHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Slip circle
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetSlipCircles()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SlipCircle.GetSlipCirclesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.SlipCircle.SlipCircleModel>>), 200)]
		public IActionResult GetSlipCirclesForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SlipCircle.GetSlipCircleForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddSlipCircle(Psz.Core.BaseData.Models.SlipCircle.SlipCircleModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SlipCircle.AddSlipCircleHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateSlipCircle(Psz.Core.BaseData.Models.SlipCircle.SlipCircleModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SlipCircle.UpdateSlipCircleHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteSlipCircle(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.SlipCircle.DeleteSlipCircleHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Pricing Groups
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetPricingGroups()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PricingGroup.GetPricingGroupsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.PricingGroup.PricingGroupModel>>), 200)]
		public IActionResult GetPricingGroupsForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PricingGroup.GetPriceGroupForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddPricingGroup(Psz.Core.BaseData.Models.PricingGroup.PricingGroupModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PricingGroup.AddPriceGroupHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdatePricingGroup(Psz.Core.BaseData.Models.PricingGroup.PricingGroupModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PricingGroup.UpdatePricingGroupHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeletePricingGroup(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.PricingGroup.DeletePriceGroupHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Adress types
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetAddressTypes()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.AddressType.GetAdressTypesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.AddressType.AdreesTypeModel>>), 200)]
		public IActionResult GetAddressTypesForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.AddressType.GetAdressTypeForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddAdressType(Psz.Core.BaseData.Models.AddressType.AdreesTypeModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.AddressType.AddAdressTypeHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateAdressType(Psz.Core.BaseData.Models.AddressType.AdreesTypeModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.AddressType.UpdateAdressTypeHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteAdressType(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.AddressType.DeleteAdressTypeHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Fibu Frames
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCustomerFrames()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Frames.GetCustomerFramesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Frames.FramesModel>>), 200)]
		public IActionResult GetCustomerFramesForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Frames.GetCustomerFramesForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddCustomerFrame(Psz.Core.BaseData.Models.Frames.FramesModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Frames.AddCustomerFrameHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateCustomerFrame(Psz.Core.BaseData.Models.Frames.FramesModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Frames.UpdateCustomerFrameHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteCustomerFrame(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.Frames.DeleteCustomerFrameHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//Shipping Methods
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetShippingMethods()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ShippingMethods.GetShippingMethodsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.ShippingMethod.ShippingMethodModel>>), 200)]
		public IActionResult GetShippingMethodsForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ShippingMethods.GetShippingMethodsForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateShippingMethod(Psz.Core.BaseData.Models.ShippingMethod.ShippingMethodModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ShippingMethods.UpdateShippingMethodHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddShippingMethod(Psz.Core.BaseData.Models.ShippingMethod.ShippingMethodModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ShippingMethods.AddShippingMethodHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteShippingMethod(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ShippingMethods.DeleteShippingMethodHandler(this.GetCurrentUser(), Id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.ObjectLog.ObjectLogModel>>), 200)]
		public IActionResult GetSettingsHistory()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.GetSettingHistoryHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//Salutation contact person
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetSalutationContactPerson()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson.GetSalutationContactPersonHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.SalutationContactPerson.SalutationContactPersonModel>>), 200)]
		public IActionResult GetSalutationContactPersonForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson.GetSalutationContactPersonForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddSalutationContactPerson(Psz.Core.BaseData.Models.Settings.SalutationContactPerson.SalutationContactPersonModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson.AddSalutationContactPersonHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateSalutationContactPerson(Psz.Core.BaseData.Models.Settings.SalutationContactPerson.SalutationContactPersonModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson.UpdateSalutationContactPerson(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteSalutationContactPerson(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson.DeleteSalutationContactPersonHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		//Address contact person
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetAddressContactPerson()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson.GetAddressContactPersonHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.AddressContactPerson.AddressContactPersonModel>>), 200)]
		public IActionResult GetAddressContactPersonForSettings()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson.GetAddressContactPersonForSettingsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddAddressContactPerson(Psz.Core.BaseData.Models.Settings.AddressContactPerson.AddressContactPersonModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson.AddAddressContactPersonHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateAddressContactPerson(Psz.Core.BaseData.Models.Settings.AddressContactPerson.AddressContactPersonModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson.UpdateAddressContactPerson(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteAddressContactPerson(int Id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson.DeleteAddressContactPersonHandler(this.GetCurrentUser(), Id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//GetLPOutdated
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.CustomerSupplierLP.LPModel>>), 200)]
		public IActionResult DownloadLPOutdated_XLS(int nr, int mnths)
		{
			try
			{
				var input = new GetOutdatedArticlesExcel() { nr = nr, mnths = mnths };
				var data = new Core.BaseData.Handlers.Settings.LP.GetLPOutdatedHandler(this.GetCurrentUser(), input).GetDataXLS();

				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"outdated-prices_-_{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return Ok("Empty file sent.");
			}
		}
		//GetLPOutdatedAllSuppliers
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.CustomerSupplierLP.LPModel>>), 200)]
		public IActionResult DownloadLPOutdatedAllSuppliers_XLS(int nr)
		{
			try
			{
				var data = new Core.BaseData.Handlers.Settings.LP.GetLPOutdatedAllSuppliersHandler(this.GetCurrentUser(), nr).GetDataXLS();

				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"LPOutdated-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.CustomerSupplierLP.LPModel>>), 200)]
		public IActionResult GetLP(int nr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.LP.GetLPHandler(this.GetCurrentUser(), nr)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.CustomerSupplierLP.LPModel>>), 200)]
		public IActionResult ImportSupplierArticlesXLS([FromForm] LPMinimalRequestModel data)
		{
			try
			{
				//var data = new LPMinimalRequestModel() {ExcelFile = ExcelFile,nr = 5 };
				var response = new Core.BaseData.Handlers.Settings.LP.SupplierInformationUpdateHandler(this.GetCurrentUser(), data)
				  .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//SupplierInformationUpdateHandler
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.CustomerSupplierLP.LPModel>>), 200)]
		public IActionResult DownloadLP_XLS(int nr)
		{
			try
			{
				var data = new Core.BaseData.Handlers.Settings.LP.GetLPHandler(this.GetCurrentUser(), nr)
					.GetDataXLS();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, nr);
				return Ok(e.Message);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<bool, List<Psz.Core.BaseData.Models.LPCheckResponseModel>>>), 200)]
		public IActionResult CheckLPExsistance(int nr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CheckLPExsistanceHandler(this.GetCurrentUser(), nr)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult CheckSettingBeforeUpdate(KeyValuePair<int, string> data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CheckSettingBeforeUpdate(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		// - 2022-07-07
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetProjectManagersForCustomerItemNumbers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.GetProjectManagersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetTechniciansForCustomerItemNumbers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.GetTechniciansHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetStufeCustomerItemNumbers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.GetStufeHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetEmployeesForCustomerItemNumbers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.GetEmployeesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCostomersForCustomerItemNumbers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.GetCustomersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel>>), 200)]
		public IActionResult GetCustomerItemNumbers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.GetAllHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult AddCustomerItemNumbers(Psz.Core.BaseData.Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult EditCustomerItemNumbers(Psz.Core.BaseData.Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.EditHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult PromoteAnalyseCustomer(Psz.Core.BaseData.Models.Settings.CustomerItemNumber.PromoteToCustomerRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.PromoteToCustomerHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult DeleteCustomerItemNumbers(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CustomerItemNumber.DeleteHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.CustomerSupplierLP.LPModel>>), 200)]
		public IActionResult GetCustomerItemNumbers_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Settings.CustomerItemNumber.GetAllHandler(this.GetCurrentUser())
					.GetDataXLS();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		#region CoC Types
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetCoCTypesValues()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CocType.GetAllValuesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.CoCType.CoCTypeResponseModel>>), 200)]
		public IActionResult GetCoCTypes()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CocType.GetAllHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult AddCoCType(Psz.Core.BaseData.Models.Settings.CoCType.CoCTypeRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CocType.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult EditCoCType(Psz.Core.BaseData.Models.Settings.CoCType.CoCTypeRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CocType.EditHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult DeleteCoCType(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.CocType.DeleteHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		#endregion Coc

		#region EDI Concern
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>>), 200)]
		public IActionResult GetEdiConcernCustomerForCreate(bool ediActiveOnly = false)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.GetCustomersForCreateHandler(this.GetCurrentUser(), ediActiveOnly)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, ediActiveOnly);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>>), 200)]
		public IActionResult GetEdiConcerns()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.GetAllHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult AddEdiConcern(Psz.Core.BaseData.Models.Settings.EdiCustomerConcern.EdiConcernAddRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult EditEdiConcern(Psz.Core.BaseData.Models.Settings.EdiCustomerConcern.EdiConcernAddRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.EditHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult DeleteEdiConcern(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.DeleteHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult AddEdiConcernCustomer(Psz.Core.BaseData.Models.Settings.EdiCustomerConcern.EdiConcernAddWithCustomersRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.AddCustomersHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult RemoveEdiConcernCustomer(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.RemoveCustomerHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>>), 200)]
		public IActionResult GetEdiConcernItems(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Settings.EdiCustomerConcern.GetItemsHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		#endregion EDI Concern

		#region HourlyRates
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetHourlyRatesValues()
		{
			try
			{
				return Ok(_settings.HourlyRate_GetAllValues(this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateResponseModel>>), 200)]
		public IActionResult GetHourlyRates()
		{
			try
			{
				return Ok(_settings.HourlyRate_Get(this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult AddHourlyRate(Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateRequestModel data)
		{
			try
			{
				return Ok(_settings.HourlyRate_Add(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult EditHourlyRate(Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateRequestModel data)
		{
			try
			{
				return Ok(_settings.HourlyRate_Edit(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult DeleteHourlyRate(int id)
		{
			try
			{
				return Ok(_settings.HourlyRate_Delete(this.GetCurrentUser(), id));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateResponseModel>), 200)]
		public IActionResult GetByProductionSite(int id)
		{
			try
			{
				return Ok(_settings.HourlyRate_GetByProductionSite(this.GetCurrentUser(), id));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		#endregion HourlyRate
	}
}
