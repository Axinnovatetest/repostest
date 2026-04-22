using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;

namespace Psz.Core.Apps.Support.Handlers.Feedback
{
	public class GetFeedbacksModulesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetFeedbacksModulesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			var modules = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("cts", "Customer Service"),
				new KeyValuePair<string, string>("mtd", "Master Data"),
				new KeyValuePair<string, string>("sld", "Sales & Distribution"),
				new KeyValuePair<string, string>("crp", "Capacity Requirement Planning"),
				new KeyValuePair<string, string>("lgt", "Logistics"),
				new KeyValuePair<string, string>("mgo", "Management Overview"),
				new KeyValuePair<string, string>("pur", "Purchase"),
				new KeyValuePair<string, string>("fnc", "Finance & Control"),
				new KeyValuePair<string, string>("mtm", "Materials Management")

			};
			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(modules);
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
	}
}
