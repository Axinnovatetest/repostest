using System;

namespace Psz.Core.BaseData.Handlers.Article.Packaging
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Packaging.Packaging _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Packaging.Packaging packaging)
		{
			_user = user;
			_data = packaging;
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
				var vdEntity = this._data.ToEntity();
				var insertedId = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Insert(vdEntity);
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "MasseLxBxH (in mm)", "",
						vdEntity.Masse_LxBxH__in_mm_,
						Enums.ObjectLogEnums.Objects.ArticleConfig_Packaging.GetDescription(),
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

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
