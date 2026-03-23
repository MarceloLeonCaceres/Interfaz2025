using System;

namespace Domain
{
    public class MarcacionCheckInOut
    {
        public string UserId { get; set; }
        public DateTime VerifyDate { get; set; }
        public string VerifyType { get; set; }
        public int VerifyState { get; set; }
        public int WorkCode { get; set; }
        public string Sn { get; set; }
    }
}
