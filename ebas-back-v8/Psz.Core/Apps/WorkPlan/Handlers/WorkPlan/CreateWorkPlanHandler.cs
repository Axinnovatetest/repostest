using Psz.Core.Apps.WorkPlan.Models.WorkPlan;
using Psz.Core.Identity.Models;
using Psz.Core.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;


namespace Psz.Core.Apps.WorkPlan.Handlers.WorkPlan
{
	public class CreateWorkPlanHandler: IHandle<Identity.Models.UserModel , ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly WorkPlanCreateModel _data;

		public CreateWorkPlanHandler(Identity.Models.UserModel user, WorkPlanCreateModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var WPheaderEntity = new Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity
				{

					ArticleId = _data.Header.ArticleId,
					CreationTime = _data.Header.CreationTime,
					CreationUserId = _data.Header.CreationUserId,
					HallId = _data.Header.HallId,
					Id = _data.Header.Id,
					IsActive = _data.Header.IsActive,
					LastEditTime = _data.Header.LastEditTime,
					LastEditUserId = _data.Header.LastEditUserId,
					Name = "AP" + _data.Header.Name,

				};
				var addedId = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Insert(WPheaderEntity);

				var WPlineItemToInsert = _data.LineItems.Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity
				{
					WorkScheduleId = addedId,
					Amount = x.Amount,
					CountryId = x.CountryId,
					CreationTime = DateTime.Now,             
					CreationUserId = _user.Id,               
					DepartementId = x.DepartementId,
					FromToolInsert = x.FromToolInsert,
					FromToolInsert2 = x.FromToolInsert2,
					HallId = x.HallId,
					
					LastEditTime = x.LastEditTime,                     
					LastEditUserId = x.LastEditUserId,
					LotSizeSTD = x.LotSizeSTD,
					OperationDescriptionId = x.OperationDescriptionId,
					OperationNumber = x.OperationNumber,
					OperationTimeSeconds = x.OperationTimeSeconds,
					OperationTimeValueAdding = x.OperationTimeValueAdding,
					PredecessorOperation = x.PredecessorOperation,
					PredecessorSubOperation = x.PredecessorSubOperation,
					RelationOperationTime = x.RelationOperationTime,
					SetupTimeMinutes = x.SetupTimeMinutes,
					StandardOccupancy = x.StandardOccupancy,
					StandardOperationId = x.StandardOperationId,
					SubOperationNumber = x.SubOperationNumber,
					TotalTimeOperation = x.TotalTimeOperation,
					WorkAreaId = x.WorkAreaId,
					Comment = x.Comment,
					WorkStationMachineId = x.WorkStationMachineId,
					OperationValueAdding = x.OperationValueAdding,
					OrderDisplayId = x.OrderDisplayId,

					CountryName = x.CountryName,
					HallName = x.HallName,
					DepartmentName = x.DepartmentName,
					WorkAreaName = x.WorkAreaName,
					WorkStationMachineName = x.WorkStationMachineName,
					StandardOperationName = x.StandardOperationName,
					OperationDescriptionName = x.OperationDescriptionName
				}).ToList();
				Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Insert(WPlineItemToInsert);

				return ResponseModel<int>.SuccessResponse(addedId);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
