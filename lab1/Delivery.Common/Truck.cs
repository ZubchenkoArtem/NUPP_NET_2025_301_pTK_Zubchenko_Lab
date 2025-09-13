using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Common
{
    public class Truck : Vehicle
    {
        // конструктор
        public Truck()
        {
            NumberOfAxles = 2;
            FuelConsumption = 20;
            HasRefrigeration = false;
        }

        public int NumberOfAxles { get; set; }
        public double FuelConsumption { get; set; } // літр/100 км
        public bool HasRefrigeration { get; set; }

        // метод
        public void Drive(double distanceKm)
        {
            Console.WriteLine($"Truck {LicensePlate} drove {distanceKm} km consuming {FuelConsumption * distanceKm / 100} liters of fuel.");
        }

        // делегат та подія
        public delegate void RefrigerationStatusChangedHandler(bool status);
        public event RefrigerationStatusChangedHandler OnRefrigerationStatusChanged;

        public void ChangeRefrigerationStatus(bool status)
        {
            HasRefrigeration = status;
            OnRefrigerationStatusChanged?.Invoke(status);
        }
    }
}