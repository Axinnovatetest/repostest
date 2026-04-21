using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class StoragesAndIndexesModel
	{
		public List<StorageLocation> storageLocations { get; set; }
		public List<KundeIndex> KundenIndexList { get; set; }
		public List<ArticleType> ArticleTypeList { get; set; }
		public class StorageLocation
		{
			public string KundenIndex { get; set; }
			public int Key { get; set; }
			public string Value { get; set; }
		}
		public class KundeIndex
		{
			public DateTime? Key { get; set; }
			public string Value { get; set; }
			public DateTime SnapshotTime { get; set; }

			public KundeIndex()
			{

			}
			public KundeIndex(Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex entity)
			{
				if(entity == null)
					return;

				// -
				Key = entity.KundenIndexDate;
				Value = entity.KundenIndex;
				SnapshotTime = entity.SnapshotTime;
			}
		}
		public class ArticleType
		{
			public int Key { get; set; }
			public string Value { get; set; }
		}
	}
}
