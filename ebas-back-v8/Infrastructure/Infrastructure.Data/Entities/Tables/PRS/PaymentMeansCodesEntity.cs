using System;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class PaymentMeansCodesEntity
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string DescriptionEnglish { get; set; }
		public PaymentMeansCodesEntity() { }
		public PaymentMeansCodesEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			Code = Convert.ToString(dataRow["Code"]);
			DescriptionEnglish = Convert.ToString(dataRow["DescriptionEnglish"]);
		}
		public PaymentMeansCodesEntity ShallowClone()
		{
			return new PaymentMeansCodesEntity
			{
				Id = Id,
				Code = Code,
				DescriptionEnglish = DescriptionEnglish,
			};
		}
	}
}
