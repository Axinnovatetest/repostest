using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	public partial class OrderService
	{
		public ResponseModel<int> AcitvateROH_RA_NeedsAgent(UserModel user)
		{
			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			try
			{
				var response = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.ROH_RA_Needs_agent(user.Id, user.Name);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<DateTime?> GetROH_RA_NeedsAgent_LastExecution(UserModel user)
		{
			if(user == null)
			{
				return ResponseModel<DateTime?>.AccessDeniedResponse();
			}
			try
			{
				var response = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetLastROH_RA_Needs_agentAgentExecutionTime();
				return ResponseModel<DateTime?>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<DateTime, string>>> GetROH_RA_NeedsAgentLogs(UserModel user)
		{
			if(user == null)
			{
				return ResponseModel<List<KeyValuePair<DateTime, string>>>.AccessDeniedResponse();
			}

			var response = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.ROH_RA_Needs_agentLogs();
			return ResponseModel<List<KeyValuePair<DateTime, string>>>.SuccessResponse(response);
		}
	}
}