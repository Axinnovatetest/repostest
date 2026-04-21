using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class AnsprechpartnerAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Ansprechpartner] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Ansprechpartner]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = "SELECT * FROM [Ansprechpartner] WHERE [Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Ansprechpartner] "
					+ " ([Abteilung],[Anrede],[Ansprechpartner],[auswahl_AB_BW],[Bemerkung],[Briefanrede], "
					+ " [eMail],[FAX],[Geburtstag],[Land],[Mobil],[Nummer],[Ort],[PLZ],[Position],[Serienbrief], "
					+ " [Sprache],[Straße],[Telefon],[Titel],[zu_Händen]) "
					+ " VALUES "
					+ " (@Abteilung,@Anrede,@Ansprechpartner,@auswahl_AB_BW,@Bemerkung,@Briefanrede, "
					+ " @eMail,@FAX,@Geburtstag,@Land,@Mobil,@Nummer,@Ort,@PLZ,@Position,@Serienbrief, "
					+ " @Sprache,@Straße,@Telefon,@Titel,@zu_Händen)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("auswahl_AB_BW", item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("eMail", item.EMail == null ? (object)DBNull.Value : item.EMail);
					sqlCommand.Parameters.AddWithValue("FAX", item.FAX == null ? (object)DBNull.Value : item.FAX);
					sqlCommand.Parameters.AddWithValue("Geburtstag", item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
					sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Mobil", item.Mobil == null ? (object)DBNull.Value : item.Mobil);
					sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("PLZ", item.PLZ == null ? (object)DBNull.Value : item.PLZ);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Serienbrief", item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
					sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Straße", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
					sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("zu_Händen", item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Nr] FROM [Ansprechpartner] WHERE [Nr] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity item)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "UPDATE [Ansprechpartner] SET "
					+ " [Abteilung]=@Abteilung, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [auswahl_AB_BW]=@auswahl_AB_BW, "
					+ " [Bemerkung]=@Bemerkung, [Briefanrede]=@Briefanrede, [eMail]=@eMail, [FAX]=@FAX, [Geburtstag]=@Geburtstag, "
					+ " [Land]=@Land, [Mobil]=@Mobil, [Nummer]=@Nummer, [Ort]=@Ort, [PLZ]=@PLZ, [Position]=@Position, [Serienbrief]=@Serienbrief, "
					+ " [Sprache]=@Sprache, [Straße]=@Straße, [Telefon]=@Telefon, [Titel]=@Titel, [zu_Händen]=@zu_Händen "
					+ " WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);

				sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
				sqlCommand.Parameters.AddWithValue("auswahl_AB_BW", item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("eMail", item.EMail == null ? (object)DBNull.Value : item.EMail);
				sqlCommand.Parameters.AddWithValue("FAX", item.FAX == null ? (object)DBNull.Value : item.FAX);
				sqlCommand.Parameters.AddWithValue("Geburtstag", item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
				sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
				sqlCommand.Parameters.AddWithValue("Mobil", item.Mobil == null ? (object)DBNull.Value : item.Mobil);
				sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
				sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
				sqlCommand.Parameters.AddWithValue("PLZ", item.PLZ == null ? (object)DBNull.Value : item.PLZ);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("Serienbrief", item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
				sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
				sqlCommand.Parameters.AddWithValue("Straße", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
				sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
				sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
				sqlCommand.Parameters.AddWithValue("zu_Händen", item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Ansprechpartner] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [Ansprechpartner] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> GetByNummer(int nummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Ansprechpartner] WHERE [Nummer]=@nummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nummer", nummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> GetByNummers(List<int> nummers)
		{
			if(nummers != null && nummers.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> results = null;
				if(nummers.Count <= maxQueryNumber)
				{
					results = getByNummers(nummers);
				}
				else
				{
					int batchNumber = nummers.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByNummers(nummers.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByNummers(nummers.GetRange(batchNumber * maxQueryNumber, nummers.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> getByNummers(List<int> nummers)
		{
			if(nummers != null && nummers.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < nummers.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, nummers[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Ansprechpartner] WHERE [Nummer] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>();
		}

		public static int DeleteBySupplierAddress(int idSupplierAddress)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Ansprechpartner] WHERE [Nummer]=@idSupplierAddress";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("idSupplierAddress", idSupplierAddress);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
