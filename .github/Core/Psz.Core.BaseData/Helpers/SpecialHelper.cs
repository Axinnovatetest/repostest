using Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.BaseData.Models.Article.Statistics.Sales;

namespace Psz.Core.BaseData.Helpers
{
	public class SpecialHelper
	{
		private static string PairDelimiter = "|";
		private static string EntryDelimiter = ";";
		public static int GetHauptLager(int fetigungLager)
		{

			switch(fetigungLager)
			{
				case 7:
					return 4;
				case 42:
					return 41;
				case 26:
					return 24;
				case 6:
				case 21:
					return 3;
				case 60:
					return 58;
				default:
					return 0;
			}
		}

		public static List<Infrastructure.Services.Email.Models.MaterialRequestbaseModel> GenerateMaterials(List<ArticleOfferRequestsEntity> allOfferForOneSupplier)
		{
			var marginList = new List<int?>();
			var generatedMaterials = new List<Infrastructure.Services.Email.Models.MaterialRequestbaseModel>();


			if(allOfferForOneSupplier is null || allOfferForOneSupplier.Count() == 0)
				return generatedMaterials;

			foreach(var singleEntity in allOfferForOneSupplier)
			{
				marginList.Clear();

				var entity = new Infrastructure.Services.Email.Models.MaterialRequestbaseModel()
				{
					MatNr = singleEntity.ManufactuerNumber,
					Jahresmenge = singleEntity.YearlyQuantity,
					Hersteller = singleEntity.SupplierContactName,
					Bez = singleEntity.OfferRequestArticleDescription,
					unit = OfferRequestsManager.GetUnitByIDForTable(int.Parse(singleEntity.unit)),
				};
				if(singleEntity.Graduatedquantity1 > 0)
				{
					marginList.Add(singleEntity.Graduatedquantity1);

				}
				if(singleEntity.Graduatedquantity1 > 0)
				{
					marginList.Add(singleEntity.Graduatedquantity2);
				}
				if(singleEntity.Graduatedquantity1 > 0)
				{
					marginList.Add(singleEntity.Graduatedquantity3);
				}

				if(marginList.Count > 0)
				{
					marginList = marginList.Where(x => x > 0).ToList().OrderBy(x => x).ToList();
					entity.StckAzhalExists = true;
					entity.Quantitymargin = string.Join("..", marginList.Where(i => i.HasValue).Select(i => i.Value.ToString()));
				}
				else
				{
					entity.Quantitymargin = string.Empty;
				}

				generatedMaterials.Add(entity);
			}

			return generatedMaterials;

		}
		public static List<int> ParseIds(string idsString)
		{
			if(string.IsNullOrWhiteSpace(idsString))
			{
				return new List<int> { -1 };
			}
			try
			{
				List<int> ids = idsString.Split(',')
										 .Select(id => int.Parse(id.Trim()))
										 .ToList();
				return ids;
			} catch(FormatException)
			{
				return new List<int> { -1 };
			}
		}

		public static string ReplacePTagsWithDiv(string input)
		{
			string result = Regex.Replace(input, @"<\s*p(\s[^>]*)?>", "<div$1>", RegexOptions.IgnoreCase);
			result = Regex.Replace(result, @"<\s*/\s*p\s*>", "</div>", RegexOptions.IgnoreCase);
			return result;
		}

		public static string CreateFileEntry(string fileName, int fileId)
		{
			return $"{fileName}{EntryDelimiter}{fileId}";
		}
		public static (string FileName, int FileId) ParseFileEntry(string fileEntry)
		{
			var parts = fileEntry.Split(new[] { EntryDelimiter }, StringSplitOptions.None);
			if(parts.Length != 2 || !int.TryParse(parts[1], out int fileId))
			{
				throw new ArgumentException("Invalid file entry format.");
			}
			return (parts[0], fileId);
		}
		public static string AppendFileEntry(string existingEntries, string newFileEntry)
		{
			if(string.IsNullOrWhiteSpace(existingEntries))
			{
				return newFileEntry;
			}
			return $"{existingEntries}{PairDelimiter}{newFileEntry}";
		}
		public static string RemoveFileEntry(string existingEntries, int fileId)
		{
			if(string.IsNullOrWhiteSpace(existingEntries))
			{
				return existingEntries;
			}
			var entries = existingEntries.Split(new[] { PairDelimiter }, StringSplitOptions.RemoveEmptyEntries);
			var updatedEntries = new List<string>();

			foreach(var entry in entries)
			{
				var parts = entry.Split(new[] { EntryDelimiter }, StringSplitOptions.None);
				if(parts.Length == 2 && int.TryParse(parts[1], out int currentId))
				{
					if(currentId != fileId)
					{
						updatedEntries.Add(entry);
					}
				}
			}
			return updatedEntries.Count > 0 ? string.Join(PairDelimiter, updatedEntries) : null;
		}
		public static List<int> GetAllFileIds(string existingEntries)
		{
			try
			{
				var fileIds = new List<int>();

				if(string.IsNullOrWhiteSpace(existingEntries))
				{
					return fileIds;
				}
				var entries = existingEntries.Split(new[] { PairDelimiter }, StringSplitOptions.RemoveEmptyEntries);

				foreach(var entry in entries)
				{
					var parts = entry.Split(new[] { EntryDelimiter }, StringSplitOptions.None);
					if(parts.Length == 2 && int.TryParse(parts[1], out int fileId))
					{
						fileIds.Add(fileId);
					}
				}

				return fileIds;
			} catch(Exception)
			{
				return null;
			}
		}

