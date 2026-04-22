using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddProjectMessageHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel _data { get; set; }
		public AddProjectMessageHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel data)
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
						ObjectLogHelper.getLog(this._user, -1, "Projektdaten_Details", "",
						$"{this._data.Artikelnummer}",
						Enums.ObjectLogEnums.Objects.ProjectData.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));

				return ResponseModel<int>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.PSZ_Projektdaten_DetailsAccess.Insert(
						this._data.ToEntity()));
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
