using Psz.Core.BaseData.Handlers;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.SalesExtension.CustomPrice
{
	public class CustomPriceModel
	{
		public int Id { get; set; }
		public int ArticleId { get; set; }
		public int CustomPriceId { get; set; }
		public int? TypeId { get; set; }
		public string Type { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? StandardPrice { get; set; }
		public int? PricingGroup { get; set; }
		public decimal? ProductionTime { get; set; }
		public decimal? ProductionCost { get; set; }
		public decimal? HourlyRate { get; set; }
		public int? PackagingTypeId { get; set; }
		public string DeliveryTimeInWorkingDays { get; set; }
		public int? LotSize { get; set; }
		public string PackagingQuantity { get; set; }
		public string PackagingType { get; set; }
		// - 2025-11-28 - show DB/Marge - Khelil/Zipproth
		public decimal? DbWithCU { get; set; }
		public decimal? DbWithoutCU { get; set; }
		public decimal? MargeWithCU { get; set; }
		public decimal? MargeWithoutCU { get; set; }

		// -
		public decimal PrevPriceQuantity { get; set; }

		public CustomPriceModel()
		{

		}
		public CustomPriceModel(Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 staffelpreisKonditionzuordnungEntity,
			Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity preisgruppenEntity)
		{
			if(staffelpreisKonditionzuordnungEntity != null)
			{
				ArticleId = (int)staffelpreisKonditionzuordnungEntity.Artikel_Nr;
				CustomPriceId = staffelpreisKonditionzuordnungEntity.Nr_Staffel;
				ProductionTime = (decimal?)staffelpreisKonditionzuordnungEntity.ProduKtionzeit;
				ProductionCost = staffelpreisKonditionzuordnungEntity.Betrag;
				HourlyRate = (decimal?)staffelpreisKonditionzuordnungEntity.Stundensatz;
				//**
				DeliveryTimeInWorkingDays = staffelpreisKonditionzuordnungEntity.DeliveryTime;
				PackagingTypeId = staffelpreisKonditionzuordnungEntity.PackagingTypeId;
				PackagingType = staffelpreisKonditionzuordnungEntity.PackagingType;
				PackagingQuantity = staffelpreisKonditionzuordnungEntity.PackagingQuantity;
				TypeId = staffelpreisKonditionzuordnungEntity.TypeId;
				Type = staffelpreisKonditionzuordnungEntity.Type;
				LotSize = staffelpreisKonditionzuordnungEntity.LotSize;
				//**
				PricingGroup = 1;
				if(preisgruppenEntity != null)
				{
					switch(staffelpreisKonditionzuordnungEntity.Staffelpreis_Typ?.Replace(" ", "")?.ToLower())
					{
						case "s1":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis1;
								Quantity = (decimal?)preisgruppenEntity.ME1;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								if(string.IsNullOrWhiteSpace(Type) || !TypeId.HasValue)
								{
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S1;
									Type = Enums.ArticleEnums.CustomPriceType.S1.GetDescription();
								}
								break;
							}
						case "s2":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis2;
								Quantity = (decimal?)preisgruppenEntity.ME2;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								if(string.IsNullOrWhiteSpace(Type) || !TypeId.HasValue)
								{
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S2;
									Type = Enums.ArticleEnums.CustomPriceType.S2.GetDescription();
								}
								break;
							}
						case "s3":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis3;
								Quantity = (decimal?)preisgruppenEntity.ME3;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								if(string.IsNullOrWhiteSpace(Type) || !TypeId.HasValue)
								{
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S3;
									Type = Enums.ArticleEnums.CustomPriceType.S3.GetDescription();
								}
								break;
							}
						case "s4":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis4;
								Quantity = (decimal?)preisgruppenEntity.ME4;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								if(string.IsNullOrWhiteSpace(Type) || !TypeId.HasValue)
								{
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S4;
									Type = Enums.ArticleEnums.CustomPriceType.S4.GetDescription();
								}
								break;
							}
						default:
							break;
					}
				}
			}
		}
		public CustomPriceModel(Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity staffelpreisKonditionzuordnungEntity,
			Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity preisgruppenEntity,
			Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity staffelpreisExtensionEntity)
		{
			if(staffelpreisExtensionEntity == null)
			{
				return;
			}

			Id = staffelpreisExtensionEntity.Id;
			DeliveryTimeInWorkingDays = staffelpreisExtensionEntity.DeliveryTime;
			PricingGroup = 1; // Standard
			PackagingTypeId = staffelpreisExtensionEntity.PackagingTypeId;
			LotSize = staffelpreisExtensionEntity.LotSize;
			Type = staffelpreisExtensionEntity.Type;
			TypeId = staffelpreisExtensionEntity.TypeId;
			PackagingQuantity = staffelpreisExtensionEntity.PackagingQuantity;
			PackagingType = staffelpreisExtensionEntity.PackagingType;

			if(staffelpreisKonditionzuordnungEntity != null)
			{
				ArticleId = (int)staffelpreisKonditionzuordnungEntity.Artikel_Nr;
				CustomPriceId = staffelpreisKonditionzuordnungEntity.Nr_Staffel;
				ProductionTime = (decimal?)staffelpreisKonditionzuordnungEntity.ProduKtionzeit;
				ProductionCost = staffelpreisKonditionzuordnungEntity.Betrag;
				HourlyRate = (decimal?)staffelpreisKonditionzuordnungEntity.Stundensatz;

				if(preisgruppenEntity != null)
				{
					switch(staffelpreisKonditionzuordnungEntity.Staffelpreis_Typ.Replace(" ", "").ToLower())
					{
						case "s1":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis1;
								Quantity = (decimal?)preisgruppenEntity.ME1;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								break;
							}
						case "s2":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis2;
								Quantity = (decimal?)preisgruppenEntity.ME2;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								break;
							}
						case "s3":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis3;
								Quantity = (decimal?)preisgruppenEntity.ME3;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								break;
							}
						case "s4":
							{
								StandardPrice = (decimal?)preisgruppenEntity.Staffelpreis4;
								Quantity = (decimal?)preisgruppenEntity.ME4;
								ArticleId = (int)preisgruppenEntity.Artikel_Nr;
								break;
							}
						default:
							break;
					}
				}
			}
		}

		public Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.StaffelpreisExtensionEntity
			{
				Id = Id,
				DeliveryTime = DeliveryTimeInWorkingDays,
				LotSize = LotSize,
				PackagingTypeId = PackagingTypeId,
				StaffelNr = CustomPriceId,
				Type = Type,
				TypeId = TypeId,
				PackagingQuantity = PackagingQuantity,
				PackagingType = PackagingType
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 ToEntity_2()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
			{
				/*Id = Id,
                DeliveryTime = DeliveryTimeInWorkingDays,
                LotSize = LotSize,
                PackagingTypeId = PackagingTypeId,
                Nr_Staffel = CustomPriceId,
                Type = Type,
                TypeId = TypeId,
                PackagingQuantity = PackagingQuantity,
                PackagingType = PackagingType*/
				//**
				Artikel_Nr = ArticleId,
				Betrag = ProductionCost,
				DeliveryTime = DeliveryTimeInWorkingDays,
				Staffelpreis_Typ = Type?.Trim().Replace(" ", ""),
				Stundensatz = HourlyRate,
				LotSize = LotSize,
				PackagingTypeId = PackagingTypeId,
				PackagingType = PackagingType,
				PackagingQuantity = PackagingQuantity,
				TypeId = TypeId,
				Type = Type,
				Nr_Staffel = CustomPriceId,
				ProduKtionzeit = (double?)ProductionTime

			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity ToStaffelEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.StaffelpreisKonditionzuordnungEntity
			{
				Nr_Staffel = CustomPriceId,
				Artikel_Nr = ArticleId,
				ProduKtionzeit = ProductionTime,
				Betrag = ProductionCost,
				Stundensatz = HourlyRate,
				Staffelpreis_Typ = Type
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 ToStaffelEntity_2(
			Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2 prevEntity,
			UserModel user,
			Enums.ObjectLogEnums.Objects objectItem,
			int objectItemId,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs,
			Enums.ObjectLogEnums.LogType logType = Enums.ObjectLogEnums.LogType.Edit)
		{
			if(prevEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(prevEntity.Betrag != ProductionCost)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Betrag", $"{prevEntity.Betrag}",
									$"{ProductionCost}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.DeliveryTime != DeliveryTimeInWorkingDays)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"DeliveryTime", $"{prevEntity.DeliveryTime}",
									$"{DeliveryTimeInWorkingDays}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Staffelpreis_Typ?.ToLower()?.Trim().Replace(" ", "") != Type?.ToLower()?.Trim().Replace(" ", ""))
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Staffelpreis_Typ", $"{prevEntity.Staffelpreis_Typ}",
									$"{Type}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Stundensatz != HourlyRate)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Stundensatz", $"{prevEntity.Stundensatz}",
									$"{HourlyRate}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.LotSize != LotSize)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"LotSize", $"{prevEntity.LotSize}",
									$"{LotSize}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.PackagingType != PackagingType)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"PackagingType", $"{prevEntity.PackagingType}",
									$"{PackagingType}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.PackagingQuantity != PackagingQuantity)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"PackagingQuantity", $"{prevEntity.PackagingQuantity}",
									$"{PackagingQuantity}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ProduKtionzeit != (double?)ProductionTime)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ProduKtionzeit", $"{prevEntity.ProduKtionzeit}",
									$"{ProductionTime}", $"{objectItem.GetDescription()}", logType));
				}
			}
			// -
			return new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
			{
				Nr_Staffel = CustomPriceId,
				Artikel_Nr = ArticleId,
				ProduKtionzeit = (double?)ProductionTime,
				Betrag = ProductionCost,
				Stundensatz = HourlyRate,
				Staffelpreis_Typ = Type,
				///
				LotSize = LotSize,
				PackagingQuantity = PackagingQuantity,
				PackagingType = PackagingType,
				PackagingTypeId = PackagingTypeId,
				Type = Type,
				TypeId = TypeId,
				DeliveryTime = DeliveryTimeInWorkingDays
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity ToPreisgruppen()
		{
			switch((Enums.ArticleEnums.CustomPriceType)TypeId)
			{
				case Enums.ArticleEnums.CustomPriceType.S2:
					return new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity
					{
						Staffelpreis2 = StandardPrice,
						ME2 = Quantity,
						Preisgruppe = PricingGroup,
						Artikel_Nr = ArticleId
					};
				case Enums.ArticleEnums.CustomPriceType.S3:
					return new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity
					{
						Staffelpreis3 = StandardPrice,
						ME3 = Quantity,
						Preisgruppe = PricingGroup,
						Artikel_Nr = ArticleId
					};
				case Enums.ArticleEnums.CustomPriceType.S4:
					return new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity
					{
						Staffelpreis4 = StandardPrice,
						ME4 = Quantity,
						Preisgruppe = PricingGroup,
						Artikel_Nr = ArticleId
					};
				case Enums.ArticleEnums.CustomPriceType.S1:
				default:
					return new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity
					{
						Staffelpreis1 = StandardPrice,
						ME1 = Quantity,
						Preisgruppe = PricingGroup,
						Artikel_Nr = ArticleId
					};
			}
		}
	}
}
