using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> SearchManager(UserModel user, string searchtext)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			try
			{
				var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get();
				if(searchtext.IsNullOrEmptyOrWitheSpaces())
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
						users?
						.Where(x => x.IsActivated)
						.Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
						.Take(10).ToList()
						);
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					users?.Where(x => x.Name.ToLower().Contains(searchtext.ToLower()) && x.IsActivated)
					.Select(y => new KeyValuePair<int, string>(y.Id, y.Name)).ToList()
					);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}