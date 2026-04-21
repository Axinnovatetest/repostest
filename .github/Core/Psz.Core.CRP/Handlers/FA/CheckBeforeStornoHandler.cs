using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class CheckBeforeStornoHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CheckBeforeStornoHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<string> Handle()
		{
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data);
			if(faEntity == null)
				return ResponseModel<string>.FailureResponse($"FA doesn't exsist");
			var lagers = new List<int> { 42, 60, 7, 6, 21, 26 };
			if(lagers.Contains((int)faEntity.Lagerort_id))
			{
				if(faEntity.FA_Gestartet.HasValue && faEntity.FA_Gestartet.Value)
					return ResponseModel<string>.FailureResponse($"Fertigungsauftrag ist Bereit Gestartet und kann nicht storniert werden.");
			}
			if(faEntity.Angebot_nr.HasValue && faEntity.Angebot_nr.Value != 0)
				return ResponseModel<string>.FailureResponse($"Auftrag muss aus AB storniert werden!");

			if(faEntity.Kennzeichen.ToLower() == "erledigt")
				return ResponseModel<string>.FailureResponse($"Auftrag ist bereits erledigt!");
			if(faEntity.Kennzeichen.ToLower() == "storno")
				return ResponseModel<string>.FailureResponse($"Auftrag ist bereits storniert!");
			//var technicArticles = Module.BSD.TechnicArticleIds;
			var horizonCheck = Helpers.HorizonsHelper.userHasFaCancelHorizonRight(faEntity.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1), _user, out List<string> messages);
			if(!horizonCheck && !Helpers.HorizonsHelper.ArticleIsTechnic(faEntity.Artikel_Nr ?? -1))
				return ResponseModel<string>.FailureResponse(messages);

			return ResponseModel<string>.SuccessResponse("");
		}
		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}
			return ResponseModel<string>.SuccessResponse("");
		}
	}
}