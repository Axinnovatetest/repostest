using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class GetSupplierArticleFilterHandler: IHandle<SupplierArticlesFilterRequestModel, ResponseModel<List<SupplierArticlesFilterResponseModel>>>
	{
		private SupplierArticlesFilterRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetSupplierArticleFilterHandler(UserModel user, SupplierArticlesFilterRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<SupplierArticlesFilterResponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<SupplierArticlesFilterResponseModel>> Perform(UserModel user, SupplierArticlesFilterRequestModel data)
		{

			//var ArticleBestellnummern = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierId(data.SupplierId);
			//var aticles = Infrastructure.Data.Access.Tables.MTM.ArtikelAccess.GetFiltered(ArticleBestellnummern.Select(x => x.Artikel_Nr.HasValue ? x.Artikel_Nr.Value : -1).ToList() , data.ArticleNumber)?.Distinct().ToList();

			var result = Infrastructure.Data.Access.Joins.MTM.Order.ArtikelFilterAccess.GetArtikelFilterBySupplierId(data.ArticleNumber, data.SupplierId);
			//if(result.Count == 0)
			//{
			//	return ResponseModel<List<SupplierArticlesFilterResponseModel>>.FailureResponse("Article Not Found");
			//}

			return ResponseModel<List<SupplierArticlesFilterResponseModel>>.SuccessResponse(result?.Select(x => new SupplierArticlesFilterResponseModel { ArticleNumber = x.Artikelnummer, Description = x.Bezeichnung_1, Id = x.Artikel_Nr }).ToList());
		}

		public ResponseModel<List<SupplierArticlesFilterResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<SupplierArticlesFilterResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<SupplierArticlesFilterResponseModel>>.SuccessResponse();
		}
	}
}
