using film_manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using film_manager.View;

namespace film_manager.ViewModel
{
    class FilmObjectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


        static string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                folder += Path.DirectorySeparatorChar;
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        FilmObject _obj;

        public FilmObjectViewModel(FilmObject filmObj)
        {
            this._obj = filmObj;
        }

        public FilmObjectViewModel(string filmPath) : this(new FilmObject() { path = filmPath, size = new FileInfo(filmPath).Length, duration = 0 }) { }

        public string FilePath
        {
            get { return this._obj.path; }
            set
            {
                this._obj.path = value;
                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Label));
            }
        }
        public double Duration => this._obj.duration;
        public long Size => this._obj.size;

        public string Name => Path.GetFileNameWithoutExtension(this.FilePath);
        public string Label
        {
            get
            {
                var m = Settings.Main;
                var f = Path.GetDirectoryName(this.FilePath);
                if (m.Length == f.Length)
                    return "";
                return f.Substring(m.Length + 1);
            }
        }





        #region Commands

        ICommand _openFile;
        public ICommand OpenFile
        {
            get
            {
                return this._openFile ?? (this._openFile = new RelayCommand(
                    o => Process.Start(this.FilePath)));
            }
        }

        ICommand _seeOnGoogle;
        public ICommand SeeOnGoogle
        {
            get
            {
                return this._seeOnGoogle ?? (this._seeOnGoogle = new RelayCommand(
                    o => Process.Start($"https://www.google.com/search?q={Uri.EscapeDataString(this.Name)}")));
            }
        }

        ICommand _openInFolder;
        public ICommand OpenInFolder
        {
            get
            {
                return this._openInFolder ?? (this._openInFolder = new RelayCommand(
                    o => Process.Start("explorer", $"/select, \"{this.FilePath}\"")));
            }
        }

        ICommand _changeLabel;
        public ICommand ChangeLabel
        {
            get
            {
                return this._changeLabel ?? (this._changeLabel = new RelayCommand(o =>
                {
                    try
                    {
                        var dialog = new InputTextBoxDialog("New label", "Please, tell me the new label name.");
                        if (dialog.ShowDialog() == true)
                        {
                            var s = dialog.Input;
                            var newPath = Path.Combine(Settings.Main, s, Path.GetFileName(this.FilePath));
                            this.MoveFile(newPath);
                        }
                    }
                    catch (Exception e) { MessageBoxDialog.ShowMessageAsync("Error", e.Message); }
                }));
            }
        }

        ICommand _rename;
        public ICommand Rename
        {
            get
            {
                return this._rename ?? (this._rename = new RelayCommand(o =>
                {
                    try
                    {
                        var dialog = new InputTextBoxDialog("Rename", "Please, tell me the new name (you should omit the extension).");
                        if (dialog.ShowDialog() == true)
                        {
                            var s = dialog.Input;
                            var newPath = Path.Combine(Path.GetDirectoryName(this.FilePath), s + Path.GetExtension(this.FilePath));
                            this.MoveFile(newPath);
                        }
                    }
                    catch (Exception e) { MessageBoxDialog.ShowMessageAsync("Error", e.Message); }
                }));
            }
        }

        #endregion





        void MoveFile(string newPath)
        {
            var parent = Path.GetDirectoryName(newPath);
            Directory.CreateDirectory(parent);
            File.Move(this.FilePath, newPath);
            this.FilePath = newPath;
        }

        public FilmObject ToFilmObject() => this._obj;
    }
}
