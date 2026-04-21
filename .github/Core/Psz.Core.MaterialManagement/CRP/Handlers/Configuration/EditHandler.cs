using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.Configuration;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Configuration
{
	public class EditHandler: IHandle<AddResponseModel, ResponseModel<int>>
	{
		private AddResponseModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public EditHandler(AddResponseModel data, Identity.Models.UserModel user)
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
			var configEntity = ConfigurationHeaderAccess.GetByCountryHall(data.CountryId, data.HallId);
			data.Id = configEntity.Id;
			data.CreationTime = DateTime.Now;
			data.CreationUserId = user.Id;
			ConfigurationHeaderAccess.Update(data.ToEntity());
			// -
			ConfigurationDetailsAccess.DeleteByHeader(configEntity.Id);
			data.Departments.ForEach(x =>
			{
				x.HeaderId = configEntity.Id;
				x.CreationUserId = user.Id;
				x.CreationTime = DateTime.Now;
				x.Validated = true;
				x.ValidatedDate = DateTime.Now;
				x.ValidatedUserId = user.Id;
			});
			ConfigurationDetailsAccess.Insert(data.Departments.Select(x => x.ToEntity())?.ToList());

			return ResponseModel<int>.SuccessResponse(configEntity.Id);
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

			if(ConfigurationHeaderAccess.GetByCountryHall(data.CountryId, data.HallId) == null)
				return ResponseModel<int>.FailureResponse("Configuration not found.");

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
