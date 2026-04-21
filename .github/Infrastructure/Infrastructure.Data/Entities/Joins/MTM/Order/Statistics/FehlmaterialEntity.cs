using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class FehlmaterialEntity
	{
		public int? Anz { get; set; }
		public string Artik_Nr { get; set; }
		public string Artik_Nr2 { get; set; }
		public string B1 { get; set; }
		public string BZ2 { get; set; }
		public int? Fer { get; set; }
		public Single? Prod { get; set; }
		public decimal? SummBe { get; set; }
		public decimal? SummFAB { get; set; }
		public DateTime? T_B1 { get; set; }
		public DateTime? T_F { get; set; }
		public decimal? UmCZ { get; set; }
		public decimal? Ver { get; set; }

		public FehlmaterialEntity() { }

		public FehlmaterialEntity(DataRow dataRow)
		{
			Anz = (dataRow["Anz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anz"]);
			Artik_Nr = (dataRow["Artik_Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artik_Nr"]);
			Artik_Nr2 = (dataRow["Artik_Nr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artik_Nr2"]);
			B1 = (dataRow["B1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["B1"]);
			BZ2 = (dataRow["BZ2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BZ2"]);
			Fer = (dataRow["Fer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fer"]);
			Prod = (dataRow["Prod"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Prod"]);
			SummBe = (dataRow["SummBe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SummBe"]);
			SummFAB = (dataRow["SummFAB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SummFAB"]);
			T_B1 = (dataRow["T_B1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["T_B1"]);
			T_F = (dataRow["T_F"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["T_F"]);
			UmCZ = (dataRow["UmCZ"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UmCZ"]);
			Ver = (dataRow["Ver"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ver"]);
		}

		public FehlmaterialEntity ShallowClone()
		{
			return new FehlmaterialEntity
			{
				Anz = Anz,
				Artik_Nr = Artik_Nr,
				Artik_Nr2 = Artik_Nr2,
				B1 = B1,
				BZ2 = BZ2,
				Fer = Fer,
				Prod = Prod,
				SummBe = SummBe,
				SummFAB = SummFAB,
				T_B1 = T_B1,
				T_F = T_F,
				UmCZ = UmCZ,
				Ver = Ver
			};
		}

	}


	public class FAFehlmaterialModel
	{
		public int Anz { get; set; }
		public string Artik_Nr { get; set; }
		public string Artik_Nr2 { get; set; }
		public string B1 { get; set; }
		public string BZ2 { get; set; }
		public int Fer { get; set; }
		public Single Prod { get; set; }
		public decimal SummBe { get; set; }
		public decimal SummFAB { get; set; }
		public DateTime T_B1 { get; set; }
		public DateTime T_F { get; set; }
		public decimal UmCZ { get; set; }
		public decimal Ver { get; set; }
		public FAFehlmaterialModel(FehlmaterialEntity entity)
		{
			if(entity == null)
			{
				return;
			}


			Anz = entity.Anz ?? 0;
			Artik_Nr = entity.Artik_Nr ?? "";
			Artik_Nr2 = entity.Artik_Nr2 ?? "";
			B1 = entity.B1 ?? "";
			BZ2 = entity.BZ2 ?? "";
			Fer = entity.Fer ?? 0;
			Prod = entity.Prod ?? 0;
			SummBe = entity.SummBe ?? 0;
			SummFAB = entity.SummFAB ?? 0;
			T_B1 = entity.T_B1 ?? DateTime.MinValue;
			T_F = entity.T_F ?? DateTime.MinValue;
			UmCZ = entity.UmCZ ?? 0;
			Ver = entity.Ver ?? 0;
		}
	}
}
