using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings
{
	public class GetSettingHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.ObjectLog.ObjectLogModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSettingHistoryHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				List<Models.ObjectLog.ObjectLogModel> results = null;
				List<string> LogObjects = new List<string> {
					Enums.ObjectLogEnums.Objects.Condition_assignement.GetDescription(),
					Enums.ObjectLogEnums.Objects.Industry.GetDescription(),
					Enums.ObjectLogEnums.Objects.Supplier_group.GetDescription(),
					Enums.ObjectLogEnums.Objects.Payement_practice.GetDescription(),
					Enums.ObjectLogEnums.Objects.Currency.GetDescription(),
					Enums.ObjectLogEnums.Objects.Slip_circle.GetDescription(),
					Enums.ObjectLogEnums.Objects.Pricing_group.GetDescription(),
					Enums.ObjectLogEnums.Objects.Adress_types.GetDescription(),
					Enums.ObjectLogEnums.Objects.Customer_frames.GetDescription(),
					Enums.ObjectLogEnums.Objects.Shipping_methods.GetDescription(),
					Enums.ObjectLogEnums.Objects.Terms_of_payement.GetDescription(),
					Enums.ObjectLogEnums.Objects.Customer_group.GetDescription(),
					Enums.ObjectLogEnums.Objects.Salutation_contact_person.GetDescription(),
					Enums.ObjectLogEnums.Objects.Address_contact_person.GetDescription(),
					Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
					Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleEmployeeAV.GetDescription(),
					Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleTeams.GetDescription(),
					Enums.ObjectLogEnums.Objects.ArticleConfig_CoCTypes.GetDescription(),
					Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
					Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcernCustomers.GetDescription(),

				};
				var logEntities = Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.GetByObjectsNames(LogObjects);

				if(logEntities != null && logEntities.Count > 0)
				{
					results = new List<Models.ObjectLog.ObjectLogModel>();
					foreach(var logEntity in logEntities)
					{
						results.Add(new Models.ObjectLog.ObjectLogModel(logEntity));
					}
				}

				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse();
		}
	}
}
