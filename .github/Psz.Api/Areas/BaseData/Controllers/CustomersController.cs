using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Models;
using Psz.Core.BaseData.Models.Address;
using Psz.Core.BaseData.Models.Customer;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]

	// cts controller
	public class CustomersController: ControllerBase
	{
		private const string MODULE = "BaseData";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetDeliveryAddresses()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetDeliveryAddressesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Address.AddressModel>>), 200)]
		public IActionResult GetDeliveryAddressesFull()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetDeliveryAddressesFullHandler(this.GetCurrentUser())
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
		public IActionResult GetCustomerGroups()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomerGroupsHandler(this.GetCurrentUser())
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
		public IActionResult GetPaymentTerms()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetPaymentTermsHandler(this.GetCurrentUser())
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
		public IActionResult GetCustomerNumbers(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomerNumbersHandler(this.GetCurrentUser(), searchText)
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
		public IActionResult GetCustomerNumbersV2(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomerNumbersV2Handler(this.GetCurrentUser(), searchText)
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
		public IActionResult GetCustomerNamesV2(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomerNamesV2Handler(this.GetCurrentUser(), searchText)
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
		public IActionResult GetCustomerNamesV3(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomerNamesV3Handler(this.GetCurrentUser(), searchText)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, searchText);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetNewCutomerNumber()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetNewCustomerNumberHandler(this.GetCurrentUser())
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
		public IActionResult Create(Psz.Core.BaseData.Models.Customer.CreateCustomerModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.CreateCustomerHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CreateForSupplier(Psz.Core.BaseData.Models.Customer.CreateCustomerModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.CreateCustomerForSupplierHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Update(Core.BaseData.Models.Customer.UpdateModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer Update");
				var response = new Core.BaseData.Handlers.Customer.UpdateHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Customer.CustomerModel>>), 200)]
		public IActionResult Get()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Customer.CustomerModel>), 200)]
		public IActionResult GetById(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Customer.SearchCustomerResponseModel>>), 200)]
		public IActionResult Search(Core.BaseData.Models.Customer.SearchCustomerModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.SearchCustomerHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Customer.SearchCustomerResponseModel>>), 200)]
		public IActionResult SearchV2(Core.BaseData.Models.Customer.SearchCustomerModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.SearchCustomerV2Handler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult Delete(int id)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer Delete");
				var response = new Core.BaseData.Handlers.Customer.DeleteCustomerHandler(id, this.GetCurrentUser())
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
		public async Task<IActionResult> UpdateImage([FromForm] Models.Articles.UpdateImageModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(await new Core.BaseData.Handlers.Customer.UpdateImageHandler(this.GetCurrentUser(), model).Handleasync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		#region new APIs
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Customer.OverviewModel>), 200)]
		public IActionResult GetCostumerOverview(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.Overview.GetCostumerOverviewHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Customer.CustomerDataModel>), 200)]
		public IActionResult GetCostumerData(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.Data.GetCustomerDataHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateCostumerData(Core.BaseData.Models.Customer.CustomerDataModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer UpdateData");
				return Ok(new Core.BaseData.Handlers.Customer.Data.UpdateCustomerDataHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.ObjectLog.ObjectLogModel>), 200)]
		public IActionResult GetHistory(int id)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Customer.GetCustomerHistoryHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Customer.CustomerAdressModel>), 200)]
		public IActionResult GetCostumerAdress(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.Adress.GetCustomerAdressHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateCostumerAdress(Core.BaseData.Models.Customer.CustomerAdressModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer UpdateAddress");
				return Ok(new Core.BaseData.Handlers.Customer.Adress.UpdateCostumerAdressHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.ContactPersonModel>>), 200)]
		public IActionResult GetCostumerContactPerson(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.ContactPerson.GetCustomerContactPersonHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddCostumerContactPerson(Core.BaseData.Models.Supplier.ContactPersonModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Customer.ContactPerson.AddContactPersonHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateCostumerContactPerson(Core.BaseData.Models.Supplier.ContactPersonModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer UpdateCPerson");
				return Ok(new Core.BaseData.Handlers.Customer.ContactPerson.UpdateContactPersonHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteCostumerContactPerson(int id)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer Delete");
				return Ok(new Core.BaseData.Handlers.Customer.ContactPerson.DeleteCustomerContactPersonHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.ShippingModel>), 200)]
		public IActionResult GetCostumerShipping(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.Shipping.GetCustomerShippingHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateCustomerShipping(Psz.Core.BaseData.Models.Customer.CustomerShippingModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer UpdateShipping");
				return Ok(new Core.BaseData.Handlers.Customer.Shipping.UpdateCustomerShippingHandler(data, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Customer.CustomerCommunicationModel>), 200)]
		public IActionResult GetCostumerCommunication(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.Communication.GetCustomerCommunicationHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateCustomerCommunication(Psz.Core.BaseData.Models.Customer.CustomerCommunicationModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Customer UpdateCommunication");
				return Ok(new Core.BaseData.Handlers.Customer.Communication.UpdateCustomerCommunicationHandler(data, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UnarchiveCustomer(int Id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Customer.UnarchiveCustomerHandler(Id, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.BaseData.Models.Customer.CustomerDropdownModel>>), 200)]
		public IActionResult GetCustomerDropdown()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Customer.GetCustomersDropdownHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCustomerDropdown_2()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Customer.GetCustomersDropdown_2Handler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion


		#region // -- Address -- //
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetAddressCategories()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.GetCategoriesHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddAddress(Core.BaseData.Models.Address.AddAdresseRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult EditAddress(Core.BaseData.Models.Address.AddressModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.EditHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<GetAdressesListResponseModel>), 200)]
		public IActionResult GetAddresses(GetAddressesListRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.GetAllHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Address.AddressModel>), 200)]
		public IActionResult GetAddress(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.GetHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteAddress(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.DeleteHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ToggleAddressBlock(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.BlockHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CopyAddress(Core.BaseData.Models.Address.CopyRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Address.CopyHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		#endregion Address


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<GetCustomerNamesForArticleReferenceModel>>), 200)]
		public IActionResult GetCustomerNamesForArticleReference(GetCustomerNamesForArticleReferenceRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomerNamesForArticleReferenceHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data.searchtext);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<CustomerCommunicationModel>), 200)]
		public IActionResult GetByKundenNummer(int kundenNummer)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Customer.GetCustomerByKundenNummerHandler(this.GetCurrentUser(), kundenNummer)
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