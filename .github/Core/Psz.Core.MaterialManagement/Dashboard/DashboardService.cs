using Psz.Core.MaterialManagement.Interfaces;
using Psz.Core.MaterialManagement.Models;
using System.Linq;

namespace Psz.Core.MaterialManagement.Dashboard
{
	public class DashboardService: IDashboardService
	{
		public ResponseModel<DashboardResponseModel> GetDashboardData(UserModel user, int data)
		{
			try
			{
				var validationResponse = this.Validate(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var emptyResponseBody = new DashboardResponseModel { };
				var openRahmen = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByStatus((int)Enums.BlanketEnums.RAStatus.Validated, data);
				if(openRahmen == null || openRahmen.Count <= 0)
				{
					return ResponseModel<DashboardResponseModel>.SuccessResponse(emptyResponseBody);
				}
				// - filter by user customers
				if(user.Access != null &&
					(user.IsGlobalDirector || user.SuperAdministrator
					|| user.Access.CustomerService?.RahmenClosure == true
					|| user.Access.MasterData?.ArticleSales == true
					|| user.Access.MasterData?.ArticlePurchase == true))
				{
					// - Admin - do nothing
				}
				else
				{
					var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(user.Id)
						.Select(e => e.CustomerNumber)?.Distinct()?.ToList();

					openRahmen = openRahmen.Where(x => customersNumbers.Contains(x.CustomerId ?? -2))?.ToList();
				}

				var raNrs = openRahmen.Select(x => x.AngeboteNr).ToList();
				var openRahmenPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetOpenByRahmen(raNrs);
				if(openRahmenPos == null || openRahmenPos.Count <= 0)
				{
					return ResponseModel<DashboardResponseModel>.SuccessResponse(emptyResponseBody);
				}

				var thresholdDate = DateTime.Today.AddMonths(3);

				// - 2024-05-10
				var openRaPosExtension = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetByRahmenStatus(openRahmenPos.Select(x => x.Nr).ToList(), thresholdDate);
				if(openRaPosExtension?.Count > 0)
				{
					emptyResponseBody.RedPositions = new List<BlanketItem>();
					emptyResponseBody.OrangePositions = new List<BlanketItem>();
					emptyResponseBody.GreenPositions = new List<BlanketItem>();

					var extensionEnities = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Get(openRaPosExtension.Select(x => x.Id).ToList());
					var angeboteneArtikelEnities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(openRaPosExtension.Select(x => x.AngeboteArtikelNr).ToList());
					var angeboteEnities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(openRaPosExtension.Select(x => x.RahmenNr).ToList());
					foreach(var item in openRaPosExtension.Where(x => x.RahmenStatus == 1))
					{
						var ext = extensionEnities?.FirstOrDefault(x => x.Id == item.Id);
						var aEntity = angeboteEnities?.FirstOrDefault(x => x.Nr == item.RahmenNr);
						var aaEntity = angeboteneArtikelEnities?.FirstOrDefault(x => x.Nr == item.AngeboteArtikelNr);
						// -
						emptyResponseBody.RedPositions.Add(new BlanketItem(aEntity, aaEntity, ext));
					}
					foreach(var item in openRaPosExtension.Where(x => x.RahmenStatus == 2))
					{
						var ext = extensionEnities?.FirstOrDefault(x => x.Id == item.Id);
						var aEntity = angeboteEnities?.FirstOrDefault(x => x.Nr == item.RahmenNr);
						var aaEntity = angeboteneArtikelEnities?.FirstOrDefault(x => x.Nr == item.AngeboteArtikelNr);
						// -
						emptyResponseBody.OrangePositions.Add(new BlanketItem(aEntity, aaEntity, ext));
					}
					foreach(var item in openRaPosExtension.Where(x => x.RahmenStatus == 3))
					{
						var ext = extensionEnities?.FirstOrDefault(x => x.Id == item.Id);
						var aEntity = angeboteEnities?.FirstOrDefault(x => x.Nr == item.RahmenNr);
						var aaEntity = angeboteneArtikelEnities?.FirstOrDefault(x => x.Nr == item.AngeboteArtikelNr);
						// -
						emptyResponseBody.GreenPositions.Add(new BlanketItem(aEntity, aaEntity, ext));
					}
				}

				return ResponseModel<DashboardResponseModel>.SuccessResponse(emptyResponseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<DashboardResponseModel> Validate(UserModel user, int data)
		{
			if(user == null/*|| user.Access.____*/)
			{
				return ResponseModel<DashboardResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<DashboardResponseModel>.SuccessResponse();
		}
	}
}
