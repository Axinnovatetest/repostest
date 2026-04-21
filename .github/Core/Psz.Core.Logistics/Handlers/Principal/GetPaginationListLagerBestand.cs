using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Principal
{
	public class GetPaginationListLagerBestand: IHandle<Identity.Models.UserModel, ResponseModel<Models.Principal.LagerBestandResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Principal.LagerBestandSearchModel _data { get; set; }
		public GetPaginationListLagerBestand(Models.Principal.LagerBestandSearchModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<Models.Principal.LagerBestandResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				int allCount = 0;
				var listeArticles = new List<Models.Principal.LagerBestandModel>();

				var BestandListEntity = Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.GetPaginationListeBestand(this._data.Lagerort, this._data.ArtikelNr, this._data.SortFieldKey, this._data.SortDesc, dataPaging);

				foreach(var articleEntity in BestandListEntity)
				{

					listeArticles.Add(new Models.Principal.LagerBestandModel(articleEntity));
				}
				var countB = Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.GetCountListeBestand(this._data.Lagerort, this._data.ArtikelNr);
				if(BestandListEntity.Count() > 0)
				{
					allCount = countB;
				}


				return ResponseModel<Models.Principal.LagerBestandResponseModel>.SuccessResponse(
					new Models.Principal.LagerBestandResponseModel()
					{
						ListArtikel = listeArticles,
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});
				;
				;
				;// ;

			} catch(Exception e)
			{
				throw;
			}
		}
		public ResponseModel<Models.Principal.LagerBestandResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Principal.LagerBestandResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Principal.LagerBestandResponseModel>.SuccessResponse();
		}
	}
}