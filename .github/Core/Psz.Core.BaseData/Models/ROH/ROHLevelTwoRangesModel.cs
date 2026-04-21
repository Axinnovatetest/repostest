using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ROH
{
	public class ROHLevelTwoRangesModel
	{
		public int? FromOrTwo { get; set; }
		public int Id { get; set; }
		public int? IdLevelTwo { get; set; }
		public string RangeValue { get; set; }
		public ROHLevelTwoRangesModel()
		{

		}
		public ROHLevelTwoRangesModel(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity entity)
		{
			FromOrTwo = entity.FromOrTwo;
			Id = entity.Id;
			IdLevelTwo = entity.IdLevelTwo;
			RangeValue = entity.RangeValue;
		}

		public Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity
			{
				FromOrTwo = FromOrTwo,
				Id = Id,
				RangeValue = RangeValue,
				IdLevelTwo = IdLevelTwo
			};
		}
	}
}