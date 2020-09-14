using FileScanner.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using FileScanner.Models;

namespace FileScanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private string selectedFolder;
        private ObservableCollection<string> folderItems = new ObservableCollection<string>();
        private ObservableCollection<string> folderFiles = new ObservableCollection<string>();
        private ObservableCollection<Files> myFiles=new ObservableCollection<Files>();
        public DelegateCommand<string> OpenFolderCommand { get; private set; }
        public DelegateCommand<string> ScanFolderCommand { get; private set; }

        public ObservableCollection<string> FolderItems { 
            get => folderItems;
            set 
            { 
                folderItems = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> FolderFiles
        {
            get => folderFiles;
            set
            {
                folderFiles = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Files> MyFiles
        {
            get => myFiles;
            set
            {
                myFiles = value;
                OnPropertyChanged();
            }
        }
        public string SelectedFolder
        {
            get => selectedFolder;
            set
            {
                selectedFolder = value;
                OnPropertyChanged();
                ScanFolderCommand.RaiseCanExecuteChanged();
            }
        }

        public MainViewModel()
        {
            OpenFolderCommand = new DelegateCommand<string>(OpenFolder);
            ScanFolderCommand = new DelegateCommand<string>(ScanFolder, CanExecuteScanFolder);

        }

        private bool CanExecuteScanFolder(string obj)
        {
            return !string.IsNullOrEmpty(SelectedFolder);
        }

        private void OpenFolder(string obj)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SelectedFolder = fbd.SelectedPath;
                }
            }
        }

        private async void ScanFolder(string dir)
        {
           
            MyFiles.Clear();
            try
            {
               await Task.Run(()=>FolderItems=new ObservableCollection<string> (GetFolders(dir)));
            }
            catch (Exception E) { };
               await Task.Run(() => GetDirs(dir));
            try
            {
              await Task.Run(()=> FolderFiles = new ObservableCollection<string>(GetFolderFiles(dir)));
            }
            catch (Exception E) { };
            await Task.Run(()=> GetFiles(dir));
  

          
              
        
           
        }

         IEnumerable<string> GetFolders(string dir)
        {
            foreach( var d in Directory.EnumerateDirectories(dir, "*",SearchOption.AllDirectories))
            {
                FolderItems.Add(d);
                yield  return d;
            }
        }
        IEnumerable<string> GetFolderFiles(string dir)
        {
            foreach (var d in Directory.EnumerateFiles(dir, "*",SearchOption.AllDirectories))
            {
               FolderFiles.Add(d);
                yield return d;
            }
        }
        private async void GetFiles(string dir)
        {
            foreach (var item in FolderFiles)
            {
             
                    await Task.Run(() => System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => this.MyFiles.Add(new Files(item)))));
                
            }

        }

        private async void GetDirs(string dir)
        {
            foreach (var d in FolderItems)
            {

                await Task.Run(() => System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => this.MyFiles.Add(new Files(d)))));
            }

        }

        ///TODO : Tester avec un dossier avec beaucoup de fichier
        ///TODO : Rendre l'application asynchrone
        ///TODO : Ajouter un try/catch pour les dossiers sans permission


    }
}
