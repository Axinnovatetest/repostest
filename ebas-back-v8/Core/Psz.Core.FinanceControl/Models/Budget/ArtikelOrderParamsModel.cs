namespace Psz.Core.FinanceControl.Models.Budget
{
	public class ArtikelOrderParamsModel
	{



		//param Version Order
		public int? Max_VO { get; set; }
		public int? Nr_version_Order_param { get; set; }
		public int? Id_Level_param { get; set; }
		public int? Id_Status_param { get; set; }
		public int? Id_Dept_param { get; set; }
		public int? Id_Land_param { get; set; }
		public string Dept_name_param { get; set; }
		public string Land_name_param { get; set; }
		public int? Id_Currency_Order_param { get; set; }
		public int? Id_Supplier_VersionOrder_param { get; set; }
		public double? TotalCost_Order_param { get; set; }
		public string Step_Order_param { get; set; }
		public int? Id_Project_param { get; set; }


		public ArtikelOrderParamsModel() { }
		public ArtikelOrderParamsModel(Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity artikelOrderEntity)
		{

			//param Version Order
			Max_VO = artikelOrderEntity.Max_VO;
			Nr_version_Order_param = artikelOrderEntity.Nr_version_Order_param;
			Id_Level_param = artikelOrderEntity.Id_Level_param;
			Id_Status_param = artikelOrderEntity.Id_Status_param;
			Id_Dept_param = artikelOrderEntity.Id_Dept_param;
			Id_Land_param = artikelOrderEntity.Id_Land_param;
			Dept_name_param = artikelOrderEntity.Dept_name_param;
			Land_name_param = artikelOrderEntity.Land_name_param;
			Id_Currency_Order_param = artikelOrderEntity.Id_Currency_Order_param;
			Id_Supplier_VersionOrder_param = artikelOrderEntity.Id_Supplier_VersionOrder_param;
			TotalCost_Order_param = artikelOrderEntity.TotalCost_Order_param;
			Step_Order_param = artikelOrderEntity.Step_Order_param;
			Id_Project_param = artikelOrderEntity.Id_Project_param;

		}

		public Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity ToBudgetSuppLands()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity
			{

				//param Version Order
				Max_VO = Max_VO,
				Nr_version_Order_param = Nr_version_Order_param,
				Id_Level_param = Id_Level_param,
				Id_Status_param = Id_Status_param,
				Id_Dept_param = Id_Dept_param,
				Id_Land_param = Id_Land_param,
				Dept_name_param = Dept_name_param,
				Land_name_param = Land_name_param,
				Id_Currency_Order_param = Id_Currency_Order_param,
				Id_Supplier_VersionOrder_param = Id_Supplier_VersionOrder_param,
				TotalCost_Order_param = TotalCost_Order_param,
				Step_Order_param = Step_Order_param,
				Id_Project_param = Id_Project_param,
			};
		}
	}
}
