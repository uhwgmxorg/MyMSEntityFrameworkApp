/******************************************************************************/
/*                                                                            */
/*   Program: MyMSEntityFrameWorkApp                                          */
/*   A simple Entity Framework application.                                   */
/*                                                                            */
/*   18.08.2014 0.0.0.0 uhwgmxorg Start                                       */
/*   04.09.2014         uhwgmxorg Changed comments                            */
/*   15.09.2014 1.1.0.0 uhwgmxorg Introduce the using command for DbContex,   */
/*                                changing database restrictions (NOT NULL).  */
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
                if (SelectedItem != null && SelectedItem.Id > 0)
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
        /// DELETE on Del-KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_TableNames_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var Grid = (DataGrid)sender;
            if (Key.Delete == e.Key)
            {
                using (var EntitiesContext = new TestDBEntities())
                {
                    try
                    {
                        long Id = ((MyMSEntityFrameWorkApp.Name)Grid.SelectedItem).Id;
                        Name EntityItem = EntitiesContext.Names.First(n => n.Id == Id);
                        EntitiesContext.Names.Remove(EntityItem);
                        EntitiesContext.SaveChanges();
                        Message = "Save Data to SQL";
                    }
                    catch (Exception)
                    {
                        Message = "ERROR on Delete";
                    }
                }
            }
        }

        /// <summary>
        /// DataGrid_TableNames_RowEditEnding
        /// INSERT or UPDATE depents on IsAddingANewItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_TableNames_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var Grid = (DataGrid)sender;
            Grid.RowEditEnding -= DataGrid_TableNames_RowEditEnding; // CommitEdit and Items.Refresh is not possible when event is subscribed
            Grid.CommitEdit();
            Grid.Items.Refresh();
            Grid.RowEditEnding += DataGrid_TableNames_RowEditEnding;
            using (var EntitiesContext = new TestDBEntities())
            {
                // INSERT
                if (IsAddingANewItem)
                {
                    if (SelectedItem != null)
                    {
                        Name EntityItem = new Name();
                        EntityItem.FirstName = SelectedItem.FirstName;
                        EntityItem.LastName = SelectedItem.LastName;
                        EntityItem.Age = SelectedItem.Age;
                        EntityItem.InsertDate = DateTime.Now;
                        EntitiesContext.Names.Add(EntityItem);
                        IsAddingANewItem = false;
                    }
                    else
                        Message = String.Format("ERROR on Insert");
                }
                else // UPDATE
                {
                    Name UpdateItem = Grid.SelectedItem as MyMSEntityFrameWorkApp.Name;
                    if(UpdateItem != null)
                    {
                        Name EntityItem = EntitiesContext.Names.Where(o => o.Id == UpdateItem.Id).FirstOrDefault();
                        if (EntityItem != null)
                        {
                            EntityItem.FirstName = UpdateItem.FirstName;
                            EntityItem.LastName = UpdateItem.LastName;
                            EntityItem.Age = UpdateItem.Age;
                            EntityItem.InsertDate = UpdateItem.InsertDate;
                        }
                        else
                            Message = String.Format("ERROR on Update");
                    }
                    else
                        Message = String.Format("ERROR on Update");
                }

                // Save the changes to SQL DB either a row is added or just updated
                try
                {
                    EntitiesContext.SaveChanges();
                    Message = "Save Data to SQL";
                }
                catch (Exception)
                {
                    Message = "ERROR on Save Data to SQL";
                }
            }
        }

        /// <summary>
        /// DataGrid_TableNames_AddingNewItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// SELECT
        /// </summary>
        private void LoadTable()
        {
            using (var EntitiesContext = new TestDBEntities())
            {
                try
                {
                    NameList = new ObservableCollection<Name>();
                    var Names = EntitiesContext.Names;
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
