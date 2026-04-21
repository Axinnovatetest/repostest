using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class LagerBestandAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity> GetListeBestand()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*) over() as NombreTotal
                                , A.[artikel-Nr] as ArtikelNr
                               , A.Artikelnummer as artikelnummer
                               ,A.[Bezeichnung 1] as Bezeichnung1 
                               ,A.[Bezeichnung 2] as Bezeichnung2
                               , L.Bestand as Bestand
                               , LO.Lagerort, L.CCID, L.CCID_Datum
                               FROM (Lager L INNER JOIN Artikel A ON L.[Artikel-Nr] = A.[Artikel-Nr]) INNER JOIN Lagerorte LO ON L.Lagerort_id = LO.Lagerort_id
                               WHERE (((L.Bestand)<>0) AND ((LO.Lagerort) Not Like 'PL%') AND ((A.Lagerartikel)=1) AND ((A.aktiv)=1))
                               ORDER BY L.Bestand, A.Artikelnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity> GetPaginationListeBestand(string lagerort, int artikeNr, string SortFieldKey, bool SortDesc, Settings.PaginModel paging)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string option1 = "";
				string option2 = "";
				string orderBy = " ORDER BY L.Bestand, A.Artikelnummer";
				if(lagerort != null && lagerort != "")
				{
					option1 = $@" and (LO.Lagerort)='{lagerort}'";
				}
				if(artikeNr != null && artikeNr > 0)
				{
					option2 = $@" and A.[Artikel-Nr]={artikeNr}";
				}
				if(SortFieldKey == "artikelnummer" && SortDesc == true)
				{
					orderBy = " ORDER BY A.Artikelnummer desc,L.Bestand ";
				}
				else if(SortFieldKey == "artikelnummer" && SortDesc == false)
				{
					orderBy = " ORDER BY A.Artikelnummer asc,L.Bestand ";
				}
				if(SortFieldKey == "bestand" && SortDesc == true)
				{
					orderBy = " ORDER BY L.Bestand desc, A.Artikelnummer";
				}
				else if(SortFieldKey == "bestand" && SortDesc == false)
				{
					orderBy = " ORDER BY L.Bestand asc,A.Artikelnummer ";
				}
				//string query = $@"SELECT 
				//                            count(*) over() as NombreTotal
				//                           , A.[artikel-Nr] as ArtikelNr
				//                           , A.Artikelnummer as artikelnummer
				//                           ,A.[Bezeichnung 1] as Bezeichnung1 
				//                           ,A.[Bezeichnung 2] as Bezeichnung2
				//                           ,LO.Lagerort as lagerort
				//                           , L.Bestand as Bestand
				//                           , LO.Lagerort, L.CCID, L.CCID_Datum
				//                           FROM (Lager L INNER JOIN Artikel A ON L.[Artikel-Nr] = A.[Artikel-Nr]) INNER JOIN Lagerorte LO ON L.Lagerort_id = LO.Lagerort_id
				//                           WHERE (((L.Bestand)<>0) AND ((LO.Lagerort) Not Like 'PL%') AND ((A.Lagerartikel)=1) AND ((A.aktiv)=1)){option1}{option2}
				//                           ";
				string query = $@"SELECT 
                                100 as NombreTotal
                               , A.[artikel-Nr] as ArtikelNr
                               , A.Artikelnummer as artikelnummer
                               ,A.[Bezeichnung 1] as Bezeichnung1 
                               ,A.[Bezeichnung 2] as Bezeichnung2
                               ,LO.Lagerort as lagerort
                               , L.Bestand as Bestand
                               , LO.Lagerort, L.CCID, L.CCID_Datum
                               FROM (Lager L INNER JOIN Artikel A ON L.[Artikel-Nr] = A.[Artikel-Nr]) INNER JOIN Lagerorte LO ON L.Lagerort_id = LO.Lagerort_id
                               WHERE (((L.Bestand)<>0) AND ((LO.Lagerort) Not Like 'PL%') AND ((A.Lagerartikel)=1) AND ((A.aktiv)=1)){option1}{option2}
                                ";
				query += orderBy;
				query += $@" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity>();
			}
		}
		public static int GetCountListeBestand(string lagerort, int artikeNr)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string option1 = "";
				string option2 = "";
				string orderBy = " ORDER BY L.Bestand, A.Artikelnummer";
				if(lagerort != null && lagerort != "")
				{
					option1 = $@" and (LO.Lagerort)='{lagerort}'";
				}
				if(artikeNr != null && artikeNr > 0)
				{
					option2 = $@" and A.[Artikel-Nr]={artikeNr}";
				}
				string query = $@"SELECT 
                                count(*) as NombreTotal
                               FROM (Lager L INNER JOIN Artikel A ON L.[Artikel-Nr] = A.[Artikel-Nr]) INNER JOIN Lagerorte LO ON L.Lagerort_id = LO.Lagerort_id
                               WHERE (((L.Bestand)<>0) AND ((LO.Lagerort) Not Like 'PL%') AND ((A.Lagerartikel)=1) AND ((A.aktiv)=1)){option1}{option2}
                                ";


				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}


		}
		public static int updateMengeArtikelImLagerWithTransaction(int ArtikelNr, int lagerID, decimal menge, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			//using (var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			//{
			//  sqlConnection.Open();

			string query = "update[Lager] "
				+ " set [Bestand]+=@menge,[letzte Bewegung] = cast(getdate() as date)"
				+ " where [Artikel-Nr]=@ArtikelNr and Lagerort_id=@lagerID";

			//query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("menge", menge == null ? (object)DBNull.Value : menge);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", ArtikelNr == null ? (object)DBNull.Value : ArtikelNr);
			sqlCommand.Parameters.AddWithValue("lagerID", lagerID == null ? (object)DBNull.Value : lagerID);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int updateMengeArtikelToZeroImLagerWithTransaction(int ArtikelNr, int lagerID, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			//using (var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			//{
			//  sqlConnection.Open();

			string query = "update[Lager] "
				+ " set [Bestand]=0,[letzte Bewegung] = cast(getdate() as date)"
				+ " where [Artikel-Nr]=@ArtikelNr and Lagerort_id=@lagerID";

			//query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ArtikelNr", ArtikelNr == null ? (object)DBNull.Value : ArtikelNr);
			sqlCommand.Parameters.AddWithValue("lagerID", lagerID == null ? (object)DBNull.Value : lagerID);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity GetBestandArtikelBy(int lagerID, int artikeNr, SqlConnection connection, SqlTransaction transaction)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"select [Artikel-Nr] as ArtikelNr
                                 ,L.Lagerort_id as lagerID
                                 ,Bestand as Bestand
                                 ,Bestand_reserviert as ReserviertBestand
                                 ,GesamtBestand as GesamtBestand
                                  ,O.Lagerort as Lagerort
                                   from lager L inner join Lagerorte O on O.Lagerort_id=L.lagerort_id
                                  where [Artikel-Nr]={artikeNr} and L.Lagerort_id={lagerID}
                                ";


				var sqlCommand = new SqlCommand(query, connection, transaction);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity(x, 1)).First();
				}
				else
				{
					return new Infrastructure.Data.Entities.Joins.Logistics.LagerBestandEntity();
				}
			}


		}

	}
}
