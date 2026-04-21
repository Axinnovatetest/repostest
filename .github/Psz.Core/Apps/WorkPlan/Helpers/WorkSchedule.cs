namespace Psz.Core.Apps.WorkPlan.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class WorkSchedule
	{
		public static int EXCEL_ROUND_DECIMALS = 4;
		public static byte[] GetWorkDetailsPDF(int workScheduleId, Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(workScheduleId);
				if(workScheduleDb == null)
				{
					throw (new Psz.Core.Exceptions.NotFoundException("Work plan not found."));
				}

				var workScheduleDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(workScheduleId);
				if(workScheduleDetailsDb == null)
				{
					throw (new Psz.Core.Exceptions.NotFoundException("Work plan operations not found."));
				}

				var resultFilePath = Path.Combine(Path.GetTempPath(), $"Workplan_{DateTime.UtcNow.ToString("yyyyMMddTHHmmss.fff")}.pdf");
				var isCreatePDF = GeneratePDF(resultFilePath, workScheduleDb, workScheduleDetailsDb);
				if(isCreatePDF == true)
				{
					return File.ReadAllBytes(resultFilePath);
				}

				return null;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static bool GeneratePDF(string resultFilePath, Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity workPlanEntity,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity> wokScheduleDetailsEntityList)
		{


			return false;
		}

		public static double GetOperationTimeValueAddng(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity workScheduleDetailsEntity)
		{
			try
			{
				if(workScheduleDetailsEntity == null)
					return 0;

				if(workScheduleDetailsEntity.OperationValueAdding == true)
				{
					if(workScheduleDetailsEntity.RelationOperationTime == 1) // piece
					{
						return (double)workScheduleDetailsEntity.Amount * (double)workScheduleDetailsEntity.OperationTimeSeconds / 60;
					}
					else
					{
						return (double)workScheduleDetailsEntity.Amount * (double)workScheduleDetailsEntity.OperationTimeSeconds / 60 / (workScheduleDetailsEntity.LotSizeSTD == 0 ? 1 : workScheduleDetailsEntity.LotSizeSTD);
					}
				}
				else
				{
					return 0;
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return 0;
			}
		}
		public static Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity setTotalTimeOperation(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity workSchedule)
		{
			// Formula taken from excel doc
			try
			{
				workSchedule.TotalTimeOperation = 0;
				// Lot
				if(workSchedule.RelationOperationTime == 0)
				{
					workSchedule.TotalTimeOperation =
						workSchedule.Amount * workSchedule.OperationTimeSeconds / 60 / workSchedule.LotSizeSTD
						+ workSchedule.SetupTimeMinutes / workSchedule.LotSizeSTD;
				}
				else
				{
					// Piece
					if(workSchedule.RelationOperationTime == 1)
					{
						workSchedule.TotalTimeOperation =
							workSchedule.Amount * workSchedule.OperationTimeSeconds / 60
							+ workSchedule.SetupTimeMinutes / workSchedule.LotSizeSTD;
					}
				}

				workSchedule.TotalTimeOperation = Math.Truncate(workSchedule.TotalTimeOperation * 100000) / 100000;
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
			}

			return workSchedule;
		}
	}
}