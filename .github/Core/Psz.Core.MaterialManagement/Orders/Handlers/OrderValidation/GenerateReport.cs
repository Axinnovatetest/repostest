using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class GenerateReportHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }
		public GenerateReportHandler(Identity.Models.UserModel user, int bestellungNr)
		{
			this._user = user;
			this._data = bestellungNr;
		}
		public async Task<ResponseModel<byte[]>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				byte[] responseBody = null;
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(_data);
				var orderData = Infrastructure.Data.Access.Joins.MTM.Order.OrderValidationAccess.GetReportData(_data);
				var data = new Psz.Core.MaterialManagement.Reporting.Models.OrderModel(orderData[0]);
				data._Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}";
				data.PositionItems = orderData.Select(x => new Psz.Core.MaterialManagement.Reporting.Models.OrderModel.PositionModel(x))?.ToList();
				data.PositionItems.ForEach(x => x.TotalPrice = x.StartAnzahl * (decimal.TryParse(x.Einzelpreis, out var y) ? y : 0));
				orderData = orderData.OrderBy(x => x.Position)?.ToList();
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
				if(bestellung.Typ == "Bestellung" && bestellung.Mandant == "PSZ electronic" && (!bestellung.Rahmenbestellung.HasValue || bestellung.Rahmenbestellung.Value == false))
				{
					data.IsKanban = false;
				}
				else if(bestellung.Typ == "Bestellung" && bestellung.Mandant == "PSZ electronic" && bestellung.Rahmenbestellung == true)
				{
					data.IsKanban = false;
				}
				else if(bestellung.Typ?.ToLower() == Enums.OrderEnums.OrderTypes.Kanaban.GetDescription().ToLower())
				{
					data.IsKanban = true;
					data.AbEmailAddress = Module.ModuleSettings.ABDestinationEmailAddress;
				}
				responseBody = await Reporting.IText.GetItextPDF(reportParams);
				//if(bestellung.Typ == "Bestellung" && bestellung.Mandant == "PSZ electronic" && (!bestellung.Rahmenbestellung.HasValue || bestellung.Rahmenbestellung.Value == false))
				//{
				//	var data = new Psz.Core.MaterialManagement.Reporting.Models.OrderModel(orderData[0]);
				//	data.PositionItems = orderData.Select(x => new Psz.Core.MaterialManagement.Reporting.Models.OrderModel.PositionModel(x))?.ToList();
				//	responseBody = Core.MaterialManagement.Reporting.IText.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
				//}
				//else if(bestellung.Typ == "Bestellung" && bestellung.Mandant == "PSZ electronic" && bestellung.Rahmenbestellung == true)
				//{
				//	var data = new Psz.Core.MaterialManagement.Reporting.Models.OrderModel(orderData[0]);
				//	data.PositionItems = orderData.Select(x => new Psz.Core.MaterialManagement.Reporting.Models.OrderModel.PositionModel(x))?.ToList();
				//	responseBody = Core.MaterialManagement.Reporting.IText.GetOrder(data, Module.ModuleSettings.ABDestinationEmailAddress);
				//}
				//else if(bestellung.Typ?.ToLower() == Enums.OrderEnums.OrderTypes.Kanaban.GetDescription().ToLower())
				//{
				//	var data = new Psz.Core.MaterialManagement.Reporting.Models.OrderModel(orderData[0]);
				//	data.PositionItems = orderData.Select(x => new Psz.Core.MaterialManagement.Reporting.Models.OrderModel.PositionModel(x))?.ToList();
				//	responseBody = Core.MaterialManagement.Reporting.IText.GetOrderKanban(data, abEmailAddress: Module.ModuleSettings.ABDestinationEmailAddress);
				//}

				return ResponseModel<byte[]>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public async Task<ResponseModel<byte[]>> ValidateAsync()
		{
			if(this._user == null)
			{
				return await ResponseModel<byte[]>.AccessDeniedResponseAsync();
			}

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(_data);
			if(bestellung == null)
				return await ResponseModel<byte[]>.FailureResponseAsync("Order Not Found");

			var orderData = Infrastructure.Data.Access.Joins.MTM.Order.OrderValidationAccess.GetReportData(_data);
			if(orderData == null || orderData.Count == 0)
				return await ResponseModel<byte[]>.FailureResponseAsync("PDF Data Not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
