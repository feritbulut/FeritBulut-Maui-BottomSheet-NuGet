using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeritBulut.Maui.BottomSheet.Enums
{
    /// <summary>
    /// Drag lock modes
    /// </summary>
    public enum GestureLockMode
    {
        /// <summary>
        /// No lock, can be freely dragged.
        /// </summary>
        None,

        /// <summary>
        /// It can only be dragged upwards.
        /// </summary>
        LockDown,

        /// <summary>
        /// It can only be dragged down.
        /// </summary>
        LockUp,

        /// <summary>
        /// Fully locked, no dragging allowed.
        /// </summary>
        Locked
    }
}
