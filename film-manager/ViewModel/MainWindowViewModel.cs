using film_manager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows.Input;
using System.Diagnostics;
using film_manager.View;

namespace film_manager.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public MainWindowViewModel()
        {
            this._settingsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "settings.txt");

            // initialize settings
            try
            {
                Settings.Main = File.ReadAllText(this._settingsPath);
            }
            catch
            {
                var inputDialog = new InputTextBoxDialog("Settings", "Welcome to my app called 'films manager'. First of all, you need to tell me the main folder (absolute) path of your films.");
                if (inputDialog.ShowDialog() == true)
                    Settings.Main = inputDialog.Input;
                else
                    Environment.Exit(0);

                try { SaveSettings(Settings.Main); }
                catch (Exception e)
                {
                    MessageBoxDialog.ShowMessage("Error", e.Message);
                    Environment.Exit(0);
                }
            }

            // initialize  films
            this.ViewFilms = Directory.GetFiles(Settings.Main, "*.mp4", SearchOption.AllDirectories).Select(o => new FilmObjectViewModel(o));
        }



        IEnumerable<FilmObjectViewModel> _viewFilms = new List<FilmObjectViewModel>();
        public IEnumerable<FilmObjectViewModel> ViewFilms
        {
            get { return this._viewFilms; }
            set
            {
                this._viewFilms = value;
                this.OnPropertyChanged(nameof(ViewFilms));
            }
        }



        string _settingsPath = "";
        public void SaveSettings(string main) => File.WriteAllText(this._settingsPath, main);



        ICommand _openFolder;
        public ICommand OpenFolder
        {
            get
            {
                return this._openFolder ?? (this._openFolder = new RelayCommand(
                    o => Process.Start("explorer", Settings.Main),
                    o => Directory.Exists(Settings.Main)));
            }
        }
    }
}
