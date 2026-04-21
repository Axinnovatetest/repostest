using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Apps.Support.Handlers.Feedback
{
	public class UpdateFeedbackTreatedHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public UpdateFeedbackTreatedHandler(Identity.Models.UserModel user, int data)
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

				var feedBackEntity = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get(_data);
				feedBackEntity.Treated = feedBackEntity.Treated.HasValue ? !feedBackEntity.Treated.Value : true;
				feedBackEntity.TreatedUser = _user.Name;
				feedBackEntity.TreatedDate = DateTime.Now;
				var resposne = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Update(feedBackEntity);

				return ResponseModel<int>.SuccessResponse(resposne);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}"); // - 1 - Input Data
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace); // - 2 - Call Log
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
			var feedBackEntity = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get(_data);
			if(feedBackEntity == null)
				return ResponseModel<int>.FailureResponse("feedback not found !");
			//($"Invalid value [{this._data.Name}] for company name")

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
