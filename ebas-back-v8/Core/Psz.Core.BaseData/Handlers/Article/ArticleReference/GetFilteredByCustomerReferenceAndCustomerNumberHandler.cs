using Infrastructure.Data.Entities.Tables.BSD;
using Psz.Core.BaseData.Models.Article.ArticleReference;
using Psz.Core.SharedKernel.Interfaces;
using System;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.ArticleReference
{
	public class GetFilteredByCustomerReferenceAndCustomerNumberHandler: IHandle<UserModel, ResponseModel<List<string>>>
	{
		private UserModel _user { get; set; }
		public List<GetLikeCustomerArticleReferenceRequestModel> _data { get; set; }
		public GetFilteredByCustomerReferenceAndCustomerNumberHandler(UserModel user, List<GetLikeCustomerArticleReferenceRequestModel> Filter)
		{
			this._user = user;
			this._data = Filter;
		}
		public ResponseModel<List<string>> Handle()
		{

			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}



			try
			{
				var restoreturn = new List<string>();
				var dataentity = _data.Select(x => new ArtikelCustomerReferencesAndCustomerIDLikeEntity() { CustomerId = x.CustomerId, CustomerReference = x.CustomerReference, supplierName = x.supplierName }).ToList();
				var dbentity = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.GetLikeReferencesAndCustomerIDs(dataentity);


				if(dbentity is null || dbentity.Count == 0)
				{
					foreach(var item in _data)
					{
						restoreturn.Add($"The Manufacturer number {item.CustomerReference} Does not exist on the customer refernece {item.supplierName} !");
					}
					return ResponseModel<List<string>>.SuccessResponse(restoreturn);
				}

				foreach(var item in _data)
				{
					var similar = dbentity.Where(x => (x.CustomerReference == item.CustomerReference)).First();
					if(similar is null || similar.CustomerId <= 0)
					{
						restoreturn.Add($"The Manufacturer number {item.CustomerReference} Does not exist on the customer refernece {item.supplierName} !");
					}
				}

				return ResponseModel<List<string>>.SuccessResponse(restoreturn);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<string>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<string>>.AccessDeniedResponse();
			}
			return ResponseModel<List<string>>.SuccessResponse();
		}

	}
}
