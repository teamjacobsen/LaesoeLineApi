namespace LaesoeLineApi.AgentBooking
{
    public class AgentBookingBookRoundTrip
    {
        public Customer Customer { get; set; } = new Customer();
        public Journey Outbound { get; set; } = new Journey();
        public Journey Return { get; set; } = new Journey();
    }
}
