﻿namespace LaesoeLineApi
{
    public interface ILaesoeLineApiClient
    {
        IAgentBookingApi AgentBooking { get; }
        ICustomerBookingApi CustomerBooking { get; }
        IMyBookingApi MyBooking { get; }
        ITimetableApi Timetable { get; }

        void SetAuthorization(string username, string password);
    }
}
