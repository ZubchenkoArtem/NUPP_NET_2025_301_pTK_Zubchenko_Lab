using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Common
{
    public class Vehicle
    {
        // статичне поле
        public static int VehicleCount;

        // конструктор статичний
        static Vehicle()
        {
            VehicleCount = 0;
        }

        // конструктор
        public Vehicle()
        {
            Id = Guid.NewGuid();
            VehicleCount++;
        }

        public Guid Id { get; set; }
        public string LicensePlate { get; set; }
        public double MaxLoadKg { get; set; }
        public double CurrentSpeed { get; set; }

        // метод
        public void DisplayInfo()
        {
            Console.WriteLine($"Vehicle {Id}, Plate: {LicensePlate}, Max Load: {MaxLoadKg}kg");
        }

        // статичний метод
        public static void ShowVehicleCount()
        {
            Console.WriteLine($"Total Vehicles: {VehicleCount}");
        }
    }
}