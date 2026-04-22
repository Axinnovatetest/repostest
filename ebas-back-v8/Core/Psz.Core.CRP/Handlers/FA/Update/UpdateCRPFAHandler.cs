using Infrastructure.Services.Utils;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public partial class FAService
	{
		public ResponseModel<List<FACRPUpdateResponseModel>> UpdateCRPFA(UserModel user, FACRPUpdateRequestModel data)
		{
			var validationResponse = ValidateUpdateCRPFA(user, data);
			if(!validationResponse.Success)
				return validationResponse;

			var transaction = new TransactionsManager();
			try
			{
				var response = new List<FACRPUpdateResponseModel>();
				var logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.ArtikelNr);
				var bom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(data.ArtikelNr);
				var bom_extension = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(data.ArtikelNr);
				var cp = Infrastructure.Data.Access.Tables.BSD.CAO_DecoupageAccess.GetLastByArticleAndBom(data.ArtikelNr, bom_extension?.BomVersion ?? -1);
				var faList = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummers(data.FaList);
				var articlesInFAList = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(faList.Select(x => x.Artikel_Nr ?? -1).Distinct().ToList());
				transaction.beginTransaction();
				foreach(var item in data.FaList)
				{
					var fa = faList.FirstOrDefault(x => x.Fertigungsnummer == item);
					var faOldArticle = articlesInFAList.FirstOrDefault(x => x.ArtikelNr == fa.Artikel_Nr);
					Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigungWithTransaction(fa.ID, transaction.connection, transaction.transaction);
					if(bom != null && bom.Count > 0)
					{
						var NewPositionsEntities = bom.Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
						{
							ID_Fertigung_HL = fa.ID,
							ID_Fertigung = fa.ID,
							Artikel_Nr = x.Artikel_Nr_des_Bauteils,
							Anzahl = fa.Anzahl * x.Anzahl,
							Lagerort_ID = fa.Lagerort_id,
							Buchen = true,
							Vorgang_Nr = x.Vorgang_Nr,
							ME_gebucht = false,
						}).ToList();
						Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(NewPositionsEntities, transaction.connection, transaction.transaction);
					}
					var priceAndTime = Helpers.CRPHelper.GetFAPriceAndTime(article.ArtikelNr, fa.Anzahl ?? 0);

					fa.Preis = (fa.Lagerort_id == 6 && fa.Technik.HasValue && fa.Technik.Value) ? 0 : priceAndTime.Key;
					fa.Zeit = priceAndTime.Value;
					fa.Artikel_Nr = data.ArtikelNr;
					fa.Kunden_Index_Datum = article.Index_Kunde_Datum;
					fa.KundenIndex = article?.Index_Kunde;
					fa.CPVersion = cp != null && cp.Validee.HasValue && cp.Validee.Value ? cp.CP_version : null;
					fa.BomVersion = bom_extension != null && bom_extension.BomStatusId > 0 ? bom_extension.BomVersion : null;
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(fa, transaction.connection, transaction.transaction);
					response.Add(new FACRPUpdateResponseModel
					{
						Fertigungsnummer = fa.Fertigungsnummer ?? -1,
						Artikelnummer = article?.ArtikelNummer,
						Produktionstermin = fa.Termin_Bestatigt1,
					});
					logs.Add(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						DateTime = DateTime.Now,
						UserId = user.Id,
						Username = user.Name,
						LogObject = "Fertigung",
						LogType = "MODIFICATIONOBJECT",
						Nr = item,
						AngebotNr = 0,
						Origin = "CRP",
						LogText = $"[Fertigung] Modified- [Artikelnummer] changed from [{faOldArticle.ArtikelNummer}] to [{article.ArtikelNummer}]"
					});
					logs.Add(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						DateTime = DateTime.Now,
						UserId = user.Id,
						Username = user.Name,
						LogObject = "Fertigung",
						LogType = "MODIFICATIONSTUCKLIST",
						Nr = item,
						AngebotNr = 0,
						Origin = "CRP",
						LogText = $"[Fertigung] [{item}] Modified- Stucklist Updated (CRP Update)"
					});
				}
				if(transaction.commit())
				{
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(logs);
					return ResponseModel<List<FACRPUpdateResponseModel>>.SuccessResponse(response);
				}
				else
				{
					transaction.rollback();
					return ResponseModel<List<FACRPUpdateResponseModel>>.FailureResponse("Error in transaction");
				}
			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<FACRPUpdateResponseModel>> ValidateUpdateCRPFA(UserModel user, FACRPUpdateRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<FACRPUpdateResponseModel>>.AccessDeniedResponse();
			if(data.FaList == null || data.FaList.Count == 0)
				return ResponseModel<List<FACRPUpdateResponseModel>>.FailureResponse("No orders provided for CRP Update");
			var faList = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummers(data.FaList);
			var fasFirstSimple = faList.Where(x => Psz.Core.CustomerService.Enums.OrderEnums.ConvertToMTDSalesItemType(x.FertigungType) == Common.Enums.ArticleEnums.SalesItemType.FirstSample).ToList();
			if(fasFirstSimple != null && fasFirstSimple.Count > 0)
				return ResponseModel<List<FACRPUpdateResponseModel>>.FailureResponse($"CRP Update not allowed for First Sample orders [{string.Join(",", fasFirstSimple.Select(f => f.Fertigungsnummer).ToList())}]");

			return ResponseModel<List<FACRPUpdateResponseModel>>.SuccessResponse();
		}
	}
}