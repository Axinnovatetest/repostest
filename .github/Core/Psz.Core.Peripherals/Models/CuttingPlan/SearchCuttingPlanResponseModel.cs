using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Peripherals.Models.CuttingPlan
{
    public class SearchCuttingPlanResponseModel
    {
        public List<MinimalCuttingplanModel> CuttingPlans { get; set; } = new List<MinimalCuttingplanModel>();
        public int AllCount { get; set; }
        public int AllPagesCount { get; set; }
        public int RequestedPage { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
