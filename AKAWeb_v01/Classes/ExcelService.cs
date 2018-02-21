using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AKAWeb_v01.Models;
using ExcelDataReader;

namespace AKAWeb_v01.Classes
{
    public class ExcelService
    {
        HashService hash_service = new HashService();
        public ExcelService()
        {
            string filePath = "C:\\AKACareerJobPostingsData2.xlsx";
            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            var reader = ExcelReaderFactory.CreateReader(stream);
            reader.Read();
            do
            {
                while (reader.Read())
                {
                    /* string name = reader.GetString(1) + " "+ reader.GetString(2);
                     string email = reader.GetString(16);
                     string password = hash_service.HashPassword("AKA_Temp_Password");
                     int access = 1;

                     string country = "US";
                     string state = reader.GetString(9);
                     string city = reader.GetString(8);
                     string zip = reader.GetString(10);
                     string street_address = reader.GetString(5) + " " + reader.GetString(6);

                     AddressModel address = new AddressModel(country, state, city, zip, street_address);

                     UserModel user = new UserModel(0, name, email, password, access, address);

                     CreateUser(user);
                    string email = reader.GetString(16);
                    string price = reader.GetDouble(23).ToString();
                    //string date = reader.GetString(26);
                    DateTime date = reader.GetDateTime(26);
                    string end_date = date.ToString("yyyy-mm-dd");
                    end_date = end_date.Replace("-00-", "-12-");
                    int product_id = GetMembershipIdByPrice(price);
                    int user_id = GetUserIdByEmail(email);

                    
                    AddProductToUser(user_id, product_id, end_date);*/

                    /*string institution = reader.GetString(5);
                    string inst_url = reader.GetString(7);
                    int bach = changeYNTo10(reader.GetString(11));
                    int ms = changeYNTo10(reader.GetString(12));
                    int phd = changeYNTo10(reader.GetString(13));
                    int asc = changeYNTo10(reader.GetString(10));

                    

                    int inst_id =addInstitution(institution, inst_url, bach, ms, phd, asc);

                    string department = reader.GetString(8);
                    string dept_website = reader.GetString(9);

                    int dept_id = addDepartment(department, dept_website);

                    int state_id = getStateIDFromText(reader.GetString(4));

                   bool success = addInstitutionHasDeptHasState(inst_id, dept_id, state_id);*/
                    string title = reader.GetString(0);
                    string category = reader.GetString(1);
                    string location = reader.GetString(2);
                    DateTime end_date = reader.GetDateTime(3);
                    string url = reader.GetString(4);
                    string submitted_by = reader.GetString(5);
                    string email = reader.GetString(6);
                    string institution = reader.GetString(7);
                    string department = reader.GetString(8);

                    JobModel job = new JobModel();
                    job.title_position = title;
                    job.category = category;
                    job.closing_date = end_date;
                    job.job_url = url;
                    job.senders_name = submitted_by;
                    job.email = email;
                    job.instintution_name = institution;
                    job.department_name = department;

                    addJobPosting(job);



                }
            } while (reader.NextResult());
        }

        private bool addJobPosting(JobModel job)
        {
            DBConnection testconn = new DBConnection();
            StringBuilder query = new StringBuilder("INSERT INTO Job_Posting(title, category, location, close_date, url, submitted_by, email, institution, department) VALUES (");
            query.Append("'");
            query.Append(job.title_position);
            query.Append("','");
            query.Append(job.category);
            query.Append("','");
            //location is the same as institution
            query.Append(job.instintution_name);
            query.Append("','");
            query.Append(job.closing_date);
            query.Append("','");
            query.Append(job.job_url);
            query.Append("','");
            query.Append(job.senders_name);
            query.Append("','");
            query.Append(job.email);
            query.Append("','");
            query.Append(job.instintution_name);
            query.Append("','");
            query.Append(job.department_name);
            query.Append("'");
            query.Append(")");



            bool success = testconn.WriteToTest(query.ToString());
            testconn.CloseConnection();
            return success;
        }

