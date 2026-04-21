using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.CRP
{
	public class FertigungFaultyTimeAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.MTM.CRP.FertigungFaultyTimeEntity> Get(List<int> lagerortId, DateTime? dateFrom, DateTime dateUntil)
		{
			if(lagerortId == null || lagerortId.Count <= 0)
				lagerortId = new List<int> { -100000 }; // - add a fake value

			if(!dateFrom.HasValue)
				dateFrom = new DateTime(DateTime.Today.Year, 1, 1);

			if(dateFrom > dateUntil)
				dateFrom = new DateTime(dateUntil.Year, 1, 1);

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT F.Termin_Bestätigt1 FaDate, F.Fertigungsnummer FaNumber, A.Artikelnummer FaArticle, F.Anzahl FaQuantity, F.Zeit  FaUnitTime, F.Zeit / 60  * F.Anzahl FaTotalTime, COALESCE(AA.Id, 0) WPL_ArticleId, COALESCE(W.Id,0) WorkScheduleId
	                                        FROM DBO.Fertigung F LEFT JOIN Artikel A ON A.[Artikel-Nr]=F.Artikel_Nr 
		                                        LEFT JOIN (SELECT * FROM Article WHERE Id NOT IN (SELECT Article_Id FROM WorkSchedule WHERE Is_Active=1)) AA ON AA.[Name]=A.Artikelnummer LEFT JOIN WorkSchedule W ON W.Article_Id=AA.Id
	                                        WHERE F.Kennzeichen='offen' AND F.Lagerort_id IN ({string.Join(",", lagerortId)}) AND F.Termin_Bestätigt1>=@dateFrom AND F.Termin_Bestätigt1<=@dateUntil ORDER BY F.Termin_Bestätigt1 ASC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateUntil", dateUntil);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.CRP.FertigungFaultyTimeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.CRP.FertigungFaultyTimeEntity>();
			}
		}
	}
}
