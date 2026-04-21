using System;

namespace Psz.Core.Apps.Support.Handlers.Feedback
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;


	public class CreateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Feedback.FeedbackGetModel _data { get; set; }

		public CreateHandler(Identity.Models.UserModel user, Models.Feedback.FeedbackGetModel data)
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

				var insertedFeedbackEntity = this._data.ToEntity();
				insertedFeedbackEntity.UserId = _user.Id;
				insertedFeedbackEntity.Username = _user.Name;
				insertedFeedbackEntity.CreationDate = DateTime.Now;
				var insertedId = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Insert(insertedFeedbackEntity);


				return ResponseModel<int>.SuccessResponse(insertedId);
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

			if(this._data == null || Convert.ToString(this._data) == "" || String.IsNullOrWhiteSpace(Convert.ToString(this._data)) || String.IsNullOrEmpty(Convert.ToString(this._data)))
				return ResponseModel<int>.FailureResponse("Empty feedback !");
			//($"Invalid value [{this._data.Name}] for company name")

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
