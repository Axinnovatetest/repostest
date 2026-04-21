using Infrastructure.Data.Entities.Tables.PRS;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.BSD
{
	public class DashboardAccess
	{
		public static List<FertigungPositionenEntity> GetByArticleInLager(int id, int? lagerId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT p.* FROM [Fertigung_Positionen] p Join Fertigung f ON f.ID=p.ID_Fertigung WHERE p.[Artikel_Nr]=@id /*AND ISNULL(f.FA_Gestartet,0)=0 -- 202212-20 - Schremmer */ AND f.Kennzeichen='offen' {(lagerId.HasValue == true ? $"AND f.lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FertigungPositionenEntity(x))?.ToList();
			}
			else
			{
				return new List<FertigungPositionenEntity>();
			}
		}
		public static List<FertigungPositionenEntity> GetOpenHbgFaByArticleInLager(int id, int? lagerId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT p.* FROM [Fertigung_Positionen] p Join Fertigung f ON f.ID=p.ID_Fertigung WHERE f.Kennzeichen = 'offen' AND p.[Artikel_Nr]=@id {(lagerId.HasValue == true ? $"AND p.lagerort_id={lagerId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FertigungPositionenEntity(x))?.ToList();
			}
			else
			{
				return new List<FertigungPositionenEntity>();
			}
		}
		public static List<AngeboteneArtikelEntity> GetOpenAbByArticleInLager(int id, int? lagerId, bool ignoreClosed = false, bool withoutFA = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT p.* FROM [Angebotene Artikel] p Join Angebote a ON a.Nr=p.[Angebot-Nr] WHERE a.[Typ] = 'Auftragsbestätigung' AND ISNULL(a.[erledigt],0)=0 AND p.[Artikel-Nr]=@id AND ISNULL(erledigt_pos,0)=0 {(ignoreClosed ? " AND [Anzahl]>0" : "")} {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")} {(withoutFA ? " AND ISNULL([Fertigungsnummer],0)=0" : "")} ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static List<AngeboteneArtikelEntity> GetOpenLsByArticleInLager(int id, int? lagerId, bool ignoreClosed = false, bool withoutFA = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT p.* FROM [Angebotene Artikel] p Join Angebote a ON a.Nr=p.[Angebot-Nr] WHERE a.[Typ] = 'Lieferschein' AND ISNULL(a.[erledigt],0)=0 AND p.[Artikel-Nr]=@id AND ISNULL(erledigt_pos,0)=0 {(ignoreClosed ? " AND [Anzahl]>0" : "")} {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")} {(withoutFA ? " AND ISNULL([Fertigungsnummer],0)=0" : "")} ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static List<AngeboteneArtikelEntity> GetOpenRaByArticleInLager(int id, int? lagerId, bool ignoreClosed = false, bool withoutFA = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT p.* FROM [Angebotene Artikel] p Join Angebote a ON a.Nr=p.[Angebot-Nr] WHERE a.[Typ] = 'Rahmenauftrag' AND ISNULL(a.[erledigt],0)=0 AND p.[Artikel-Nr]=@id AND ISNULL(erledigt_pos,0)=0 {(ignoreClosed ? " AND [Anzahl]>0" : "")} {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")} {(withoutFA ? " AND ISNULL([Fertigungsnummer],0)=0" : "")} ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}

		public static List<MinimalArtikelEntity> GetUniversalArticles()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Artikel] a JOIN [PSZ_Nummerschlüssel Kunde] k on CAST(k.Nummerschlüssel as nvarchar)=LEFT(a.Artikelnummer,3) WHERE LEN(a.Artikelnummer)>3 and a.Warengruppe='EF' AND k.[IsUniversal]=1;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new MinimalArtikelEntity(x))?.ToList();
			}
			else
			{
				return new List<MinimalArtikelEntity>();
			}
		}
	}
}
