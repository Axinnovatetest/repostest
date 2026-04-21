using Infrastructure.Data.Entities.Tables.BSD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ArticleReference;

public class GetArtikelCustomerReferencesModel
{
	public int? ArticleId { get; set; }
	public DateTime? CreateDate { get; set; }
	public int? CreateUser { get; set; }
	public string CreateUserName { get; set; }
	public int? CustomerId { get; set; }
	public string CustomerName { get; set; }
	public int? CustomerNumber { get; set; }
	public string CustomerReference { get; set; }
	public DateTime? EditDate { get; set; }
	public int? EditUser { get; set; }
	public string EditUserName { get; set; }
	public int Id { get; set; }
	public GetArtikelCustomerReferencesModel(ArtikelCustomerReferencesEntity data)
	{
		ArticleId = data.ArticleId;
		CreateDate = data.CreateDate;
		CreateUser = data.CreateUser;
		CreateUserName = data.CreateUserName;
		CustomerName = data.CustomerName;
		CustomerId = data.CustomerId;
		CustomerNumber = data.CustomerNumber;
		CustomerReference = data.CustomerReference;
		EditDate = data.EditDate;
		EditUser = data.EditUser;
		EditUserName = data.EditUserName;
		Id = data.Id;
	}
}
