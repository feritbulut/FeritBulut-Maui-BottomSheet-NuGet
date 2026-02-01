using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeritBulut.Maui.BottomSheet.EventArgs
{
    /// <summary>
    /// Arguments for the event triggered when the snap point is reached.
    /// </summary>
    public class SnapPointReachedEventArgs : System.EventArgs
    {
        /// <summary>
        /// The reached snap point value
        /// </summary>
        public double SnapPoint { get; }

        /// <summary>
        /// Index of the snap point
        /// </summary>
        public int SnapPointIndex { get; }

        public SnapPointReachedEventArgs(double snapPoint, int snapPointIndex)
        {
            SnapPoint = snapPoint;
            SnapPointIndex = snapPointIndex;
        }
    }
}
