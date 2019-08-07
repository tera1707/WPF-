using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvSensorLibrary
{
    public static class Extensions
    {
        public static string toEnvSensorUUID(this string omittedUuid)
        {
            return "0C4C" + omittedUuid + "-7700-46F4-AA96-D5E974E32A54";
        }
    }
}
