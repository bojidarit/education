namespace WpfAppWithCatel.Models
{
	using Catel.Data;
	using System.Globalization;

	public class LanguageModel : IdNameModel
	{
		public LanguageModel() { }

		public LanguageModel(int id, string cultureName)
		{
			base.Id = id;
			this.CultureName = cultureName;
			this.Culture = CultureInfo.CreateSpecificCulture(cultureName);
			base.Name = this.Culture.NativeName;
		}

		public LanguageModel(int id, CultureInfo cultureInfo)
		{
			base.Id = id;
			this.Culture = cultureInfo;
			this.CultureName = this.Culture.Name;
			base.Name = this.Culture.NativeName;
		}

		public string CultureName
		{
			get { return GetValue<string>(CultureNameProperty); }
			set { SetValue(CultureNameProperty, value); }
		}
		public static readonly PropertyData CultureNameProperty = RegisterProperty("CultureName", typeof(string), null);

		public CultureInfo Culture
		{
			get { return GetValue<CultureInfo>(CultureProperty); }
			set { SetValue(CultureProperty, value); }
		}
		public static readonly PropertyData CultureProperty = RegisterProperty("Culture", typeof(CultureInfo), null);

		public override string ToString()
		{
			string culture = this.Culture != null ? $" ({this.Culture.Name})" : string.Empty;
			return $"{this.Name}{culture}";
		}
	}
}
