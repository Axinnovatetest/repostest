using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Support.Handlers.Feedback
{
	public class GetModulesFeedbackCountHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, int>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetModulesFeedbackCountHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<string, int>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var feedbackEntities = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get();
				var response = new List<KeyValuePair<string, int>>();
				if(feedbackEntities != null && feedbackEntities.Count > 0)
				{
					var _modules = feedbackEntities.Select(x => x.Module).Distinct().ToList();
					foreach(var item in _modules)
					{
						response.Add(new KeyValuePair<string, int>(item, feedbackEntities.Count(y => y.Module == item)));
					}
				}

				return ResponseModel<List<KeyValuePair<string, int>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<string, int>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, int>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, int>>>.SuccessResponse();
		}
	}
}
