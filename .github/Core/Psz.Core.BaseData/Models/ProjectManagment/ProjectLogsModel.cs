using System;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class ProjectLogsModel
	{
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public string LogMessage { get; set; }
		public string Username { get; set; }
		public DateTime? Date { get; set; }
		public ProjectLogsModel()
		{

		}
	}
}