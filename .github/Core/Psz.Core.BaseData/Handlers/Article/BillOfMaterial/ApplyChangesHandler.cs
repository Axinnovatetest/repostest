using Infrastructure.Services.Utils;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class ApplyChangesHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.BillOfMaterial.ImportRequestModel _data { get; set; }
		public ApplyChangesHandler(UserModel user, Models.Article.BillOfMaterial.ImportRequestModel positions)
		{
			this._user = user;
			this._data = positions;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				#region // -- transaction-based logic -- //

				if(this._data.Data != null && this._data.Data.Count > 0)
				{
					botransaction.beginTransaction();

					lock((Locks.ArticleEditLock.GetOrAdd(this._data.Data[0].ArticleParentId, new object())))
					{
						var validationResponse = this.Validate();
						if(!validationResponse.Success)
						{
							return validationResponse;
						}

						var _positions = this._data.Data;
						reorderPositionNumbers(_positions);
						var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(_positions.Select(x => x.ArticleId)?.ToList(), botransaction.connection, botransaction.transaction)
							?.Where(x => x.aktiv != false)?.ToList();
						var positionEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetWithTransaction(_positions.Select(x => x.Id)?.ToList(), botransaction.connection, botransaction.transaction);

						// -
						var stcklistenCalcul_before_entity = Infrastructure.Data.Access.Tables.BSD.Stucklisten_costenAccess.Get(this._data.Data[0].ArticleParentId, botransaction.connection, botransaction.transaction);
						var stcklistenCalcul_before = new Models.Article.BillOfMaterial.StucklistenCostenModel(stcklistenCalcul_before_entity);
						//
						List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>> _changes = new List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>>();
						if(_positions != null)
						{
							foreach(var pos in _positions)
							{
								var posEntity = positionEntities.FirstOrDefault(x => x.Nr == pos.Id);
								int newPositionId = -1;
								// parents positions
								if(pos.CRUDState.HasValue && pos.CRUDState == 1)//update
								{
									var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == pos.ArticleId);
									pos.ArticleNumber = article.ArtikelNummer;
									pos.ArticleDesignation = article.Bezeichnung1;
									//
									_changes.AddRange(PrepareListChanges(pos, botransaction));
									Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.UpdateWithTransaction(pos.ToEntity(posEntity), botransaction.connection, botransaction.transaction);

								}
								if(pos.CRUDState.HasValue && pos.CRUDState == 0)//add
								{
									var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(pos.ArticleParentId, botransaction.connection, botransaction.transaction);
									var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == pos.ArticleId);
									pos.ArticleNumber = article.ArtikelNummer;
									pos.ArticleDesignation = article.Bezeichnung1;
									pos.ArticleId = article.ArtikelNr;
									//
									var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, pos.Id, 0, parentArticle.ArtikelNummer, null, pos.Quantity.ToString(),
									null, pos.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.Add);
									Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
									_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("new", log));
									newPositionId = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.InsertWithTransaction(pos.ToEntity(null), botransaction.connection, botransaction.transaction);
								}
								if(pos.CRUDState.HasValue && pos.CRUDState == 2)//delete
								{
									var bom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetWithTransaction(pos.Id, botransaction.connection, botransaction.transaction);
									var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction((int)bom?.Artikel_Nr, botransaction.connection, botransaction.transaction);
									var altPositions = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBom(pos.Id, botransaction.connection, botransaction.transaction);
									//
									var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, pos.Id, 0, parentArticle.ArtikelNummer, null, bom.Anzahl?.ToString("0.000000000000"),
									null, bom.Artikelnummer, Enums.ObjectLogEnums.BOMLogType.Delete);
									Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
									_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("delete", log));
									Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.DeleteWithTransaction(pos.Id, botransaction.connection, botransaction.transaction);

									//delete alt positions if exsists
									if(altPositions != null && altPositions.Count > 0)
									{
										//delete alt attachement if exsist
										if(pos.DocumentId.HasValue)
											altPositions.ForEach(x => Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)x.DocumentId, botransaction));

										Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.DeleteWithTransaction(altPositions.Select(x => x.Nr)?.ToList(), botransaction.connection, botransaction.transaction);
									}
									//delete attachement if exsist
									if(pos.DocumentId.HasValue)
										Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)pos.DocumentId, botransaction);

								}

								// alternatives positions
								if(pos.BomPositionsAlt != null && pos.BomPositionsAlt.Count > 0)
								{
									if(pos.CRUDState == 0) // - New Parent Position
									{
										foreach(var alt in pos.BomPositionsAlt)
										{
											if(alt.CRUDState.HasValue && (alt.CRUDState == 0 || alt.CRUDState == 1)) // add or edit alt
											{
												alt.OriginalPositionId = newPositionId;
												var originalPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetWithTransaction(alt.OriginalPositionId, botransaction.connection, botransaction.transaction);
												var maxPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetMaxPositionByOriginalBom(alt.OriginalPositionId, botransaction.connection, botransaction.transaction);
												var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction((int)originalPosition.Artikel_Nr, botransaction.connection, botransaction.transaction);
												// Supposed to come without article designation and number
												var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumberWithTransaction(alt.ArticleNumber, null, botransaction.connection, botransaction.transaction);
												alt.ArticleNumber = article.ArtikelNummer;
												alt.ArticleDesignation = article.Bezeichnung1;
												alt.Position = (maxPosition + 10).ToString("D3");
												alt.ParentArticleId = parentArticle.ArtikelNr;
												//
												var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, alt.OriginalPositionId, 1, parentArticle.ArtikelNummer, null, alt.Quantity.ToString(),
												null, alt.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.Add);
												Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
												_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("newAlt", log));
												Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.InsertWithTransaction(alt.ToEntity(null), botransaction.connection, botransaction.transaction);
											}
										}
									}
									else if(pos.CRUDState == 1)
									{
										foreach(var alt in pos.BomPositionsAlt)
										{
											var altPosEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetWithTransaction(alt.Id, botransaction.connection, botransaction.transaction);
											if(alt.CRUDState.HasValue && alt.CRUDState == 0)//add alt
											{
												var originalPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetWithTransaction(pos.Id, botransaction.connection, botransaction.transaction);
												var maxPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetMaxPositionByOriginalBom(alt.OriginalPositionId, botransaction.connection, botransaction.transaction);
												var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction((int)originalPosition.Artikel_Nr, botransaction.connection, botransaction.transaction);
												// Supposed to come without article designation and number
												var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumberWithTransaction(alt.ArticleNumber, null, botransaction.connection, botransaction.transaction);
												alt.ArticleNumber = article.ArtikelNummer;
												alt.ArticleDesignation = article.Bezeichnung1;
												alt.Position = (maxPosition + 10).ToString("D3");
												alt.ParentArticleId = parentArticle.ArtikelNr;
												//
												var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, alt.OriginalPositionId, 1, parentArticle.ArtikelNummer, null, alt.Quantity.ToString(),
												null, alt.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.Add);
												Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
												_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("newAlt", log));
												Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.InsertWithTransaction(alt.ToEntity(null), botransaction.connection, botransaction.transaction);
											}
											if(alt.CRUDState.HasValue && alt.CRUDState == 1)//update alt
											{
												_changes.AddRange(PrepareListChangesAlt(alt, botransaction));
												Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.UpdateWithTransaction(alt.ToEntity(altPosEntity), botransaction.connection, botransaction.transaction);
											}
											if(alt.CRUDState.HasValue && alt.CRUDState == 2)//delete alt
											{
												var positionAlt = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetWithTransaction(alt.Id, botransaction.connection, botransaction.transaction);
												var originalPosition = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetWithTransaction(positionAlt.OriginalStucklistenNr, botransaction.connection, botransaction.transaction);
												var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction((int)originalPosition.Artikel_Nr, botransaction.connection, botransaction.transaction);
												//
												var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, alt.Id, 1, parentArticle.ArtikelNummer, null, positionAlt.Anzahl?.ToString("0.000000000000"),
												null, positionAlt.ArtikelNummer, Enums.ObjectLogEnums.BOMLogType.Delete);
												_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("deleteAlt", log));
												Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
												Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.DeleteWithTransaction(alt.Id, botransaction.connection, botransaction.transaction);
												//delete attachement if exsist
												if(alt.DocumentId.HasValue)
													Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)alt.DocumentId, botransaction);
											}
										}
									}
								}
							}
						}
						//mail handeling
						if(_changes != null && _changes.Count > 0)
						{
							var orinalPosChanges = _changes.Where(x => x.Key == "qty_change" || x.Key == "article_change" || x.Key == "delete" || x.Key == "new").ToList();
							var altPosChanges = _changes.Where(x => x.Key == "article_changeAlt" || x.Key == "qty_changeAlt" || x.Key == "deleteAlt" || x.Key == "newAlt").ToList();
							//
							var stcklistenCalcul_after_entity = Infrastructure.Data.Access.Tables.BSD.Stucklisten_costenAccess.Get(this._data.Data[0].ArticleParentId, botransaction.connection, botransaction.transaction);
							var stcklistenCalcul_after = new Models.Article.BillOfMaterial.StucklistenCostenModel(stcklistenCalcul_after_entity);
							var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data.Data[0].ArticleParentId, botransaction.connection, botransaction.transaction);
							string emailTitle = $"[BOM Update] Change Log – Approval Status: {parentArticle.Freigabestatus}", emailContent;
							emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif; max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
							emailContent += $"<span style='font-size:1.5em'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")}</span><br/>";
							emailContent += $"<br/><span style='font-size:1.15em'><strong>{this._user.Name?.ToUpper()}</strong> just made {_changes.Count} change(s) in the BOM list of the Article [{parentArticle.ArtikelNummer}]</span></br></br>";
							emailContent += $"<hr>";
							if(orinalPosChanges != null && orinalPosChanges.Count > 0)
							{
								var firstFiveItems = (orinalPosChanges.Count <= 5) ? orinalPosChanges : orinalPosChanges.Take(5);
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> Material costs before changes :{stcklistenCalcul_before.Materiel_cost}</span></br>";
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> Material costs after changes :{stcklistenCalcul_after.Materiel_cost}</span></br>";
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> DB before changes:{stcklistenCalcul_before.DB}</span></br>";
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> DB after changes :{stcklistenCalcul_after.DB}</span></br>";
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> Percent before changes :{stcklistenCalcul_before.Prozent}%</span></br>";
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> Percent after changes :{stcklistenCalcul_after.Prozent}%</span></br>";
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> Freigabestatus :{parentArticle.Freigabestatus}</span></br>";
								emailContent += $"<br/><span style='font-size:1.em;font-weight:bold'> Date/Time :{DateTime.Now}</span></br>";
								emailContent += $"<hr>";
								emailContent += $"<br/><span style='font-size:1.5em;font-weight:bold'>CHANGES IN MAIN POSITIONS</span></br>";
								foreach(var item in firstFiveItems)
								{
									if(item.Key == "article_change")
									{
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>BOM Article :{item.Value.Stück_Artikelnummer_Aktuell}</span><span style='font-weight:bold;color:coral'> CHANGED</span></br>";
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>Article Number Before :{item.Value.Stück_Artikelnummer_Voränderung}</span></br>";
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>Article Number After :{item.Value.Stück_Artikelnummer_Voränderung}</span></br></br>";
										emailContent += "------------------------------";
									}
									if(item.Key == "qty_change")
									{
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>BOM Article :{item.Value.Stück_Artikelnummer_Aktuell}</span><span style='font-weight:bold;color:coral'> CHANGED</span></br>";
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>Quantity Before :{item.Value.Alter_menge}</span></br>";
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>Quantity After :{item.Value.Neuer_menge}</span></br></br>";
										emailContent += "------------------------------";
									}
									if(item.Key == "new")
									{
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>BOM Article :{item.Value.Stück_Artikelnummer_Aktuell}</span><span style='font-weight:bold;color:green'> NEW</span></br>";
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>Quantity :{item.Value.Neuer_menge}</span></br></br>";
										emailContent += "------------------------------";
									}
									if(item.Key == "delete")
									{
										emailContent += $"<br/><span style='font-size:1em;font-weight:bold'>BOM Article :{item.Value.Stück_Artikelnummer_Aktuell}</span><span style='font-weight:bold;color:red'> DELETED</span></br></br>";
										emailContent += "------------------------------";
									}
								}
							}
							emailContent += $"<hr>";
							if(altPosChanges != null && altPosChanges.Count > 0)
							{
								emailContent += $"<br/><span style='font-size:1.5em;font-weight:bold'>CHANGES IN ALTERNATIVE POSITIONS</span></br>";
								emailContent += "<ul>";
								foreach(var item in altPosChanges)
								{
									emailContent += $"<li>{item.Value.Status}</li>";
								}
								emailContent += "</ul>";
							}
							if(orinalPosChanges.Count >= 5)
								emailContent += "<br/><br/><span style='font-size:1em;font-weight:bold;color:red'>OTHER CHANGES HAVE BEEN MADE, PLEASE CHECK ATTACHED EXCEL TO SEE ALL CHANGED ITEMS</span>";
							emailContent += "<br/>All the above change(s) has been applyed and logged.</div>/</br>Regards.";

							//getting mail destinations from article site
							List<string> _adresses = new List<string>();

							_adresses.AddRange(getEmailUserByArticleProductionSite(this._data.Data[0].ArticleParentId, parentArticle.ArtikelNummer, botransaction));

							//adding article manager if exsist
							var managerEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetSingleByArtikelNr(this._data.Data[0].ArticleParentId, botransaction.connection, botransaction.transaction);
							if(managerEntity != null)
							{
								var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(managerEntity.UserId, botransaction.connection, botransaction.transaction);
								if(userEntity != null && userEntity.IsArchived == false)
								{
									if(!_adresses.Contains(userEntity.Email))
										_adresses.Add(userEntity.Email);
								}
							}
							try
							{
								string excelFilePath = null;
								// Send email notification
								if(orinalPosChanges.Count > 5)
								{
									var excel = new Psz.Core.BaseData.Helpers.CreateBomChangesExcel(parentArticle.ArtikelNummer, _changes, this._user);
									excelFilePath = excel.CreateExcel();
								}
								Module.EmailingService.SendBomEmailAsync(0, emailTitle, emailContent, _adresses, excelFilePath, null);
								if(orinalPosChanges.Count > 5)
									File.Delete(excelFilePath);
							} catch(Exception ex)
							{
								Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", Module.EmailingService.EmailParamtersModel.BOMEmailDestinations)}]"));
								Infrastructure.Services.Logging.Logger.Log(ex);
								//throw new Exception($"Unable to send email to [{string.Join(", ", Module.EmailingService.EmailParamtersModel.BOMEmailDestinations)}]");
							}
						}


						// - set Bom in Prep & Increment Version if it was VALIDATED
						var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.Data[0].ArticleParentId, botransaction.connection, botransaction.transaction);
						if(articleExtEntity.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved)
						{
							articleExtEntity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation;
							articleExtEntity.BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription();
							// - 2022-07-13 - upgrade BOM only if user choses to - 2022-07-25 - deprecated
							// if (this._data.UpgradeBomVersion)
							{
								articleExtEntity.BomVersion = articleExtEntity.BomVersion + 1;
							}
							Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.UpdateWithTransaction(articleExtEntity, botransaction.connection, botransaction.transaction);

							// -- Article level Logging
							Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
								new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
                                    // - Status change log
                                    ObjectLogHelper.getLog(this._user, articleExtEntity.ArticleId, "Article BOM Status from", $"{Enums.ArticleEnums.BomStatus.Approved.GetDescription()}",
									$"{Enums.ArticleEnums.BomStatus.InPreparation.GetDescription()}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit),
                                    // - Version change log
                                    ObjectLogHelper.getLog(this._user, articleExtEntity.ArticleId, "Article BOM Version from", $"{((articleExtEntity.BomVersion??0)-1)}",
									$"{articleExtEntity.BomVersion}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit)
								}, botransaction.connection, botransaction.transaction);

							// -- BOM level logging
							Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(
								new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> {
                                    // - Status change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									Enums.ArticleEnums.BomStatus.Approved.GetDescription(), Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(), Enums.ObjectLogEnums.BOMLogType.StatusChange),
                                    // - Version change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									((articleExtEntity.BomVersion??0)-1).ToString(), articleExtEntity.BomVersion?.ToString(), Enums.ObjectLogEnums.BOMLogType.Version)
								}, botransaction.connection, botransaction.transaction);
						}

						// - update Article Grosse
						var parentArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data.Data[0].ArticleParentId, botransaction.connection, botransaction.transaction);
						if(parentArticleEntity != null && parentArticleEntity.Stuckliste == true && parentArticleEntity.aktiv == true)
						{
							var newGroesse = 0m;
							positionEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(parentArticleEntity.ArtikelNr, botransaction.connection, botransaction.transaction);
							if(positionEntities != null && positionEntities.Count > 0)
							{
								var positionArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(positionEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1).ToList(), botransaction.connection, botransaction.transaction);
								for(int i = 0; i < positionEntities.Count; i++)
								{
									var posArticle = positionArticles.Find(x => x.ArtikelNr == positionEntities[i].Artikel_Nr_des_Bauteils);
									newGroesse += ((decimal?)positionEntities[i].Anzahl * posArticle?.Größe) ?? 0;
								}
							}

							//-
							Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.updateGrosse(parentArticleEntity.ArtikelNr, newGroesse, botransaction.connection, botransaction.transaction);
						}

						// - 2022-03-30
						CreateHandler.generateFileDAT(this._data.Data[0].ArticleParentId, false, botransaction);


						//TODO: handle transaction state (success or failure)
						if(botransaction.commit())
						{
							// -
							return ResponseModel<int>.SuccessResponse(1);
						}
						else
						{
							return ResponseModel<int>.FailureResponse("Transaction error");
						}
					}
				}
				else
				{
					return ResponseModel<int>.SuccessResponse();
				}

				#endregion // -- transaction-based logic -- //

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(this._data.Data == null || this._data.Data.Count <= 0)
			{
				return ResponseModel<int>.FailureResponse("No data found");
			}
			var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Data[0].ArticleParentId);
			if(parentArticle == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}

			// - no negative quantity
			var errors = new List<string>();
			var workingData = this._data.Data.Where(x => x.CRUDState != 2)?.ToList();
			if(workingData == null || workingData.Count <= 0)
			{
				return ResponseModel<int>.FailureResponse("No change detected");
			}
			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(workingData.Select(x => x.ArticleId)?.ToList());
			foreach(var item in workingData)
			{
				if(articleEntities.FindIndex(x => x.ArtikelNr == item.ArticleId) < 0)
				{
					errors.Add($"Pos [{item.ArticleNumber}]: not found");
				}
			}

			workingData.Where(x => x.CRUDState != 2 && (!x.Quantity.HasValue || x.Quantity <= 0))?.ToList().ForEach(x => errors.Add($"Pos [{x.ArticleNumber}]: Invalid quantity"));
			if(errors.Count > 0)
				return ResponseModel<int>.FailureResponse(errors);

			if(this._data != null && this._data.Data.Count > 0)
			{
				var articleNumbers = new List<string>();

				workingData?.ForEach(x =>
					{
						articleNumbers.Add(x.ArticleNumber);
						if(x.BomPositionsAlt != null && x.BomPositionsAlt.Count > 0)
						{
							x.BomPositionsAlt.Where(y => y.CRUDState != 2)?.ToList().ForEach(y => articleNumbers.Add(y.ArticleNumber));
						}
					});

				var duplicateNumbers = articleNumbers.GroupBy(x => x)
						.Where(group => group.Count() > 1)
						?.Select(group => group.Key);
				if(duplicateNumbers != null && duplicateNumbers.Count() > 0)
					return ResponseModel<int>.FailureResponse(duplicateNumbers.Select(x => $"{x} is duplicated")?.ToList());
			}

			if(this._user.IsGlobalDirector != true && this._user.SuperAdministrator != true && parentArticle.IsEDrawing == true && this._user.Access.MasterData.EDrawingEdit != true)
			{
				return ResponseModel<int>.FailureResponse($"Edit aborted: User cannot edit this article because E-Drawing is activated.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
		public List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>> PrepareListChanges(Models.Article.BillOfMaterial.BomPosition _position, TransactionsManager botransaction)
		{
			List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>> _changes = new List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>>();
			var stucklistEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetWithTransaction(_position.Id, botransaction.connection, botransaction.transaction);
			var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(_position.ArticleParentId, botransaction.connection, botransaction.transaction);
			if(stucklistEntity.Artikel_Nr_des_Bauteils != _position.ArticleId || stucklistEntity.Artikelnummer != _position.ArticleNumber)
			{
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, _position.Id, 0, parentArticle.ArtikelNummer, stucklistEntity.Anzahl?.ToString("0.000000000000"), null,
				stucklistEntity.Artikelnummer, _position.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.EditArt);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				//
				_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("article_change", log));
			}

			if(decimal.Parse((stucklistEntity.Anzahl ?? -1).ToString("0.000000000000")) != _position.Quantity)
			{
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, _position.Id, 0, parentArticle.ArtikelNummer, stucklistEntity.Anzahl?.ToString("0.000000000000"), _position.Quantity.ToString(),
				stucklistEntity.Artikelnummer, stucklistEntity.Artikelnummer, Enums.ObjectLogEnums.BOMLogType.EditQty);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				//
				_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("qty_change", log));
			}
			return _changes;
		}
		public List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>> PrepareListChangesAlt(Models.Article.BillOfMaterial.BomPositionAlt _altPosition, TransactionsManager botransaction)
		{
			List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>> _changes = new List<KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>>();
			var stucklistEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetWithTransaction(_altPosition.Id, botransaction.connection, botransaction.transaction);
			var parentArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(_altPosition.ParentArticleId, botransaction.connection, botransaction.transaction);
			if(stucklistEntity.ArtikelNr != _altPosition.ArticleId || stucklistEntity.ArtikelNummer != _altPosition.ArticleNumber)
			{
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, _altPosition.Id, 1, parentArticle.ArtikelNummer, stucklistEntity.Anzahl?.ToString("0.000000000000"), null,
				stucklistEntity.ArtikelNummer, _altPosition.ArticleNumber, Enums.ObjectLogEnums.BOMLogType.EditArt);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("article_changeAlt", log));
			}
			if(decimal.Parse((stucklistEntity.Anzahl ?? -1).ToString("0.000000000000")) != _altPosition.Quantity)
			{
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, _altPosition.Id, 1, parentArticle.ArtikelNummer, stucklistEntity.Anzahl.ToString(), _altPosition.Quantity.ToString(),
				_altPosition.ArticleNumber, stucklistEntity.ArtikelNummer, Enums.ObjectLogEnums.BOMLogType.EditQty);
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				_changes.Add(new KeyValuePair<string, Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>("qty_changeAlt", log));
			}
			return _changes;
		}
		public static string GetProductionSiteById(int placeID)
		{
			if(/*placeID == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.BETN ||*/ placeID == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.TN || placeID == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.WS || placeID == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN)
				return "TN";
			else if(placeID == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.AL)
				return "AL";
			else if(placeID == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.DE)
				return "DE";
			else if(placeID == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.CZ)
				return "CZ";
			else
				return null;
		}
		public static string GetProductionSiteArticle(string article)
		{
			if(string.IsNullOrWhiteSpace(article))
				return "CZ";

			string suffix = article.Trim().ToUpper().Substring(article.Length - 2);
			if(suffix == "TN")
				return "TN";
			else if(suffix == "AL")
				return "AL";
			else if(suffix == "DE")
				return "DE";
			else
				return "CZ";
		}
		private void reorderPositionNumbers(List<Models.Article.BillOfMaterial.BomPosition> positions)
		{
			int pos = 10;
			positions = positions.OrderBy(x => x.Position).ToList();
			for(int i = 0; i < positions.Count; i++)
			{
				if(positions[i].CRUDState != 2)
				{
					// - set it for update
					if(positions[i].CRUDState != 0 && positions[i].CRUDState != 1)
					{
						positions[i].CRUDState = 1;
					}
					positions[i].Position = pos.ToString("D3");
					pos += 10;

					// - alt
					if(positions[i].BomPositionsAlt != null && positions[i].BomPositionsAlt.Count > 0)
					{
						int altpos = 10;
						for(int j = 0; j < positions[i].BomPositionsAlt.Count; j++)
						{
							if(positions[i].BomPositionsAlt[j].CRUDState != 2)
							{
								// - set it for update
								if(positions[i].BomPositionsAlt[j].CRUDState != 0 && positions[i].BomPositionsAlt[j].CRUDState != 1)
								{
									positions[i].BomPositionsAlt[j].CRUDState = 1;
								}
								positions[i].BomPositionsAlt[j].Position = altpos.ToString("D3");
								altpos += 10;
							}
						}
					}
				}
			}
		}

		public static List<string> getEmailUserByArticleProductionSite(int articleId, string articleNumber, TransactionsManager botransaction = null)
		{
			var shared = true;
			if(botransaction == null)
			{
				shared = false;
				botransaction = new TransactionsManager();
				botransaction.beginTransaction();
			}

			try
			{
				string site = string.Empty;
				var parentArticleProduction = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(articleId, botransaction.connection, botransaction.transaction);
				if(parentArticleProduction != null && parentArticleProduction.ProductionPlace1_Id.HasValue)
					site = GetProductionSiteById(parentArticleProduction.ProductionPlace1_Id.Value);
				else
					site = GetProductionSiteArticle(articleNumber);

				var mailsEntity = Infrastructure.Data.Access.Tables.BSD.BomMailUsersAccess.GetBySite(site, botransaction.connection, botransaction.transaction);
				if(!shared)
				{
					if(botransaction.commit())
					{
						if(mailsEntity != null)
							return mailsEntity.Select(x => x.UserEmail).ToList();
					}
					else
					{
						return new List<string>();
					}
				}
				else
				{
					if(mailsEntity != null)
						return mailsEntity.Select(x => x.UserEmail).ToList();
				}

				if(mailsEntity != null)
					return mailsEntity.Select(x => x.UserEmail).ToList();

			} catch(Exception e)
			{
				throw;
			}


			return new List<string>();
		}
	}
}
