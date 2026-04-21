using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteTechnicianHandler: IHandle<int, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteTechnicianHandler(Identity.Models.UserModel user, int id)
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

				// -
				var oldEntity = Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.Get(this._data);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data, "Technician", $"{oldEntity.Name}",
						$"",
						Enums.ObjectLogEnums.Objects.Technician.GetDescription(),
						Enums.ObjectLogEnums.LogType.Delete));

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.Delete(this._data));
			} catch(Exception)
			{

				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.PSZ_TechnikerAccess.Get(this._data) == null)
				return ResponseModel<int>.FailureResponse("Technician not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
