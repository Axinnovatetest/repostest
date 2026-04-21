using Geocoding.Microsoft.Json;
using MoreLinq;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Psz.Core.FinanceControl.Enums.BudgetEnums;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjectTypesDistinctHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public GetProjectTypesDistinctHandler(Identity.Models.UserModel user /*, int value*/ )
		{
			this._user = user;
			//this._data = value;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var items = new List<KeyValuePair<int, string>>();

				// retrieve all enumeration option
				var projectTypes = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>().ToList();
				if(projectTypes?.Count > 0)
				{
					//AddRange 
					items.AddRange(projectTypes.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription())));
				}

				// -
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(items);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
