using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class GetSupplierArticlesHandler: IHandle<SupplierArticleRequestModel, ResponseModel<List<SupplierArticleResponseModel>>>
	{

		private SupplierArticleRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetSupplierArticlesHandler(UserModel user, SupplierArticleRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<SupplierArticleResponseModel>> Handle()
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
		public ResponseModel<List<SupplierArticleResponseModel>> Perform(UserModel user, SupplierArticleRequestModel data)
		{
			var ArticleBestellnummern = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierId(data.SuppplierId);
			var aticles = Infrastructure.Data.Access.Tables.MTM.ArtikelAccess.Get(ArticleBestellnummern.Select(x => x.Artikel_Nr.HasValue ? x.Artikel_Nr.Value : -1).ToList())?.Where(x => x.aktiv == true).Distinct().ToList();

			return ResponseModel<List<SupplierArticleResponseModel>>.SuccessResponse(aticles.Select(x => new SupplierArticleResponseModel { ArticleNumber = x.Artikelnummer, Description = x.Bezeichnung_1, Id = x.Artikel_Nr }).ToList());
		}


		public ResponseModel<List<SupplierArticleResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<SupplierArticleResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<SupplierArticleResponseModel>>.SuccessResponse();
		}
	}
}
