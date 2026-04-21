using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Peripherals.Models.CuttingPlan
{
    public class SearchCuttingPlanModel
    {
        public bool ValidatedOnly { get; set; }
        public bool UnvalidatedOnly { get; set; }
        public string PSZArticle { get; set; }
        public int Lager { get; set; }
        public DateTime? DateCreation { get; set; }
        public int RequestedPage { get; set; }
        public int ItemsPerPage { get; set; }
        public bool OnlyInProgress { get; set; }
        public string SortFieldKey { get; set; }
        public bool SortDesc { get; set; }

        public SearchCuttingPlanModel()
        {

        }
    }
}
