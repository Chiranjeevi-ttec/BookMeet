namespace BookMeet.API.APIModel
{
    public class ScheduleRequest
    {
        public string? Mail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AvailabilityViewInterval { get; set; }
    }
}
