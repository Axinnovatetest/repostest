using Psz.Core.BaseData.Models.ROH;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System;

namespace Psz.Core.BaseData.Handlers.ROH
{
	public partial class RohArtikelnummer
	{
		public ResponseModel<int> AddRohArtikelnummerLevel3(RohArtikelnummerLevel3Model data, UserModel user)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				if(user == null)
					return ResponseModel<int>.AccessDeniedResponse();
				if(user.Access?.MasterData?.AddRohArtikelNummer != true)
					return ResponseModel<int>.FailureResponse("User does not have access");
				var exsist = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level3Access.GetByAllDetails(data.IdLevelOne ?? -1, data.IdLevelTwo ?? -1, data.PartOrder ?? -1, data.Name, data.Part);
				if(exsist != null)
					return ResponseModel<int>.FailureResponse("Property value with the same details already exsists.");
				var entityLevelOne = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Get(data.IdLevelOne ?? -1);
				var validateErrors = new List<string>();
				var inConflict = Helpers.ROHHelper.AreInConflict(entityLevelOne.Part.Replace('x', '0'), data.Part, validateErrors);
				if((inConflict || validateErrors?.Count > 0) && data.PartOrder == 1)
					return ResponseModel<int>.FailureResponse($"Property value part [{data.Part}] is in conflict with its article type part [{entityLevelOne.Part}]");
				var sameLevel2AndSamePartOrder = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level3Access.GetByIdLevelOneAndDiffrentIdLevelTwoAndPartOrder(data.IdLevelOne ?? -1, data.IdLevelTwo ?? -1, data.PartOrder ?? -1);
				if(sameLevel2AndSamePartOrder != null && sameLevel2AndSamePartOrder.Count > 0)
				{
					var errors = new List<string>();
					sameLevel2AndSamePartOrder.ForEach(x =>
					{
						var part2 = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level2Access.Get(x.IdLevelTwo ?? -1);
						if(Helpers.ROHHelper.AreInConflict(x.Part, data.Part, validateErrors))
							errors.Add($"The entred part [{data.Part}] and the part [{x.Part}] of the property value [{x.Name}] of the property [{part2.Name}] effects the same part order and are in conflict.");
					});
					if(errors != null && errors.Count > 0)
						return ResponseModel<int>.FailureResponse(errors);
				}
				var entity = data.ToEntity();
				var response = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level3Access.InsertWithTransaction(entity, botransaction.connection, botransaction.transaction);
				//Logging 
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
		new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
									ObjectLogHelper.getLog(user, response, "ROH Artikel level 3 name :",
									"",
									$"{data.Name}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Level one name:",
									"",
									$"{data.NameLevelOne}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Level two name:",
									"",
									$"{data.NameLevelTwo}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Id level one :",
									"",
									$"{data.IdLevelOne}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Id level two :",
									"",
									$"{data.IdLevelTwo}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add),
									ObjectLogHelper.getLog(user,response, "ROH Artikel Part number level one :",
									"",
									$"{data.PartNumberLevelOne}",
									Enums.ObjectLogEnums.Objects.ArticleRohNumber.GetDescription(),
									Enums.ObjectLogEnums.LogType.Add)
		}, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
				return ResponseModel<int>.SuccessResponse(response);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}