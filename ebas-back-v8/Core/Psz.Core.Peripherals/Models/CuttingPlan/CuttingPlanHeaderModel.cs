using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Peripherals.Models.CuttingPlan
{
    public class CuttingPlanHeaderModel
    {
        public bool? Aktive { get; set; }
        public int? Artikel_Nr { get; set; }
        public string Artikelnummer { get; set; }
        public string Bezeichnung { get; set; }
        public int? BOM_version { get; set; }
        public string changee_par { get; set; }
        public int? CP_version { get; set; }
        public string cree_par { get; set; }
        public DateTime? date_changement { get; set; }
        public DateTime? date_creation { get; set; }
        public DateTime? Date_index { get; set; }
        public DateTime? date_validee { get; set; }
        public int ID_Nr { get; set; }
        public string Kunde { get; set; }
        public string Kunden_Index { get; set; }
        public int? Lager { get; set; }
        public bool? Validee { get; set; }
        public string validee_par { get; set; }
        public bool BOMValidation { get; set; }

        public CuttingPlanHeaderModel()
        {

        }
        public CuttingPlanHeaderModel(Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity cuttingPlanEntity)
        {
            if (cuttingPlanEntity != null)
            {
                var stucklistenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(cuttingPlanEntity.Artikel_Nr ?? -1);
                Aktive = cuttingPlanEntity.Aktive;
                Artikel_Nr = cuttingPlanEntity.Artikel_Nr;
                Artikelnummer = cuttingPlanEntity.Artikelnummer;
                Bezeichnung = cuttingPlanEntity.Bezeichnung;
                BOM_version = cuttingPlanEntity.BOM_version;
                changee_par = cuttingPlanEntity.changee_par;
                CP_version = cuttingPlanEntity.CP_version;
                cree_par = cuttingPlanEntity.cree_par;
                date_changement = cuttingPlanEntity.date_changement;
                date_creation = cuttingPlanEntity.date_creation;
                Date_index = cuttingPlanEntity.Date_index;
                date_validee = cuttingPlanEntity.date_validee;
                ID_Nr = cuttingPlanEntity.ID_Nr;
                Kunde = cuttingPlanEntity.Kunde;
                Kunden_Index = cuttingPlanEntity.Kunden_Index;
                Lager = cuttingPlanEntity.Lager;
                Validee = cuttingPlanEntity.Validee;
                validee_par = cuttingPlanEntity.validee_par;
                BOMValidation = (stucklistenExtensionEntity!=null && stucklistenExtensionEntity.BomStatusId.HasValue && stucklistenExtensionEntity.BomStatusId.Value == 1) ? true : false;
            }
        }
        public Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity ToEntity()
        {
            return new Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity
            {
                Aktive = Aktive,
                Artikel_Nr = Artikel_Nr,
                Artikelnummer = Artikelnummer,
                Bezeichnung = Bezeichnung,
                BOM_version = BOM_version,
                changee_par = changee_par,
                CP_version = CP_version,
                cree_par = cree_par,
                date_changement = date_changement,
                date_creation = date_creation,
                Date_index = Date_index,
                date_validee = date_validee,
                ID_Nr = ID_Nr,
                Kunde = Kunde,
                Kunden_Index = Kunden_Index,
                Lager = Lager,
                Validee = Validee,
                validee_par = validee_par,
            };
        }
    }
}
