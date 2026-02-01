using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Hosting;

namespace FeritBulut.Maui.BottomSheet.Extensions
{
    /// <summary>
    /// Extension methods for adding the Bottom Sheet library to the application.
    /// </summary>
    public static class BottomSheetExtensions
    {
        /// <summary>
        /// Adds Bottom Sheet controls to the application.
        /// </summary>
        /// <param name="builder">MauiAppBuilder instance</param>
        /// <returns>MauiAppBuilder</returns>
        public static MauiAppBuilder UseBottomSheet(this MauiAppBuilder builder)
        {
            // Handlers or additional configurations may be added here in the future.
            return builder;
        }
    }
}
