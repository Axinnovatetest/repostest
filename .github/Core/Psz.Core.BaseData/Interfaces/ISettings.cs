using Psz.Core.Common.Models;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Interfaces
{
	public interface ISettings
	{
		#region IHourlyRate
		public ResponseModel<List<KeyValuePair<string, string>>> HourlyRate_GetAllValues(Identity.Models.UserModel user);
		public ResponseModel<List<Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateResponseModel>> HourlyRate_Get(Identity.Models.UserModel user);
		public ResponseModel<int> HourlyRate_Add(Identity.Models.UserModel user, Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateRequestModel data);
		public ResponseModel<int> HourlyRate_Edit(Identity.Models.UserModel user, Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateRequestModel data);
		public ResponseModel<int> HourlyRate_Delete(Identity.Models.UserModel user, int id);
		public ResponseModel<Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateResponseModel> HourlyRate_GetByProductionSite(Identity.Models.UserModel user, int siteId);

		#endregion IHourlyrate
	}
	public class Settings: ISettings
	{
		public ResponseModel<List<KeyValuePair<string, string>>> HourlyRate_GetAllValues(Identity.Models.UserModel user)
		{
			return new Core.BaseData.Handlers.Settings.HourlyRate.GetAllValuesHandler(user)
				   .Handle();
		}
		public ResponseModel<List<Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateResponseModel>> HourlyRate_Get(Identity.Models.UserModel user)
		{
			return new Core.BaseData.Handlers.Settings.HourlyRate.GetAllHandler(user)
				   .Handle();
		}
		public ResponseModel<int> HourlyRate_Add(Identity.Models.UserModel user, Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateRequestModel data)
		{
			return new Core.BaseData.Handlers.Settings.HourlyRate.AddHandler(user, data)
				   .Handle();
		}
		public ResponseModel<int> HourlyRate_Edit(Identity.Models.UserModel user, Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateRequestModel data)
		{
			return new Core.BaseData.Handlers.Settings.HourlyRate.EditHandler(user, data)
				   .Handle();
		}
		public ResponseModel<int> HourlyRate_Delete(Identity.Models.UserModel user, int id)
		{
			return new Core.BaseData.Handlers.Settings.HourlyRate.DeleteHandler(user, id)
				   .Handle();
		}
		public ResponseModel<Psz.Core.BaseData.Models.Settings.HourlyRate.HourlyRateResponseModel> HourlyRate_GetByProductionSite(Identity.Models.UserModel user, int siteId)
		{
			return new Core.BaseData.Handlers.Settings.HourlyRate.GetByProductionSiteHandler(user, siteId)
				   .Handle();
		}
	}
}
