namespace LaesoeLineApi
{
    public enum ApiStatus
    {
        Success,

        VehicleNotFound,
        VehicleNotValid,
        DepartureNotFound,
        OutboundDepartureNotFound,
        ReturnDepartureNotFound,

        GatewayTimeout
    }
}
