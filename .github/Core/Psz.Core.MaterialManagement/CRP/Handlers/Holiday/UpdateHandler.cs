using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Holiday;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Holiday
{
	public class UpdateHandler: IHandle<Models.Holiday.UpdateModel, ResponseModel<object>>
	{
		private Models.Holiday.UpdateModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public UpdateHandler(Models.Holiday.UpdateModel data,
			Core.Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<object> Handle()
		{
			lock(Locks.HolidayLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					var holidayEntity = HolidayAccess.Get(data.Id);
					if(holidayEntity == null || holidayEntity.IsArchived)
					{
						throw new Psz.Core.SharedKernel.Exceptions.NotFoundException();
					}

					var errors = new List<ResponseModel<object>.ResponseError>();

					// Missing: user access to Holiday's Hall

					if(this.data.Day.Date != holidayEntity.Day.Date
						&& this.data.Day.Date <= DateTime.Today)
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = "Day is not valid",
						});
					}

					if(string.IsNullOrWhiteSpace(this.data.Name))
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = "Name is empty",
						});
					}

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList());
					}

					holidayEntity.Day = this.data.Day;
					holidayEntity.Name = this.data.Name.Trim();
					holidayEntity.WeekNumber = Helpers.DateTimeHelper.GetIso8601WeekOfYear(this.data.Day);
					if(this.data.EditSimilar == true)
					{
						HolidayAccess.UpdateWSimilar(holidayEntity);
					}
					else
					{
						HolidayAccess.Update(holidayEntity);
					}


					return ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		ResponseModel<object> IHandle<UpdateModel, ResponseModel<object>>.Validate()
		{
			throw new NotImplementedException();
		}
	}
}
