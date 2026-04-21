using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class VerifyFABeforeDruckHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public VerifyFABeforeDruckHandler(int data, Identity.Models.UserModel user)
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

				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(faEntity.Artikel_Nr ?? -1);
				List<int> lagers = new List<int> { 42, 60, 7, 6, 21, 26 };
				if(lagers.Contains((int)faEntity.Lagerort_id))
					return ResponseModel<int>.FailureResponse($"FA Druck für TN oder CZ oder AL erfolgt nur aus dem PPS-Software, Bitte IT-Abteilung kontaktieren");
				if(articleEntity.FreigabestatusTNIntern == "N" && !faEntity.Erstmuster.Value && !faEntity.Technik.Value && !articleEntity.ArtikelNummer.ToLower().Contains("reparatur")
					&& !articleEntity.ArtikelNummer.ToLower().Contains("analyse") && !articleEntity.ArtikelNummer.ToLower().Contains("endkontrol") && !articleEntity.ArtikelNummer.ToLower().Contains("umbau"))
					return ResponseModel<int>.FailureResponse($"Status Intern nicht freigegeben für Serien Produktion");
				if(articleEntity.Freigabestatus == "p" ||
					(articleEntity.Freigabestatus == "N" && !faEntity.Erstmuster.Value && !faEntity.Technik.Value && !articleEntity.ArtikelNummer.ToLower().Contains("reparatur")
					&& !articleEntity.ArtikelNummer.ToLower().Contains("analyse") && !articleEntity.ArtikelNummer.ToLower().Contains("endkontrol") && !articleEntity.ArtikelNummer.ToLower().Contains("umbau")))
					return ResponseModel<int>.FailureResponse($"FA-Druck derzeit nicht möglich! Status-extern auf [{articleEntity.Freigabestatus}] gesetzt!");
				if((faEntity.Termin_Bestatigt1 <= DateTime.Now.AddDays(21) && !faEntity.Erstmuster.Value)
					|| (faEntity.Termin_Bestatigt1 <= DateTime.Now.AddDays(28) && faEntity.Erstmuster.Value) && faEntity.Lagerort_id == 26)
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				else if((faEntity.Termin_Bestatigt1 <= DateTime.Now.AddDays(14) && !faEntity.Erstmuster.Value)
					  || (faEntity.Termin_Bestatigt1 <= DateTime.Now.AddDays(28) && faEntity.Erstmuster.Value) && faEntity.Lagerort_id == 15)
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				else
				{
					if(!faEntity.Erstmuster.Value && faEntity.Lagerort_id == 26)
						return ResponseModel<int>.FailureResponse($"Sie haben keine Berechtigung oder Produktionstermin ist nicht innerhalb von 21 Tagen !");
					else if(!faEntity.Erstmuster.Value && faEntity.Lagerort_id == 15)
						return ResponseModel<int>.FailureResponse($"Sie haben keine Berechtigung oder Produktionstermin ist nicht innerhalb von 14 Tagen !");
					else
						return ResponseModel<int>.FailureResponse($"Sie haben keine Berechtigung oder Produktionstermin ist nicht innerhalb von 28 Tagen !");
				}

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
			if(faEntity == null)
			{
				return ResponseModel<int>.FailureResponse($"FA not found");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}