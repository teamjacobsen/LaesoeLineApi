namespace LaesoeLineApi
{
    public enum Vehicle
    {
        [VehicleValue("none", "", seasonPassOptionValue: "")]
        None,

        [VehicleValue("car", "19", seasonPassOptionValue: "319", includeInAvailability: true)]
        Car,

        [VehicleValue("car+shorttrailer12", "CR46")]
        CarShortTrailer12,

        [VehicleValue("car+mediumtrailer12", "CRM46")]
        CarMediumTrailer12,

        [VehicleValue("car+talltrailer12", "CRH46", includeInAvailability: true)]
        CarTallTrailer12,

        [VehicleValue("van", "21", includeInAvailability: true)]
        Van
    }
}