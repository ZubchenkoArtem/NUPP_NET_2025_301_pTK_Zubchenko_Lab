using System;

namespace Delivery.Infrastructure.Models
{
    // Наслідуємо загальні властивості від VehicleModel
    public class TruckModel : VehicleModel
    {
        // НЕ дублюємо FuelConsumption / MaxLoadKg / LicensePlate — вони вже в VehicleModel
        public bool HasRefrigeration { get; set; }
        public int NumberOfAxles { get; set; }
        // інші специфічні властивості для TruckModel (за потреби)
    }
}
