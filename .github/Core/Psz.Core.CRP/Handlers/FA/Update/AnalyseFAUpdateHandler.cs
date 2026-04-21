using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public class AnalyseFAUpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FAAnalyseShneidereiKabelGeschnittenDataModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AnalyseFAUpdateHandler(FAAnalyseShneidereiKabelGeschnittenDataModel data, Identity.Models.UserModel user)
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
				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer);
				if(this._data.FA_Begonnen_isDisabled && this._data.FA_Begonnen_check)
				{
					if(this._data.FA_Begonnen_check)
						faEntity.FA_begonnen = DateTime.Now;
					else
						faEntity.FA_begonnen = null;
				}
				if(!this._data.Gewerk_1_isDisabled)
				{
					if(this._data.Gewerk_1_check)
						faEntity.Gewerk_1 = "True";
					else
						faEntity.Gewerk_1 = "False";
				}

				if(!this._data.Gewerk_2_isDisabled)
					faEntity.Gewerk_2 = "True";
				if(!this._data.Gewerk_2_isDisabled && !this._data.Gewerk_2_check)
					faEntity.Gewerk_2 = "False";

				if(!this._data.Gewerk_3_isDisabled)
					faEntity.Gewerk_3 = "True";
				if(!this._data.Gewerk_3_isDisabled && !this._data.Gewerk_3_check)
					faEntity.Gewerk_3 = "False";


				if(!this._data.Kabel_geschnitten_isDisabled && this._data.Kabel_geschnitten_check)
				{
					faEntity.Kabel_geschnitten = this._data.Kabel_geschnitten_check;
					faEntity.Kabel_geschnitten_Datum = DateTime.Now;
				}

				faEntity.Gewerk_Teilweise_Bemerkung = this._data.Bemerkung;
				faEntity.Check_Gewerk1 = this._data.Gewerk_1_check;
				faEntity.Check_Gewerk2 = this._data.Gewerk_2_check;
				faEntity.Check_Gewerk3 = this._data.Gewerk_3_check;
				//
				faEntity.Check_Gewerk1_Teilweise = this._data.Gewerk_1T_check;
				faEntity.Check_Gewerk2_Teilweise = this._data.Gewerk_2T_check;
				faEntity.Check_Gewerk3_Teilweise = this._data.Gewerk_3T_check;
				//
				faEntity.Check_FAbegonnen = this._data.FA_Begonnen_check;
				faEntity.Check_Kabelgeschnitten = this._data.Kabel_geschnitten_check;
				var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(faEntity);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
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

			var fa = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAListShneidereiKabelGeschnittenExact(this._data.Lager ?? 0, this._data.Fertigungsnummer.ToString());
			if(fa == null || fa.Count <= 0)
				return ResponseModel<int>.FailureResponse("Fa not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}