using OfficeOpenXml;
using OfficeOpenXml.Style;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using System.Drawing;
using System.Globalization;

namespace Psz.Core.MaterialManagement.Helpers
{
	public class ExcelHelper
	{
		public static ExcelWorksheet FAVerschiebungExcel(List<FAVerschiebungModel> _data, ExcelPackage Ep)
		{
			ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add($"FA Verschiebung");
			//headers
			Sheet.Cells["A1"].Value = "Fertigungsnummer";
			Sheet.Cells["B1"].Value = "Änderungsdatum";
			Sheet.Cells["C1"].Value = "Termin Aktuell";
			Sheet.Cells["D1"].Value = "Termin_Ursprünglich";
			Sheet.Cells["E1"].Value = "Termin_voränderung";
			Sheet.Cells["F1"].Value = "Zeitraum";
			Sheet.Cells["G1"].Value = "Lagerort_id";
			Sheet.Cells["H1"].Value = "Kennzeichen";
			List<string> Headers = new List<string>
			{ "A1","B1","C1","D1","E1","F1","G1","H1" };
			foreach(string header in Headers)
			{
				Sheet.Cells[header].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			}
			int row = 2;
			var cl = new CultureInfo("de-DE");
			foreach(var p in _data)
			{
				var diffrence = !p.Termin_voranderung.HasValue
					? (p.Termin_Bestatigt1.Value - p.Termin_Ursprunglich.Value).Days
					: (p.Termin_Bestatigt1.Value - p.Termin_voranderung.Value).Days;

				Sheet.Cells[string.Format("A{0}", row)].Value = p.Fertigungsnummer;
				Sheet.Cells[string.Format("B{0}", row)].Value = p.Anderungsdatum.HasValue ? p.Anderungsdatum.Value.ToString("dd-MMM-yy", cl) : "";
				Sheet.Cells[string.Format("C{0}", row)].Value = p.Termin_Bestatigt1.HasValue ? p.Termin_Bestatigt1.Value.ToString("dd-MMM-yy", cl) : "";
				Sheet.Cells[string.Format("D{0}", row)].Value = p.Termin_Ursprunglich.HasValue ? p.Termin_Ursprunglich.Value.ToString("dd-MMM-yy", cl) : "";
				Sheet.Cells[string.Format("E{0}", row)].Value = p.Termin_voranderung.HasValue ? p.Termin_voranderung.Value.ToString("dd-MMM-yy", cl) : "";
				Sheet.Cells[string.Format("F{0}", row)].Value = diffrence;
				Sheet.Cells[string.Format("G{0}", row)].Value = p.Lagerort_id;
				Sheet.Cells[string.Format("H{0}", row)].Value = p.Kennzeichen;
				row++;
			}
			Sheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
			Sheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.White);
			Sheet.Cells["A1:H1"].Style.Font.Color.SetColor(Color.Blue);
			Sheet.Cells["A1:H1"].Style.Font.Bold = true;

			Sheet.Cells[string.Format("A1:H{0}", row - 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
			Sheet.Cells[string.Format("A1:H{0}", row - 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
			Sheet.Cells[string.Format("A1:H{0}", row - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
			Sheet.Cells[string.Format("A1:H{0}", row - 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;

			Sheet.Cells[Sheet.Dimension.Address].AutoFitColumns();
			Sheet.Calculate();
			return Sheet;
		}
	}
}
