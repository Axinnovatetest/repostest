using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderModel
	{
		public int Id { get; set; }
		public int Id_Order { get; set; }
		public string Number { get; set; }
		public string Type { get; set; }
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public string Description { get; set; }
		public string ProjectState { get; set; }
		public string ProjectClientName { get; set; }
		public string ProjectClientNumber { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int? Status { get; set; }
		public int? Level { get; set; }
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public int? MandantId { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public int ResponsableId { get; set; }
		public string ResponsableName { get; set; }
		public DateTime? OrderDate { get; set; }
		public decimal TotalAmount { get; set; }

		public DateTime? ApprovalTime { get; set; }
		public int? ApprovalUserId { get; set; }
		public int? LastRejectionLevel { get; set; }
		public DateTime? LastRejectionTime { get; set; }
		public int? LastRejectionUserId { get; set; }
		public List<ValidatorModel> Validators { get; set; }

		public List<Article.ArticleModel> Articles { get; set; }
		public List<Files.FilesModel> File { get; set; }


		public string BillingContactName { get; set; }
		public string BillingAddress { get; set; }
		public int BillingCompanyId { get; set; }
		public string BillingCompanyName { get; set; }
		public int? BillingDepartmentId { get; set; }
		public string BillingDepartmentName { get; set; }
		public string BillingTelephone { get; set; }
		public string BillingFax { get; set; }


		public string DeliveryAddress { get; set; }
		public int? DeliveryCompanyId { get; set; }
		public string DeliveryCompanyName { get; set; }
		public int? DeliveryDepartmentId { get; set; }
		public string DeliveryDepartmentName { get; set; }
		public string DeliveryTelephone { get; set; }
		public string DeliveryFax { get; set; }
		public bool? Archived { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public bool? Deleted { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }

		public int? StorageLocationId { get; set; }
		public string StorageLocationName { get; set; }


		public string SupplierAnrede { get; set; }
		public string SupplierName2 { get; set; }
		public string SupplierName3 { get; set; }
		public string SupplierContact { get; set; }
		public string SupplierDepartment { get; set; }
		public string SupplierStreet { get; set; }
		public string SupplierPostalCode { get; set; }
		public string SupplierCity { get; set; }
		public string SupplierCountry { get; set; }
		public string SupplierSalutations { get; set; }

		public string SupplierNumber { get; set; }
		public string SupplierNummer { get; set; }
		public string SupplierTradingTerm { get; set; }
		public string SupplierPaymentMethod { get; set; }
		public string SupplierPaymentTerm { get; set; }
		public string SupplierTelephone { get; set; }
		public string SupplierFax { get; set; }
		public string SupplierEmail { get; set; }

		public decimal TotalAmountDefaultCurrency { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencySymbol { get; set; }

		public bool CanValidate { get; set; }

		// - 
		public string OrderPlacedEmailMessage { get; set; }
		public string OrderPlacedEmailTitle { get; set; }
		public int? OrderPlacedReportFileId { get; set; }
		public string OrderPlacedSendingEmail { get; set; }
		public string OrderPlacedSupplierEmail { get; set; }
		public DateTime? OrderPlacedTime { get; set; }
		public string OrderPlacedUserEmail { get; set; }
		public int? OrderPlacedUserId { get; set; }
		public string OrderPlacedUserName { get; set; }
		public string OrderPlacementCCEmail { get; set; } // semi-column separated emails

		// - payment types - Purchase / Leasing
		public int? PoPaymentType { get; set; }
		public string PoPaymentTypeName { get; set; }
		public int? LeasingNbMonths { get; set; }
		public int? LeasingStartMonth { get; set; }
		public int? LeasingStartYear { get; set; }
		public decimal? LeasingTotalAmount { get; set; }
		public decimal? LeasingMonthAmount { get; set; }
		public decimal LeasingCurrentYearAmount { get; set; }
		public decimal? Discount { get; set; }
		public decimal? TotalAmountBeforeDiscount { get; set; }
		public decimal? TotalAmountDefaultCurrencyBeforeDiscount { get; set; }

		// - 
		// Nb of booked positions (BestellteArtikel)
		public int? PositionsBooked { get; set; }
		public int? PositionsCount { get; set; }
		public bool? BookingComplete { get; set; }
		// Nb of booking order (Bestellungen)
		public int BookingCount { get; set; }

		// -
		public bool CanViewInvoice { get; set; }


		public int? AllocationType { get; set; }
		public string AllocationTypeName { get; set; }
		public DateTime? ValidationRequestDate { get; set; }

		// - 2024-02-17 - Kechiche - Projects are by default visible at department level only 
		public bool SiteLevelVisibility { get; set; } = false;

		public OrderModel()
		{ }
		public OrderModel(
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity bestellungEntity,
			 Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
			 List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> articles,
			 List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> bestellteArticles,
			 List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> articleCustomerEntity,
			 List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> orderFileEntities,
			 List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> validators,
			 List<Infrastructure.Data.Entities.Tables.COR.UserEntity> usersValidators,
			 Core.Identity.Models.UserModel user = null,
			 int bookingCount = 0
			)
		{
			if(orderEntity == null)
				return;

			var supllierAdd = bestellungEntity?.Straße_Postfach?.Split('|');
			var supplierCity = bestellungEntity?.Land_PLZ_Ort?.Split('|');

			this.Id = orderEntity.Id;
			this.Id_Order = orderEntity.OrderId;
			this.Number = orderEntity.OrderNumber;
			this.MandantId = orderEntity.MandantId;
			this.ProjectId = orderEntity.ProjectId ?? -1;
			this.ProjectName = orderEntity.ProjectName;
			this.ProjectClientName = projectEntity?.CustomerName;
			this.ProjectClientNumber = projectEntity?.CustomerNr;
			this.Description = orderEntity?.Description;
			this.ProjectState = projectEntity?.Id_State == 1 ? "Active" : "Not Active or suspended";
			this.DepartmentId = orderEntity.DepartmentId;
			this.DepartmentName = orderEntity.DepartmentName;
			this.Status = orderEntity.Status;
			this.Level = orderEntity.Level;
			this.CompanyId = orderEntity.CompanyId ?? -1;
			this.CompanyName = orderEntity.CompanyName;

			this.ApprovalTime = orderEntity.ApprovalTime;
			this.ApprovalUserId = orderEntity.ApprovalUserId;
			this.LastRejectionLevel = orderEntity.LastRejectionLevel;
			this.LastRejectionTime = orderEntity.LastRejectionTime;
			this.LastRejectionUserId = orderEntity.LastRejectionUserId;

			this.SupplierId = orderEntity.SupplierId;
			this.SupplierName = orderEntity.SupplierName;
			this.SupplierAnrede = bestellungEntity?.Anrede;
			this.SupplierName2 = bestellungEntity?.Name2;
			this.SupplierName3 = bestellungEntity?.Name3;
			this.SupplierContact = bestellungEntity?.Ansprechpartner;
			this.SupplierDepartment = bestellungEntity?.Abteilung;
			this.SupplierStreet = supllierAdd?[0];
			this.SupplierPostalCode = supllierAdd?.Count() > 1 ? supllierAdd[1] : "";
			this.SupplierCity = supplierCity?[0];
			this.SupplierCountry = supplierCity?.Count() > 1 ? supplierCity[1] : "";
			this.SupplierSalutations = bestellungEntity?.Briefanrede;
			this.SupplierNumber = orderEntity.SupplierNumber;
			this.SupplierNummer = orderEntity.SupplierNummer;
			this.SupplierTradingTerm = orderEntity.SupplierTradingTerm;
			this.SupplierPaymentMethod = orderEntity.SupplierPaymentMethod;
			this.SupplierPaymentTerm = orderEntity.SupplierPaymentTerm;
			this.SupplierTelephone = orderEntity.SupplierTelephone;
			this.SupplierFax = orderEntity.SupplierFax;
			this.SupplierEmail = orderEntity.SupplierEmail;

			this.CurrencyId = orderEntity.CurrencyId;

			this.CurrencyName = orderEntity.CurrencyName;
			this.ResponsableId = orderEntity.IssuerId;

			this.ResponsableName = orderEntity.IssuerName;
			this.OrderDate = orderEntity.CreationDate;
			this.Type = orderEntity.OrderType;

			this.DeliveryAddress = orderEntity.DeliveryAddress;
			this.DeliveryDepartmentId = orderEntity.DeliveryDepartmentId;
			this.DeliveryDepartmentName = orderEntity.DeliveryDepartmentName;
			this.DeliveryCompanyId = orderEntity.DeliveryCompanyId ?? -1;
			this.DeliveryCompanyName = orderEntity.DeliveryCompanyName;
			this.DeliveryTelephone = orderEntity.DeliveryTelephone;
			this.DeliveryFax = orderEntity.DeliveryFax;

			this.BillingContactName = orderEntity.BillingContactName;
			this.BillingAddress = orderEntity.BillingAddress;
			this.BillingDepartmentId = orderEntity.BillingDepartmentId;
			this.BillingDepartmentName = orderEntity.BillingDepartmentName;
			this.BillingCompanyId = orderEntity.BillingCompanyId ?? -1;
			this.BillingCompanyName = orderEntity.BillingCompanyName;
			this.BillingTelephone = orderEntity.BillingTelephone;
			this.BillingFax = orderEntity.BillingFax;

			this.Archived = orderEntity.Archived;
			this.ArchiveTime = orderEntity.ArchiveTime;
			this.ArchiveUserId = orderEntity.ArchiveUserId;

			this.Deleted = orderEntity.Deleted;
			this.DeleteTime = orderEntity.DeleteTime;
			this.DeleteUserId = orderEntity.DeleteUserId;

			this.StorageLocationId = orderEntity.StorageLocationId;
			this.StorageLocationName = orderEntity.StorageLocationName;

			this.ValidationRequestDate = orderEntity.ValidationRequestTime;
			this.SiteLevelVisibility = projectEntity?.SiteLevelVisibility ?? false;


			if(validators != null && validators.Count > 0)
			{
				this.Validators = new List<ValidatorModel>();
				for(var i = 0; i < validators.Count - 1; i++)
				{
					var item = validators[i];
					var _user = usersValidators.Find(x => x.Id == item.Id_Validator);
					this.Validators.Add(new ValidatorModel(item, _user));
				}

				// - Purchase
				if(orderEntity.OrderType.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().Trim().ToLower() && orderEntity.ProjectId > 0)
				{
					this.Validators.Add(new ValidatorModel(validators[validators.Count - 1],
				   new Infrastructure.Data.Entities.Tables.COR.UserEntity
				   {
					   Id = -1,
					   Name = $"Purchase [{projectEntity.CompanyName}]"
				   }));
				}
				else
				{
					this.Validators.Add(new ValidatorModel(validators[validators.Count - 1],
				   new Infrastructure.Data.Entities.Tables.COR.UserEntity
				   {
					   Id = -1,
					   Name = $"Purchase [{orderEntity.CompanyName}]"
				   }));
				}

			}
			if(orderEntity.OrderType.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription().Trim().ToLower())
			{
				var _user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity.IssuerId);
				var userCompany = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? 0);
				var lastValidator1 = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userCompany.FinanceValidatorOneId ?? 0)
				?? Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userCompany.FinanceValidatorTowId ?? 0);
				this.Validators = new List<ValidatorModel>
				{
					new ValidatorModel(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity{ Id_Validator=_user.Id, Level=0}, _user),
					new Models.Budget.Order.ValidatorModel(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity {  Level=1}, lastValidator1 )
				};
				// - 2025-01-15 show status & level if complete validated
				if(orderEntity.Level > 0)
				{
					this.Status = 1;
					this.Level = 1;

					if(orderEntity.ApprovalUserId is not null && orderEntity.ApprovalUserId > 0)
					{
						this.Status = 2;
						this.Level = 2;
					}
				}
			}

			TotalAmountBeforeDiscount = orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
				? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(articles, false)
				: (orderEntity.LeasingTotalAmount ?? 0);
			TotalAmount = orderEntity.Discount.HasValue && orderEntity.Discount.Value > 0 ? (1 - orderEntity.Discount.Value / 100) * (TotalAmountBeforeDiscount ?? 0) : (TotalAmountBeforeDiscount ?? 0);

			TotalAmountDefaultCurrencyBeforeDiscount = orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
				? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(articles, false, true)
				: (orderEntity.LeasingTotalAmount ?? 0);
			TotalAmountDefaultCurrency = orderEntity.Discount.HasValue && orderEntity.Discount.Value > 0 ? (1 - orderEntity.Discount.Value / 100) * (TotalAmountDefaultCurrencyBeforeDiscount ?? 0) : (TotalAmountDefaultCurrencyBeforeDiscount ?? 0);

			if(articles != null && articles.Count > 0)
			{
				Articles = new List<Article.ArticleModel>();
				foreach(var item in articles)
				{
					var bestellteArticleItem = bestellteArticles?.Find(x => x.Nr == item.BestellteArtikelNr);
					Articles.Add(new Article.ArticleModel(item, bestellteArticleItem, articleCustomerEntity));
				}
			}
			File = orderFileEntities == null || orderFileEntities.Count <= 0
				? null
				: orderFileEntities.Select(x => new Files.FilesModel(x))?.ToList();

			DefaultCurrencyId = orderEntity.DefaultCurrencyId;
			DefaultCurrencySymbol = orderEntity.DefaultCurrencyName; // - FIXME:

			// - 
			OrderPlacedEmailMessage = orderEntity.OrderPlacedEmailMessage;
			OrderPlacedEmailTitle = orderEntity.OrderPlacedEmailTitle;
			OrderPlacedReportFileId = orderEntity.OrderPlacedReportFileId;
			OrderPlacedSendingEmail = orderEntity.OrderPlacedSendingEmail;
			OrderPlacedSupplierEmail = orderEntity.OrderPlacedSupplierEmail;
			OrderPlacedTime = orderEntity.OrderPlacedTime;
			OrderPlacedUserEmail = orderEntity.OrderPlacedUserEmail;
			OrderPlacedUserId = orderEntity.OrderPlacedUserId;
			OrderPlacedUserName = orderEntity.OrderPlacedUserName;
			OrderPlacementCCEmail = orderEntity.OrderPlacementCCEmail;

			// - payment types - Purchase / Leasing
			PoPaymentType = orderEntity.PoPaymentType;
			PoPaymentTypeName = orderEntity.PoPaymentTypeName;

			LeasingNbMonths = orderEntity.LeasingNbMonths;
			LeasingStartMonth = orderEntity.LeasingStartMonth;
			LeasingStartYear = orderEntity.LeasingStartYear;
			LeasingTotalAmount = orderEntity.LeasingTotalAmount;
			LeasingMonthAmount = orderEntity.LeasingMonthAmount;

			Discount = orderEntity.Discount;

			// -
			BookingComplete = bestellungEntity?.erledigt;
			PositionsBooked = bestellteArticles != null && bestellteArticles.Count > 0
				? bestellteArticles.Where(x => x.erledigt_pos.HasValue && x.erledigt_pos.Value)?.Count()
				: 0;
			PositionsCount = bestellteArticles != null && bestellteArticles.Count > 0
				? bestellteArticles.Count()
				: 0;

			BookingCount = bookingCount;
			CanViewInvoice = orderEntity.ApprovalTime.HasValue && bookingCount > 0 && (
					(user?.IsGlobalDirector ?? false)
					|| (user?.SuperAdministrator ?? false)
					|| user?.Id == orderEntity.IssuerId
					|| ((user?.Access?.Financial.ModuleActivated ?? false)
						&& (
							(orderEntity.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower() && ((user?.Access?.Financial.Budget.CommandeExternalViewInvoice ?? false) || (user?.Access?.Financial.Budget.CommandeExternalViewInvoiceAllGroup ?? false)))
							|| (orderEntity.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower() && ((user?.Access?.Financial.Budget.CommandeInternalViewInvoice ?? false) || (user?.Access?.Financial.Budget.CommandeInternalViewInvoiceAllGroup ?? false)))
					))
					);

			this.AllocationType = orderEntity.AllocationType;
			this.AllocationTypeName = orderEntity.AllocationTypeName;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity ToOrderExtension()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity
			{
				Id = Id,
				OrderId = Id_Order,
				DepartmentName = DepartmentName,
				CurrencyId = CurrencyId,
				MandantId = CompanyId,
				DepartmentId = DepartmentId,
				CompanyId = CompanyId,
				ProjectId = ProjectId,
				ProjectName = ProjectName,
				SupplierId = SupplierId,
				SupplierName = SupplierName,
				SupplierNumber = SupplierNumber,
				SupplierNummer = SupplierNummer,
				SupplierTradingTerm = SupplierTradingTerm,
				SupplierPaymentMethod = SupplierPaymentMethod,
				SupplierPaymentTerm = SupplierPaymentTerm,
				SupplierTelephone = SupplierTelephone,
				SupplierFax = SupplierFax,
				SupplierEmail = SupplierEmail,
				CurrencyName = CurrencyName,
				IssuerName = ResponsableName,
				IssuerId = ResponsableId,
				CompanyName = CompanyName,
				CreationDate = OrderDate,
				OrderNumber = Number,
				OrderType = Type,
				Level = Level,
				Status = Status,

				ApprovalTime = ApprovalTime,
				ApprovalUserId = ApprovalUserId,
				LastRejectionLevel = LastRejectionLevel,
				LastRejectionTime = LastRejectionTime,
				LastRejectionUserId = LastRejectionUserId,

				Deleted = Deleted,
				DeleteTime = DeleteTime,
				DeleteUserId = DeleteUserId,
				DeliveryAddress = DeliveryAddress,
				DeliveryDepartmentId = DeliveryDepartmentId,
				DeliveryDepartmentName = DeliveryDepartmentName,
				DeliveryCompanyId = DeliveryCompanyId,
				DeliveryCompanyName = DeliveryCompanyName,
				DeliveryFax = DeliveryFax,
				DeliveryTelephone = DeliveryTelephone,

				Archived = Archived,
				ArchiveTime = ArchiveTime,
				ArchiveUserId = ArchiveUserId,
				StorageLocationId = StorageLocationId,
				StorageLocationName = StorageLocationName,
				Description = Description,
				LocationId = StorageLocationId,

				BillingContactName = BillingContactName ?? ResponsableName,
				BillingAddress = BillingAddress,
				BillingDepartmentId = BillingDepartmentId ?? DepartmentId,
				BillingDepartmentName = BillingDepartmentName ?? DepartmentName,
				BillingCompanyId = BillingCompanyId,
				BillingCompanyName = BillingCompanyName ?? CompanyName,
				BillingFax = BillingFax,
				BillingTelephone = BillingTelephone,


				// - 
				OrderPlacedEmailMessage = OrderPlacedEmailMessage,
				OrderPlacedEmailTitle = OrderPlacedEmailTitle,
				OrderPlacedReportFileId = OrderPlacedReportFileId,
				OrderPlacedSendingEmail = OrderPlacedSendingEmail,
				OrderPlacedSupplierEmail = OrderPlacedSupplierEmail,
				OrderPlacedTime = OrderPlacedTime,
				OrderPlacedUserEmail = OrderPlacedUserEmail,
				OrderPlacedUserId = OrderPlacedUserId,
				OrderPlacedUserName = OrderPlacedUserName,
				OrderPlacementCCEmail = OrderPlacementCCEmail,

				// - payment types - Purchase / Leasing
				PoPaymentType = PoPaymentType,
				PoPaymentTypeName = PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing
				? Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing.GetDescription()
				: Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase.GetDescription(),

				LeasingNbMonths = LeasingNbMonths,
				LeasingStartMonth = LeasingStartMonth,
				LeasingStartYear = LeasingStartYear,
				LeasingTotalAmount = LeasingTotalAmount,
				LeasingMonthAmount = LeasingMonthAmount,
				Discount = Discount,

				AllocationType = AllocationType ?? 0,
				AllocationTypeName = AllocationTypeName,
			};
		}
		public Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity ToBestellungenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity
			{
				AB_Nr_Lieferant = null,
				Abteilung = DepartmentName,
				Anfrage_Lieferfrist = null,
				Anrede = SupplierAnrede,
				Ansprechpartner = SupplierContact,
				Bearbeiter = ResponsableId,
				Belegkreis = null,
				Bemerkungen = Description,
				Benutzer = ResponsableName,
				best_id = null,
				Bestellbestatigung_erbeten_bis = DateTime.Now.AddDays(3),/// 
				Bestellung_Nr = Id_Order,
				Bezug = "",
				Briefanrede = SupplierSalutations,
				datueber = null,
				Datum = OrderDate,
				Eingangslieferscheinnr = "",
				Eingangsrechnungsnr = "",
				erledigt = false,
				Frachtfreigrenze = null,
				Freitext = "",
				gebucht = null,
				gedruckt = null,
				Ihr_Zeichen = "",
				In_Bearbeitung = null,
				Kanban = null,
				Konditionen = "",
				Kreditorennummer = "",
				Kundenbestellung = null,
				Land_PLZ_Ort = $"{SupplierCity} | {SupplierCountry}",
				Lieferanten_Nr = SupplierId,
				Liefertermin = null,
				Loschen = null,
				Mahnung = null,
				Mandant = CompanyName,
				Mindestbestellwert = null,
				Name2 = SupplierName2,
				Name3 = SupplierName3,
				Neu = null,
				Nr = Id_Order,
				nr_anf = null,
				nr_bes = null,
				nr_gut = null,
				nr_RB = null,
				nr_sto = null,
				nr_war = null,
				Offnen = null,
				Personal_Nr = null,
				Projekt_Nr = Id_Order.ToString(), // ProjectName,
				Rabatt = null,
				Rahmenbestellung = null,
				Straße_Postfach = $"{SupplierStreet} | {SupplierPostalCode}",
				Typ = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.ORDER_TYPE,
				Unser_Zeichen = null,
				USt = null,
				Versandart = null,
				Vorname_NameFirma = SupplierName, // >>>>>>>>>> Ridha
				Wahrung = null,
				Zahlungsweise = null,
				Zahlungsziel = null,
			};
		}
	}
}
