using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Tools
{
	public class BillCalculator
	{
		public static Response Calculate(List<Element> elements,
			List<Tax> taxs = null,
			decimal withheldPercentageValue = 0)
		{
			#region Check & Fix params
			elements = elements ?? new List<Element>();
			taxs = taxs ?? new List<Tax>();

			if(elements.Count == 0 && taxs.Count == 0)
			{
				throw new Exception("No inputs");
			}

			foreach(var item in elements)
			{
				if(item == null)
				{
					throw new Exception("Item empty");
				}

				if(item.Quantity < 0)
				{
					throw new Exception("Negative Quantity");
				}

				if(item.SubUnitPrice < 0)
				{
					throw new Exception("Negative UnitPrice");
				}

				if(item.DiscountPercentage < 0 || item.DiscountPercentage > 100)
				{
					throw new IndexOutOfRangeException("DiscountPercentage is out of range");
				}

				if(item.VatPercentage < 0 || item.VatPercentage > 100)
				{
					throw new IndexOutOfRangeException("VatPercentage is out of range");
				}
			}

			foreach(var tax in taxs)
			{
				if(tax.Value <= 0)
				{
					throw new Exception("Wrong tax value");
				}
			}
			#endregion

			var response = new Response();

			var responseTaxsElementBased = taxs.FindAll(e => !e.IsDocumentBased && e.Type != TaxTypes.FixedAmount); // can't be fixed amount
			var responseTaxsDocumentBased = taxs.FindAll(e => e.IsDocumentBased);

			response.Elements = new List<Response.Element>();
			response.Vats = new List<Response.Vat>();

			foreach(var inputElement in elements)
			{
				var element = new Response.Element();

				element.Reference = inputElement.Reference;
				element.SubUnitPrice = inputElement.SubUnitPrice;
				element.Quantity = inputElement.Quantity;
				element.DiscountPercentage = inputElement.DiscountPercentage;
				element.VatPercentage = inputElement.VatPercentage;
				element.CustomTaxs = inputElement.CustomTaxs;

				#region > Sub Total Amount (Before Discount)
				element.SubTotalAmountBeforeDiscount = element.Quantity > 0
					? (element.SubUnitPrice * element.Quantity)
					: 0;
				#endregion

				#region > Discount
				element.DiscountAmount = element.SubTotalAmountBeforeDiscount * element.DiscountPercentage / 100;
				element.SubTotalAmount = element.SubTotalAmountBeforeDiscount - element.DiscountAmount;
				#endregion

				#region > Element Taxs 
				if(element.CustomTaxs != null)
				{
					foreach(var tax in element.CustomTaxs)
					{
						var taxAmount = element.SubTotalAmount * tax.Value / 100;

						element.ElementTaxsAmount += taxAmount;

						var responseTax = response.ElementsTaxs.Find(e => e.Reference == tax.Reference);
						if(responseTax == null)
						{
							response.ElementsTaxs.Add(new Response.Tax()
							{
								Reference = tax.Reference,
								Amount = taxAmount,
								IsDocumentBased = tax.IsDocumentBased,
								Name = tax.Name,
								Type = tax.Type,
								Value = tax.Value,
							});
						}
						else
						{
							responseTax.Amount += taxAmount;
						}
					}
				}

				foreach(var tax in responseTaxsElementBased)
				{
					var taxAmount = element.SubTotalAmount * tax.Value / 100;

					element.ElementTaxsAmount += taxAmount;

					var responseTax = response.ElementsTaxs.Find(e => e.Reference == tax.Reference);
					if(responseTax == null)
					{
						response.ElementsTaxs.Add(new Response.Tax()
						{
							Reference = tax.Reference,
							Amount = taxAmount,
							IsDocumentBased = tax.IsDocumentBased,
							Name = tax.Name,
							Type = tax.Type,
							Value = tax.Value,
						});
					}
					else
					{
						responseTax.Amount += taxAmount;
					}
				}

				element.AmountAfterElementTaxsApplication = element.SubTotalAmount + element.ElementTaxsAmount;
				#endregion

				#region > VAT
				var vatValue = element.VatPercentage;
				var vatBase = element.AmountAfterElementTaxsApplication;
				var vatAmount = vatBase * (vatValue / 100);

				element.VatAmount = vatAmount;
				element.AmountAfterVatApplication = element.AmountAfterElementTaxsApplication + vatAmount;

				var vat = response.Vats.Find(e => e.Value == element.VatPercentage);
				if(vat == null)
				{
					response.Vats.Add(new Response.Vat
					{
						Value = vatValue,
						Amount = vatAmount,
						Base = vatBase,
						Name = vatValue.ToString("G29") + "%"
					});
				}
				else
				{
					vat.Base += vatBase;
					vat.Amount += vatAmount;
				}
				#endregion

				#region > Document-Based Taxs
				foreach(var tax in responseTaxsDocumentBased)
				{
					var taxAmount = tax.Type == TaxTypes.FixedAmount
						? tax.Value
						: element.AmountAfterVatApplication * tax.Value / 100;

					var responseTax = response.DocumentTaxs.Find(e => e.Reference == tax.Reference);
					if(responseTax == null)
					{
						element.DocumentTaxsAmount += taxAmount;

						response.DocumentTaxs.Add(new Response.Tax()
						{
							Reference = tax.Reference,
							Amount = taxAmount,
							IsDocumentBased = tax.IsDocumentBased,
							Name = tax.Name,
							Type = tax.Type,
							Value = tax.Value,
						});
					}
					else if(tax.Type != TaxTypes.FixedAmount)
					{
						element.DocumentTaxsAmount += taxAmount;
						responseTax.Amount += taxAmount;
					}
				}
				#endregion

				// > Total
				element.TotalAmount = element.AmountAfterVatApplication + element.DocumentTaxsAmount;

				response.Elements.Add(element);
			}

			response.SubTotalAmountBeforeDiscount = response.Elements.Sum(e => e.SubTotalAmountBeforeDiscount);
			response.DiscountAmount = response.Elements.Sum(e => e.DiscountAmount);

			response.SubTotalAmount = response.Elements.Sum(e => e.SubTotalAmount);

			response.ElementsTaxsAmount = response.Elements.Sum(e => e.ElementTaxsAmount);
			response.AmountAfterTaxsApplication = response.Elements.Sum(e => e.AmountAfterElementTaxsApplication);

			response.VatAmount = response.Elements.Sum(e => e.VatAmount);
			response.AmountAfterVatApplication = response.AmountAfterTaxsApplication + response.VatAmount;

			response.DocumentTaxsAmount = 0;
			foreach(var tax in responseTaxsDocumentBased)
			{
				var taxAmount = tax.Type == TaxTypes.FixedAmount
					? tax.Value
					: response.AmountAfterVatApplication * tax.Value / 100;

				response.DocumentTaxsAmount += taxAmount;
			}

			response.TotalAmount = response.AmountAfterVatApplication + response.DocumentTaxsAmount;

			response.WithheldAtSourceAmount = 0;
			if(withheldPercentageValue > 0)
			{
				response.WithheldAtSourceAmount = response.TotalAmount - (response.TotalAmount * (response.WithheldAtSourceAmount / 100));
			}

			return response;
		}

		public class Response
		{
			public List<Response.Element> Elements { get; set; } = new List<Element>();

			public decimal SubTotalAmountBeforeDiscount { get; set; } // << Sub Total Before Discount
			public decimal DiscountAmount { get; set; } // << Discount

			public decimal SubTotalAmount { get; set; } // << Sub Total

			public List<Response.Tax> ElementsTaxs { get; set; } = new List<Response.Tax>(); // << Elements Taxs
			public decimal ElementsTaxsAmount { get; set; }
			public decimal AmountAfterTaxsApplication { get; set; }

			public List<Vat> Vats { get; set; } = new List<Vat>(); // << Vats
			public decimal VatAmount { get; set; } // Total VAT
			public decimal AmountAfterVatApplication { get; set; }

			public List<Response.Tax> DocumentTaxs { get; set; } = new List<Response.Tax>(); // << Document Taxs
			public decimal DocumentTaxsAmount { get; set; }

			public decimal TotalAmount { get; set; } // << Total

			public decimal WithheldAtSourceAmount { get; set; } // << FR : "Retenu à la source"

			public class Element
			{
				public int Reference { get; set; }
				public decimal SubUnitPrice { get; set; }
				public decimal Quantity { get; set; }
				public decimal DiscountPercentage { get; set; }
				public decimal VatPercentage { get; set; }
				public List<BillCalculator.Tax> CustomTaxs { get; set; } = new List<BillCalculator.Tax>();

				public decimal SubTotalAmountBeforeDiscount { get; set; }
				public decimal DiscountAmount { get; set; }
				public decimal SubTotalAmount { get; set; }

				public decimal AmountAfterElementTaxsApplication { get; set; }
				public decimal ElementTaxsAmount { get; set; }

				public decimal VatAmount { get; set; }
				public decimal AmountAfterVatApplication { get; set; }

				public decimal DocumentTaxsAmount { get; set; }

				public decimal TotalAmount { get; set; } // << Total TTC // << After Document-Based Taxs Application
			}

			public class Vat
			{
				public string Name { get; set; }
				public decimal Value { get; set; }
				public decimal Amount { get; set; }
				public decimal Base { get; set; }
			}

			public class Tax
			{
				public int Reference { get; set; }
				public string Name { get; set; }
				public TaxTypes Type { get; set; }
				public decimal Value { get; set; }
				public bool IsDocumentBased { get; set; }
				public decimal Amount { get; set; }
			}
		}

		public class Element
		{
			public int Reference { get; set; }
			public decimal SubUnitPrice { get; set; }
			public decimal Quantity { get; set; }
			public decimal DiscountPercentage { get; set; }
			public decimal VatPercentage { get; set; }
			public List<Tax> CustomTaxs { get; set; } = new List<Tax>();
		}

		public class Tax
		{
			public int Reference { get; set; }
			public string Name { get; set; }
			public TaxTypes Type { get; set; }
			public decimal Value { get; set; }
			public bool IsDocumentBased { get; set; }
		}

		public enum TaxTypes
		{
			Percentage = 0,
			FixedAmount = 1
		}

		private class ComputeElement
		{
			public decimal TPDiscounted { get; set; }
			public decimal TPDTaxed { get; set; }
			public decimal TPDTVat { get; set; }
			public decimal TPDTVTaxed { get; set; }
			public decimal VatValue { get; set; }
			public decimal SubTotalAmountBeforeDiscount { get; set; }

			public ComputeElement()
			{
				SubTotalAmountBeforeDiscount = 0;
				TPDiscounted = 0;
				TPDTaxed = 0;
				TPDTVat = 0;
				TPDTVTaxed = 0;
				VatValue = 0;
			}
		}
	}
}
