namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetBlanketByIdHandler //: IHandle<Identity.Models.UserModel, ResponseModel<BlanketModel>>
	{
		//private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }

		//public GetBlanketByIdHandler(Identity.Models.UserModel user,int id)
		//{
		//    this._user = user;
		//    this._data = id;         
		//}
		//public ResponseModel<BlanketModel> Handle()
		//{
		//    try
		//    {
		//        var validationResponse = this.Validate();
		//        if (!validationResponse.Success)
		//        {
		//            return validationResponse;
		//        }

		//        var response = new BlanketModel();


		//        var blanketEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
		//        var BlanketFileEntity = Infrastructure.Data.Access.Tables.CTS.CTSBlanketFilesAccess.GetByAngebotNr(this._data);
		//        var blanketExtensionEntity = Infrastructure.Data.Access.Tables.CTS.__CTS_AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data);

		//        // - add extension if not exist
		//        if (blanketExtensionEntity == null)
		//        {
		//            Infrastructure.Data.Access.Tables.CTS.__CTS_AngeboteBlanketExtensionAccess.Insert(
		//                new Infrastructure.Data.Entities.Tables.CTS.__CTS_AngeboteBlanketExtensionEntity
		//                {
		//                    AngeboteNr = this._data,
		//                    Anhage = null,
		//                    Archived = null,
		//                    ArchiveTime = null,
		//                    ArchiveUserId = null,
		//                    Auftraggeber = null,
		//                    CreateTime = DateTime.Now,
		//                    CreateUserId = this._user.Id,
		//                    Id = this._data,
		//                    LastEditTime = null,
		//                    LastEditUserId = null,
		//                    Warenemfanger = null
		//                });
		//            blanketExtensionEntity = Infrastructure.Data.Access.Tables.CTS.__CTS_AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data);
		//        }


		//        // - get articles
		//        var BlanketDetailsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data);
		//        var BlanketDetailsExtensionEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngebotNr(BlanketDetailsEntities?.Select(x=> x.Nr)?.ToList());


		//        // - for Old Add extensions
		//        var newExtensions = new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
		//        var newExtensionsArticleIds = new List<int>();
		//        // - add missing items
		//        if (BlanketDetailsEntities != null && BlanketDetailsEntities.Count > 0)
		//        {
		//            foreach (var item in BlanketDetailsEntities)
		//            {
		//                var extension = BlanketDetailsExtensionEntities.Find(x => x.AngeboteArtikelNr == item.Nr);
		//                if (extension == null)
		//                {
		//                    newExtensions.Add(new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity
		//                    {
		//                        AngeboteArtikelNr = item.Nr,
		//                        Bezeichnung = item.Bezeichnung1,
		//                        Gesamtpreis = item.Gesamtpreis,
		//                        GultigAb = null,
		//                        GultigBis = null,
		//                        Id = -1,
		//                        KundenMatNummer = null,
		//                        Material = null,
		//                        ME = null,
		//                        Preis = null, // item.VKEinzelpreis // item.Einzelpreis // depending on ontract Direction
		//                        Wahrung = null,
		//                        Zielmenge = item.OriginalAnzahl //FIXME: to check  mit Khelil
		//                    });
		//                    newExtensionsArticleIds.Add(item.Nr);
		//                }
		//            }
		//            // save new extensions
		//            Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Insert(newExtensions);
		//        }


		//        // -
		//        response = new BlanketModel(blanketEntity, blanketExtensionEntity);
		//        response.BlanketDetails = new List<BlanketItem> { };
		//        BlanketDetailsExtensionEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngebotNr(BlanketDetailsEntities?.Select(x => x.Nr)?.ToList());
		//        // - 
		//        foreach (var item in BlanketDetailsEntities)
		//        {
		//            var extension = BlanketDetailsExtensionEntities.Find(x => x.AngeboteArtikelNr == item.Nr);
		//            response.BlanketDetails.Add(new BlanketItem(item, extension));
		//        }
		//        //if (BlanketFileEntity != null && BlanketFileEntity.Count > 0)
		//        //    response.FileIds = BlanketFileEntity.Select(x => x.Id).ToList();

		//        return ResponseModel<BlanketModel>.SuccessResponse(response);
		//    }

		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
		//        throw e;
		//    }
		//}
		//public ResponseModel<BlanketModel> Validate()
		//{
		//    if (this._user == null/*
		//        || this._user.Access.____*/)
		//    {
		//        return ResponseModel<BlanketModel>.AccessDeniedResponse();
		//    }

		//    var blanketEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
		//    //var x = blanketEntity.Typ.Trim().ToLower();
		//    //var y = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT.Trim().ToLower();
		//    if (blanketEntity == null || (blanketEntity != null && blanketEntity.Typ.Trim().ToLower() != 
		//        Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT.Trim().ToLower()))
		//        return ResponseModel<BlanketModel>.FailureResponse("Order not found");

		//    // -
		//    return ResponseModel<BlanketModel>.SuccessResponse();
		//}
	}
}
