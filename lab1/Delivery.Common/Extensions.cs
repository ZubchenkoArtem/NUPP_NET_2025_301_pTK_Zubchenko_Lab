using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Common
{
    public static class Extensions
    {
        // метод розширення для Vehicle
        public static void PrintLicensePlate(this Vehicle vehicle)
        {
            Console.WriteLine($"Vehicle plate: {vehicle.LicensePlate}");
        }
    }
}