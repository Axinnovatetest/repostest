using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Industry
{
	public class GetCustomerIndustriesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomerIndustriesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var industryEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.GetByType(1)
					?? new List<Infrastructure.Data.Entities.Tables.BSD.IndustryEntity>();

				// - add missing Industry values from Kunden table
				if(refreshIndustry(this._user, industryEntities, Enums.AddressEnums.Industries.Customer))
				{
					industryEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.GetByType((int)Enums.AddressEnums.Industries.Customer);
				}

				// - 
				var S = industryEntities.OrderBy(t => t.Name != null)
					.ThenByDescending(t => t.Name).ToArray();
				var response = new List<KeyValuePair<string, string>>();

				foreach(var industryEntity in S)
				{
					response.Add(new KeyValuePair<string, string>(industryEntity.Name, $"{industryEntity.Id}||{industryEntity.Name}"));
				}

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
		public static bool refreshIndustry(Identity.Models.UserModel user,
			List<Infrastructure.Data.Entities.Tables.BSD.IndustryEntity> industryEntities,
			Enums.AddressEnums.Industries type)
		{
			var newIndustries = new List<Infrastructure.Data.Entities.Tables.BSD.IndustryEntity>();
			List<string> industriesFromCustomers =
				(type == Enums.AddressEnums.Industries.Customer
					? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetBranches()
					: Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetBranches())
				?? new List<string>();
			foreach(var item in industriesFromCustomers)
			{
				if(!industryEntities.Exists(x => x.Name.Trim().ToLower() == item.Trim().ToLower()
				&& x.Type == (int)type))
				{
					newIndustries.Add(new Infrastructure.Data.Entities.Tables.BSD.IndustryEntity
					{
						CreationTime = DateTime.Now,
						CreationUserId = user.Id,
						Description = "",
						Id = -1,
						LastUpdateTime = null,
						LastUpdateUserId = null,
						Name = item,
						Type = (int)type
					});
				}
			}

			// - 
			if(newIndustries.Count > 0)
			{
				Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Insert(newIndustries);
				return true;
			}

			// - 
			return false;
		}
	}
}
