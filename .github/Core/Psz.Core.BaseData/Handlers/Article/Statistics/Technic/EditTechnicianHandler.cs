using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class EditTechnicianHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Statistics.Technic.TechnicianModel _data { get; set; }
		public EditTechnicianHandler(UserModel user, Models.Article.Statistics.Technic.TechnicianModel data)
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

				var oldEntity = Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.Get(this._data.Id);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.Id, "Technician", $"{oldEntity.Name}",
						$"{this._data.Name}",
						Enums.ObjectLogEnums.Objects.Technician.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
				// -
				return ResponseModel<int>.SuccessResponse(
						Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.Update(this._data.ToEntity()));
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

			if(Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.Get(this._data.Id) == null)
			{
				return ResponseModel<int>.FailureResponse("Technician not found");
			}

			if(string.IsNullOrWhiteSpace(this._data.Name))
			{
				return ResponseModel<int>.FailureResponse("Name must not be null");
			}

			var sameNameEntities = Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.GetByName(this._data.Name)
				?.Where(x => x.ID != this._data.Id)?.ToList();
			if(sameNameEntities != null && sameNameEntities.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Technician [{this._data.Name}] already exists.");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
