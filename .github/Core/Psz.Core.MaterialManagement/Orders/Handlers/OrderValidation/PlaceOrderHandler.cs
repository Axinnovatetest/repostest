using Psz.Core.MaterialManagement.Orders.Models.OrderValidation;
using Psz.Core.SharedKernel.Interfaces;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class PlaceOrderHandler: IHandleAsync<placeOrderRequestModel, ResponseModel<PlaceOrderResponseModel>>
	{
		private placeOrderRequestModel _data { get; set; }
		private UserModel _user { get; set; }

		public PlaceOrderHandler(UserModel user, placeOrderRequestModel data)
		{
			this._data = data;
			this._user = user;
		}

		public async Task<ResponseModel<PlaceOrderResponseModel>> HandleAsync()
		{
			try
			{
				var validation = await ValidateAsync();
				if(!validation.Success)
				{
					return validation;
				}

				return await Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//Change to Async later on.
		private async Task<ResponseModel<PlaceOrderResponseModel>> Perform()
		{
			try
			{
				var orderEntity = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(Convert.ToInt32(this._data.OrderId));
				var attachments = new List<KeyValuePair<string, System.IO.Stream>> { };
				bool emailSent = false;


				var orderPlacementHistory = new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity();
				//Generate Order.
				try
				{
					var orderData = Infrastructure.Data.Access.Joins.MTM.Order.OrderValidationAccess.GetReportData(Convert.ToInt32(this._data.OrderId));
					orderData = orderData.OrderBy(x => x.Position)?.ToList();
					var data = new Psz.Core.MaterialManagement.Reporting.Models.OrderModel(orderData[0]);
					data._Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
					data.PositionItems = orderData.Select(x => new Psz.Core.MaterialManagement.Reporting.Models.OrderModel.PositionModel(x))?.ToList();
					data.PositionItems.ForEach(x => x.TotalPrice = x.StartAnzahl * (decimal.TryParse(x.Einzelpreis, out var y) ? y : 0));
					var footerData = Reporting.Models.OrderFooterModel.CreateFooter();
					var reportParams = new Reporting.Models.ITextHeaderFooterProps
					{
						BodyData = data,
						DocumentTitle = "",
						BodyTemplate = "PRS_ORD_Body",
						FooterCenterText = "",
						FooterData = footerData,
						FooterTemplate = "PRS_ORD_Footer",
						HasFooter = true,
						FooterLeftText = "",
						HasHeader = true,
						FooterWithCounter = false,
						HeaderFirstPageOnly = false,
						HeaderLogoWithCounter = true,
						HeaderLogoWithText = false,
						HeaderText = "",
						Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}",
						Rotate = false
					};
					byte[] report = null;
					//if(orderEntity.Typ == "Bestellung" && orderEntity.Mandant == "PSZ electronic" && (!orderEntity.Rahmenbestellung.HasValue || orderEntity.Rahmenbestellung.Value == false))
					//{
					//	var data = new Psz.Core.MaterialManagement.Reporting.Models.OrderModel(orderData[0]);
					//	data.PositionItems = orderData.Select(x => new Psz.Core.MaterialManagement.Reporting.Models.OrderModel.PositionModel(x))?.ToList();
					//	report = Psz.Core.MaterialManagement.Reporting.IText.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
					//}
					//else if(orderEntity.Typ == "Bestellung" && orderEntity.Mandant == "PSZ electronic" && orderEntity.Rahmenbestellung == true)
					//{
					//	var data = new Psz.Core.MaterialManagement.Reporting.Models.OrderModel(orderData[0]);
					//	data.PositionItems = orderData.Select(x => new Psz.Core.MaterialManagement.Reporting.Models.OrderModel.PositionModel(x))?.ToList();
					//	report = Psz.Core.MaterialManagement.Reporting.IText.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
					//}
					if(orderEntity.Typ == "Bestellung" && orderEntity.Mandant == "PSZ electronic" && (!orderEntity.Rahmenbestellung.HasValue || orderEntity.Rahmenbestellung.Value == false))
					{
						data.IsKanban = false;
						report = await Reporting.IText.GetItextPDF(reportParams);
					}
					else if(orderEntity.Typ == "Bestellung" && orderEntity.Mandant == "PSZ electronic" && orderEntity.Rahmenbestellung == true)
					{
						data.IsKanban = false;
						report = await Reporting.IText.GetItextPDF(reportParams);
					}
					else if(orderEntity.Typ?.ToLower() == Enums.OrderEnums.OrderTypes.Kanaban.GetDescription().ToLower())
					{
						report = null;
					}
					//else
					//{ report = null; }

					if(report != null)
					{
						// Add PO to attachments.
						var fileName = $"{orderEntity.Bestellung_Nr}.pdf";
						attachments.Add(new KeyValuePair<string, System.IO.Stream>(fileName, new System.IO.MemoryStream(report) as System.IO.Stream));
					}
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
					return ResponseModel<PlaceOrderResponseModel>.FailureResponse("Error Sending Email.");
				}

				var attachementIds = new List<string>();

				// Add user attached files
				try
				{
					if(this._data.Files != null && this._data.Files.Count > 0)
					{
						var listfiles = new List<Infrastructure.Data.Entities.Tables.FileEntity>();

						foreach(var fileItem in this._data.Files)
						{
							byte[] fileBytes;
							var fileName = fileItem.FileName;
							using(var ms = new MemoryStream())
							{
								fileItem.CopyTo(ms);
								fileBytes = ms.ToArray();
							}

							attachments.Add(new KeyValuePair<string, System.IO.Stream>(fileName, new System.IO.MemoryStream(fileBytes)));
							var id = Common.Helpers.ImageFileHelper.updateImage(null, fileBytes, fileName.Substring(fileName.IndexOf('.') + 1), null, fileName.Substring(0, fileName.IndexOf('.')));
							attachementIds.Add(id.ToString());
						}
					}
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
					return ResponseModel<PlaceOrderResponseModel>.FailureResponse("Error Sending Email.");
				}

				// Add user attachements
				var ccEmailaddresses = new List<string> { };
				ccEmailaddresses.Add(this._user.Email);

				if(!string.IsNullOrWhiteSpace(this._data.OrderPlacementCCEmail))
					ccEmailaddresses.AddRange(this._data.OrderPlacementCCEmail.Split(';'));

				orderPlacementHistory.OrderId = orderEntity.Nr;
				orderPlacementHistory.SenderUserEmail = this._user.Email;
				orderPlacementHistory.SenderUserId = this._user.Id;
				orderPlacementHistory.SenderUserName = this._user.Username;
				orderPlacementHistory.SenderCC = true;
				orderPlacementHistory.ToEmail = this._data.SupplierEmail;
				orderPlacementHistory.AttachmentIds = attachementIds == null ? "" : string.Join('|', attachementIds);
				orderPlacementHistory.EmailMessage = this._data.EmailBody;
				orderPlacementHistory.EmailTitle = this._data.EmailTitle;
				orderPlacementHistory.SendingTime = DateTime.Now;

				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.Insert(new Helpers.LogHelper(
					orderEntity.Nr,
					orderEntity.Bestellung_Nr ?? -1,
					int.TryParse(orderEntity.Projekt_Nr, out var val) ? val : 0,
					orderEntity.Typ,
					Helpers.LogHelper.LogType.PLACEORDER,
					"MTM",
					_user).LogMTM(orderEntity.Nr));

				try
				{
					emailSent = Module.EmailingService.SendEmail(
					   this._data.EmailTitle,
					   this._data.EmailBody,
					   this._data.SupplierEmail?.Split(';')?.ToList(),
					   attachments,
					   ccEmailaddresses,
					   false
					   );
					Infrastructure.Data.Access.Tables.PRS.OrderPlacementHistoryAccess.Insert(orderPlacementHistory);

				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", this._data.SupplierEmail)}]", ex));
					return ResponseModel<PlaceOrderResponseModel>.FailureResponse("Error Sending Email.");
				}


				if(emailSent)
				{
					Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.Insert(new Helpers.LogHelper(
					orderEntity.Nr,
					orderEntity.Bestellung_Nr ?? -1,
					int.TryParse(orderEntity.Projekt_Nr, out var x) ? x : 0,
					orderEntity.Typ,
					Helpers.LogHelper.LogType.PLACESUCCESSORDER,
					"MTM",
					_user).LogMTM(orderEntity.Nr));
					//orderEntity.gedruckt = true; // To Check with Sani on Monday.
					//Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Update(orderEntity);
					return ResponseModel<PlaceOrderResponseModel>.SuccessResponse();
				}

				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("Error Sending Email.");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<ResponseModel<PlaceOrderResponseModel>> ValidateAsync()
		{
			if(_user == null)
			{
				return await ResponseModel<PlaceOrderResponseModel>.AccessDeniedResponseAsync();
			}

			if(_user.Number == 0)
				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("User need to have a User Number");

			if(!int.TryParse(this._data.OrderId, out int orderId))
			{
				return await ResponseModel<PlaceOrderResponseModel>.FailureResponseAsync("Order Id incorrect format.");
			}

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(orderId);

			if(bestellung == null)
				return await ResponseModel<PlaceOrderResponseModel>.FailureResponseAsync("Order not found");

			if(bestellung.gebucht == null || bestellung.gebucht == false)
			{
				return await ResponseModel<PlaceOrderResponseModel>.FailureResponseAsync("Can't place unvalidated Order.");

			}

			var orderItems = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(bestellung.Nr);
			if(orderItems == null || orderItems.Count == 0)
			{
				return await ResponseModel<PlaceOrderResponseModel>.FailureResponseAsync("No Position found");
			}

			foreach(var orderITem in orderItems)
			{
				if(orderITem.Bestatigter_Termin is null || orderITem.Liefertermin is null)
				{
					return await ResponseModel<PlaceOrderResponseModel>.FailureResponseAsync("All Positions should have an Delivery Date and a Confirmed Delivery Date ");
				}
			}

			var Lieferanten = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(bestellung.Lieferanten_Nr.HasValue ? bestellung.Lieferanten_Nr.Value : 0);
			if(Lieferanten is null)
			{
				return await ResponseModel<PlaceOrderResponseModel>.FailureResponseAsync("No Supplier found");
			}

			return await ResponseModel<PlaceOrderResponseModel>.SuccessResponseAsync();
		}
	}
}
