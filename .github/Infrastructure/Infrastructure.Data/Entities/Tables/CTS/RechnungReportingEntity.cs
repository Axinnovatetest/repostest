using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class RechnungReportingEntity
	{
		public string Footer1 { get; set; }
		public string Footer10 { get; set; }
		public string Footer11 { get; set; }
		public string Footer12 { get; set; }
		public string Footer13 { get; set; }
		public string Footer14 { get; set; }
		public string Footer15 { get; set; }
		public string Footer16 { get; set; }
		public string Footer17 { get; set; }
		public string Footer18 { get; set; }
		public string Footer19 { get; set; }
		public string Footer2 { get; set; }
		public string Footer20 { get; set; }
		public string Footer21 { get; set; }
		public string Footer22 { get; set; }
		public string Footer23 { get; set; }
		public string Footer3 { get; set; }
		public string Footer4 { get; set; }
		public string Footer5 { get; set; }
		public string Footer6 { get; set; }
		public string Footer7 { get; set; }
		public string Footer8 { get; set; }
		public string Footer9 { get; set; }
		public string Header1 { get; set; }
		public string Header2 { get; set; }
		public string Header3 { get; set; }
		public string Header4 { get; set; }
		public string Header5 { get; set; }
		public int Id { get; set; }
		public int? Lager { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUser { get; set; }
		public string List1Column1 { get; set; }
		public string List1Column10 { get; set; }
		public string List1Column11 { get; set; }
		public string List1Column12 { get; set; }
		public string List1Column13 { get; set; }
		public string List1Column14 { get; set; }
		public string List1Column15 { get; set; }
		public string List1Column16 { get; set; }
		public string List1Column17 { get; set; }
		public string List1Column18 { get; set; }
		public string List1Column2 { get; set; }
		public string List1Column3 { get; set; }
		public string List1Column4 { get; set; }
		public string List1Column5 { get; set; }
		public string List1Column6 { get; set; }
		public string List1Column7 { get; set; }
		public string List1Column8 { get; set; }
		public string List1Column9 { get; set; }
		public string List2Column1 { get; set; }
		public string List2Column2 { get; set; }
		public string List2Column3 { get; set; }
		public string List2Column4 { get; set; }
		public string List2Column5 { get; set; }
		public string List2Column6 { get; set; }
		public string List2Column7 { get; set; }
		public string List2Sum { get; set; }
		public int? LogoId { get; set; }
		public string SumTitle1 { get; set; }
		public string SumTitle2 { get; set; }
		public string SumTitle3 { get; set; }
		public string SumTitle4 { get; set; }
		public string SumTitle5 { get; set; }
		public string Title1 { get; set; }
		public string Title2 { get; set; }
		public string Title3 { get; set; }
		public string Title4 { get; set; }
		public string Title5 { get; set; }
		public string Title6 { get; set; }
		public string Title7 { get; set; }

		public RechnungReportingEntity() { }

		public RechnungReportingEntity(DataRow dataRow)
		{
			Footer1 = (dataRow["Footer1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer1"]);
			Footer10 = (dataRow["Footer10"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer10"]);
			Footer11 = (dataRow["Footer11"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer11"]);
			Footer12 = (dataRow["Footer12"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer12"]);
			Footer13 = (dataRow["Footer13"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer13"]);
			Footer14 = (dataRow["Footer14"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer14"]);
			Footer15 = (dataRow["Footer15"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer15"]);
			Footer16 = (dataRow["Footer16"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer16"]);
			Footer17 = (dataRow["Footer17"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer17"]);
			Footer18 = (dataRow["Footer18"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer18"]);
			Footer19 = (dataRow["Footer19"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer19"]);
			Footer2 = (dataRow["Footer2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer2"]);
			Footer20 = (dataRow["Footer20"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer20"]);
			Footer21 = (dataRow["Footer21"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer21"]);
			Footer22 = (dataRow["Footer22"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer22"]);
			Footer23 = (dataRow["Footer23"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer23"]);
			Footer3 = (dataRow["Footer3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer3"]);
			Footer4 = (dataRow["Footer4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer4"]);
			Footer5 = (dataRow["Footer5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer5"]);
			Footer6 = (dataRow["Footer6"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer6"]);
			Footer7 = (dataRow["Footer7"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer7"]);
			Footer8 = (dataRow["Footer8"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer8"]);
			Footer9 = (dataRow["Footer9"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer9"]);
			Header1 = (dataRow["Header1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Header1"]);
			Header2 = (dataRow["Header2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Header2"]);
			Header3 = (dataRow["Header3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Header3"]);
			Header4 = (dataRow["Header4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Header4"]);
			Header5 = (dataRow["Header5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Header5"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Lager = (dataRow["Lager"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lager"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUser = (dataRow["LastUpdateUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUser"]);
			List1Column1 = (dataRow["List1Column1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column1"]);
			List1Column10 = (dataRow["List1Column10"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column10"]);
			List1Column11 = (dataRow["List1Column11"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column11"]);
			List1Column12 = (dataRow["List1Column12"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column12"]);
			List1Column13 = (dataRow["List1Column13"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column13"]);
			List1Column14 = (dataRow["List1Column14"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column14"]);
			List1Column15 = (dataRow["List1Column15"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column15"]);
			List1Column16 = (dataRow["List1Column16"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column16"]);
			List1Column17 = (dataRow["List1Column17"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column17"]);
			List1Column18 = (dataRow["List1Column18"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column18"]);
			List1Column2 = (dataRow["List1Column2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column2"]);
			List1Column3 = (dataRow["List1Column3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column3"]);
			List1Column4 = (dataRow["List1Column4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column4"]);
			List1Column5 = (dataRow["List1Column5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column5"]);
			List1Column6 = (dataRow["List1Column6"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column6"]);
			List1Column7 = (dataRow["List1Column7"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column7"]);
			List1Column8 = (dataRow["List1Column8"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column8"]);
			List1Column9 = (dataRow["List1Column9"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List1Column9"]);
			List2Column1 = (dataRow["List2Column1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Column1"]);
			List2Column2 = (dataRow["List2Column2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Column2"]);
			List2Column3 = (dataRow["List2Column3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Column3"]);
			List2Column4 = (dataRow["List2Column4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Column4"]);
			List2Column5 = (dataRow["List2Column5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Column5"]);
			List2Column6 = (dataRow["List2Column6"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Column6"]);
			List2Column7 = (dataRow["List2Column7"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Column7"]);
			List2Sum = (dataRow["List2Sum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["List2Sum"]);
			LogoId = (dataRow["LogoId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LogoId"]);
			SumTitle1 = (dataRow["SumTitle1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SumTitle1"]);
			SumTitle2 = (dataRow["SumTitle2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SumTitle2"]);
			SumTitle3 = (dataRow["SumTitle3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SumTitle3"]);
			SumTitle4 = (dataRow["SumTitle4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SumTitle4"]);
			SumTitle5 = (dataRow["SumTitle5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SumTitle5"]);
			Title1 = (dataRow["Title1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title1"]);
			Title2 = (dataRow["Title2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title2"]);
			Title3 = (dataRow["Title3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title3"]);
			Title4 = (dataRow["Title4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title4"]);
			Title5 = (dataRow["Title5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title5"]);
			Title6 = (dataRow["Title6"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title6"]);
			Title7 = (dataRow["Title7"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title7"]);
		}
	}
}

