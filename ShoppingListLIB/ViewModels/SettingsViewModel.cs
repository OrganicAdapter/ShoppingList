using System;
using System.Collections.Generic;
using UniversalExtensions.MVVM;
using Windows.Globalization;
using Windows.UI.Popups;

namespace ShoppingListLIB.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields



        #endregion //Fields

        #region Properties

        public Dictionary<string, string> Cultures { get; set; }

        public RelayCommand Load { get; set; }
        public RelayCommand<object> CultureSelected { get; set; }

        #endregion //Properties

        #region Constructor

        public SettingsViewModel()
        {
            Cultures = new Dictionary<string, string>()
            {
                {"United States", "en-US"},
                {"Hungary", "hu-HU"}
            };

            Load = new RelayCommand(ExecuteLoad);
            CultureSelected = new RelayCommand<object>(ExecuteCultureSelected);
        }

        #endregion //Constructor

        #region Methods

        private void ExecuteLoad()
        {
            
        }

        private async void ExecuteCultureSelected(object culture)
        {
            var split = culture.ToString().Split(',');
            var res = split[1].Replace("]", "");

            ApplicationLanguages.PrimaryLanguageOverride = res.Trim();

            MessageDialog dialog = new MessageDialog("Please restart the application.");
            await dialog.ShowAsync();
        }

        #endregion //Methods           
    }
}