		public static List<Attachments> GetAllAttachments(string existingEntries)
		{
			try
			{
				var attachmentsList = new List<Attachments>();

				if(string.IsNullOrWhiteSpace(existingEntries))
				{
					return attachmentsList; // Return an empty list if the string is null or empty
				}

				var entries = existingEntries.Split(new[] { PairDelimiter }, StringSplitOptions.RemoveEmptyEntries);

				foreach(var entry in entries)
				{
					var parts = entry.Split(new[] { EntryDelimiter }, StringSplitOptions.None);
					if(parts.Length == 2 && int.TryParse(parts[1], out int fileId))
					{
						attachmentsList.Add(new Attachments
						{
							FileName = parts[0],
							FiledId = fileId
						});
					}
				}

				return attachmentsList;
			} catch(Exception)
			{
				return null;
			}
		}
		public static KeyValuePair<int, int> GetWeekAndYearFromValue(string input)
		{
			if(string.IsNullOrWhiteSpace(input))
				throw new ArgumentException("Input cannot be null or empty.");

			var parts = input.Split('/');
			if(parts.Length != 2 || !parts[1].StartsWith("KW"))
				throw new FormatException("Input must be in format 'YYYY/W##'.");

			if(!int.TryParse(parts[0], out int year))
				throw new FormatException("Year is not a valid integer.");

			string weekPart = parts[1].Substring(2); // Remove the 'W'
			if(!int.TryParse(weekPart, out int week))
				throw new FormatException("Week is not a valid integer.");

			return new KeyValuePair<int, int>(year, week);
		}

		public static List<ArticleROHNeedStockAsList> CutOrderWaste(List<ArticleROHNeedStockAsList> data, decimal minToOrderQty)
		{
			var lastIndexWithMinToOrder = GetLastIndexWithMinToOrder(data);
			if(lastIndexWithMinToOrder == -1)
				return data; // No MinToOrder found, return original data

			// take the chunk of the list after the last Min to order not null
			var lastElementWithMinToOrder = data[lastIndexWithMinToOrder];
			var dataToCut = data.Skip(lastIndexWithMinToOrder + 1).ToList();
			var sumNeeds = dataToCut.Sum(x => x.Need);
			if(sumNeeds < lastElementWithMinToOrder.MinToOrder)
			{
				data[lastIndexWithMinToOrder].MinToOrder = sumNeeds + data[lastIndexWithMinToOrder].Need - data[lastIndexWithMinToOrder].Lager;
			}
			for(int i = 1; i < data.Count; i++)
			{
				//var toTakeFrom = data[i - 1].MinToOrder == 0 ? data[i - 1].Lager : data[i - 1].MinToOrder;
				data[i].Lager = data[i - 1].Lager + data[i - 1].MinToOrder - data[i - 1].Need;
			}

			return data;
		}
		public static ArticleROHNeedStockResponseModel ListToInstance(List<ArticleROHNeedStockAsList> data, ArticleROHNeedStockResponseModel item)
		{
			for(int i = 1; i <= 52; i++)
			{
				var labelProperty = item.GetType().GetProperty($"Label_CW{i}");
				var orderProperty = item.GetType().GetProperty($"Order_CW{i}");
				var needProperty = item.GetType().GetProperty($"Need_CW{i}");
				var lagerProperty = item.GetType().GetProperty($"Lager_CW{i}");
				var minToOrderProperty = item.GetType().GetProperty($"MinToOrder_CW{i}");

				var labelToSet = $"{data[i - 1].Year}/KW{data[i - 1].Week}";
				var orderToSet = data[i - 1].Order;
				var needToSet = data[i - 1].Need;
				var lagerToSet = data[i - 1].Lager;
				var minToOrderToSet = data[i - 1].MinToOrder;

				labelProperty.SetValue(item, labelToSet);
				orderProperty.SetValue(item, orderToSet);
				needProperty.SetValue(item, needToSet);
				lagerProperty.SetValue(item, lagerToSet);
				minToOrderProperty.SetValue(item, minToOrderToSet);
			}
			return item;
		}
		public static int GetLastIndexWithMinToOrder(List<ArticleROHNeedStockAsList> data)
		{
			for(int i = data.Count - 1; i >= 0; i--)
			{
				if(data[i].MinToOrder > 0)
				{
					return i;
				}
			}
			return -1; // Return -1 if no index with MinToOrder > 0 is found
		}
	}
}
