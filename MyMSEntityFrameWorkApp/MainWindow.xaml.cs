/******************************************************************************/
/*                                                                            */
/*   Program: MyMSEntityFrameWorkApp                                          */
/*   A simple Entity Framework application.                                   */
/*                                                                            */
/*   18.08.2014 0.0.0.0 uhwgmxorg Start                                       */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMSEntityFrameWorkApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string message;
        public string Message { get { return message; } set { message = value; OnPropertyChanged("Message"); } }

        TestDBEntities TestDBEntitiesContext;  
        public ObservableCollection<Name> NameList { get; set; }
        MyMSEntityFrameWorkApp.Name SelectedItem { get; set; }

        public bool IsAddingANewItem { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            IsAddingANewItem = false;
            DataContext = this;

            LoadTable();
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Reloade_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            LoadTable();
        }

        /// <summary>
        /// Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion

        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// Lable_Message_MouseDown
        /// Clear Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lable_Message_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Message = "";
        }

        /// <summary>
        /// DataGrid_TableNames_SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_TableNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var Grid = (DataGrid)sender;
                SelectedItem = Grid.SelectedItem as MyMSEntityFrameWorkApp.Name;
                if (SelectedItem.Id > 0)
                    Message = String.Format("SelectedItem is {0} {1} {2} {3}", SelectedItem.Id, SelectedItem.FirstName, SelectedItem.LastName, SelectedItem.Age);
            }
            catch (Exception)
            {
                Message = "Nothing is selected";
            }
        }

        /// <summary>
        /// DataGrid_TableNames_PreviewKeyDown
        /// Here we perform the Delete
        /// Delete on Del-KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_TableNames_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var Grid = (DataGrid)sender;
            if (Key.Delete == e.Key)
            {
                try
                {
                    long Id = ((MyMSEntityFrameWorkApp.Name)Grid.SelectedItem).Id;
                    var Record = TestDBEntitiesContext.Names.First(n => n.Id == Id);
                    TestDBEntitiesContext.Names.Remove(Record);
                    TestDBEntitiesContext.SaveChanges();
                    Message = "Save Data to SQL";
                }
                catch(Exception)
                {
                    Message = "ERROR by Delete";
                }
            }
        }

        /// <summary>
        /// DataGrid_TableNames_RowEditEnding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_TableNames_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var Grid = (DataGrid)sender;
            Grid.RowEditEnding -= DataGrid_TableNames_RowEditEnding;
            Grid.CommitEdit();
            Grid.Items.Refresh();
            Grid.RowEditEnding += DataGrid_TableNames_RowEditEnding;
            if (IsAddingANewItem)
            {
                Name EFRecord = new Name();
                EFRecord.FirstName = SelectedItem.FirstName;
                EFRecord.LastName = SelectedItem.LastName;
                EFRecord.Age = SelectedItem.Age;
                EFRecord.InsertDate = DateTime.Now;
                TestDBEntitiesContext.Names.Add(EFRecord);
                IsAddingANewItem = false;
            }
            try
            {
                TestDBEntitiesContext.SaveChanges();
                Message = "Save Data to SQL";
            }
            catch (Exception)
            {
                Message = "ERROR by Insert";
            }
        }


        private void DataGrid_TableNames_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            IsAddingANewItem = true;
        }

        #endregion

        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// LoadTable
        /// </summary>
        private void LoadTable()
        {
            try
            {
                TestDBEntitiesContext = new TestDBEntities();

                NameList = new ObservableCollection<Name>();

                var Names = TestDBEntitiesContext.Names;
                foreach (var name in Names)
                    NameList.Add(name);

                DataGrid_TableNames.ItemsSource = NameList;

                Message = "Load Data from SQL";
            }
            catch (Exception)
            {
                Message = "ERROR can not load data from SQL-Server!";
            }
        }

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="p"></param>
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        #endregion

    }
}
