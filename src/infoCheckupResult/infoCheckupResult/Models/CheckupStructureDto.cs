namespace infoCheckupResult.Models
{
    public class CheckupStructureDto
    {
        public int CheckupClassId { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int ClassSortOrder { get; set; }
        public int CheckupItemGroupId { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int GroupSortOrder { get; set; }
    }
}
