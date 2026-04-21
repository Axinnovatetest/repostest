namespace Psz.Core.Models
{
	public class ValueTextModel
	{
		public string Value { get; set; }
		public string Text { get; set; }

		public ValueTextModel()
		{ }
		public ValueTextModel(string value, string text)
		{
			this.Value = value;
			this.Text = text;
		}
	}
}
