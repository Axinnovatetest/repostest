namespace Psz.Core.Apps.EDI.Handlers.Delfor.Production
{
	//using Psz.Core.Common.Models;
	//using Psz.Core.SharedKernel.Interfaces;
	//public class CancelCommandHandler : IHandle<Identity.Models.UserModel, ResponseModel<int>>
	//{
	//    private Identity.Models.UserModel _user { get; set; }
	//    private Models.Delfor.Production.CancelCommandModel _data { get; set; }

	//    public CancelCommandHandler(Identity.Models.UserModel user, Models.Delfor.Production.CancelCommandModel data)
	//    {
	//        this._user = user;
	//        this._data = data;
	//    }

	//    public ResponseModel<int> Handle()
	//    {
	//        lock (Locks.DLF_ProductionLock)
	//        {
	//            try
	//            {
	//                Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.CancelProduction(this._data.Id);
	//                return ResponseModel<int>.SuccessResponse(this._data.Id);

	//                var validationResponse = this.Validate();
	//                if (!validationResponse.Success)
	//                {
	//                    return validationResponse;
	//                }

	//                var fertigungEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Id);
	//                var angeboteArtikel = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(fertigungEntity.Angebot_Artikel_Nr != null ? fertigungEntity.Angebot_Artikel_Nr.Value : -1);

	//                ////
	//                Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigung(fertigungEntity.ID);
	//                //
	//                fertigungEntity.Kennzeichen = "STORNO";
	//                fertigungEntity.Bemerkung = $"STORNO am {DateTime.Now}, GRUND: [{this._data.Notes}]  / {fertigungEntity.Bemerkung}";  // <<<< Stornogrund
	//                fertigungEntity.Angebot_nr = 0;
	//                fertigungEntity.Angebot_Artikel_Nr = 0;
	//                fertigungEntity.Anzahl = 0;
	//                Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateAfterCancel(fertigungEntity);
	//                //
	//                Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateFertigungsnummer(angeboteArtikel.Nr, 0);

	//                return ResponseModel<int>.SuccessResponse(fertigungEntity.ID);
	//            }
	//            catch (Exception e)
	//            {
	//                Infrastructure.Services.Logging.Logger.Log(e);
	//                throw e;
	//            }
	//        }
	//    }

	//    public ResponseModel<int> Validate()
	//    {
	//        try
	//        {
	//            if (this._user == null/*
	//            || this._user.Access.____*/)
	//            {
	//                return ResponseModel<int>.AccessDeniedResponse();
	//            }

	//            var errors = new List<ResponseModel<int>.ResponseError>();

	//            var fertigungEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Id);
	//            if (fertigungEntity == null)
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Production command not found" });
	//                return new ResponseModel<int>() { Errors = errors };
	//            }
	//            if (fertigungEntity?.FA_Gestartet != null && fertigungEntity?.FA_Gestartet == true)
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Production command is 'Gestartet'" });
	//            }

	//            var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(fertigungEntity.Artikel_Nr != null ? fertigungEntity.Artikel_Nr.Value : -1);
	//            if (artikelEntity == null)
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Article not found" });
	//                return new ResponseModel<int>() { Errors = errors };
	//            }
	//            if (!string.IsNullOrEmpty(artikelEntity?.Freigabestatus) && artikelEntity?.Freigabestatus?.ToUpper() == "O")
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Article is 'Obsolete'" });
	//            }

	//            var angeboteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(fertigungEntity.Angebot_nr != null ? fertigungEntity.Angebot_nr.Value : -1);
	//            if (angeboteEntity == null)
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Order not found" });
	//                return new ResponseModel<int>() { Errors = errors };
	//            }
	//            //if (angeboteEntity?.Typ == null || angeboteEntity?.Typ.ToLower() != Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Confirmation).ToLower()) // "Auftragsbestätigung"
	//            //{
	//            //    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Order not 'Auftragsbestätigung'" });
	//            //}
	//            if (angeboteEntity?.Nr == 0)
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Order 'Nr' is zero." });
	//            }

	//            var angeboteneArtikel = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(fertigungEntity.Angebot_Artikel_Nr != null ? fertigungEntity.Angebot_Artikel_Nr.Value : -1);
	//            if (angeboteneArtikel == null)
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item not found" });
	//                return new ResponseModel<int>() { Errors = errors };
	//            }
	//            if (angeboteneArtikel?.Fertigungsnummer == null || angeboteneArtikel?.Fertigungsnummer.Value == 0)
	//            {
	//                errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item 'Fertigungsnummer' is zero." });
	//            }


	//            // >>>
	//            if (errors.Count > 0)
	//            {
	//                return new ResponseModel<int>() { Errors = errors };
	//            }

	//        }
	//        catch (Exception e)
	//        {
	//            Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, e.StackTrace);
	//            throw;
	//        }
	//        return ResponseModel<int>.SuccessResponse();
	//    }


	//}
}
