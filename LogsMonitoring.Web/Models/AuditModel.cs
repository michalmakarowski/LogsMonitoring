using Dapper.Contrib.Extensions;

namespace LogsMonitoring.Web.Models
{
    [Table("AuditLogs")]
    public class AuditModel
    {
        public int Id { get; set; }
        public string OperationType { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string ExecutingUser { get; set; }
    }
}
