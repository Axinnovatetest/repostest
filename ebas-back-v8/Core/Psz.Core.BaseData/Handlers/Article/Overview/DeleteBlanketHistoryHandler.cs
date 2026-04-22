using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class DeleteBlanketHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteBlanketHistoryHandler(Identity.Models.UserModel user, int data)
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

				// -
				return ResponseModel<int>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.Delete(
						this._data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// -
			if(Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.Get(this._data) == null)
			{
				return ResponseModel<int>.FailureResponse("History not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
