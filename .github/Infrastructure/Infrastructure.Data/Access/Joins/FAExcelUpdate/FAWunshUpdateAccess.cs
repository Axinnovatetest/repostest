using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Joins.FAExcelUpdate
{
	public class FAWunshUpdateAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity> GetUpdatedWunshAdmin(string joint, int null_or_not, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string _complement = null_or_not == 1 ? "IS NULL" : "IS NOT NULL";
				//sqlConnection.Open();
				string query = "SELECT Fertigung.ID, Fertigung.Termin_Bestätigt1, Fertigung.Fertigungsnummer, Artikel.Artikelnummer, tblExcelImport.Termin" + " " +
							  "FROM Fertigung INNER JOIN" + " " +
							   "(" + joint + ")" + " " +
							   "tblExcelImport" + " " +
							   "ON Fertigung.Fertigungsnummer = tblExcelImport.Fertigungsnummer" + " " +
							   "INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]" + " " +
							   "WHERE tblExcelImport.Termin " + _complement + "";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity> GetUpdatedWunshUser(string joint, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				//sqlConnection.Open();

				string query = $@"SELECT Fertigung.ID, Fertigung.Termin_Bestätigt1, Fertigung.Kennzeichen, 
IIf(ISNULL([Kabel_geschnitten],0) = 0,0,1) AS Geschnitten,
IIf(ISNULL([Check_FAbegonnen],0) = 0, 0, 1) AS Begonnen, 
IIf(ISNULL([Check_Gewerk2_Teilweise],0) = 0, 0, 1) AS GT2, 
IIf(ISNULL([Check_Gewerk3_Teilweise],0) = 0, 0, 1) AS GT3,
IIf(ISNULL([Check_Gewerk1_Teilweise],0) = 0, 0, 1) AS GT1, 
IIf(ISNULL([Check_Gewerk1],0) = 0, 0, 1) AS G1, 
IIf(ISNULL([Check_Gewerk2],0) = 0, 0, 1) AS G2,
IIf(ISNULL([Check_Gewerk3],0) = 0, 0, 1) AS G3, 
Fertigung.gedruckt, Fertigung.FA_Druckdatum, tblExcelImport.Termin, Fertigung.Fertigungsnummer, Artikel.Artikelnummer,Fertigung.FA_Gestartet,Fertigung.Lagerort_id
FROM
(Fertigung INNER JOIN
({joint}) 
tblExcelImport ON Fertigung.Fertigungsnummer = tblExcelImport.Fertigungsnummer)
left JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
WHERE


(Fertigung.Kennzeichen = N'offen' AND 
IIf(ISNULL([Kabel_geschnitten],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_FAbegonnen],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk2_Teilweise],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk3_Teilweise],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk1_Teilweise],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk1],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk2],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk3],0) = 0, 0, 1) = 0 AND
Fertigung.gedruckt = 0 AND
Fertigung.FA_Druckdatum Is Null AND 
tblExcelImport.Termin >= GETDATE() + 21 AND
Fertigung.Lagerort_id <> 42 and Fertigung.Lagerort_id <> 7 and Fertigung.Lagerort_id <> 60 and Fertigung.Lagerort_id <> 102 and Fertigung.Lagerort_id <> 6 and Fertigung.Lagerort_id <> 26)
and Artikel.Artikelnummer is not null						 
OR

