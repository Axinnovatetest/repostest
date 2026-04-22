using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Psz.Core.Apps.EDI.Tools
{
	public class FileWatcher
	{
		private FileSystemWatcher fileSystemWatcher;
		private string _newOrdersDirectory { get; set; }
		private string _errorOrderDirectory { get; set; }
		private string _processedOrdersDirectory { get; set; }

		public FileWatcher(string newOrdersDirectory,
			string errorOrderDirectory,
			string processedOrdersDirectory)
		{
			try
			{
				_errorOrderDirectory = errorOrderDirectory;
				_newOrdersDirectory = newOrdersDirectory;
				_processedOrdersDirectory = processedOrdersDirectory;

				Task.Run(() => processFiles());

				fileSystemWatcher = new FileSystemWatcher(newOrdersDirectory);

				// Watch all files.
				fileSystemWatcher.Filter = "";
				// -
				fileSystemWatcher.Created += new FileSystemEventHandler(fileCreated);
				fileSystemWatcher.Changed += new FileSystemEventHandler(fileCreated);
				fileSystemWatcher.Renamed += new RenamedEventHandler(fileCreated);

				// Start watching
				fileSystemWatcher.EnableRaisingEvents = true;

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, e.StackTrace);
				throw;
			}
		}

		private void fileCreated(object sender, FileSystemEventArgs e)
		{
			//lock (Locks.DocumentsLock)
			{
				Task.Run(() => processFiles());
			}
		}

		#region > New
		private void processFiles()
		{
			lock(Locks.DocumentsLock)
			{
				Thread.Sleep(1100);

				var newOrderErpelIndustryMessages = new List<Tuple<string, Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage, OportedOrdersHandler>>();
				var orderChangeErpelIndustryMessages = new List<Tuple<string, Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage, OportedOrdersHandler>>();

				foreach(var file in new DirectoryInfo(_newOrdersDirectory).GetFiles())
				{
					var deseralizer = new OportedOrdersHandler(file.FullName,
						_errorOrderDirectory,
						_processedOrdersDirectory);

					var erpelIndustryMessage = deserializeFile(file.FullName, deseralizer);
					if(erpelIndustryMessage == null)
					{
						continue;
					}

					if(erpelIndustryMessage.Item2.Document.Header.MessageHeader.MessageType.ToUpper() == "ORDERS")
					{
						newOrderErpelIndustryMessages.Add(erpelIndustryMessage);
					}
					else if(erpelIndustryMessage.Item2.Document.Header.MessageHeader.MessageType.ToUpper() == "ORDCHG")
					{
						orderChangeErpelIndustryMessages.Add(erpelIndustryMessage);
					}
				}

				foreach(var newOrderErpelIndustryMessage in newOrderErpelIndustryMessages)
				{
					newOrderErpelIndustryMessage.Item3.Handle(newOrderErpelIndustryMessage.Item2, newOrderErpelIndustryMessage.Item1);
				}

				foreach(var orderChangeErpelIndustryMessage in orderChangeErpelIndustryMessages)
				{
					orderChangeErpelIndustryMessage.Item3.Handle(orderChangeErpelIndustryMessage.Item2, orderChangeErpelIndustryMessage.Item1);
				}
			}
		}
		private Tuple<string, Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage, OportedOrdersHandler> deserializeFile(string fileName, OportedOrdersHandler deseralizer)
		{
			try
			{
				Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage erpelIndustryMessage = null;
				string documentName = null;

				while(isFileLocked(fileName))
				{
					//Thread.Sleep(300);
				}

				var wrongFormatDetected = false;
				using(var stream = new FileStream(fileName, FileMode.Open))
				{
					var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Models.OrderResponse.ErpelIndustryMessage));

					try
					{
						erpelIndustryMessage = (Models.OrderResponse.ErpelIndustryMessage)xmlSerializer.Deserialize(stream);

						var position = stream.Name.LastIndexOf('\\');
						documentName = stream.Name.Substring(position, stream.Name.Length - position);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
						wrongFormatDetected = true;
					}

					stream.Close();
				}

				if(wrongFormatDetected)
				{
					deseralizer.HandleError("\\" + System.IO.Path.GetFileName(fileName),
						"Wrong Document Format",
						"Other",
						-1);
					return null;
				}

				return new Tuple<string, Models.OrderResponse.ErpelIndustryMessage, OportedOrdersHandler>(documentName, erpelIndustryMessage, deseralizer);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return null;
			}
		}
		#endregion

		private bool isFileLocked(string fileName)
		{
			FileStream stream = null;

			try
			{
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
			} catch(IOException e)
			{
				if(e.HResult != -2147024864)
				{
					return false;
				}
				return true;
			} finally
			{
				if(stream != null)
				{
					stream.Close();
				}
			}

			return false;
		}


		public void moveErrorToNewFile(string fileName)
		{
			var moveTo = Path.Combine(_newOrdersDirectory, Path.GetFileName(fileName));
			var moveFrom = fileName; // Path.Combine(_errorOrderDirectory, Path.GetFileName(fileName));
			lock(Locks.DocumentsLock)
			{
				try
				{
					moveTo = OportedOrdersHandler.checkAndFixFileName(moveTo);
					OportedOrdersHandler.createIfNotExists(moveTo);

					File.Move(moveFrom, moveTo);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
