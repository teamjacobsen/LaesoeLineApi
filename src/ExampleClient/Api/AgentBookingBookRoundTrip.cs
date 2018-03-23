namespace ExampleClient.Api
{
    public class AgentBookingBookRoundTrip
    {
        public AgentBookingCustomer Customer { get; set; } = new AgentBookingCustomer();
        public AgentBookingJourney Outbound { get; set; } = new AgentBookingJourney();
        public AgentBookingJourney Return { get; set; } = new AgentBookingJourney();
    }
}
