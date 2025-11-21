using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace Delivery.Common.Entities
{
    // клас користувача для Identity
    public class DeliveryUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
