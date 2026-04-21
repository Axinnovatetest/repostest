using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class KundenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.KundenEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Kunden] WHERE Nr=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity> Get()
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Kunden]";

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
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			}

			int max = Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			}

			var sqlConnection = new SqlConnection(Settings.ConnectionString);
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

			sqlCommand.CommandText = $"SELECT * FROM [Kunden] WHERE Nr IN ({queryIds})";


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
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.KundenEntity item)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO [Kunden] ([Belegkreis],[Bemerkungen],[Branche],[Bruttofakturierung],[Debitoren-Nr],[EG - Identifikationsnummer],[Eilzuschlag],[Factoring],[fibu_rahmen],[gesperrt für weitere Lieferungen],[Grund],[Grund für Sperre],[Karenztage],[Konditionszuordnungs-Nr],[Kreditlimit],[Kundengruppe],[Lieferantenummer (Kunden)],[Lieferscheinadresse],[LSADR],[LSADRANG],[LSADRAUF],[LSADRGUT],[LSADRPROF],[LSADRRG],[LSADRSTO],[LSRG],[Mahngebühr 1],[Mahngebühr 2],[Mahngebühr 3],[Mahnsperre],[Mindermengenzuschlag],[nummer],[OPOS],[Preisgruppe],[Preisgruppe2],[Rabattgruppe],[Regelmäßig anschreiben ?],[RG-Abteilung],[RG-Land/PLZ/ORT],[RG-Strasse-Postfach],[Sprache],[Standardversand],[Umsatzsteuer berechnen],[Versandart],[Verzugszinsen],[Verzugszinsen ab Mahnstufe],[Währung],[Zahlung erwartet nach],[Zahlungskondition],[Zahlungsmoral],[Zahlungsweise],[Zielaufschlag],[zolltarif_nr])  VALUES (@Belegkreis,@Bemerkungen,@Branche,@Bruttofakturierung,@Debitoren_Nr,@EG___Identifikationsnummer,@Eilzuschlag,@Factoring,@fibu_rahmen,@gesperrt_für_weitere_Lieferungen,@Grund,@Grund_für_Sperre,@Karenztage,@Konditionszuordnungs_Nr,@Kreditlimit,@Kundengruppe,@Lieferantenummer__Kunden_,@Lieferscheinadresse,@LSADR,@LSADRANG,@LSADRAUF,@LSADRGUT,@LSADRPROF,@LSADRRG,@LSADRSTO,@LSRG,@Mahngebühr_1,@Mahngebühr_2,@Mahngebühr_3,@Mahnsperre,@Mindermengenzuschlag,@nummer,@OPOS,@Preisgruppe,@Preisgruppe2,@Rabattgruppe,@Regelmäßig_anschreiben__,@RG_Abteilung,@RG_Land_PLZ_ORT,@RG_Strasse_Postfach,@Sprache,@Standardversand,@Umsatzsteuer_berechnen,@Versandart,@Verzugszinsen,@Verzugszinsen_ab_Mahnstufe,@Währung,@Zahlung_erwartet_nach,@Zahlungskondition,@Zahlungsmoral,@Zahlungsweise,@Zielaufschlag,@zolltarif_nr)";
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



			int response = sqlCommand.ExecuteNonQuery();

			if(response > 0)
			{
				query = "SELECT Nr FROM [Kunden] WHERE Nr = @@IDENTITY";
				sqlCommand = new SqlCommand(query, sqlConnection);
				object insertedId = sqlCommand.ExecuteScalar();

				if(insertedId != null)
				{
					response = int.Parse(insertedId.ToString());
				}
				else
				{
					response = -1;
				}
			}
			else
			{
				response = -1;
			}

			sqlConnection.Close();

			return response;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.KundenEntity item)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Kunden] SET [Belegkreis]=@Belegkreis, [Bemerkungen]=@Bemerkungen, [Branche]=@Branche, [Bruttofakturierung]=@Bruttofakturierung, [Debitoren-Nr]=@Debitoren_Nr, [EG - Identifikationsnummer]=@EG___Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Factoring]=@Factoring, [fibu_rahmen]=@fibu_rahmen, [gesperrt für weitere Lieferungen]=@gesperrt_für_weitere_Lieferungen, [Grund]=@Grund, [Grund für Sperre]=@Grund_für_Sperre, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditlimit]=@Kreditlimit, [Kundengruppe]=@Kundengruppe, [Lieferantenummer (Kunden)]=@Lieferantenummer__Kunden_, [Lieferscheinadresse]=@Lieferscheinadresse, [LSADR]=@LSADR, [LSADRANG]=@LSADRANG, [LSADRAUF]=@LSADRAUF, [LSADRGUT]=@LSADRGUT, [LSADRPROF]=@LSADRPROF, [LSADRRG]=@LSADRRG, [LSADRSTO]=@LSADRSTO, [LSRG]=@LSRG, [Mahngebühr 1]=@Mahngebühr_1, [Mahngebühr 2]=@Mahngebühr_2, [Mahngebühr 3]=@Mahngebühr_3, [Mahnsperre]=@Mahnsperre, [Mindermengenzuschlag]=@Mindermengenzuschlag, [nummer]=@nummer, [OPOS]=@OPOS, [Preisgruppe]=@Preisgruppe, [Preisgruppe2]=@Preisgruppe2, [Rabattgruppe]=@Rabattgruppe, [Regelmäßig anschreiben ?]=@Regelmäßig_anschreiben__, [RG-Abteilung]=@RG_Abteilung, [RG-Land/PLZ/ORT]=@RG_Land_PLZ_ORT, [RG-Strasse-Postfach]=@RG_Strasse_Postfach, [Sprache]=@Sprache, [Standardversand]=@Standardversand, [Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Verzugszinsen]=@Verzugszinsen, [Verzugszinsen ab Mahnstufe]=@Verzugszinsen_ab_Mahnstufe, [Währung]=@Währung, [Zahlung erwartet nach]=@Zahlung_erwartet_nach, [Zahlungskondition]=@Zahlungskondition, [Zahlungsmoral]=@Zahlungsmoral, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [zolltarif_nr]=@zolltarif_nr WHERE [Nr]=@Nr";

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

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}

		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM [Kunden] WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Nr", id);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConection.Close();

			return response;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.FNC.KundenEntity GetByNummer(int nummer)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM [Kunden] WHERE nummer=@nummer";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("nummer", nummer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.KundenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity> GetByNummers(List<int> nummers)
		{
			if(nummers == null || nummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			}

			int max = Settings.MAX_BATCH_SIZE;

			if(nummers.Count <= max)
			{
				return getByNummers(nummers);
			}

			int batchNumber = nummers.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(getByNummers(nummers.GetRange(i * max, max)));
			}
			result.AddRange(getByNummers(nummers.GetRange(batchNumber * max, nummers.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity> getByNummers(List<int> nummers)
		{
			if(nummers == null || nummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			}

			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;

			string queryIds = string.Empty;
			for(int i = 0; i < nummers.Count; i++)
			{
				queryIds += nummers[i] + ",";
				//queryIds += "@Id" + i + ",";
				//sqlCommand.Parameters.AddWithValue("Id" + i, nummers[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = $"SELECT * FROM [Kunden] WHERE nummer IN ({queryIds})";


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
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
			}
		}
		public static int GetMaxNummber()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT coalesce(MAX([nummer]), 0) AS MaxNummer FROM [Kunden]";

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


		public static List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity> SearchByNumberName(
			int? nummer, string name, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
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
						query += " WHERE [kundennummer]=@nummer AND kd.nr is not null and ad.Adresstyp=1";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null and ad.Adresstyp=1";
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
				return new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>();
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
						query += " WHERE [kundennummer]=@nummer AND kd.nr is not null and ad.Adresstyp=1";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Kundennummer is not null AND kd.nr is not null and ad.Adresstyp=1";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}
		public static List<Entities.Tables.FNC.KundenEntity> GetLikeNumber(string nummer)
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

		public static List<Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity> GetCustomerProject()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT [adressen].[Name1] as Customer_Name, [adressen].[Kundennummer], [adressen].[Ort], [Kunden].[Nr] " +
					"FROM[Kunden] " +
					"INNER JOIN[adressen] ON [Kunden].[nummer] = [adressen].[Nr] " +
					"WHERE((([adressen].[Kundennummer]) Is Not Null) AND(([adressen].[Adresstyp])=1))";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toListCustomerProject(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity>();
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.KundenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.FNC.KundenEntity(dataRow));
			}
			return result;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity> toListCustomerProject(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.FNC.Kunden_Project_BudgetEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
