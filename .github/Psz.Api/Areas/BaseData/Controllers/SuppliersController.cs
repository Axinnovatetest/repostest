using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Models.Files;
using Psz.Core.BaseData.Models.Supplier;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class SuppliersController: ControllerBase
	{
		private const string MODULE = "BaseData";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.SupplierModel>>), 200)]
		public IActionResult Get()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetSuppliersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.AdressenListModel>>), 200)]
		public IActionResult GetSuppliersList(bool? includeLieferAddress = null)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetSuppliersListHandler(this.GetCurrentUser(), includeLieferAddress)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.AdressenListModel>>), 200)]
		public IActionResult GetFilteredSuppliersList(GetFilteredSuppliersListRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetFilteredSuppliersListHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.SupplierModel>), 200)]
		public IActionResult GetSingle(int id)
		{
			try
			{
				var requestData = new Core.BaseData.Models.Supplier.GetSupplierRequestModel()
				{
					SupplierId = id,
					User = this.GetCurrentUser(),
				};

				var response = new Core.BaseData.Handlers.Supplier.GetSupplierHandler(requestData)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.MinimalSupplierModel>>), 200)]
		public IActionResult GetMinimal()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetMinimalSuppliersHandler(this.GetCurrentUser())
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
		public IActionResult GetNewSupplierNumber()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetNewSupplierNumberHandler(this.GetCurrentUser())
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
		public IActionResult CreateSupplier(Psz.Core.BaseData.Models.Supplier.SupplierCreateModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier Create");
				var response = new Core.BaseData.Handlers.Supplier.CreateSupplierHandler(data, this.GetCurrentUser())
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
		public IActionResult CreateSupplierForCustomer(Psz.Core.BaseData.Models.Supplier.SupplierCreateModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.CreateSupplierForCustomerHandler(data, this.GetCurrentUser())
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
		public IActionResult Update(Core.BaseData.Models.Supplier.UpdateModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier Update");
				var response = new Core.BaseData.Handlers.Supplier.UpdateHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.SearchSupplierResponseModel>>), 200)]
		public IActionResult Search(Core.BaseData.Models.Supplier.SearchSupplierModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.SearchSupplierHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.SearchSupplierResponseModel>>), 200)]
		public IActionResult SearchV2(Core.BaseData.Models.Supplier.SearchSupplierModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.SearchSupplierV2Handler(data, this.GetCurrentUser())
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
		public IActionResult DeleteSupplier(int id)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier Delete");
				var response = new Core.BaseData.Handlers.Supplier.DeleteSupplierHandler(id, this.GetCurrentUser())
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
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Language.LanguageModel>>), 200)]
		public IActionResult GetLanguages()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Language.GetHandler(this.GetCurrentUser())
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
		public IActionResult GetSupplierNumbers(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetSupplierNumbersHandler(this.GetCurrentUser(), searchText)
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
		public IActionResult GetSupplierNumbersV2(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetSupplierNumbersV2Handler(this.GetCurrentUser(), searchText)
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
		public IActionResult GetSupplierNumbersV3(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetSupplierNumbersV3Handler(this.GetCurrentUser(), searchText)
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
		public IActionResult GetSupplierNames(string searchText)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetSupplierNamesHandler(this.GetCurrentUser(), searchText)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.GetGeoLocationModel>), 200)]
		public IActionResult GetGeolocation(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetGeoLocationHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateGeolocation(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.UpdateGeolocationHandler(this.GetCurrentUser(), id)
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
		public IActionResult SetGeolocation(Core.BaseData.Models.Supplier.SetGeolocationModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.SetGeolocationHandler(this.GetCurrentUser(), data)
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
				return Ok(await new Core.BaseData.Handlers.Supplier.UpdateImageHandler(this.GetCurrentUser(), model).Handleasync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.OverviewModel>), 200)]
		public IActionResult GetSupplierOverview(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.Overview.GetSupplierOverviewHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.SupplierDataModel>), 200)]
		public IActionResult GetSupplierData(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.Data.GetSupplierDataHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateSupplierData(Core.BaseData.Models.Supplier.SupplierDataModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier UpdateData");
				return Ok(new Core.BaseData.Handlers.Supplier.Data.UpdateSupplierDataHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.SupplierAdressModel>), 200)]
		public IActionResult GetSupplierAdress(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.Adress.GetSupplierAdressHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateSupplierAdress(Core.BaseData.Models.Supplier.SupplierAdressModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier UpdateAddress");
				return Ok(new Core.BaseData.Handlers.Supplier.Adress.UpdateSupplierAdressHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateSupplierAdressNotes(Core.BaseData.Models.Supplier.AddressNotesRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier UpdateAddressNote");
				return Ok(new Core.BaseData.Handlers.Supplier.Adress.UpdateSupplierNotesHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.ObjectLog.ObjectLogModel>), 200)]
		public IActionResult GetHistory(int id)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Supplier.GetSupplierHistoryHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.SupplierCommunicationModel>), 200)]
		public IActionResult GetSupplierCommunication(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.Communication.GetSupplierCommunicationHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateSupplierCommunication(Psz.Core.BaseData.Models.Supplier.SupplierCommunicationModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier UpdateCommunication");
				return Ok(new Core.BaseData.Handlers.Supplier.Communication.UpdateSupplierCommunication(data, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Supplier.ShippingModel>), 200)]
		public IActionResult GetSupplierShipping(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.Shipping.GetSupplierShippingHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateSupplierShipping(Psz.Core.BaseData.Models.Supplier.SupplierShippingModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier UpdateShipping");
				return Ok(new Core.BaseData.Handlers.Supplier.Shipping.UpdateSupplierShippingHandler(data, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//contact person
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Supplier.ContactPersonModel>>), 200)]
		public IActionResult GetSupplierContactPerson(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.ContactPerson.GetSupplierContactPersonHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddSupplierContactPerson(Core.BaseData.Models.Supplier.ContactPersonModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Supplier.ContactPerson.AddContactPersonHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateSupplierContactPerson(Core.BaseData.Models.Supplier.ContactPersonModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier UpdateCPerson");
				return Ok(new Core.BaseData.Handlers.Supplier.ContactPerson.UpdateContactPersonHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteSupplierContactPerson(int id)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Supplier DeleteCPerson");
				return Ok(new Core.BaseData.Handlers.Supplier.ContactPerson.DeleteCustomerContactPersonHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UnarchiveSupplier(int Id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Supplier.UnarchiveSupplierHandler(Id, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.BaseData.Models.Supplier.SupplierDropdownModel>>), 200)]
		public IActionResult GetSuppliersDropdown()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Supplier.GetSuppliersDropdownHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		[SupportedOSPlatform("windows")]
		public async Task<IActionResult> UploadSupplierAttachmentFiles([FromForm] SupplierFilesUploadModel data)
		{
			try
			{

				return Ok(await new Psz.Core.BaseData.Handlers.Supplier.Files.SuppliersFilesUploadAsyncHandler(this.GetCurrentUser(), data, this.HttpContext.RequestAborted).HandleAsync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		[SupportedOSPlatform("windows")]
		public async Task<IActionResult> GetFilesNameFromDirectory(GetSupplierAttachmentModel data)
		{
			try
			{
				return Ok(await new Psz.Core.BaseData.Handlers.Supplier.Files.GetFilesNameFromDirectoryAsyncHandler(this.GetCurrentUser(), data, this.HttpContext.RequestAborted).HandleAsync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<DownloadSupplierFileModel>), 200)]
		public async Task<IActionResult> ReadFileFromDisk(int ModuleId, int Module)
		{
			try
			{
				var data = new DownloadSupplierFileModel()
				{
					Module = Module,
					ModuleId = ModuleId,
				};
				var file = await new Psz.Core.BaseData.Handlers.Supplier.Files.ReadFileFromDiskAsyncHandler(this.GetCurrentUser(), data, this.HttpContext.RequestAborted).HandleAsync();

				if(file.Success && file.Body.file.Length > 0)
				{
					return new FileContentResult(file.Body.file, file.Body.MIMEtype)
					{
						FileDownloadName = $"{file.Body.FileName}"
					};
				}
				else
				{
					return Ok(await ResponseModel<FileContentResult>.FailureResponseAsync("Unable to fetch file"));
				}

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public async Task<IActionResult> DeleteFilesNameFromDirectory(DeleteSupplierFileModel data)
		{
			try
			{
				return Ok(await new Psz.Core.BaseData.Handlers.Supplier.Files.DeleteFileAsyncHandler(this.GetCurrentUser(), data, this.HttpContext.RequestAborted).HandleAsync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetLevels()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Supplier.GetLevelsHandler(this.GetCurrentUser())
				   .Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}