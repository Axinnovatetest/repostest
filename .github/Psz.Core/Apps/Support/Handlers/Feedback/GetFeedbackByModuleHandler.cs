using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Support.Handlers.Feedback
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetFeedbackByModuleHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Feedback.FeedbackGetModel>>>
	{
		private string _module { get; set; }
		private string _type { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetFeedbackByModuleHandler(Identity.Models.UserModel user, string module, string type)
		{
			this._user = user;
			this._module = module;
			this._type = type;
		}

		public ResponseModel<List<Models.Feedback.FeedbackGetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var _feedbackEntitiesByModule = new List<Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity>();
				var feedbackEntities = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get() ?? new List<Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_FeedbacksEntity>();
				var response = new List<Models.Feedback.FeedbackGetModel>();
				if(feedbackEntities != null && feedbackEntities.Count > 0)
					_feedbackEntitiesByModule = feedbackEntities.Where(x => x.Module == _module).ToList();
				if(!string.IsNullOrEmpty(_type) && !string.IsNullOrWhiteSpace(_type))
					_feedbackEntitiesByModule = _feedbackEntitiesByModule?.Where(a => a.FeedbackType == _type).ToList();

				if(_feedbackEntitiesByModule != null && _feedbackEntitiesByModule.Count > 0)
					response = _feedbackEntitiesByModule.Select(x => new Models.Feedback.FeedbackGetModel(x)).ToList();

				return ResponseModel<List<Models.Feedback.FeedbackGetModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Feedback.FeedbackGetModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Feedback.FeedbackGetModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Feedback.FeedbackGetModel>>.SuccessResponse();
		}
	}
}
