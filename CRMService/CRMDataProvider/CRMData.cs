using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHLB.CRMService.Shared;

namespace FHLB.CRMDataProvider
{
    public static class CRMData
    {       
        const string _connectionString = "data source=localhost;initial catalog=AzurePOC;integrated security=True;MultipleActiveResultSets=True;App=POCCRMApp";

        public static bool LoadDataInCrmTable(DataTable source)
        {
            bool isSuccess = false;
            bool exists = false;
            int rowNum = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                String queryInsert = @"INSERT INTO [dbo].[TestCRMSource]
           ([CustNum] ,[FirstName]  ,[LastName] ,[EmailAddress]  ,[AccountNumber]  ,[CurrentBalance] ,[CurrentAssets] ,[CreatedOn] ,[StatusCode]) 
            VALUES (@custnum,@fname, @lname, @email, @acctnum, @curbal, @curasset, @created, @status)";

                string querySelect = "Select id From [dbo].[TestCRMSource] Where CustNum=@custnum";
                string queryUpdate = "Update [dbo].[TestCRMSource] SET StatusCode='I', ModifiedOn=@modifiedon Where CustNum=@custnum AND StatusCode = 'A'";
                connection.Open();

                foreach (DataRow item in source.Rows)
                {
                    rowNum++;
                    exists = false;
                    using (SqlCommand commandSelect = new SqlCommand(querySelect, connection))
                    {
                        commandSelect.Parameters.AddWithValue("@custnum", item["CustNum"]);

                        exists = !string.IsNullOrEmpty(Convert.ToString(commandSelect.ExecuteScalar()));
                        if (exists)
                        {
                            //delete existing
                            using (SqlCommand commandUpdate = new SqlCommand(queryUpdate, connection))
                            {
                                commandUpdate.Parameters.AddWithValue("@custnum", item["CustNum"]);
                                commandUpdate.Parameters.AddWithValue("@modifiedon", DateTime.Now);
                                int result = commandUpdate.ExecuteNonQuery();

                                // Check Error
                                if (result < 0)
                                    CommonFunctions.LogInConsole("Error inserting data into Database for row# " + rowNum);
                            }

                        }

                        //insert
                        using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                        {
                            commandInsert.Parameters.AddWithValue("@custnum", item["CustNum"]);
                            commandInsert.Parameters.AddWithValue("@fname", item["Fname"]);
                            commandInsert.Parameters.AddWithValue("@lname", item["LName"]);
                            commandInsert.Parameters.AddWithValue("@email", item["Email"]);
                            commandInsert.Parameters.AddWithValue("@acctnum", item["AcctNum"]);
                            commandInsert.Parameters.AddWithValue("@curbal", item["Balance"]);
                            commandInsert.Parameters.AddWithValue("@curasset", item["Asset"]);
                            commandInsert.Parameters.AddWithValue("@created", DateTime.Now);
                            commandInsert.Parameters.AddWithValue("@status", "A");
                            int result = commandInsert.ExecuteNonQuery();

                            // Check Error and log
                            if (result < 0)
                                CommonFunctions.LogInConsole("Error inserting data into Database for row# " + rowNum);
                        }
                    }
                }
                connection.Close();

            }

            CommonFunctions.LogInConsole("Data loaded in CRM Table");
            return isSuccess;
        }
    }
}
