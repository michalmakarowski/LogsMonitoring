using LogsMonitoring.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace LogsMonitoring.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            List<RouteValueDictionary> dataBaseList = DataBaseList();
            return View(dataBaseList);
        }

        public List<RouteValueDictionary> DataBaseList() 
        {
            List<RouteValueDictionary> list = new List<RouteValueDictionary>();

            string conString = "Server=DESKTOP-18IL65C\\SQLEXPRESS;Database=Test;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                // Get the schema information for the database
                DataTable schema = con.GetSchema("Tables");

                // Extract the table names from the schema
                foreach (DataRow row in schema.Rows)
                {
                    string tableName = (string)row["TABLE_NAME"];
                    RouteValueDictionary tableData = new RouteValueDictionary();
                    tableData["TableName"] = tableName;
                    list.Add(tableData);
                }

            }

            return list;
        }

        public ActionResult Employees()
        {
            // Odczytaj logi z pliku tekstowego
            string logFilePath = @"D:\LogsMonitoring\LogsMonitoring.Web\LogsMonitoring.Web\App_data\EmployeeLog.txt";
            string[] logs = System.IO.File.ReadAllLines(logFilePath);

            // Przekazanie logów do widoku
            ViewBag.EmployeeLogs = logs;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}