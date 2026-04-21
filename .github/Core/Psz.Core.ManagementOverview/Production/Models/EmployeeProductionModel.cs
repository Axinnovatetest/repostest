using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Models
{
	public class EmployeeProductionModel
	{
		public long Id { get; set; }
		public int? Lagerort_id { get; set; }
		public int? Jahr { get; set; }
		public int? KW{ get; set; }
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
		public EmployeeProductionModel()
		{

		}
		public EmployeeProductionModel(Infrastructure.Data.Entities.Tables.MGO.EmployeeProductionEntity employeeEntity)
		{
			if(employeeEntity == null)
			{
				return;
			}
			Id = employeeEntity.Id;
			Lagerort_id = employeeEntity.Lagerort_id;
			Jahr = employeeEntity.Jahr;
			KW = employeeEntity.KW;
			AnzahlDirektMitarbeiter = employeeEntity.AnzahlDirektMitarbeiter;
			AnzahlDirektMitarbeiterWertschoepfend = employeeEntity.AnzahlDirektMitarbeiterWertschoepfend;
			AnzahlDirektMitarbeiterNichtWertschoepfend = employeeEntity.AnzahlDirektMitarbeiterNichtWertschoepfend;
			AnzahlIndirektMitarbeiter = employeeEntity.AnzahlIndirektMitarbeiter;
			GeplanteEinstellungenDirekt = employeeEntity.GeplanteEinstellungenDirekt;
			GeplanteEinstellungenIndirekt = employeeEntity.GeplanteEinstellungenIndirekt;
			Austritte = employeeEntity.Austritte;
			GelieferteStundenSerie = employeeEntity.GelieferteStundenSerie;
			GelieferteStundenErstmuster = employeeEntity.GelieferteStundenErstmuster;
			GeplanteStundenSerie = employeeEntity.GeplanteStundenSerie;
			RueckstandSerie = employeeEntity.RueckstandSerie;
			RueckstandErstmuster = employeeEntity.RueckstandErstmuster;
			GeplanteStundenErstmuster = employeeEntity.GeplanteStundenErstmuster;
			LagerwertROH = employeeEntity.LagerwertROH;
			AusschussWert = employeeEntity.AusschussWert;
			AnzahlReklamationenErhalten = employeeEntity.AnzahlReklamationenErhalten;
			AnzahlOffeneReklamationen = employeeEntity.AnzahlOffeneReklamationen;
			BudgetAusgabe = employeeEntity.BudgetAusgabe;
			datum = employeeEntity.datum;

		}
	}
}
