using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Helpers
{
	public class ItemElementHelper
	{
		public static void UpdateLagerExtension(Core.Identity.Models.UserModel user, UpdateLagerExtensionModel data, Infrastructure.Services.Utils.TransactionsManager sqlTransaction = null)
		{
			UpdateLagerExtension(user, new List<UpdateLagerExtensionModel> { data }, sqlTransaction);

		}
		public static void UpdateLagerExtension(Core.Identity.Models.UserModel user, List<UpdateLagerExtensionModel> dataItems, Infrastructure.Services.Utils.TransactionsManager sqlTransaction = null)
		{
			try
			{
				foreach(var data in dataItems)
				{
					if(data.OldKundenIndex == data.NewKundenIndex)
					{
						//same index
						if(data.OldLagerorId == data.NewLagerorId)
						{
							//same index same lager
							if(data.OldBestand == data.NewBestand)
							{
								//same lager same index same bestand
							}
							else
							{
								//same lager same index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
						else
						{
							//same index diffrent lager
							if(data.OldBestand == data.NewBestand)
							{
								//diffrent lager same index same bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
							else
							{
								//diffrent lager same index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
					}
					else
					{
						//diffrent index
						if(data.OldLagerorId == data.NewLagerorId)
						{
							//diffrent index same lager
							if(data.OldBestand == data.NewBestand)
							{
								//diffrent lager same index same bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
							else
							{
								//diffrent lager same index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
						else
						{
							//diffrent index diffrent lager
							if(data.OldBestand == data.NewBestand)
							{
								//diffrent lager diffrent index same bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
							else
							{
								//diffrent lager diffrent index diffrent bestand
								#region  Entity
								//return old betstand
								var lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.OldKundenIndex, data.OldLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand += data.OldBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = data.OldBestand, // 0,
										Id = -1,
										Index_Kunde = data.OldKundenIndex,
										Lagerort_id = data.OldLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								//substract new betstand
								lagerExtEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(data.ArticleId, data.NewKundenIndex, data.NewLagerorId);
								if(lagerExtEntity != null)
								{
									lagerExtEntity.Bestand -= data.NewBestand;
									lagerExtEntity.LastEditTime = DateTime.Now;
									lagerExtEntity.LastEditUserId = user.Id;
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Update(lagerExtEntity);
								}
								else
								{
									lagerExtEntity = new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
									{
										ArtikelNr = data.ArticleId,
										Bestand = -data.NewBestand,
										Id = -1,
										Index_Kunde = data.NewKundenIndex,
										Lagerort_id = data.NewLagerorId,
										LastEditTime = DateTime.Now,
										LastEditUserId = user.Id,
									};
									Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.Insert(lagerExtEntity);
								}
								#endregion
							}
						}
					}
				}
			} catch(Exception e)
			{

				throw;
			}
		}
		public class UpdateLagerExtensionModel
		{
			public int ArticleId { get; set; }
			public string OldKundenIndex { get; set; }
			public string NewKundenIndex { get; set; }
			public int OldLagerorId { get; set; }
			public int NewLagerorId { get; set; }
			public decimal OldBestand { get; set; }
			public decimal NewBestand { get; set; }
		}
	}
}