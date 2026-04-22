using Infrastructure.Data.Entities.Joins;
using Infrastructure.Data.Entities.Tables.PRS;
using OfficeOpenXml;
using Psz.Core.BaseData.Models.CustomerSupplierLP;
using Psz.Core.BaseData.Tools;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Settings.LP
{
	public class SupplierInformationUpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<SuppliersExcelBulkUpdateReponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private LPMinimalRequestModel _data { get; set; }

		public SupplierInformationUpdateHandler(Identity.Models.UserModel user, LPMinimalRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<SuppliersExcelBulkUpdateReponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				if(_data.ExcelFile is null || _data.ExcelFile?.Length == 0)
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"Invalid Excel File !");

				var validFile = Psz.Core.BaseData.Helpers.ExcelHelper.ExcelValidator.ValidateExcelFileColumns(_data.ExcelFile);
				if(!validFile)
				{
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"Invalid Excel File, File may not contains Data !");
				}
				var InvalidCells = Psz.Core.BaseData.Helpers.ExcelHelper.ExcelValidator.ValidateExcelFile(_data.ExcelFile).ToList();
				// uncomment For sever Validation 
				/*if(InvalidCells is not null && InvalidCells.Count > 0)
					return new ResponseModel<List<string>>(InvalidCells);*/

				return VerifyExcel();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static string SaveExcelFile(byte[] data)
		{
			var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
			File.WriteAllBytes(fileName, data);
			return fileName;
		}
		public ResponseModel<SuppliersExcelBulkUpdateReponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.SuccessResponse();
		}
		public ResponseModel<SuppliersExcelBulkUpdateReponseModel> VerifyExcel()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				// varaibles declaration
				List<string> mismatchcounter;
				var errors = new List<string>();
				List<LPMinimalModel> ArticlestoUpdate = new List<LPMinimalModel>();
				List<LPMinimalModel> OldArticlestoUpdate = new List<LPMinimalModel>();
				List<LPMinimalModel> InvalidArtciles = new List<LPMinimalModel>();
				// License for EPPLUS
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Filter Changes in Excel
				var Changedarticles = ReadDataFromExcelFile(_data.ExcelFile);
				if(ValidationFeedBack.excelMappingFeedBackModels is not null && ValidationFeedBack.excelMappingFeedBackModels.Count > 0)
				{
					var badcellsvaluesdetected = ValidationFeedBack.excelMappingFeedBackModels.Select(model => $"[{model.col}, {model.row}]").ToList();
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"Here are some Problem detected in these columns and rows  : {string.Join(",", badcellsvaluesdetected)} ,´please verify the data and try again ,update was not performed !");
				}

				var LPEntity = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetLPbyArtiklenummers(_data.nr, Changedarticles.Select(x => x.Artikelnummer).ToList());
				var BaseEntities = LPEntity.Select(x => new LPMinimalModel(x)).ToList();

				Changedarticles = Changedarticles.OrderBy(x => x.Artikelnummer).ToList();
				BaseEntities = BaseEntities.OrderBy(x => x.Artikelnummer).ToList();

				var datamismatch = ArtcilesMismatch(Changedarticles, BaseEntities, out mismatchcounter); //|| InvalidList(Changedarticles);
				if(datamismatch)
				{
					var ChangeArticlesNummer = Changedarticles.Select(x => x.Artikelnummer).ToList();
					var BaseArticlesNummer = BaseEntities.Select(x => x.Artikelnummer).ToList();
					var mismatch = ChangeArticlesNummer.Except(BaseArticlesNummer).ToList();
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"there is a  mismatch fore these articles,either the artikelnummer is duplicated , wrong or it does not exist  : {string.Join(", ", mismatch)} !");
				}
				LPMinimalModel.InvalidArticles.Clear();
				LPMinimalModel.HasContetnWillBeErrased.Clear();
				foreach(var pair in Changedarticles.Zip(BaseEntities, (item1, item2) => new { Item1 = item1, Item2 = item2 }))
				{
					if(pair.Item1.Equals(pair.Item2))
					{
						ArticlestoUpdate.Add(pair.Item1);
						OldArticlestoUpdate.Add(pair.Item2);
					}
				}

				if(LPMinimalModel.HasContetnWillBeErrased.Count() > 0)
				{
					var invalidArticlesNames = LPMinimalModel.HasContetnWillBeErrased.Select(x => x.Artikelnummer).ToList();
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"Angebot or Bestell_Nr is Empty and it Will be Errased for these Articles : {string.Join(",", invalidArticlesNames)} ,please provide non empty data ,update was not performed !");
				}
				if(LPMinimalModel.InvalidArticles.Count() > 0)
				{
					var invalidArticlesNames = LPMinimalModel.InvalidArticles.Select(x => x.Artikelnummer).ToList();
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"Invalid Dates For These Articles : {string.Join(",", invalidArticlesNames)} !");
				}
				foreach(var pair in ArticlestoUpdate.Zip(OldArticlestoUpdate, (item1, item2) => new { Item1 = item1, Item2 = item2 }))
				{
					pair.Item1.UpdateChangesLogs(pair.Item2);
				}
				// Update Logic 
				botransaction.beginTransaction();
				if(ArticlestoUpdate.Count == 0)
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"No changes Detected , No Update Will be performed !");
				var resultLog = new List<UpdateLogSummary>();
				foreach(var item in ArticlestoUpdate)
				{
					var data = new Infrastructure.Data.Entities.Joins.LPMinimalUpdateEntity()
					{
						Bestell_NrChangesLog = item.Bestell_NrChangesLog,
						EinkaufspreisChangesLog = item.EinkaufspreisChangesLog,
						Angebot_DatumChangesLog = item.Angebot_DatumChangesLog,
						AngebotChangesLog = item.AngebotChangesLog,
						MindestbestellmengeChangesLog = item.MindestbestellmengeChangesLog,
						WiederbeschaffungszeitraumChangesLog = item.WiederbeschaffungszeitraumChangesLog,

						Bestell_NrChanged = item.Bestell_NrChanged,
						EinkaufspreisChanged = item.EinkaufspreisChanged,
						Angebot_DatumChanged = item.Angebot_DatumChanged,
						AngebotChanged = item.AngebotChanged,
						MindestbestellmengeChanged = item.MindestbestellmengeChanged,
						WiederbeschaffungszeitraumChanged = item.WiederbeschaffungszeitraumChanged,

						Bestell_Nr = item.Bestell_Nr,
						Angebot = item.Angebot,
						Angebot_Datum = item.Angebot_Datum,
						Einkaufspreis = item.Einkaufspreis,
						Mindestbestellmenge = item.Mindestbestellmenge,
						Artikelnummer = item.Artikelnummer,
						Nr = item.Nr,
						ArtikelNr = item.ArtikelNr,
						Wiederbeschaffungszeitraum = item.Wiederbeschaffungszeitraum,

						Einkaufspreis1 = item.Einkaufspreis1,
						Einkaufspreis2 = item.Einkaufspreis2,
						Einkaufspreis1_gultig_bis = item.Einkaufspreis1_gultig_bis,
						Einkaufspreis2_gultig_bis = item.Einkaufspreis2_gultig_bis
					};

					Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.BuildUpdateQuery(data, data.Nr);
					var logs = AddArticleLog(data);
					if(logs is not null && logs.Count() > 0)
					{
						resultLog.AddRange(logs);
					}
				}
				if(botransaction.commit())
				{
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.SuccessResponse(new SuppliersExcelBulkUpdateReponseModel() { data = resultLog.Select(x => new UpdateLogSummaryMinimalModel(x)).ToList() });
				}
				else
				{
					return ResponseModel<SuppliersExcelBulkUpdateReponseModel>.FailureResponse(key: "1", value: $"batch Updating Articles Failed !");
				}

			} catch(Exception exception)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public bool HasInvalidAttributes(LPMinimalModel data)
		{
			if(string.IsNullOrEmpty(data.Artikelnummer) ||
				string.IsNullOrEmpty(data.Bestell_Nr) ||
				data.Einkaufspreis == null || data.Einkaufspreis == 0 ||
				string.IsNullOrEmpty(data.Angebot) ||
				data.Angebot_Datum == null ||
				data.Wiederbeschaffungszeitraum == null || data.Wiederbeschaffungszeitraum == 0 ||
				data.Mindestbestellmenge == null || data.Mindestbestellmenge == 0)
			{
				return true;
			}
			return false;
		}
		private bool InvalidList(List<LPMinimalModel> received)
		{
			var Invalidelement = received.Where(x => HasInvalidAttributes(x)).ToList();

			if(Invalidelement is not null && Invalidelement.Count > 0)
				return true;

			return false;
		}

		private bool ArtcilesMismatch(List<LPMinimalModel> received, List<LPMinimalModel> retrievedFromtheDb, out List<string> Mismatchcounters)
		{
			Mismatchcounters = new List<string>();

			try
			{
				if(received is null || received.Count == 0 || retrievedFromtheDb is null || retrievedFromtheDb.Count == 0 || retrievedFromtheDb.Count != received.Count)
					return true;

				int counter = 0;

				foreach(var item in received)
				{
					if(item.Artikelnummer != retrievedFromtheDb.ElementAt(counter).Artikelnummer)
					{
						Mismatchcounters.Add(item.Artikelnummer);
					}
					if(string.IsNullOrEmpty(item.Artikelnummer) || string.IsNullOrWhiteSpace(item.Artikelnummer) || string.IsNullOrEmpty(item.Artikelnummer) || string.IsNullOrWhiteSpace(item.Artikelnummer))
					{
						Mismatchcounters.Add(item.Artikelnummer);
					}
					counter++;
				}
				if(Mismatchcounters.Count > 0)
					return true;
				return false;

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return false;
		}
		private List<UpdateLogSummary> AddArticleLog(LPMinimalUpdateEntity item)
		{
			var logs = new List<UpdateLogSummary>();

			if(item.MindestbestellmengeChanged)
			{
				var Log = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity()
				{
					LastUpdateUsername = _user.Username,
					LastUpdateUserId = _user.Id,
					LastUpdateUserFullName = _user.Name,
					LogDescription = item.MindestbestellmengeChangesLog,
					LogObject = "Article",
					LastUpdateTime = DateTime.Now,
					LogObjectId = item.ArtikelNr,
				};
				logs.Add(new UpdateLogSummary() { Artikelnummer = item.Artikelnummer, ArtikleNr = item.ArtikelNr, changeLog = item.MindestbestellmengeChangesLog });
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(Log);
			}

			if(item.WiederbeschaffungszeitraumChanged)
			{
				var Log = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity()
				{
					LastUpdateUsername = _user.Username,
					LastUpdateUserId = _user.Id,
					LastUpdateUserFullName = _user.Name,
					LogDescription = item.WiederbeschaffungszeitraumChangesLog,
					LogObject = "Article",
					LastUpdateTime = DateTime.Now,
					LogObjectId = item.ArtikelNr,
				};
				logs.Add(new UpdateLogSummary() { Artikelnummer = item.Artikelnummer, ArtikleNr = item.ArtikelNr, changeLog = item.WiederbeschaffungszeitraumChangesLog });
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(Log);
			}
			if(item.Angebot_DatumChanged)
			{
				var Log = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity()
				{
					LastUpdateUsername = _user.Username,
					LastUpdateUserId = _user.Id,
					LastUpdateUserFullName = _user.Name,
					LogDescription = item.Angebot_DatumChangesLog,
					LogObject = "Article",
					LastUpdateTime = DateTime.Now,
					LogObjectId = item.ArtikelNr,
				};
				logs.Add(new UpdateLogSummary() { Artikelnummer = item.Artikelnummer, ArtikleNr = item.ArtikelNr, changeLog = item.Angebot_DatumChangesLog });
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(Log);
			}
			if(item.AngebotChanged)
			{
				var Log = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity()
				{
					LastUpdateUsername = _user.Username,
					LastUpdateUserId = _user.Id,
					LastUpdateUserFullName = _user.Name,
					LogDescription = item.AngebotChangesLog,
					LogObject = "Article",
					LastUpdateTime = DateTime.Now,
					LogObjectId = item.ArtikelNr,
				};
				logs.Add(new UpdateLogSummary() { Artikelnummer = item.Artikelnummer, ArtikleNr = item.ArtikelNr, changeLog = item.AngebotChangesLog });
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(Log);
			}
			if(item.EinkaufspreisChanged)
			{
				var Log = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity()
				{
					LastUpdateUsername = _user.Username,
					LastUpdateUserId = _user.Id,
					LastUpdateUserFullName = _user.Name,
					LogDescription = item.EinkaufspreisChangesLog,
					LogObject = "Article",
					LastUpdateTime = DateTime.Now,
					LogObjectId = item.ArtikelNr,
				};
				logs.Add(new UpdateLogSummary() { Artikelnummer = item.Artikelnummer, ArtikleNr = item.ArtikelNr, changeLog = item.EinkaufspreisChangesLog });
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(Log);
			}
			if(item.Bestell_NrChanged)
			{
				var Log = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity()
				{
					LastUpdateUsername = _user.Username,
					LastUpdateUserId = _user.Id,
					LastUpdateUserFullName = _user.Name,
					LogDescription = item.Bestell_NrChangesLog,
					LogObject = "Article",
					LastUpdateTime = DateTime.Now,
					LogObjectId = item.ArtikelNr,
				};
				logs.Add(new UpdateLogSummary() { Artikelnummer = item.Artikelnummer, ArtikleNr = item.ArtikelNr, changeLog = item.Bestell_NrChangesLog });
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(Log);
			}

			return logs;
		}
		private List<LPMinimalModel> ReadDataFromExcelFile(Microsoft.AspNetCore.Http.IFormFile ExcelFile)
		{
			using(MemoryStream memoryStream = new MemoryStream())
			{
				ExcelFile.CopyTo(memoryStream);
				memoryStream.Seek(0, SeekOrigin.Begin);
				return ConvertWorksheetToCollection(memoryStream);
			}
		}
		private List<LPMinimalModel> ConvertWorksheetToCollection(MemoryStream memoryStream)
		{
			List<LPMinimalModel> data = new List<LPMinimalModel>();
			using(ExcelPackage excel = new ExcelPackage(memoryStream))
			{
				var workSheet = excel.Workbook.Worksheets["Lieferant - LP"];
				data = workSheet.MapXLSSheetToDTO<LPMinimalModel>().ToList();
			}
			return data;
		}
	}
}
