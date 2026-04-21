using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.Configuration
{
	public class UnitOfMeasureRequestModel
	{
		public int Id { get; set; }
		public string Symbol { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public UnitOfMeasureRequestModel()
		{

		}
		public Infrastructure.Data.Entities.Tables.BSD.UnitOfMeasureEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.UnitOfMeasureEntity
			{
				Id = Id,
				Symbol = Symbol,
				Name = Name,
				Description = Description,
			};
		}
	}
	public class UnitOfMeasureResponseModel
	{
		public int Id { get; set; }
		public string Symbol { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public UnitOfMeasureResponseModel(Infrastructure.Data.Entities.Tables.BSD.UnitOfMeasureEntity entity)
		{
			if(entity == null)
				return;
			// -
			Id = entity.Id;
			Symbol = entity.Symbol;
			Name = entity.Name;
			Description = entity.Description;
		}
	}
}
