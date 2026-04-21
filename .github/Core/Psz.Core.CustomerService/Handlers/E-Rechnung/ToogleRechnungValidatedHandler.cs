using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class ToogleRechnungValidatedHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ToogleRechnungValidatedHandler(Identity.Models.UserModel user, int data)
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

				var rechnung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				var originalValue = rechnung.Gebucht;
				var rechnungCreated = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetByRechnungNr(_data);
				if(rechnungCreated != null && rechnungCreated.CustomerRechnungType == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription())
				{
					var angebotEntites = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByAngebotNr(rechnungCreated.RechnungForfallNr.ToString());
					angebotEntites?.ForEach(x =>
					{
						x.Gebucht = !originalValue;
						x.Benutzer = (originalValue.HasValue && originalValue.Value) ? "" : $"Gebucht, {_user.Name},{DateTime.Now}";
					});
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(angebotEntites);
				}
				rechnung.Gebucht = !originalValue;
				rechnung.Benutzer = (originalValue.HasValue && originalValue.Value) ? "" : $"Gebucht, {_user.Name},{DateTime.Now}";
				var response = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(rechnung);

				var _log = new LogHelper(rechnung.Nr, rechnung.Angebot_Nr ?? -1,
							int.TryParse(rechnung.Projekt_Nr, out var val) ? val : 0, rechnung.Typ, LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user)
							.LogCTS("Validated", rechnung.Gebucht.ToString(), (!rechnung.Gebucht).ToString(), 0);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);


				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var rechnung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(rechnung == null)
				return ResponseModel<int>.FailureResponse("Invoice not found");
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
