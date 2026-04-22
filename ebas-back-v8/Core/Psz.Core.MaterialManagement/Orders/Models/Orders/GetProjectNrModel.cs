namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class GetProjectNrReponseModel
	{
		public string ProjectNr { get; set; }

		public GetProjectNrReponseModel(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity bestellungenEntity)
		{
			ProjectNr = bestellungenEntity.Projekt_Nr;
		}

	}
	public class GetProjectNrRequestModel
	{
		public string Filter { get; set; }
	}
}
