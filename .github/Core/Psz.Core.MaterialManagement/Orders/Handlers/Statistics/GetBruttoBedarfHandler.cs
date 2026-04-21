using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Globalization;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetBruttoBedarfHandler: IHandle<Identity.Models.UserModel, ResponseModel<BedarfResponseModel>>
	{

		private BedarfRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBruttoBedarfHandler(Identity.Models.UserModel user, BedarfRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<BedarfResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				string fertigungNumber, fertigungLager;
				switch(this._data.Land)
				{
					case 1:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.TN}";
						fertigungLager = $"Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.TN.GetDescription()}";
						break;
					case 2:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.WS}";
						fertigungLager = $"{Common.Enums.ArticleEnums.ArticleProductionPlace.WS.GetDescription()}";
						break;
					case 3:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.AL}";
						fertigungLager = $"Albanien - {Common.Enums.ArticleEnums.ArticleProductionPlace.AL.GetDescription()}";
						break;
					case 4:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.CZ}";
						fertigungLager = $"Eigenfertigung - {Common.Enums.ArticleEnums.ArticleProductionPlace.CZ.GetDescription()}";
						break;
					case 5:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.DE}";
						fertigungLager = $"Fertigung - {Common.Enums.ArticleEnums.ArticleProductionPlace.DE.GetDescription()}";
						break;
					//case 6:
					//	fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.BETN}";
					//	fertigungLager = $"Benane Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.BETN.GetDescription()}";
					//	break;
					case 7:
						fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN}";
						fertigungLager = $"Ghezala Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN.GetDescription()}";
						break;
					default:
						fertigungNumber = "";
						fertigungLager = "";
						break;
				}


				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.ArticleId);
				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.BruttoBedarf.getQueryParamed(this._user.Id, this._data.ArticleId, this._data.ArticleNumber, this._data.Land, fertigungNumber, fertigungLager, Module.ALVirtualBestandArticleIds);
				var BedarfPositive = results?.Item1.Select(x => new BedarfResponseModel.NeedsModel(x))?.ToList();
				var BedarfNegative = results?.Item2.Select(x => new BedarfResponseModel.NeedsModel(x))?.ToList();

				decimal bestand = 0m;
				decimal reserviert = 0m;
				;
				if(BedarfNegative != null && BedarfNegative.Count > 0)
				{
					bestand = BedarfNegative[0].Verfug_Ini ?? 0m;
					reserviert = BedarfNegative[0].Reserviert_Menge ?? 0m;
				}
				else if(BedarfPositive != null && BedarfPositive.Count > 0)
				{
					bestand = BedarfPositive[0].Verfug_Ini ?? 0m;
					reserviert = BedarfPositive[0].Reserviert_Menge ?? 0m;
				}
				var response = new BedarfResponseModel
				{
					DataHeader = new BedarfResponseModel.Header
					{
						Date = DateTime.Now.ToString("dddd, dd MMMM yyyy", new CultureInfo("de-DE")),
						Artikelnummer = _data.ArticleNumber,
						Bezeichung = article.Bezeichnung1.Length > 30 ? article.Bezeichnung1.Substring(0, 30) : article.Bezeichnung1,
						Bestand = bestand,
						Reserviert = reserviert,
						Title = results?.Item5,
					},
					BedarfPositive = results?.Item1.Select(x => new BedarfResponseModel.NeedsModel(x))?.ToList(),
					BedarfNegative = results?.Item2.Select(x => new BedarfResponseModel.NeedsModel(x))?.ToList(),
					Suppliers = results?.Item3.Select(x => new BedarfResponseModel.SupplierModel(x))?.ToList(),
					SubItems = results?.Item4.Select(x => new BedarfResponseModel.BestellungModel(x))?.ToList()
				};

				return ResponseModel<BedarfResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<BedarfResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<BedarfResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<BedarfResponseModel>.SuccessResponse();
		}
		public ResponseModel<byte[]> GetPDF()
		{
			var data = this.Handle();
			var dataEntities = new BedarfResponseModel();
			if(data.Success)
			{
				dataEntities = data.Body;
			}
			var resportData = dataEntities.ToPDFModel();
			var fastreport = new MaterialManagement.Helpers.FastReport(Module.ReportsTemplatePath);
			var response = fastreport.GenerateBruttoBedarfPDF(Enums.StatisticsEnums.ReportType.BRUTTO_BEDARF, resportData);
			return ResponseModel<byte[]>.SuccessResponse(response);
		}

	}
}
