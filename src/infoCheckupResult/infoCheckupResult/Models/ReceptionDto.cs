using System;

namespace infoCheckupResult.Models
{
    public class ReceptionDto
    {
        public int ReceptionId { get; set; }
        public DateTime ReceptionDate { get; set; }
        public int PatientId { get; set; }
        public string ChartNo { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string SocialNumber { get; set; }
        public string MaskedSocialNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SocialNumber))
                    return string.Empty;

                string value = SocialNumber.Replace("-", "");

                if (value.Length != 13)
                    return SocialNumber;

                return value.Substring(0, 6)
                    + "-"
                    + value.Substring(6, 7);
            }
        }
        public short? PatientAge { get; set; }
        public string ResultStatus { get; set; }
        public string ResultStatusName { get; set; }
        public string ReceptionMemo { get; set; }

        
    }
}
