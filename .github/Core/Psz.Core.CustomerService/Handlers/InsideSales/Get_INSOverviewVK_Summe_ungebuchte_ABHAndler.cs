using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System.Linq;
using System;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<INSOverviewVK_Summe_ungebuchte_ABResponseModel> Get_INSOverviewVK_Summe_ungebuchte_AB(UserModel user, INSOverviewVK_Summe_ungebuchte_ABRequestModel data)
		{
			if(data == null)
				return ResponseModel<INSOverviewVK_Summe_ungebuchte_ABResponseModel>.AccessDeniedResponse();

			try
			{
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "angebotnr":
							sortFieldName = "aa.[Angebot-Nr]";
							break;
						case "artikelnummer":
							sortFieldName = "ar.[Artikelnummer]";
							break;
						case "kunde":
							sortFieldName = "ad.[Name1]";
							break;
						case "produktionslager":
							sortFieldName = "b.[ProductionPlace1_Id]";
							break;
						case "mitarbeiter":
							sortFieldName = "u.[Name]";
							break;
						case "gesamtpreis":
							sortFieldName = "aa.[Gesamtpreis]";
							break;
						case "verkaufspreis":
							sortFieldName = "pg.[Verkaufspreis]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				int? userId = user.IsAdministrator || user.IsCorporateDirector || user.IsGlobalDirector || user.Access.Purchase.AllCustomers
							? null
							: user.Id;
				var entities = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_VK_Summe_ungebuchte_AB_Table(
					data.Artikelnummer,
					data.Kundennummer,
					data.MitarbeiterId,
					data.Produktionslager,
					userId,
					dataPaging,
					dataSorting);
				var count = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_VK_Summe_ungebuchte_AB_Table_Count(
					data.Artikelnummer,
					data.Kundennummer,
				data.MitarbeiterId,
					data.Produktionslager,
					userId);

				var response = new INSOverviewVK_Summe_ungebuchte_ABResponseModel
				{
					Items = entities?.Select(e => new INSOverviewVK_Summe_ungebuchte_ABModel(e)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<INSOverviewVK_Summe_ungebuchte_ABResponseModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}