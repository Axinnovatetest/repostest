using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Infrastructure.Data.Entities.Tables.EDI.FileTrackingInEntity;

namespace Psz.Core.Apps.WorkPlan.Helpers
{
	public class FileHelper
	{
		public static List<InsertInEntity> ExtractFilesFromLogs(string rootPath)
		{
			var allResults = new List<InsertInEntity>();

			// 1. Get all .log files from the folder and its subfolders modified within the last 2 days
			var logFiles = Directory
				.GetFiles(rootPath, "*.log", SearchOption.AllDirectories)
				.Where(path => File.GetLastWriteTime(path) >= DateTime.Now.AddDays(-2))
				.ToList();

			// 2. Define the regex pattern once for matching log entries
			var regex = new Regex(@"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d+)\s+Transfer done:\s+'[^']*/([^/\\]+)\.xml'.*?\[(\d+)\]",
				RegexOptions.Compiled);

			// 3. Iterate over the filtered log files
			foreach(var path in logFiles)
			{
				if(!File.Exists(path))
					continue;

				// Read the full content of the current log file
				string content = File.ReadAllText(path);

				// Find all regex matches in the file content
				var matches = regex.Matches(content);

				// Process each matched entry
				foreach(Match match in matches)
				{
					string timestampStr = match.Groups[1].Value; // Extract timestamp string
					string fileName = match.Groups[2].Value;     // Extract file name without extension
					string number = match.Groups[3].Value;       // Extract the number inside brackets

					DateTime? parsedTimestamp = null;
					// Try to parse the timestamp string into a DateTime object with exact format
					if(DateTime.TryParseExact(timestampStr, "yyyy-MM-dd HH:mm:ss.fff",
						System.Globalization.CultureInfo.InvariantCulture,
						System.Globalization.DateTimeStyles.None,
						out DateTime dt))
					{
						parsedTimestamp = dt;
					}

					// Add the extracted data as a new entity to the results list
					allResults.Add(new InsertInEntity
					{
						FileName = fileName,
						Number = number,
						FileDateTime = parsedTimestamp,
						Segment2 = null // optional field "DocumentNumber"
					});
				}
			}
			return allResults;
		}
		//public static List<FileHelperExtractDocumentsEntity> ExtractDocumentInfo(string rootPath)
		//{
		//	var results = new List<FileHelperExtractDocumentsEntity>();

		//	var xmlFiles = Directory.GetFiles(rootPath, "*.xml", SearchOption.AllDirectories);

		//	foreach(var path in xmlFiles)
		//	{
		//		if(!File.Exists(path))
		//			continue;

		//		try
		//		{
		//			XDocument doc = XDocument.Load(path);
		//			var root = doc.Root;
		//			if(root == null)
		//				continue;

		//			// Récupérer namespace 'header'
		//			XNamespace headerNs = root.Attributes()
		//				.FirstOrDefault(a => a.IsNamespaceDeclaration && a.Name.LocalName == "header")
		//				?.Value ?? "";

		//			// Récupérer namespace 'document'
		//			XNamespace docNs = root.Attributes()
		//				.FirstOrDefault(a => a.IsNamespaceDeclaration && a.Name.LocalName == "document")
		//				?.Value ?? "";

		//			// DocumentNumber
		//			string documentNumber = !string.IsNullOrEmpty(docNs.NamespaceName)
		//				? doc.Descendants(docNs + "DocumentNumber").FirstOrDefault()?.Value
		//				: doc.Descendants("DocumentNumber").FirstOrDefault()?.Value;

		//			// MessageType
		//			string messageType = !string.IsNullOrEmpty(docNs.NamespaceName)
		//				? doc.Descendants(docNs + "MessageType").FirstOrDefault()?.Value
		//				: doc.Descendants("MessageType").FirstOrDefault()?.Value;

		//			// SenderId
		//			string senderId = !string.IsNullOrEmpty(headerNs.NamespaceName)
		//				? doc.Descendants(headerNs + "Sender").Descendants(headerNs + "id").FirstOrDefault()?.Value
		//				: doc.Descendants("Sender").Descendants("id").FirstOrDefault()?.Value;

		//			// RecipientId
		//			string recipientId = !string.IsNullOrEmpty(headerNs.NamespaceName)
		//				? doc.Descendants(headerNs + "Recipient").Descendants(headerNs + "id").FirstOrDefault()?.Value
		//				: doc.Descendants("Recipient").Descendants("id").FirstOrDefault()?.Value;

