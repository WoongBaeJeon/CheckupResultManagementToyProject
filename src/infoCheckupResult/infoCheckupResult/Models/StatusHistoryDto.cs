using System;

namespace infoCheckupResult.Models
{
    public class StatusHistoryDto
    {
        public long StatusHistId { get; set; }
        public int ReceptionId { get; set; }
        public string BeforeStatus { get; set; }
        public string BeforeStatusName { get; set; }
        public string AfterStatus { get; set; }
        public string AfterStatusName { get; set; }
        public string ProcessType { get; set; }
        public string ProcessTypeName { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string ProcessedBy { get; set; }
    }
}
