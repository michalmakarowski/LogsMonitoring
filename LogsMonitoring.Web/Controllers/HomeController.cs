using LogsMonitoring.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

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
            string logFilePath = @"D:\LogsMonitoring\LogsMonitoring.Web\LogsMonitoring.Web\App_data\EmployeeLog.txt";
            string[] logs = System.IO.File.ReadAllLines(logFilePath);

            ViewBag.EmployeeLogs = logs;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SendEmail(EmailModel emailModel)
        {
            if (ModelState.IsValid)
            {
                // Ustawienia serwera SMTP
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 465;
                string smtpUsername = "LogsMonitoringAd@gmail.com";
                string smtpPassword = "euwnhzcozuaohtlc";

                // Tworzenie wiadomości e-mail
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailModel.FromEmail);
                mailMessage.To.Add(emailModel.ToEmail);
                mailMessage.Subject = emailModel.Subject;
                mailMessage.Body = emailModel.Body;

                // Konfiguracja klienta SMTP
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                // Wysyłanie wiadomości e-mail
                try
                {
                    smtpClient.Send(mailMessage);
                    ViewBag.Message = "Wiadomość e-mail została wysłana.";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Wystąpił błąd podczas wysyłania wiadomości e-mail: " + ex.Message;
                }
            }

            return View();
        }

    }
}