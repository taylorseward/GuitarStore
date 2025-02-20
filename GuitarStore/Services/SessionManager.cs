using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace GuitarStore.Services
{
    public static class SessionManager
    {
        public static int? GetCurrentUserId()
        {
            // Retrieve the user ID from Preferences
            if (Preferences.ContainsKey("CurrentUserId"))
            {
                return Preferences.Get("CurrentUserId", -1);
            }
            return null;
        }

        public static void ClearSession()
        {
            // Remove the user ID from Preferences
            Preferences.Remove("CurrentUserId");
        }
    }
}
