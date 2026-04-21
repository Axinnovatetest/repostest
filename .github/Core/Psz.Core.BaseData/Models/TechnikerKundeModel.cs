using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models
{
	public class TechnikerKundeModel
	{
		public TechnikerKundeModel(Infrastructure.Data.Entities.Tables.TechnikerKundeEntity entity)
		{

			Id = entity.Id;
			TechnikerKundeName = entity.TechnikerKundeName;

		}
		public int Id { get; set; }
		public string TechnikerKundeName { get; set; }
	}
}
