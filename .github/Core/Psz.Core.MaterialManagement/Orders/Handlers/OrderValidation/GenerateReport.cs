using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class GenerateReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int data { get; set; }
		public GenerateReportHandler(Identity.Models.UserModel user, int bestellungNr)
		{
			this._user = user;
			this.data = bestellungNr;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] responseBody = null;
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data);
				var orderData = Infrastructure.Data.Access.Joins.MTM.Order.OrderValidationAccess.GetReportData(data);
				orderData = orderData.OrderBy(x => x.Position)?.ToList();
				if(bestellung.Typ == "Bestellung" && bestellung.Mandant == "PSZ electronic" && (!bestellung.Rahmenbestellung.HasValue || bestellung.Rahmenbestellung.Value == false))
				{
					var data = new Infrastructure.Services.Reporting.IText.PRS.OrderModel(orderData[0]);
					data.PositionItems = orderData.Select(x => new Infrastructure.Services.Reporting.IText.PRS.OrderModel.PositionModel(x))?.ToList();
					responseBody = Infrastructure.Services.Reporting.IText.PRS.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
				}
				else if(bestellung.Typ == "Bestellung" && bestellung.Mandant == "PSZ electronic" && bestellung.Rahmenbestellung == true)
				{
					var data = new Infrastructure.Services.Reporting.IText.PRS.OrderModel(orderData[0]);
					data.PositionItems = orderData.Select(x => new Infrastructure.Services.Reporting.IText.PRS.OrderModel.PositionModel(x))?.ToList();
					responseBody = Infrastructure.Services.Reporting.IText.PRS.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
				}
				else if(bestellung.Typ?.ToLower() == Enums.OrderEnums.OrderTypes.Kanaban.GetDescription().ToLower())
				{
					var data = new Infrastructure.Services.Reporting.IText.PRS.OrderModel(orderData[0]);
					data.PositionItems = orderData.Select(x => new Infrastructure.Services.Reporting.IText.PRS.OrderModel.PositionModel(x))?.ToList();
					responseBody = Infrastructure.Services.Reporting.IText.PRS.GetOrderKanban(data,abEmailAddress: Module.ModuleSettings.ABDestinationEmailAddress);
				}

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data);
			if(bestellung == null)
				return ResponseModel<byte[]>.FailureResponse("Order Not Found");

			var orderData = Infrastructure.Data.Access.Joins.MTM.Order.OrderValidationAccess.GetReportData(data);
			if(orderData == null || orderData.Count == 0)
				return ResponseModel<byte[]>.FailureResponse("PDF Data Not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
