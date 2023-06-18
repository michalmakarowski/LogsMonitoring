using System.ComponentModel.DataAnnotations;

namespace LogsMonitoring.Web.Models
{
    public class EmailModel
    {
        //[Required(ErrorMessage = "Pole Od jest wymagane.")]
        //[EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
        //public string FromEmail { get; set; }

        [Required(ErrorMessage = "Pole Do jest wymagane.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
        public string ToEmail { get; set; }

        public string TableName { get; set; }

        //[Required(ErrorMessage = "Pole Temat jest wymagane.")]
        //public string Subject { get; set; }

        //[Required(ErrorMessage = "Pole Treść jest wymagane.")]
        //public string Body { get; set; }
    }
}