(Fertigung.Kennzeichen = N'offen' AND
IIf(ISNULL([Kabel_geschnitten],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_FAbegonnen],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk2_Teilweise],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk3_Teilweise],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk1_Teilweise],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk1],0) = 0, 0, 1) = 0 AND
IIf(ISNULL([Check_Gewerk2],0) = 0, 0, 1) = 0 AND 
IIf(ISNULL([Check_Gewerk3],0) = 0, 0, 1) = 0 AND
Fertigung.gedruckt = 0 AND 
Fertigung.FA_Druckdatum Is Null AND
tblExcelImport.Termin >= GETDATE() + 21 
AND (Fertigung.Lagerort_id = 42 or Fertigung.Lagerort_id = 6 or Fertigung.Lagerort_id = 60 or Fertigung.Lagerort_id = 102 or Fertigung.Lagerort_id = 7 or Fertigung.Lagerort_id = 26) AND
(Fertigung.FA_Gestartet = 0 Or Fertigung.FA_Gestartet Is Null))
and Artikel.Artikelnummer is not null";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList2(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity> GetNotUpdatedWunshUser(string joint, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				//sqlConnection.Open();

				string query = $@"SELECT Fertigung.ID,Fertigung.Termin_Bestätigt1, Fertigung.Kennzeichen, IIf(ISNULL([Kabel_geschnitten],0)=0,0,1) AS Geschnitten, 
IIf(ISNULL([Check_FAbegonnen],0)=0,0,1) AS Begonnen, IIf(ISNULL([Check_Gewerk2_Teilweise],0)=0,0,1) AS GT2,  
IIf(ISNULL([Check_Gewerk3_Teilweise],0)=0,0,1) AS GT3, IIf(ISNULL([Check_Gewerk1_Teilweise],0)=0,0,1) AS GT1,  
IIf(ISNULL([Check_Gewerk1],0)=0,0,1) AS G1, IIf(ISNULL([Check_Gewerk2],0)=0,0,1) AS G2,  
IIf(ISNULL([Check_Gewerk3],0)=0,0,1) AS G3, Fertigung.gedruckt, Fertigung.FA_Druckdatum, tblExcelImport.Termin, 
Fertigung.Fertigungsnummer, IIF(Artikel.Artikelnummer is null,'Not Found',Artikel.Artikelnummer) AS Artikelnummer, Fertigung.FA_Gestartet 
FROM
(Fertigung INNER JOIN
({joint})
tblExcelImport ON Fertigung.Fertigungsnummer = tblExcelImport.Fertigungsnummer) left JOIN Artikel 
ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
WHERE 
((Fertigung.Kennzeichen)<>N'offen') or
(((Fertigung.Kennzeichen)=N'offen') 
AND ((IIf(ISNULL([Kabel_geschnitten],0)=0,0,1))<>0))  OR 
(((IIf(ISNULL([Check_FAbegonnen],0)=0,0,1))<>0)) 
OR (((IIf(ISNULL([Check_Gewerk2_Teilweise],0)=0,0,1))<>0))  
OR (((IIf(ISNULL([Check_Gewerk3_Teilweise],0)=0,0,1))<>0)) 
OR (((IIf(ISNULL([Check_Gewerk1_Teilweise],0)=0,0,1))<>0))  
OR (((IIf(ISNULL([Check_Gewerk1],0)=0,0,1))<>0) 
AND ((IIf(ISNULL([Check_Gewerk2],0)=0,0,1))<>0))  
OR (((IIf(ISNULL([Check_Gewerk3],0)=0,0,1))<>0)) 
OR (((Fertigung.gedruckt)<>0))  OR 
((Not (Fertigung.FA_Druckdatum) Is Null)) 
OR (((tblExcelImport.Termin)<GETDATE()+21))  
OR (((Fertigung.FA_Gestartet)=1 
OR (Fertigung.FA_Gestartet)=-1) AND 
((Fertigung.Lagerort_id)=42 or(Fertigung.Lagerort_id)=7 or(Fertigung.Lagerort_id)=6 
or(Fertigung.Lagerort_id)=60 or(Fertigung.Lagerort_id)=102 or(Fertigung.Lagerort_id)=26))
or Artikel.Artikelnummer is null;";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toList2(dataTable);
			}
			else
			{
				return null;
			}
		}
		#region Helpers
		private static List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity(dataRow)); }
			return list;
		}
		private static List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity> toList2(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
