using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class CP_snapshot_positionsEntity
	{
		public int? Artikel_Nr_FG { get; set; }
		public int? Artikel_Nr_ROH { get; set; }
		public string Artikelnummer_FG { get; set; }
		public string Artikelnummer_ROH { get; set; }
		public int? BOM_version { get; set; }
		public string Box { get; set; }
		public bool? Changed { get; set; }
		public int? Code_1 { get; set; }
		public int? Code_2 { get; set; }
		public int? Code_3 { get; set; }
		public string Contact_A { get; set; }
		public string Contact_B { get; set; }
		public string Couleur_impression { get; set; }
		public int? CP_version { get; set; }
		public decimal? Deg_B { get; set; }
		public decimal? Degunage_A { get; set; }
		public decimal? Denudage1 { get; set; }
		public decimal? Denudage2 { get; set; }
		public string Dichtung_A { get; set; }
		public string Dichtung_B { get; set; }
		public int? Gewerk { get; set; }
		public int Id { get; set; }
		public int? ID_Nr { get; set; }
		public string Impression { get; set; }
		public string Impression_Gauche { get; set; }
		public string Impression_Milieu { get; set; }
		public string Kunden_Index { get; set; }
		public decimal? Length { get; set; }
		public int? Menge { get; set; }
		public string Outil_A { get; set; }
		public string Outil_B { get; set; }
		public string Position { get; set; }
		public decimal? Tol { get; set; }
		public decimal? Triage_A { get; set; }
		public decimal? Triage_B { get; set; }
		public string ZESuivante { get; set; }

		public CP_snapshot_positionsEntity() { }

		public CP_snapshot_positionsEntity(DataRow dataRow)
		{
			Artikel_Nr_FG = (dataRow["Artikel_Nr_FG"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr_FG"]);
			Artikel_Nr_ROH = (dataRow["Artikel_Nr_ROH"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr_ROH"]);
			Artikelnummer_FG = (dataRow["Artikelnummer_FG"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer_FG"]);
			Artikelnummer_ROH = (dataRow["Artikelnummer_ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer_ROH"]);
			BOM_version = (dataRow["BOM_version"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BOM_version"]);
			Box = (dataRow["Box"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Box"]);
			Changed = (dataRow["Changed"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Changed"]);
			Code_1 = (dataRow["Code_1"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Code_1"]);
			Code_2 = (dataRow["Code_2"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Code_2"]);
			Code_3 = (dataRow["Code_3"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Code_3"]);
			Contact_A = (dataRow["Contact_A"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Contact_A"]);
			Contact_B = (dataRow["Contact_B"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Contact_B"]);
			Couleur_impression = (dataRow["Couleur_impression"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Couleur_impression"]);
			CP_version = (dataRow["CP_version"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CP_version"]);
			Deg_B = (dataRow["Deg_B"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Deg_B"]);
			Degunage_A = (dataRow["Degunage_A"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Degunage_A"]);
			Denudage1 = (dataRow["Denudage1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Denudage1"]);
			Denudage2 = (dataRow["Denudage2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Denudage2"]);
			Dichtung_A = (dataRow["Dichtung_A"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dichtung_A"]);
			Dichtung_B = (dataRow["Dichtung_B"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dichtung_B"]);
			Gewerk = (dataRow["Gewerk"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Gewerk"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ID_Nr = (dataRow["ID_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Nr"]);
			Impression = (dataRow["Impression"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Impression"]);
			Impression_Gauche = (dataRow["Impression_Gauche"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Impression_Gauche"]);
			Impression_Milieu = (dataRow["Impression_Milieu"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Impression_Milieu"]);
			Kunden_Index = (dataRow["Kunden_Index"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunden_Index"]);
			Length = (dataRow["Length"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Length"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
			Outil_A = (dataRow["Outil_A"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Outil_A"]);
			Outil_B = (dataRow["Outil_B"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Outil_B"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
			Tol = (dataRow["Tol"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Tol"]);
			Triage_A = (dataRow["Triage_A"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Triage_A"]);
			Triage_B = (dataRow["Triage_B"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Triage_B"]);
			ZESuivante = (dataRow["ZESuivante"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ZESuivante"]);
		}
	}
}

