using LogsMonitoring.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Dapper.Contrib.Extensions;
using System.Text;

namespace LogsMonitoring.Web.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private string _conString;
        private string _fromMail;
        private List<AuditModel> audits = new List<AuditModel>();
        public HomeController(IConfiguration configuration) 
        {
            _configuration = configuration;
            _conString = _configuration.GetConnectionString("myNorthWindDB");
            _fromMail = "LogsMonitoringAd@gmail.com";
        }

        public IActionResult Index()
        {
            List<RouteValueDictionary> dataBaseList = DataBaseList();
            return View(dataBaseList);
        }

        public List<RouteValueDictionary> DataBaseList() 
        {
            List<RouteValueDictionary> list = new List<RouteValueDictionary>();


            using (SqlConnection con = new SqlConnection(_conString))
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
            
            using (IDbConnection con = new SqlConnection(_conString))
            {
                audits = con.GetAll<AuditModel>().Where(x=>x.Message.Contains("Employees")).ToList();
                ViewBag.EmployeeLogs = audits;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SendEmail(EmailModel emailModel)
        {
            using (IDbConnection con = new SqlConnection(_conString))
            {
                audits = con.GetAll<AuditModel>().ToList();
            }

            List<AuditModel> listaObiektow = audits;
            StringBuilder sb = new StringBuilder();

            foreach (var obiekt in listaObiektow)
            {
                sb.AppendLine($"{obiekt.Id} - {obiekt.OperationType} - {obiekt.Date} - {obiekt.Message} - {obiekt.ExecutingUser}");
            }

            string wynik = sb.ToString();

            if (ModelState.IsValid)
            {
                // Ustawienia serwera SMTP
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "LogsMonitoringAd@gmail.com";
                string smtpPassword = "euwnhzcozuaohtlc";

                // Tworzenie wiadomości e-mail
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_fromMail);
                mailMessage.To.Add(emailModel.ToEmail);
                mailMessage.Subject = "Employee audit logs";
                mailMessage.Body = wynik;

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