﻿using FHLB.AzureServiceManager;
using FHLB.CRMService.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonFunctions.LogInConsole("Customer Service Started");
            try
            {
                CommonFunctions.LogInConsole("Reading from Queue01...");
                string nextFromQueue01 = AzureProvider.ReadNextFromQueue01();
                CommonFunctions.LogInConsole("Next message from Queue01 : " + nextFromQueue01);
                string custNum = CommonFunctions.GetCustNumFromMessage(nextFromQueue01);
                if (!string.IsNullOrEmpty(custNum))
                {
                    var custJson = CommonFunctions.GetCustomerJson(custNum);
                    CommonFunctions.LogInConsole(custJson);
                }
                else
                {
                    CommonFunctions.LogInConsole("Customer Number not found");
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.LogInConsole(ex.Message);
                //CommonFunctions.LogInConsole(ex.StackTrace);
                CommonFunctions.LogInConsole("Next message from Queue01 Not Available");

            }
            finally
            {
                CommonFunctions.LogInConsole("Customer Service Ended");
            }
        }
    }
}
