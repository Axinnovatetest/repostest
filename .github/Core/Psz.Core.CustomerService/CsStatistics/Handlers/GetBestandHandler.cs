using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetBestandHandler: IHandle<Identity.Models.UserModel, ResponseModel<BestandReportModel>>
	{
		private BestandEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBestandHandler(Identity.Models.UserModel user, BestandEntryModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<BestandReportModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

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
					//get lists
					var _contacts = bestandEntity.Select(a => a.CS_Kontakt).Distinct().ToList();
					var _clients = bestandEntity.Select(a => a.Kunde).Distinct().ToList();

					//filling contacts list
					foreach(var contact in _contacts)
					{
						var sumWertContact = bestandEntity.Where(a => a.CS_Kontakt == contact).Sum(b => b.Wert ?? 0);
						var sumVKContact = bestandEntity.Where(a => a.CS_Kontakt == contact).Sum(b => b.VK ?? 0);
						ReportData.Contact.Add(new BestandReportContactModel(contact, sumWertContact, sumVKContact));
					}
					//filling clients list
					foreach(var client in _clients)
					{
						var sumWertClient = bestandEntity.Where(a => a.Kunde == client).Sum(b => b.Wert ?? 0);
						var sumVKClient = bestandEntity.Where(a => a.Kunde == client).Sum(b => b.VK ?? 0);
						var contactClient = bestandEntity.FirstOrDefault(a => a.Kunde == client).CS_Kontakt;
						ReportData.Clients.Add(new BestandReportClientsModel(client, contactClient, sumWertClient, sumVKClient));
						//filing lager list
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


				return ResponseModel<BestandReportModel>.SuccessResponse(ReportData);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<BestandReportModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<BestandReportModel>.AccessDeniedResponse();
			}

			return ResponseModel<BestandReportModel>.SuccessResponse();
		}
	}
}

