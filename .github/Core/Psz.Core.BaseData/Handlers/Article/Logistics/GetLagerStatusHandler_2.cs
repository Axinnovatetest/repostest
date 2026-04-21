//using Psz.Core.Common.Models;
//using Psz.Core.Identity.Models;
//using Psz.Core.SharedKernel.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Psz.Core.BaseData.Handlers.Article.Logistics
//{
//    public class GetLagerStatusHandler_2 : IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>>
//    {
//        private UserModel _user { get; set; }
//        public int _data { get; set; }
//        public GetLagerStatusHandler_2(UserModel user, int articleId)
//        {
//            this._user = user;
//            this._data = articleId;
//        }
//        public ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel> Handle()
//        {
//            try
//            {
//                var validationResponse = this.Validate();
//                if (!validationResponse.Success)
//                {
//                    return validationResponse;
//                }
//                var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
//                var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrAndBestand(this._data);
//                var lagerortEntiy = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get();
//                var lagerExtensionEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIds(new List<int> { this._data });

//                var results = new List<Models.Article.Logistics.LagerExtensionModel>();
//                if (articleEntity?.Warengruppe?.Trim()?.ToLower() == "ef")
//                {
//                    results = getForFinishGoods(articleEntity, lagerEntities, lagerExtensionEntities);
//                }
//                else
//                {
//                    results = getForRawMaterial(articleEntity, lagerEntities);
//                }
//                var response = new Models.Article.Logistics.LagerStatusGeneralModel(articleEntity, results?.Where(x => x.LagerEntity?.Bestand != 0)?.ToList());
//                if (response.LagerStatus != null)
//                {
//                    foreach (var item in response.LagerStatus)
//                    {
//                        item.LagerName = lagerortEntiy.FirstOrDefault(x => x.LagerortId == item.LagerID)?.Lagerort;
//                    }
//                }
//                return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.SuccessResponse(response);
//            }
//            catch (Exception exception)
//            {
//                Infrastructure.Services.Logging.Logger.Log(exception);
//                throw;
//            }
//        }
//        public ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel> Validate()
//        {
//            if (this._user == null/*
//                || this._user.Access.____*/)
//            {
//                return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.AccessDeniedResponse();
//            }

//            if (Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
//                return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.FailureResponse("Article not found");

//            return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.SuccessResponse();
//        }
//        List<Models.Article.Logistics.LagerExtensionModel>  getForFinishGoods(
//            Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity,
//            List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> lagerEntities,
//            List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> lagerExtensionEntities)
//        {
//            var results = new List<Models.Article.Logistics.LagerExtensionModel>();
//            if (lagerEntities != null)
//            {
//                foreach (var lagerItem in lagerEntities)
//                {

//                    //var y = new Models.Article.Logistics.LagerExtensionModel
//                    //{
//                    //    LagerEntity = lagerItem,
//                    //    KundenIndex = articleEntity.Index_Kunde,
//                    //};
//                    //y.LagerEntity.Bestand = lagerItem.Bestand;
//                    //results.Add(y);

//                    #region // - Multi KI
//                    var ki = lagerExtensionEntities.FindAll(x => x.ArtikelNr == (lagerItem.Artikel_Nr ?? -1) && x.Lagerort_id == (lagerItem.Lagerort_id ?? -1));

//                    // - add Bestand by KI
//                    if (ki != null && ki.Count > 0)
//                    {
//                        foreach (var itemKi in ki)
//                        {
//                            var y = new Models.Article.Logistics.LagerExtensionModel
//                            {
//                                LagerEntity = new Infrastructure.Data.Entities.Tables.PRS.LagerEntity
//                                {
//                                    AB = lagerItem.AB,
//                                    Artikel_Nr = lagerItem.Artikel_Nr,
//                                    Bestand = lagerItem.Bestand,
//                                    Bestand_reserviert = lagerItem.Bestand_reserviert,
//                                    Bestellvorschläge = lagerItem.Bestellvorschläge,
//                                    BW = lagerItem.BW,
//                                    CCID = lagerItem.CCID,
//                                    CCID_Datum = lagerItem.CCID_Datum,
//                                    Dispoformel = lagerItem.Dispoformel,
//                                    Durchschnittspreis = lagerItem.Durchschnittspreis,
//                                    GesamtBestand = lagerItem.GesamtBestand,
//                                    Höchstbestand = lagerItem.Höchstbestand,
//                                    ID = lagerItem.ID,
//                                    Lagerort_id = lagerItem.Lagerort_id,
//                                    letzte_Bewegung = lagerItem.letzte_Bewegung,
//                                    Meldebestand = lagerItem.Meldebestand,
//                                    Mindestbestand = lagerItem.Mindestbestand,
//                                    Sollbestand = lagerItem.Sollbestand,
//                                },
//                                KundenIndex = itemKi.Index_Kunde
//                            };
//                            y.LagerEntity.Bestand = itemKi.Bestand;
//                            results.Add(y);
//                        }
//                    }
//                    else // - KI not in Extesnsion yet, add Best w/ Article.KI
//                    {
//                        var y = new Models.Article.Logistics.LagerExtensionModel
//                        {
//                            LagerEntity = new Infrastructure.Data.Entities.Tables.PRS.LagerEntity
//                            {
//                                AB = lagerItem.AB,
//                                Artikel_Nr = lagerItem.Artikel_Nr,
//                                Bestand = lagerItem.Bestand,
//                                Bestand_reserviert = lagerItem.Bestand_reserviert,
//                                Bestellvorschläge = lagerItem.Bestellvorschläge,
//                                BW = lagerItem.BW,
//                                CCID = lagerItem.CCID,
//                                CCID_Datum = lagerItem.CCID_Datum,
//                                Dispoformel = lagerItem.Dispoformel,
//                                Durchschnittspreis = lagerItem.Durchschnittspreis,
//                                GesamtBestand = lagerItem.GesamtBestand,
//                                Höchstbestand = lagerItem.Höchstbestand,
//                                ID = lagerItem.ID,
//                                Lagerort_id = lagerItem.Lagerort_id,
//                                letzte_Bewegung = lagerItem.letzte_Bewegung,
//                                Meldebestand = lagerItem.Meldebestand,
//                                Mindestbestand = lagerItem.Mindestbestand,
//                                Sollbestand = lagerItem.Sollbestand,
//                            },
//                            KundenIndex = articleEntity.Index_Kunde
//                        };
//                        y.LagerEntity.Bestand = lagerItem.Bestand;
//                        results.Add(y);
//                    }

