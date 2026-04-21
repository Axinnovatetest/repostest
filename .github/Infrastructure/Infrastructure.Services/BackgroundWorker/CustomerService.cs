using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.BackgroundWorker
{
	public static class CustomerService
	{
		public static async Task WelcomeMadAsync(Email.EmailParamtersModel emailParams)
		{
			var errors = new List<string> { };
			var emailService = new Email.MailKit();
			emailService.InitiateEmailSender(emailParams);

			// -
			await checkOrderPositionsAsync(emailParams, errors);


			// - 
			if(errors.Count > 0)
			{
				await emailService.SendEmailAsync("Order Pos Checks", $"ERRORS:\n{formatList(errors, true)}", new List<string> { emailParams.AdminEmail });
			}
			else
			{
				await emailService.SendEmailAsync("Order Pos Checks", $"No errors found.", new List<string> { emailParams.AdminEmail });
			}
		}
		internal static async Task checkOrderPositionsAsync(Email.EmailParamtersModel emailParams, List<string> error)
		{
			var _minDate = new DateTime(DateTime.Today.Year, 1, 1);
			List<decimal> _vats = new List<decimal> { 0, 0.19m };
			////bool _isDone = false;
			////int _batchSize = 10;
			////var openOrdersCount = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCount(isDone: _isDone);
			////if(openOrdersCount > 0)
			////{
			////    int n = openOrdersCount / _batchSize;
			////    if(openOrdersCount % _batchSize > 0)
			////    {
			////        n++;
			////    }

			////    //- 
			////    for (int i = 0; i < n; i++)
			////    {
			////        var orderEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(isDone: _isDone, new Data.Access.Settings.PaginModel
			////        {
			////            FirstRowNumber = i * _batchSize,
			////            RequestRows = _batchSize
			////        });
			////        if(orderEntities != null && orderEntities.Count > 0)
			////        {
			////            var orderPosEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyAngebote(orderEntities.Select(x => x.Nr).ToList());
			////            foreach (var orderItem in orderEntities)
			////            {
			////                var orderPosItems = orderPosEntities.Where(x => x.AngebotNr == orderItem.Nr)?.ToList();
			////                if(orderPosItems != null && orderPosItems.Count > 0)
			////                {
			////                    decimal vat = orderPosItems[0].USt ?? 0;
			////                    foreach (var posItem in orderPosEntities)
			////                    {
			////                        // -
			////                        if (_vats.Exists(y => y == posItem.USt) == false)
			////                        {
			////                            error.Add($"Pos [{posItem.Position}] in Order [{orderItem.Nr}] has WRONG VAT [{posItem.USt}]");
			////                        }

			////                        // -
			////                        if (posItem.USt != vat)
			////                        {
			////                            error.Add($"Pos [{posItem.Position}] in Order [{orderItem.Nr}] has DIFFERENT VAT [{posItem.USt}] than reminder of Pos.");
			////                        }
			////                    }
			////                }
			////            }
			////        }
			////    }
			////}
			///


			// - 202205-13
			var failedPositions = await Infrastructure.Data.Access.Joins.CTS.BackgroundWorkerAccess.GetPosWrongVatAsync(_vats, _minDate);
			if(failedPositions != null && failedPositions.Count > 0)
			{
				failedPositions = failedPositions?.DistinctBy(x => x.Nr).ToList();
				var orders = failedPositions.Select(x => x.AngebotNr).Distinct().OrderBy(x => x).ToList();
				foreach(var item in orders)
				{
					var posItems = failedPositions.Where(x => x.AngebotNr == item).ToList();
					error.Add($"Order [<a href='http://customer-service{emailParams.AppDomaineName}/#/order-responses/{item}'>{item}</a>]" +
							$"<ol>" +
							string.Join("", posItems.OrderBy(x => x.Position).Select(posItem => $"<li>Pos ({posItem.Nr})[{posItem.Position}] has WRONG VAT [{posItem.USt}]</li>")) +
							$"</ol>");
				}
			}
		}
		static string formatList(List<string> data, bool ordered = false)
		{
			var content = ordered ? "<ol>" : "<ul>";
			// -
			if(data != null && data.Count > 0)
			{
				foreach(var item in data)
				{
					content += $"<li>{item}</li>";
				}
			}

			content += (ordered ? "</ol>" : "</ul>");
			//  - 
			return content;
		}
	}
}
