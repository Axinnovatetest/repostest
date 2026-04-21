using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psz.Core.Peripherals.Models.CuttingPlan
{
    public class MinimalCuttingplanModel
    {
        public int Id { get; set; }
        public int? Artikel_nr { get; set; }
        public string Kunden_index { get; set; }
        public string Artikelnummer { get; set; }
        public DateTime? Date_creation { get; set; }
        public string Createdby { get; set; }
        public bool? validted { get; set; }
        public int Nbr_positions { get; set; }
        public List<int> Gewerks { get; set; }
        public int? CP_version { get; set; }
        public int? BOM_version { get; set; }
        public MinimalCuttingplanModel()
        {

        }

        public MinimalCuttingplanModel(Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity cuttingPlanEntity)
        {
            if (cuttingPlanEntity != null)
            {
                var positionsEntity = Infrastructure.Data.Access.Tables.PRF.CAO_Decoupage_PositionAccess.GetByArtikel_nr(cuttingPlanEntity.Artikel_Nr ?? -1);
                Artikel_nr = cuttingPlanEntity.Artikel_Nr;
                Artikelnummer = cuttingPlanEntity.Artikelnummer;
                BOM_version = cuttingPlanEntity.BOM_version;
                CP_version = cuttingPlanEntity.CP_version;
                Createdby = cuttingPlanEntity.cree_par;
                Date_creation = cuttingPlanEntity.date_creation;
                Id = cuttingPlanEntity.ID_Nr;
                Kunden_index = cuttingPlanEntity.Kunden_Index;
                validted = cuttingPlanEntity.Validee;
                //
                if (positionsEntity != null && positionsEntity.Count > 0)
                {
                    Nbr_positions = positionsEntity.Count;
                    Gewerks = positionsEntity.Select(p => (int)p.Gewerk).Distinct().ToList();
                }
            }
        }
    }
}
