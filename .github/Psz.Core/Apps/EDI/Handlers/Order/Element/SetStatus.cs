using Newtonsoft.Json;
using System;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class OrderElementExtension
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
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Deleted;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
						return;
					}

					// Secondary Position  
					if(orderElement.PositionZUEDI != null /*&& orderElement.PositionZUEDI.Value > 0*/)
					{
						if(orderElement.Anzahl == orderElementExt.OriginalQuantity && orderElement.Liefertermin?.Date == orderElementExt.DesiredDate?.Date)
						{
							orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
							return;
						}
						else
						{
							orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
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
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
						return;
					}
					/// They both contain data
					if(splitPositions != null && splitPositions.Count > 0 && originalSplitPositions != null && originalSplitPositions.Count > 0)
					{
						/// Not number of items
						if(splitPositions.Count != originalSplitPositions.Count)
						{
							orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
							return;
						}

						/// Each item should have a match (from both sides)
						foreach(var item in splitPositions)
						{
							var match = originalSplitPositions.Find(x => x.OriginalQuantity == item.Anzahl && x.DesiredDate?.Date == item.Liefertermin?.Date);
							if(match == null)
							{
								orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
								Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
								return;
							}
						}
						foreach(var item in originalSplitPositions)
						{
							var match = splitPositions.Find(x => x.Anzahl == item.OriginalQuantity && x.Liefertermin?.Date == item.DesiredDate?.Date);
							if(match == null)
							{
								orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
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
							&& Tools.Converts.nearlyEqual((float)orderElementExt.OriginalGesamtpreis, (float)totalAmout, 0.001f)
							//&& orderElementExt.OriginalVKGesamtpreis == orderElement.VKGesamtpreis
							&& orderElementExt.DesiredDate?.Date == orderElement.Liefertermin?.Date)
					{
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
					}
					else
					{
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt);
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static void SetStatus(int orderElementId, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			try
			{
				var orderElement = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(orderElementId, botransaction.connection, botransaction.transaction);
				var totalAmout = orderElement.Gesamtpreis;
				if(orderElement == null)
				{
					return;
				}

				var orderElementExt = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderElementId(orderElementId, botransaction.connection, botransaction.transaction);
				if(orderElementExt != null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace,
						$"***** {JsonConvert.SerializeObject(orderElementExt)} // {JsonConvert.SerializeObject(orderElement)}");
					if(orderElement.Anzahl <= 0)
					{
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Deleted;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
						return;
					}

					// Secondary Position  
					if(orderElement.PositionZUEDI != null /*&& orderElement.PositionZUEDI.Value > 0*/)
					{
						if(orderElement.Anzahl == orderElementExt.OriginalQuantity && orderElement.Liefertermin?.Date == orderElementExt.DesiredDate?.Date)
						{
							orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
							return;
						}
						else
						{
							orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
							return;
						}
					}

					var splitPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetSplitPositions(orderElementId, botransaction.connection, botransaction.transaction);
					var originalSplitPositions = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetSecondaryByOrderId(orderElement.AngebotNr != null ? orderElement.AngebotNr.Value : -1,
						orderElement.Position != null ? orderElement.Position.Value : -1, botransaction.connection, botransaction.transaction);

					/// One set is empty and the other is not
					if(((splitPositions == null || splitPositions?.Count <= 0) && (originalSplitPositions != null && originalSplitPositions?.Count > 0))
						|| ((splitPositions != null && splitPositions?.Count > 0) && (originalSplitPositions == null || originalSplitPositions?.Count <= 0)))
					{
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
						return;
					}
					/// They both contain data
					if(splitPositions != null && splitPositions.Count > 0 && originalSplitPositions != null && originalSplitPositions.Count > 0)
					{
						/// Not number of items
						if(splitPositions.Count != originalSplitPositions.Count)
						{
							orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
							Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
							return;
						}

						/// Each item should have a match (from both sides)
						foreach(var item in splitPositions)
						{
							var match = originalSplitPositions.Find(x => x.OriginalQuantity == item.Anzahl && x.DesiredDate?.Date == item.Liefertermin?.Date);
							if(match == null)
							{
								orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
								Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
								return;
							}
						}
						foreach(var item in originalSplitPositions)
						{
							var match = splitPositions.Find(x => x.Anzahl == item.OriginalQuantity && x.Liefertermin?.Date == item.DesiredDate?.Date);
							if(match == null)
							{
								orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
								Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
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
							&& Tools.Converts.nearlyEqual((float)orderElementExt.OriginalGesamtpreis, (float)totalAmout, 0.001f)
							//&& orderElementExt.OriginalVKGesamtpreis == orderElement.VKGesamtpreis
							&& orderElementExt.DesiredDate?.Date == orderElement.Liefertermin?.Date)
					{
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
					}
					else
					{
						orderElementExt.Status = (int)Enums.OrderElementEnums.OrderElementStatus.Changed;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.UpdateStatus(orderElementExt, botransaction.connection, botransaction.transaction);
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
