using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.ROH
{
	public class ROHPropertiesModel
	{
		public int IdLevelTwo { get; set; }
		public string NameLevelTwo { get; set; }
		public string PartLevelTwo { get; set; }
		public bool Required { get; set; }
		public bool IsFreeText { get; set; }
		public bool IsRange { get; set; }
		public List<ValuesLevelThreeModel> ValuesLevelThree { get; set; }
		public List<RangesLevelTwoModel> RangesLevelTwoFrom { get; set; }
		public List<RangesLevelTwoModel> RangesLevelTwoTo { get; set; }
	}
	public class ValuesLevelThreeModel
	{
		public int Key { get; set; }
		public string Value { get; set; }
		public bool IsFreeText { get; set; }
		public ValuesLevelThreeModel()
		{

		}
		public ValuesLevelThreeModel(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level3Entity entity)
		{
			Key = entity.Id;
			Value = entity.Name;
			IsFreeText = entity.IsFreeText ?? false;
		}
	}
	public class RangesLevelTwoModel
	{
		public int IdLevelTwo { get; set; }
		public int Type { get; set; }
		public int Key { get; set; }
		public string Value { get; set; }
		public RangesLevelTwoModel()
		{

		}
		public RangesLevelTwoModel(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity entity)
		{
			IdLevelTwo = entity.IdLevelTwo ?? -1;
			Key = entity.Id;
			Value = entity.RangeValue;
		}
	}
}