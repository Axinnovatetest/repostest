using Psz.Core.Apps.Purchase.Models.Analyse;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Analyse
	{
		public static Core.Models.ResponseModel<List<Models.Analyse.CalculateTurnoverResponseModel>> CalculateTurnover(Models.Analyse.CalculateTurnoverRequestModel data,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null /*|| !user.Access.Purchase.ModuleActivated*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return new Core.Models.ResponseModel<List<Models.Analyse.CalculateTurnoverResponseModel>>()
				{
					Success = true,
					Body = CalculateTurnoverInternal(data, user)
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static List<Models.Analyse.CalculateTurnoverResponseModel> CalculateTurnoverInternal(Models.Analyse.CalculateTurnoverRequestModel data,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				var dates = data.GetDates();

				#region > Customers Filter
				var searchCustomerNumbers = new List<int>();

				if(user.Access.Purchase.AllCustomers)
				{
					searchCustomerNumbers = data.CustomersNumbers;
				}
				else
				{
					var userCustomerNumbers = new List<int>();

					var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
					userCustomerNumbers = userCustomers.Select(e => e.CustomerNumber).ToList();

					if(data.CustomersNumbers != null && data.CustomersNumbers.Count > 0)
					{
						searchCustomerNumbers = data.CustomersNumbers.FindAll(e => userCustomerNumbers.Contains(e));
					}
					else
					{
						searchCustomerNumbers = userCustomerNumbers;
					}
				}
				#endregion

				var monthlyCustomersTotals = new List<Tuple<AnalyseTimePointModel.CalculatedDataModel.CalculatedDataItemModel, List<KeyValuePair<int, decimal>>>>();
				foreach(var date in dates.Customs)
				{
					var typs = data.Estimated
						? new List<string>() { Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION }
						: new List<string>() { Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE };
					var customersTotalsItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetKundenNrSumGesamtpreis(typs,
						searchCustomerNumbers,
						true,
						(data.Estimated ? false : (bool?)null),
						(data.Estimated ? false : (bool?)null),
						(data.Estimated ? (DateTime?)null : date.From),
						(data.Estimated ? (DateTime?)null : date.To),
						(data.Estimated ? date.From : (DateTime?)null),
						(data.Estimated ? date.To : (DateTime?)null));

					monthlyCustomersTotals.Add(new Tuple<AnalyseTimePointModel.CalculatedDataModel.CalculatedDataItemModel, List<KeyValuePair<int, decimal>>>(
						date,
						customersTotalsItem));
				}

				var response = new List<Models.Analyse.CalculateTurnoverResponseModel>();

				var customerNumbers = new List<int>();
				monthlyCustomersTotals.ForEach(e => customerNumbers.AddRange(e.Item2.Select(x => x.Key).ToList()));
				customerNumbers = customerNumbers.Distinct().ToList();

				var customers = Apps.EDI.Handlers.Customers.GetByNumbers(customerNumbers);

				foreach(var monthlyCustomersTotal in monthlyCustomersTotals)
				{
					var responseItem = new Models.Analyse.CalculateTurnoverResponseModel()
					{
						TotalTurnover = monthlyCustomersTotal.Item2.Sum(e => e.Value),
						Customers = new List<Models.Analyse.CalculateTurnoverResponseModel.CustomerTurnoverModel>(),
						RequestUnitData = monthlyCustomersTotal.Item1.RequestUnitData,
						RequestUnitType = monthlyCustomersTotal.Item1.RequestUnitType,
						RequestYear = monthlyCustomersTotal.Item1.RequestYear,
						CalculatedStartDate = monthlyCustomersTotal.Item1.From,
						CalculatedEndDate = monthlyCustomersTotal.Item1.To,
					};

					foreach(var customerTotal in monthlyCustomersTotal.Item2)
					{
						var customer = customers?.Body?.Find(e => e.CustomerNumber == customerTotal.Key);
						if(customer == null)
						{
							continue;
						}

						responseItem.Customers.Add(new Models.Analyse.CalculateTurnoverResponseModel.CustomerTurnoverModel()
						{
							CustomerId = customer.Id,
							CustomerName = customer.Name,
							CustomerNumber = customer.CustomerNumber,
							TotalTurnover = customerTotal.Value,
						});
					}

					if(responseItem.Customers.Count > 10)
					{
						responseItem.Customers = shortlistCustomers(responseItem.Customers);
					}

					response.Add(responseItem);
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Analyse.CalculateTurnoverResponseModel.CustomerTurnoverModel> shortlistCustomers(
			List<Models.Analyse.CalculateTurnoverResponseModel.CustomerTurnoverModel> customerTurnoverModel)
		{

			customerTurnoverModel = customerTurnoverModel.OrderByDescending(x => x.TotalTurnover).ToList();
			var newResponse = new List<Models.Analyse.CalculateTurnoverResponseModel.CustomerTurnoverModel> { };

			var others = 0m;
			int ii = 0;
			for(; ii < 9; ii++)
			{
				newResponse.Add(customerTurnoverModel[ii]);
			}

			for(; ii < customerTurnoverModel.Count; ii++)
			{
				others += customerTurnoverModel[ii].TotalTurnover;
			}

			newResponse.Add(new Models.Analyse.CalculateTurnoverResponseModel.CustomerTurnoverModel
			{
				CustomerId = -1,
				CustomerName = "Others",
				CustomerNumber = -1,
				TotalTurnover = others
			});

			return newResponse;
		}

		public static Core.Models.ResponseModel<List<int>> GenerateWeeks(WeekRequestModel request)
		{
			List<int> weeks = new List<int>();

			if(request.Year > 0)
			{
				List<int> weekNumbers = Common.Helpers.DateHelpers.GetWeekNumbersForYear(request.Year);

				if(request.Months.Count > 0)
				{
					foreach(int month in request.Months)
					{
						DateTime startOfMonth = new DateTime(request.Year, month, 1);
						DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
						foreach(int week in weekNumbers)
						{
							DateTime firstDayOfWeek = Common.Helpers.DateHelpers.GetFirstDateOfWeek(request.Year, week);

							if(firstDayOfWeek >= startOfMonth && firstDayOfWeek <= endOfMonth)
							{
								if(!weeks.Contains(week))
								{
									weeks.Add(week);
								}
							}
						}
					}
				}
				else
				{
					weeks = weekNumbers;
				}
			}

			return Core.Models.ResponseModel<List<int>>.SuccessResponse(weeks);
		}

		public class FilterProcessorHandler
		{
			private readonly List<ISpecification<FilterCriteria>> _specifications;

			public FilterProcessorHandler()
			{
				_specifications = new List<ISpecification<FilterCriteria>>
		{
			new FillQuartalsFromMonthsSpecification(),
			new FillMonthsAndQuartalsFromWeeksSpecification(),
			new FillMonthsFromQuartalsSpecification(),
			new FillWeeksFromMonthsSpecification()
		};
			}

			public ResponseModel<FilterProcessorResponseModel> ProcessFilters(FilterCriteria criteria)
			{
				foreach(var spec in _specifications)
				{
					if(spec.IsSatisfiedBy(criteria))
					{
						spec.Apply(criteria);
					}
				}
				var response = new FilterProcessorResponseModel(criteria.Quartals, criteria.Months, criteria.Weeks);



				return ResponseModel<FilterProcessorResponseModel>.SuccessResponse(response);
			}
		}


	}
}