		//			// Si DocumentNumber trouvé, ajouter à la liste
		//			if(!string.IsNullOrEmpty(documentNumber))
		//			{
		//				var pathWithoutExtension = Path.Combine(
		//					Path.GetDirectoryName(path) ?? string.Empty,
		//					Path.GetFileNameWithoutExtension(path)
		//				);

		//				var relativePath = Path.GetRelativePath(rootPath, pathWithoutExtension);

		//				results.Add(new FileHelperExtractDocumentsEntity
		//				{
		//					DocumentNumber = documentNumber,
		//					FilePath = relativePath,
		//					MessageType = messageType ?? "",
		//					CustomerNumber = senderId ?? "",
		//					RecipientId = recipientId ?? ""
		//				});
		//			}
		//		} catch(Exception ex)
		//		{
		//			Console.WriteLine($"Erreur fichier {path}: {ex.Message}");
		//		}
		//	}

		//	return results;
		//}
		public static List<FileHelperExtractDocumentsEntity> ExtractDocumentInfo(List<string> filePaths)
		{
			var results = new List<FileHelperExtractDocumentsEntity>();

			// Return empty list if input list is null or empty
			if(filePaths == null || filePaths.Count == 0)
				return results;

			// Loop through each file path
			foreach(var filePath in filePaths)
			{
				// Skip if file path is null/empty or file does not exist
				if(string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
					continue;

				try
				{
					// Load the XML document from the file
					XDocument doc = XDocument.Load(filePath);
					var root = doc.Root;
					if(root == null)
						continue;

					// Get 'header' namespace if declared
					XNamespace headerNs = root.Attributes()
						.FirstOrDefault(a => a.IsNamespaceDeclaration && a.Name.LocalName == "header")
						?.Value ?? "";

					// Get 'document' namespace if declared
					XNamespace docNs = root.Attributes()
						.FirstOrDefault(a => a.IsNamespaceDeclaration && a.Name.LocalName == "document")
						?.Value ?? "";

					// Extract DocumentNumber element text
					string documentNumber = !string.IsNullOrEmpty(docNs.NamespaceName)
						? doc.Descendants(docNs + "DocumentNumber").FirstOrDefault()?.Value
						: doc.Descendants("DocumentNumber").FirstOrDefault()?.Value;

					// Extract MessageType element text
					string messageType = !string.IsNullOrEmpty(docNs.NamespaceName)
						? doc.Descendants(docNs + "MessageType").FirstOrDefault()?.Value
						: doc.Descendants("MessageType").FirstOrDefault()?.Value;

					// Extract Sender ID value 
					string senderId = !string.IsNullOrEmpty(headerNs.NamespaceName)
						? doc.Descendants(headerNs + "Sender").Descendants(headerNs + "id").FirstOrDefault()?.Value
						: doc.Descendants("Sender").Descendants("id").FirstOrDefault()?.Value;

					// Extract Recipient ID value
					string recipientId = !string.IsNullOrEmpty(headerNs.NamespaceName)
						? doc.Descendants(headerNs + "Recipient").Descendants(headerNs + "id").FirstOrDefault()?.Value
						: doc.Descendants("Recipient").Descendants("id").FirstOrDefault()?.Value;

					// If DocumentNumber is found, add an entry to the results list
					if(!string.IsNullOrEmpty(documentNumber))
					{
						// Get only the filename from the full file path
						var fileName = Path.GetFileNameWithoutExtension(filePath);

						results.Add(new FileHelperExtractDocumentsEntity
						{
							DocumentNumber = documentNumber,
							FilePath = fileName,
							MessageType = messageType ?? "",
							CustomerNumber = senderId ?? "",
							RecipientId = recipientId ?? ""
						});
					}
				} catch(Exception ex)
				{
					// Log any errors that occur while processing a file
					Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
				}
			}

			// Return the list of extracted document information entities
			return results;
		}

		public static string ExtractFileIdentifier(string path)
		{
			string fileName = Path.GetFileNameWithoutExtension(path);

			if(string.IsNullOrEmpty(fileName))
				return null;

			if(fileName.EndsWith("-"))
				fileName = fileName.Substring(0, fileName.Length - 1);

			var parts = fileName.Split('_');
			if(parts.Length == 2 && Guid.TryParse(parts[1], out _))
			{
				return parts[0] + "_" + parts[1];
			}

			return null;
		}

	}

}