        private bool addInstitutionHasDeptHasState(int inst_id, int dept_id, int state_id)
        {
            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO Institution_Has_State_Has_Department(institution_id, department_id, state_id) " +
                "VALUES(" + inst_id + ", " + dept_id + ","+state_id+")";
            bool success = testconn.WriteToTest(query);
            testconn.CloseConnection();
            return success;
        }

        private int getStateIDFromText(string state)
        {
            DBConnection testconn = new DBConnection();
            string query = "SELECT id from States WHERE state_acronym = '" + state + "' OR state_name = '" + state + "'";
            SqlDataReader dataReader = testconn.ReadFromTest(query);
            int id = 0; 
            if (dataReader.HasRows)
            {
                dataReader.Read();

                id = Int32.Parse(dataReader.GetValue(0).ToString());
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return id;
        }

        private int addDepartment(string department, string dept_website)
        {
            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO Departments(department, department_website) " +
                "VALUES('" + department + "', '" + dept_website + "')";
            int id = testconn.WriteToTestReturnID(query);
            testconn.CloseConnection();
            return id;
        }

        private int addInstitution(string institution, string inst_url, int bach, int ms, int phd, int asc)
        {

            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO Institutions(institution, institution_website, bachelors, masters, doctoral, associates) " +
                "VALUES('" + institution + "', '" + inst_url + "', "+bach+","+ms+","+phd+","+asc+")";
            int id = testconn.WriteToTestReturnID(query);
            testconn.CloseConnection();
            return id;
        }

        private int changeYNTo10(string YN)
        {
            if (YN == "Yes" || YN == "YES" || YN == "yes" || YN == "y" || YN == "Y")
            {
                return 1;
            }
            else if (YN == "No" || YN == "NO" || YN == "no" || YN == "n" || YN == "N" || YN=="" || YN == " ")
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        private bool addState(string acronym, string name)
        {
            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO States(state_acronym, state_name) VALUES('" + acronym + "', '"+name+"')";
            bool result = testconn.WriteToTest(query);
            testconn.CloseConnection();
            return result;


        }

        private bool CreateUser(UserModel user)
        {
            bool created = false;
            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO Users(name, email, password, access) VALUES ('" + user.name + "', '" + user.email + "', '" + user.password + "'," + user.access + ")";
            string getId = "SELECT IDENT_CURRENT('Users')";
            SqlDataReader dataReader;
            if (testconn.WriteToTest(query))
            {
                dataReader = testconn.ReadFromTest(getId);
                dataReader.Read();
                int id = Int32.Parse(dataReader.GetValue(0).ToString());
                user.id = id;
                AddAddressToUser(user);
                created = true;
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return created;

        }

        private void AddAddressToUser(UserModel user)
        {
            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO User_Has_Address (country, state, city, street_address, zip, user_id) VALUES('" + user.address.country + "', '" + user.address.state + "'," +
"'" + user.address.city + "','" + user.address.street_address + "', '" + user.address.zip + "', " + user.id + ")";
            testconn.WriteToTest(query);
            testconn.CloseDataReader();
            testconn.CloseConnection();


        }

        private int GetMembershipIdByPrice(string price)
        {
            DBConnection testconn = new DBConnection();
            string query = "SELECT id from Products where type = 'Membership' AND cost = '" + price+"'";
            SqlDataReader dataReader = testconn.ReadFromTest(query);
            dataReader.Read();
            int product_id = Int32.Parse(dataReader.GetValue(0).ToString());

            testconn.CloseDataReader();
            testconn.CloseConnection();
            return product_id;
        }

        private int GetUserIdByEmail(string email)
        {
            DBConnection testconn = new DBConnection();
            string query = "SELECT id from Users WHERE email = '" + email+"'";
            SqlDataReader dataReader = testconn.ReadFromTest(query);
            dataReader.Read();
            int user_id = Int32.Parse(dataReader.GetValue(0).ToString());
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return user_id;
        }

     

        private void AddProductToUser(int user_id, int product_id, string end_date)
        {
            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO User_Has_Product (user_id, product_id, product_end, isValid) VALUES(" + user_id.ToString() + "," + product_id.ToString() + ", '" + end_date + "',1)";
            string b = "";
            testconn.WriteToTest(query);
            testconn.CloseConnection();
        }



    }
}