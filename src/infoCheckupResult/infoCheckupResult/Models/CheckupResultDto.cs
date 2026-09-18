using System;

namespace infoCheckupResult.Models
{
    public class CheckupResultDto
    {
        public int CheckupClassId { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int ClassSortOrder { get; set; }
        public int CheckupItemGroupId { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int GroupSortOrder { get; set; }
        public int CheckupItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string InputType { get; set; }
        public int? CodeGroupId { get; set; }
        public string Unit { get; set; }
        public string RequiredYn { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public int? MaxLength { get; set; }
        public int ItemSortOrder { get; set; }
        public long? ResultId { get; set; }
        public string ResultValue { get; set; }
        public string OriginalResultValue { get; set; }
        public string DisplayResultValue { get; set; }
        public ResultJudgementStatus JudgementStatus { get; set; }
        public string JudgementText { get; set; }
        public string JudgementReason { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
