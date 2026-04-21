using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview: IInsideSalesOveview
	{
		public ResponseModel<List<LabelValueModel>> Get_INSOverviewMindesbestand_AuswertungChart(UserModel user, Mindesbestand_AuswertungRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<LabelValueModel>>.AccessDeniedResponse();

			int? userId = user.IsAdministrator || user.IsCorporateDirector || user.IsGlobalDirector || user.Access.Purchase.AllCustomers
			? null
			: user.Id;
			var Mindesbestand_Auswertung_Plus = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Mindesbestand_Auswertung(data.CustomerNumber, data.MitarbeiterId, data.Artikelnummer, userId, true);
			var Mindesbestand_Auswertung_Minus = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Mindesbestand_Auswertung(data.CustomerNumber, data.MitarbeiterId, data.Artikelnummer, userId);

			var response = new List<LabelValueModel>
			{
				new LabelValueModel
			{
				Label = "Unterdeckungssumme",
				Value = Mindesbestand_Auswertung_Minus
			},
				new LabelValueModel
			{
				Label = "Überdeckungssumme",
				Value = Mindesbestand_Auswertung_Plus
			}
			};

			return ResponseModel<List<LabelValueModel>>.SuccessResponse(response);
		}
	}
}