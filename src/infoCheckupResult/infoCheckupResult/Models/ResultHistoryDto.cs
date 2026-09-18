using System;

namespace infoCheckupResult.Models
{
    public class ResultHistoryDto
    {
        public long ResultHistId { get; set; }
        public int ReceptionId { get; set; }
        public int CheckupClassId { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int CheckupItemGroupId { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int CheckupItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BeforeValue { get; set; }
        public string AfterValue { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string ProcessedBy { get; set; }
    }
}
