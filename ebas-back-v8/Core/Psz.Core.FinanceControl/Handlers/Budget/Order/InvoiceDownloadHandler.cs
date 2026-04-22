using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.IO;
	using System.IO.Compression;

	public class InvoiceDownloadHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private List<int> _data { get; set; }
		private string _languageCode { get; set; }
		public InvoiceDownloadHandler(List<int> orderIds, string langCode, UserModel user)
		{
			_user = user;
			_data = orderIds;
			_languageCode = langCode;
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

				//
				var tempFolder = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToString("yyyyMMddTHHmmss")}");
				int nbFiles = 0;
				if(Directory.Exists(tempFolder))
					Directory.Delete(tempFolder, true);

				Directory.CreateDirectory(tempFolder);

				foreach(var orderId in this._data)
				{
					var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderId);
					var invoiceEntities = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.GetByOrderId(orderId);
					if(invoiceEntities != null && invoiceEntities.Count > 0)
					{
						var poFolder = Path.Combine(tempFolder, orderEntity.OrderId.ToString());
						Directory.CreateDirectory(poFolder);
						foreach(var invoiceItem in invoiceEntities)
						{
							var pdfContent = InvoiceHandler.generateReportDataPDF(invoiceItem.Id, null, true);
							System.IO.File.WriteAllBytes(Path.Combine(poFolder, $"INV-{invoiceItem.Id}.pdf"), pdfContent);
							nbFiles++;
						}
					}
				}

				if(nbFiles > 0)
				{
					string zipPath = Path.Combine(Path.GetTempPath(), $"{this._user.Id}-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.zip");
					ZipFile.CreateFromDirectory(tempFolder, zipPath);
					return ResponseModel<byte[]>.SuccessResponse(File.ReadAllBytes(zipPath));
				}

				return ResponseModel<byte[]>.SuccessResponse(null);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			if(this._data == null || this._data.Count <= 0)
				return ResponseModel<byte[]>.FailureResponse(key: "1", value: "Empty list");

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<byte[]>.FailureResponse(key: "1", value: "User not found");


			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
