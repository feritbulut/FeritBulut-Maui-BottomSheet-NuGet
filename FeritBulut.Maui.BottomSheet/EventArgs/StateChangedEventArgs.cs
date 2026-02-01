using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeritBulut.Maui.BottomSheet.Enums;

namespace FeritBulut.Maui.BottomSheet.EventArgs
{
    /// <summary>
    /// Arguments for the event triggered when the Bottom Sheet state changes.
    /// </summary>
    public class StateChangedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Previous situation
        /// </summary>
        public BottomSheetState OldState { get; }

        /// <summary>
        /// The new (current) situation
        /// </summary>
        public BottomSheetState NewState { get; }

        /// <summary>
        /// StateChangedEventArgs constructor
        /// </summary>
        /// <param name="oldState">Previous situation</param>
        /// <param name="newState">Next situation</param>
        public StateChangedEventArgs(BottomSheetState oldState, BottomSheetState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }
}
