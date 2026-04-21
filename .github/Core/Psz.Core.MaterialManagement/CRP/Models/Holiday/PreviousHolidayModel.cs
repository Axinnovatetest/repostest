namespace Psz.Core.MaterialManagement.CRP.Models.Holiday
{
	public class PreviousHolidayResponseModel: HolidayModel
	{
		public bool Selected { get; set; }
		public PreviousHolidayResponseModel()
		{

		}
		public PreviousHolidayResponseModel(
			Infrastructure.Data.Entities.Tables.MTM.HolidayEntity holidayEntity,
			string creationUserName, string userLanguage = "en")
			: base(holidayEntity, userLanguage)
		{
			Selected = false;
		}
	}
}
