using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.PricingGroup
{
	public class DeletePriceGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeletePriceGroupHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var priceGroupEntity = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Get(this._data);
				var logType = Enums.ObjectLogEnums.LogType.Delete;
				var log = ObjectLogHelper.getLog(this._user, this._data, "Pricing group", priceGroupEntity.Preisgruppe.ToString(), null, Enums.ObjectLogEnums.Objects.Pricing_group.GetDescription(), logType);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Delete(this._data);
				return ResponseModel<int>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var priceGroupEntity = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Get(this._data);
			if(priceGroupEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Pricing Group not found" }
						}
				};
			}
			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByPricingGroup(priceGroupEntity.Preisgruppe ?? -1);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return ResponseModel<int>.FailureResponse(
					$"Cannot delete Pricing Group. The following customer(s) use this Pricing [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Kundennummer} - {x.Name1}")?.ToList())}]");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
