using System.Linq;
using System.Reflection;

namespace Psz.Core.MaterialManagement.Orders.Models
{
	public class DropDownMenu
	{
		public string Id { get; set; }
		public string Value { get; set; }

		public static List<DropDownMenu> GetDropDownMenu<T>(List<T> data, string Id, string Name)
				where T : class
		{
			// filter out empty values white space values?
			// Id is a string to accomodate string Ids in the database.
			return data.Select(x => new DropDownMenu() { Id = Convert.ToString(getObjectValue<T>(x, Id)), Value = Convert.ToString(getObjectValue<T>(x, Name)) })
				//.Where(x => !String.IsNullOrEmpty(x.Value) && !String.IsNullOrWhiteSpace(x.Value) )
				.ToList();

		}
		private static object getObjectValue<T>(T obj, string prop)
		{
			Type myType = obj.GetType();
			IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

			return props.FirstOrDefault(x => x.Name == prop)?.GetValue(obj, null);
		}

	}
}
