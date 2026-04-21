using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Interfaces
{
	public interface IInsideSalesOveview
	{
		ResponseModel<INSOverviewRückständige_BestellungenModelResponseModel> Get_INSOverviewRückständige_Bestellungen(UserModel user, INSOverviewRückständige_BestellungenModelRequestModel data);
		ResponseModel<INSOverviewVK_Summe_ungebuchte_ABResponseModel> Get_INSOverviewVK_Summe_ungebuchte_AB(UserModel user, INSOverviewVK_Summe_ungebuchte_ABRequestModel data);
		ResponseModel<INSOverviewMindesbestand_AuswertungResponseModel> Get_INSOverviewMindesbestand_Auswertung(UserModel user, INSOverviewMindesbestand_AuswertungRequestModel data);
		ResponseModel<ChartsAndOlderDatesModel> Get_INSOverviewRückständige_BestellungenChart(UserModel user, Ruckstandige_BestellungenRequestModel data);
		ResponseModel<List<DateValueOrderModel>> Get_INSOverviewUmsatz_Aktuelle_WocheChart(UserModel user);
		ResponseModel<ChartsAndOlderDatesModel> Get_INSOverviewVK_Summe_Ungebuchte_ABsChart(UserModel user, Ruckstandige_BestellungenRequestModel data);
		ResponseModel<List<LabelValueModel>> Get_INSOverviewMindesbestand_AuswertungChart(UserModel user, Mindesbestand_AuswertungRequestModel data);
		ResponseModel<List<KeyValuePair<int, string>>> GetUsersForOverview(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetCustomersForOverview(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetWarehousesForOverview(UserModel user);
		ResponseModel<INSOverviewRückständige_BestellungenDetailsResponseModel> Get_INSOverviewRückständige_BestellungenDetails(UserModel user, INSOverviewRückständige_BestellungenDetailsRequestModel data);
		ResponseModel<INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel> Get_INSOverviewUmsatz_Aktuelle_WocheDetails(UserModel user, INSOverviewUmsatz_Aktuelle_WocheDetailsRequestModel data);
		ResponseModel<INSOverviewVK_Summe_ungebuchte_ABDetailsResponseModel> Get_INSOverviewVK_Summe_ungebuchte_ABDetails(UserModel user, INSOverviewVK_Summe_ungebuchte_ABDetailsRequestModel data);
		ResponseModel<List<KeyValuePair<string, string>>> GetWeeksForChartFilter(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetArticlesForChartsFilter(UserModel user, string text);
	}
}