using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ManagerUser
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public AddHandler(Identity.Models.UserModel user, int model)
		{
			this._user = user;
			this._data = model;
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

				var userEntitiy = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data);
				if(userEntitiy != null)
				{
					var managerUser = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByUserId(userEntitiy.Id);
					if(managerUser == null)
					{
						var insertedId = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.Insert(
							   new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity
							   {
								   Id = -1,
								   ArtikelNr = -1,
								   UserId = userEntitiy.Id,
								   UserName = userEntitiy.Username,
								   UserFullName = userEntitiy.Name
							   });
						if(insertedId > 0)
						{
							Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
								ObjectLogHelper.getLog(this._user, insertedId, "UserName", "",
								userEntitiy.Username,
								Enums.ObjectLogEnums.Objects.ArticleConfig_ManagerUsers.GetDescription(),
								Enums.ObjectLogEnums.LogType.Add));
						}
					}
					return ResponseModel<int>.SuccessResponse();
				}
				return ResponseModel<int>.SuccessResponse(-1);

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
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data);
			if(userEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>()
				   {
					   new ResponseModel<int>.ResponseError(){ Key = "", Value= "User not found"}
				   }
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
