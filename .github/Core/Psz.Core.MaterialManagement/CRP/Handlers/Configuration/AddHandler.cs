using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.Configuration;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Configuration
{
	public class AddHandler: IHandle<AddResponseModel, ResponseModel<int>>
	{
		private AddResponseModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public AddHandler(AddResponseModel data, Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					var validation = Validate();
					if(!validation.Success)
					{
						return validation;
					}

					return Perform(this.user, this.data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<int> Perform(Identity.Models.UserModel user, AddResponseModel data)
		{
			data.CreationTime = DateTime.Now;
			data.CreationUserId = user.Id;
			var headerId = ConfigurationHeaderAccess.Insert(data.ToEntity());
			if(headerId > 0)
			{
				data.Departments.ForEach(x =>
				{
					x.HeaderId = headerId;
					x.CreationUserId = user.Id;
					x.CreationTime = DateTime.Now;
					x.Validated = true;
					x.ValidatedDate = DateTime.Now;
					x.ValidatedUserId = user.Id;
				});
				ConfigurationDetailsAccess.Insert(data.Departments.Select(x => x.ToEntity())?.ToList());
			}

			return ResponseModel<int>.SuccessResponse(headerId);
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();

			if(CountryAccess.Get(this.data.CountryId) == null)
				return ResponseModel<int>.FailureResponse("Country not found.");

			if(HallAccess.Get(this.data.HallId) == null)
				return ResponseModel<int>.FailureResponse("Hall not found.");

			if(this.data.ProductionOrderThreshold <= 0)
				return ResponseModel<int>.FailureResponse("Invalid value for Threshold.");

			var configEntity = ConfigurationHeaderAccess.GetByCountryHall(data.CountryId, data.HallId);
			if(configEntity != null)
				return ResponseModel<int>.FailureResponse($"Configuration [{data.CountryName}] [{data.HallName}] exists with FA=[{(configEntity.ProductionOrderThreshold - Math.Truncate(configEntity.ProductionOrderThreshold) > 0 ? configEntity.ProductionOrderThreshold.ToString("#.##") : ((int)configEntity.ProductionOrderThreshold).ToString())}].");

			if(this.data.Departments != null && this.data.Departments.Count > 0)
			{
				var ids = this.data.Departments.Select(x => x.Id)?.ToList();
				var departmentEntities = DepartmentAccess.Get(ids);
				var missingDepartments = departmentEntities.Where(x => !ids.Exists(y => y == x.Id))?.ToList();
				if(missingDepartments != null && missingDepartments.Count > 0)
					return ResponseModel<int>.FailureResponse(missingDepartments.Select(x => x.Name)?.ToList());
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
