using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.WorkPlan.Models
{
	public class WorkPlanSearchModel:IPaginatedRequestModel
	{
		public string? Workplan { get; set; }
		public string? Hall { get; set; }
		public string? Country { get; set; }
		public string? Article_Nummer { get; set; }
		public bool? Is_Active { get; set; }

	}
	public class WorkPlanListModel
	{
		public int Id { get; set; }
		public string Work_Plan { get; set; }
		public int Hall_Id { get; set; }
		public string Hall { get; set; }
		public int Country_Id { get; set; }
		public string Country { get; set; }
		public int Article_Id { get; set; }
		public string Article_Nummer { get; set; }
		public int WpCount { get; set; }
		public int WSCount { get; set; }
		public bool Is_Active { get; set; }
		public DateTime Creation_Date { get; set; }
		public int? Creation_User_Id { get; set; }
		public DateTime? Last_Edit_Date { get; set; }
		public int? Last_Edit_User_Id { get; set; }
		public string Last_Edit_Username { get; set; }
		public WorkPlanListModel(Infrastructure.Data.Entities.Joins.WPL.WorkPlanMinimalEntity entity)
		{
			Id = entity.Id;
			Work_Plan = entity.Work_Plan;
			Hall_Id = entity.Hall_Id;
			Hall = entity.Hall;
			Country_Id = entity.Country_Id;	
			Country = entity.Country;
			Article_Id = entity.Article_Id;
			Article_Nummer = entity.Article_Nummer;
			WpCount = entity.WpCount;
			WSCount = entity.WSCount;
			Is_Active = entity.Is_Active;
			Creation_Date = entity.Creation_Date;
			Creation_User_Id = entity.Creation_User_Id;
			Last_Edit_Date = entity.Last_Edit_Date;
			Last_Edit_User_Id= entity.Last_Edit_User_Id;	
			Last_Edit_Username = entity.Last_Edit_Username;
		}
	}


	public class WorkPlanResponseModel: IPaginatedResponseModel<WorkPlanListModel>
	{

	}
}
