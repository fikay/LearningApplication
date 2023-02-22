using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace WpfApp_Zoo_Manger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            string connectionstring = ConfigurationManager.ConnectionStrings["WpfApp_Zoo_Manger.Properties.Settings.FikayoDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionstring);
            ShowZoos();
            
           
        }

        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);
                    Zoolist.DisplayMemberPath = "Location";
                    Zoolist.SelectedValuePath = "Id";
                    Zoolist.ItemsSource = zooTable.DefaultView;

                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString()); 
            }
        }

        private void ShowAnimal()
        {
            try
            {
                string Query = "Select * from Animal a inner join ZooAnimal za on a.id = za.AnimalId where za.ZooId = @ZooId ";
                SqlCommand sqlCommand = new SqlCommand(Query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using(sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId",Zoolist.SelectedValue);
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    Animallist.DisplayMemberPath = "Name";
                    Animallist.SelectedValuePath = "Id";
                    Animallist.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void Zoolist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAnimal();

        }
    }
}
