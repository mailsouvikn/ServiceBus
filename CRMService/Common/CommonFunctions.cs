using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using System.Web.Script.Serialization;


namespace FHLB.CRMService.Shared
{
    public static class CommonFunctions
    {
        public static void LogInConsole(string input)
        {
            Console.WriteLine(DateTime.Now.ToString() + " : " + input);
        }

        public static string GetCustNumFromMessage(string message)
        {
            if (message.IndexOf('@') < 0 || message.IndexOf('=') < 0)
            {
                LogInConsole("Invalid Message : " + message);
                return "";
            }
            return message.Split(new char[] { '@' })[0].Split(new char[] { '=' })[1];
        }

        public static DataTable LoadDataFromCSV(string filepath, string sheetname)
        {

            DataTable dt = new DataTable(sheetname);
            using (StreamReader sr = new StreamReader(filepath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }
            LogInConsole("Data read for CSV file");
            return dt;
        }

        public static string GetCustomerJson(string custNum)
        {
            CustomerDto cust = new CustomerDto()
            {
                Id = 1,
                AccountNumber = 50105560,
                Asset = 4500000,
                Balance = 340000,
                CustNum = custNum,
                FirstName = "John",
                LastName = "Smith",
                Email = "jsmith@email.com"
            };

            return new JavaScriptSerializer().Serialize(cust);
        }


    }
}
