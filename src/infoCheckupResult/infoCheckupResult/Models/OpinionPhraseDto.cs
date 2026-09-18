namespace infoCheckupResult.Models
{
    public class OpinionPhraseDto
    {
        public int OpinionPhraseId { get; set; }
        public int? CheckupItemId { get; set; }
        public string ScopeName { get; set; }
        public string PhraseName { get; set; }
        public string PhraseText { get; set; }
        public int SortOrder { get; set; }
    }
}
