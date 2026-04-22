using System;

namespace Psz.Core.CRP.Models.FA
{
	public class FAWunchWerkLogModel
	{
		public DateTime? Dateupdate { get; set; }
		public int Id { get; set; }
		public string Typ { get; set; }
		public string userName { get; set; }
		public FAWunchWerkLogModel()
		{

		}
		public FAWunchWerkLogModel(Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_UpdateEntity entity)
		{
			Dateupdate = entity.Dateupdate;
			Id = entity.Id;
			Typ = entity.Typ;
			userName = entity.userName;
		}
	}
}