using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddTechnicianHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Statistics.Technic.TechnicianModel _data { get; set; }
		public AddTechnicianHandler(UserModel user, Models.Article.Statistics.Technic.TechnicianModel data)
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

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, -1, "Technician", "",
						$"{this._data.Name}",
						Enums.ObjectLogEnums.Objects.Technician.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));
				// -
				return ResponseModel<int>.SuccessResponse(
						Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.Insert(this._data.ToEntity()));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(string.IsNullOrWhiteSpace(this._data.Name))
			{
				return ResponseModel<int>.FailureResponse("Name must not be null");
			}

			var sameNameEntities = Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.GetByName(this._data.Name);
			if(sameNameEntities != null && sameNameEntities.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Technician [{this._data.Name}] already exists.");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
