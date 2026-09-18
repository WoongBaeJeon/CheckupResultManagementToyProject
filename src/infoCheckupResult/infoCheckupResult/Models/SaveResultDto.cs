namespace infoCheckupResult.Models
{
    public class SaveResultDto
    {
        public int ReceptionId { get; set; }
        public string ResultStatus { get; set; }
        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int DeletedCount { get; set; }
        public int SavedResultCount { get; set; }
    }
}
