namespace LaesoeLineApi.CustomerBooking
{
    public class CustomerBookingBookOneWay
    {
        public CustomerBookingJourney Journey { get; set; } = new CustomerBookingJourney();
        public bool Local { get; set; }
    }
}
