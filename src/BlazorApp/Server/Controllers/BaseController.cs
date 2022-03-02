using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blazor.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string FarmId
        {
            get
            {
                var farmId = string.Empty;

                var cookie = Request.Cookies["FarmId"];

                if (!string.IsNullOrWhiteSpace(cookie))
                    farmId = cookie;

                return farmId;
            }
        }

        protected string FarmName
        {
            get
            {
                var farmName = string.Empty;

                var cookie = Request.Cookies["FarmName"];

                if (!string.IsNullOrWhiteSpace(cookie))
                    farmName = cookie;

                return farmName;
            }
        }

        protected string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        protected string TenantId
        {
            get
            {
                var storeId = User.FindFirstValue("TenantId");

                return storeId;
            }
        }

        protected Guid Guid()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        protected string GuidStr()
        {
            return Guid().ToString().ToLower();
        }
    }
}
