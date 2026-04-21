using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> SearchCable(UserModel user, string searchtext)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			try
			{
				if(searchtext.IsNullOrEmptyOrWitheSpaces())
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>> { });
				else
				{
					var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumberV3(searchtext);
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
						articles?.Select(x => new KeyValuePair<int, string>(x.ArtikelNr, x.ArtikelNummer)).ToList()
						);
				}
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}