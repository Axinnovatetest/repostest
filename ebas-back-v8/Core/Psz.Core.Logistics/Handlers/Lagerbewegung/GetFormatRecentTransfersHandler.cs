using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetFormatRecentTransfersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Lagebewegung.FormatRecentTransfer>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetFormatRecentTransfersHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<Models.Lagebewegung.FormatRecentTransfer>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<Models.Lagebewegung.FormatRecentTransfer>();

				var r = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFormatDataRecent();
				if(r?.Count > 0)
				{
					var export = r.Where(x => x.SiteFromName == "VOH");
					if(export?.Count() > 0)
					{
						response.AddRange(export.Select(x => new Models.Lagebewegung.FormatRecentTransfer(x, x.SiteToName ?? x.SiteFromName, Enums.FormatEnums.TransferTypes.Delivery)).ToList());
					}
					var import = r.Where(x => x.SiteToName == "VOH");
					if(import?.Count() > 0)
					{
						response.AddRange(import.Select(x => new Models.Lagebewegung.FormatRecentTransfer(x, x.SiteFromName ?? x.SiteToName, Enums.FormatEnums.TransferTypes.Production)).ToList());
					}
					// - 2024-07-11 - Feneis - ALL transfers invloving L17 are Einfuhr
					for(int i = 0; i < response.Count; i++)
					{
						if(response[i].LagerFrom == 17 || response[i].LagerTo == 17)
						{
							response[i].TransferType = Enums.FormatEnums.TransferTypes.Production;
						}
					}
				}

				// - 2024-06-27 - log data
				Infrastructure.Data.Access.Tables.Logistics.FormatExportLogAccess.Get(response
					?.Select(x => new Tuple<DateTime, int, int>(x.TransferDate, x.LagerFrom, x.LagerTo))
					?.Distinct())
						?.Distinct()?.ToList()
						?.ForEach(l =>
						{
							response
							?.Where(t => t.TransferDate == l.SelectedDate && t.LagerFrom == l.SelectedLagerFrom && t.LagerTo == l.SelectedLagerTo)
							?.ToList()?.ForEach(z =>
							{
								z.LogUserId = l.ExportUserId ?? -1;
								z.LogDate = l.ExportDate ?? DateTime.MinValue;
								z.LogUsername = l.ExportUserName;
							});
						});
				// -
				return ResponseModel<List<Models.Lagebewegung.FormatRecentTransfer>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Lagebewegung.FormatRecentTransfer>> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Lagebewegung.FormatRecentTransfer>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Lagebewegung.FormatRecentTransfer>>.SuccessResponse();
		}
	}
}
