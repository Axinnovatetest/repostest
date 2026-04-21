using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Helpers
{
	public class ItemElementHelper
	{
		internal static void SetStatus(int orderElementId)
		{
			try
			{
				var orderElement = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(orderElementId);
				var totalAmout = orderElement.Gesamtpreis;
				if(orderElement == null)
				{
					return;
				}

				var orderElementExt = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderElementId(orderElementId);
				if(orderElementExt != null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace,
						$"***** {JsonConvert.SerializeObject(orderElementExt)} // {JsonConvert.SerializeObject(orderElement)}");
					if(orderElement.Anzahl <= 0)
					{
						orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Deleted;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
						return;
					}

					// Secondary Position  
					if(orderElement.PositionZUEDI != null /*&& orderElement.PositionZUEDI.Value > 0*/)
					{
						if(orderElement.Anzahl == orderElementExt.OriginalQuantity && orderElement.Liefertermin?.Date == orderElementExt.DesiredDate?.Date)
						{
							orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Original;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
							return;
						}
						else
						{
							orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Changed;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
							return;
						}
					}

					var splitPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetSplitPositions(orderElementId);
					var originalSplitPositions = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetSecondaryByOrderId(orderElement.AngebotNr != null ? orderElement.AngebotNr.Value : -1,
						orderElement.Position != null ? orderElement.Position.Value : -1);

					/// One set is empty and the other is not
					if(((splitPositions == null || splitPositions?.Count <= 0) && (originalSplitPositions != null && originalSplitPositions?.Count > 0))
						|| ((splitPositions != null && splitPositions?.Count > 0) && (originalSplitPositions == null || originalSplitPositions?.Count <= 0)))
					{
						orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Changed;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
						return;
					}
					/// They both contain data
					if(splitPositions != null && splitPositions.Count > 0 && originalSplitPositions != null && originalSplitPositions.Count > 0)
					{
						/// Not number of items
						if(splitPositions.Count != originalSplitPositions.Count)
						{
							orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Changed;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
							return;
						}

						/// Each item should have a match (from both sides)
						foreach(var item in splitPositions)
						{
							var match = originalSplitPositions.Find(x => x.OriginalQuantity == item.Anzahl && x.DesiredDate?.Date == item.Liefertermin?.Date);
							if(match == null)
							{
								orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Changed;
								Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
								return;
							}
						}
						foreach(var item in originalSplitPositions)
						{
							var match = splitPositions.Find(x => x.Anzahl == item.OriginalQuantity && x.Liefertermin?.Date == item.DesiredDate?.Date);
							if(match == null)
							{
								orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Changed;
								Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
								return;
							}
						}
					}

					//
					if(splitPositions != null && splitPositions.Count > 0)
					{
						foreach(var item in splitPositions)
						{
							totalAmout += item.Gesamtpreis;
						}
					}

					if(orderElementExt.OriginalQuantity == orderElement.Anzahl
							&& SpecialHelper.nearlyEqual((float)orderElementExt.OriginalGesamtpreis, (float)totalAmout, 0.001f)
							//&& orderElementExt.OriginalVKGesamtpreis == orderElement.VKGesamtpreis
							&& orderElementExt.DesiredDate?.Date == orderElement.Liefertermin?.Date)
					{
						orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Original;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
					}
					else
					{
						orderElementExt.Status = (int)Enums.OrderEnums.OrderElementStatus.Changed;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static void UpdateLagerExtension(Core.Identity.Models.UserModel user, UpdateLagerExtensionModel data, Infrastructure.Services.Utils.TransactionsManager sqlTransaction = null)
		{
			UpdateLagerExtension(user, new List<UpdateLagerExtensionModel> { data }, sqlTransaction);

		}

		public static void UpdateLagerExtension(Core.Identity.Models.UserModel user, List<UpdateLagerExtensionModel> dataItems, Infrastructure.Services.Utils.TransactionsManager sqlTransaction = null)
		{
			try
			{
				foreach(var data in dataItems)
				{
					if(data.OldKundenIndex == data.NewKundenIndex)
					{
						//same index
						if(data.OldLagerorId == data.NewLagerorId)
						{
							//same index same lager
							if(data.OldBestand == data.NewBestand)
							{
								//same lager same index same bestand
							}
							else
							{
								//same lager same index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
						else
						{
							//same index diffrent lager
							if(data.OldBestand == data.NewBestand)
							{
								//diffrent lager same index same bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
							else
							{
								//diffrent lager same index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
					}
					else
					{
						//diffrent index
						if(data.OldLagerorId == data.NewLagerorId)
						{
							//diffrent index same lager
							if(data.OldBestand == data.NewBestand)
							{
								//diffrent lager same index same bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
							else
							{
								//diffrent lager same index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
						else
						{
							//diffrent index diffrent lager
							if(data.OldBestand == data.NewBestand)
							{
								//diffrent lager diffrent index same bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
							else
							{
								//diffrent lager diffrent index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
					}
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public class UpdateLagerExtensionModel
		{
			public int ArticleId { get; set; }
			public string OldKundenIndex { get; set; }
			public string NewKundenIndex { get; set; }
			public int OldLagerorId { get; set; }
			public int NewLagerorId { get; set; }
			public decimal OldBestand { get; set; }
			public decimal NewBestand { get; set; }
		}
	}
}
