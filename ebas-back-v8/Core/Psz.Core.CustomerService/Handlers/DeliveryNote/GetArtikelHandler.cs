using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.DeliveryNote;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.DeliveryNote
{
	public class GetArtikelHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderElementModel>>
	{
		//private string _data1 { get; set; }

		//private decimal _data3 { get; set; }
		private Models.DeliveryNote.GetArticleModel _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }

		public GetArtikelHandler(Identity.Models.UserModel user, Models.DeliveryNote.GetArticleModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<OrderElementModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var ArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);

				var responseBody = new OrderElementModel();

				if(ArtikelEntity != null)
				{
					var UnitPriceBasis = Convert.ToDecimal(ArtikelEntity?.Preiseinheit ?? 0);

					responseBody = new OrderElementModel
					{
						//ItemTypeId = -1,//elementDb.Typ,
						Id = -1,//elementDb.Nr,
						OrderNumber = null,//elementDb.AngebotNr?.ToString(),
						OrderId = -1,//elementDb.AngebotNr ?? -1,
									 //Done = false,//(elementDb.erledigt_pos ?? false),
									 //RP = false,//elementDb.RP ?? false,
									 //Position = 0,// elementDb.Position ?? 0,
						OpenQuantity_Quantity = this._data.Quantity,//Convert.ToDecimal(elementDb.Anzahl ?? 0),
						DesiredDate = null,//elementDb.DesiredDate ?? DateTime.Now.AddDays(+30),***
						DeliveryDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),//elementDb.Liefertermin,
																							//StorageLocationId = -1,//storageLocationDb != null ? storageLocationDb.LagerortId : -1,
																							//StorageLocationName = "",//storageLocationDb?.Lagerort,
						Discount = 0m,//Convert.ToDecimal(elementDb.Rabatt ?? 0),
						VAT = ArtikelEntity?.Umsatzsteuer ?? 0,
						//Math.Round(Convert.ToDecimal(ArtikelEntity?.Umsatzsteuer ?? 0) * 100, 2),//Convert.ToDecimal(elementDb.USt ?? 0),***
						CopperBase = ArtikelEntity?.Kupferbasis ?? 0,//elementDb.Kupferbasis ?? 0,***
						DelFixed = ArtikelEntity?.DELFixiert ?? false,//elementDb.DELFixiert ?? false,***
						DelNote = ArtikelEntity?.DEL ?? 0,//elementDb.DEL ?? 0,***
						CopperWeight = Convert.ToDecimal(ArtikelEntity?.CuGewicht ?? 0),//Convert.ToDecimal(elementDb.EinzelCuGewicht ?? 0),***
						ProductionNumber = 0,//(elementDb.Fertigungsnummer ?? 0),
											 // >>>>>>>>>>>>>>>>
						FixedUnitPrice = false,//elementDb.EKPreise_Fix ?? false, // <<<<<< !
						FixedTotalPrice = false,//elementDb.VKFestpreis ?? false,
												//UnitPrice = 0,//Convert.ToDecimal(elementDb.VKEinzelpreis ?? 0),***
												//TotalPrice = 0,//Convert.ToDecimal(elementDb.VKGesamtpreis ?? 0),
						UnitPriceBasis = Convert.ToDecimal(ArtikelEntity?.Preiseinheit ?? 0),//Convert.ToDecimal(elementDb.Preiseinheit ?? 0),***
						/*to check*/
						UnloadingPoint = ArtikelEntity.Abladestelle,//elementDb.Abladestelle,
						OpenQuantity_UnitPrice = 0,//Convert.ToDecimal(elementDb.Einzelpreis ?? 0),
						OpenQuantity_TotalPrice = 0,//Convert.ToDecimal(elementDb.Gesamtpreis ?? 0),
						OpenQuantity_CopperWeight = 0,//Convert.ToDecimal(elementDb.GesamtCuGewicht ?? 0),
						OpenQuantity_CopperSurcharge = 0,//Convert.ToDecimal(elementDb.Gesamtkupferzuschlag ?? 0),
						OriginalOrderQuantity = this._data.Quantity,//Convert.ToDecimal(elementDb.OriginalAnzahl ?? 0),
						OriginalOrderAmount = 0,//(decimal)getOriginalPrice(elementDb, elementDbExtension), // <<< Convert.ToDecimal(elementDb.Gesamtpreis ?? 0),
						DeliveredQuantity = 0,//Convert.ToDecimal(elementDb.Geliefert ?? 0),
											  // >>>>>>>>>>>>>>>>
						FreeText = string.Empty,//elementDb.Freies_Format_EDI,
						Note1 = string.Empty,//elementDb.Bemerkungsfeld1,
						Note2 = string.Empty,//elementDb.Bemerkungsfeld2,

						// >>>>>>>>>>>>>>>>
						// >>>>>>>>>>>>>>
						Designation1 = ArtikelEntity?.Bezeichnung1,
						Designation2 = ArtikelEntity?.Bezeichnung2,
						Designation3 = ArtikelEntity?.Bezeichnung3,
						MeasureUnitQualifier = ArtikelEntity?.Einheit,
						DrawingIndex = ArtikelEntity?.Index_Kunde,
						Index_Kunde = ArtikelEntity?.Index_Kunde,
						Index_Kunde_Datum = ArtikelEntity?.Index_Kunde_Datum,
					};
					//calculations
					bool fixedPrice = false;/* ArtikelEntity?.VKFestpreis ?? false*/
					;
					if(this._data.VK_fixed.HasValue)
						fixedPrice = this._data.VK_fixed.Value;
					else
						fixedPrice = ArtikelEntity?.VKFestpreis ?? false;

					var itemSupplierNumber = !string.IsNullOrWhiteSpace(responseBody.ItemNumber) ? responseBody.ItemNumber : null;
					var itemsPricingGroup = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(new List<int> { ArtikelEntity.ArtikelNr }).FirstOrDefault();
					var me1 = 0m;
					var me2 = 0m;
					var me3 = 0m;
					var me4 = 0m;
					var pm1 = 0m;
					var pm2 = 0m;
					var pm3 = 0m;
					var pm4 = 0m;
					var verkaufspreis = 0m;
					if(itemsPricingGroup != null)
					{
						me1 = Convert.ToDecimal(itemsPricingGroup.ME1 ?? 0m);
						me2 = Convert.ToDecimal(itemsPricingGroup.ME2 ?? 0m);
						me3 = Convert.ToDecimal(itemsPricingGroup.ME3 ?? 0m);
						me4 = Convert.ToDecimal(itemsPricingGroup.ME4 ?? 0m);
						pm1 = Convert.ToDecimal(itemsPricingGroup.PM1 ?? 0m);
						pm2 = Convert.ToDecimal(itemsPricingGroup.PM2 ?? 0m);
						pm3 = Convert.ToDecimal(itemsPricingGroup.PM3 ?? 0m);
						pm4 = Convert.ToDecimal(itemsPricingGroup.PM4 ?? 0m);
						verkaufspreis = Convert.ToDecimal(itemsPricingGroup.Verkaufspreis ?? 0m);
					}
					//
					var vkUnitPrice = (!this._data.VK_price.HasValue) ? CalculateVkUnitPrice(fixedPrice,
						verkaufspreis,
						responseBody.OriginalOrderQuantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4) : this._data.VK_price;

					var singleCopperSurcharge = CalculateSingleCopperSurcharge(fixedPrice,
					   responseBody.DelNote,
						responseBody.CopperWeight,
						responseBody.CopperBase);

					var totalCopperSurcharge = CalculateTotalCopperSurcharge(fixedPrice,
						responseBody.OriginalOrderQuantity,
						singleCopperSurcharge);

					var unitPrice = CalculateUnitPrice(fixedPrice,
						responseBody.UnitPriceBasis ?? 0,
						responseBody.OriginalOrderQuantity,
						(decimal)vkUnitPrice,
						verkaufspreis,
						singleCopperSurcharge,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var totalPrice = CalculateTotalPrice(responseBody.UnitPriceBasis ?? 0,
						unitPrice,
						responseBody.OriginalOrderQuantity,
						responseBody.Discount);

					var vKTotalPrice = CalculateVkTotalPrice(responseBody.UnitPriceBasis ?? 0,
						(decimal)vkUnitPrice,
						responseBody.OriginalOrderQuantity);

					var totalCuWeight = CalculateTotalWeight(responseBody.OriginalOrderQuantity,
					   responseBody.CopperWeight);

					//filling
					responseBody.CopperSurcharge = !(responseBody.FixedUnitPrice) ? decimal.Round((Convert.ToDecimal((responseBody?.DelNote * 1.01m) - responseBody?.CopperBase) / 100m) * responseBody.CopperWeight, 2) : 0;
					responseBody.UnitPrice = (decimal)vkUnitPrice;
					responseBody.TotalPrice = vKTotalPrice;
					responseBody.OpenQuantity_UnitPrice = unitPrice;
					responseBody.OpenQuantity_TotalPrice = totalPrice;
					responseBody.OpenQuantity_CopperWeight = totalCuWeight;
					responseBody.OpenQuantity_CopperSurcharge = totalCopperSurcharge;
					responseBody.OriginalOrderAmount = totalPrice;
					//
					responseBody.FixedTotalPrice = fixedPrice;//elementDb.VKFestpreis ?? false,
					responseBody.CalculatedValue = (UnitPriceBasis != 0) ?
						(this._data.Quantity / UnitPriceBasis) * unitPrice * (1 - 0m) : 0;
				}
				return ResponseModel<OrderElementModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<OrderElementModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<OrderElementModel>.AccessDeniedResponse();
			}
			var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
			var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(artikelEntity.ArtikelNr);
			if(itemPricingGroupDb == null)
			{
				return ResponseModel<OrderElementModel>.FailureResponse($"Article {this._data.ArticleNumber} has no verkaufspreis");
			}
			return ResponseModel<OrderElementModel>.SuccessResponse();
		}

		internal static decimal CalculateVkUnitPrice(bool fixedPrice,
			decimal verkaufspreis,
			decimal orderedQuantity,
			decimal me1,
			decimal me2,
			decimal me3,
			decimal me4,
			decimal pm2,
			decimal pm3,
			decimal pm4)
		{
			if(fixedPrice == true)
			{
				return verkaufspreis;
			}
			else if(orderedQuantity <= me1)
			{
				return verkaufspreis;
			}
			else if(orderedQuantity > me1 && orderedQuantity <= me2)
			{
				return verkaufspreis - verkaufspreis * pm2 / 100;
			}
			else if(orderedQuantity > me2 && orderedQuantity <= me3)
			{
				return verkaufspreis - verkaufspreis * pm3 / 100;
			}
			else if(orderedQuantity > me3 && orderedQuantity <= me4)
			{
				return verkaufspreis - verkaufspreis * pm4 / 100;
			}
			else
			{
				return verkaufspreis;
			}
		}

		internal static decimal CalculateTotalPrice(decimal unitPriceBasis,
		   decimal einzelpreis,
		   decimal ordredQuantity,
		   decimal discount)
		{
			return (unitPriceBasis > 0 ? (einzelpreis / unitPriceBasis) : einzelpreis)
				* ordredQuantity
				* (1m - discount);
		}

		internal static decimal CalculateUnitPrice(bool isFixedFrice,
		decimal unitPriceBasis,
		decimal orderedQuantity,
		decimal vKEinzelpreis,
		decimal verkaufspreis,
		decimal einzelkupferzuschlag,
		decimal me1,
		decimal me2,
		decimal me3,
		decimal me4,
		decimal pm2,
		decimal pm3,
		decimal pm4)
		{
			if(isFixedFrice)
			{
				return vKEinzelpreis;
			}
			else if(orderedQuantity <= me1)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis);
			}
			else if(orderedQuantity > me1 && orderedQuantity <= me2)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis - verkaufspreis * pm2 / 100);
			}
			else if(orderedQuantity > me2 && orderedQuantity <= me3)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis - verkaufspreis * pm3 / 100);
			}
			else if(orderedQuantity > me3 && orderedQuantity <= me4)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis - verkaufspreis * pm4 / 100);
			}
			else
			{
				return einzelkupferzuschlag * unitPriceBasis + vKEinzelpreis;
			}
		}

		internal static decimal CalculateSingleCopperSurcharge(bool fixedPrice,
			int del,
			decimal cuGewicht,
			int kupferbasis)
		{
			return !fixedPrice
				? decimal.Round((Convert.ToDecimal((del * 1.01m) - kupferbasis) / 100m) * cuGewicht, 2)
				: 0;
		}

		internal static decimal CalculateVkTotalPrice(decimal unitPriceBasis,
			decimal vKEinzelpreis,
			decimal ordredQuantity)
		{
			return ordredQuantity
				* (unitPriceBasis > 0 ? (vKEinzelpreis / unitPriceBasis) : vKEinzelpreis);
		}

		internal static decimal CalculateTotalWeight(decimal ordredQuantity,
			decimal cuGewicht)
		{
			return ordredQuantity * cuGewicht;
		}

		internal static decimal CalculateTotalCopperSurcharge(bool fixedPrice,
			decimal ordredQuantity,
			decimal einzelkupferzuschlag)
		{
			return fixedPrice
				? 0
				: (ordredQuantity * einzelkupferzuschlag);
		}
	}

}
