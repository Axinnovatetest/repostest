using Newtonsoft.Json;
using Psz.Core.MaterialManagement.WorkPlan.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;


namespace Psz.Core.MaterialManagement.WorkPlan.Handlers
{
	public class WorkPlansSearchHandler: IHandle<Identity.Models.UserModel, ResponseModel<WorkPlanResponseModel>>
	{
		private readonly WorkPlanSearchModel _data;
		private readonly Identity.Models.UserModel _user;

		public WorkPlansSearchHandler(WorkPlanSearchModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<WorkPlanResponseModel> Handle()
		{
			try
			{
				var validationresponse = this.Validate();
				if(!validationresponse.Success)
				{
					return validationresponse;
				}
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					var sortFieldName = "";
					switch(this._data.SortField.ToLower())
					{
						default:
						case "workplan": 
						case "name":
							sortFieldName = "A.[Artikelnummer]";
							break;

						case "artikelnummer":
						case "article":
							sortFieldName = "A.[Artikelnummer]";
							break;

						case "country":
							sortFieldName = "C.[Name]"; 
							break;

						case "hall":
							sortFieldName = "H.[Name]";
							break;

						case "is_active":
						case "active":
							sortFieldName = "W.[Is_Active]";
							break;

						case "createdon":
						case "creation_date":
							sortFieldName = "W.[Creation_Date]";
							break;

						case "lasteditdate":
						case "last_edited_on":
							sortFieldName = "W.[Last_Edit_Date]";
							break;

						case "editedby":
						//case "last_edit_username":
							sortFieldName = "U.[Name]";
							break;

						case "wpcount":
							sortFieldName = "E.WPCount";
							break;

						case "wscount":
						case "detailscount":
							sortFieldName = "D.DetailsCount";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				#endregion
				var data = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetWorkPlansList(_data.Workplan, _data.Article_Nummer, _data.Country, _data.Hall, _data.Is_Active, dataSorting, dataPaging);
				var count = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetWorkPlansListCount(_data.Workplan, _data.Article_Nummer, _data.Country, _data.Hall, _data.Is_Active);
				return ResponseModel<WorkPlanResponseModel>.SuccessResponse(
				new WorkPlanResponseModel()
				{
					Items = data?.Select(x => new WorkPlanListModel(x)).ToList(),
					PageRequested = this._data.RequestedPage,
					PageSize = this._data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / this._data.PageSize)) : 0,
				});

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}

		public ResponseModel<WorkPlanResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.access.____*/)
			{
				return ResponseModel<WorkPlanResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<WorkPlanResponseModel>.SuccessResponse();
		}
	}
}
