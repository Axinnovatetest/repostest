using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
    public class Budget_OrderEntity
    {
        public string AB_Nr_Lieferant { get; set; }
        public string Abteilung { get; set; }
        public DateTime? Anfrage_Lieferfrist { get; set; }
        public string Anrede { get; set; }
        public string Ansprechpartner { get; set; }
        public DateTime? ApprovalTime { get; set; }
        public int? ApprovalUserId { get; set; }
        public bool? Archived { get; set; }
        public DateTime? ArchiveTime { get; set; }
        public int? ArchiveUserId { get; set; }
        public int? Bearbeiter { get; set; }
        public int? Belegkreis { get; set; }
        public string Bemerkungen { get; set; }
        public string Benutzer { get; set; }
        public int? best_id { get; set; }
        public DateTime? Bestellbestaetigung_erbeten_bis { get; set; }
        public int? Bestellung_Nr { get; set; }
        public string Bezug { get; set; }
        public string Briefanrede { get; set; }
        public bool? datueber { get; set; }
        public DateTime? Datum { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? DeleteUserId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Dept_name { get; set; }
        public string Description { get; set; }
        public string Eingangslieferscheinnr { get; set; }
        public string Eingangsrechnungsnr { get; set; }
        public bool? erledigt { get; set; }
        public double? Frachtfreigrenze { get; set; }
        public string Freitext { get; set; }
        public bool? gebucht { get; set; }
        public bool? gedruckt { get; set; }
        public int? Id_Currency_Order { get; set; }
        public int? Id_Dept { get; set; }
        public int? Id_Land { get; set; }
        public int Id_Order { get; set; }
        public int? Id_Project { get; set; }
        public int? Id_Supplier { get; set; }
        public int Id_User { get; set; }
        public string Ihr_Zeichen { get; set; }
        public bool? In_Bearbeitung { get; set; }
        public string InternalContact { get; set; }
        public bool? Kanban { get; set; }
        public string Konditionen { get; set; }
        public string Kreditorennummer { get; set; }
        public int? Kundenbestellung { get; set; }
        public string Land_PLZ_Ort { get; set; }
        public string Land_name { get; set; }
        public int? LastRejectionLevel { get; set; }
        public DateTime? LastRejectionTime { get; set; }
        public int? LastRejectionUserId { get; set; }
        public int? Level { get; set; }
        public int? Lieferanten_Nr { get; set; }
        public DateTime? Liefertermin { get; set; }
        public int? Location_Id { get; set; }
        public bool? Loeschen { get; set; }
        public DateTime? Mahnung { get; set; }
        public string Mandant { get; set; }
        public int? MandantId { get; set; }
        public double? Mindestbestellwert { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public bool? Neu { get; set; }
        public int? nr_anf { get; set; }
        public int? nr_bes { get; set; }
        public int? nr_gut { get; set; }
        public int? nr_RB { get; set; }
        public int? nr_sto { get; set; }
        public int? nr_war { get; set; }
        public bool? Oeffnen { get; set; }
        public DateTime? Order_date { get; set; }
        public string Order_Number { get; set; }
        public int? Personal_Nr { get; set; }
        public string Projekt_Nr { get; set; }
        public double? Rabatt { get; set; }
        public bool? Rahmenbestellung { get; set; }
        public int? Status { get; set; }
        public int? StorageLocationId { get; set; }
        public string StorageLocationName { get; set; }
        public string Strasse_Postfach { get; set; }
        public string Typ { get; set; }
        public string Type_Order { get; set; }
        public string Unser_Zeichen { get; set; }
        public double? USt { get; set; }
        public string Versandart { get; set; }
        public string Vorname_NameFirma { get; set; }
        public int? Waehrung { get; set; }
        public string Zahlungsweise { get; set; }
        public string Zahlungsziel { get; set; }

        public Budget_OrderEntity() { }

        public Budget_OrderEntity(DataRow dataRow)
        {
            AB_Nr_Lieferant = dataRow["AB-Nr_Lieferant"] == DBNull.Value ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
            Abteilung = dataRow["Abteilung"] == DBNull.Value ? "" : Convert.ToString(dataRow["Abteilung"]);
            Anfrage_Lieferfrist = dataRow["Anfrage_Lieferfrist"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Anfrage_Lieferfrist"]);
            Anrede = dataRow["Anrede"] == DBNull.Value ? "" : Convert.ToString(dataRow["Anrede"]);
            Ansprechpartner = dataRow["Ansprechpartner"] == DBNull.Value ? "" : Convert.ToString(dataRow["Ansprechpartner"]);
            ApprovalTime = dataRow["ApprovalTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ApprovalTime"]);
            ApprovalUserId = dataRow["ApprovalUserId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ApprovalUserId"]);
            Archived = dataRow["Archived"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Archived"]);
            ArchiveTime = dataRow["ArchiveTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
            ArchiveUserId = dataRow["ArchiveUserId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
            Bearbeiter = dataRow["Bearbeiter"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Bearbeiter"]);
            Belegkreis = dataRow["Belegkreis"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
            Bemerkungen = dataRow["Bemerkungen"] == DBNull.Value ? "" : Convert.ToString(dataRow["Bemerkungen"]);
            Benutzer = dataRow["Benutzer"] == DBNull.Value ? "" : Convert.ToString(dataRow["Benutzer"]);
            best_id = dataRow["best_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["best_id"]);
            
            // Correction du nom de colonne avec caractères spéciaux
            Bestellbestaetigung_erbeten_bis = dataRow["Bestellbestätigung erbeten bis"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestellbestätigung erbeten bis"]);
            
            Bestellung_Nr = dataRow["Bestellung-Nr"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
            Bezug = dataRow["Bezug"] == DBNull.Value ? "" : Convert.ToString(dataRow["Bezug"]);
            Briefanrede = dataRow["Briefanrede"] == DBNull.Value ? "" : Convert.ToString(dataRow["Briefanrede"]);
            datueber = dataRow["datueber"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["datueber"]);
            Datum = dataRow["Datum"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
            Deleted = dataRow["Deleted"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Deleted"]);
            DeleteTime = dataRow["DeleteTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["DeleteTime"]);
            DeleteUserId = dataRow["DeleteUserId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DeleteUserId"]);
            DeliveryAddress = dataRow["DeliveryAddress"] == DBNull.Value ? "" : Convert.ToString(dataRow["DeliveryAddress"]);
            Dept_name = dataRow["Dept_name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Dept_name"]);
            Description = dataRow["Description"] == DBNull.Value ? "" : Convert.ToString(dataRow["Description"]);
            Eingangslieferscheinnr = dataRow["Eingangslieferscheinnr"] == DBNull.Value ? "" : Convert.ToString(dataRow["Eingangslieferscheinnr"]);
            Eingangsrechnungsnr = dataRow["Eingangsrechnungsnr"] == DBNull.Value ? "" : Convert.ToString(dataRow["Eingangsrechnungsnr"]);
            erledigt = dataRow["erledigt"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["erledigt"]);
            Frachtfreigrenze = dataRow["Frachtfreigrenze"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["Frachtfreigrenze"]);
            Freitext = dataRow["Freitext"] == DBNull.Value ? "" : Convert.ToString(dataRow["Freitext"]);
            gebucht = dataRow["gebucht"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
            gedruckt = dataRow["gedruckt"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["gedruckt"]);
            Id_Currency_Order = dataRow["Id_Currency_Order"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Order"]);
            Id_Dept = dataRow["Id_Dept"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Id_Dept"]);
            Id_Land = dataRow["Id_Land"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Id_Land"]);
            Id_Order = Convert.ToInt32(dataRow["Id_Order"]);
            Id_Project = dataRow["Id_Project"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Id_Project"]);
            Id_Supplier = dataRow["Id_Supplier"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier"]);
            Id_User = Convert.ToInt32(dataRow["Id_User"]);
            Ihr_Zeichen = dataRow["Ihr Zeichen"] == DBNull.Value ? "" : Convert.ToString(dataRow["Ihr Zeichen"]);
            In_Bearbeitung = dataRow["In Bearbeitung"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
            InternalContact = dataRow["InternalContact"] == DBNull.Value ? "" : Convert.ToString(dataRow["InternalContact"]);
            Kanban = dataRow["Kanban"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
            Konditionen = dataRow["Konditionen"] == DBNull.Value ? "" : Convert.ToString(dataRow["Konditionen"]);
            Kreditorennummer = dataRow["Kreditorennummer"] == DBNull.Value ? "" : Convert.ToString(dataRow["Kreditorennummer"]);
            Kundenbestellung = dataRow["Kundenbestellung"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Kundenbestellung"]);
            Land_PLZ_Ort = dataRow["Land/PLZ/Ort"] == DBNull.Value ? "" : Convert.ToString(dataRow["Land/PLZ/Ort"]);
            Land_name = dataRow["Land_name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Land_name"]);
            LastRejectionLevel = dataRow["LastRejectionLevel"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["LastRejectionLevel"]);
            LastRejectionTime = dataRow["LastRejectionTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastRejectionTime"]);
            LastRejectionUserId = dataRow["LastRejectionUserId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["LastRejectionUserId"]);
            Level = dataRow["Level"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Level"]);
            Lieferanten_Nr = dataRow["Lieferanten-Nr"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Lieferanten-Nr"]);
            Liefertermin = dataRow["Liefertermin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
            Location_Id = dataRow["Location_Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Location_Id"]);
            
            // Correction de Löschen
            Loeschen = dataRow["Löschen"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
            
            Mahnung = dataRow["Mahnung"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Mahnung"]);
            Mandant = dataRow["Mandant"] == DBNull.Value ? "" : Convert.ToString(dataRow["Mandant"]);
            MandantId = dataRow["MandantId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MandantId"]);
            Mindestbestellwert = dataRow["Mindestbestellwert"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["Mindestbestellwert"]);
            Name2 = dataRow["Name2"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name2"]);
            Name3 = dataRow["Name3"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name3"]);
            Neu = dataRow["Neu"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Neu"]);
            nr_anf = dataRow["nr_anf"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["nr_anf"]);
            nr_bes = dataRow["nr_bes"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["nr_bes"]);
            nr_gut = dataRow["nr_gut"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["nr_gut"]);
            nr_RB = dataRow["nr_RB"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["nr_RB"]);
            nr_sto = dataRow["nr_sto"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["nr_sto"]);
            nr_war = dataRow["nr_war"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["nr_war"]);
            
            // Correction de Öffnen
            Oeffnen = dataRow["Öffnen"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Öffnen"]);
            
            Order_date = dataRow["Order_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Order_date"]);
            Order_Number = dataRow["Order_Number"] == DBNull.Value ? "" : Convert.ToString(dataRow["Order_Number"]);
            Personal_Nr = dataRow["Personal-Nr"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Personal-Nr"]);
            Projekt_Nr = dataRow["Projekt-Nr"] == DBNull.Value ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
            Rabatt = dataRow["Rabatt"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["Rabatt"]);
            Rahmenbestellung = dataRow["Rahmenbestellung"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["Rahmenbestellung"]);
            Status = dataRow["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Status"]);
            StorageLocationId = dataRow["StorageLocationId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["StorageLocationId"]);
            StorageLocationName = dataRow["StorageLocationName"] == DBNull.Value ? "" : Convert.ToString(dataRow["StorageLocationName"]);
            
            // Correction de Straße
            Strasse_Postfach = dataRow["Straße/Postfach"] == DBNull.Value ? "" : Convert.ToString(dataRow["Straße/Postfach"]);
            
            Typ = dataRow["Typ"] == DBNull.Value ? "" : Convert.ToString(dataRow["Typ"]);
            Type_Order = dataRow["Type_Order"] == DBNull.Value ? "" : Convert.ToString(dataRow["Type_Order"]);
            Unser_Zeichen = dataRow["Unser Zeichen"] == DBNull.Value ? "" : Convert.ToString(dataRow["Unser Zeichen"]);
            USt = dataRow["USt"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["USt"]);
            Versandart = dataRow["Versandart"] == DBNull.Value ? "" : Convert.ToString(dataRow["Versandart"]);
            Vorname_NameFirma = dataRow["Vorname/NameFirma"] == DBNull.Value ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
            
            // Correction de Währung
            Waehrung = dataRow["Währung"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Währung"]);
            
            Zahlungsweise = dataRow["Zahlungsweise"] == DBNull.Value ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
            Zahlungsziel = dataRow["Zahlungsziel"] == DBNull.Value ? "" : Convert.ToString(dataRow["Zahlungsziel"]);
        }
    }
}