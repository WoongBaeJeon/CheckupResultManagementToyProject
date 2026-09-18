using System.Collections.Generic;

namespace infoCheckupResult.Models
{
    public class ReceptionResultData
    {
        public ReceptionResultData()
        {
            Results = new List<CheckupResultDto>();
            CommonCodes = new List<CommonCodeDto>();
        }

        public ReceptionHeaderDto Header { get; set; }
        public IList<CheckupResultDto> Results { get; private set; }
        public IList<CommonCodeDto> CommonCodes { get; private set; }
    }
}
