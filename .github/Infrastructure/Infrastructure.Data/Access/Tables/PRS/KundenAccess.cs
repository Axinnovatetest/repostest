using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class KundenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.KundenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM Kunden WHERE Nr=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Kunden";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

			sqlCommand.CommandText = $"SELECT * FROM Kunden WHERE Nr IN ({queryIds})";


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.KundenEntity item)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO [Kunden] ([Belegkreis],[Bemerkungen],[Branche],[Bruttofakturierung],[Debitoren-Nr],[EG - Identifikationsnummer],[Eilzuschlag],[Factoring],[fibu_rahmen],[gesperrt für weitere Lieferungen],[Grund],[Grund für Sperre],[Karenztage],[Konditionszuordnungs-Nr],[Kreditlimit],[Kundengruppe],[Lieferantenummer (Kunden)],[Lieferscheinadresse],[LSADR],[LSADRANG],[LSADRAUF],[LSADRGUT],[LSADRPROF],[LSADRRG],[LSADRSTO],[LSRG],[Mahngebühr 1],[Mahngebühr 2],[Mahngebühr 3],[Mahnsperre],[Mindermengenzuschlag],[nummer],[OPOS],[Preisgruppe],[Preisgruppe2],[Rabattgruppe],[Regelmäßig anschreiben ?],[RG-Abteilung],[RG-Land/PLZ/ORT],[RG-Strasse-Postfach],[Sprache],[Standardversand],[Umsatzsteuer berechnen],[Versandart],[Verzugszinsen],[Verzugszinsen ab Mahnstufe],[Währung],[Zahlung erwartet nach],[Zahlungskondition],[Zahlungsmoral],[Zahlungsweise],[Zielaufschlag],[zolltarif_nr])  VALUES (@Belegkreis,@Bemerkungen,@Branche,@Bruttofakturierung,@Debitoren_Nr,@EG___Identifikationsnummer,@Eilzuschlag,@Factoring,@fibu_rahmen,@gesperrt_für_weitere_Lieferungen,@Grund,@Grund_für_Sperre,@Karenztage,@Konditionszuordnungs_Nr,@Kreditlimit,@Kundengruppe,@Lieferantenummer__Kunden_,@Lieferscheinadresse,@LSADR,@LSADRANG,@LSADRAUF,@LSADRGUT,@LSADRPROF,@LSADRRG,@LSADRSTO,@LSRG,@Mahngebühr_1,@Mahngebühr_2,@Mahngebühr_3,@Mahnsperre,@Mindermengenzuschlag,@nummer,@OPOS,@Preisgruppe,@Preisgruppe2,@Rabattgruppe,@Regelmäßig_anschreiben__,@RG_Abteilung,@RG_Land_PLZ_ORT,@RG_Strasse_Postfach,@Sprache,@Standardversand,@Umsatzsteuer_berechnen,@Versandart,@Verzugszinsen,@Verzugszinsen_ab_Mahnstufe,@Währung,@Zahlung_erwartet_nach,@Zahlungskondition,@Zahlungsmoral,@Zahlungsweise,@Zielaufschlag,@zolltarif_nr,@CodeTypeInLS,@CodeTypeInLSId);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
			sqlCommand.Parameters.AddWithValue("Bruttofakturierung", item.Bruttofakturierung == null ? (object)DBNull.Value : item.Bruttofakturierung);
			sqlCommand.Parameters.AddWithValue("Debitoren_Nr", item.Debitoren_Nr == null ? (object)DBNull.Value : item.Debitoren_Nr);
			sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
			sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
			sqlCommand.Parameters.AddWithValue("Factoring", item.Factoring == null ? (object)DBNull.Value : item.Factoring);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Lieferungen", item.gesperrt_für_weitere_Lieferungen == null ? (object)DBNull.Value : item.gesperrt_für_weitere_Lieferungen);
			sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
			sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
			sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
			sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
			sqlCommand.Parameters.AddWithValue("Kreditlimit", item.Kreditlimit == null ? (object)DBNull.Value : item.Kreditlimit);
			sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);
			sqlCommand.Parameters.AddWithValue("Lieferantenummer__Kunden_", item.Lieferantenummer__Kunden_ == null ? (object)DBNull.Value : item.Lieferantenummer__Kunden_);
			sqlCommand.Parameters.AddWithValue("Lieferscheinadresse", item.Lieferscheinadresse == null ? (object)DBNull.Value : item.Lieferscheinadresse);
			sqlCommand.Parameters.AddWithValue("LSADR", item.LSADR == null ? (object)DBNull.Value : item.LSADR);
			sqlCommand.Parameters.AddWithValue("LSADRANG", item.LSADRANG == null ? (object)DBNull.Value : item.LSADRANG);
			sqlCommand.Parameters.AddWithValue("LSADRAUF", item.LSADRAUF == null ? (object)DBNull.Value : item.LSADRAUF);
			sqlCommand.Parameters.AddWithValue("LSADRGUT", item.LSADRGUT == null ? (object)DBNull.Value : item.LSADRGUT);
			sqlCommand.Parameters.AddWithValue("LSADRPROF", item.LSADRPROF == null ? (object)DBNull.Value : item.LSADRPROF);
			sqlCommand.Parameters.AddWithValue("LSADRRG", item.LSADRRG == null ? (object)DBNull.Value : item.LSADRRG);
			sqlCommand.Parameters.AddWithValue("LSADRSTO", item.LSADRSTO == null ? (object)DBNull.Value : item.LSADRSTO);
			sqlCommand.Parameters.AddWithValue("LSRG", item.LSRG == null ? (object)DBNull.Value : item.LSRG);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_1", item.Mahngebühr_1 == null ? (object)DBNull.Value : item.Mahngebühr_1);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_2", item.Mahngebühr_2 == null ? (object)DBNull.Value : item.Mahngebühr_2);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_3", item.Mahngebühr_3 == null ? (object)DBNull.Value : item.Mahngebühr_3);
			sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
			sqlCommand.Parameters.AddWithValue("Mindermengenzuschlag", item.Mindermengenzuschlag == null ? (object)DBNull.Value : item.Mindermengenzuschlag);
			sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("OPOS", item.OPOS == null ? (object)DBNull.Value : item.OPOS);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Preisgruppe2", item.Preisgruppe2 == null ? (object)DBNull.Value : item.Preisgruppe2);
			sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
			sqlCommand.Parameters.AddWithValue("Regelmäßig_anschreiben__", item.Regelmäßig_anschreiben__ == null ? (object)DBNull.Value : item.Regelmäßig_anschreiben__);
			sqlCommand.Parameters.AddWithValue("RG_Abteilung", item.RG_Abteilung == null ? (object)DBNull.Value : item.RG_Abteilung);
			sqlCommand.Parameters.AddWithValue("RG_Land_PLZ_ORT", item.RG_Land_PLZ_ORT == null ? (object)DBNull.Value : item.RG_Land_PLZ_ORT);
			sqlCommand.Parameters.AddWithValue("RG_Strasse_Postfach", item.RG_Strasse_Postfach == null ? (object)DBNull.Value : item.RG_Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			sqlCommand.Parameters.AddWithValue("Standardversand", item.Standardversand == null ? (object)DBNull.Value : item.Standardversand);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen", item.Verzugszinsen == null ? (object)DBNull.Value : item.Verzugszinsen);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen_ab_Mahnstufe", item.Verzugszinsen_ab_Mahnstufe == null ? (object)DBNull.Value : item.Verzugszinsen_ab_Mahnstufe);
			sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);
			sqlCommand.Parameters.AddWithValue("Zahlung_erwartet_nach", item.Zahlung_erwartet_nach == null ? (object)DBNull.Value : item.Zahlung_erwartet_nach);
			sqlCommand.Parameters.AddWithValue("Zahlungskondition", item.Zahlungskondition == null ? (object)DBNull.Value : item.Zahlungskondition);
			sqlCommand.Parameters.AddWithValue("Zahlungsmoral", item.Zahlungsmoral == null ? (object)DBNull.Value : item.Zahlungsmoral);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
			sqlCommand.Parameters.AddWithValue("zolltarif_nr", item.zolltarif_nr == null ? (object)DBNull.Value : item.zolltarif_nr);
			// 18-03/2025 ticket #43321
			sqlCommand.Parameters.AddWithValue("CodeTypeInLS", item.CodeTypeInLS == null ? (object)DBNull.Value : item.CodeTypeInLS);
			sqlCommand.Parameters.AddWithValue("CodeTypeInLSId", item.CodeTypeInLSId == null ? (object)DBNull.Value : item.CodeTypeInLSId);


			var result = sqlCommand.ExecuteScalar();
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.KundenEntity item)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [DEL-Fixiert]=@DELFixiert,[DEL]=@DEL,[Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Branche]=@Branche, [Bruttofakturierung]=@Bruttofakturierung, [Debitoren-Nr]=@Debitoren_Nr, [EG - Identifikationsnummer]=@EG___Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Factoring]=@Factoring, [fibu_rahmen]=@fibu_rahmen, [gesperrt für weitere Lieferungen]=@gesperrt_für_weitere_Lieferungen, [Grund]=@Grund, [Grund für Sperre]=@Grund_für_Sperre, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditlimit]=@Kreditlimit, [Kundengruppe]=@Kundengruppe, [Lieferantenummer (Kunden)]=@Lieferantenummer__Kunden_, [Lieferscheinadresse]=@Lieferscheinadresse, [Mahngebühr 1]=@Mahngebühr_1, [Mahngebühr 2]=@Mahngebühr_2, [Mahngebühr 3]=@Mahngebühr_3, [Mahnsperre]=@Mahnsperre, [Mindermengenzuschlag]=@Mindermengenzuschlag, [nummer]=@nummer, [OPOS]=@OPOS, [Preisgruppe]=@Preisgruppe, [Preisgruppe2]=@Preisgruppe2, [Rabattgruppe]=@Rabattgruppe, [Regelmäßig anschreiben ?]=@Regelmäßig_anschreiben__, [RG-Abteilung]=@RG_Abteilung, [RG-Land/PLZ/ORT]=@RG_Land_PLZ_ORT, [RG-Strasse-Postfach]=@RG_Strasse_Postfach, [Sprache]=@Sprache,[Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Verzugszinsen]=@Verzugszinsen, [Währung]=@Währung,[Verzugszinsen ab Mahnstufe]=@Verzugszinsen_ab_Mahnstufe, [Zahlung erwartet nach]=@Zahlung_erwartet_nach, [Zahlungskondition]=@Zahlungskondition, [Zahlungsmoral]=@Zahlungsmoral, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [zolltarif_nr]=@zolltarif_nr,[CodeTypeInLS]=@CodeTypeInLS,[CodeTypeInLSId]=@CodeTypeInLSId WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
			sqlCommand.Parameters.AddWithValue("Bruttofakturierung", item.Bruttofakturierung == null ? (object)DBNull.Value : item.Bruttofakturierung);
			sqlCommand.Parameters.AddWithValue("Debitoren_Nr", item.Debitoren_Nr == null ? (object)DBNull.Value : item.Debitoren_Nr);
			sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
			sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
			sqlCommand.Parameters.AddWithValue("Factoring", item.Factoring == null ? (object)DBNull.Value : item.Factoring);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Lieferungen", item.gesperrt_für_weitere_Lieferungen == null ? (object)DBNull.Value : item.gesperrt_für_weitere_Lieferungen);
			sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
			sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
			sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
			sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
			sqlCommand.Parameters.AddWithValue("Kreditlimit", item.Kreditlimit == null ? (object)DBNull.Value : item.Kreditlimit);
			sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);
			sqlCommand.Parameters.AddWithValue("Lieferantenummer__Kunden_", item.Lieferantenummer__Kunden_ == null ? (object)DBNull.Value : item.Lieferantenummer__Kunden_);
			sqlCommand.Parameters.AddWithValue("Lieferscheinadresse", item.Lieferscheinadresse == null ? (object)DBNull.Value : item.Lieferscheinadresse);
			//sqlCommand.Parameters.AddWithValue("LSADR", item.LSADR == null ? (object)DBNull.Value : item.LSADR);
			//sqlCommand.Parameters.AddWithValue("LSADRANG", item.LSADRANG == null ? (object)DBNull.Value : item.LSADRANG);
			//sqlCommand.Parameters.AddWithValue("LSADRAUF", item.LSADRAUF == null ? (object)DBNull.Value : item.LSADRAUF);
			//sqlCommand.Parameters.AddWithValue("LSADRGUT", item.LSADRGUT == null ? (object)DBNull.Value : item.LSADRGUT);
			//sqlCommand.Parameters.AddWithValue("LSADRPROF", item.LSADRPROF == null ? (object)DBNull.Value : item.LSADRPROF);
			//sqlCommand.Parameters.AddWithValue("LSADRRG", item.LSADRRG == null ? (object)DBNull.Value : item.LSADRRG);
			//sqlCommand.Parameters.AddWithValue("LSADRSTO", item.LSADRSTO == null ? (object)DBNull.Value : item.LSADRSTO);
			//sqlCommand.Parameters.AddWithValue("LSRG", item.LSRG == null ? (object)DBNull.Value : item.LSRG);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_1", item.Mahngebühr_1 == null ? (object)DBNull.Value : item.Mahngebühr_1);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_2", item.Mahngebühr_2 == null ? (object)DBNull.Value : item.Mahngebühr_2);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_3", item.Mahngebühr_3 == null ? (object)DBNull.Value : item.Mahngebühr_3);
			sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
			sqlCommand.Parameters.AddWithValue("Mindermengenzuschlag", item.Mindermengenzuschlag == null ? (object)DBNull.Value : item.Mindermengenzuschlag);
			sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("OPOS", item.OPOS == null ? (object)DBNull.Value : item.OPOS);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Preisgruppe2", item.Preisgruppe2 == null ? (object)DBNull.Value : item.Preisgruppe2);
			sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
			sqlCommand.Parameters.AddWithValue("Regelmäßig_anschreiben__", item.Regelmäßig_anschreiben__ == null ? (object)DBNull.Value : item.Regelmäßig_anschreiben__);
			sqlCommand.Parameters.AddWithValue("RG_Abteilung", item.RG_Abteilung == null ? (object)DBNull.Value : item.RG_Abteilung);
			sqlCommand.Parameters.AddWithValue("RG_Land_PLZ_ORT", item.RG_Land_PLZ_ORT == null ? (object)DBNull.Value : item.RG_Land_PLZ_ORT);
			sqlCommand.Parameters.AddWithValue("RG_Strasse_Postfach", item.RG_Strasse_Postfach == null ? (object)DBNull.Value : item.RG_Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			//sqlCommand.Parameters.AddWithValue("Standardversand", item.Standardversand == null ? (object)DBNull.Value : item.Standardversand);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen", item.Verzugszinsen == null ? (object)DBNull.Value : item.Verzugszinsen);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen_ab_Mahnstufe", item.Verzugszinsen_ab_Mahnstufe == null ? (object)DBNull.Value : item.Verzugszinsen_ab_Mahnstufe);
			sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);
			sqlCommand.Parameters.AddWithValue("Zahlung_erwartet_nach", item.Zahlung_erwartet_nach == null ? (object)DBNull.Value : item.Zahlung_erwartet_nach);
			sqlCommand.Parameters.AddWithValue("Zahlungskondition", item.Zahlungskondition == null ? (object)DBNull.Value : item.Zahlungskondition);
			sqlCommand.Parameters.AddWithValue("Zahlungsmoral", item.Zahlungsmoral == null ? (object)DBNull.Value : item.Zahlungsmoral);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
			sqlCommand.Parameters.AddWithValue("zolltarif_nr", item.zolltarif_nr == null ? (object)DBNull.Value : item.zolltarif_nr);
			sqlCommand.Parameters.AddWithValue("DELFixiert", item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			// 18-03/2025 ticket #43321
			sqlCommand.Parameters.AddWithValue("CodeTypeInLS", item.CodeTypeInLS == null ? (object)DBNull.Value : item.CodeTypeInLS);
			sqlCommand.Parameters.AddWithValue("CodeTypeInLSId", item.CodeTypeInLSId == null ? (object)DBNull.Value : item.CodeTypeInLSId);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}

		public static int UpdateLanguage(int Nr, int languageId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [Sprache]=@Sprache WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", Nr);
			sqlCommand.Parameters.AddWithValue("Sprache", languageId);
			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int UpdateShipping(Infrastructure.Data.Entities.Tables.PRS.KundenEntity item)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [LSADR]=@LSADR, [LSADRANG]=@LSADRANG, [LSADRAUF]=@LSADRAUF, [LSADRGUT]=@LSADRGUT, [LSADRPROF]=@LSADRPROF, [LSADRRG]=@LSADRRG, [LSADRSTO]=@LSADRSTO,[LSRG]=@LSRG,[Standardversand]=@Standardversand WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("LSADR", item.LSADR == null ? (object)DBNull.Value : item.LSADR);
			sqlCommand.Parameters.AddWithValue("LSADRANG", item.LSADRANG == null ? (object)DBNull.Value : item.LSADRANG);
			sqlCommand.Parameters.AddWithValue("LSADRAUF", item.LSADRAUF == null ? (object)DBNull.Value : item.LSADRAUF);
			sqlCommand.Parameters.AddWithValue("LSADRGUT", item.LSADRGUT == null ? (object)DBNull.Value : item.LSADRGUT);
			sqlCommand.Parameters.AddWithValue("LSADRPROF", item.LSADRPROF == null ? (object)DBNull.Value : item.LSADRPROF);
			sqlCommand.Parameters.AddWithValue("LSADRRG", item.LSADRRG == null ? (object)DBNull.Value : item.LSADRRG);
			sqlCommand.Parameters.AddWithValue("LSADRSTO", item.LSADRSTO == null ? (object)DBNull.Value : item.LSADRSTO);
			sqlCommand.Parameters.AddWithValue("LSRG", item.LSRG == null ? (object)DBNull.Value : item.LSRG);
			sqlCommand.Parameters.AddWithValue("Standardversand", item.Standardversand == null ? (object)DBNull.Value : item.Standardversand);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int UpdateIndustryCascade(string oldBranche, string newBranche)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [Branche]=@newBranche where [Branche]=@oldBranche";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("newBranche", newBranche);
			sqlCommand.Parameters.AddWithValue("oldBranche", oldBranche);
			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int UpdateShippingMethodCascade(string oldShipping, string newShipping)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [Standardversand]=@newShipping where [Standardversand]=@oldShipping";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("oldShipping", oldShipping);
			sqlCommand.Parameters.AddWithValue("newShipping", newShipping);
			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM Kunden WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Nr", id);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConection.Close();

			return response;
		}
		#endregion

		#region Custom Methods
		public static int UpdateOverview(Infrastructure.Data.Entities.Tables.PRS.KundenEntity item)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [Branche]=@Branche,[Kundengruppe]=@Kundengruppe WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);

			sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
			sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.KundenEntity GetByNummer(int nummer)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Kunden WHERE nummer=@nummer";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("nummer", nummer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.KundenEntity GetByNummer(int nummer, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "SELECT * FROM Kunden WHERE nummer=@nummer";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("nummer", nummer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByNummers(IEnumerable<int> nummers)
		{
			if(nummers == null || nummers.Count() == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(nummers.Count() <= max)
			{
				return getByNummers(nummers);
			}

			int batchNumber = nummers.Count() / max;
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(getByNummers(nummers.Skip(i * max).Take(max)));
			}

			// Handle the remaining items (if any)
			int remaining = nummers.Count() - batchNumber * max;
			if(remaining > 0)
			{
				result.AddRange(getByNummers(nummers.Skip(batchNumber * max).Take(remaining)));
			}
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> getByNummers(IEnumerable<int> nummers)
		{
			if(nummers == null || nummers.Count() == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;

			string queryIds = string.Empty;
			for(int i = 0; i < nummers.Count(); i++)
			{
				queryIds += nummers.ElementAt(i) + ",";
				//queryIds += "@Id" + i + ",";
				//sqlCommand.Parameters.AddWithValue("Id" + i, nummers[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = $"SELECT * FROM Kunden WHERE nummer IN ({queryIds})";


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static int GetMaxNummber()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT MAX([nummer]) AS MaxNummer FROM [Kunden]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return 0;
			}

			var maxNummer = (dataTable.Rows[0]["MaxNummer"] == System.DBNull.Value)
				? (int?)null
				: Convert.ToInt32(dataTable.Rows[0]["MaxNummer"]);

			return maxNummer ?? 0;
		}
		public static bool GetCustomerWithDiscoutGroup(int discountGroupId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where [Rabattgruppe]=@discountGroupId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("discountGroupId", discountGroupId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByDiscoutGroup(int discountGroupId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where [Rabattgruppe]=@discountGroupId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("discountGroupId", discountGroupId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithConditionAssignement(int conditionAssignement)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where [Konditionszuordnungs-Nr]=@conditionAssignement";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("conditionAssignement", conditionAssignement);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByConditionAssignement(int conditionAssignement)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where [Konditionszuordnungs-Nr]=@conditionAssignement";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("conditionAssignement", conditionAssignement);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithIndustry(string industry)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Branche=@industry";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("industry", industry);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByIndustry(string industry)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where Branche=@industry";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("industry", industry);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithCustomerGroup(int group)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Kundengruppe=@group";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("group", group);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByCustomerGroup(int group)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where Kundengruppe=@group";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("group", group);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithPayementPractice(int practice)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Zahlungsmoral=@practice";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("practice", practice);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByPayementPractice(int practice)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where Zahlungsmoral=@practice";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("practice", practice);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithTermOfPayement(int term)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Zahlungskondition=@term";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("term", term);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByTermOfPayement(int term)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where Zahlungskondition=@term";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("term", term);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithCurrency(int currency)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Währung=@currency";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("currency", currency);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByCurrency(int currency)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where Währung=@currency";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("currency", currency);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithSlipCircle(int circle)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Belegkreis=@circle";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("circle", circle);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetBySlipCircle(int circle)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where Belegkreis=@circle";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("circle", circle);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithPricingGroup(int price)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Preisgruppe=@price or Preisgruppe2=@price";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("price", price);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByPricingGroup(int price)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Kunden] where Preisgruppe=@price or Preisgruppe2=@price";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("price", price);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithFibuFrame(int farme)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where fibu_rahmen=@farme";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("farme", farme);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByFibuFrame(int farme)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where fibu_rahmen=@farme";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("farme", farme);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static bool GetCustomerWithShippingMethod(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Kunden] where Standardversand=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByShippingMethod(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Kunden] where Standardversand=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> SearchByNumberName(
			int? nummer,
			string name,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT kd.* FROM [adressen] AS ad LEFT JOIN [Kunden] AS kd ON ad.Nr = kd.nummer ";


				using(var sqlCommand = new SqlCommand())
				{
					if(nummer.HasValue)
					{
						//query += " WHERE [kundennummer]=@nummer AND kd.nr is not null and ad.Adresstyp=1";
						query += " WHERE [kundennummer]=@nummer AND kd.nr is not null";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						//query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null and ad.Adresstyp=1";
						query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null";

					}

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY [kundennummer] DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			return toList(dataTable);
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> SearchByNumberNameV2(
			int? nummer,
			string name,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT kd.* FROM [adressen] AS ad LEFT JOIN [Kunden] AS kd ON ad.Nr = kd.nummer ";


				using(var sqlCommand = new SqlCommand())
				{
					if(nummer.HasValue)
					{
						//query += " WHERE [kundennummer]=@nummer AND kd.nr is not null and ad.Adresstyp=1";
						query += " WHERE ad.Nr=@nummer AND kd.nr is not null";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						query += $" WHERE ad.Nr = '{name}' AND ad.Kundennummer is not null AND kd.nr is not null";

					}

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY [kundennummer] DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			return toList(dataTable);
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> SearchArchived(
			int? nummer,
			string name,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT kd.* FROM [adressen] AS ad LEFT JOIN [Kunden] AS kd ON ad.Nr = kd.nummer
                               inner join __BSD_KundenExtension X on kd.Nr=X.Nr
                               where ad.Adresstyp=1 and X.IsArchived=1";
				using(var sqlCommand = new SqlCommand())
				{
					if(nummer.HasValue)
					{
						//query += " AND [kundennummer]=@nummer AND kd.nr is not null and ad.Adresstyp=1";
						query += " AND [kundennummer]=@nummer AND kd.nr is not null";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						//query += $" AND (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null and ad.Adresstyp=1";
						query += $" AND (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null";

					}

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY [kundennummer] DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			return toList(dataTable);
		}
		public static int SearchByNumberName_CountAll(int? nummer, string name)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [adressen] AS ad LEFT JOIN [Kunden] AS kd ON ad.Nr = kd.nummer ";

				using(var sqlCommand = new SqlCommand())
				{

					if(nummer.HasValue)
					{
						//query += " WHERE [kundennummer]=@nummer AND kd.nr is not null and ad.Adresstyp=1";
						query += " WHERE [kundennummer]=@nummer AND kd.nr is not null";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						//query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null and ad.Adresstyp=1";
						query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null";

					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}


		public static List<Entities.Tables.PRS.KundenEntity> GetLikeNumber(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [Kunden] WHERE Nr IS NOT NULL AND [nummer] LIKE '{nummer}%' ORDER by [nummer] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.KundenEntity GetByAddressNr(int nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [Kunden] WHERE Nr IS NOT NULL AND [nummer]={nummer} ORDER by [nummer] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.KundenEntity GetByAddressNr(int nummer, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("", sqlConnection, sqlTransaction))
			{
				sqlCommand.CommandText = $"SELECT * FROM [Kunden] WHERE Nr IS NOT NULL AND [nummer]={nummer} ORDER by [nummer] ASC";
				sqlCommand.Connection = sqlConnection;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.KundenEntity> GetByAddressNr(List<int> nummers)
		{
			if(nummers == null || nummers.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [Kunden] WHERE Nr IS NOT NULL AND [nummer] IN ({string.Join(",", nummers)}) ORDER by [nummer] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<string> GetBranches()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT Branche FROM [Kunden]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["Branche"].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<string> GetGroups()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT Kundengruppe FROM [Kunden]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["Kundengruppe"].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.KundenEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "INSERT INTO [Kunden] ([Belegkreis],[Bemerkungen],[Branche],[Bruttofakturierung],[Debitoren-Nr],[EG - Identifikationsnummer],[Eilzuschlag],[Factoring],[fibu_rahmen],[gesperrt für weitere Lieferungen],[Grund],[Grund für Sperre],[Karenztage],[Konditionszuordnungs-Nr],[Kreditlimit],[Kundengruppe],[Lieferantenummer (Kunden)],[Lieferscheinadresse],[LSADR],[LSADRANG],[LSADRAUF],[LSADRGUT],[LSADRPROF],[LSADRRG],[LSADRSTO],[LSRG],[Mahngebühr 1],[Mahngebühr 2],[Mahngebühr 3],[Mahnsperre],[Mindermengenzuschlag],[nummer],[OPOS],[Preisgruppe],[Preisgruppe2],[Rabattgruppe],[Regelmäßig anschreiben ?],[RG-Abteilung],[RG-Land/PLZ/ORT],[RG-Strasse-Postfach],[Sprache],[Standardversand],[Umsatzsteuer berechnen],[Versandart],[Verzugszinsen],[Verzugszinsen ab Mahnstufe],[Währung],[Zahlung erwartet nach],[Zahlungskondition],[Zahlungsmoral],[Zahlungsweise],[Zielaufschlag],[zolltarif_nr],[CodeTypeInLS],[CodeTypeInLSId])  VALUES (@Belegkreis,@Bemerkungen,@Branche,@Bruttofakturierung,@Debitoren_Nr,@EG___Identifikationsnummer,@Eilzuschlag,@Factoring,@fibu_rahmen,@gesperrt_für_weitere_Lieferungen,@Grund,@Grund_für_Sperre,@Karenztage,@Konditionszuordnungs_Nr,@Kreditlimit,@Kundengruppe,@Lieferantenummer__Kunden_,@Lieferscheinadresse,@LSADR,@LSADRANG,@LSADRAUF,@LSADRGUT,@LSADRPROF,@LSADRRG,@LSADRSTO,@LSRG,@Mahngebühr_1,@Mahngebühr_2,@Mahngebühr_3,@Mahnsperre,@Mindermengenzuschlag,@nummer,@OPOS,@Preisgruppe,@Preisgruppe2,@Rabattgruppe,@Regelmäßig_anschreiben__,@RG_Abteilung,@RG_Land_PLZ_ORT,@RG_Strasse_Postfach,@Sprache,@Standardversand,@Umsatzsteuer_berechnen,@Versandart,@Verzugszinsen,@Verzugszinsen_ab_Mahnstufe,@Währung,@Zahlung_erwartet_nach,@Zahlungskondition,@Zahlungsmoral,@Zahlungsweise,@Zielaufschlag,@zolltarif_nr,@CodeTypeInLS,@CodeTypeInLSId);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
			sqlCommand.Parameters.AddWithValue("Bruttofakturierung", item.Bruttofakturierung == null ? (object)DBNull.Value : item.Bruttofakturierung);
			sqlCommand.Parameters.AddWithValue("Debitoren_Nr", item.Debitoren_Nr == null ? (object)DBNull.Value : item.Debitoren_Nr);
			sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
			sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
			sqlCommand.Parameters.AddWithValue("Factoring", item.Factoring == null ? (object)DBNull.Value : item.Factoring);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Lieferungen", item.gesperrt_für_weitere_Lieferungen == null ? (object)DBNull.Value : item.gesperrt_für_weitere_Lieferungen);
			sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
			sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
			sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
			sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
			sqlCommand.Parameters.AddWithValue("Kreditlimit", item.Kreditlimit == null ? (object)DBNull.Value : item.Kreditlimit);
			sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);
			sqlCommand.Parameters.AddWithValue("Lieferantenummer__Kunden_", item.Lieferantenummer__Kunden_ == null ? (object)DBNull.Value : item.Lieferantenummer__Kunden_);
			sqlCommand.Parameters.AddWithValue("Lieferscheinadresse", item.Lieferscheinadresse == null ? (object)DBNull.Value : item.Lieferscheinadresse);
			sqlCommand.Parameters.AddWithValue("LSADR", item.LSADR == null ? (object)DBNull.Value : item.LSADR);
			sqlCommand.Parameters.AddWithValue("LSADRANG", item.LSADRANG == null ? (object)DBNull.Value : item.LSADRANG);
			sqlCommand.Parameters.AddWithValue("LSADRAUF", item.LSADRAUF == null ? (object)DBNull.Value : item.LSADRAUF);
			sqlCommand.Parameters.AddWithValue("LSADRGUT", item.LSADRGUT == null ? (object)DBNull.Value : item.LSADRGUT);
			sqlCommand.Parameters.AddWithValue("LSADRPROF", item.LSADRPROF == null ? (object)DBNull.Value : item.LSADRPROF);
			sqlCommand.Parameters.AddWithValue("LSADRRG", item.LSADRRG == null ? (object)DBNull.Value : item.LSADRRG);
			sqlCommand.Parameters.AddWithValue("LSADRSTO", item.LSADRSTO == null ? (object)DBNull.Value : item.LSADRSTO);
			sqlCommand.Parameters.AddWithValue("LSRG", item.LSRG == null ? (object)DBNull.Value : item.LSRG);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_1", item.Mahngebühr_1 == null ? (object)DBNull.Value : item.Mahngebühr_1);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_2", item.Mahngebühr_2 == null ? (object)DBNull.Value : item.Mahngebühr_2);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_3", item.Mahngebühr_3 == null ? (object)DBNull.Value : item.Mahngebühr_3);
			sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
			sqlCommand.Parameters.AddWithValue("Mindermengenzuschlag", item.Mindermengenzuschlag == null ? (object)DBNull.Value : item.Mindermengenzuschlag);
			sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("OPOS", item.OPOS == null ? (object)DBNull.Value : item.OPOS);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Preisgruppe2", item.Preisgruppe2 == null ? (object)DBNull.Value : item.Preisgruppe2);
			sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
			sqlCommand.Parameters.AddWithValue("Regelmäßig_anschreiben__", item.Regelmäßig_anschreiben__ == null ? (object)DBNull.Value : item.Regelmäßig_anschreiben__);
			sqlCommand.Parameters.AddWithValue("RG_Abteilung", item.RG_Abteilung == null ? (object)DBNull.Value : item.RG_Abteilung);
			sqlCommand.Parameters.AddWithValue("RG_Land_PLZ_ORT", item.RG_Land_PLZ_ORT == null ? (object)DBNull.Value : item.RG_Land_PLZ_ORT);
			sqlCommand.Parameters.AddWithValue("RG_Strasse_Postfach", item.RG_Strasse_Postfach == null ? (object)DBNull.Value : item.RG_Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			sqlCommand.Parameters.AddWithValue("Standardversand", item.Standardversand == null ? (object)DBNull.Value : item.Standardversand);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen", item.Verzugszinsen == null ? (object)DBNull.Value : item.Verzugszinsen);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen_ab_Mahnstufe", item.Verzugszinsen_ab_Mahnstufe == null ? (object)DBNull.Value : item.Verzugszinsen_ab_Mahnstufe);
			sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);
			sqlCommand.Parameters.AddWithValue("Zahlung_erwartet_nach", item.Zahlung_erwartet_nach == null ? (object)DBNull.Value : item.Zahlung_erwartet_nach);
			sqlCommand.Parameters.AddWithValue("Zahlungskondition", item.Zahlungskondition == null ? (object)DBNull.Value : item.Zahlungskondition);
			sqlCommand.Parameters.AddWithValue("Zahlungsmoral", item.Zahlungsmoral == null ? (object)DBNull.Value : item.Zahlungsmoral);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
			sqlCommand.Parameters.AddWithValue("zolltarif_nr", item.zolltarif_nr == null ? (object)DBNull.Value : item.zolltarif_nr);
			// 18-03/2025 ticket #43321
			sqlCommand.Parameters.AddWithValue("CodeTypeInLS", item.CodeTypeInLS == null ? (object)DBNull.Value : item.CodeTypeInLS);
			sqlCommand.Parameters.AddWithValue("CodeTypeInLSId", item.CodeTypeInLSId == null ? (object)DBNull.Value : item.CodeTypeInLSId);

			var result = sqlCommand.ExecuteScalar();
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			return response;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.KundenEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{

			string query = "UPDATE [Kunden] SET [Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Branche]=@Branche, [Bruttofakturierung]=@Bruttofakturierung, [Debitoren-Nr]=@Debitoren_Nr, [EG - Identifikationsnummer]=@EG___Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Factoring]=@Factoring, [fibu_rahmen]=@fibu_rahmen, [gesperrt für weitere Lieferungen]=@gesperrt_für_weitere_Lieferungen, [Grund]=@Grund, [Grund für Sperre]=@Grund_für_Sperre, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditlimit]=@Kreditlimit, [Kundengruppe]=@Kundengruppe, [Lieferantenummer (Kunden)]=@Lieferantenummer__Kunden_, [Lieferscheinadresse]=@Lieferscheinadresse, [Mahngebühr 1]=@Mahngebühr_1, [Mahngebühr 2]=@Mahngebühr_2, [Mahngebühr 3]=@Mahngebühr_3, [Mahnsperre]=@Mahnsperre, [Mindermengenzuschlag]=@Mindermengenzuschlag, [nummer]=@nummer, [OPOS]=@OPOS, [Preisgruppe]=@Preisgruppe, [Preisgruppe2]=@Preisgruppe2, [Rabattgruppe]=@Rabattgruppe, [Regelmäßig anschreiben ?]=@Regelmäßig_anschreiben__, [RG-Abteilung]=@RG_Abteilung, [RG-Land/PLZ/ORT]=@RG_Land_PLZ_ORT, [RG-Strasse-Postfach]=@RG_Strasse_Postfach, [Sprache]=@Sprache,[Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Verzugszinsen]=@Verzugszinsen, [Währung]=@Währung,[Verzugszinsen ab Mahnstufe]=@Verzugszinsen_ab_Mahnstufe, [Zahlung erwartet nach]=@Zahlung_erwartet_nach, [Zahlungskondition]=@Zahlungskondition, [Zahlungsmoral]=@Zahlungsmoral, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [zolltarif_nr]=@zolltarif_nr WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
			sqlCommand.Parameters.AddWithValue("Bruttofakturierung", item.Bruttofakturierung == null ? (object)DBNull.Value : item.Bruttofakturierung);
			sqlCommand.Parameters.AddWithValue("Debitoren_Nr", item.Debitoren_Nr == null ? (object)DBNull.Value : item.Debitoren_Nr);
			sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
			sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
			sqlCommand.Parameters.AddWithValue("Factoring", item.Factoring == null ? (object)DBNull.Value : item.Factoring);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Lieferungen", item.gesperrt_für_weitere_Lieferungen == null ? (object)DBNull.Value : item.gesperrt_für_weitere_Lieferungen);
			sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
			sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
			sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
			sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
			sqlCommand.Parameters.AddWithValue("Kreditlimit", item.Kreditlimit == null ? (object)DBNull.Value : item.Kreditlimit);
			sqlCommand.Parameters.AddWithValue("Kundengruppe", item.Kundengruppe == null ? (object)DBNull.Value : item.Kundengruppe);
			sqlCommand.Parameters.AddWithValue("Lieferantenummer__Kunden_", item.Lieferantenummer__Kunden_ == null ? (object)DBNull.Value : item.Lieferantenummer__Kunden_);
			sqlCommand.Parameters.AddWithValue("Lieferscheinadresse", item.Lieferscheinadresse == null ? (object)DBNull.Value : item.Lieferscheinadresse);
			//sqlCommand.Parameters.AddWithValue("LSADR", item.LSADR == null ? (object)DBNull.Value : item.LSADR);
			//sqlCommand.Parameters.AddWithValue("LSADRANG", item.LSADRANG == null ? (object)DBNull.Value : item.LSADRANG);
			//sqlCommand.Parameters.AddWithValue("LSADRAUF", item.LSADRAUF == null ? (object)DBNull.Value : item.LSADRAUF);
			//sqlCommand.Parameters.AddWithValue("LSADRGUT", item.LSADRGUT == null ? (object)DBNull.Value : item.LSADRGUT);
			//sqlCommand.Parameters.AddWithValue("LSADRPROF", item.LSADRPROF == null ? (object)DBNull.Value : item.LSADRPROF);
			//sqlCommand.Parameters.AddWithValue("LSADRRG", item.LSADRRG == null ? (object)DBNull.Value : item.LSADRRG);
			//sqlCommand.Parameters.AddWithValue("LSADRSTO", item.LSADRSTO == null ? (object)DBNull.Value : item.LSADRSTO);
			//sqlCommand.Parameters.AddWithValue("LSRG", item.LSRG == null ? (object)DBNull.Value : item.LSRG);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_1", item.Mahngebühr_1 == null ? (object)DBNull.Value : item.Mahngebühr_1);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_2", item.Mahngebühr_2 == null ? (object)DBNull.Value : item.Mahngebühr_2);
			sqlCommand.Parameters.AddWithValue("Mahngebühr_3", item.Mahngebühr_3 == null ? (object)DBNull.Value : item.Mahngebühr_3);
			sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
			sqlCommand.Parameters.AddWithValue("Mindermengenzuschlag", item.Mindermengenzuschlag == null ? (object)DBNull.Value : item.Mindermengenzuschlag);
			sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("OPOS", item.OPOS == null ? (object)DBNull.Value : item.OPOS);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Preisgruppe2", item.Preisgruppe2 == null ? (object)DBNull.Value : item.Preisgruppe2);
			sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
			sqlCommand.Parameters.AddWithValue("Regelmäßig_anschreiben__", item.Regelmäßig_anschreiben__ == null ? (object)DBNull.Value : item.Regelmäßig_anschreiben__);
			sqlCommand.Parameters.AddWithValue("RG_Abteilung", item.RG_Abteilung == null ? (object)DBNull.Value : item.RG_Abteilung);
			sqlCommand.Parameters.AddWithValue("RG_Land_PLZ_ORT", item.RG_Land_PLZ_ORT == null ? (object)DBNull.Value : item.RG_Land_PLZ_ORT);
			sqlCommand.Parameters.AddWithValue("RG_Strasse_Postfach", item.RG_Strasse_Postfach == null ? (object)DBNull.Value : item.RG_Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			//sqlCommand.Parameters.AddWithValue("Standardversand", item.Standardversand == null ? (object)DBNull.Value : item.Standardversand);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen", item.Verzugszinsen == null ? (object)DBNull.Value : item.Verzugszinsen);
			sqlCommand.Parameters.AddWithValue("Verzugszinsen_ab_Mahnstufe", item.Verzugszinsen_ab_Mahnstufe == null ? (object)DBNull.Value : item.Verzugszinsen_ab_Mahnstufe);
			sqlCommand.Parameters.AddWithValue("Währung", item.Währung == null ? (object)DBNull.Value : item.Währung);
			sqlCommand.Parameters.AddWithValue("Zahlung_erwartet_nach", item.Zahlung_erwartet_nach == null ? (object)DBNull.Value : item.Zahlung_erwartet_nach);
			sqlCommand.Parameters.AddWithValue("Zahlungskondition", item.Zahlungskondition == null ? (object)DBNull.Value : item.Zahlungskondition);
			sqlCommand.Parameters.AddWithValue("Zahlungsmoral", item.Zahlungsmoral == null ? (object)DBNull.Value : item.Zahlungsmoral);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
			sqlCommand.Parameters.AddWithValue("zolltarif_nr", item.zolltarif_nr == null ? (object)DBNull.Value : item.zolltarif_nr);

			int response = sqlCommand.ExecuteNonQuery();

			return response;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.KundenEntity GetWithTransaction(int id, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM Kunden WHERE Nr=@Id", sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetForCreate()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * from kunden WHERE ISNULL([gesperrt für weitere Lieferungen],0)=0";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> GetByNummersForCreate(List<int> nummers)
		{
			if(nummers == null || nummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}

			SqlDataAdapter selectAdapter;
			var dt = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				var sqlCommand = new SqlCommand($"SELECT * FROM Kunden WHERE nummer IN ({string.Join(",", nummers)}) AND ISNULL([gesperrt für weitere Lieferungen],0)=0;", sqlConnection);
				selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dt);
				sqlConnection.Close();
			}

			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
			}
		}
		public static int UpdateOrderAddress(int Nr,string OrderAddress)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [OrderAddress]=@OrderAddress WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", Nr);

			sqlCommand.Parameters.AddWithValue("OrderAddress", OrderAddress == null ? (object)DBNull.Value : OrderAddress);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.PRS.KundenEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
