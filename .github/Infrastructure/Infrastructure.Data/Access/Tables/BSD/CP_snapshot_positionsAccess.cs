using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class CP_snapshot_positionsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CP_snapshot_positions] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CP_snapshot_positions]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [CP_snapshot_positions] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [CP_snapshot_positions] ([Artikel_Nr_FG],[Artikel_Nr_ROH],[Artikelnummer_FG],[Artikelnummer_ROH],[BOM_version],[Box],[Changed],[Code_1],[Code_2],[Code_3],[Contact_A],[Contact_B],[Couleur_impression],[CP_version],[Deg_B],[Degunage_A],[Denudage1],[Denudage2],[Dichtung_A],[Dichtung_B],[Gewerk],[ID_Nr],[Impression],[Impression_Gauche],[Impression_Milieu],[Kunden_Index],[Length],[Menge],[Outil_A],[Outil_B],[Position],[Tol],[Triage_A],[Triage_B],[ZESuivante])  VALUES (@Artikel_Nr_FG,@Artikel_Nr_ROH,@Artikelnummer_FG,@Artikelnummer_ROH,@BOM_version,@Box,@Changed,@Code_1,@Code_2,@Code_3,@Contact_A,@Contact_B,@Couleur_impression,@CP_version,@Deg_B,@Degunage_A,@Denudage1,@Denudage2,@Dichtung_A,@Dichtung_B,@Gewerk,@ID_Nr,@Impression,@Impression_Gauche,@Impression_Milieu,@Kunden_Index,@Length,@Menge,@Outil_A,@Outil_B,@Position,@Tol,@Triage_A,@Triage_B,@ZESuivante); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikel_Nr_FG", item.Artikel_Nr_FG == null ? (object)DBNull.Value : item.Artikel_Nr_FG);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_ROH", item.Artikel_Nr_ROH == null ? (object)DBNull.Value : item.Artikel_Nr_ROH);
					sqlCommand.Parameters.AddWithValue("Artikelnummer_FG", item.Artikelnummer_FG == null ? (object)DBNull.Value : item.Artikelnummer_FG);
					sqlCommand.Parameters.AddWithValue("Artikelnummer_ROH", item.Artikelnummer_ROH == null ? (object)DBNull.Value : item.Artikelnummer_ROH);
					sqlCommand.Parameters.AddWithValue("BOM_version", item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
					sqlCommand.Parameters.AddWithValue("Box", item.Box == null ? (object)DBNull.Value : item.Box);
					sqlCommand.Parameters.AddWithValue("Changed", item.Changed == null ? (object)DBNull.Value : item.Changed);
					sqlCommand.Parameters.AddWithValue("Code_1", item.Code_1 == null ? (object)DBNull.Value : item.Code_1);
					sqlCommand.Parameters.AddWithValue("Code_2", item.Code_2 == null ? (object)DBNull.Value : item.Code_2);
					sqlCommand.Parameters.AddWithValue("Code_3", item.Code_3 == null ? (object)DBNull.Value : item.Code_3);
					sqlCommand.Parameters.AddWithValue("Contact_A", item.Contact_A == null ? (object)DBNull.Value : item.Contact_A);
					sqlCommand.Parameters.AddWithValue("Contact_B", item.Contact_B == null ? (object)DBNull.Value : item.Contact_B);
					sqlCommand.Parameters.AddWithValue("Couleur_impression", item.Couleur_impression == null ? (object)DBNull.Value : item.Couleur_impression);
					sqlCommand.Parameters.AddWithValue("CP_version", item.CP_version == null ? (object)DBNull.Value : item.CP_version);
					sqlCommand.Parameters.AddWithValue("Deg_B", item.Deg_B == null ? (object)DBNull.Value : item.Deg_B);
					sqlCommand.Parameters.AddWithValue("Degunage_A", item.Degunage_A == null ? (object)DBNull.Value : item.Degunage_A);
					sqlCommand.Parameters.AddWithValue("Denudage1", item.Denudage1 == null ? (object)DBNull.Value : item.Denudage1);
					sqlCommand.Parameters.AddWithValue("Denudage2", item.Denudage2 == null ? (object)DBNull.Value : item.Denudage2);
					sqlCommand.Parameters.AddWithValue("Dichtung_A", item.Dichtung_A == null ? (object)DBNull.Value : item.Dichtung_A);
					sqlCommand.Parameters.AddWithValue("Dichtung_B", item.Dichtung_B == null ? (object)DBNull.Value : item.Dichtung_B);
					sqlCommand.Parameters.AddWithValue("Gewerk", item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
					sqlCommand.Parameters.AddWithValue("ID_Nr", item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
					sqlCommand.Parameters.AddWithValue("Impression", item.Impression == null ? (object)DBNull.Value : item.Impression);
					sqlCommand.Parameters.AddWithValue("Impression_Gauche", item.Impression_Gauche == null ? (object)DBNull.Value : item.Impression_Gauche);
					sqlCommand.Parameters.AddWithValue("Impression_Milieu", item.Impression_Milieu == null ? (object)DBNull.Value : item.Impression_Milieu);
					sqlCommand.Parameters.AddWithValue("Kunden_Index", item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
					sqlCommand.Parameters.AddWithValue("Length", item.Length == null ? (object)DBNull.Value : item.Length);
					sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("Outil_A", item.Outil_A == null ? (object)DBNull.Value : item.Outil_A);
					sqlCommand.Parameters.AddWithValue("Outil_B", item.Outil_B == null ? (object)DBNull.Value : item.Outil_B);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Tol", item.Tol == null ? (object)DBNull.Value : item.Tol);
					sqlCommand.Parameters.AddWithValue("Triage_A", item.Triage_A == null ? (object)DBNull.Value : item.Triage_A);
					sqlCommand.Parameters.AddWithValue("Triage_B", item.Triage_B == null ? (object)DBNull.Value : item.Triage_B);
					sqlCommand.Parameters.AddWithValue("ZESuivante", item.ZESuivante == null ? (object)DBNull.Value : item.ZESuivante);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 36; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [CP_snapshot_positions] ([Artikel_Nr_FG],[Artikel_Nr_ROH],[Artikelnummer_FG],[Artikelnummer_ROH],[BOM_version],[Box],[Changed],[Code_1],[Code_2],[Code_3],[Contact_A],[Contact_B],[Couleur_impression],[CP_version],[Deg_B],[Degunage_A],[Denudage1],[Denudage2],[Dichtung_A],[Dichtung_B],[Gewerk],[ID_Nr],[Impression],[Impression_Gauche],[Impression_Milieu],[Kunden_Index],[Length],[Menge],[Outil_A],[Outil_B],[Position],[Tol],[Triage_A],[Triage_B],[ZESuivante]) VALUES ( "

							+ "@Artikel_Nr_FG" + i + ","
							+ "@Artikel_Nr_ROH" + i + ","
							+ "@Artikelnummer_FG" + i + ","
							+ "@Artikelnummer_ROH" + i + ","
							+ "@BOM_version" + i + ","
							+ "@Box" + i + ","
							+ "@Changed" + i + ","
							+ "@Code_1" + i + ","
							+ "@Code_2" + i + ","
							+ "@Code_3" + i + ","
							+ "@Contact_A" + i + ","
							+ "@Contact_B" + i + ","
							+ "@Couleur_impression" + i + ","
							+ "@CP_version" + i + ","
							+ "@Deg_B" + i + ","
							+ "@Degunage_A" + i + ","
							+ "@Denudage1" + i + ","
							+ "@Denudage2" + i + ","
							+ "@Dichtung_A" + i + ","
							+ "@Dichtung_B" + i + ","
							+ "@Gewerk" + i + ","
							+ "@ID_Nr" + i + ","
							+ "@Impression" + i + ","
							+ "@Impression_Gauche" + i + ","
							+ "@Impression_Milieu" + i + ","
							+ "@Kunden_Index" + i + ","
							+ "@Length" + i + ","
							+ "@Menge" + i + ","
							+ "@Outil_A" + i + ","
							+ "@Outil_B" + i + ","
							+ "@Position" + i + ","
							+ "@Tol" + i + ","
							+ "@Triage_A" + i + ","
							+ "@Triage_B" + i + ","
							+ "@ZESuivante" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikel_Nr_FG" + i, item.Artikel_Nr_FG == null ? (object)DBNull.Value : item.Artikel_Nr_FG);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_ROH" + i, item.Artikel_Nr_ROH == null ? (object)DBNull.Value : item.Artikel_Nr_ROH);
						sqlCommand.Parameters.AddWithValue("Artikelnummer_FG" + i, item.Artikelnummer_FG == null ? (object)DBNull.Value : item.Artikelnummer_FG);
						sqlCommand.Parameters.AddWithValue("Artikelnummer_ROH" + i, item.Artikelnummer_ROH == null ? (object)DBNull.Value : item.Artikelnummer_ROH);
						sqlCommand.Parameters.AddWithValue("BOM_version" + i, item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
						sqlCommand.Parameters.AddWithValue("Box" + i, item.Box == null ? (object)DBNull.Value : item.Box);
						sqlCommand.Parameters.AddWithValue("Changed" + i, item.Changed == null ? (object)DBNull.Value : item.Changed);
						sqlCommand.Parameters.AddWithValue("Code_1" + i, item.Code_1 == null ? (object)DBNull.Value : item.Code_1);
						sqlCommand.Parameters.AddWithValue("Code_2" + i, item.Code_2 == null ? (object)DBNull.Value : item.Code_2);
						sqlCommand.Parameters.AddWithValue("Code_3" + i, item.Code_3 == null ? (object)DBNull.Value : item.Code_3);
						sqlCommand.Parameters.AddWithValue("Contact_A" + i, item.Contact_A == null ? (object)DBNull.Value : item.Contact_A);
						sqlCommand.Parameters.AddWithValue("Contact_B" + i, item.Contact_B == null ? (object)DBNull.Value : item.Contact_B);
						sqlCommand.Parameters.AddWithValue("Couleur_impression" + i, item.Couleur_impression == null ? (object)DBNull.Value : item.Couleur_impression);
						sqlCommand.Parameters.AddWithValue("CP_version" + i, item.CP_version == null ? (object)DBNull.Value : item.CP_version);
						sqlCommand.Parameters.AddWithValue("Deg_B" + i, item.Deg_B == null ? (object)DBNull.Value : item.Deg_B);
						sqlCommand.Parameters.AddWithValue("Degunage_A" + i, item.Degunage_A == null ? (object)DBNull.Value : item.Degunage_A);
						sqlCommand.Parameters.AddWithValue("Denudage1" + i, item.Denudage1 == null ? (object)DBNull.Value : item.Denudage1);
						sqlCommand.Parameters.AddWithValue("Denudage2" + i, item.Denudage2 == null ? (object)DBNull.Value : item.Denudage2);
						sqlCommand.Parameters.AddWithValue("Dichtung_A" + i, item.Dichtung_A == null ? (object)DBNull.Value : item.Dichtung_A);
						sqlCommand.Parameters.AddWithValue("Dichtung_B" + i, item.Dichtung_B == null ? (object)DBNull.Value : item.Dichtung_B);
						sqlCommand.Parameters.AddWithValue("Gewerk" + i, item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
						sqlCommand.Parameters.AddWithValue("ID_Nr" + i, item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
						sqlCommand.Parameters.AddWithValue("Impression" + i, item.Impression == null ? (object)DBNull.Value : item.Impression);
						sqlCommand.Parameters.AddWithValue("Impression_Gauche" + i, item.Impression_Gauche == null ? (object)DBNull.Value : item.Impression_Gauche);
						sqlCommand.Parameters.AddWithValue("Impression_Milieu" + i, item.Impression_Milieu == null ? (object)DBNull.Value : item.Impression_Milieu);
						sqlCommand.Parameters.AddWithValue("Kunden_Index" + i, item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
						sqlCommand.Parameters.AddWithValue("Length" + i, item.Length == null ? (object)DBNull.Value : item.Length);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("Outil_A" + i, item.Outil_A == null ? (object)DBNull.Value : item.Outil_A);
						sqlCommand.Parameters.AddWithValue("Outil_B" + i, item.Outil_B == null ? (object)DBNull.Value : item.Outil_B);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Tol" + i, item.Tol == null ? (object)DBNull.Value : item.Tol);
						sqlCommand.Parameters.AddWithValue("Triage_A" + i, item.Triage_A == null ? (object)DBNull.Value : item.Triage_A);
						sqlCommand.Parameters.AddWithValue("Triage_B" + i, item.Triage_B == null ? (object)DBNull.Value : item.Triage_B);
						sqlCommand.Parameters.AddWithValue("ZESuivante" + i, item.ZESuivante == null ? (object)DBNull.Value : item.ZESuivante);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [CP_snapshot_positions] SET [Artikel_Nr_FG]=@Artikel_Nr_FG, [Artikel_Nr_ROH]=@Artikel_Nr_ROH, [Artikelnummer_FG]=@Artikelnummer_FG, [Artikelnummer_ROH]=@Artikelnummer_ROH, [BOM_version]=@BOM_version, [Box]=@Box, [Changed]=@Changed, [Code_1]=@Code_1, [Code_2]=@Code_2, [Code_3]=@Code_3, [Contact_A]=@Contact_A, [Contact_B]=@Contact_B, [Couleur_impression]=@Couleur_impression, [CP_version]=@CP_version, [Deg_B]=@Deg_B, [Degunage_A]=@Degunage_A, [Denudage1]=@Denudage1, [Denudage2]=@Denudage2, [Dichtung_A]=@Dichtung_A, [Dichtung_B]=@Dichtung_B, [Gewerk]=@Gewerk, [ID_Nr]=@ID_Nr, [Impression]=@Impression, [Impression_Gauche]=@Impression_Gauche, [Impression_Milieu]=@Impression_Milieu, [Kunden_Index]=@Kunden_Index, [Length]=@Length, [Menge]=@Menge, [Outil_A]=@Outil_A, [Outil_B]=@Outil_B, [Position]=@Position, [Tol]=@Tol, [Triage_A]=@Triage_A, [Triage_B]=@Triage_B, [ZESuivante]=@ZESuivante WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr_FG", item.Artikel_Nr_FG == null ? (object)DBNull.Value : item.Artikel_Nr_FG);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr_ROH", item.Artikel_Nr_ROH == null ? (object)DBNull.Value : item.Artikel_Nr_ROH);
				sqlCommand.Parameters.AddWithValue("Artikelnummer_FG", item.Artikelnummer_FG == null ? (object)DBNull.Value : item.Artikelnummer_FG);
				sqlCommand.Parameters.AddWithValue("Artikelnummer_ROH", item.Artikelnummer_ROH == null ? (object)DBNull.Value : item.Artikelnummer_ROH);
				sqlCommand.Parameters.AddWithValue("BOM_version", item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
				sqlCommand.Parameters.AddWithValue("Box", item.Box == null ? (object)DBNull.Value : item.Box);
				sqlCommand.Parameters.AddWithValue("Changed", item.Changed == null ? (object)DBNull.Value : item.Changed);
				sqlCommand.Parameters.AddWithValue("Code_1", item.Code_1 == null ? (object)DBNull.Value : item.Code_1);
				sqlCommand.Parameters.AddWithValue("Code_2", item.Code_2 == null ? (object)DBNull.Value : item.Code_2);
				sqlCommand.Parameters.AddWithValue("Code_3", item.Code_3 == null ? (object)DBNull.Value : item.Code_3);
				sqlCommand.Parameters.AddWithValue("Contact_A", item.Contact_A == null ? (object)DBNull.Value : item.Contact_A);
				sqlCommand.Parameters.AddWithValue("Contact_B", item.Contact_B == null ? (object)DBNull.Value : item.Contact_B);
				sqlCommand.Parameters.AddWithValue("Couleur_impression", item.Couleur_impression == null ? (object)DBNull.Value : item.Couleur_impression);
				sqlCommand.Parameters.AddWithValue("CP_version", item.CP_version == null ? (object)DBNull.Value : item.CP_version);
				sqlCommand.Parameters.AddWithValue("Deg_B", item.Deg_B == null ? (object)DBNull.Value : item.Deg_B);
				sqlCommand.Parameters.AddWithValue("Degunage_A", item.Degunage_A == null ? (object)DBNull.Value : item.Degunage_A);
				sqlCommand.Parameters.AddWithValue("Denudage1", item.Denudage1 == null ? (object)DBNull.Value : item.Denudage1);
				sqlCommand.Parameters.AddWithValue("Denudage2", item.Denudage2 == null ? (object)DBNull.Value : item.Denudage2);
				sqlCommand.Parameters.AddWithValue("Dichtung_A", item.Dichtung_A == null ? (object)DBNull.Value : item.Dichtung_A);
				sqlCommand.Parameters.AddWithValue("Dichtung_B", item.Dichtung_B == null ? (object)DBNull.Value : item.Dichtung_B);
				sqlCommand.Parameters.AddWithValue("Gewerk", item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
				sqlCommand.Parameters.AddWithValue("ID_Nr", item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
				sqlCommand.Parameters.AddWithValue("Impression", item.Impression == null ? (object)DBNull.Value : item.Impression);
				sqlCommand.Parameters.AddWithValue("Impression_Gauche", item.Impression_Gauche == null ? (object)DBNull.Value : item.Impression_Gauche);
				sqlCommand.Parameters.AddWithValue("Impression_Milieu", item.Impression_Milieu == null ? (object)DBNull.Value : item.Impression_Milieu);
				sqlCommand.Parameters.AddWithValue("Kunden_Index", item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
				sqlCommand.Parameters.AddWithValue("Length", item.Length == null ? (object)DBNull.Value : item.Length);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("Outil_A", item.Outil_A == null ? (object)DBNull.Value : item.Outil_A);
				sqlCommand.Parameters.AddWithValue("Outil_B", item.Outil_B == null ? (object)DBNull.Value : item.Outil_B);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("Tol", item.Tol == null ? (object)DBNull.Value : item.Tol);
				sqlCommand.Parameters.AddWithValue("Triage_A", item.Triage_A == null ? (object)DBNull.Value : item.Triage_A);
				sqlCommand.Parameters.AddWithValue("Triage_B", item.Triage_B == null ? (object)DBNull.Value : item.Triage_B);
				sqlCommand.Parameters.AddWithValue("ZESuivante", item.ZESuivante == null ? (object)DBNull.Value : item.ZESuivante);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 36; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [CP_snapshot_positions] SET "

							+ "[Artikel_Nr_FG]=@Artikel_Nr_FG" + i + ","
							+ "[Artikel_Nr_ROH]=@Artikel_Nr_ROH" + i + ","
							+ "[Artikelnummer_FG]=@Artikelnummer_FG" + i + ","
							+ "[Artikelnummer_ROH]=@Artikelnummer_ROH" + i + ","
							+ "[BOM_version]=@BOM_version" + i + ","
							+ "[Box]=@Box" + i + ","
							+ "[Changed]=@Changed" + i + ","
							+ "[Code_1]=@Code_1" + i + ","
							+ "[Code_2]=@Code_2" + i + ","
							+ "[Code_3]=@Code_3" + i + ","
							+ "[Contact_A]=@Contact_A" + i + ","
							+ "[Contact_B]=@Contact_B" + i + ","
							+ "[Couleur_impression]=@Couleur_impression" + i + ","
							+ "[CP_version]=@CP_version" + i + ","
							+ "[Deg_B]=@Deg_B" + i + ","
							+ "[Degunage_A]=@Degunage_A" + i + ","
							+ "[Denudage1]=@Denudage1" + i + ","
							+ "[Denudage2]=@Denudage2" + i + ","
							+ "[Dichtung_A]=@Dichtung_A" + i + ","
							+ "[Dichtung_B]=@Dichtung_B" + i + ","
							+ "[Gewerk]=@Gewerk" + i + ","
							+ "[ID_Nr]=@ID_Nr" + i + ","
							+ "[Impression]=@Impression" + i + ","
							+ "[Impression_Gauche]=@Impression_Gauche" + i + ","
							+ "[Impression_Milieu]=@Impression_Milieu" + i + ","
							+ "[Kunden_Index]=@Kunden_Index" + i + ","
							+ "[Length]=@Length" + i + ","
							+ "[Menge]=@Menge" + i + ","
							+ "[Outil_A]=@Outil_A" + i + ","
							+ "[Outil_B]=@Outil_B" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[Tol]=@Tol" + i + ","
							+ "[Triage_A]=@Triage_A" + i + ","
							+ "[Triage_B]=@Triage_B" + i + ","
							+ "[ZESuivante]=@ZESuivante" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_FG" + i, item.Artikel_Nr_FG == null ? (object)DBNull.Value : item.Artikel_Nr_FG);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_ROH" + i, item.Artikel_Nr_ROH == null ? (object)DBNull.Value : item.Artikel_Nr_ROH);
						sqlCommand.Parameters.AddWithValue("Artikelnummer_FG" + i, item.Artikelnummer_FG == null ? (object)DBNull.Value : item.Artikelnummer_FG);
						sqlCommand.Parameters.AddWithValue("Artikelnummer_ROH" + i, item.Artikelnummer_ROH == null ? (object)DBNull.Value : item.Artikelnummer_ROH);
						sqlCommand.Parameters.AddWithValue("BOM_version" + i, item.BOM_version == null ? (object)DBNull.Value : item.BOM_version);
						sqlCommand.Parameters.AddWithValue("Box" + i, item.Box == null ? (object)DBNull.Value : item.Box);
						sqlCommand.Parameters.AddWithValue("Changed" + i, item.Changed == null ? (object)DBNull.Value : item.Changed);
						sqlCommand.Parameters.AddWithValue("Code_1" + i, item.Code_1 == null ? (object)DBNull.Value : item.Code_1);
						sqlCommand.Parameters.AddWithValue("Code_2" + i, item.Code_2 == null ? (object)DBNull.Value : item.Code_2);
						sqlCommand.Parameters.AddWithValue("Code_3" + i, item.Code_3 == null ? (object)DBNull.Value : item.Code_3);
						sqlCommand.Parameters.AddWithValue("Contact_A" + i, item.Contact_A == null ? (object)DBNull.Value : item.Contact_A);
						sqlCommand.Parameters.AddWithValue("Contact_B" + i, item.Contact_B == null ? (object)DBNull.Value : item.Contact_B);
						sqlCommand.Parameters.AddWithValue("Couleur_impression" + i, item.Couleur_impression == null ? (object)DBNull.Value : item.Couleur_impression);
						sqlCommand.Parameters.AddWithValue("CP_version" + i, item.CP_version == null ? (object)DBNull.Value : item.CP_version);
						sqlCommand.Parameters.AddWithValue("Deg_B" + i, item.Deg_B == null ? (object)DBNull.Value : item.Deg_B);
						sqlCommand.Parameters.AddWithValue("Degunage_A" + i, item.Degunage_A == null ? (object)DBNull.Value : item.Degunage_A);
						sqlCommand.Parameters.AddWithValue("Denudage1" + i, item.Denudage1 == null ? (object)DBNull.Value : item.Denudage1);
						sqlCommand.Parameters.AddWithValue("Denudage2" + i, item.Denudage2 == null ? (object)DBNull.Value : item.Denudage2);
						sqlCommand.Parameters.AddWithValue("Dichtung_A" + i, item.Dichtung_A == null ? (object)DBNull.Value : item.Dichtung_A);
						sqlCommand.Parameters.AddWithValue("Dichtung_B" + i, item.Dichtung_B == null ? (object)DBNull.Value : item.Dichtung_B);
						sqlCommand.Parameters.AddWithValue("Gewerk" + i, item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
						sqlCommand.Parameters.AddWithValue("ID_Nr" + i, item.ID_Nr == null ? (object)DBNull.Value : item.ID_Nr);
						sqlCommand.Parameters.AddWithValue("Impression" + i, item.Impression == null ? (object)DBNull.Value : item.Impression);
						sqlCommand.Parameters.AddWithValue("Impression_Gauche" + i, item.Impression_Gauche == null ? (object)DBNull.Value : item.Impression_Gauche);
						sqlCommand.Parameters.AddWithValue("Impression_Milieu" + i, item.Impression_Milieu == null ? (object)DBNull.Value : item.Impression_Milieu);
						sqlCommand.Parameters.AddWithValue("Kunden_Index" + i, item.Kunden_Index == null ? (object)DBNull.Value : item.Kunden_Index);
						sqlCommand.Parameters.AddWithValue("Length" + i, item.Length == null ? (object)DBNull.Value : item.Length);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("Outil_A" + i, item.Outil_A == null ? (object)DBNull.Value : item.Outil_A);
						sqlCommand.Parameters.AddWithValue("Outil_B" + i, item.Outil_B == null ? (object)DBNull.Value : item.Outil_B);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Tol" + i, item.Tol == null ? (object)DBNull.Value : item.Tol);
						sqlCommand.Parameters.AddWithValue("Triage_A" + i, item.Triage_A == null ? (object)DBNull.Value : item.Triage_A);
						sqlCommand.Parameters.AddWithValue("Triage_B" + i, item.Triage_B == null ? (object)DBNull.Value : item.Triage_B);
						sqlCommand.Parameters.AddWithValue("ZESuivante" + i, item.ZESuivante == null ? (object)DBNull.Value : item.ZESuivante);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [CP_snapshot_positions] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					string query = "DELETE FROM [CP_snapshot_positions] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity ExistsByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP (1) * FROM [CP_snapshot_positions] WHERE [Artikel_Nr_FG]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		//public static Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity GetLastByArticle(int articleId, int bomVersion)
		//{
		//    var dataTable = new DataTable();
		//    using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//    {
		//        sqlConnection.Open();
		//        string query = @"SELECT TOP 1 * FROM [CP_snapshot_positions] WHERE [Artikel_Nr_FG]=@articleId AND [BOM_version]=@bomVersion 
		//                            AND [CP_Version] IN (SELECT MAX(CP_version) FROM [CP_snapshot_positions] WHERE [Artikel_Nr_FG] = @articleId AND [BOM_version] = @bomVersion  GROUP BY [Artikel_Nr_FG], [BOM_version])";
		//        var sqlCommand = new SqlCommand(query, sqlConnection);
		//        sqlCommand.Parameters.AddWithValue("articleId", articleId);
		//        sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);

		//        new SqlDataAdapter(sqlCommand).Fill(dataTable);

		//    }

		//    if (dataTable.Rows.Count > 0)
		//    {
		//        return new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity(dataTable.Rows[0]);
		//    }
		//    else
		//    {
		//        return null;
		//    }
		//}

		public static Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity GetLastByArticle(int articleId, int bomVersion)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT TOP 1 * FROM [CP_snapshot_positions] WHERE [Artikel_Nr_FG]=@articleId AND [BOM_version]=@bomVersion 
                                    AND [CP_Version] IN (SELECT MAX(CP_version) FROM [CP_snapshot_positions] WHERE [Artikel_Nr_FG] = @articleId AND [BOM_version] = @bomVersion  GROUP BY [Artikel_Nr_FG], [BOM_version])";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity GetLastByArticle(int articleId, int bomVersion, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = @"SELECT TOP 1 * FROM [CP_snapshot_positions] WHERE [Artikel_Nr_FG]=@articleId AND [BOM_version]=@bomVersion 
                                    AND [CP_Version] IN (SELECT MAX(CP_version) FROM [CP_snapshot_positions] WHERE [Artikel_Nr_FG] = @articleId AND [BOM_version] = @bomVersion  GROUP BY [Artikel_Nr_FG], [BOM_version])";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("articleId", articleId);
			sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
