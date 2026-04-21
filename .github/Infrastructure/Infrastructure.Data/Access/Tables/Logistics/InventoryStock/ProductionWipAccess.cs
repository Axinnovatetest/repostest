using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
	public class ProductionWipAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ProductionWip] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ProductionWip]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ProductionWip] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Inventory].[ProductionWip] ([ArticleAssembled],[ArticleCrimped],[ArticleCut],[ArticleElectricalInspected],[ArticleGesamtkosten],[ArticleMaterialkosten],[ArticleOpticalInspected],[ArticlePicked],[ArticlePreped],[ArtikelNr],[Due],[FA],[FaAssembled],[FaCrimped],[FaCut],[FaElectricalInspected],[FaOpticalInspected],[FaPicked],[FaPreped],[IdFa],[InventoryYear],[Item],[LagerId],[OpenQty],[UserAssembled],[UserAssembledPercent],[UserCrimped],[UserCrimpedPercent],[UserCut],[UserCutPercent],[UserElectricalInspected],[UserElectricalInspectedPercent],[UserOpticalInspected],[UserOpticalInspectedPercent],[UserPicked],[UserPickedPercent],[UserPreped],[UserPrepedPercent]) OUTPUT INSERTED.[Id] VALUES (@ArticleAssembled,@ArticleCrimped,@ArticleCut,@ArticleElectricalInspected,@ArticleGesamtkosten,@ArticleMaterialkosten,@ArticleOpticalInspected,@ArticlePicked,@ArticlePreped,@ArtikelNr,@Due,@FA,@FaAssembled,@FaCrimped,@FaCut,@FaElectricalInspected,@FaOpticalInspected,@FaPicked,@FaPreped,@IdFa,@InventoryYear,@Item,@LagerId,@OpenQty,@UserAssembled,@UserAssembledPercent,@UserCrimped,@UserCrimpedPercent,@UserCut,@UserCutPercent,@UserElectricalInspected,@UserElectricalInspectedPercent,@UserOpticalInspected,@UserOpticalInspectedPercent,@UserPicked,@UserPickedPercent,@UserPreped,@UserPrepedPercent); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleAssembled", item.ArticleAssembled);
					sqlCommand.Parameters.AddWithValue("ArticleCrimped", item.ArticleCrimped);
					sqlCommand.Parameters.AddWithValue("ArticleCut", item.ArticleCut);
					sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected", item.ArticleElectricalInspected);
					sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten", item.ArticleGesamtkosten);
					sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten", item.ArticleMaterialkosten);
					sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected", item.ArticleOpticalInspected);
					sqlCommand.Parameters.AddWithValue("ArticlePicked", item.ArticlePicked);
					sqlCommand.Parameters.AddWithValue("ArticlePreped", item.ArticlePreped);
					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Due", item.Due);
					sqlCommand.Parameters.AddWithValue("FA", item.FA);
					sqlCommand.Parameters.AddWithValue("FaAssembled", item.FaAssembled);
					sqlCommand.Parameters.AddWithValue("FaCrimped", item.FaCrimped);
					sqlCommand.Parameters.AddWithValue("FaCut", item.FaCut);
					sqlCommand.Parameters.AddWithValue("FaElectricalInspected", item.FaElectricalInspected);
					sqlCommand.Parameters.AddWithValue("FaOpticalInspected", item.FaOpticalInspected);
					sqlCommand.Parameters.AddWithValue("FaPicked", item.FaPicked);
					sqlCommand.Parameters.AddWithValue("FaPreped", item.FaPreped);
					sqlCommand.Parameters.AddWithValue("IdFa", item.IdFa);
					sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("Item", item.Item);
					sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
					sqlCommand.Parameters.AddWithValue("OpenQty", item.OpenQty);
					sqlCommand.Parameters.AddWithValue("UserAssembled", item.UserAssembled);
					sqlCommand.Parameters.AddWithValue("UserAssembledPercent", item.UserAssembledPercent);
					sqlCommand.Parameters.AddWithValue("UserCrimped", item.UserCrimped);
					sqlCommand.Parameters.AddWithValue("UserCrimpedPercent", item.UserCrimpedPercent);
					sqlCommand.Parameters.AddWithValue("UserCut", item.UserCut);
					sqlCommand.Parameters.AddWithValue("UserCutPercent", item.UserCutPercent);
					sqlCommand.Parameters.AddWithValue("UserElectricalInspected", item.UserElectricalInspected);
					sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent", item.UserElectricalInspectedPercent);
					sqlCommand.Parameters.AddWithValue("UserOpticalInspected", item.UserOpticalInspected);
					sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent", item.UserOpticalInspectedPercent);
					sqlCommand.Parameters.AddWithValue("UserPicked", item.UserPicked);
					sqlCommand.Parameters.AddWithValue("UserPickedPercent", item.UserPickedPercent);
					sqlCommand.Parameters.AddWithValue("UserPreped", item.UserPreped);
					sqlCommand.Parameters.AddWithValue("UserPrepedPercent", item.UserPrepedPercent);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 39; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Inventory].[ProductionWip] ([ArticleAssembled],[ArticleCrimped],[ArticleCut],[ArticleElectricalInspected],[ArticleGesamtkosten],[ArticleMaterialkosten],[ArticleOpticalInspected],[ArticlePicked],[ArticlePreped],[ArtikelNr],[Due],[FA],[FaAssembled],[FaCrimped],[FaCut],[FaElectricalInspected],[FaOpticalInspected],[FaPicked],[FaPreped],[IdFa],[InventoryYear],[Item],[LagerId],[OpenQty],[UserAssembled],[UserAssembledPercent],[UserCrimped],[UserCrimpedPercent],[UserCut],[UserCutPercent],[UserElectricalInspected],[UserElectricalInspectedPercent],[UserOpticalInspected],[UserOpticalInspectedPercent],[UserPicked],[UserPickedPercent],[UserPreped],[UserPrepedPercent]) VALUES ( "

							+ "@ArticleAssembled" + i + ","
							+ "@ArticleCrimped" + i + ","
							+ "@ArticleCut" + i + ","
							+ "@ArticleElectricalInspected" + i + ","
							+ "@ArticleGesamtkosten" + i + ","
							+ "@ArticleMaterialkosten" + i + ","
							+ "@ArticleOpticalInspected" + i + ","
							+ "@ArticlePicked" + i + ","
							+ "@ArticlePreped" + i + ","
							+ "@ArtikelNr" + i + ","
							+ "@Due" + i + ","
							+ "@FA" + i + ","
							+ "@FaAssembled" + i + ","
							+ "@FaCrimped" + i + ","
							+ "@FaCut" + i + ","
							+ "@FaElectricalInspected" + i + ","
							+ "@FaOpticalInspected" + i + ","
							+ "@FaPicked" + i + ","
							+ "@FaPreped" + i + ","
							+ "@IdFa" + i + ","
							+ "@InventoryYear" + i + ","
							+ "@Item" + i + ","
							+ "@LagerId" + i + ","
							+ "@OpenQty" + i + ","
							+ "@UserAssembled" + i + ","
							+ "@UserAssembledPercent" + i + ","
							+ "@UserCrimped" + i + ","
							+ "@UserCrimpedPercent" + i + ","
							+ "@UserCut" + i + ","
							+ "@UserCutPercent" + i + ","
							+ "@UserElectricalInspected" + i + ","
							+ "@UserElectricalInspectedPercent" + i + ","
							+ "@UserOpticalInspected" + i + ","
							+ "@UserOpticalInspectedPercent" + i + ","
							+ "@UserPicked" + i + ","
							+ "@UserPickedPercent" + i + ","
							+ "@UserPreped" + i + ","
							+ "@UserPrepedPercent" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleAssembled" + i, item.ArticleAssembled);
						sqlCommand.Parameters.AddWithValue("ArticleCrimped" + i, item.ArticleCrimped);
						sqlCommand.Parameters.AddWithValue("ArticleCut" + i, item.ArticleCut);
						sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected" + i, item.ArticleElectricalInspected);
						sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten" + i, item.ArticleGesamtkosten);
						sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten" + i, item.ArticleMaterialkosten);
						sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected" + i, item.ArticleOpticalInspected);
						sqlCommand.Parameters.AddWithValue("ArticlePicked" + i, item.ArticlePicked);
						sqlCommand.Parameters.AddWithValue("ArticlePreped" + i, item.ArticlePreped);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Due" + i, item.Due);
						sqlCommand.Parameters.AddWithValue("FA" + i, item.FA);
						sqlCommand.Parameters.AddWithValue("FaAssembled" + i, item.FaAssembled);
						sqlCommand.Parameters.AddWithValue("FaCrimped" + i, item.FaCrimped);
						sqlCommand.Parameters.AddWithValue("FaCut" + i, item.FaCut);
						sqlCommand.Parameters.AddWithValue("FaElectricalInspected" + i, item.FaElectricalInspected);
						sqlCommand.Parameters.AddWithValue("FaOpticalInspected" + i, item.FaOpticalInspected);
						sqlCommand.Parameters.AddWithValue("FaPicked" + i, item.FaPicked);
						sqlCommand.Parameters.AddWithValue("FaPreped" + i, item.FaPreped);
						sqlCommand.Parameters.AddWithValue("IdFa" + i, item.IdFa);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("Item" + i, item.Item);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("OpenQty" + i, item.OpenQty);
						sqlCommand.Parameters.AddWithValue("UserAssembled" + i, item.UserAssembled);
						sqlCommand.Parameters.AddWithValue("UserAssembledPercent" + i, item.UserAssembledPercent);
						sqlCommand.Parameters.AddWithValue("UserCrimped" + i, item.UserCrimped);
						sqlCommand.Parameters.AddWithValue("UserCrimpedPercent" + i, item.UserCrimpedPercent);
						sqlCommand.Parameters.AddWithValue("UserCut" + i, item.UserCut);
						sqlCommand.Parameters.AddWithValue("UserCutPercent" + i, item.UserCutPercent);
						sqlCommand.Parameters.AddWithValue("UserElectricalInspected" + i, item.UserElectricalInspected);
						sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent" + i, item.UserElectricalInspectedPercent);
						sqlCommand.Parameters.AddWithValue("UserOpticalInspected" + i, item.UserOpticalInspected);
						sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent" + i, item.UserOpticalInspectedPercent);
						sqlCommand.Parameters.AddWithValue("UserPicked" + i, item.UserPicked);
						sqlCommand.Parameters.AddWithValue("UserPickedPercent" + i, item.UserPickedPercent);
						sqlCommand.Parameters.AddWithValue("UserPreped" + i, item.UserPreped);
						sqlCommand.Parameters.AddWithValue("UserPrepedPercent" + i, item.UserPrepedPercent);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Inventory].[ProductionWip] SET [ArticleAssembled]=@ArticleAssembled, [ArticleCrimped]=@ArticleCrimped, [ArticleCut]=@ArticleCut, [ArticleElectricalInspected]=@ArticleElectricalInspected, [ArticleGesamtkosten]=@ArticleGesamtkosten, [ArticleMaterialkosten]=@ArticleMaterialkosten, [ArticleOpticalInspected]=@ArticleOpticalInspected, [ArticlePicked]=@ArticlePicked, [ArticlePreped]=@ArticlePreped, [ArtikelNr]=@ArtikelNr, [Due]=@Due, [FA]=@FA, [FaAssembled]=@FaAssembled, [FaCrimped]=@FaCrimped, [FaCut]=@FaCut, [FaElectricalInspected]=@FaElectricalInspected, [FaOpticalInspected]=@FaOpticalInspected, [FaPicked]=@FaPicked, [FaPreped]=@FaPreped, [IdFa]=@IdFa, [InventoryYear]=@InventoryYear, [Item]=@Item, [LagerId]=@LagerId, [OpenQty]=@OpenQty, [UserAssembled]=@UserAssembled, [UserAssembledPercent]=@UserAssembledPercent, [UserCrimped]=@UserCrimped, [UserCrimpedPercent]=@UserCrimpedPercent, [UserCut]=@UserCut, [UserCutPercent]=@UserCutPercent, [UserElectricalInspected]=@UserElectricalInspected, [UserElectricalInspectedPercent]=@UserElectricalInspectedPercent, [UserOpticalInspected]=@UserOpticalInspected, [UserOpticalInspectedPercent]=@UserOpticalInspectedPercent, [UserPicked]=@UserPicked, [UserPickedPercent]=@UserPickedPercent, [UserPreped]=@UserPreped, [UserPrepedPercent]=@UserPrepedPercent WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleAssembled", item.ArticleAssembled);
				sqlCommand.Parameters.AddWithValue("ArticleCrimped", item.ArticleCrimped);
				sqlCommand.Parameters.AddWithValue("ArticleCut", item.ArticleCut);
				sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected", item.ArticleElectricalInspected);
				sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten", item.ArticleGesamtkosten);
				sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten", item.ArticleMaterialkosten);
				sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected", item.ArticleOpticalInspected);
				sqlCommand.Parameters.AddWithValue("ArticlePicked", item.ArticlePicked);
				sqlCommand.Parameters.AddWithValue("ArticlePreped", item.ArticlePreped);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Due", item.Due);
				sqlCommand.Parameters.AddWithValue("FA", item.FA);
				sqlCommand.Parameters.AddWithValue("FaAssembled", item.FaAssembled);
				sqlCommand.Parameters.AddWithValue("FaCrimped", item.FaCrimped);
				sqlCommand.Parameters.AddWithValue("FaCut", item.FaCut);
				sqlCommand.Parameters.AddWithValue("FaElectricalInspected", item.FaElectricalInspected);
				sqlCommand.Parameters.AddWithValue("FaOpticalInspected", item.FaOpticalInspected);
				sqlCommand.Parameters.AddWithValue("FaPicked", item.FaPicked);
				sqlCommand.Parameters.AddWithValue("FaPreped", item.FaPreped);
				sqlCommand.Parameters.AddWithValue("IdFa", item.IdFa);
				sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
				sqlCommand.Parameters.AddWithValue("Item", item.Item);
				sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
				sqlCommand.Parameters.AddWithValue("OpenQty", item.OpenQty);
				sqlCommand.Parameters.AddWithValue("UserAssembled", item.UserAssembled);
				sqlCommand.Parameters.AddWithValue("UserAssembledPercent", item.UserAssembledPercent);
				sqlCommand.Parameters.AddWithValue("UserCrimped", item.UserCrimped);
				sqlCommand.Parameters.AddWithValue("UserCrimpedPercent", item.UserCrimpedPercent);
				sqlCommand.Parameters.AddWithValue("UserCut", item.UserCut);
				sqlCommand.Parameters.AddWithValue("UserCutPercent", item.UserCutPercent);
				sqlCommand.Parameters.AddWithValue("UserElectricalInspected", item.UserElectricalInspected);
				sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent", item.UserElectricalInspectedPercent);
				sqlCommand.Parameters.AddWithValue("UserOpticalInspected", item.UserOpticalInspected);
				sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent", item.UserOpticalInspectedPercent);
				sqlCommand.Parameters.AddWithValue("UserPicked", item.UserPicked);
				sqlCommand.Parameters.AddWithValue("UserPickedPercent", item.UserPickedPercent);
				sqlCommand.Parameters.AddWithValue("UserPreped", item.UserPreped);
				sqlCommand.Parameters.AddWithValue("UserPrepedPercent", item.UserPrepedPercent);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 39; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Inventory].[ProductionWip] SET "

							+ "[ArticleAssembled]=@ArticleAssembled" + i + ","
							+ "[ArticleCrimped]=@ArticleCrimped" + i + ","
							+ "[ArticleCut]=@ArticleCut" + i + ","
							+ "[ArticleElectricalInspected]=@ArticleElectricalInspected" + i + ","
							+ "[ArticleGesamtkosten]=@ArticleGesamtkosten" + i + ","
							+ "[ArticleMaterialkosten]=@ArticleMaterialkosten" + i + ","
							+ "[ArticleOpticalInspected]=@ArticleOpticalInspected" + i + ","
							+ "[ArticlePicked]=@ArticlePicked" + i + ","
							+ "[ArticlePreped]=@ArticlePreped" + i + ","
							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Due]=@Due" + i + ","
							+ "[FA]=@FA" + i + ","
							+ "[FaAssembled]=@FaAssembled" + i + ","
							+ "[FaCrimped]=@FaCrimped" + i + ","
							+ "[FaCut]=@FaCut" + i + ","
							+ "[FaElectricalInspected]=@FaElectricalInspected" + i + ","
							+ "[FaOpticalInspected]=@FaOpticalInspected" + i + ","
							+ "[FaPicked]=@FaPicked" + i + ","
							+ "[FaPreped]=@FaPreped" + i + ","
							+ "[IdFa]=@IdFa" + i + ","
							+ "[InventoryYear]=@InventoryYear" + i + ","
							+ "[Item]=@Item" + i + ","
							+ "[LagerId]=@LagerId" + i + ","
							+ "[OpenQty]=@OpenQty" + i + ","
							+ "[UserAssembled]=@UserAssembled" + i + ","
							+ "[UserAssembledPercent]=@UserAssembledPercent" + i + ","
							+ "[UserCrimped]=@UserCrimped" + i + ","
							+ "[UserCrimpedPercent]=@UserCrimpedPercent" + i + ","
							+ "[UserCut]=@UserCut" + i + ","
							+ "[UserCutPercent]=@UserCutPercent" + i + ","
							+ "[UserElectricalInspected]=@UserElectricalInspected" + i + ","
							+ "[UserElectricalInspectedPercent]=@UserElectricalInspectedPercent" + i + ","
							+ "[UserOpticalInspected]=@UserOpticalInspected" + i + ","
							+ "[UserOpticalInspectedPercent]=@UserOpticalInspectedPercent" + i + ","
							+ "[UserPicked]=@UserPicked" + i + ","
							+ "[UserPickedPercent]=@UserPickedPercent" + i + ","
							+ "[UserPreped]=@UserPreped" + i + ","
							+ "[UserPrepedPercent]=@UserPrepedPercent" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleAssembled" + i, item.ArticleAssembled);
						sqlCommand.Parameters.AddWithValue("ArticleCrimped" + i, item.ArticleCrimped);
						sqlCommand.Parameters.AddWithValue("ArticleCut" + i, item.ArticleCut);
						sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected" + i, item.ArticleElectricalInspected);
						sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten" + i, item.ArticleGesamtkosten);
						sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten" + i, item.ArticleMaterialkosten);
						sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected" + i, item.ArticleOpticalInspected);
						sqlCommand.Parameters.AddWithValue("ArticlePicked" + i, item.ArticlePicked);
						sqlCommand.Parameters.AddWithValue("ArticlePreped" + i, item.ArticlePreped);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Due" + i, item.Due);
						sqlCommand.Parameters.AddWithValue("FA" + i, item.FA);
						sqlCommand.Parameters.AddWithValue("FaAssembled" + i, item.FaAssembled);
						sqlCommand.Parameters.AddWithValue("FaCrimped" + i, item.FaCrimped);
						sqlCommand.Parameters.AddWithValue("FaCut" + i, item.FaCut);
						sqlCommand.Parameters.AddWithValue("FaElectricalInspected" + i, item.FaElectricalInspected);
						sqlCommand.Parameters.AddWithValue("FaOpticalInspected" + i, item.FaOpticalInspected);
						sqlCommand.Parameters.AddWithValue("FaPicked" + i, item.FaPicked);
						sqlCommand.Parameters.AddWithValue("FaPreped" + i, item.FaPreped);
						sqlCommand.Parameters.AddWithValue("IdFa" + i, item.IdFa);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("Item" + i, item.Item);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("OpenQty" + i, item.OpenQty);
						sqlCommand.Parameters.AddWithValue("UserAssembled" + i, item.UserAssembled);
						sqlCommand.Parameters.AddWithValue("UserAssembledPercent" + i, item.UserAssembledPercent);
						sqlCommand.Parameters.AddWithValue("UserCrimped" + i, item.UserCrimped);
						sqlCommand.Parameters.AddWithValue("UserCrimpedPercent" + i, item.UserCrimpedPercent);
						sqlCommand.Parameters.AddWithValue("UserCut" + i, item.UserCut);
						sqlCommand.Parameters.AddWithValue("UserCutPercent" + i, item.UserCutPercent);
						sqlCommand.Parameters.AddWithValue("UserElectricalInspected" + i, item.UserElectricalInspected);
						sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent" + i, item.UserElectricalInspectedPercent);
						sqlCommand.Parameters.AddWithValue("UserOpticalInspected" + i, item.UserOpticalInspected);
						sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent" + i, item.UserOpticalInspectedPercent);
						sqlCommand.Parameters.AddWithValue("UserPicked" + i, item.UserPicked);
						sqlCommand.Parameters.AddWithValue("UserPickedPercent" + i, item.UserPickedPercent);
						sqlCommand.Parameters.AddWithValue("UserPreped" + i, item.UserPreped);
						sqlCommand.Parameters.AddWithValue("UserPrepedPercent" + i, item.UserPrepedPercent);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Inventory].[ProductionWip] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [Inventory].[ProductionWip] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[ProductionWip] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[ProductionWip]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ProductionWip] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Inventory].[ProductionWip] ([ArticleAssembled],[ArticleCrimped],[ArticleCut],[ArticleElectricalInspected],[ArticleGesamtkosten],[ArticleMaterialkosten],[ArticleOpticalInspected],[ArticlePicked],[ArticlePreped],[ArtikelNr],[Due],[FA],[FaAssembled],[FaCrimped],[FaCut],[FaElectricalInspected],[FaOpticalInspected],[FaPicked],[FaPreped],[IdFa],[InventoryYear],[Item],[LagerId],[OpenQty],[UserAssembled],[UserAssembledPercent],[UserCrimped],[UserCrimpedPercent],[UserCut],[UserCutPercent],[UserElectricalInspected],[UserElectricalInspectedPercent],[UserOpticalInspected],[UserOpticalInspectedPercent],[UserPicked],[UserPickedPercent],[UserPreped],[UserPrepedPercent]) OUTPUT INSERTED.[Id] VALUES (@ArticleAssembled,@ArticleCrimped,@ArticleCut,@ArticleElectricalInspected,@ArticleGesamtkosten,@ArticleMaterialkosten,@ArticleOpticalInspected,@ArticlePicked,@ArticlePreped,@ArtikelNr,@Due,@FA,@FaAssembled,@FaCrimped,@FaCut,@FaElectricalInspected,@FaOpticalInspected,@FaPicked,@FaPreped,@IdFa,@InventoryYear,@Item,@LagerId,@OpenQty,@UserAssembled,@UserAssembledPercent,@UserCrimped,@UserCrimpedPercent,@UserCut,@UserCutPercent,@UserElectricalInspected,@UserElectricalInspectedPercent,@UserOpticalInspected,@UserOpticalInspectedPercent,@UserPicked,@UserPickedPercent,@UserPreped,@UserPrepedPercent); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleAssembled", item.ArticleAssembled);
			sqlCommand.Parameters.AddWithValue("ArticleCrimped", item.ArticleCrimped);
			sqlCommand.Parameters.AddWithValue("ArticleCut", item.ArticleCut);
			sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected", item.ArticleElectricalInspected);
			sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten", item.ArticleGesamtkosten);
			sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten", item.ArticleMaterialkosten);
			sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected", item.ArticleOpticalInspected);
			sqlCommand.Parameters.AddWithValue("ArticlePicked", item.ArticlePicked);
			sqlCommand.Parameters.AddWithValue("ArticlePreped", item.ArticlePreped);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Due", item.Due);
			sqlCommand.Parameters.AddWithValue("FA", item.FA);
			sqlCommand.Parameters.AddWithValue("FaAssembled", item.FaAssembled);
			sqlCommand.Parameters.AddWithValue("FaCrimped", item.FaCrimped);
			sqlCommand.Parameters.AddWithValue("FaCut", item.FaCut);
			sqlCommand.Parameters.AddWithValue("FaElectricalInspected", item.FaElectricalInspected);
			sqlCommand.Parameters.AddWithValue("FaOpticalInspected", item.FaOpticalInspected);
			sqlCommand.Parameters.AddWithValue("FaPicked", item.FaPicked);
			sqlCommand.Parameters.AddWithValue("FaPreped", item.FaPreped);
			sqlCommand.Parameters.AddWithValue("IdFa", item.IdFa);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("Item", item.Item);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("OpenQty", item.OpenQty);
			sqlCommand.Parameters.AddWithValue("UserAssembled", item.UserAssembled);
			sqlCommand.Parameters.AddWithValue("UserAssembledPercent", item.UserAssembledPercent);
			sqlCommand.Parameters.AddWithValue("UserCrimped", item.UserCrimped);
			sqlCommand.Parameters.AddWithValue("UserCrimpedPercent", item.UserCrimpedPercent);
			sqlCommand.Parameters.AddWithValue("UserCut", item.UserCut);
			sqlCommand.Parameters.AddWithValue("UserCutPercent", item.UserCutPercent);
			sqlCommand.Parameters.AddWithValue("UserElectricalInspected", item.UserElectricalInspected);
			sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent", item.UserElectricalInspectedPercent);
			sqlCommand.Parameters.AddWithValue("UserOpticalInspected", item.UserOpticalInspected);
			sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent", item.UserOpticalInspectedPercent);
			sqlCommand.Parameters.AddWithValue("UserPicked", item.UserPicked);
			sqlCommand.Parameters.AddWithValue("UserPickedPercent", item.UserPickedPercent);
			sqlCommand.Parameters.AddWithValue("UserPreped", item.UserPreped);
			sqlCommand.Parameters.AddWithValue("UserPrepedPercent", item.UserPrepedPercent);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 39; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Inventory].[ProductionWip] ([ArticleAssembled],[ArticleCrimped],[ArticleCut],[ArticleElectricalInspected],[ArticleGesamtkosten],[ArticleMaterialkosten],[ArticleOpticalInspected],[ArticlePicked],[ArticlePreped],[ArtikelNr],[Due],[FA],[FaAssembled],[FaCrimped],[FaCut],[FaElectricalInspected],[FaOpticalInspected],[FaPicked],[FaPreped],[IdFa],[InventoryYear],[Item],[LagerId],[OpenQty],[UserAssembled],[UserAssembledPercent],[UserCrimped],[UserCrimpedPercent],[UserCut],[UserCutPercent],[UserElectricalInspected],[UserElectricalInspectedPercent],[UserOpticalInspected],[UserOpticalInspectedPercent],[UserPicked],[UserPickedPercent],[UserPreped],[UserPrepedPercent]) VALUES ( "

						+ "@ArticleAssembled" + i + ","
						+ "@ArticleCrimped" + i + ","
						+ "@ArticleCut" + i + ","
						+ "@ArticleElectricalInspected" + i + ","
						+ "@ArticleGesamtkosten" + i + ","
						+ "@ArticleMaterialkosten" + i + ","
						+ "@ArticleOpticalInspected" + i + ","
						+ "@ArticlePicked" + i + ","
						+ "@ArticlePreped" + i + ","
						+ "@ArtikelNr" + i + ","
						+ "@Due" + i + ","
						+ "@FA" + i + ","
						+ "@FaAssembled" + i + ","
						+ "@FaCrimped" + i + ","
						+ "@FaCut" + i + ","
						+ "@FaElectricalInspected" + i + ","
						+ "@FaOpticalInspected" + i + ","
						+ "@FaPicked" + i + ","
						+ "@FaPreped" + i + ","
						+ "@IdFa" + i + ","
						+ "@InventoryYear" + i + ","
						+ "@Item" + i + ","
						+ "@LagerId" + i + ","
						+ "@OpenQty" + i + ","
						+ "@UserAssembled" + i + ","
						+ "@UserAssembledPercent" + i + ","
						+ "@UserCrimped" + i + ","
						+ "@UserCrimpedPercent" + i + ","
						+ "@UserCut" + i + ","
						+ "@UserCutPercent" + i + ","
						+ "@UserElectricalInspected" + i + ","
						+ "@UserElectricalInspectedPercent" + i + ","
						+ "@UserOpticalInspected" + i + ","
						+ "@UserOpticalInspectedPercent" + i + ","
						+ "@UserPicked" + i + ","
						+ "@UserPickedPercent" + i + ","
						+ "@UserPreped" + i + ","
						+ "@UserPrepedPercent" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleAssembled" + i, item.ArticleAssembled);
					sqlCommand.Parameters.AddWithValue("ArticleCrimped" + i, item.ArticleCrimped);
					sqlCommand.Parameters.AddWithValue("ArticleCut" + i, item.ArticleCut);
					sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected" + i, item.ArticleElectricalInspected);
					sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten" + i, item.ArticleGesamtkosten);
					sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten" + i, item.ArticleMaterialkosten);
					sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected" + i, item.ArticleOpticalInspected);
					sqlCommand.Parameters.AddWithValue("ArticlePicked" + i, item.ArticlePicked);
					sqlCommand.Parameters.AddWithValue("ArticlePreped" + i, item.ArticlePreped);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Due" + i, item.Due);
					sqlCommand.Parameters.AddWithValue("FA" + i, item.FA);
					sqlCommand.Parameters.AddWithValue("FaAssembled" + i, item.FaAssembled);
					sqlCommand.Parameters.AddWithValue("FaCrimped" + i, item.FaCrimped);
					sqlCommand.Parameters.AddWithValue("FaCut" + i, item.FaCut);
					sqlCommand.Parameters.AddWithValue("FaElectricalInspected" + i, item.FaElectricalInspected);
					sqlCommand.Parameters.AddWithValue("FaOpticalInspected" + i, item.FaOpticalInspected);
					sqlCommand.Parameters.AddWithValue("FaPicked" + i, item.FaPicked);
					sqlCommand.Parameters.AddWithValue("FaPreped" + i, item.FaPreped);
					sqlCommand.Parameters.AddWithValue("IdFa" + i, item.IdFa);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("Item" + i, item.Item);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("OpenQty" + i, item.OpenQty);
					sqlCommand.Parameters.AddWithValue("UserAssembled" + i, item.UserAssembled);
					sqlCommand.Parameters.AddWithValue("UserAssembledPercent" + i, item.UserAssembledPercent);
					sqlCommand.Parameters.AddWithValue("UserCrimped" + i, item.UserCrimped);
					sqlCommand.Parameters.AddWithValue("UserCrimpedPercent" + i, item.UserCrimpedPercent);
					sqlCommand.Parameters.AddWithValue("UserCut" + i, item.UserCut);
					sqlCommand.Parameters.AddWithValue("UserCutPercent" + i, item.UserCutPercent);
					sqlCommand.Parameters.AddWithValue("UserElectricalInspected" + i, item.UserElectricalInspected);
					sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent" + i, item.UserElectricalInspectedPercent);
					sqlCommand.Parameters.AddWithValue("UserOpticalInspected" + i, item.UserOpticalInspected);
					sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent" + i, item.UserOpticalInspectedPercent);
					sqlCommand.Parameters.AddWithValue("UserPicked" + i, item.UserPicked);
					sqlCommand.Parameters.AddWithValue("UserPickedPercent" + i, item.UserPickedPercent);
					sqlCommand.Parameters.AddWithValue("UserPreped" + i, item.UserPreped);
					sqlCommand.Parameters.AddWithValue("UserPrepedPercent" + i, item.UserPrepedPercent);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Inventory].[ProductionWip] SET [ArticleAssembled]=@ArticleAssembled, [ArticleCrimped]=@ArticleCrimped, [ArticleCut]=@ArticleCut, [ArticleElectricalInspected]=@ArticleElectricalInspected, [ArticleGesamtkosten]=@ArticleGesamtkosten, [ArticleMaterialkosten]=@ArticleMaterialkosten, [ArticleOpticalInspected]=@ArticleOpticalInspected, [ArticlePicked]=@ArticlePicked, [ArticlePreped]=@ArticlePreped, [ArtikelNr]=@ArtikelNr, [Due]=@Due, [FA]=@FA, [FaAssembled]=@FaAssembled, [FaCrimped]=@FaCrimped, [FaCut]=@FaCut, [FaElectricalInspected]=@FaElectricalInspected, [FaOpticalInspected]=@FaOpticalInspected, [FaPicked]=@FaPicked, [FaPreped]=@FaPreped, [IdFa]=@IdFa, [InventoryYear]=@InventoryYear, [Item]=@Item, [LagerId]=@LagerId, [OpenQty]=@OpenQty, [UserAssembled]=@UserAssembled, [UserAssembledPercent]=@UserAssembledPercent, [UserCrimped]=@UserCrimped, [UserCrimpedPercent]=@UserCrimpedPercent, [UserCut]=@UserCut, [UserCutPercent]=@UserCutPercent, [UserElectricalInspected]=@UserElectricalInspected, [UserElectricalInspectedPercent]=@UserElectricalInspectedPercent, [UserOpticalInspected]=@UserOpticalInspected, [UserOpticalInspectedPercent]=@UserOpticalInspectedPercent, [UserPicked]=@UserPicked, [UserPickedPercent]=@UserPickedPercent, [UserPreped]=@UserPreped, [UserPrepedPercent]=@UserPrepedPercent WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleAssembled", item.ArticleAssembled);
			sqlCommand.Parameters.AddWithValue("ArticleCrimped", item.ArticleCrimped);
			sqlCommand.Parameters.AddWithValue("ArticleCut", item.ArticleCut);
			sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected", item.ArticleElectricalInspected);
			sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten", item.ArticleGesamtkosten);
			sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten", item.ArticleMaterialkosten);
			sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected", item.ArticleOpticalInspected);
			sqlCommand.Parameters.AddWithValue("ArticlePicked", item.ArticlePicked);
			sqlCommand.Parameters.AddWithValue("ArticlePreped", item.ArticlePreped);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Due", item.Due);
			sqlCommand.Parameters.AddWithValue("FA", item.FA);
			sqlCommand.Parameters.AddWithValue("FaAssembled", item.FaAssembled);
			sqlCommand.Parameters.AddWithValue("FaCrimped", item.FaCrimped);
			sqlCommand.Parameters.AddWithValue("FaCut", item.FaCut);
			sqlCommand.Parameters.AddWithValue("FaElectricalInspected", item.FaElectricalInspected);
			sqlCommand.Parameters.AddWithValue("FaOpticalInspected", item.FaOpticalInspected);
			sqlCommand.Parameters.AddWithValue("FaPicked", item.FaPicked);
			sqlCommand.Parameters.AddWithValue("FaPreped", item.FaPreped);
			sqlCommand.Parameters.AddWithValue("IdFa", item.IdFa);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("Item", item.Item);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("OpenQty", item.OpenQty);
			sqlCommand.Parameters.AddWithValue("UserAssembled", item.UserAssembled);
			sqlCommand.Parameters.AddWithValue("UserAssembledPercent", item.UserAssembledPercent);
			sqlCommand.Parameters.AddWithValue("UserCrimped", item.UserCrimped);
			sqlCommand.Parameters.AddWithValue("UserCrimpedPercent", item.UserCrimpedPercent);
			sqlCommand.Parameters.AddWithValue("UserCut", item.UserCut);
			sqlCommand.Parameters.AddWithValue("UserCutPercent", item.UserCutPercent);
			sqlCommand.Parameters.AddWithValue("UserElectricalInspected", item.UserElectricalInspected);
			sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent", item.UserElectricalInspectedPercent);
			sqlCommand.Parameters.AddWithValue("UserOpticalInspected", item.UserOpticalInspected);
			sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent", item.UserOpticalInspectedPercent);
			sqlCommand.Parameters.AddWithValue("UserPicked", item.UserPicked);
			sqlCommand.Parameters.AddWithValue("UserPickedPercent", item.UserPickedPercent);
			sqlCommand.Parameters.AddWithValue("UserPreped", item.UserPreped);
			sqlCommand.Parameters.AddWithValue("UserPrepedPercent", item.UserPrepedPercent);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 39; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Inventory].[ProductionWip] SET "

					+ "[ArticleAssembled]=@ArticleAssembled" + i + ","
					+ "[ArticleCrimped]=@ArticleCrimped" + i + ","
					+ "[ArticleCut]=@ArticleCut" + i + ","
					+ "[ArticleElectricalInspected]=@ArticleElectricalInspected" + i + ","
					+ "[ArticleGesamtkosten]=@ArticleGesamtkosten" + i + ","
					+ "[ArticleMaterialkosten]=@ArticleMaterialkosten" + i + ","
					+ "[ArticleOpticalInspected]=@ArticleOpticalInspected" + i + ","
					+ "[ArticlePicked]=@ArticlePicked" + i + ","
					+ "[ArticlePreped]=@ArticlePreped" + i + ","
					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[Due]=@Due" + i + ","
					+ "[FA]=@FA" + i + ","
					+ "[FaAssembled]=@FaAssembled" + i + ","
					+ "[FaCrimped]=@FaCrimped" + i + ","
					+ "[FaCut]=@FaCut" + i + ","
					+ "[FaElectricalInspected]=@FaElectricalInspected" + i + ","
					+ "[FaOpticalInspected]=@FaOpticalInspected" + i + ","
					+ "[FaPicked]=@FaPicked" + i + ","
					+ "[FaPreped]=@FaPreped" + i + ","
					+ "[IdFa]=@IdFa" + i + ","
					+ "[InventoryYear]=@InventoryYear" + i + ","
					+ "[Item]=@Item" + i + ","
					+ "[LagerId]=@LagerId" + i + ","
					+ "[OpenQty]=@OpenQty" + i + ","
					+ "[UserAssembled]=@UserAssembled" + i + ","
					+ "[UserAssembledPercent]=@UserAssembledPercent" + i + ","
					+ "[UserCrimped]=@UserCrimped" + i + ","
					+ "[UserCrimpedPercent]=@UserCrimpedPercent" + i + ","
					+ "[UserCut]=@UserCut" + i + ","
					+ "[UserCutPercent]=@UserCutPercent" + i + ","
					+ "[UserElectricalInspected]=@UserElectricalInspected" + i + ","
					+ "[UserElectricalInspectedPercent]=@UserElectricalInspectedPercent" + i + ","
					+ "[UserOpticalInspected]=@UserOpticalInspected" + i + ","
					+ "[UserOpticalInspectedPercent]=@UserOpticalInspectedPercent" + i + ","
					+ "[UserPicked]=@UserPicked" + i + ","
					+ "[UserPickedPercent]=@UserPickedPercent" + i + ","
					+ "[UserPreped]=@UserPreped" + i + ","
					+ "[UserPrepedPercent]=@UserPrepedPercent" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleAssembled" + i, item.ArticleAssembled);
					sqlCommand.Parameters.AddWithValue("ArticleCrimped" + i, item.ArticleCrimped);
					sqlCommand.Parameters.AddWithValue("ArticleCut" + i, item.ArticleCut);
					sqlCommand.Parameters.AddWithValue("ArticleElectricalInspected" + i, item.ArticleElectricalInspected);
					sqlCommand.Parameters.AddWithValue("ArticleGesamtkosten" + i, item.ArticleGesamtkosten);
					sqlCommand.Parameters.AddWithValue("ArticleMaterialkosten" + i, item.ArticleMaterialkosten);
					sqlCommand.Parameters.AddWithValue("ArticleOpticalInspected" + i, item.ArticleOpticalInspected);
					sqlCommand.Parameters.AddWithValue("ArticlePicked" + i, item.ArticlePicked);
					sqlCommand.Parameters.AddWithValue("ArticlePreped" + i, item.ArticlePreped);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Due" + i, item.Due);
					sqlCommand.Parameters.AddWithValue("FA" + i, item.FA);
					sqlCommand.Parameters.AddWithValue("FaAssembled" + i, item.FaAssembled);
					sqlCommand.Parameters.AddWithValue("FaCrimped" + i, item.FaCrimped);
					sqlCommand.Parameters.AddWithValue("FaCut" + i, item.FaCut);
					sqlCommand.Parameters.AddWithValue("FaElectricalInspected" + i, item.FaElectricalInspected);
					sqlCommand.Parameters.AddWithValue("FaOpticalInspected" + i, item.FaOpticalInspected);
					sqlCommand.Parameters.AddWithValue("FaPicked" + i, item.FaPicked);
					sqlCommand.Parameters.AddWithValue("FaPreped" + i, item.FaPreped);
					sqlCommand.Parameters.AddWithValue("IdFa" + i, item.IdFa);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("Item" + i, item.Item);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("OpenQty" + i, item.OpenQty);
					sqlCommand.Parameters.AddWithValue("UserAssembled" + i, item.UserAssembled);
					sqlCommand.Parameters.AddWithValue("UserAssembledPercent" + i, item.UserAssembledPercent);
					sqlCommand.Parameters.AddWithValue("UserCrimped" + i, item.UserCrimped);
					sqlCommand.Parameters.AddWithValue("UserCrimpedPercent" + i, item.UserCrimpedPercent);
					sqlCommand.Parameters.AddWithValue("UserCut" + i, item.UserCut);
					sqlCommand.Parameters.AddWithValue("UserCutPercent" + i, item.UserCutPercent);
					sqlCommand.Parameters.AddWithValue("UserElectricalInspected" + i, item.UserElectricalInspected);
					sqlCommand.Parameters.AddWithValue("UserElectricalInspectedPercent" + i, item.UserElectricalInspectedPercent);
					sqlCommand.Parameters.AddWithValue("UserOpticalInspected" + i, item.UserOpticalInspected);
					sqlCommand.Parameters.AddWithValue("UserOpticalInspectedPercent" + i, item.UserOpticalInspectedPercent);
					sqlCommand.Parameters.AddWithValue("UserPicked" + i, item.UserPicked);
					sqlCommand.Parameters.AddWithValue("UserPickedPercent" + i, item.UserPickedPercent);
					sqlCommand.Parameters.AddWithValue("UserPreped" + i, item.UserPreped);
					sqlCommand.Parameters.AddWithValue("UserPrepedPercent" + i, item.UserPrepedPercent);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Inventory].[ProductionWip] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [Inventory].[ProductionWip] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int CountWipProdData(string? filterSearch, int? LagerId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				// Base query with LagerId filter
				var query = "SELECT COUNT(*) FROM [Inventory].[ProductionWip] WHERE LagerId = @LagerId";

				// Optional search filter
				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @"
                AND (
                    FA LIKE @filterSearch
                    OR Item LIKE @filterSearch
                    OR CAST(OpenQty AS NVARCHAR) LIKE @filterSearch
                    OR CAST(OInspected AS NVARCHAR) LIKE @filterSearch
                )";
				}

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.Add("@LagerId", SqlDbType.Int).Value = LagerId ?? 0;

					if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
						sqlCommand.Parameters.Add("@filterSearch", SqlDbType.NVarChar, 100).Value = $"%{filterSearch}%";

					sqlCommand.CommandTimeout = 300;
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var val) ? val : 0;
				}
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> GetWipProdData(
		string? filterSearch, int? LagerId, Settings.SortingModel? dataSorting, Settings.PaginModel? dataPaging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				// Base query with LagerId filter
				var query = "SELECT * FROM [Inventory].[ProductionWip] WHERE LagerId = @LagerId";
				var isFirstClause = true;

				// Optional search filter
				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @"
                AND (
                    FA LIKE @filterSearch
                    OR Item LIKE @filterSearch
                    OR CAST(OpenQty AS NVARCHAR) LIKE @filterSearch
                    OR CAST(OInspected AS NVARCHAR) LIKE @filterSearch
                )
            ";
					isFirstClause = false;
				}

				// Sorting
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")}";
				}
				else
				{
					query += " ORDER BY FA DESC";
				}

				// Paging
				if(dataPaging != null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY";
				}

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.Add("@LagerId", SqlDbType.Int).Value = LagerId ?? 0;

					if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
						sqlCommand.Parameters.Add("@filterSearch", SqlDbType.NVarChar, 100).Value = $"%{filterSearch}%";

					DbExecution.Fill(sqlCommand, dataTable);
				}
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();

			}

		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> GetWipP2rodDataForXls(int warehouseId)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT * FROM [Inventory].[ProductionWip] WHERE ISNULL([UserElectricalInspectedPercent],0)=0 AND [LagerId] in(@lagerId,IIF(@lagerId=42,7,-1)) ORDER BY [ArtikelNr];";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerId", warehouseId);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> GetXlsWipProdData(int warehouseId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT * FROM  [Inventory].[ProductionWip] WHERE [LagerId]=@lagerId 
					AND [FaPicked]>=0 AND [FaCut]>=0 AND [FaPreped]>=0 AND [FaAssembled]>=0 AND [FaCrimped]>=0 AND [FaOpticalInspected]>=0 AND [FaElectricalInspected]>=0
					ORDER BY ArtikelNr, FA";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerId", warehouseId);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();

			}

		}
		public static int InitializeWipTable(int lagerId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"SET NOCOUNT ON;
							-- ======================================================
							-- Step 2: Prep Fa & article
							-- ======================================================
							IF OBJECT_ID('tempdb..#Materialkosten') IS NOT NULL DROP TABLE #Materialkosten;
							CREATE TABLE #Materialkosten (
								[Artikelnummer] NVARCHAR(50), 
								/*[Bezeichnung] NVARCHAR(250), */
								[Verkaufspreis] DECIMAL(27,2), 
								[Materialkosten] DECIMAL(27,2), 
								[Arbeitskosten] DECIMAL(27,2),
								[FertigungType] NVARCHAR(50)
							);

							;WITH FaArticles AS (SELECT DISTINCT Artikel_Nr, FertigungType FROM Fertigung WHERE ISNULL(FA_Gestartet,0)=1 
											AND FA_begonnen IS NOT NULL 
											AND kennzeichen='offen' 
											AND Lagerort_id in(@lagerId,IIF(@lagerId=42,7,-1))
											)
							INSERT INTO #Materialkosten([Artikelnummer], [Verkaufspreis], [Materialkosten], [Arbeitskosten], [FertigungType])
							SELECT DISTINCT
											a.[Artikelnummer], 
											AVG(ISNULL(e.Verkaufspreis,0)) AS [Verkaufspreis], 
											AVG(ISNULL(e.Einkaufspreis,0)) AS [Materialkosten], 
											AVG(ISNULL(TRY_CAST(ISNULL(e.Produktionskosten,0) AS DECIMAL(27,5)),0)) AS [Arbeitskosten],
											[FertigungType]
							FROM Artikel a 
							JOIN FaArticles f
											ON a.[Artikel-Nr] = f.Artikel_Nr
							LEFT JOIN __BSD_ArtikelSalesExtension e 
											ON e.ArticleNr = a.[Artikel-Nr] 
											AND e.ArticleSalesType = f.FertigungType
							GROUP BY a.Artikelnummer,FertigungType
							ORDER BY a.Artikelnummer;

							-- ======================================================
							-- Step 3: Build dynamic columns and insert into #Stammdaten
							-- ======================================================
							INSERT INTO [Inventory].[ProductionWip] ([ArtikelNr],[Item],[IdFa],[FA],[OpenQty],[Due],[LagerId],
											[ArticleGesamtkosten],[ArticleMaterialkosten],[InventoryYear],
											[ArticlePicked],[ArticleCut],[ArticlePreped],[ArticleAssembled],[ArticleCrimped],[ArticleOpticalInspected],[ArticleElectricalInspected],
											[UserPicked],[UserCut],[UserPreped],[UserAssembled],[UserCrimped],[UserOpticalInspected],[UserElectricalInspected],
											[UserPickedPercent],[UserCutPercent],[UserPrepedPercent],[UserAssembledPercent],[UserCrimpedPercent],[UserOpticalInspectedPercent],[UserElectricalInspectedPercent],
											[FaPicked],[FaCut],[FaPreped],[FaAssembled],[FaCrimped],[FaOpticalInspected],[FaElectricalInspected]) 
							OUTPUT INSERTED.[Id]
							SELECT 
								/*
								a.[Bezeichnung 1],*/
								a.[Artikel-Nr],
								a.Artikelnummer,
								f.ID, 
								f.Fertigungsnummer, 
								f.Anzahl AS [OpenQty], 
								f.Termin_Bestätigt1 FertigungDate,
								f.Lagerort_id,
								ISNULL(m.Materialkosten,0) + ISNULL(m.Arbeitskosten,0) AS Gesamt,
								ISNULL(m.Materialkosten,0) AS Materialkosten,
								GETDATE() AS [InventoryYear],
								0 [ArticlePicked],0 [ArticleCut],0 [ArticlePreped],0 [ArticleAssembled],0 [ArticleCrimped],0 [ArticleOpticalInspected],0 [ArticleElectricalInspected],
								0 [UserPicked],0 [UserCut],0 [UserPreped],0 [UserAssembled],0 [UserCrimped],0 [UserOpticalInspected],0 [UserElectricalInspected],
								0 [UserPickedPercent],0 [UserCutPercent],0 [UserPrepedPercent],0 [UserAssembledPercent],0 [UserCrimpedPercent],0 [UserOpticalInspectedPercent],0 [UserElectricalInspectedPercent],
								0 [FaPicked],0 [FaCut],0 [FaPreped],0 [FaAssembled],0 [FaCrimped],0 [FaOpticalInspected],0 [FaElectricalInspected]

							FROM Fertigung f
							LEFT JOIN Artikel a ON a.[Artikel-Nr] = f.Artikel_Nr
							LEFT JOIN #Materialkosten m ON m.Artikelnummer=a.Artikelnummer AND m.FertigungType=f.FertigungType
							WHERE ISNULL(f.FA_Gestartet,0)=1 
								/*AND f.FA_begonnen IS NOT NULL */
								AND f.kennzeichen='offen'
								AND f.Lagerort_id in(@lagerId,IIF(@lagerId=42,7,-1))
							ORDER BY [Artikel-Nr];
				

							-- ======================================================
							-- Step 4: Update Article Stammdaten
							-- ======================================================
							UPDATE [Inventory].[ProductionWip] SET [ArticlePicked] = [ArticleGesamtkosten] * (SELECT [Percent] / 100.0 FROM [Inventory].[WorkArea] WHERE [Step]='Kommissionierung' AND [LagerId]=@lagerId);
							UPDATE [Inventory].[ProductionWip] SET [ArticleCut] = [ArticleGesamtkosten] * (SELECT [Percent] / 100.0 FROM [Inventory].[WorkArea] WHERE [Step]='Schneiderei' AND [LagerId]=@lagerId);
							UPDATE [Inventory].[ProductionWip] SET [ArticlePreped] = [ArticleGesamtkosten] * (SELECT [Percent] / 100.0 FROM [Inventory].[WorkArea] WHERE [Step]='Vorbereitung' AND [LagerId]=@lagerId);
							UPDATE [Inventory].[ProductionWip] SET [ArticleAssembled] = [ArticleGesamtkosten] * (SELECT [Percent] / 100.0 FROM [Inventory].[WorkArea] WHERE [Step]='Montage' AND [LagerId]=@lagerId);
							UPDATE [Inventory].[ProductionWip] SET [ArticleCrimped] = [ArticleGesamtkosten] * (SELECT [Percent] / 100.0 FROM [Inventory].[WorkArea] WHERE [Step]='Krimp' AND [LagerId]=@lagerId);
							UPDATE [Inventory].[ProductionWip] SET [ArticleOpticalInspected] = [ArticleGesamtkosten] * (SELECT [Percent] / 100.0 FROM [Inventory].[WorkArea] WHERE [Step]='Optische Kontrolle' AND [LagerId]=@lagerId);
							UPDATE [Inventory].[ProductionWip] SET [ArticleElectricalInspected] = [ArticleGesamtkosten] * (SELECT [Percent] / 100.0 FROM [Inventory].[WorkArea] WHERE [Step]='Elektrische Kontrolle' AND [LagerId]=@lagerId);
							

							-- ======================================================
							-- Step 5: Update complete Pick & Cut fertigung
							-- ======================================================
							UPDATE w SET UserCut=100, UserCutPercent=100 FROM [Inventory].[ProductionWip] w JOIN Fertigung f on f.ID=w.IdFa WHERE ISNULL(f.Kabel_geschnitten,0)=1 AND w.LagerId=@lagerId;
							UPDATE w SET UserPicked=100, UserPickedPercent=100 FROM [Inventory].[ProductionWip] w JOIN Fertigung f on f.ID=w.IdFa WHERE ISNULL(f.Kommisioniert_komplett,0)=1 AND w.LagerId=@lagerId;

							SELECT SCOPE_IDENTITY();
				";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("lagerId", lagerId);
			
			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> GetByFa(IEnumerable<int> ids)
		{
			if(ids != null && ids.Count() > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> results = null;
				if(ids.Count() <= maxQueryNumber)
				{
					results = getByFa(ids);
				}
				else
				{
					int batchNumber = ids.Count() / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByFa(ids.Skip(i * maxQueryNumber).Take(maxQueryNumber)));
					}
					results.AddRange(getByFa(ids.Skip(batchNumber * maxQueryNumber).Take(ids.Count() - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> getByFa(IEnumerable<int> ids)
		{
			if(ids != null && ids.Count() > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count(); i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids.ElementAt(i));
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ProductionWip] WHERE [FA] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
		}
		public static int UpdatePercent(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> items, bool isVersioning, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = $@"
					/*-- updated Values --- */
					IF OBJECT_ID('tempdb..#updatedValues') IS NOT NULL DROP TABLE #updatedValues;
					CREATE TABLE #updatedValues(
							[Fertigungsnummer]  INT,
							[Picked] DECIMAL(7,2),
							[Cut] DECIMAL(7,2),
							[Preped] DECIMAL(7,2),
							[Assembled] DECIMAL(7,2),
							[Crimped] DECIMAL(7,2),
							[OpticalInsp] DECIMAL(7,2),
							[ElectricalInsp] DECIMAL(7,2),
							[PickedPercent] DECIMAL(7,2),
							[CutPercent] DECIMAL(7,2),
							[PrepedPercent] DECIMAL(7,2),
							[AssembledPercent] DECIMAL(7,2),
							[CrimpedPercent] DECIMAL(7,2),
							[OpticalInspPercent] DECIMAL(7,2),
							[ElectricalInspPercent] DECIMAL(7,2)
							);
					INSERT INTO #updatedValues([Fertigungsnummer], [Picked], [Cut], [Preped], [Assembled], [Crimped], [OpticalInsp], [ElectricalInsp],
							[PickedPercent], [CutPercent], [PrepedPercent], [AssembledPercent], [CrimpedPercent], [OpticalInspPercent], [ElectricalInspPercent]) VALUES
					  ({string.Join("),(", items.Select(x=> $@"{x.FA},{x.UserPicked.ToString(CultureInfo.InvariantCulture)},{x.UserCut.ToString(CultureInfo.InvariantCulture)},{x.UserPreped.ToString(CultureInfo.InvariantCulture)},{x.UserAssembled.ToString(CultureInfo.InvariantCulture)},{x.UserCrimped.ToString(CultureInfo.InvariantCulture)},{x.UserOpticalInspected.ToString(CultureInfo.InvariantCulture)},{x.UserElectricalInspected.ToString(CultureInfo.InvariantCulture)},{x.UserPickedPercent.ToString(CultureInfo.InvariantCulture)},{x.UserCutPercent.ToString(CultureInfo.InvariantCulture)},{x.UserPrepedPercent.ToString(CultureInfo.InvariantCulture)},{x.UserAssembledPercent.ToString(CultureInfo.InvariantCulture)},{x.UserCrimpedPercent.ToString(CultureInfo.InvariantCulture)},{x.UserOpticalInspectedPercent.ToString(CultureInfo.InvariantCulture)},{x.UserElectricalInspectedPercent.ToString(CultureInfo.InvariantCulture)}"))});

					/**********************************/
					/*Commission*/
                    ;WITH faCommissionned AS (
                        SELECT f.[Fertigungsnummer],
                        SUM(fp.Anzahl) - SUM(ISNULL(tg.Menge_reserviert,0)) AS Menge , 
                        CAST( 100 * ((SUM(fp.Anzahl) - SUM(ISNULL(tg.Menge_reserviert,0)))/SUM(fp.Anzahl)) AS MONEY) AS PercentComm 
                        FROM Fertigung f 
                            JOIN #updatedValues u on u.[Fertigungsnummer]=f.[Fertigungsnummer]
                            JOIN Fertigung_Positionen fp on fp.ID_Fertigung=f.ID
                            LEFT JOIN tbl_Planung_gestartet tg ON tg.Artikel_Nr=fp.Artikel_Nr AND tg.ID_Fertigung=fp.ID_Fertigung
                            LEFT JOIN tbl_Planung_gestartet_ROH2 tr ON tr.Artikel_Nr=fp.Artikel_Nr AND tr.ID_Fertigung=fp.ID_Fertigung
                            WHERE tr.Artikel_Nr IS NULL
                            GROUP BY f.Fertigungsnummer )
					UPDATE w SET 
						UserPicked=f.PercentComm,
						UserPickedPercent = f.PercentComm
					FROM [Inventory].[ProductionWip] w JOIN faCommissionned f ON f.[Fertigungsnummer]=w.FA;

					/* Cutting */
					{(isVersioning == true
					?$@"
						;WITH faNeeded AS (
						SELECT 
							f.Fertigungsnummer, a.Artikelnummer, f.BomVersion, f.CPVersion, f.Originalanzahl 
							FROM Fertigung f INNER JOIN Artikel a ON a.[Artikel-Nr]=f.Artikel_Nr
							INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer
							WHERE ISNULL(f.Kabel_geschnitten,0)=0
						),
						CuttingData AS (
						SELECT SUM(ISNULL(Menge,0)) qty_total_to_cut,
						Artikelnummer_FG, p.BOM_version, p.CP_version
						FROM CP_snapshot_positions p JOIN CP_snapshot_header h 
							on h.Artikelnummer=p.Artikelnummer_FG AND h.BOM_version=p.BOM_version AND h.CP_version=p.CP_version
							INNER JOIN (SELECT DISTINCT Artikelnummer FROM Fertigung f 
								INNER JOIN  Artikel a on a.[Artikel-Nr]=f.Artikel_Nr 
								INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer) fa 
								ON fa.Artikelnummer=h.Artikelnummer
						GROUP BY Artikelnummer_FG, p.BOM_version, p.CP_version
						),
						CuttingDataPPS AS (
							SELECT 
							FA_Nummer, SUM(ISNULL(Menge,0)) AS qty_cutted
							FROM Tbl_Schneiderei_PPS s JOIN #updatedValues u on u.Fertigungsnummer=s.FA_Nummer
							WHERE (KorrekturSchniederei is null or KorrekturSchniederei=0)
							GROUP BY FA_Nummer
						)
						UPDATE w SET w.UserCut = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1)), 
							w.UserCutPercent = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1))
							FROM [Inventory].[ProductionWip] w JOIN faNeeded f ON f.[Fertigungsnummer]=w.FA
							LEFT JOIN CuttingData c on c.Artikelnummer_FG=f.Artikelnummer AND c.BOM_version=f.BomVersion AND c.CP_version=f.CPVersion 
							LEFT JOIN CuttingDataPPS s ON s.FA_Nummer=f.Fertigungsnummer "
					:$@"
						;WITH faNeeded AS (
							SELECT 
								f.Fertigungsnummer, a.Artikelnummer, f.KundenIndex, f.Originalanzahl 
								FROM Fertigung f INNER JOIN Artikel a ON a.[Artikel-Nr]=f.Artikel_Nr
								INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer
								WHERE ISNULL(f.Kabel_geschnitten,0)=0
							),
							CuttingData AS (
							SELECT SUM(ISNULL(Menge,0)) qty_total_to_cut,
							Artikelnummer_FG, p.Kunden_Index
							FROM CAO_Decoupage_Position p JOIN CAO_Decoupage h 
								on h.Artikelnummer=p.Artikelnummer_FG AND h.Kunden_Index=p.Kunden_Index
								INNER JOIN (SELECT DISTINCT Artikelnummer FROM Fertigung f 
									INNER JOIN  Artikel a on a.[Artikel-Nr]=f.Artikel_Nr 
									INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer) fa 
									ON fa.Artikelnummer=h.Artikelnummer
							GROUP BY Artikelnummer_FG, p.Kunden_Index
							),
							CuttingDataPPS AS (
								SELECT 
								FA_Nummer, SUM(ISNULL(Menge,0)) AS qty_cutted
								FROM Tbl_Schneiderei_PPS s JOIN #updatedValues u on u.Fertigungsnummer=s.FA_Nummer
								WHERE (KorrekturSchniederei is null or KorrekturSchniederei=0)
								GROUP BY FA_Nummer
							)
							UPDATE w SET w.UserCut = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1)), 
								w.UserCutPercent = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1))
								FROM [Inventory].[ProductionWip] w JOIN faNeeded f ON f.[Fertigungsnummer]=w.FA
								LEFT JOIN CuttingData c on c.Artikelnummer_FG=f.Artikelnummer AND c.Kunden_Index=f.KundenIndex 
								LEFT JOIN CuttingDataPPS s ON s.FA_Nummer=f.Fertigungsnummer 
					;")}
					/**********************************/

					
					UPDATE w SET w.UserPreped=u.Preped, w.UserPrepedPercent=u.PrepedPercent
					FROM [Inventory].[ProductionWip] w 
					JOIN #updatedValues u on u.Fertigungsnummer=w.FA
					JOIN [Inventory].[WorkArea] wa on wa.Step='Vorbereitung' AND wa.[LagerId]=w.LagerId
					JOIN Fertigung f on f.ID=w.IdFa WHERE f.kennzeichen='offen'/* AND ISNULL(w.[UserElectricalInspectedPercent],0)=0*/;

					UPDATE w SET w.UserAssembled=u.Assembled, w.UserAssembledPercent=u.AssembledPercent
					FROM [Inventory].[ProductionWip] w 
					JOIN #updatedValues u on u.Fertigungsnummer=w.FA
					JOIN [Inventory].[WorkArea] wa on wa.Step='Montage' AND wa.[LagerId]=w.LagerId
					JOIN Fertigung f on f.ID=w.IdFa WHERE f.kennzeichen='offen'/* AND ISNULL(w.[UserElectricalInspectedPercent],0)=0*/;

					UPDATE w SET w.UserCrimped=u.Crimped, w.UserCrimpedPercent=u.CrimpedPercent
					FROM [Inventory].[ProductionWip] w 
					JOIN #updatedValues u on u.Fertigungsnummer=w.FA
					JOIN [Inventory].[WorkArea] wa on wa.Step='Krimp' AND wa.[LagerId]=w.LagerId
					JOIN Fertigung f on f.ID=w.IdFa WHERE f.kennzeichen='offen'/* AND ISNULL(w.[UserElectricalInspectedPercent],0)=0*/;

					UPDATE w SET w.UserElectricalInspected=u.ElectricalInsp, w.UserElectricalInspectedPercent=u.ElectricalInspPercent
					FROM [Inventory].[ProductionWip] w 
					JOIN #updatedValues u on u.Fertigungsnummer=w.FA
					JOIN [Inventory].[WorkArea] wa on wa.Step='Elektrische Kontrolle' AND wa.[LagerId]=w.LagerId
					JOIN Fertigung f on f.ID=w.IdFa WHERE f.kennzeichen='offen'/* AND ISNULL(w.[UserElectricalInspectedPercent],0)=0*/;

					UPDATE w SET w.UserOpticalInspected=u.OpticalInsp, w.UserOpticalInspectedPercent=u.OpticalInspPercent
					FROM [Inventory].[ProductionWip] w 
					JOIN #updatedValues u on u.Fertigungsnummer=w.FA
					JOIN [Inventory].[WorkArea] wa on wa.Step='Optische Kontrolle' AND wa.[LagerId]=w.LagerId
					JOIN Fertigung f on f.ID=w.IdFa WHERE f.kennzeichen='offen'/* AND ISNULL(w.[UserElectricalInspectedPercent],0)=0*/


					/* Update Fa computed values */
					UPDATE w SET w.FaPicked=ISNULL(f.Anzahl, 1) * ArticlePicked * UserPickedPercent / 100.0
						, w.FaCut=ISNULL(f.Anzahl, 1) * ArticleCut * UserCutPercent / 100.0
						, w.FaPreped=ISNULL(f.Anzahl, 1) * w.ArticlePreped * w.UserPrepedPercent / 100.0
						, w.FaAssembled=ISNULL(f.Anzahl, 1) * w.ArticleAssembled * w.UserAssembledPercent / 100.0
						, w.FaCrimped=ISNULL(f.Anzahl, 1) * w.ArticleCrimped * w.UserCrimpedPercent / 100.0
						, w.FaElectricalInspected=ISNULL(f.Anzahl, 1) * w.ArticleElectricalInspected * w.UserElectricalInspectedPercent / 100.0
						, w.FaOpticalInspected=ISNULL(f.Anzahl, 1) * w.ArticleOpticalInspected * w.UserOpticalInspectedPercent / 100.0
					FROM [Inventory].[ProductionWip] w 
					JOIN #updatedValues u on u.Fertigungsnummer=w.FA
					JOIN [Inventory].[WorkArea] wa on wa.Step='Optische Kontrolle' AND wa.[LagerId]=w.LagerId
					JOIN Fertigung f on f.ID=w.IdFa WHERE f.kennzeichen='offen'/* AND ISNULL(w.[UserElectricalInspectedPercent],0)=0*/;";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}
		public static int UpdatePercent(int lagerId, bool isVersioning, SqlConnection connection, SqlTransaction transaction)
		{
				string query = $@"
					IF OBJECT_ID('tempdb..#updatedValues') IS NOT NULL DROP TABLE #updatedValues;
					CREATE TABLE #updatedValues([Fertigungsnummer]  INT);
					INSERT INTO #updatedValues([Fertigungsnummer]) SELECT DISTINCT [Fertigungsnummer] FROM Fertigung f WHERE f.Lagerort_id={lagerId} AND ISNULL(f.FA_Gestartet,0)=1 AND f.FA_begonnen IS NOT NULL AND f.kennzeichen='offen';
					
					/**********************************/
					/*Commission*/
                    ;WITH faCommissionned AS (
                        SELECT f.[Fertigungsnummer],
                        SUM(fp.Anzahl) - SUM(ISNULL(tg.Menge_reserviert,0)) AS Menge , 
                        CAST( 100 * ((SUM(fp.Anzahl) - SUM(ISNULL(tg.Menge_reserviert,0)))/SUM(fp.Anzahl)) AS MONEY) AS PercentComm 
                        FROM Fertigung f 
                            JOIN #updatedValues u on u.[Fertigungsnummer]=f.[Fertigungsnummer]
                            JOIN Fertigung_Positionen fp on fp.ID_Fertigung=f.ID
                            LEFT JOIN tbl_Planung_gestartet tg ON tg.Artikel_Nr=fp.Artikel_Nr AND tg.ID_Fertigung=fp.ID_Fertigung
                            LEFT JOIN tbl_Planung_gestartet_ROH2 tr ON tr.Artikel_Nr=fp.Artikel_Nr AND tr.ID_Fertigung=fp.ID_Fertigung
                            WHERE tr.Artikel_Nr IS NULL
                            GROUP BY f.Fertigungsnummer )
					UPDATE w SET 
						UserPicked=f.PercentComm,
						UserPickedPercent = f.PercentComm
					FROM [Inventory].[ProductionWip] w JOIN faCommissionned f ON f.[Fertigungsnummer]=w.FA;

					/* Cutting */
					{(isVersioning == true
					? $@"
						;WITH faNeeded AS (
						SELECT 
							f.Fertigungsnummer, a.Artikelnummer, f.BomVersion, f.CPVersion, f.Originalanzahl 
							FROM Fertigung f INNER JOIN Artikel a ON a.[Artikel-Nr]=f.Artikel_Nr
							INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer
							WHERE ISNULL(f.Kabel_geschnitten,0)=0
						),
						CuttingData AS (
						SELECT SUM(ISNULL(Menge,0)) qty_total_to_cut,
						Artikelnummer_FG, p.BOM_version, p.CP_version
						FROM CP_snapshot_positions p JOIN CP_snapshot_header h 
							on h.Artikelnummer=p.Artikelnummer_FG AND h.BOM_version=p.BOM_version AND h.CP_version=p.CP_version
							INNER JOIN (SELECT DISTINCT Artikelnummer FROM Fertigung f 
								INNER JOIN  Artikel a on a.[Artikel-Nr]=f.Artikel_Nr 
								INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer) fa 
								ON fa.Artikelnummer=h.Artikelnummer
						GROUP BY Artikelnummer_FG, p.BOM_version, p.CP_version
						),
						CuttingDataPPS AS (
							SELECT 
							FA_Nummer, SUM(ISNULL(Menge,0)) AS qty_cutted
							FROM Tbl_Schneiderei_PPS s JOIN #updatedValues u on u.Fertigungsnummer=s.FA_Nummer
							WHERE (KorrekturSchniederei is null or KorrekturSchniederei=0)
							GROUP BY FA_Nummer
						)
						UPDATE w SET w.UserCut = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1)), 
							w.UserCutPercent = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1))
							FROM [Inventory].[ProductionWip] w JOIN faNeeded f ON f.[Fertigungsnummer]=w.FA
							LEFT JOIN CuttingData c on c.Artikelnummer_FG=f.Artikelnummer AND c.BOM_version=f.BomVersion AND c.CP_version=f.CPVersion 
							LEFT JOIN CuttingDataPPS s ON s.FA_Nummer=f.Fertigungsnummer "
					: $@"
						;WITH faNeeded AS (
							SELECT 
								f.Fertigungsnummer, a.Artikelnummer, f.KundenIndex, f.Originalanzahl 
								FROM Fertigung f INNER JOIN Artikel a ON a.[Artikel-Nr]=f.Artikel_Nr
								INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer
								WHERE ISNULL(f.Kabel_geschnitten,0)=0
							),
							CuttingData AS (
							SELECT SUM(ISNULL(Menge,0)) qty_total_to_cut,
							Artikelnummer_FG, p.Kunden_Index
							FROM CAO_Decoupage_Position p JOIN CAO_Decoupage h 
								on h.Artikelnummer=p.Artikelnummer_FG AND h.Kunden_Index=p.Kunden_Index
								INNER JOIN (SELECT DISTINCT Artikelnummer FROM Fertigung f 
									INNER JOIN  Artikel a on a.[Artikel-Nr]=f.Artikel_Nr 
									INNER JOIN #updatedValues u on u.Fertigungsnummer=f.Fertigungsnummer) fa 
									ON fa.Artikelnummer=h.Artikelnummer
							GROUP BY Artikelnummer_FG, p.Kunden_Index
							),
							CuttingDataPPS AS (
								SELECT 
								FA_Nummer, SUM(ISNULL(Menge,0)) AS qty_cutted
								FROM Tbl_Schneiderei_PPS s JOIN #updatedValues u on u.Fertigungsnummer=s.FA_Nummer
								WHERE (KorrekturSchniederei is null or KorrekturSchniederei=0)
								GROUP BY FA_Nummer
							)
							UPDATE w SET w.UserCut = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1)), 
								w.UserCutPercent = 100 * ISNULL(s.qty_cutted,0) / (ISNULL(f.Originalanzahl,1) * ISNULL(c.qty_total_to_cut,1))
								FROM [Inventory].[ProductionWip] w JOIN faNeeded f ON f.[Fertigungsnummer]=w.FA
								LEFT JOIN CuttingData c on c.Artikelnummer_FG=f.Artikelnummer AND c.Kunden_Index=f.KundenIndex 
								LEFT JOIN CuttingDataPPS s ON s.FA_Nummer=f.Fertigungsnummer ")}
					/**********************************/

					/* Update Fa computed values */
					UPDATE w SET w.FaPicked=ISNULL(f.Anzahl, 1) * ArticlePicked * UserPickedPercent / 100.0
						, w.FaCut=ISNULL(f.Anzahl, 1) * ArticleCut * UserCutPercent / 100.0
						, w.FaPreped=ISNULL(f.Anzahl, 1) * w.ArticlePreped * w.UserPrepedPercent / 100.0
						, w.FaAssembled=ISNULL(f.Anzahl, 1) * w.ArticleAssembled * w.UserAssembledPercent / 100.0
						, w.FaCrimped=ISNULL(f.Anzahl, 1) * w.ArticleCrimped * w.UserCrimpedPercent / 100.0
						, w.FaElectricalInspected=ISNULL(f.Anzahl, 1) * w.ArticleElectricalInspected * w.UserElectricalInspectedPercent / 100.0
						, w.FaOpticalInspected=ISNULL(f.Anzahl, 1) * w.ArticleOpticalInspected * w.UserOpticalInspectedPercent / 100.0
					FROM [Inventory].[ProductionWip] w 
					JOIN #updatedValues u on u.Fertigungsnummer=w.FA
					JOIN [Inventory].[WorkArea] wa on wa.Step='Optische Kontrolle' AND wa.[LagerId]=w.LagerId
					JOIN Fertigung f on f.ID=w.IdFa WHERE f.kennzeichen='offen'/* AND ISNULL(w.[UserElectricalInspectedPercent],0)=0*/;
