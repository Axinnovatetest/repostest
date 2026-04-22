using System.ComponentModel;

namespace Psz.Core.FinanceControl.Enums
{
	public class BudgetEnums
	{
		public static int MaxOrderValidationStep { get; } = 1;
		public static int CompletedOrderValidationStep { get; } = 110;
		public enum freezeOrUnfreeze
		{
			Freeze = 1,
			Unfreeze = 2
		}
		public enum ProjectTypes: int
		{
			[Description("External")]
			External = 1,
			[Description("Internal")]
			Internal = 2,
			[Description("Finance")]
			Finance = 3
		}
		public enum OrderTypes: int
		{
			[Description("Purchase")]
			Purchase = 0,
			[Description("Leasing")]
			Leasing = 1
		}
		public enum ProjectApprovalStatuses: int
		{
			[Description("Pending")]
			Inactive = 0,
			[Description("Approved")]
			Active = 1,
			[Description("Rejected")]
			Reject = 2
		}
		public enum ProjectStatuses: int
		{
			[Description("Suspended")]
			Suspended = 0,
			[Description("Active")]
			Active = 1,
			[Description("Closed")]
			Closed = 2
		}
		public enum OrderStatuses: int
		{
			Validated = 0,
			Rejected = 1
		}
		public enum ValidationStatuses: int
		{
			[Description("Validation")]
			Validation = 0,
			[Description("Rejection")]
			Rejection = 1,
			[Description("Completion")]
			Completion = 2
		}
		public enum ValidationLevels: int
		{
			[Description("Draft")]
			Draft = 0,
			[Description("Department Director")]
			DepartmentDirector = 1,
			[Description("Site Director")]
			SiteDirector = 2,
			[Description("Site Purchase")]
			Purchase = 3,
			[Description("Fully Validated")]
			FullyValidated = 4,
			[Description("Super Validator")]
			SuperValidator = 6
		}
		public enum StatisticsTypes: int
		{
			[Description("Un-Placed")]
			Unplaced = 0,
			[Description("Unconfirmed")]
			Unconfirmed = 1,
			[Description("Delivery Overdue")]
			DeliveryOverdue = 2,
			[Description("Next Deliveries")]
			NextDeliveries = 3,
			[Description("Booked")]
			Booked = 4,
			[Description("Open Leasings")]
			OpenLeasing = 5
		}
		public enum OrderWorkflowActions
		{
			[Description("Create")]
			Create = 0,
			[Description("Validation Request")]
			ValidationRequest = 1,
			[Description("Validate")]
			Validate = 2,
			[Description("Reject")]
			Reject = 3,
			[Description("Place")]
			Place = 4,
			[Description("Unvalidate")]
			Unvalidate = 5,
			[Description("Cancel")]
			Cancel = 6,
			[Description("Archive")]
			Archive = 7,
			[Description("Delete")]
			Delete = 8,
			[Description("Book")]
			Book = 9,
			[Description("Unarchive")]
			Unarchive = 10,
			[Description("Unbook")]
			Unbook = 11,
		}
		public enum ProjectWorkflowActions
		{
			[Description("Create")]
			Create = 0,
			[Description("Archive")]
			Archive = 1,
			[Description("Unarchive")]
			Unarchive = 2,
			[Description("Close")]
			Close = 3,
			[Description("Open")]
			Open = 4,
			[Description("Delete")]
			Delete = 5,
			[Description("UpdateStatus")]
			UpdateStatus = 6,
			[Description("UpdateCustomerBudget")]
			UpdateCustomerBudget = 7,
			[Description("UpdateInternalBudget")]
			UpdateInternalBudget = 8,
		}
		public static string GetOrderValidationStatus(int validationLevel)
		{
			if(validationLevel <= 0)
				return "Draft";

			switch(validationLevel)
			{
				//case 1:
				//    return "First Validation";
				//case 2:
				//    return "Second Validation";
				//case 3:
				//    return "Third Validation";
				//case 4:
				//    return "Fourth Validation";
				//case 5:
				//    return "Fifth Validation";
				//case 6:
				//    return "Sixth Validation";
				//case 7:
				//    return "Seventh Validation";
				//case 8:
				//    return "Eighth Validation";
				//case 9:
				//    return "Ninth Validation";
				//case 10:
				//    return "Tenth Validation";
				case 110:
					return "Completed";
				default:
					return "In progress";
			}
		}
		public static StatisticsTypes getStatisticsFromName(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return (StatisticsTypes)(-1);

			if(name.ToLower().Trim() == "unplaced")
				return StatisticsTypes.Unplaced;

			if(name.ToLower().Trim() == "unconfirmed")
				return StatisticsTypes.Unconfirmed;

			if(name.ToLower().Trim() == "deliveryoverdue")
				return StatisticsTypes.DeliveryOverdue;

			if(name.ToLower().Trim() == "nextdeliveries")
				return StatisticsTypes.NextDeliveries;

			if(name.ToLower().Trim() == "booked")
				return StatisticsTypes.Booked;

			if(name.ToLower().Trim() == "openleasing")
				return StatisticsTypes.OpenLeasing;

			return (StatisticsTypes)(-1);
		}
		public enum ReceptionSearchFields
		{
			[Description("Nr")]
			Nr = 0,
			[Description("OrderNUmber")]
			OrderNumber = 1,
			[Description("Type")]
			Type = 2,
			[Description("Employee")]
			Employee = 3,
			[Description("Company")]
			Company = 4,
			[Description("Department")]
			Department = 5,
			[Description("Supplier")]
			Supplier = 6,
			[Description("Date")]
			Date = 7,
			[Description("DeliveryDate")]
			DeliveryDate = 8,
			[Description("Done")]
			Done = 9,
		}
		public enum AllocationTypes: int
		{
			[Description("Fix")]
			Fix = 0,
			[Description("Invest")]
			Invest = 1
		}
	}
}
