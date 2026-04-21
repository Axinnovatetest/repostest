namespace Psz.Core.MaterialManagement.Models
{
	public class RahmenListRequestModel: IPaginatedRequestModel
	{
		public int? ArtikelNr { get; set; }
		public string DocumentNumber { get; set; }
		public string PrefailNumber { get; set; }
		public string ProjectNumber { get; set; }
		public List<int> SuppliersIds { get; set; }
		public bool OnlyExpired { get; set; }
		public int? Status { get; set; }
	}

	public class RahmenListResponseModel: IPaginatedResponseModel<RahmenListModel>
	{

	}
	public class RahmenListModel
	{
		public int Nr { get; set; }
		public string ProjectNumber { get; set; }
		public string DocumentNumber { get; set; }
		public int? PrefailNumber { get; set; }
		public string Supplier { get; set; }
		public DateTime? CreateDate { get; set; }
		public DateTime? DueDate { get; set; }
		public string State { get; set; }
		public int? StateId { get; set; }
		public RahmenListModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity entity)
		{
			var extension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(entity.Nr);
			Nr = entity.Nr;
			ProjectNumber = entity.Projekt_Nr;
			DocumentNumber = entity.Bezug;
			PrefailNumber = entity.Angebot_Nr;
			Supplier = entity.Vorname_NameFirma;
			CreateDate = entity.Datum;
			DueDate = entity.Falligkeit;
			StateId = extension.StatusId;
			State = ((Enums.OrderEnums.RAStatus)extension.StatusId).GetDescription();
		}
	}
}
