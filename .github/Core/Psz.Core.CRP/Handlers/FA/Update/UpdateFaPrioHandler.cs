using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA.Update;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public class UpdateFaPrioHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly UpdateFAPrioRequestModel _data;

		public UpdateFaPrioHandler(Identity.Models.UserModel user, UpdateFAPrioRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;
			try
			{
				var fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(_data.FAId);
				var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdatePrioById(_data.FAId, _data.Prio);
				var log = new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
				{
					DateTime = DateTime.Now,
					LogObject = "Fertigung",
					LogType = Helpers.LogHelper.LogType.MODIFICATIONOBJECT.GetDescription(),
					LogText = $"Prio changed from [{!_data.Prio}] to [{_data.Prio}]",
					UserId = _user.Id,
					Username = _user.Name,
					Nr = fa.Fertigungsnummer

				};
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(log);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			var fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(_data.FAId);
			if(fa.Kennzeichen != Enums.FAEnums.FaStatus.Offen.GetDescription())
				return ResponseModel<int>.FailureResponse($"FA is [{fa.Kennzeichen}] Prio udpate is not permitted .");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}