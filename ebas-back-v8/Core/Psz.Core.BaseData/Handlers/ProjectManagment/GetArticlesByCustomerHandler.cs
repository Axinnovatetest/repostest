using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetArticlesByCustomer(UserModel user, GetArticlesByCustomerRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			try
			{
				var customerNumbersFromConcerns = GetCuntomerNumbersByConcerns(data.CustomerNumber);
				var kries = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
				if(customerNumbersFromConcerns != null && customerNumbersFromConcerns.Count > 0)
					kries = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByCustomerNumber(customerNumbersFromConcerns);
				else
					kries = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByCustomerNumber(data.CustomerNumber);


				var response = new List<KeyValuePair<int, string>>();
				if(kries != null && kries.Count > 0)
				{
					var numbers = kries.Select(x => x.Nummerschlüssel).ToList();
					foreach(var number in numbers)
					{
						var artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumberPreffix(number);
						response.AddRange(artikel?.Select(x => new KeyValuePair<int, string>(x.ArtikelNr, x.ArtikelNummer)).ToList());
					}
				}
				if(!data.SearchText.IsNullOrEmptyOrWitheSpaces())
					response = response?.Where(x => x.Value.ToLower().Contains(data.SearchText.ToLower())).ToList();
				else
					response = response?.Take(10).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private static List<int> GetCuntomerNumbersByConcerns(int customerNumber)
		{
			var result = new List<int>();
			result.Add(customerNumber);
			var concernCustomerNumber = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumber(customerNumber);
			var concernsItems = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByConcernId(concernCustomerNumber?.ConcernId ?? -1);
			result = concernsItems?.Select(x => x.CustomerNumber ?? -1).ToList();
			return result.DistinctBy(x => x).ToList();
		}
	}
}
