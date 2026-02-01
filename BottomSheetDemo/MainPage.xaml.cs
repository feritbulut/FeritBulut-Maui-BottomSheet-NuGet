using FeritBulut.Maui.BottomSheet.Enums;
using FeritBulut.Maui.BottomSheet.EventArgs;

namespace BottomSheetDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeSnapPoints();
        }

        private void InitializeSnapPoints()
        {
            // Add custom points to the Snap Points Bottom Sheet.
            snapPointsBottomSheet.AddSnapPoint(0.20); // %20
            snapPointsBottomSheet.AddSnapPoint(0.40); // %40
            snapPointsBottomSheet.AddSnapPoint(0.60); // %60
            snapPointsBottomSheet.AddSnapPoint(0.80); // %80
        }

        #region Temel Bottom Sheet

        private void OnCollapsedClicked(object sender, EventArgs e)
        {
            basicBottomSheet.Show(BottomSheetState.Collapsed);
        }

        private void OnHalfExpandedClicked(object sender, EventArgs e)
        {
            basicBottomSheet.Show(BottomSheetState.HalfExpanded);
        }

        private void OnExpandedClicked(object sender, EventArgs e)
        {
            basicBottomSheet.Show(BottomSheetState.Expanded);
        }

        private void OnCloseBasicClicked(object sender, EventArgs e)
        {
            basicBottomSheet.Hide();
        }

        private void OnBottomSheetStateChanged(object? sender, StateChangedEventArgs e)
        {
            stateLabel.Text = $"Durum: {e.OldState} → {e.NewState}";
        }

        #endregion

        #region Music Player Bottom Sheet

        private void OnMusicPlayerClicked(object sender, EventArgs e)
        {
            musicBottomSheet.Show(BottomSheetState.Collapsed);
        }

        private void OnPeekContentTapped(object? sender, EventArgs e)
        {
            // Make sure the content opens completely when you click on "Peek content".
            if (musicBottomSheet.State == BottomSheetState.Collapsed)
            {
                musicBottomSheet.Show(BottomSheetState.Expanded);
            }
        }

        #endregion

        #region List Bottom Sheet

        private void OnListBottomSheetClicked(object sender, EventArgs e)
        {
            listBottomSheet.Show(BottomSheetState.HalfExpanded);
        }

        #endregion

        #region Snap Points Bottom Sheet

        private void OnSnapPointsClicked(object sender, EventArgs e)
        {
            snapPointsBottomSheet.Show(BottomSheetState.HalfExpanded);
        }

        private void OnSnapPointReached(object? sender, SnapPointReachedEventArgs e)
        {
            var percentage = (int)(e.SnapPoint * 100);
            snapPointLabel.Text = $"Mevcut: %{percentage} (Index: {e.SnapPointIndex})";
        }

        private void OnCloseSnapPointsClicked(object sender, EventArgs e)
        {
            snapPointsBottomSheet.Hide();
        }

        #endregion

        #region Form Bottom Sheet

        private void OnFormBottomSheetClicked(object sender, EventArgs e)
        {
            formBottomSheet.Show(BottomSheetState.HalfExpanded);
        }

        private void OnCloseFormClicked(object sender, EventArgs e)
        {
            formBottomSheet.Hide();
        }

        #endregion

        #region Locked Bottom Sheet

        private void OnLockedBottomSheetClicked(object sender, EventArgs e)
        {
            lockedBottomSheet.Show(BottomSheetState.HalfExpanded);
            lockedBottomSheet.UnlockGesture();
            UpdateLockStatus();
        }

        private void OnLockClicked(object sender, EventArgs e)
        {
            lockedBottomSheet.LockGesture();
            UpdateLockStatus();
        }

        private void OnUnlockClicked(object sender, EventArgs e)
        {
            lockedBottomSheet.UnlockGesture();
            UpdateLockStatus();
        }

        private void OnLockUpClicked(object sender, EventArgs e)
        {
            lockedBottomSheet.GestureLockMode = GestureLockMode.LockUp;
            UpdateLockStatus();
        }

        private void OnLockDownClicked(object sender, EventArgs e)
        {
            lockedBottomSheet.GestureLockMode = GestureLockMode.LockDown;
            UpdateLockStatus();
        }

        private void UpdateLockStatus()
        {
            var mode = lockedBottomSheet.GestureLockMode;
            lockStatusLabel.Text = mode switch
            {
                GestureLockMode.None => "State: 🔓 Opened",
                GestureLockMode.Locked => "State: 🔒 Fully Locked",
                GestureLockMode.LockUp => "State: ⬇️ Just Down",
                GestureLockMode.LockDown => "State: ⬆️ Just Up",
                _ => "Durum: ?"
            };
        }

        private void OnCloseLockedClicked(object sender, EventArgs e)
        {
            lockedBottomSheet.UnlockGesture(); // Unlock it before closing it.
            lockedBottomSheet.Hide();
        }

        #endregion

        #region Colorful Bottom Sheet

        private void OnColorfulBottomSheetClicked(object sender, EventArgs e)
        {
            colorfulBottomSheet.Show(BottomSheetState.HalfExpanded);
        }

        private void OnCloseColorfulClicked(object sender, EventArgs e)
        {
            colorfulBottomSheet.Hide();
        }

        #endregion
    }
}
