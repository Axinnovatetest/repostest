using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Entities.Tables.MGO
{
	public class EmployeeProductionEntity
	{
		public long Id { get; set; }
		public int? Jahr { get; set; }
		public int? Lagerort_id { get; set; }
		public int? KW { get; set; }
		public decimal? AnzahlDirektMitarbeiter { get; set; }
		public decimal? AnzahlDirektMitarbeiterWertschoepfend { get; set; }
		public decimal? AnzahlDirektMitarbeiterNichtWertschoepfend { get; set; }
		public decimal? AnzahlIndirektMitarbeiter { get; set; }
		public decimal? GeplanteEinstellungenDirekt { get; set; }
		public decimal? GeplanteEinstellungenIndirekt { get; set; }
		public decimal? Austritte { get; set; }
		public decimal? GelieferteStundenSerie { get; set; }
		public decimal? GelieferteStundenErstmuster { get; set; }
		public decimal? GeplanteStundenSerie { get; set; }
		public decimal? GeplanteStundenErstmuster { get; set; }
		public decimal? RueckstandSerie { get; set; }
		public decimal? RueckstandErstmuster { get; set; }
		public decimal? LagerwertROH { get; set; }
		public decimal? AusschussWert { get; set; }
		public int? AnzahlReklamationenErhalten { get; set; }
		public int? AnzahlOffeneReklamationen { get; set; }
		public decimal? BudgetAusgabe { get; set; }
		public DateTime? datum { get; set; }
		public EmployeeProductionEntity()
		{
		}

		public EmployeeProductionEntity(DataRow dataRow)
		{
			Id = (dataRow["id"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["id"]);
			Jahr= (dataRow["Jahr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Jahr"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerort_id"]);
			KW = (dataRow["KW"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["KW"]);
			AnzahlDirektMitarbeiter = (dataRow["Anzahl_Direkt_Mitarbeiter"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl_Direkt_Mitarbeiter"]);
			AnzahlDirektMitarbeiterWertschoepfend = (dataRow["Anzahl_Direkt_Mitarbeiter_Wertschoepfend"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl_Direkt_Mitarbeiter_Wertschoepfend"]);
			AnzahlDirektMitarbeiterNichtWertschoepfend = (dataRow["Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl_Direkt_Mitarbeiter_Nicht_Wertschoepfend"]);
			AnzahlIndirektMitarbeiter = (dataRow["Anzahl_Indirekt_Mitarbeiter"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl_Indirekt_Mitarbeiter"]);
			GeplanteEinstellungenDirekt = (dataRow["Geplante_Einstellungen_Direkt"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Geplante_Einstellungen_Direkt"]);
			GeplanteEinstellungenIndirekt = (dataRow["Geplante_Einstellungen_Indirekt"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Geplante_Einstellungen_Indirekt"]);
			Austritte = (dataRow["Austritte"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Austritte"]);
			GelieferteStundenSerie = (dataRow["Gelieferte_Stunden_Serie"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gelieferte_Stunden_Serie"]);
			GelieferteStundenErstmuster = (dataRow["Gelieferte_Stunden_Erstmuster"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gelieferte_Stunden_Erstmuster"]);
			GeplanteStundenSerie = (dataRow["Geplante_Stunden_Serie"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Geplante_Stunden_Serie"]);
			GeplanteStundenErstmuster = (dataRow["Geplante_Stunden_Erstmuster"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Geplante_Stunden_Erstmuster"]);
			RueckstandSerie = (dataRow["Rueckstand_Serie"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Rueckstand_Serie"]);
			RueckstandErstmuster = (dataRow["Rueckstand_Erstmuster"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Rueckstand_Erstmuster"]);
			LagerwertROH = (dataRow["Lagerwert_ROH"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Lagerwert_ROH"]);
			AusschussWert = (dataRow["Ausschuss_Wert"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Ausschuss_Wert"]);
			AnzahlReklamationenErhalten = (dataRow["Anzahl_Reklamationen_Erhalten"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Anzahl_Reklamationen_Erhalten"]);
			AnzahlOffeneReklamationen = (dataRow["Anzahl_Offene_Reklamationen"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Anzahl_Offene_Reklamationen"]);
			BudgetAusgabe = (dataRow["Budget_Ausgabe"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Budget_Ausgabe"]);
			datum = (dataRow["LastUpdate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdate"]);


		}
	}
}
