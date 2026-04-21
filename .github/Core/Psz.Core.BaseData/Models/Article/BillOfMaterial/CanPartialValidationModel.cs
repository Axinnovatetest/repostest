namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class CanPartialValidationModel
	{
		public bool CanPartialValidation { get; set; }
		public int? LastBomVersion { get; set; }
		public string LastBomIndexKunde { get; set; }
		public bool Value { get; set; } = false;
	}
}
