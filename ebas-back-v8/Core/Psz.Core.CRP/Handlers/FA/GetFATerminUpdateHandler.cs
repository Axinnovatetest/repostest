using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFATerminUpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<FAUpdateTerminModel>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFATerminUpdateHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<FAUpdateTerminModel> Handle()
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
				var angebotArticleEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByFertigungsnummer(this._data);
				var angebotEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(angebotArticleEntity?.AngebotNr ?? -1);
				var CSKontackEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetCSKontakt(this._data);

				var response = new FAUpdateTerminModel(faEntity, articleEntity, angebotEntity, angebotArticleEntity, CSKontackEntity?.Item3);
				return ResponseModel<FAUpdateTerminModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<FAUpdateTerminModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<FAUpdateTerminModel>.AccessDeniedResponse();
			}
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data);
			if(faEntity == null)
				return ResponseModel<FAUpdateTerminModel>.FailureResponse(key: "1", value: $"FA not found");

			// - 2022-05-30 - ignore checks for Admin users
			if(this._user.Access?.CustomerService?.FAWerkWunshAdmin != true && this._user.Access?.CustomerService?.FaAdmin != true)
			{
				if(faEntity.FA_Gestartet.HasValue && faEntity.FA_Gestartet.Value /*&& faEntity.Lagerort_id!=15*/)
					return ResponseModel<FAUpdateTerminModel>.FailureResponse(key: "1", value: $"Termin kann nicht verschoben werden FA ist bereits gestartet!");

				if(faEntity.Check_FAbegonnen.HasValue && faEntity.Check_FAbegonnen.Value)
					return ResponseModel<FAUpdateTerminModel>.FailureResponse(key: "1", value: $"FA bereits gestartet! Keine Terminverschiebung mehr möglich!");

				if(faEntity.Kennzeichen.ToLower() == "storno" || faEntity.Kennzeichen.ToLower() == "erledigt")
					return ResponseModel<FAUpdateTerminModel>.FailureResponse(key: "1", value: $"date change not possible fa status is {faEntity.Kennzeichen}");
			}

			return ResponseModel<FAUpdateTerminModel>.SuccessResponse();
		}
	}
}