";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int ResetPercent(int lagerId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"UPDATE [Inventory].[ProductionWip] SET [UserPicked] = 0,
								[UserCut] = 0,
								[UserPreped] = 0,
								[UserAssembled] = 0,
								[UserCrimped] = 0,
								[UserOpticalInspected] = 0,
								[UserElectricalInspected] = 0,
								[UserPickedPercent] = 0,
								[UserCutPercent] = 0,
								[UserPrepedPercent] = 0,
								[UserAssembledPercent] = 0,
								[UserCrimpedPercent] = 0,
								[UserOpticalInspectedPercent] = 0,
								[UserElectricalInspectedPercent] = 0,
								[FaPicked] = 0,
								[FaCut] = 0,
								[FaPreped] = 0,
								[FaAssembled] = 0,
								[FaCrimped] = 0,
								[FaOpticalInspected] = 0,
								[FaElectricalInspected] = 0 WHERE [LagerId]=@lagerId AND YEAR([InventoryYear])=YEAR(GETDATE());
							-- ======================================================
							UPDATE w SET UserCut=100, UserCutPercent=100 FROM [Inventory].[ProductionWip] w JOIN Fertigung f on f.ID=w.IdFa WHERE ISNULL(f.Kabel_geschnitten,0)=1 AND w.LagerId=@lagerId;
							UPDATE w SET UserPicked=100, UserPickedPercent=100 FROM [Inventory].[ProductionWip] w JOIN Fertigung f on f.ID=w.IdFa WHERE ISNULL(f.Kommisioniert_komplett,0)=1 AND w.LagerId=@lagerId;";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int ResetWipTable(int lagerId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"DELETE FROM [Inventory].[ProductionWip] WHERE [LagerId]=@lagerId AND YEAR([InventoryYear])=YEAR(GETDATE());";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipTotalEntity> GetWipTotals(int warehouseId, int year = 0)
		{
			if(year == 0) year = DateTime.Now.Year;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [Id], [IdFa], [FA], [OpenQty], [Due], [InventoryYear], [ArtikelNr], [Item], [LagerId], [FaPicked]+[FaCut]+[FaPreped]+[FaAssembled]+[FaCrimped]+[FaOpticalInspected]+[FaElectricalInspected] [Total] 
								FROM  [Inventory].[ProductionWip] WHERE LagerId=@lagerId AND YEAR([InventoryYear])=@year 
									AND [FaPicked]>=0 AND [FaCut]>=0 AND [FaPreped]>=0 AND [FaAssembled]>=0 AND [FaCrimped]>=0 AND [FaOpticalInspected]>=0 AND [FaElectricalInspected]>=0
									ORDER BY FA;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerId", warehouseId);
				sqlCommand.Parameters.AddWithValue("year", year);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipTotalEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipTotalEntity>();

			}

		}
		#endregion Custom Methods
	}
}
