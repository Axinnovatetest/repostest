using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psz.Core.Peripherals.Models.CuttingPlan
{
    public class FullCuttingPlanModel
    {
        public CuttingPlanHeaderModel Header { get; set; }
        public List<CuttingPlanPositionsModel> Positions { get; set; }
        public List<int> Gewerks { get; set; }
        public int ActiveGewerk { get; set; }

        public FullCuttingPlanModel()
        {

        }
        public FullCuttingPlanModel(Infrastructure.Data.Entities.Tables.PRF.CAO_DecoupageEntity _header, List<Infrastructure.Data.Entities.Tables.PRF.CAO_Decoupage_PositionEntity> _positions)
        {
            Header = new CuttingPlanHeaderModel(_header);

            if (_positions != null && _positions.Count > 0)
            {
                this.Positions = new List<CuttingPlanPositionsModel>();
                for (var i = 0; i < _positions.Count; i++)
                {
                    var item = _positions[i];
                    this.Positions.Add(new CuttingPlanPositionsModel(item));
                }
                //
                Gewerks = _positions.Select(x => (int)x.Gewerk).Distinct().ToList();
                Gewerks.Sort();
                if (Gewerks != null && Gewerks.Count > 0)
                {
                    ActiveGewerk = Gewerks.Min();
                }
            }
        }
    }
}
