using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.InternalStatus
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.Quality.InternalStatusModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Configuration.Quality.InternalStatusModel status)
		{
			_user = user;
			_data = status;
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

				// 
				this._data.CreateTime = DateTime.Now;
				this._data.CreateUserId = this._user.Id;
				var vdEntity = this._data.ToEntity();
				var insertedId = Infrastructure.Data.Access.Tables.BSD.ArtikelInternalStatusAccess.Insert(vdEntity);
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Status", "",
						vdEntity.Status,
						Enums.ObjectLogEnums.Objects.ArticleConfig_InternalStatus.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));
				}
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
			if(this._data.Status.Length > 5)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"status name should not be more then 5 caracters !");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
