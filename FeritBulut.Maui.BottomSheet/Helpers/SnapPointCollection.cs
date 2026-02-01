using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace FeritBulut.Maui.BottomSheet.Helpers
{
    /// <summary>
    /// Collection that manages Snap points
    /// </summary>
    public class SnapPointCollection : ObservableCollection<double>
    {
        /// <summary>
        /// It finds the snap point closest to the given value.
        /// </summary>
        /// <param name="value">Current value (ratio between 0-1)</param>
        /// <returns>The nearest snapshot point</returns>
        public double FindNearest(double value)
        {
            if (Count == 0)
                return value;

            double nearest = this[0];
            double minDistance = Math.Abs(value - nearest);

            foreach (var point in this)
            {
                var distance = Math.Abs(value - point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = point;
                }
            }

            return nearest;
        }

        /// <summary>
        /// It finds the snap point one level above the given value.
        /// </summary>
        public double? FindNext(double value)
        {
            var sorted = this.OrderBy(x => x).ToList();
            return sorted.FirstOrDefault(x => x > value + 0.01);
        }

        /// <summary>
        /// It finds the snap point one level below the given value.
        /// </summary>
        public double? FindPrevious(double value)
        {
            var sorted = this.OrderByDescending(x => x).ToList();
            return sorted.FirstOrDefault(x => x < value - 0.01);
        }

        /// <summary>
        /// Finds the index of the snap point.
        /// </summary>
        public int IndexOfNearest(double value)
        {
            var nearest = FindNearest(value);
            return IndexOf(nearest);
        }
    }
}