//                    // - add rest of Bestand w current KI
//                    //var restBestand = (lagerItem.Bestand ?? 0) - (results?.Where(x => x.LagerEntity?.Lagerort_id == lagerItem.Lagerort_id)?.Sum(x => x?.LagerEntity?.Bestand) ?? 0);
//                    //if (restBestand > 0)
//                    //{
//                    //    var currKiIdx = results.FindIndex(x => x.KundenIndex == articleEntity?.Index_Kunde && x.LagerEntity?.Lagerort_id == lagerItem.Lagerort_id);

//                    //    // - if exists, add to Bestand
//                    //    if (currKiIdx >= 0)
//                    //    {
//                    //        results[currKiIdx].LagerEntity.Bestand = (results[currKiIdx].LagerEntity.Bestand ?? 0) + restBestand;
//                    //    }
//                    //    else
//                    //    {
//                    //        // - add new entry
//                    //        var y = new Models.Article.Logistics.LagerExtensionModel
//                    //        {
//                    //            LagerEntity = lagerItem,
//                    //            KundenIndex = articleEntity?.Index_Kunde
//                    //        };
//                    //        y.LagerEntity.Bestand = restBestand;
//                    //        results.Add(y);
//                    //    }
//                    //}
//                    #endregion Multi
//                }
//            }

//            return results;
//        }
//        List<Models.Article.Logistics.LagerExtensionModel> getForRawMaterial(
//            Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity,
//            List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> lagerEntities)
//        {
//            var results = new List<Models.Article.Logistics.LagerExtensionModel>();
//            if (lagerEntities != null)
//            {
//                foreach (var lagerItem in lagerEntities)
//                {
//                    var y = new Models.Article.Logistics.LagerExtensionModel
//                    {
//                        LagerEntity = lagerItem,
//                        KundenIndex = articleEntity.Index_Kunde,
//                    };
//                    y.LagerEntity.Bestand = lagerItem.Bestand;
//                    results.Add(y);
//                }
//            }

