using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports
{
	public class GetBestandReportHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private BestandEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBestandReportHandler(Identity.Models.UserModel user, BestandEntryModel data)
		{
			this._user = user;
			this._data = data;
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

				byte[] response = null;
				//initialising
				var ReportData = new BestandReportModel();
				ReportData.Contact = new List<BestandReportContactModel> { };
				ReportData.Clients = new List<BestandReportClientsModel> { };
				ReportData.Lager = new List<BestandReportLagerModel> { };
				var bestandEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetBestandProCSByFilter(_data.Contact, _data.Client, _data.Lager);
				if(bestandEntity != null && bestandEntity.Count > 0)
				{
					//filling details list
					ReportData.Details = bestandEntity.Select(a => new BestandReportDetailsModel(a)).ToList();
					//get ditincts lists
					var _contacts = bestandEntity.Select(a => a.CS_Kontakt).Distinct().ToList();
					var _clients = bestandEntity.Select(a => a.Kunde).Distinct().ToList();

					//filling contacts list
					ReportData.Contact = _contacts.Select(a => new BestandReportContactModel(
						  a,
						  bestandEntity.Where(x => x.CS_Kontakt == a).Sum(y => y.Wert ?? 0),
						  bestandEntity.Where(x => x.CS_Kontakt == a).Sum(y => y.VK ?? 0)
						  )).ToList();
					//filling clients list
					ReportData.Clients = _clients.Select(a => new BestandReportClientsModel(
							 a,
							  bestandEntity.FirstOrDefault(x => x.Kunde == a).CS_Kontakt,
							  bestandEntity.Where(x => x.Kunde == a).Sum(y => y.Wert ?? 0),
							  bestandEntity.Where(x => x.Kunde == a).Sum(y => y.VK ?? 0)
							 )).ToList();
					//filling lager list
					foreach(var client in _clients)
					{
						var lagerClient = bestandEntity.Where(a => a.Kunde == client).Select(b => b.Lagerort).Distinct().ToList();
						foreach(var lager in lagerClient)
						{
							var sumWertLager = bestandEntity.Where(a => a.Kunde == client && a.Lagerort == lager).Sum(b => b.Wert ?? 0);
							var sumVKLager = bestandEntity.Where(a => a.Kunde == client && a.Lagerort == lager).Sum(b => b.VK ?? 0);
							var contactClientLager = bestandEntity.FirstOrDefault(a => a.Kunde == client && a.Lagerort == lager).CS_Kontakt;
							ReportData.Lager.Add(new BestandReportLagerModel(lager, client, contactClientLager, sumWertLager, sumVKLager));
						}

					}
				}

				response = Module.CS_ReportingService.GenerateBestandReport(Enums.ReportingEnums.ReportType.CTS_BESTAND, ReportData);

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
