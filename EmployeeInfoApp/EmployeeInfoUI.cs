using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EmployeeInfoApp
{
    public partial class EmployeeInfoUI : Form
    {
        public EmployeeInfoUI()
        {
            InitializeComponent();
        }

        public bool isEmailExist = false;
        public bool isUpdateMode = false;
        public bool isDeleteMode = false;
        public int employeeid;
        private void label3_Click(object sender, EventArgs e)
        {

        }

        employeeInfo anEmployee = new employeeInfo();

        private void saveButton_Click(object sender, EventArgs e)
        {

            anEmployee.name = nameTextBox.Text;
            anEmployee.address = addressTextBox.Text;
            anEmployee.email = emailTextBox.Text;
            anEmployee.salary = salaryTextBox.Text;
            isEmailExist = IsEmailExists(anEmployee.email);

            if (isUpdateMode)
            {
                string connectionString = @"SERVER=PC-301-04\SQLEXPRESS;Database=Employee_Info;Integrated Security=true";
                SqlConnection connection = new SqlConnection(connectionString);
                string query = "UPDATE name SET name='" + anEmployee.name + "',address='" + anEmployee.address + "',email='" +
                               anEmployee.email + "',salary='" + anEmployee.salary + "' WHERE id='" + employeeid + "'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int inserted = command.ExecuteNonQuery();
                connection.Close();
                if (inserted > 0)
                {
                    MessageBox.Show("Updated");
                    isUpdateMode = false;
                    saveButton.Text = "Save";
                    isDeleteMode = false;
                    showButton.Text = "Show";
                    employeeid = 0;
                    ShowAllEmployeeInfo();
                }
                else
                {
                    MessageBox.Show("Not Updated");
                }
            }
            else
            {
                if (isEmailExist)
                {
                    MessageBox.Show("Email Already Exists");
                    return;
                }
                string connectionString = @"SERVER=PC-301-04\SQLEXPRESS;Database=Employee_Info;Integrated Security=true";
                SqlConnection connection = new SqlConnection(connectionString);
                string query = "INSERT INTO name VALUES('" + anEmployee.name + "','" + anEmployee.address + "','" +
                               anEmployee.email + "','" + anEmployee.salary + "')";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int inserted = command.ExecuteNonQuery();
                connection.Close();
                if (inserted > 0)
                {
                    MessageBox.Show("Saved");
                }
                else
                {
                    MessageBox.Show("Not saved");
                }
     
            }
                
 
        }
            
        public bool IsEmailExists(string email)
        {
            string connectionString = @"SERVER=PC-301-04\SQLEXPRESS;Database=Employee_Info;Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM name WHERE email='" +email+ "'";
            SqlCommand command = new SqlCommand(query, connection);

            bool isEmailExist = false;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                isEmailExist = true;
                break;
                

            }
            reader.Close();
            connection.Close();
            return isEmailExist;

        }

        private void showButton_Click(object sender, EventArgs e)
        {
            string connectionString = @"SERVER=PC-301-04\SQLEXPRESS;Database=Employee_Info;Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM name";
            SqlCommand command = new SqlCommand(query, connection);

            
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();


            List<employeeInfo> employeelist=new List<employeeInfo>();


            while (reader.Read())
            {
                employeeInfo employee=new employeeInfo();
                employee.id = int.Parse(reader["id"].ToString());
                employee.name = reader["name"].ToString();
                employee.address = reader["address"].ToString();
                employee.email = reader["email"].ToString();
                employee.salary = reader["salary"].ToString(); 

             employeelist.Add(employee);


            }
            reader.Close();
            connection.Close();
            LoademployeelistView(employeelist);
        }

        public void LoademployeelistView(List<employeeInfo> employee)
        {
            foreach (var employee1  in employee)
            {
               
                ListViewItem item=new ListViewItem(employee1.id.ToString());
                item.SubItems.Add(employee1.name);
                item.SubItems.Add(employee1.address);
                item.SubItems.Add(employee1.email);
                item.SubItems.Add(employee1.salary);

                employeeListView.Items.Add(item);
            }

        }


        private void showListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           // ListViewItem item = showListBox.SelectedItems[01];
        }

        public void ShowAllEmployeeInfo()
        {
            string connectionString = @"SERVER=PC-301-04\SQLEXPRESS;Database=Employee_Info;Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM name";
            SqlCommand command = new SqlCommand(query, connection);

            
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();


            List<employeeInfo> employeelist=new List<employeeInfo>();


            while (reader.Read())
            {
                employeeInfo employee=new employeeInfo();
                employee.id =int.Parse( reader["id"].ToString());
                employee.name = reader["name"].ToString();
                employee.address = reader["address"].ToString();
                employee.email = reader["email"].ToString();
                employee.salary = reader["salary"].ToString(); 

             employeelist.Add(employee);


            }
            reader.Close();
            connection.Close();
            LoademployeelistView(employeelist);
        
        }

        public void employeeListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = employeeListView.SelectedItems[0];
            int ID = int.Parse(item.Text.ToString());

            employeeInfo employee = GetEmployeeBYID(ID);
            if (employee!= null)
            {
                isUpdateMode = true;
                saveButton.Text = "Update";
                showButton.Text = "Delete";
                isDeleteMode = true;
                employeeid = Convert.ToInt32(employee.id);
                nameTextBox.Text = employee.name;
                addressTextBox.Text = employee.address;
                emailTextBox.Text = employee.email;
                salaryTextBox.Text = employee.salary;

            }


        }

        public employeeInfo GetEmployeeBYID(int id)
        {
            string connectionString = @"SERVER=PC-301-04\SQLEXPRESS;Database=Employee_Info;Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM name WHERE id='"+id+"' ";
            SqlCommand command = new SqlCommand(query, connection);


            connection.Open();
            SqlDataReader reader = command.ExecuteReader();


            List<employeeInfo> employeelist = new List<employeeInfo>();


            while (reader.Read())
            {
                employeeInfo employee = new employeeInfo();
                employee.id = int.Parse(reader["id"].ToString());
                employee.name = reader["name"].ToString();
                employee.address = reader["address"].ToString();
                employee.email = reader["email"].ToString();
                employee.salary = reader["salary"].ToString();

                employeelist.Add(employee);


            }
            reader.Close();
            connection.Close();
            return employeelist.FirstOrDefault();
        }

        private void EmployeeInfoUI_Load(object sender, EventArgs e)
        {
            ShowAllEmployeeInfo();
        }
    

}


}
    

