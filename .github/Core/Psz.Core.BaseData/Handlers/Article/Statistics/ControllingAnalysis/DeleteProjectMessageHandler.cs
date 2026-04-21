using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteProjectMessageHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteProjectMessageHandler(UserModel user, int data)
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
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data, "Projektdaten_Details", "",
						$"{this._data}",
						Enums.ObjectLogEnums.Objects.ProjectData.GetDescription(),
						Enums.ObjectLogEnums.LogType.Delete));

				return ResponseModel<int>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.PSZ_Projektdaten_DetailsAccess.Delete(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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
