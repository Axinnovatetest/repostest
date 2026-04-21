using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.Statistics.MGO
{
	public class StatisticsAccess
	{
		#region Default Methods
		public static List<Entities.Tables.Statistics.MGO.GrundeEntity> GetGrunde()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT DISTINCT  [PSZ_Lagerentnahme Gründe].ID as Id, [PSZ_Lagerentnahme Gründe].Grund_D as Name
									FROM[PSZ_Lagerentnahme Gründe]
									ORDER BY[PSZ_Lagerentnahme Gründe].ID;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Statistics.MGO.GrundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Statistics.MGO.GrundeEntity>();
			}
		}

		public static List<Entities.Tables.Statistics.MGO.ArticleHistoryEntity> GetArticleHistory()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT top(1000) 
								[Id]
								,[LastUpdateUserId]
								,[LastUpdateUsername]
								,[LastUpdateUserFullName]
								,[LastUpdateTime]
								,[LogObjectId]
								,[LogObject]
								,[LogDescription]

								FROM [__MGO_ObjectLog]
								ORDER BY Id desc;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Statistics.MGO.ArticleHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Statistics.MGO.ArticleHistoryEntity>();
			}
		}


		public static List<Entities.Tables.Statistics.MGO.ReasonChangeCommitteeEntity> GetReasonChangeCommittee(System.DateTime from, System.DateTime to, string articleNumber, int? lagerId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT Lagerbewegungen.Typ,Lagerbewegungen.Id, Lagerbewegungen.Datum, Artikel.[Artikel-Nr], Artikel.Artikelnummer, Lagerbewegungen_Artikel.Anzahl, Lagerbewegungen_Artikel.Lager_von, Lagerbewegungen_Artikel.Grund " +
					"FROM Lagerbewegungen INNER JOIN (Lagerbewegungen_Artikel INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] " +
					"= Artikel.[Artikel-Nr]) ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id " +
					"WHERE Lagerbewegungen.Typ='entnahme' AND Lagerbewegungen.Datum>=@from And " +
					"Lagerbewegungen.Datum<=@to  "
					+ (string.IsNullOrWhiteSpace(articleNumber) ? string.Empty : "AND Artikel.Artikelnummer Like '%" + articleNumber + "%'")
					+ (lagerId is null || lagerId.Value == 0 ? string.Empty : "AND Lagerbewegungen_Artikel.Lager_von=@lagerId");

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);
				sqlCommand.Parameters.AddWithValue("from", from);
				sqlCommand.Parameters.AddWithValue("to", to);
				if(!string.IsNullOrWhiteSpace(articleNumber))
				{
					sqlCommand.Parameters.AddWithValue("articleNumber", articleNumber);
				}

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Statistics.MGO.ReasonChangeCommitteeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Statistics.MGO.ReasonChangeCommitteeEntity>();
			}
		}


		public static List<Entities.Tables.Statistics.MGO.SalesInjectionEntity> GetSalesInjection(System.DateTime from, System.DateTime to)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"WITH [PSZ_VOH-FG-VK und Produktionskosten tABELLE] AS 
					(
					SELECT [Fertigung_Fertigungsvorgang].[Anzahl]*[Verkaufspreis] AS VK, Fertigung_Fertigungsvorgang.Datum, Fertigung.FertigungsNummer, Fertigung_Fertigungsvorgang.Anzahl AS Menge, Artikel.Artikelnummer, Artikel.[Bezeichnung 1] as Bezeichnung, Fertigung.Originalanzahl, Fertigung.Anzahl_erledigt , Fertigung.Preis, Fertigung.Preis*Fertigung_Fertigungsvorgang.Anzahl AS Produktionskosten, 'Vohenstrauß' AS Ausdr1 
					FROM ((Fertigung_Fertigungsvorgang LEFT JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) LEFT JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
					WHERE (((Fertigung_Fertigungsvorgang.Datum)>=@from And (Fertigung_Fertigungsvorgang.Datum)<=@to) AND ((Fertigung.Lagerort_id)=15) AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0))
					)
					SELECT [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Artikelnummer, SUM([PSZ_VOH-FG-VK und Produktionskosten tABELLE].VK) AS VK, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Datum, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Fertigungsnummer, SUM([PSZ_VOH-FG-VK und Produktionskosten tABELLE].Menge) AS Menge, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].[Bezeichnung], [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Originalanzahl, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Anzahl_erledigt as AnzahlErledigt, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Preis, SUM([PSZ_VOH-FG-VK und Produktionskosten tABELLE].Produktionskosten) AS Produktionskosten, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Ausdr1 as Ausdr, 'Werkzeugbau' AS Produktionsbereich
					FROM
					[PSZ_VOH-FG-VK und Produktionskosten tABELLE]
					GROUP BY [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Artikelnummer, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Datum, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Fertigungsnummer, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].[Bezeichnung], [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Originalanzahl, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Anzahl_erledigt , [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Preis, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Ausdr1 
					HAVING ((([PSZ_VOH-FG-VK und Produktionskosten tABELLE].Artikelnummer) Like '987%'))
					UNION ALL SELECT [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Artikelnummer, SUM([PSZ_VOH-FG-VK und Produktionskosten tABELLE].VK) VK, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Datum, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Fertigungsnummer, SUM([PSZ_VOH-FG-VK und Produktionskosten tABELLE].Menge) AS Menge, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].[Bezeichnung], [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Originalanzahl, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Anzahl_erledigt, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Preis, SUM([PSZ_VOH-FG-VK und Produktionskosten tABELLE].Produktionskosten) AS Produktionskosten, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Ausdr1 as Ausdr, 'Spritzguß' AS Produktionsbereich
					FROM [PSZ_VOH-FG-VK und Produktionskosten tABELLE]
					GROUP BY [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Artikelnummer, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Datum, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Fertigungsnummer, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].[Bezeichnung], [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Originalanzahl, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Anzahl_erledigt, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Preis, [PSZ_VOH-FG-VK und Produktionskosten tABELLE].Ausdr1
					HAVING ((([PSZ_VOH-FG-VK und Produktionskosten tABELLE].Artikelnummer) Not Like '987%'));";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("from", from);
				sqlCommand.Parameters.AddWithValue("to", to);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.Statistics.MGO.SalesInjectionEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.Statistics.MGO.SalesInjectionEntity>();
			}
		}


		public static List<Entities.Tables.Statistics.MGO.PSZFGTNEntity> GetPSZFGTN(string countryCode, System.DateTime from, System.DateTime to, string prefixView)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');
				string table1Name = $"[##table1_{connectionId}]";
				string table2Name = $"[##table2_{connectionId}]";

				string query = $@"
							DROP TABLE IF EXISTS {table1Name};
							SELECT Fertigung.Preis, [PSZ{countryCode}_Lieferliste täglich].[Anzahl_aktuelle Lieferung]*Fertigung.Preis AS Arbeitskosten, [PSZ{countryCode}_Lieferliste täglich].Datum, [PSZ{countryCode}_Lieferliste täglich].Artikelnummer, Preisgruppen.Verkaufspreis, [PSZ{countryCode}_Lieferliste täglich].[Anzahl_aktuelle Lieferung]*Preisgruppen.verkaufspreis AS Umsatz_verkaufspreis, Artikel.Stundensatz, Artikel.Produktionszeit 
							INTO {table1Name}
							FROM [PSZ_Nummerschlüssel Kunde], (([PSZ{countryCode}_Lieferliste täglich] INNER JOIN Fertigung ON [PSZ{countryCode}_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer) INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
							WHERE ((([PSZ{countryCode}_Lieferliste täglich].Datum)>=@from And ([PSZ{countryCode}_Lieferliste täglich].Datum)<=@to) AND ((Left([PSZ{countryCode}_Lieferliste täglich].[Artikelnummer],3))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))
							ORDER BY [PSZ{countryCode}_Lieferliste täglich].Datum, [PSZ{countryCode}_Lieferliste täglich].Artikelnummer, [PSZ{countryCode}_Lieferliste täglich].Fertigungsnummer, [PSZ{countryCode}_Lieferliste täglich].Uhrzeit;
		
							DROP TABLE IF EXISTS {table2Name};								
							SELECT {table1Name}.Artikelnummer, Sum({table1Name}.Arbeitskosten) AS SummevonArbeitskosten, Sum({table1Name}.Umsatz_verkaufspreis) AS SummevonUmsatz_verkaufspreis, {table1Name}.Stundensatz, {table1Name}.Produktionszeit 
							INTO {table2Name}
							FROM {table1Name}
							GROUP BY {table1Name}.Artikelnummer, {table1Name}.Stundensatz, {table1Name}.Produktionszeit
							ORDER BY Sum({table1Name}.Umsatz_verkaufspreis) DESC;


							SELECT {table2Name}.Artikelnummer, {table2Name}.SummevonUmsatz_verkaufspreis AS [Umsatz PSZ_TN], {table2Name}.SummevonArbeitskosten AS [Arbeitskosten PSZ_TN], {table2Name}.Stundensatz, {table2Name}.Produktionszeit, [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel].[Marge ohne CU], [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel].[Marge mit CU], v.[Prod FA Zeit]*100 AS [Produktivität (FA Zeit)], v.[Prod Artikelzeit]*100 AS [Produktivität(Artikelzeit)]
							FROM ({table2Name} INNER JOIN [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel] ON {table2Name}.Artikelnummer = [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel].Artikelnummer) LEFT JOIN View_Produktivitat{prefixView} AS v ON {table2Name}.Artikelnummer = v.Artikelnummer
							ORDER BY {table2Name}.SummevonUmsatz_verkaufspreis DESC;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("from", from);
				sqlCommand.Parameters.AddWithValue("to", to);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.Statistics.MGO.PSZFGTNEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.Statistics.MGO.PSZFGTNEntity>();
			}
		}


		#endregion

		#region Custom Methods
		public static Entities.Tables.Statistics.MGO.ReasonChangeCommitteeEntity GetGrund(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT TOP 1 Lagerbewegungen.Typ,Lagerbewegungen.Id, Lagerbewegungen.Datum, Artikel.[Artikel-Nr], Artikel.Artikelnummer, Lagerbewegungen_Artikel.Anzahl, Lagerbewegungen_Artikel.Lager_von, Lagerbewegungen_Artikel.Grund " +
					"FROM Lagerbewegungen INNER JOIN (Lagerbewegungen_Artikel INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] " +
					"= Artikel.[Artikel-Nr]) ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id " +
					"WHERE Lagerbewegungen.Typ='entnahme' AND Lagerbewegungen.ID=@id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Statistics.MGO.ReasonChangeCommitteeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static KeyValuePair<int, int> GetDelKupferbasis()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT  ISNULL(DEL,0) DEL, Kupferbasis FROM 
				(SELECT TOP 1 Kupferbasis, [Artikel-Nr] FROM Artikel) AS A2,
				(SELECT TOP 1 ISNULL(DEL,0) AS DEL, [Artikel-Nr] FROM Artikel WHERE [DEL fixiert]=0) AS A1";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new KeyValuePair<int, int>(int.TryParse(dataTable.Rows[0][0].ToString(), out var del) ? del : 0,
					int.TryParse(dataTable.Rows[0][1].ToString(), out var kupf) ? kupf : 0);
			}
			else
			{
				return new KeyValuePair<int, int>(0, 0);
			}
		}

		public static int UpdateGrunde(int Id, int IdGrunde, Entities.Tables.PRS.ObjectLogEntity item)
		{
			int result = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"Update Lagerbewegungen_Artikel SET Grund=@IdGrunde Where Lagerbewegungen_id=@Lagerbewegungen_id;

								INSERT INTO [__MGO_ObjectLog] ([LastUpdateTime] ,[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId])
								VALUES (@LastUpdateTime,@LastUpdateUserFullName,@LastUpdateUserId,@LastUpdateUsername,@LogDescription,@LogObject,@LogObjectId);";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", Id);
				sqlCommand.Parameters.AddWithValue("IdGrunde", IdGrunde);

				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
				sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
				sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
				sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId);
				result = DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return result;
		}
		public static int UpdateArticle(int DEL, Entities.Tables.PRS.ObjectLogEntity item1, Entities.Tables.PRS.ObjectLogEntity item2, string LogDescription)
		{
			int result = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"UPDATE Artikel SET Artikel.DEL=@DEL WHERE Artikel.[DEL fixiert]=0; 
								UPDATE Artikel SET Artikel.Kupferbasis = 150;

								INSERT INTO  [__PRS_ObjectLog] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId])
								SELECT
										@LastUpdateTime1,@LastUpdateUserFullName1,@LastUpdateUserId1,@LastUpdateUsername1,@LogDescription1,@LogObject1,[Artikel-Nr]
								FROM Artikel Where [DEL fixiert]=0;

								INSERT INTO  [__PRS_ObjectLog] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId])
								SELECT
										@LastUpdateTime2,@LastUpdateUserFullName2,@LastUpdateUserId2,@LastUpdateUsername2,@LogDescription2,@LogObject2,[Artikel-Nr]
								FROM Artikel;

								INSERT INTO [__MGO_ObjectLog] ([LastUpdateTime] ,[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId])
								VALUES(@LastUpdateTime1, @LastUpdateUserFullName1, @LastUpdateUserId1, @LastUpdateUsername1, @LogDescription, @LogObject1,0);";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("DEL", DEL);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime1", item1.LastUpdateTime == null ? (object)DBNull.Value : item1.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName1", item1.LastUpdateUserFullName == null ? (object)DBNull.Value : item1.LastUpdateUserFullName);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId1", item1.LastUpdateUserId == null ? (object)DBNull.Value : item1.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUsername1", item1.LastUpdateUsername == null ? (object)DBNull.Value : item1.LastUpdateUsername);
				sqlCommand.Parameters.AddWithValue("LogDescription1", item1.LogDescription == null ? (object)DBNull.Value : item1.LogDescription);
				sqlCommand.Parameters.AddWithValue("LogObject1", item1.LogObject == null ? (object)DBNull.Value : item1.LogObject);
				sqlCommand.Parameters.AddWithValue("LogObjectId1", item1.LogObjectId);

				sqlCommand.Parameters.AddWithValue("LastUpdateTime2", item2.LastUpdateTime == null ? (object)DBNull.Value : item2.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName2", item2.LastUpdateUserFullName == null ? (object)DBNull.Value : item2.LastUpdateUserFullName);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId2", item2.LastUpdateUserId == null ? (object)DBNull.Value : item2.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUsername2", item2.LastUpdateUsername == null ? (object)DBNull.Value : item2.LastUpdateUsername);
				sqlCommand.Parameters.AddWithValue("LogDescription2", item2.LogDescription == null ? (object)DBNull.Value : item2.LogDescription);
				sqlCommand.Parameters.AddWithValue("LogObject2", item2.LogObject == null ? (object)DBNull.Value : item2.LogObject);
				sqlCommand.Parameters.AddWithValue("LogObjectId2", item2.LogObjectId);

				sqlCommand.Parameters.AddWithValue("LogDescription", LogDescription);

				result = DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return result;
		}

		#endregion
	}
}
