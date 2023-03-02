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
            AllAnimalsbox();
            
           
        }

        private void showZooselectedText()
        {
            try
            {
                string query = "select * from Zoo where Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);  
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);    
               

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", Zoolist.SelectedValue);
                    DataTable zooTable = new DataTable();

                   adapter.Fill(zooTable);              
                    addBox.Text = zooTable.Rows[0]["Location"].ToString();

                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }
        private void showAnimalselectedText()
        {
            try
            {
                string query = "select * from Animal where Id = @Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);


                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Id", AllAnimals.SelectedValue);
                    DataTable AnimalTable = new DataTable();

                    adapter.Fill(AnimalTable);
                    addBox.Text = AnimalTable.Rows[0]["Name"].ToString();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
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

        private void AllAnimalsbox()
        {
            try
            {
                string query = "Select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query,sqlConnection);

                using(sqlDataAdapter)
                {
                    DataTable AnimalTable = new DataTable();
                    sqlDataAdapter.Fill(AnimalTable);

                    AllAnimals.DisplayMemberPath = "Name";
                    AllAnimals.SelectedValuePath = "Id";
                    AllAnimals.ItemsSource = AnimalTable.DefaultView;
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
              //  MessageBox.Show(e.ToString());
            }
        }

        private void Zoolist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAnimal();
            showZooselectedText();          
        }

        private void AllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showAnimalselectedText();
        }

        private void Addbutton_Click(object sender, RoutedEventArgs e)
        {
            if(AllAnimals.SelectedItem == null || Zoolist.SelectedItem == null)
            {
                MessageBox.Show("No Animal/ Zoo selected");
            }
            else
            {
                try
                {
                    string query = " insert into ZooAnimal values (@ZooId, @AnimalId)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ZooId", Zoolist.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AnimalId", AllAnimals.SelectedValue);
                    sqlCommand.ExecuteScalar();
                }
                catch(Exception r)
                {
                    MessageBox.Show(r.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAnimal();
                }
               
            }
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            
            if(Zoolist.SelectedItem == null)
            {
                MessageBox.Show("No Zoo selected");
            }
            else
            {
                try
                {
                    string query = "Delete from Zoo where Id = @ZooId";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ZooId", Zoolist.SelectedValue);
                    sqlCommand.ExecuteScalar();          
                }
                catch(Exception f)
                {
                    MessageBox.Show(f.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowZoos();
                }

            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(addBox.Text) || string.IsNullOrWhiteSpace(addBox.Text))
            {
                MessageBox.Show("NO TEXT IN THE BOX");
            }
            else
            {
                try
                {
                    string query = "Insert into ZOO values (@Location) ";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Location", addBox.Text);
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception f)
                {
                    MessageBox.Show(f.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowZoos();
                    addBox.Clear();
                }
                
            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (Animallist.SelectedItem == null || Zoolist.SelectedItem == null)
            {
                MessageBox.Show("No Animal/ Zoo selected");
            }
            else
            {
                try
                {
                    string query = " Delete from ZooAnimal values (@ZooId, @AnimalId)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ZooId", Zoolist.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AnimalId", Animallist.SelectedValue);
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception r)
                {
                    MessageBox.Show(r.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAnimal();
                }

            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(addBox.Text) || string.IsNullOrWhiteSpace(addBox.Text))
            {
                MessageBox.Show("NO TEXT IN THE BOX");
            }
            else
            {
                try
                {
                    string query = "Insert into Animal values (@Name) ";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Name", addBox.Text);
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception f)
                {
                    MessageBox.Show(f.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    AllAnimalsbox();
                    addBox.Clear();
                }

            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (AllAnimals.SelectedItem == null)
            {
                MessageBox.Show("No Animal selected");
            }
            else
            {
                try
                {
                    string query = "Delete from Animal where Id = @Id";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Id", AllAnimals.SelectedValue);
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception f)
                {
                    MessageBox.Show(f.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    AllAnimalsbox();
                }

            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Update Animal set Name =@Name where Id=@Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", addBox.Text);
                sqlCommand.Parameters.AddWithValue("@Id", AllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
                // selecctedValue =Convert.ToInt32(Zoolist.SelectedValue);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.ToString());
            }
            finally
            {
                sqlConnection.Close();
                AllAnimalsbox();
                addBox.Clear();
            }
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
           // int ?selecctedValue;
           
            try
            {
                string query = "Update ZOO set Location =@Location where Id=@Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", addBox.Text);
                sqlCommand.Parameters.AddWithValue("@Id", Zoolist.SelectedValue);
                sqlCommand.ExecuteScalar();
               // selecctedValue =Convert.ToInt32(Zoolist.SelectedValue);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                addBox.Clear();
            }

        }

       
    }
}
