using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class EditProjectMessageHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel _data { get; set; }
		public EditProjectMessageHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel data)
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
						ObjectLogHelper.getLog(this._user, this._data.ID, "Projektdaten_Details", "",
						$"{this._data.Artikelnummer}",
						Enums.ObjectLogEnums.Objects.ProjectData.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));

				return ResponseModel<int>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.PSZ_Projektdaten_DetailsAccess.Update(
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

			if(Infrastructure.Data.Access.Tables.BSD.PSZ_Projektdaten_DetailsAccess.Get(this._data.ID) == null)
				return ResponseModel<int>.FailureResponse("Project message not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
