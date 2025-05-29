using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ClickAndEat.Helpers
{
    public static class SecureStringHelper
    {
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToBSTR(securePassword);
                return Marshal.PtrToStringBSTR(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(unmanagedString);
            }
        }
    }
}
