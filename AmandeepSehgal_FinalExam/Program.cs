using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Xml;

namespace AmandeepSehgal_FinalExam
{
    class Program
    {

        static SqlConnection scon;
        static SqlDataAdapter da;
        static DataSet ds = new DataSet();

        static void Main(string[] args)
        {
            int choice = 0, input = 0;
            string u_name = "", pass = "";
            GetConnectionString();

            Console.WriteLine("Welcome to Amandeep's Application!\n\n");
            Console.WriteLine("Already have an account? LogIn. (Enter 1)\n");
            Console.WriteLine("Dont have an account? SignUp. (Enter 2)\n");
            int z = int.Parse(Console.ReadLine());

            if (z == 1)
            { 
            while (choice == 0)
            {
                Console.WriteLine("\nPlease enter the credentials to log in. \n(For testing purposes, Username: Testing || Password: Tsigret \n");
                Console.WriteLine("Please enter username: ");
                u_name = Console.ReadLine();
                while (u_name == "")
                {
                    Console.WriteLine("Please enter username: ");
                    u_name = Console.ReadLine();
                }
                Console.WriteLine("\nEnter password: ");
                pass = Console.ReadLine();
                while (pass == "")
                {
                    Console.WriteLine("\nEnter password: ");
                    pass = Console.ReadLine();
                }

                string s = GetConnectionString();
                scon = new SqlConnection(s);
                string query = "select * from Customer_Detail";

                da = new SqlDataAdapter(query, scon);

                da.Fill(ds);

                // Add  a Primary key to DataTable
                DataColumn[] pk = new DataColumn[1];
                pk[0] = ds.Tables[0].Columns[0];
                ds.Tables[0].PrimaryKey = pk;

                int opt = 0;
                int topo = 0;
                foreach (DataTable dt in ds.Tables)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (String.Compare(dr[0].ToString(), u_name) == 0 && String.Compare(dr[1].ToString(), pass) == 0)
                        {

                            topo = 1;

                        }
                        else if (String.Compare(dr[0].ToString(), u_name) == 1 && String.Compare(dr[1].ToString(), pass) == 1)
                        {

                            opt = 0;

                        }
                    }
                }

                if (topo == 1)
                {
                    opt = 1;
                }

                if (opt == 1)
                {
                    Console.WriteLine("\nWelcome " + u_name);
                    int g = 0;
                    while (g == 0)
                    {
                        Console.WriteLine("\nTasks available: ");
                        Console.WriteLine("1. Edit Customer Profile\n2. Buy Products\n3. Logout\n4. Exit Application\n");
                        Console.WriteLine("\nSelect one of the following options: ");
                        input = int.Parse(Console.ReadLine());

                        switch (input)
                        {
                            case 1:
                                EditRecord(u_name);
                                break;
                            case 2:
                                BuyProduct(u_name);
                                break;
                            case 3:
                                g = 1;
                                break;
                            case 4:
                                g = 1;
                                choice = 1;
                                break;
                        }
                    }
                }
                else if (opt == 0)
                {
                    Console.WriteLine("Invalid username and password!");
                    Console.WriteLine("Don't have an account? SignUp! (Y/N) : ");
                    string sup = Console.ReadLine();
                    if (string.Compare(sup, "Y") == 1)
                    {
                        signUp();
                    }
                    else if (string.Compare(sup, "N") == 1)
                        continue;
                    else
                        continue;

                }
            }
        }
            else if (z == 2)
            {
                signUp();
            }

        }


        static void EditRecord(string name)
        {
            int a = 0, b = 0, limit = 0;
            string city = "";

            while (b == 0)
            {
                Console.WriteLine("Enter new city name: ");
                city = Console.ReadLine();
                if (city.All(char.IsLetter))
                {
                    b = 1;
                }
                else
                    Console.WriteLine("\nInvalid input. Please retry!");
            }
            while (a == 0)
            {
                Console.WriteLine("Enter new credit limit (1000-5000): ");
                limit = int.Parse(Console.ReadLine());
                if (limit > 1000 && limit < 5000)
                {
                    a = 1;
                }
                else
                    Console.WriteLine("\nInvalid input. Please retry!");
            }

            string s = GetConnectionString();
            scon = new SqlConnection(s);
            
            using (SqlConnection scon = new SqlConnection(s))
            {
                
                SqlCommand cmd = scon.CreateCommand();
                cmd.CommandText = "update Customers set City = @city, CreditLimit=@limit where CustFirstName=@name";
                cmd.Parameters.AddWithValue("city", city);
                cmd.Parameters.AddWithValue("limit", limit);
                cmd.Parameters.AddWithValue("name", name);
                scon.Open();
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Record updated!");

        }
        static void BuyProduct(string top)
        {
            string conn = GetConnectionString();

            using (SqlConnection scon = new SqlConnection(conn))
            {

                //Create command object
                SqlCommand cmd = scon.CreateCommand();

                //Open connection
                scon.Open();

                string query = "select Distinct ProductType from Product where ProductType is not NULL";  //alternate to semi colons confusion
                cmd.CommandText = query;

                //Execute the command
                SqlDataReader rd = cmd.ExecuteReader();
                int i = 1;
                String[] arr = new string[5];
                //Process results returned
                while (rd.Read())
                {
                    string m_name = rd[0].ToString();

                    Console.WriteLine(i + ". " + m_name + "\n");
                    arr[i] = m_name;
                    i++;
                }
                scon.Close();
                scon.Open();
                Console.WriteLine("\nEnter product type number: ");
                int prodNum = int.Parse(Console.ReadLine());
                SqlCommand cmd1 = scon.CreateCommand();
                string query1 = "select Descr from Product where ProductType='" + arr[prodNum].ToString() + "'";  //alternate to semi colons confusion
                cmd1.CommandText = query1;

                //Execute the command
                SqlDataReader rd1 = cmd1.ExecuteReader();
                int j = 1;
                String[] arr1 = new string[5];
                //Process results returned
                while (rd1.Read())
                {
                    string p_name = rd1[0].ToString();

                    Console.WriteLine(j + ". " + p_name + "\n");
                    arr1[j] = p_name;
                    j++;
                }
                scon.Close();
                scon.Open();
                Console.WriteLine("Enter product number: ");
                int num = int.Parse(Console.ReadLine());
                SqlCommand cmd2 = scon.CreateCommand();
                string query2 = "select * from Product where Descr='" + arr1[num] + "'";  //alternate to semi colons confusion
                cmd2.CommandText = query2;

                //Execute the command
                SqlDataReader rd2 = cmd2.ExecuteReader();
                int prod_num = 0;
                string price = "";
                int q = 0;
                //Process results returned
                while (rd2.Read())
                {
                    prod_num = (int)rd2[0];
                    price = rd2[3].ToString();
                    q = (int)rd2[4];
                    Console.WriteLine("The Price of " + arr1[num] + " is: $" + price + "\n");
                }

                int o = 0, quan = 0, w = 0, cust_no;
                
                while (o == 0 || w == 0)
                {
                    Console.WriteLine("Enter quantity you want to buy: ");
                    quan = int.Parse(Console.ReadLine());
                    
                    if (quan <= 0)
                    {
                        Console.WriteLine("Please enter a positive integer: ");
                        continue;
                    }
                    else
                    {
                        o = 1;
                        
                    }
                       
                    if (q >= quan)
                    {
                        w = 1;
                       cust_no = addRecordsSales(top);
                        addRecordsInvoice(top, cust_no, prod_num, quan, price, q);
                    }
                    else
                    {
                        Console.WriteLine("We dont have the required number of items onhand. Please enter a new number: ");
                        w = 0;
                        continue;
                    }
                }
                scon.Close();
            }
        }

        static int addRecordsSales(string u_name)
        {

            string conn = GetConnectionString();
            int cust_num = 0;
            using (SqlConnection scon = new SqlConnection(conn))
            {

                //Create command object
                SqlCommand cmd = scon.CreateCommand();

                //Open connection
                scon.Open();

                string query = "select * from Customers where CustFirstName='" + u_name + "'";  //alternate to semi colons confusion
                cmd.CommandText = query;

                //Execute the command
                SqlDataReader rd = cmd.ExecuteReader();

                //Process results returned
                while (rd.Read())
                {
                    cust_num = (int)rd[0];
                }
                
                scon.Close();
            }

            
            Sales2(cust_num);
            return (cust_num);
        }
        static void Sales2(int cust_num)
        {
            string conn = GetConnectionString();
            GetData1();

            DataRow dr = ds.Tables[0].NewRow();
            DateTime date = DateTime.Now;
            dr[3] = date;
            dr[4] = 1;
            dr[5] = cust_num;

            ds.Tables[0].Rows.Add(dr);

            SqlCommandBuilder cmd1 = new SqlCommandBuilder(da);
            da.InsertCommand = cmd1.GetInsertCommand();
            da.Update(ds.Tables[0]);

            Console.WriteLine("Record has been added!\n");

        }

        static void GetData1()
        {
            string s = GetConnectionString();
            scon = new SqlConnection(s);
            string query1 = "select * from Sales";
            da = new SqlDataAdapter(query1, scon);
            ds.EnforceConstraints = false;
            da.Fill(ds);

            DataColumn[] pk = new DataColumn[1];
            pk[0] = ds.Tables[0].Columns[0];
            ds.Tables[0].PrimaryKey = pk;
        }

        static void addRecordsInvoice(string u_name, int cust_num, int prod_num, int quan, string price, int q)
        {
            string conn = GetConnectionString();
            int sale_num = 0;
            using (SqlConnection scon = new SqlConnection(conn))
            {

                //Create command object
                SqlCommand cmd = scon.CreateCommand();

                //Open connection
                scon.Open();

                string query1 = "select * from Sales where Custnum='" + cust_num + "'";  //alternate to semi colons confusion
                cmd.CommandText = query1;

                //Execute the command
                SqlDataReader rd = cmd.ExecuteReader();

                //Process results returned
                while (rd.Read())
                {
                    sale_num = (int)rd[0];
                }
                Console.WriteLine(sale_num);
                scon.Close();
            }
            
            Invoice2(u_name, sale_num, prod_num, quan, price, q);
        }

        static void Invoice2(string u_name, int sale_num, int prod_num, int quan, string price, int q)
        {
           


            string conn = GetConnectionString();

            using (SqlConnection scon = new SqlConnection(conn))
            {

                //Create command object
                SqlCommand cmd = scon.CreateCommand();

                //Open connection
                scon.Open();

                //Give command object -> command/query
                string query = "insert into SalesInvoice(SalesNum, ProductNum, Quantity, SalesPrice) values(@sale_num, @prod_num, @quan, @price)";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("sale_num", sale_num);
                cmd.Parameters.AddWithValue("prod_num", prod_num);
                cmd.Parameters.AddWithValue("quan", quan);
                cmd.Parameters.AddWithValue("price", price);

                //execute the command
                cmd.ExecuteNonQuery();
            }

                OnHand(prod_num, quan, q);

        }

        static void OnHand(int prod_num, int quan, int q)
        {
            string conn = GetConnectionString();
            using (SqlConnection scon = new SqlConnection(conn))
            {
                quan = q - quan;
                SqlCommand cmd = scon.CreateCommand();
                cmd.CommandText = "update Product set OnHand = @quan where ProductNum=@prod_num";
                cmd.Parameters.AddWithValue("quan", quan);
                cmd.Parameters.AddWithValue("prod_num", prod_num);
                scon.Open();
                cmd.ExecuteNonQuery(); 
            }
            Console.WriteLine("Quantity on hand updated!");
        }
        static void GetData()
        {
            
            string s = GetConnectionString();
            scon = new SqlConnection(s);
            string queryx = "select * from SalesInvoice";
            da = new SqlDataAdapter(queryx, scon);
            ds.EnforceConstraints = false;
            da.Fill(ds);
            //rename datatable
            ds.Tables[0].TableName = "SalesInvoice";

            //Add a primary key to data table
            DataColumn[] pk = new DataColumn[1];
            pk[0] = ds.Tables[0].Columns[0];
            ds.Tables[0].PrimaryKey = pk;
        }
        static void signUp()
        {
           
            String fName="", lName="", city="";
            int limit = 0;
            Console.WriteLine("\nEnter your details: \n");

            Console.WriteLine("Enter first name: ");
            fName = Console.ReadLine();
            while (fName.All(char.IsDigit) == true || string.IsNullOrEmpty(fName))
            {
                Console.WriteLine("\nInvalid input! Please enter first name again!\n"); 
                fName = Console.ReadLine();
            }

            Console.WriteLine("\nEnter last name: ");
            lName = Console.ReadLine();
            while (lName.All(char.IsDigit) == true || string.IsNullOrEmpty(lName))
            {
                Console.WriteLine("\nInvalid input! Please enter last name again!\n");
                lName = Console.ReadLine();
            }

            Console.WriteLine("\nEnter city: ");
            city = Console.ReadLine();
            while (city.All(char.IsDigit) == true || string.IsNullOrEmpty(city))
            {
                Console.WriteLine("\nInvalid input! Please enter city name again!");
                city = Console.ReadLine();
            }

            Console.WriteLine("\nEnter credit limit: ");
            String l = Console.ReadLine();
            bool a = int.TryParse(l, out limit);
            while (string.IsNullOrEmpty(l) || a == false)
            {
                Console.WriteLine("\nInvalid input! Enter credit limit again!");
                limit = int.Parse(Console.ReadLine());
            }

            string conn = GetConnectionString();

            using (SqlConnection scon = new SqlConnection(conn))
            {

                //Create command object
                SqlCommand cmd = scon.CreateCommand();

                //Open connection
                scon.Open();

                //Give command object -> command/query
                string query = "insert into Customers(CustFirstName, CustLastName, City, CreditLimit) values(@fName, @lName, @city, @limit)";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("fname", fName);
                cmd.Parameters.AddWithValue("lname", lName);
                cmd.Parameters.AddWithValue("city", city);
                cmd.Parameters.AddWithValue("limit", limit);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Record has been inserted!\n");

                newUserPass(fName, lName);
            }
        }

        static void newUserPass(string fName, string lName)
        {
            String user_name = fName;
            String password = "";
            char[] charArr1 = fName.ToCharArray();
            int i = 0, j = 0;
            char[] pass = new Char[10];
            foreach (char ch in charArr1)
            {
                if (i % 2 == 0)
                {
                    pass[j] = ch;
                    j++;
                }
                i++;
            }
            char[] charArr2 = lName.ToCharArray();
            int x = charArr2.Length;
            for (int h = x - 1; h >= x - 3; h--)
            {
                pass[j] = charArr2[h];
                j++;
            }
            password = new string(pass);

            string conn = GetConnectionString();

            using (SqlConnection scon = new SqlConnection(conn))
            {

                //Create command object
                SqlCommand cmd = scon.CreateCommand();

                //Open connection
                scon.Open();

                //Give command object -> command/query
                string query = "insert into Customer_Detail(CustUserName, CustPassword) values(@fName, @password)";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("fname", fName);
                cmd.Parameters.AddWithValue("password", password);
                cmd.ExecuteNonQuery();
                
            }
        }

        static string GetConnectionString()
        {
            ConfigurationBuilder cb = new ConfigurationBuilder();

            cb.SetBasePath(Directory.GetCurrentDirectory());

            cb.AddJsonFile("config.json");

            IConfiguration config = cb.Build();

            return config["ConnectionString:FinalExamMdf"].ToString();
        }

    }
}

