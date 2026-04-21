using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.ArticleContactTechnic
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public DeleteHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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
				var entity = Infrastructure.Data.Access.Tables.BSD.ArticleContactTechnicAccess.Get(this._data);
				var deletedId = Infrastructure.Data.Access.Tables.BSD.ArticleContactTechnicAccess.Delete(this._data);
				if(deletedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, entity.Id, "Name",
						entity.Name, "",
						Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleContactTechnic.GetDescription(),
						Enums.ObjectLogEnums.LogType.Delete));
				}
				return ResponseModel<int>.SuccessResponse(deletedId);
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

			var entity = Infrastructure.Data.Access.Tables.BSD.ArticleContactTechnicAccess.Get(this._data);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Contact not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
