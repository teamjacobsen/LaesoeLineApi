namespace LaesoeLineApi
{
    public enum Vehicle
    {
        [VehicleValue("", seasonPassOptionValue: "")]
        None,

        [VehicleValue("19", seasonPassOptionValue: "319", includeInAvailability: true)]
        Car,

        [VehicleValue("CR46")]
        CarShortTrailer12,

        [VehicleValue("CRM46")]
        CarMediumTrailer12,

        [VehicleValue("CRH46", includeInAvailability: true)]
        CarTallTrailer12,

        [VehicleValue("21")]
        Van
    }
}