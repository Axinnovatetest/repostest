using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetAvailableProductionPlacesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.CountryFullResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.IndexSiblingsRequestModel _data { get; set; }

		public GetAvailableProductionPlacesHandler(Identity.Models.UserModel user, Models.Article.IndexSiblingsRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Article.CountryFullResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetIndexSiblings(this._data.CustomerNumber, this._data.CustomerItemNumber, this._data.CustomerItemIndex, this._data.IsSpecialNumber)
					 ?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
				var sites = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
				if(articleEntities != null && articleEntities.Count > 0)
				{
					sites = sites?.Where(x => !articleEntities.Exists(y => y.ProductionSiteSequence == x.LagerortId))?.ToList();
					countries = countries?.Where(x => sites.Exists(y => y.CountryId == x.Id))?.ToList();
				}

				// -
				return ResponseModel<List<Models.Article.CountryFullResponseModel>>.SuccessResponse(
					countries.Select(x => new Models.Article.CountryFullResponseModel(x, sites?.FindAll(y => y.CountryId == x.Id))).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.CountryFullResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.CountryFullResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.CountryFullResponseModel>>.SuccessResponse();
		}
	}

}
