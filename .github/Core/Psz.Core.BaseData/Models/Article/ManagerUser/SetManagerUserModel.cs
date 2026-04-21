using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.ManagerUser
{
	public class SetManagerUserModel
	{
		public int ArtikelNr { get; set; }
		public List<int> UserIds { get; set; }
		public SetManagerUserModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity artikelManagerUserEntity)
		{
		}
	}
}
