using OfficeOpenXml;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetFAVerschiebungHandler: IHandle<Identity.Models.UserModel, ResponseModel<FAVerschiebungResponseModel>>
	{

		private FAVerschiebungRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAVerschiebungHandler(Identity.Models.UserModel user, FAVerschiebungRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<FAVerschiebungResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new FAVerschiebungResponseModel();
				var entities = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.FehlerMaterialFAAccess.GetFAVerschiebung(_data.Lager, _data.Period);
				response = new FAVerschiebungResponseModel
				{
					Period = _data.Period,
					Lagerort = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.Get(_data.Lager)?.Lagerort,
					Data = entities?.Select(x => new FAVerschiebungModel(x)).ToList(),
				};

				return ResponseModel<FAVerschiebungResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<FAVerschiebungResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<FAVerschiebungResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<FAVerschiebungResponseModel>.SuccessResponse();
		}

		public byte[] GetExcel()
		{
			var data = this.Handle();
			var dataEntities = new List<FAVerschiebungModel>();
			if(data.Success)
			{
				dataEntities = data.Body.Data;
			}

			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"FA Verschiebung{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			ExcelPackage Ep = new ExcelPackage(file);
			var excel = Psz.Core.MaterialManagement.Helpers.ExcelHelper.FAVerschiebungExcel(dataEntities, Ep);
			Ep.Workbook.Properties.Title = "FA Verschiebung";
			Ep.Workbook.Properties.Author = "PSZ ERP";
			Ep.Workbook.Properties.Company = "PSZ ERP";
			Ep.Save();
			var response = File.ReadAllBytes(filePath);
			return response;
		}

	}
}
