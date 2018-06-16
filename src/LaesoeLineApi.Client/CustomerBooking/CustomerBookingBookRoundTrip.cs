namespace LaesoeLineApi.CustomerBooking
{
    public class CustomerBookingBookRoundTrip
    {
        public CustomerBookingJourney Outbound { get; set; } = new CustomerBookingJourney();
        public CustomerBookingJourney Return { get; set; } = new CustomerBookingJourney();
        public bool Local { get; set; }
    }
}
