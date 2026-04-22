using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Tools
{
	public static class Extensions
	{
		public static IEnumerable<T> MapXLSSheetToDTO<T>(this ExcelWorksheet worksheet) where T : new()
		{
			ValidationFeedBack.excelMappingFeedBackModels.Clear();

			Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(Column);

			var columns = typeof(T)
					.GetProperties()
					.Where(x => x.CustomAttributes.Any(columnOnly))
			.Select(p => new
			{
				Property = p,
				Column = p.GetCustomAttributes<Column>().First().ColumnIndex
			}).ToList();


			var rows = worksheet.Cells
				.Select(cell => cell.Start.Row)
				.Distinct()
				.OrderBy(x => x);


			var collection = rows.Skip(2)
				.Select(row =>
				{


					var tnew = new T();
					columns.ForEach(col =>
					{

						try
						{

							var val = worksheet.Cells[row, col.Column];
							if(val.Value == null)
							{
								col.Property.SetValue(tnew, null);
								return;
							}
							if(col.Property.PropertyType == typeof(Int32))
							{
								col.Property.SetValue(tnew, val.GetValue<int>());
								return;
							}
							if(col.Property.PropertyType == typeof(Int32?))
							{
								col.Property.SetValue(tnew, val.GetValue<int?>());
								return;
							}
							if(col.Property.PropertyType == typeof(double))
							{
								col.Property.SetValue(tnew, val.GetValue<double>());
								return;
							}
							if(col.Property.PropertyType == typeof(double?))
							{
								col.Property.SetValue(tnew, val.GetValue<double?>());
								return;
							}
							if(col.Property.PropertyType == typeof(DateTime))
							{
								col.Property.SetValue(tnew, val.GetValue<DateTime>());
								return;
							}
							if(col.Property.PropertyType == typeof(DateTime?))
							{
								col.Property.SetValue(tnew, val.GetValue<DateTime?>());
								return;
							}
							if(col.Property.PropertyType == typeof(decimal?))
							{
								col.Property.SetValue(tnew, val.GetValue<decimal?>());
								return;
							}
							if(col.Property.PropertyType == typeof(decimal))
							{
								col.Property.SetValue(tnew, val.GetValue<decimal>());
								return;
							}
							col.Property.SetValue(tnew, val.GetValue<string>());
						} catch(Exception e)
						{
							ValidationFeedBack.excelMappingFeedBackModels.Add(new ExcelMappingFeedBackModel() { col = col.Column, row = row });
						}

					});
					return tnew;
				});
			return collection;
		}
	}
	public class ValidationFeedBack
	{
		public static List<ExcelMappingFeedBackModel> excelMappingFeedBackModels = new List<ExcelMappingFeedBackModel>();
	}
	public class ExcelMappingFeedBackModel
	{
		public string ArtikleNummer { get; set; }
		public int row { get; set; }
		public int col { get; set; }
	}
}
