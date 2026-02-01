using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeritBulut.Maui.BottomSheet.Enums
{

    /// <summary>
    /// Defines the opening states of the Bottom Sheet.
    /// </summary>

    public enum BottomSheetState
    {
        /// <summary>
        /// Bottom Sheet is completely covered (invisible)
        /// </summary>
        Hidden = 0,

        /// <summary>
        /// The bottom sheet is minimally open (only the handle is visible or a small portion of it).
        /// </summary>
        Collapsed = 1,

        /// <summary>
        /// Bottom Sheet is semi-open (approximately half the screen)
        /// </summary>
        HalfExpanded = 2,

        /// <summary>
        /// Bottom Sheet fully open (covers most of the screen)
        /// </summary>
        Expanded = 3
    }
}
