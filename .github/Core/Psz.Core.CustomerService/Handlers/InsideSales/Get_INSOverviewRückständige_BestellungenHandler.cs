using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<INSOverviewRückständige_BestellungenModelResponseModel> Get_INSOverviewRückständige_Bestellungen(UserModel user, INSOverviewRückständige_BestellungenModelRequestModel data)
		{
			if(user == null)
				return ResponseModel<INSOverviewRückständige_BestellungenModelResponseModel>.AccessDeniedResponse();
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
						case "liefertermin":
							sortFieldName = "CONVERT(date,aa.Liefertermin)";
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
				var entities = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Rückständige_Bestellungen_Table(
					data.Artikelnummer,
					data.Kundennummer,
					data.MitarbeiterId,
					data.Produktionslager,
					userId,
					dataPaging,
					dataSorting);
				var count = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Rückständige_Bestellungen_Table_Count(
					data.Artikelnummer,
					data.Kundennummer,
				data.MitarbeiterId,
					data.Produktionslager,
					userId);

				var response = new INSOverviewRückständige_BestellungenModelResponseModel
				{
					Items = entities?.Select(e => new INSOverviewRückständige_BestellungenModel(e)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<INSOverviewRückständige_BestellungenModelResponseModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
