using System;
using System.Collections.Generic;
using System.Text;

namespace Psz.Core.Peripherals.Models.CuttingPlan
{
    public class CuttingPlanPositionsModel
    {
        public int? Artikel_Nr_FG { get; set; }
        public int? Artikel_Nr_ROH { get; set; }
        public string Artikelnummer_FG { get; set; }
        public string Artikelnummer_ROH { get; set; }
        public string Box { get; set; }
        public bool? Changed { get; set; }
        public int? Code_1 { get; set; }
        public int? Code_2 { get; set; }
        public int? Code_3 { get; set; }
        public string Contact_A { get; set; }
        public string Contact_B { get; set; }
        public string Couleur_impression { get; set; }
        public decimal? Deg_B { get; set; }
        public decimal? Degunage_A { get; set; }
        public decimal? Denudage1 { get; set; }
        public decimal? Denudage2 { get; set; }
        public string Dichtung_A { get; set; }
        public string Dichtung_B { get; set; }
        public int? Gewerk { get; set; }
        public int ID_Nr { get; set; }
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

        public CuttingPlanPositionsModel()
        {

        }

        public CuttingPlanPositionsModel(Infrastructure.Data.Entities.Tables.PRF.CAO_Decoupage_PositionEntity positions_entity)
        {
            Artikel_Nr_FG = positions_entity.Artikel_Nr_FG;
            Artikel_Nr_ROH = positions_entity.Artikel_Nr_ROH;
            Artikelnummer_FG = positions_entity.Artikelnummer_FG;
            Artikelnummer_ROH = positions_entity.Artikelnummer_ROH;
            Box = positions_entity.Box;
            Changed = positions_entity.Changed;
            Code_1 = positions_entity.Code_1;
            Code_2 = positions_entity.Code_2;
            Code_3 = positions_entity.Code_3;
            Contact_A = positions_entity.Contact_A;
            Contact_B = positions_entity.Contact_B;
            Couleur_impression = positions_entity.Couleur_impression;
            Deg_B = positions_entity.Deg_B;
            Degunage_A = positions_entity.Degunage_A;
            Denudage1 = positions_entity.Denudage1;
            Denudage2 = positions_entity.Denudage2;
            Dichtung_A = positions_entity.Dichtung_A;
            Dichtung_B = positions_entity.Dichtung_B;
            Gewerk = positions_entity.Gewerk;
            ID_Nr = positions_entity.ID_Nr;
            Impression = positions_entity.Impression;
            Impression_Gauche = positions_entity.Impression_Gauche;
            Impression_Milieu = positions_entity.Impression_Milieu;
            Kunden_Index = positions_entity.Kunden_Index;
            Length = positions_entity.Length;
            Menge = positions_entity.Menge;
            Outil_A = positions_entity.Outil_A;
            Outil_B = positions_entity.Outil_B;
            Position = positions_entity.Position;
            Tol = positions_entity.Tol;
            Triage_A = positions_entity.Triage_A;
            Triage_B = positions_entity.Triage_B;
            ZESuivante = positions_entity.ZESuivante;

        }
    }
}
