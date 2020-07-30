namespace WpfAppWithCatel.ViewModels
{
	using Catel.Data;
	using Catel.MVVM;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.Linq;
	using System.Threading.Tasks;
	using WpfAppWithCatel.Extensions;
	using WpfAppWithCatel.Models;
	using WPFLocalizeExtension.Engine;

	public class MainWindowViewModel : ViewModelBase
	{
		private string[] _allCultures = { "en-US", "de-DE", "bg-BG" };

		public MainWindowViewModel()
		{
			CloseCommand = new Command(OnCloseCommandExecute);

			Task task = InitializeLanguagesAsync();
		}

		#region Properties

		public override string Title => "WPF Application With CATEL MVVM and Localization Demo";

		public ObservableCollection<LanguageModel> Languages
		{
			get { return GetValue<ObservableCollection<LanguageModel>>(LanguagesProperty); }
			set { SetValue(LanguagesProperty, value); }
		}
		public static readonly PropertyData LanguagesProperty = RegisterProperty("Languages", typeof(ObservableCollection<LanguageModel>), null);

		#region SelectedLanguage

		public LanguageModel SelectedLanguage
		{
			get { return GetValue<LanguageModel>(SelectedLanguageProperty); }
			set { SetValue(SelectedLanguageProperty, value); }
		}
		public static readonly PropertyData SelectedLanguageProperty =
			RegisterProperty("SelectedLanguage", typeof(LanguageModel), null,
				(sender, e) => ((MainWindowViewModel)sender).OnSelectedLanguageChanged(e));

		private void OnSelectedLanguageChanged(AdvancedPropertyChangedEventArgs e)
		{
			if (this.SelectedLanguage != null)
			{
				Task task = SetCulture(this.SelectedLanguage.Culture);
			}
		}

		public string SupportedLanguages
		{
			get { return GetValue<string>(SupportedLanguagesProperty); }
			set { SetValue(SupportedLanguagesProperty, value); }
		}
		public static readonly PropertyData SupportedLanguagesProperty = RegisterProperty("SupportedLanguages", typeof(string), null);

		public string DateFormattedString
		{
			get { return GetValue<string>(DateFormattedStringProperty); }
			set { SetValue(DateFormattedStringProperty, value); }
		}
		public static readonly PropertyData DateFormattedStringProperty = RegisterProperty("DateFormattedString", typeof(string), null);

		public string NumberFormattedString
		{
			get { return GetValue<string>(NumberFormattedStringProperty); }
			set { SetValue(NumberFormattedStringProperty, value); }
		}
		public static readonly PropertyData NumberFormattedStringProperty = RegisterProperty("NumberFormattedString", typeof(string), null);

		#endregion //SelectedLanguage

		#endregion //Properties

		#region Commands

		#region CloseCommand

		public Command CloseCommand { get; private set; }

		private void OnCloseCommandExecute()
		{
			System.Environment.Exit(0);
		}

		#endregion //CloseCommand

		#endregion //Commands

		#region Methods

		private async Task<bool> SetCulture(CultureInfo culture)
		{
			if (culture == null)
			{
				await this.ShowError("Culture is null.", "Setting culture");
				return false;
			}

			try
			{
				System.Threading.Thread.CurrentThread.CurrentCulture = culture;
				System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
				LocalizeDictionary.Instance.Culture = culture;

				if (this.Languages != null)
				{
					IEnumerable<string> cultures = this.Languages.Select(l => $"'{GetLocText(l?.Culture.TwoLetterISOLanguageName)}'");
					this.SupportedLanguages = string.Join(", ", cultures);
					this.DateFormattedString = $"{DateTime.Today.ToLongDateString()} ({DateTime.Today.ToShortDateString()})";
					this.NumberFormattedString = 123456.789m.ToString();
				}
				
			}
			catch (Exception ex)
			{
				await this.ShowError(ex.Message, ex.GetType().Name);
			}

			return true;

		}

		private string GetLocText(string key)
		{
			try { return LocalizeDictionary.Instance.DefaultProvider.GetLocalizedObject(key, null, LocalizeDictionary.Instance.Culture).ToString(); }
			catch { return string.Empty; }
		}

		private async Task<bool> InitializeLanguagesAsync()
		{
			try
			{
				this.Languages = new ObservableCollection<LanguageModel>(_allCultures.Select((value, index) => new LanguageModel(index++, value)));
				this.Languages.Add(new LanguageModel(999, CultureInfo.InvariantCulture));
				this.SelectedLanguage = this.Languages.FirstOrDefault();
				return true;
			}
			catch (Exception ex)
			{
				await this.ShowError(ex.Message, ex.GetType().Name);
				return false;
			}
		}

		#endregion //Methods
	}
}
