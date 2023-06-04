namespace trocitos.mvc.DTOs
{
    public class AvailabilityCheck
    {
        public int ReqPartySize { get; set; }
        public DateTime ReqDate { get; set; }
        public TimeOnly ReqStartTime { get; set; }
        public bool ReqOutside { get; set; }
    }
}