//            return results;
//        }
//    }
//}
using Psz.Core.BaseData.Models.Article.Logistics;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class GetLagerStatusHandler_2: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>>
	{
		private UserModel _user { get; set; }
		public LagerRequestModel _data { get; set; }
		public GetLagerStatusHandler_2(UserModel user, LagerRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrAndBestand(this._data.ArticleId, this._data.IncludeMinStock ?? false);
				var lagerortEntiy = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get();
				var lagerExtensionEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIds(new List<int> { this._data.ArticleId });

				var results = new List<Models.Article.Logistics.LagerExtensionModel>();
				if(lagerEntities != null)
				{
					foreach(var lagerItem in lagerEntities)
					{

						var y = new Models.Article.Logistics.LagerExtensionModel
						{
							LagerEntity = lagerItem,
							KundenIndex = articleEntity.Index_Kunde,
						};
						y.LagerEntity.Bestand = lagerItem.Bestand;
						results.Add(y);

						#region // - Multi KI
						//var ki = lagerExtensionEntities.FindAll(x => x.ArtikelNr == (lagerItem.Artikel_Nr ?? -1) && x.Lagerort_id == (lagerItem.Lagerort_id ?? -1));

						//// - add Bestand by KI
						//if (ki != null && ki.Count > 0)
						//{
						//    foreach (var itemKi in ki)
						//    {
						//        var y = new Models.Article.Logistics.LagerExtensionModel
						//        {
						//            LagerEntity = new Infrastructure.Data.Entities.Tables.PRS.LagerEntity
						//            {
						//                AB = lagerItem.AB,
						//                Artikel_Nr = lagerItem.Artikel_Nr,
						//                Bestand = lagerItem.Bestand,
						//                Bestand_reserviert = lagerItem.Bestand_reserviert,
						//                Bestellvorschläge = lagerItem.Bestellvorschläge,
						//                BW = lagerItem.BW,
						//                CCID = lagerItem.CCID,
						//                CCID_Datum = lagerItem.CCID_Datum,
						//                Dispoformel = lagerItem.Dispoformel,
						//                Durchschnittspreis = lagerItem.Durchschnittspreis,
						//                GesamtBestand = lagerItem.GesamtBestand,
						//                Höchstbestand = lagerItem.Höchstbestand,
						//                ID = lagerItem.ID,
						//                Lagerort_id = lagerItem.Lagerort_id,
						//                letzte_Bewegung = lagerItem.letzte_Bewegung,
						//                Meldebestand = lagerItem.Meldebestand,
						//                Mindestbestand = lagerItem.Mindestbestand,
						//                Sollbestand = lagerItem.Sollbestand,
						//            },
						//            KundenIndex = itemKi.Index_Kunde
						//        };
						//        y.LagerEntity.Bestand = itemKi.Bestand;
						//        results.Add(y);
						//    }
						//}
						//else // - KI not in Extesnsion yet, add Best w/ Article.KI
						//{
						//    var y = new Models.Article.Logistics.LagerExtensionModel
						//    {
						//        LagerEntity = new Infrastructure.Data.Entities.Tables.PRS.LagerEntity
						//        {
						//            AB = lagerItem.AB,
						//            Artikel_Nr = lagerItem.Artikel_Nr,
						//            Bestand = lagerItem.Bestand,
						//            Bestand_reserviert = lagerItem.Bestand_reserviert,
						//            Bestellvorschläge = lagerItem.Bestellvorschläge,
						//            BW = lagerItem.BW,
						//            CCID = lagerItem.CCID,
						//            CCID_Datum = lagerItem.CCID_Datum,
						//            Dispoformel = lagerItem.Dispoformel,
						//            Durchschnittspreis = lagerItem.Durchschnittspreis,
						//            GesamtBestand = lagerItem.GesamtBestand,
						//            Höchstbestand = lagerItem.Höchstbestand,
						//            ID = lagerItem.ID,
						//            Lagerort_id = lagerItem.Lagerort_id,
						//            letzte_Bewegung = lagerItem.letzte_Bewegung,
						//            Meldebestand = lagerItem.Meldebestand,
						//            Mindestbestand = lagerItem.Mindestbestand,
						//            Sollbestand = lagerItem.Sollbestand,
						//        },
						//        KundenIndex = articleEntity.Index_Kunde
						//    };
						//    y.LagerEntity.Bestand = lagerItem.Bestand;
						//    results.Add(y);
						//}

						//// - add rest of Bestand w current KI
						////var restBestand = (lagerItem.Bestand ?? 0) - (results?.Where(x => x.LagerEntity?.Lagerort_id == lagerItem.Lagerort_id)?.Sum(x => x?.LagerEntity?.Bestand) ?? 0);
						////if (restBestand > 0)
						////{
						////    var currKiIdx = results.FindIndex(x => x.KundenIndex == articleEntity?.Index_Kunde && x.LagerEntity?.Lagerort_id == lagerItem.Lagerort_id);

						////    // - if exists, add to Bestand
						////    if (currKiIdx >= 0)
						////    {
						////        results[currKiIdx].LagerEntity.Bestand = (results[currKiIdx].LagerEntity.Bestand ?? 0) + restBestand;
						////    }
						////    else
						////    {
						////        // - add new entry
						////        var y = new Models.Article.Logistics.LagerExtensionModel
						////        {
						////            LagerEntity = lagerItem,
						////            KundenIndex = articleEntity?.Index_Kunde
						////        };
						////        y.LagerEntity.Bestand = restBestand;
						////        results.Add(y);
						////    }
						////}
						#endregion Multi
					}
				}

				var response = new Models.Article.Logistics.LagerStatusGeneralModel(articleEntity, this._data.IncludeMinStock == true
						? results?.Where(x => x.LagerEntity?.Bestand != 0 || (x.LagerEntity.Bestand == 0 && x.LagerEntity.Mindestbestand > 0))?.ToList()
						: results?.Where(x => x.LagerEntity?.Bestand != 0)?.ToList());
				if(response.LagerStatus != null)
				{
					foreach(var item in response.LagerStatus)
					{
						item.LagerName = lagerortEntiy.FirstOrDefault(x => x.LagerortId == item.LagerID)?.Lagerort;
					}
				}
				return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.FailureResponse("Article not found");

			return ResponseModel<Models.Article.Logistics.LagerStatusGeneralModel>.SuccessResponse();
		}
	}
}
