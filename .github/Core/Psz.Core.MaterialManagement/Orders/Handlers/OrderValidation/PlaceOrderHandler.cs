using Psz.Core.MaterialManagement.Orders.Models.OrderValidation;
using Psz.Core.SharedKernel.Interfaces;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class PlaceOrderHandler: IHandle<placeOrderRequestModel, ResponseModel<PlaceOrderResponseModel>>
	{
		private placeOrderRequestModel data { get; set; }
		private UserModel user { get; set; }

		public PlaceOrderHandler(UserModel user, placeOrderRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<PlaceOrderResponseModel> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//Change to Async later on.
		private ResponseModel<PlaceOrderResponseModel> Perform()
		{
			try
			{
				var orderEntity = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(Convert.ToInt32(this.data.OrderId));
				var attachments = new List<KeyValuePair<string, System.IO.Stream>> { };
				bool emailSent = false;


				var orderPlacementHistory = new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity();
				//Generate Order.
				try
				{
					var orderData = Infrastructure.Data.Access.Joins.MTM.Order.OrderValidationAccess.GetReportData(Convert.ToInt32(this.data.OrderId));
					orderData = orderData.OrderBy(x => x.Position)?.ToList();
					byte[] report;
					if(orderEntity.Typ == "Bestellung" && orderEntity.Mandant == "PSZ electronic" && (!orderEntity.Rahmenbestellung.HasValue || orderEntity.Rahmenbestellung.Value == false))
					{
						var data = new Infrastructure.Services.Reporting.IText.PRS.OrderModel(orderData[0]);
						data.PositionItems = orderData.Select(x => new Infrastructure.Services.Reporting.IText.PRS.OrderModel.PositionModel(x))?.ToList();
						report = Infrastructure.Services.Reporting.IText.PRS.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
					}
					else if(orderEntity.Typ == "Bestellung" && orderEntity.Mandant == "PSZ electronic" && orderEntity.Rahmenbestellung == true)
					{
						var data = new Infrastructure.Services.Reporting.IText.PRS.OrderModel(orderData[0]);
						data.PositionItems = orderData.Select(x => new Infrastructure.Services.Reporting.IText.PRS.OrderModel.PositionModel(x))?.ToList();
						report = Infrastructure.Services.Reporting.IText.PRS.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
					}
					else
					{ report = null; }

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
					if(this.data.Files != null && this.data.Files.Count > 0)
					{
						var listfiles = new List<Infrastructure.Data.Entities.Tables.FileEntity>();

						foreach(var fileItem in this.data.Files)
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
				ccEmailaddresses.Add(this.user.Email);

				if(!string.IsNullOrWhiteSpace(this.data.OrderPlacementCCEmail))
					ccEmailaddresses.AddRange(this.data.OrderPlacementCCEmail.Split(';'));

				orderPlacementHistory.OrderId = orderEntity.Nr;
				orderPlacementHistory.SenderUserEmail = this.user.Email;
				orderPlacementHistory.SenderUserId = this.user.Id;
				orderPlacementHistory.SenderUserName = this.user.Username;
				orderPlacementHistory.SenderCC = true;
				orderPlacementHistory.ToEmail = this.data.SupplierEmail;
				orderPlacementHistory.AttachmentIds = attachementIds == null ? "" : string.Join('|', attachementIds);
				orderPlacementHistory.EmailMessage = this.data.EmailBody;
				orderPlacementHistory.EmailTitle = this.data.EmailTitle;
				orderPlacementHistory.SendingTime = DateTime.Now;

				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.Insert(new Helpers.LogHelper(
					orderEntity.Nr,
					orderEntity.Bestellung_Nr ?? -1,
					int.TryParse(orderEntity.Projekt_Nr, out var val) ? val : 0,
					orderEntity.Typ,
					Helpers.LogHelper.LogType.PLACEORDER,
					"MTM",
					user).LogMTM(orderEntity.Nr));

				try
				{
					emailSent = Module.EmailingService.SendEmail(
					   this.data.EmailTitle,
					   this.data.EmailBody,
					   this.data.SupplierEmail?.Split(';')?.ToList(),
					   attachments,
					   ccEmailaddresses,
					   false
					   );
					Infrastructure.Data.Access.Tables.PRS.OrderPlacementHistoryAccess.Insert(orderPlacementHistory);

				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", this.data.SupplierEmail)}]", ex));
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
					user).LogMTM(orderEntity.Nr));
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

		public ResponseModel<PlaceOrderResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<PlaceOrderResponseModel>.AccessDeniedResponse();
			}

			if(user.Number == 0)
				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("User need to have a User Number");

			if(!int.TryParse(this.data.OrderId, out int orderId))
			{
				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("Order Id incorrect format.");
			}

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(orderId);

			if(bestellung == null)
				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("Order not found");

			if(bestellung.gebucht == null || bestellung.gebucht == false)
			{
				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("Can't place unvalidated Order.");

			}

			var orderItems = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(bestellung.Nr);
			if(orderItems == null || orderItems.Count == 0)
			{
				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("No Position found");
			}

			foreach(var orderITem in orderItems)
			{
				if(orderITem.Bestatigter_Termin is null || orderITem.Liefertermin is null)
				{
					return ResponseModel<PlaceOrderResponseModel>.FailureResponse("All Positions should have an Delivery Date and a Confirmed Delivery Date ");
				}
			}

			var Lieferanten = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(bestellung.Lieferanten_Nr.HasValue ? bestellung.Lieferanten_Nr.Value : 0);
			if(Lieferanten is null)
			{
				return ResponseModel<PlaceOrderResponseModel>.FailureResponse("No Supplier found");
			}

			return ResponseModel<PlaceOrderResponseModel>.SuccessResponse();
		}
	}
}
