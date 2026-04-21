using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class ConvertArticleRahmenHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ConvertedRahmensModel>>>
	{
		private RahmensToConvertModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ConvertArticleRahmenHandler(Identity.Models.UserModel user, RahmensToConvertModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<ConvertedRahmensModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<ConvertedRahmensModel>();
				var check = this.CheckDataErrors();

				if(check.Errors.Count > 0)
				{
					response.Add(new ConvertedRahmensModel
					{
						ArtikelNummer = _data.Artikelnummer,
						Errors = check.Errors,
						RahmenNr = null,
						Supplier = "",
						Artikel_Nr = _data.Artikel_Nr ?? -1,
						RahmenAngebotNr = null
					});
					return ResponseModel<List<ConvertedRahmensModel>>.SuccessResponse(response);
				}

				if(_data.Rahmen.HasValue && _data.Rahmen.Value && !_data.Rahmen_Nr.StringIsNullOrEmptyOrWhiteSpaces())
				{
					var raCreateResponse = new CreateBlanketHandler(new Models.OrderProcessing.CreateOrderModel
					{
						BlanketTypeId = (int)Enums.BlanketEnums.Types.purchase,
						CustomerId = 217,//psz
						SupplierId = check.lieferantenEntity.Nr,
						DocumentCustomer = _data.Rahmen_Nr,
						Converted = true,
					}, _user).Handle();
					if(raCreateResponse.Success)
					{
						var rahmenDate = _data.Rahmenauslauf ?? DateTime.Now;
						var raPositinCreateResponse = new CreateBlanketElementsHandler(new BlanketItem
						{
							AngebotNr = raCreateResponse.Body,
							Bezeichnung = check.ArticleEntity.Bezeichnung2,
							DateOfExpiry = rahmenDate,
							ExtensionDate = rahmenDate, // - 2025-08-14 Hejdukova remove ExtDate .AddMonths(3),
							KundenMatNummer = check.ArticleEntity.Bezeichnung1,
							ME = check.ArticleEntity.Einheit,
							Material = check.ArticleEntity.ArtikelNummer,
							MaterialNr = check.ArticleEntity.ArtikelNr,
							Preis = check.BestellnummernEntity.Einkaufspreis,
							BasePrice = check.BestellnummernEntity.Einkaufspreis ?? 0,
							ValidFrom = rahmenDate,
							WahrungId = 18,
							Zielmenge = _data.Rahmenmenge
						}, _user).Handle();
						if(!raPositinCreateResponse.Success)
						{
							new Core.CustomerService.Handlers.Blanket.DeleteBlanketHandler(_user, raCreateResponse.Body).Handle();
							response.Add(new ConvertedRahmensModel
							{
								ArtikelNummer = _data.Artikelnummer,
								Errors = raPositinCreateResponse.Errors.Select(x => x.Value).ToList(),
								RahmenNr = null,
								Supplier = "",
								Artikel_Nr = _data.Artikel_Nr ?? -1,
								RahmenAngebotNr = null
							});
							return ResponseModel<List<ConvertedRahmensModel>>.SuccessResponse(response);
						}
						else
						{
							var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(raCreateResponse.Body);
							RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Validated.GetDescription();
							RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Validated;
							Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Update(RahmenExtensionEntity);
							response.Add(new ConvertedRahmensModel
							{
								ArtikelNummer = _data.Artikelnummer,
								RahmenNr = raCreateResponse.Body,
								Supplier = check.AdressenEntity?.Name1,
								RahmenAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(raCreateResponse.Body).Angebot_Nr,
								Artikel_Nr = _data.Artikel_Nr ?? -1,
								Errors = null,
							});
							updateBlanketRestQty(_data.Rahmen_Nr, raPositinCreateResponse.Body);
						}
					}
				}
				if(_data.Rahmen2.HasValue && _data.Rahmen2.Value && !_data.Rahmen_Nr2.StringIsNullOrEmptyOrWhiteSpaces())
				{
					var raCreateResponse = new CreateBlanketHandler(new Models.OrderProcessing.CreateOrderModel
					{
						BlanketTypeId = (int)Enums.BlanketEnums.Types.purchase,
						CustomerId = 217,//psz
						SupplierId = check.lieferantenEntity.Nr,
						DocumentCustomer = _data.Rahmen_Nr2,
						Converted = true,
					}, _user).Handle();
					if(raCreateResponse.Success)
					{
						var rahmenDate = _data.Rahmenauslauf2 ?? DateTime.Now;
						var raPositinCreateResponse = new CreateBlanketElementsHandler(new BlanketItem
						{
							AngebotNr = raCreateResponse.Body,
							Bezeichnung = check.ArticleEntity.Bezeichnung2,
							DateOfExpiry = rahmenDate,
							ExtensionDate = rahmenDate, // - 2025-08-14 Hejdukova remove ExtDate .AddMonths(3),
							KundenMatNummer = check.ArticleEntity.Bezeichnung1,
							ME = check.ArticleEntity.Einheit,
							Material = check.ArticleEntity.ArtikelNummer,
							MaterialNr = check.ArticleEntity.ArtikelNr,
							Preis = check.BestellnummernEntity.Einkaufspreis,
							BasePrice = check.BestellnummernEntity.Einkaufspreis ?? 0,
							ValidFrom = rahmenDate,
							WahrungId = 18,
							Zielmenge = _data.Rahmenmenge2
						}, _user).Handle();
						if(!raPositinCreateResponse.Success)
						{
							new Core.CustomerService.Handlers.Blanket.DeleteBlanketHandler(_user, raCreateResponse.Body).Handle();
							response.Add(new ConvertedRahmensModel
							{
								ArtikelNummer = _data.Artikelnummer,
								Errors = raPositinCreateResponse.Errors.Select(x => x.Value).ToList(),
								RahmenNr = null,
								Supplier = "",
								Artikel_Nr = _data.Artikel_Nr ?? -1,
								RahmenAngebotNr = null
							});
							return ResponseModel<List<ConvertedRahmensModel>>.SuccessResponse(response);
						}
						else
						{
							var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(raCreateResponse.Body);
							RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Validated.GetDescription();
							RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Validated;
							Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Update(RahmenExtensionEntity);
							response.Add(new ConvertedRahmensModel
							{
								ArtikelNummer = _data.Artikelnummer,
								RahmenNr = raCreateResponse.Body,
								Supplier = check.AdressenEntity?.Name1,
								RahmenAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(raCreateResponse.Body).Angebot_Nr,
								Artikel_Nr = _data.Artikel_Nr ?? -1,
								Errors = null,
							});
							updateBlanketRestQty(_data.Rahmen_Nr2, raPositinCreateResponse.Body);
						}
					}
				}

				return ResponseModel<List<ConvertedRahmensModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<ConvertedRahmensModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<ConvertedRahmensModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ConvertedRahmensModel>>.SuccessResponse();
		}
		public ArticleRahmenCheckModel CheckDataErrors()
		{
			var result = new ArticleRahmenCheckModel();
			result.Errors = new List<string>();

			var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.Artikel_Nr ?? -1);
			var bestellnummernEntity = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetByArticleIdDefaultSupplier(_data.Artikel_Nr ?? -1);
			var supplierAdress = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();
			if(bestellnummernEntity == null)
				result.Errors.Add("Article does not have standard supplier");
			var supplier = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(bestellnummernEntity.Lieferanten_Nr ?? -1);
			if(supplier == null)
				result.Errors.Add("Article standard supplier not found");
			else
			{
				supplierAdress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplier.Nummer ?? -1);
				//RA1
				if(_data.Rahmen.HasValue && _data.Rahmen.Value && !_data.Rahmen_Nr.StringIsNullOrEmptyOrWhiteSpaces())
				{
					var ramenCheck1 = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypAndDocumentAndCustomer__(
				 Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract), _data.Rahmen_Nr, supplierAdress.Nr);
					if(ramenCheck1 > 0)
						result.Errors.Add($"Rahmen [{_data.Rahmen_Nr}] already created");
					if(!_data.Rahmenmenge.HasValue || _data.Rahmenmenge.Value <= 0)
						result.Errors.Add($"Rahmenmenge for Rahmen1 not valid.");
				}
				//RA2
				if(_data.Rahmen2.HasValue && _data.Rahmen2.Value && !_data.Rahmen_Nr2.StringIsNullOrEmptyOrWhiteSpaces())
				{
					var ramenCheck2 = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypAndDocumentAndCustomer__(
				Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract), _data.Rahmen_Nr2, supplierAdress.Nr);
					if(ramenCheck2 > 0)
						result.Errors.Add($"Rahmen [{_data.Rahmen_Nr2}] already created");
					if(!_data.Rahmenmenge2.HasValue || _data.Rahmenmenge2.Value <= 0)
						result.Errors.Add($"Rahmenmenge for Rahmen2 not valid.");
				}
			}

			result.ArticleEntity = article;
			result.AdressenEntity = supplierAdress;
			result.lieferantenEntity = supplier;
			result.BestellnummernEntity = bestellnummernEntity;

			return result;
		}
		public static int updateBlanketRestQty(string blanketDocument, int blanketPositionId)
		{
			try
			{
				var qty = 0m;
				//qty calculation
				var articleEntitites = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByBlanket(blanketDocument);
				if(articleEntitites != null && articleEntitites.Count > 0)
				{
					var orderEntities = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetByBlanket(articleEntitites.Select(x => x.Bestellung_Nr ?? -1)?.ToList());

					foreach(var articleItem in articleEntitites)
					{
						var orderItem = orderEntities?.Find(x => x.Nr == articleItem.Bestellung_Nr);
						if(orderItem != null)
						{
							qty += (articleItem.Start_Anzahl ?? 0);
							articleItem.RA_Pos_zu_Bestellposition = blanketPositionId;
						}
					}
					//qty update
					Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Update(articleEntitites);
					var blanketPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(blanketPositionId);
					blanketPositionEntity.Anzahl = blanketPositionEntity.OriginalAnzahl - qty;
					blanketPositionEntity.Geliefert = qty;
					Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(blanketPositionEntity.AngebotNr ?? -1);
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(blanketPositionEntity);
				}
				return 1;
			} catch(Exception e)
			{
				return -1;
				throw;
			}
		}
	}
}