using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeritBulut.Maui.BottomSheet.Enums;

namespace FeritBulut.Maui.BottomSheet.EventArgs
{
    /// <summary>
    /// Arguments for drag events
    /// </summary>
    public class BottomSheetDragEventArgs : System.EventArgs
    {
        /// <summary>
        /// Current Y position
        /// </summary>
        public double TranslationY { get; }

        /// <summary>
        /// Drag direction
        /// </summary>
        public GestureDirection Direction { get; }

        /// <summary>
        /// Drag speed (velocity)
        /// </summary>
        public double Velocity { get; }

        /// <summary>
        /// Current height ratio (0-1)
        /// </summary>
        public double CurrentRatio { get; }

        public BottomSheetDragEventArgs(double translationY, GestureDirection direction, double velocity, double currentRatio)
        {
            TranslationY = translationY;
            Direction = direction;
            Velocity = velocity;
            CurrentRatio = currentRatio;
        }
    }
}
