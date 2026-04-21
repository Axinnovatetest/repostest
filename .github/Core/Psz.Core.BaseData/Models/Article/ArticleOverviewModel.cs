using Infrastructure.Data.Entities.Tables.BSD;
using Microsoft.AspNetCore.SignalR.Protocol;
using Psz.Core.BaseData.Handlers;
using Psz.Core.BaseData.Models.Article.ManagerUser;
using Psz.Core.BaseData.Models.ObjectLog;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleOverviewModel
	{
		public int ArticleImageId { get; set; }
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Bezeichnung2 { get; set; }
		public string Bezeichnung3 { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public string KundeIndex { get; set; }
		public DateTime? KundeIndexDatum { get; set; }
		public string Langtext { get; set; }
		public string Verpackung { get; set; }
		public string Lieferzeit { get; set; }
		public bool aktiv { get; set; }
		public string CustomerItemNumber { get; set; }

		public List<ObjectLogModel> ListLogs { get; set; }
		public List<ManagerUserModel> ListManagers { get; set; }
		public IEnumerable<string> ToolsTN { get; set; }
		public IEnumerable<string> ToolsBETN { get; set; }
		public IEnumerable<string> ToolsWSTN { get; set; }
		public IEnumerable<string> ToolsGZTN { get; set; }
		public IEnumerable<string> ToolsAL { get; set; }
		public string Artikelbezeichnung { get; set; }
		public int? Losgroesse { get; set; }
		public string CustomerName { get; set; }
		public string Consumption12Months { get; set; }
		public string OrderNumber { get; set; }
		public string Projektname { get; set; }
		public decimal? Produktionlosgrosse { get; set; }
		public int? ManufacturerPreviousArticleId { get; set; }
		public string ManufacturerPreviousArticle { get; set; }
		public int? ManufacturerNextArticleId { get; set; }
		public string ManufacturerNextArticle { get; set; }
		public string Manufacturer { get; set; }
		public string ManufacturerNumber { get; set; }
		public string Warengruppe { get; set; }

		public int? CustomerTechnicId { get; set; }
		public string CustomerTechnic { get; set; }
		public string CustomerEnd { get; set; }
		public int CustomerNumber { get; set; }
		public int CustomerId { get; set; }
		public ArticleOverviewModel() { }
		public ArticleOverviewModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			if(artikelEntity == null)
				return;

			ArtikelNr = artikelEntity.ArtikelNr;
			ArtikelNummer = artikelEntity.ArtikelNummer;
			Bezeichnung1 = artikelEntity.Bezeichnung1;
			Bezeichnung2 = artikelEntity.Bezeichnung2;
			Bezeichnung3 = artikelEntity.Bezeichnung3;
			Langtext = artikelEntity.Langtext;
			Verpackung = artikelEntity.Verpackung;
			aktiv = artikelEntity.aktiv ?? false;
			CustomerItemNumber = artikelEntity.CustomerItemNumber;

			ManufacturerPreviousArticleId = artikelEntity.ManufacturerPreviousArticleId;
			ManufacturerPreviousArticle = artikelEntity.ManufacturerPreviousArticle;
			ManufacturerNextArticleId = artikelEntity.ManufacturerNextArticleId;
			ManufacturerNextArticle = artikelEntity.ManufacturerNextArticle;

			CustomerTechnicId = artikelEntity.CustomerTechnicId;
			CustomerTechnic = artikelEntity.CustomerTechnic;
			CustomerEnd = artikelEntity.CustomerEnd;
		}
		public ArticleOverviewModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> listLogs,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity managerEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity,
			Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity)
		{
			if(artikelEntity == null)
				return;
			ArtikelNr = artikelEntity.ArtikelNr;
			ArtikelNummer = artikelEntity.ArtikelNummer;
			Bezeichnung1 = artikelEntity.Bezeichnung1;
			Bezeichnung2 = artikelEntity.Bezeichnung2;
			Bezeichnung3 = artikelEntity.Bezeichnung3;
			KundeIndex = artikelEntity.Index_Kunde;
			KundeIndexDatum = artikelEntity.Index_Kunde_Datum;
			Langtext = artikelEntity.Langtext;
			Verpackung = artikelEntity.Verpackung;
			Lieferzeit = artikelEntity.Lieferzeit;
			aktiv = artikelEntity.aktiv ?? false;
			CustomerItemNumber = artikelEntity.CustomerItemNumber;

			ManufacturerPreviousArticleId = artikelEntity.ManufacturerPreviousArticleId;
			ManufacturerPreviousArticle = artikelEntity.ManufacturerPreviousArticle;
			ManufacturerNextArticleId = artikelEntity.ManufacturerNextArticleId;
			ManufacturerNextArticle = artikelEntity.ManufacturerNextArticle;

			CustomerTechnicId = artikelEntity.CustomerTechnicId;
			CustomerTechnic = artikelEntity.CustomerTechnic;
			CustomerEnd = artikelEntity.CustomerEnd;
			CustomerName = adressenEntity?.Name1 ?? "";
			CustomerNumber = adressenEntity?.Kundennummer ?? 0;
			CustomerId = kundenEntity?.Nr ?? 0;

			if(listLogs != null && listLogs.Count > 0)
			{
				this.ListLogs = new List<ObjectLogModel>();
				for(var i = 0; i < listLogs.Count; i++)
				{
					var item = listLogs[i];
					this.ListLogs.Add(new ObjectLogModel(item));
				}
			}

			if(managerEntity != null)
			{
				this.ListManagers = new List<ManagerUserModel> { new ManagerUserModel(managerEntity) };
			}
			ArticleImageId = artikelEntity.ArticleImageId;

			Artikelbezeichnung = artikelEntity.Artikelbezeichnung;
			Losgroesse = artikelEntity.Losgroesse;
			Produktionlosgrosse = artikelEntity.ProductionLotSize;
			Projektname = artikelEntity.Projektname;
			Manufacturer = artikelEntity.Manufacturer;
			ManufacturerNumber = artikelEntity.ManufacturerNumber;
			Warengruppe = artikelEntity.Warengruppe;
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = this.ArtikelNr,
				ArtikelNummer = this.ArtikelNummer,
				Bezeichnung1 = this.Bezeichnung1,
				Bezeichnung2 = this.Bezeichnung2,
				Bezeichnung3 = this.Bezeichnung3,
				Langtext = this.Langtext,
				Verpackung = this.Verpackung,


				Projektname = this.Projektname,
				CustomerTechnicId = this.CustomerTechnicId,
				CustomerTechnic = this.CustomerTechnic,
				CustomerEnd = this.CustomerEnd,
				Manufacturer = this.Manufacturer,
				ManufacturerNumber = this.ManufacturerNumber,
			};
		}
		#region Overview

		public class Blanket
		{
			public int OrderId { get; set; }
			public bool Rahmen { get; set; }
			public string Rahmen_Nr { get; set; }
			public decimal? Rahmenmenge { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public decimal? Restmenge { get; set; } = 0;

			// -
			public int ArticleId { get; set; }
			public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToBlanketEntity(bool isFirst,
				Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity prevEntity,
				UserModel user = null,
				Enums.ObjectLogEnums.Objects objectItem = Enums.ObjectLogEnums.Objects.Article,
				int objectItemId = -1,
				List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs = null,
				Enums.ObjectLogEnums.LogType logType = Enums.ObjectLogEnums.LogType.Edit)
			{
				if(isFirst)
				{
					if(prevEntity != null)
					{
						if(objectLogs == null)
							objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

						if(prevEntity.Rahmen != Rahmen)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmen", $"{prevEntity.Rahmen}",
											$"{Rahmen}", $"{objectItem.GetDescription()}", logType));
						}
						if(prevEntity.RahmenNr != Rahmen_Nr)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmen_Nr", $"{prevEntity.RahmenNr}",
											$"{Rahmen_Nr}", $"{objectItem.GetDescription()}", logType));
						}
						if(prevEntity.Rahmenmenge != Rahmenmenge)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmenmenge", $"{prevEntity.Rahmenmenge}",
											$"{Rahmenmenge}", $"{objectItem.GetDescription()}", logType));
						}
						if(prevEntity.Rahmenauslauf != Rahmenauslauf)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmenauslauf", $"{prevEntity.Rahmenauslauf}",
											$"{Rahmenauslauf}", $"{objectItem.GetDescription()}", logType));
						}
					}
					// -

					return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
					{
						ArtikelNr = ArticleId,
						RahmenNr = Rahmen_Nr,
						Rahmenmenge = Rahmenmenge,
						Rahmen = Rahmen,
						Rahmenauslauf = Rahmenauslauf
					};
				}
				else
				{
					if(prevEntity != null)
					{
						if(objectLogs == null)
							objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

						if(prevEntity.Rahmen2 != Rahmen)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmen", $"{prevEntity.Rahmen2}",
											$"{Rahmen}", $"{objectItem.GetDescription()}", logType));
						}
						if(prevEntity.RahmenNr2 != Rahmen_Nr)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmen_Nr2", $"{prevEntity.RahmenNr2}",
											$"{Rahmen_Nr}", $"{objectItem.GetDescription()}", logType));
						}
						if(prevEntity.Rahmenmenge2 != Rahmenmenge)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmenmenge2", $"{prevEntity.Rahmenmenge2}",
											$"{Rahmenmenge}", $"{objectItem.GetDescription()}", logType));
						}
						if(prevEntity.Rahmenauslauf2 != Rahmenauslauf)
						{
							objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rahmenauslauf2", $"{prevEntity.Rahmenauslauf2}",
											$"{Rahmenauslauf}", $"{objectItem.GetDescription()}", logType));
						}
					}
					// -

					return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
					{
						ArtikelNr = ArticleId,
						RahmenNr2 = Rahmen_Nr,
						Rahmenmenge2 = Rahmenmenge,
						Rahmenauslauf2 = Rahmenauslauf,
						Rahmen2 = Rahmen,
					};
				}
			}
		}
		public class BlanketDetail
		{
			public int? OrderNumber { get; set; }
			public string OrderType { get; set; }
			public string Rahmen_Nr { get; set; }
			public decimal? Quantity { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public decimal? Restmenge { get; set; } = 0;
			public BlanketDetail(
				Infrastructure.Data.Entities.Tables.BSD.Bestellte_ArtikelEntity bestellte_ArtikelEntity,
				Infrastructure.Data.Entities.Tables.BSD.BestellungenEntity bestellungenEntity)
			{
				if(bestellte_ArtikelEntity == null || bestellungenEntity == null)
					return;

				OrderNumber = bestellungenEntity.Bestellung_Nr;
				OrderType = bestellungenEntity.Typ;
				Rahmen_Nr = bestellte_ArtikelEntity.InfoRahmennummer;
				Quantity = bestellte_ArtikelEntity.Start_Anzahl;
			}
		}

		public class BlanketHistoryResponseModel
		{
			public int Id { get; set; }
			public int ArticleId { get; set; }
			public DateTime? Date { get; set; }
			public string User { get; set; }
			public string Topic { get; set; }
			public string Description { get; set; }

			public BlanketHistoryResponseModel(PSZ_ArtikelhistorieEntity artikelhistorieEntity)
			{
				if(artikelhistorieEntity == null)
					return;

				// -
				Id = artikelhistorieEntity.ID;
				ArticleId = artikelhistorieEntity.Artikel_Nr ?? -1;
				Date = artikelhistorieEntity.Datum_Anderung;
				User = artikelhistorieEntity.Anderung_von;
				Topic = artikelhistorieEntity.Anderungsbereich;
				Description = artikelhistorieEntity.Anderungsbeschreibung;
			}

			public Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity ToEntity()
			{
				return new PSZ_ArtikelhistorieEntity
				{
					ID = Id,
					Artikel_Nr = ArticleId,
					Datum_Anderung = Date,
					Anderung_von = User,
					Anderungsbereich = Topic,
					Anderungsbeschreibung = Description
				};
			}
		}

		public class PurchaseDetailsResponseModel
		{
			public int ArticleId { get; set; }
			public int StandardSupplierId { get; set; }
			public string StandardSupplierName { get; set; }
			public decimal? StandardPurchasePrice { get; set; }
			public string OrderNumber { get; set; }
		}
		#endregion
	}
}
