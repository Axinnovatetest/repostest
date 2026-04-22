using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class GetFormatTransferSitesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<FormatTransferSiteResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetFormatTransferSitesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<FormatTransferSiteResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var response = new List<FormatTransferSiteResponseModel>();
				var sites = Infrastructure.Data.Access.Tables.Logistics.WerkeAccess.Get();
				if(sites != null && sites.Count > 0)
				{
					// - companies & countries are of SMALL number so no need to filter. Do it when this no longer the case.
					var companies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
					var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
					// -
					foreach(var item in sites)
					{
						if(item.Country?.ToLower()?.Trim()=="de")
						{
							continue;
						}
						var company = companies?.FirstOrDefault(x => x.Id == item.IdCompany);
						var country = countries?.FirstOrDefault(x => x.Id == company?.CountryId);
						if(company.Closed == true)
						{
							continue;
						}
						response.Add(new FormatTransferSiteResponseModel
						{
							SiteId = item.Id,
							SiteName = item.Name1,
							SiteShortName = item.SiteName,
							CompanyId = company?.Id ?? 0,
							CountryId = country?.Id ?? 0,
							CountryName = country?.Name,
						});
					}
				}
				return ResponseModel<List<FormatTransferSiteResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<FormatTransferSiteResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<FormatTransferSiteResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<FormatTransferSiteResponseModel>>.SuccessResponse();
		}
	}
}
