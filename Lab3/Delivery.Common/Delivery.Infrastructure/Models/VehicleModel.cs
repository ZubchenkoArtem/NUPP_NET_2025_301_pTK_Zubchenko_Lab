public class VehicleModel
{
    public Guid Id { get; set; } = Guid.NewGuid(); // ключ тут
    public string LicensePlate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public double MaxLoadKg { get; set; }
    public double FuelConsumption { get; set; }
    public bool IsOperational { get; set; } = true;
}

// Похідні класи не мають [Key]!
public class TruckModel : VehicleModel
{
    public bool HasRefrigeration { get; set; }
    public int NumberOfAxles { get; set; }
}

public class DroneModel : VehicleModel
{
    public double BatteryLevel { get; set; }
    public double MaxPayloadKg { get; set; }
    public double MaxFlightTimeMinutes { get; set; }
    public double MaxAltitudeMeters { get; set; }
}
