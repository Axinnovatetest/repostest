using System;

namespace Psz.Core.BaseData.Handlers.Article.ArticleClass
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.ArticleClass.ArticleClass _data { get; set; }
		public AddHandler(UserModel user, Models.Article.ArticleClass.ArticleClass articleClass)
		{
			_user = user;
			_data = articleClass;
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
				var insertedId = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Insert(vdEntity);
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Bezeichnung", "",
						vdEntity.Bezeichnung,
						Enums.ObjectLogEnums.Objects.ArticleConfig_Class.GetDescription(),
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

			if(string.IsNullOrWhiteSpace(this._data.Klassifizierung))
				return ResponseModel<int>.FailureResponse($"Article Klassifizierung should have a value");

			var articleClassEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.GetByName(this._data.Klassifizierung);
			if(articleClassEntities != null && articleClassEntities.Count > 0)
				return ResponseModel<int>.FailureResponse($"Article Klassifizierung [{this._data.Klassifizierung}] exists");


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
