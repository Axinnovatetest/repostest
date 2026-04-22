using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public partial class FAService
	{
		public ResponseModel<List<FACRPNotRequiredROHResponseModel>> GetNotRequiredROH(UserModel user, FACRPNotRequiredROHRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<FACRPNotRequiredROHResponseModel>>.AccessDeniedResponse();

			try
			{
				var response = new List<FACRPNotRequiredROHResponseModel>();
				var allOrders = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(data.FaList);
				foreach(var fa in data.FaList)
				{
					var faEntity = allOrders.FirstOrDefault(f => f.Fertigungsnummer == fa);
					var newBom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(data.ArtikelNr);
					var oldBom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(faEntity.Artikel_Nr ?? -1);

					var newBomArticles = newBom.Select(x => x.Artikel_Nr_des_Bauteils ?? -1).ToList();
					var oldBomArticles = oldBom.Select(x => x.Artikel_Nr_des_Bauteils ?? -1).ToList();

					var AllArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(oldBomArticles.Union(newBomArticles).ToList());

					var notRequiredROHArticles = oldBomArticles.Except(newBomArticles).ToList();
					if(notRequiredROHArticles != null && notRequiredROHArticles.Count > 0)
					{
						foreach(var article in notRequiredROHArticles)
						{
							var needed = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetROHUpcomingNeededQtyForCRPUpdate(article, faEntity.ID);
							var neededInOldBomLine = oldBom.FirstOrDefault(x => x.Artikel_Nr_des_Bauteils == article);
							var neededInOldBomQty = neededInOldBomLine?.Anzahl ?? 0;

							var difference = (decimal)neededInOldBomQty - needed;
							if(difference > 0)
							{
								response.Add(new FACRPNotRequiredROHResponseModel
								{
									ArtikelNr = article,
									Artikelnummer = AllArticles?.FirstOrDefault(x => x.ArtikelNr == article)?.ArtikelNummer,
									NeededInOtherOrders = needed,
									Diffrence = difference,
									NeededInOrderToUpdate = (decimal)neededInOldBomQty
								});
							}
						}
					}
				}
				response = response?.DistinctBy(r => r.ArtikelNr).ToList();
				return ResponseModel<List<FACRPNotRequiredROHResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}