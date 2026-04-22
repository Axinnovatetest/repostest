using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System.Linq;
using System;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<INSOverviewMindesbestand_AuswertungResponseModel> Get_INSOverviewMindesbestand_Auswertung(UserModel user, INSOverviewMindesbestand_AuswertungRequestModel data)
		{
			if(data == null)
				return ResponseModel<INSOverviewMindesbestand_AuswertungResponseModel>.AccessDeniedResponse();

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
						case "artikelnummer":
							sortFieldName = "ar.[Artikelnummer]";
							break;
						case "kunde":
							sortFieldName = "ad.[Name1]";
							break;
						case "verkaufspreis":
							sortFieldName = "pr.[Verkaufspreis]";
							break;
						case "produktionslager":
							sortFieldName = "b.[ProductionPlace1_Id]";
							break;
						case "mitarbeiter":
							sortFieldName = "u.[Name]";
							break;
						case "mindestbestand":
							sortFieldName = "[Bestand]";
							break;
						case "differenz":
							sortFieldName = "[Differenz]";
							break;
						case "differenzwert":
							sortFieldName = "[Differenzwert]";
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
				var entities = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Mindesbestand_Auswertung_Table(
					data.Artikelnummer,
					data.Kundennummer,
					data.MitarbeiterId,
					data.Produktionslager,
					userId,
					data.Type,
					dataPaging,
					dataSorting);
				var count = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Mindesbestand_Auswertung_Table_Count(
					data.Artikelnummer,
					data.Kundennummer,
				data.MitarbeiterId,
					data.Produktionslager,
					userId,
					data.Type);

				var response = new INSOverviewMindesbestand_AuswertungResponseModel
				{
					Items = entities?.Select(e => new INSOverviewMindesbestand_AuswertungModel(e)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<INSOverviewMindesbestand_AuswertungResponseModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}