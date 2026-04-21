using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetArtikelstamm_KlassifizierungHandler: IHandle<GetArtikelstamm_KlassifizierungTrimmedRequestModel, ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>>>
	{
		private GetArtikelstamm_KlassifizierungTrimmedRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetArtikelstamm_KlassifizierungHandler(UserModel user, GetArtikelstamm_KlassifizierungTrimmedRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>> Handle()
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
		private ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>> Perform(UserModel user, GetArtikelstamm_KlassifizierungTrimmedRequestModel data)
		{
			var response = new List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>();

			var fetchedFamilies = Infrastructure.Data.Access.Tables.MTM.Orders.Artikelstamm_KlassifizierungAccess.GetALLTrimmed();

			if(fetchedFamilies.Count() == 0 || fetchedFamilies is null)
				return ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>>.NotFoundResponse();

			foreach(var item in fetchedFamilies)
			{
				response.Add(new GetArtikelstamm_KlassifizierungTrimmedReposneModel(item));
			}

			return ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>>.SuccessResponse(response);
		}
		public ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetArtikelstamm_KlassifizierungTrimmedReposneModel>>.SuccessResponse();
		}
	}
}
