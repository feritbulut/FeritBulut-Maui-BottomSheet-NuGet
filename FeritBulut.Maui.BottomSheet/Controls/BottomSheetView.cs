using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using FeritBulut.Maui.BottomSheet.Enums;
using FeritBulut.Maui.BottomSheet.EventArgs;
using FeritBulut.Maui.BottomSheet.Helpers;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls.Shapes;

namespace FeritBulut.Maui.BottomSheet.Controls;

/// <summary>
/// Professional, fully featured Bottom Sheet check.
/// </summary>
public class BottomSheetView : ContentView
{
    #region Private Fields

    private readonly Grid _rootGrid;
    private readonly BoxView _overlay;
    private readonly Border _sheetContainer;
    private readonly BoxView _handleBar;
    private readonly ContentView _headerContainer;
    private readonly ContentView _peekContainer;
    private readonly ScrollView _contentScrollView;
    private readonly ContentView _contentContainer;
    private readonly PanGestureRecognizer _panGesture;

    private double _currentTranslationY;
    private double _sheetHeight;
    private double _containerHeight;
    private bool _isAnimating;
    private double _lastPanY;
    private DateTime _lastPanTime;
    private double _velocity;
    private bool _isKeyboardVisible;
    private double _keyboardHeight;

    #endregion

    #region Bindable Properties

    // ===== CONDITION FEATURES =====

    public static readonly BindableProperty StateProperty = BindableProperty.Create(
        nameof(State),
        typeof(BottomSheetState),
        typeof(BottomSheetView),
        BottomSheetState.Hidden,
        propertyChanged: OnStateChanged);

    public static readonly BindableProperty AllowedStatesProperty = BindableProperty.Create(
        nameof(AllowedStates),
        typeof(BottomSheetState[]),
        typeof(BottomSheetView),
        new[] { BottomSheetState.Hidden, BottomSheetState.Collapsed, BottomSheetState.HalfExpanded, BottomSheetState.Expanded });

    // ===== CONTENT FEATURES =====

    public static readonly BindableProperty SheetContentProperty = BindableProperty.Create(
        nameof(SheetContent),
        typeof(View),
        typeof(BottomSheetView),
        null,
        propertyChanged: OnSheetContentChanged);

    public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(
        nameof(HeaderContent),
        typeof(View),
        typeof(BottomSheetView),
        null,
        propertyChanged: OnHeaderContentChanged);

    public static readonly BindableProperty PeekContentProperty = BindableProperty.Create(
        nameof(PeekContent),
        typeof(View),
        typeof(BottomSheetView),
        null,
        propertyChanged: OnPeekContentChanged);

    // ===== SIZE SPECIFICATIONS =====

    public static readonly BindableProperty CollapsedRatioProperty = BindableProperty.Create(
        nameof(CollapsedRatio),
        typeof(double),
        typeof(BottomSheetView),
        0.15);

    public static readonly BindableProperty HalfExpandedRatioProperty = BindableProperty.Create(
        nameof(HalfExpandedRatio),
        typeof(double),
        typeof(BottomSheetView),
        0.5);

    public static readonly BindableProperty ExpandedRatioProperty = BindableProperty.Create(
        nameof(ExpandedRatio),
        typeof(double),
        typeof(BottomSheetView),
        0.92);

    public static readonly BindableProperty PeekHeightProperty = BindableProperty.Create(
        nameof(PeekHeight),
        typeof(double),
        typeof(BottomSheetView),
        80.0);

    // ===== SNAP POINTS =====

    public static readonly BindableProperty SnapPointsProperty = BindableProperty.Create(
        nameof(SnapPoints),
        typeof(SnapPointCollection),
        typeof(BottomSheetView),
        null);

    public static readonly BindableProperty UseSnapPointsProperty = BindableProperty.Create(
        nameof(UseSnapPoints),
        typeof(bool),
        typeof(BottomSheetView),
        false);

    // ===== APPEARANCE FEATURES =====

    public static readonly BindableProperty SheetBackgroundColorProperty = BindableProperty.Create(
        nameof(SheetBackgroundColor),
        typeof(Color),
        typeof(BottomSheetView),
        Colors.White,
        propertyChanged: OnSheetBackgroundColorChanged);

    public static readonly BindableProperty OverlayColorProperty = BindableProperty.Create(
        nameof(OverlayColor),
        typeof(Color),
        typeof(BottomSheetView),
        Color.FromArgb("#80000000"),
        propertyChanged: OnOverlayColorChanged);

    public static readonly BindableProperty HandleColorProperty = BindableProperty.Create(
        nameof(HandleColor),
        typeof(Color),
        typeof(BottomSheetView),
        Colors.LightGray,
        propertyChanged: OnHandleColorChanged);

    public static readonly BindableProperty IsHandleVisibleProperty = BindableProperty.Create(
        nameof(IsHandleVisible),
        typeof(bool),
        typeof(BottomSheetView),
        true,
        propertyChanged: OnIsHandleVisibleChanged);

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        nameof(CornerRadius),
        typeof(double),
        typeof(BottomSheetView),
        20.0,
        propertyChanged: OnCornerRadiusChanged);

    public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(
        nameof(HasShadow),
        typeof(bool),
        typeof(BottomSheetView),
        true,
        propertyChanged: OnHasShadowChanged);

    // ===== BLUR & EFFECT FEATURES =====

    public static readonly BindableProperty EnableBackdropBlurProperty = BindableProperty.Create(
        nameof(EnableBackdropBlur),
        typeof(bool),
        typeof(BottomSheetView),
        false,
        propertyChanged: OnEnableBackdropBlurChanged);

    public static readonly BindableProperty BlurIntensityProperty = BindableProperty.Create(
        nameof(BlurIntensity),
        typeof(double),
        typeof(BottomSheetView),
        10.0);

    public static readonly BindableProperty EnableBounceEffectProperty = BindableProperty.Create(
        nameof(EnableBounceEffect),
        typeof(bool),
        typeof(BottomSheetView),
        true);

    public static readonly BindableProperty BounceDistanceProperty = BindableProperty.Create(
        nameof(BounceDistance),
        typeof(double),
        typeof(BottomSheetView),
        30.0);

    // ===== BEHAVIORAL CHARACTERISTICS =====

    public static readonly BindableProperty CloseOnOverlayTapProperty = BindableProperty.Create(
        nameof(CloseOnOverlayTap),
        typeof(bool),
        typeof(BottomSheetView),
        true);

    public static readonly BindableProperty IsDraggableProperty = BindableProperty.Create(
        nameof(IsDraggable),
        typeof(bool),
        typeof(BottomSheetView),
        true,
        propertyChanged: OnIsDraggableChanged);

    public static readonly BindableProperty GestureLockModeProperty = BindableProperty.Create(
        nameof(GestureLockMode),
        typeof(GestureLockMode),
        typeof(BottomSheetView),
        GestureLockMode.None);

    public static readonly BindableProperty LockAtStateProperty = BindableProperty.Create(
        nameof(LockAtState),
        typeof(BottomSheetState?),
        typeof(BottomSheetView),
        null);

    public static readonly BindableProperty IsKeyboardAwareProperty = BindableProperty.Create(
        nameof(IsKeyboardAware),
        typeof(bool),
        typeof(BottomSheetView),
        true);

    public static readonly BindableProperty AnimationDurationProperty = BindableProperty.Create(
        nameof(AnimationDuration),
        typeof(uint),
        typeof(BottomSheetView),
        (uint)250);

    public static readonly BindableProperty AnimationEasingProperty = BindableProperty.Create(
        nameof(AnimationEasing),
        typeof(Easing),
        typeof(BottomSheetView),
        Easing.CubicOut);

    public static readonly BindableProperty VelocityThresholdProperty = BindableProperty.Create(
        nameof(VelocityThreshold),
        typeof(double),
        typeof(BottomSheetView),
        1000.0);

    // ===== SCROLL FEATURES =====

    public static readonly BindableProperty EnableContentScrollProperty = BindableProperty.Create(
        nameof(EnableContentScroll),
        typeof(bool),
        typeof(BottomSheetView),
        true,
        propertyChanged: OnEnableContentScrollChanged);

    public static readonly BindableProperty ScrollOrientationProperty = BindableProperty.Create(
        nameof(ScrollOrientation),
        typeof(ScrollOrientation),
        typeof(BottomSheetView),
        ScrollOrientation.Vertical);

    #endregion

    #region Properties

    public BottomSheetState State
    {
        get => (BottomSheetState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    public BottomSheetState[] AllowedStates
    {
        get => (BottomSheetState[])GetValue(AllowedStatesProperty);
        set => SetValue(AllowedStatesProperty, value);
    }

    public View SheetContent
    {
        get => (View)GetValue(SheetContentProperty);
        set => SetValue(SheetContentProperty, value);
    }

    public View HeaderContent
    {
        get => (View)GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public View PeekContent
    {
        get => (View)GetValue(PeekContentProperty);
        set => SetValue(PeekContentProperty, value);
    }

    public double CollapsedRatio
    {
        get => (double)GetValue(CollapsedRatioProperty);
        set => SetValue(CollapsedRatioProperty, Math.Clamp(value, 0.05, 0.4));
    }

    public double HalfExpandedRatio
    {
        get => (double)GetValue(HalfExpandedRatioProperty);
        set => SetValue(HalfExpandedRatioProperty, Math.Clamp(value, 0.3, 0.7));
    }

    public double ExpandedRatio
    {
        get => (double)GetValue(ExpandedRatioProperty);
        set => SetValue(ExpandedRatioProperty, Math.Clamp(value, 0.7, 1.0));
    }

    public double PeekHeight
    {
        get => (double)GetValue(PeekHeightProperty);
        set => SetValue(PeekHeightProperty, value);
    }

    public SnapPointCollection SnapPoints
    {
        get => (SnapPointCollection)GetValue(SnapPointsProperty);
        set => SetValue(SnapPointsProperty, value);
    }

    public bool UseSnapPoints
    {
        get => (bool)GetValue(UseSnapPointsProperty);
        set => SetValue(UseSnapPointsProperty, value);
    }

    public Color SheetBackgroundColor
    {
        get => (Color)GetValue(SheetBackgroundColorProperty);
        set => SetValue(SheetBackgroundColorProperty, value);
    }

    public Color OverlayColor
    {
        get => (Color)GetValue(OverlayColorProperty);
        set => SetValue(OverlayColorProperty, value);
    }

    public Color HandleColor
    {
        get => (Color)GetValue(HandleColorProperty);
        set => SetValue(HandleColorProperty, value);
    }

    public bool IsHandleVisible
    {
        get => (bool)GetValue(IsHandleVisibleProperty);
        set => SetValue(IsHandleVisibleProperty, value);
    }

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public bool HasShadow
    {
        get => (bool)GetValue(HasShadowProperty);
        set => SetValue(HasShadowProperty, value);
    }

    public bool EnableBackdropBlur
    {
        get => (bool)GetValue(EnableBackdropBlurProperty);
        set => SetValue(EnableBackdropBlurProperty, value);
    }

    public double BlurIntensity
    {
        get => (double)GetValue(BlurIntensityProperty);
        set => SetValue(BlurIntensityProperty, value);
    }

    public bool EnableBounceEffect
    {
        get => (bool)GetValue(EnableBounceEffectProperty);
        set => SetValue(EnableBounceEffectProperty, value);
    }

    public double BounceDistance
    {
        get => (double)GetValue(BounceDistanceProperty);
        set => SetValue(BounceDistanceProperty, value);
    }

    public bool CloseOnOverlayTap
    {
        get => (bool)GetValue(CloseOnOverlayTapProperty);
        set => SetValue(CloseOnOverlayTapProperty, value);
    }

    public bool IsDraggable
    {
        get => (bool)GetValue(IsDraggableProperty);
        set => SetValue(IsDraggableProperty, value);
    }

    public GestureLockMode GestureLockMode
    {
        get => (GestureLockMode)GetValue(GestureLockModeProperty);
        set => SetValue(GestureLockModeProperty, value);
    }

    public BottomSheetState? LockAtState
    {
        get => (BottomSheetState?)GetValue(LockAtStateProperty);
        set => SetValue(LockAtStateProperty, value);
    }

    public bool IsKeyboardAware
    {
        get => (bool)GetValue(IsKeyboardAwareProperty);
        set => SetValue(IsKeyboardAwareProperty, value);
    }

    public uint AnimationDuration
    {
        get => (uint)GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    public Easing AnimationEasing
    {
        get => (Easing)GetValue(AnimationEasingProperty);
        set => SetValue(AnimationEasingProperty, value);
    }

    public double VelocityThreshold
    {
        get => (double)GetValue(VelocityThresholdProperty);
        set => SetValue(VelocityThresholdProperty, value);
    }

    public bool EnableContentScroll
    {
        get => (bool)GetValue(EnableContentScrollProperty);
        set => SetValue(EnableContentScrollProperty, value);
    }

    public ScrollOrientation ScrollOrientation
    {
        get => (ScrollOrientation)GetValue(ScrollOrientationProperty);
        set => SetValue(ScrollOrientationProperty, value);
    }

    #endregion

    #region Events

    /// <summary>
    /// It is triggered when the situation changes.
    /// </summary>
    public event EventHandler<StateChangedEventArgs>? StateChanged;

    /// <summary>
    /// It is triggered during dragging.
    /// </summary>
    public event EventHandler<BottomSheetDragEventArgs>? Dragging;

    /// <summary>
    /// It is triggered when dragging begins.
    /// </summary>
    public event EventHandler? DragStarted;

    /// <summary>
    /// It is triggered when the dragging is finished.
    /// </summary>
    public event EventHandler? DragEnded;

    /// <summary>
    /// It is triggered when the snap point is reached.
    /// </summary>
    public event EventHandler<SnapPointReachedEventArgs>? SnapPointReached;

    /// <summary>
    /// It is triggered when the sheet is fully opened.
    /// </summary>
    public event EventHandler? Opened;

    /// <summary>
    /// It is triggered when the sheet is completely closed.
    /// </summary>
    public event EventHandler? Closed;

    /// <summary>
    /// Peek is triggered when its content is clicked.
    /// </summary>
    public event EventHandler? PeekContentTapped;

    #endregion

    #region Constructor

    public BottomSheetView()
    {
        // Start your Snap Points collection
        SnapPoints = new SnapPointCollection();

        // Main grid
        _rootGrid = new Grid();

        // Overlay
        _overlay = new BoxView
        {
            Color = OverlayColor,
            Opacity = 0,
            IsVisible = false
        };

        var overlayTapGesture = new TapGestureRecognizer();
        overlayTapGesture.Tapped += OnOverlayTapped;
        _overlay.GestureRecognizers.Add(overlayTapGesture);

        // Handle bar
        _handleBar = new BoxView
        {
            Color = HandleColor,
            HeightRequest = 5,
            WidthRequest = 45,
            CornerRadius = 2.5,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 12, 0, 8),
            IsVisible = IsHandleVisible
        };

        // Header container
        _headerContainer = new ContentView
        {
            IsVisible = false,
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Fill
        };

        // Peek container
        _peekContainer = new ContentView
        {
            IsVisible = false,
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Fill
        };

        var peekTapGesture = new TapGestureRecognizer();
        peekTapGesture.Tapped += OnPeekContentTapped;
        _peekContainer.GestureRecognizers.Add(peekTapGesture);

        // Content container with ScrollView
        _contentContainer = new ContentView
        {
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill
        };

        _contentScrollView = new ScrollView
        {
            Orientation = ScrollOrientation,
            Content = _contentContainer,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill
        };

        // Sheet layout
        var sheetStackLayout = new StackLayout
        {
            Spacing = 0,
            Children =
            {
                _handleBar,
                _peekContainer,
                _headerContainer,
                _contentScrollView
            }
        };

        // Sheet container
        _sheetContainer = new Border
        {
            BackgroundColor = SheetBackgroundColor,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(CornerRadius, CornerRadius, 0, 0)
            },
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            Padding = new Thickness(0),
            Content = sheetStackLayout,
            VerticalOptions = LayoutOptions.End,
            HorizontalOptions = LayoutOptions.Fill,
            Shadow = HasShadow ? new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Offset = new Point(0, -2),
                Radius = 10,
                Opacity = 0.3f
            } : null
        };

        // Pan gesture
        _panGesture = new PanGestureRecognizer();
        _panGesture.PanUpdated += OnPanUpdated;
        _sheetContainer.GestureRecognizers.Add(_panGesture);

        // Root add to grid
        _rootGrid.Children.Add(_overlay);
        _rootGrid.Children.Add(_sheetContainer);

        Content = _rootGrid;

        // Initial state
        _sheetContainer.TranslationY = 2000;
        _sheetContainer.IsVisible = false;

        // Keyboard helper'ı start
        InitializeKeyboardHandling();
    }

    #endregion

    #region Keyboard Handling

    private void InitializeKeyboardHandling()
    {
        if (!IsKeyboardAware) return;

        try
        {
            KeyboardHelper.Initialize();
            KeyboardHelper.KeyboardShown += OnKeyboardShown;
            KeyboardHelper.KeyboardHidden += OnKeyboardHidden;
        }
        catch
        {
            // If the platform doesn't support it, just continue silently.
        }
    }

    private void OnKeyboardShown(object? sender, double height)
    {
        if (!IsKeyboardAware || State == BottomSheetState.Hidden) return;

        _isKeyboardVisible = true;
        _keyboardHeight = height;

        //Scroll the sheet as far up as the keyboard allows.
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await _sheetContainer.TranslateTo(0, _sheetContainer.TranslationY - height, AnimationDuration, AnimationEasing);
        });
    }

    private void OnKeyboardHidden(object? sender, System.EventArgs e)
    {
        if (!IsKeyboardAware || !_isKeyboardVisible) return;

        _isKeyboardVisible = false;

        // Restore the sheet to its original position.
        MainThread.BeginInvokeOnMainThread(() =>
        {
            AnimateToState(State, State);
        });
    }

    #endregion

    #region Property Changed Handlers

    private static void OnStateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet)
        {
            var oldState = (BottomSheetState)oldValue;
            var newState = (BottomSheetState)newValue;
            sheet.AnimateToState(newState, oldState);
        }
    }

    private static void OnSheetContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is View content)
        {
            sheet._contentContainer.Content = content;
        }
    }

    private static void OnHeaderContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet)
        {
            if (newValue is View content)
            {
                sheet._headerContainer.Content = content;
                sheet._headerContainer.IsVisible = true;
            }
            else
            {
                sheet._headerContainer.IsVisible = false;
            }
        }
    }

    private static void OnPeekContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet)
        {
            if (newValue is View content)
            {
                sheet._peekContainer.Content = content;
                sheet._peekContainer.IsVisible = true;
            }
            else
            {
                sheet._peekContainer.IsVisible = false;
            }
        }
    }

    private static void OnSheetBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is Color color)
        {
            sheet._sheetContainer.BackgroundColor = color;
        }
    }

    private static void OnOverlayColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is Color color)
        {
            sheet._overlay.Color = color;
        }
    }

    private static void OnHandleColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is Color color)
        {
            sheet._handleBar.Color = color;
        }
    }

    private static void OnIsHandleVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is bool isVisible)
        {
            sheet._handleBar.IsVisible = isVisible;
        }
    }

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is double radius)
        {
            sheet._sheetContainer.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(radius, radius, 0, 0)
            };
        }
    }

    private static void OnHasShadowChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is bool hasShadow)
        {
            sheet._sheetContainer.Shadow = hasShadow ? new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Offset = new Point(0, -2),
                Radius = 10,
                Opacity = 0.3f
            } : null;
        }
    }

    private static void OnEnableBackdropBlurChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // Requires platform-specific blur implementation.
        // For now, we are simulating it with overlay opacity.
    }

    private static void OnIsDraggableChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is bool isDraggable)
        {
            if (isDraggable)
            {
                if (!sheet._sheetContainer.GestureRecognizers.Contains(sheet._panGesture))
                    sheet._sheetContainer.GestureRecognizers.Add(sheet._panGesture);
            }
            else
            {
                sheet._sheetContainer.GestureRecognizers.Remove(sheet._panGesture);
            }
        }
    }

    private static void OnEnableContentScrollChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BottomSheetView sheet && newValue is bool enableScroll)
        {
            sheet._contentScrollView.IsEnabled = enableScroll;
        }
    }

    #endregion

    #region Gesture Handlers

    private void OnOverlayTapped(object? sender, TappedEventArgs e)
    {
        if (CloseOnOverlayTap && !IsGestureLocked())
        {
            State = BottomSheetState.Hidden;
        }
    }

    private void OnPeekContentTapped(object? sender, TappedEventArgs e)
    {
        PeekContentTapped?.Invoke(this, System.EventArgs.Empty);

        // Peek is partially opened when clicked.
        if (State == BottomSheetState.Collapsed || State == BottomSheetState.Hidden)
        {
            State = BottomSheetState.HalfExpanded;
        }
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (_isAnimating || IsGestureLocked()) return;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                HandlePanStarted();
                break;

            case GestureStatus.Running:
                HandlePanRunning(e.TotalY);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                HandlePanCompleted();
                break;
        }
    }

    private void HandlePanStarted()
    {
        _currentTranslationY = _sheetContainer.TranslationY;
        _lastPanY = 0;
        _lastPanTime = DateTime.Now;
        _velocity = 0;

        DragStarted?.Invoke(this, System.EventArgs.Empty);
    }

    private void HandlePanRunning(double totalY)
    {
        // Calculate velocity
        var now = DateTime.Now;
        var timeDelta = (now - _lastPanTime).TotalSeconds;
        if (timeDelta > 0)
        {
            _velocity = (totalY - _lastPanY) / timeDelta;
        }
        _lastPanY = totalY;
        _lastPanTime = now;

        // Direction control
        var direction = totalY > 0 ? GestureDirection.Down : GestureDirection.Up;

        // Lock mode control
        if (GestureLockMode == GestureLockMode.LockDown && direction == GestureDirection.Down)
            return;
        if (GestureLockMode == GestureLockMode.LockUp && direction == GestureDirection.Up)
            return;

        var newTranslationY = _currentTranslationY + totalY;
        _containerHeight = _rootGrid.Height > 0 ? _rootGrid.Height : 800;

        // Set boundaries.
        var minTranslationY = _containerHeight * (1 - ExpandedRatio);
        var maxTranslationY = _containerHeight;

        // Bounce effect
        if (EnableBounceEffect)
        {
            if (newTranslationY < minTranslationY)
            {
                // Bounce at the upper limit
                var overScroll = minTranslationY - newTranslationY;
                var dampedOverScroll = BounceDistance * (1 - Math.Exp(-overScroll / BounceDistance));
                newTranslationY = minTranslationY - dampedOverScroll;
            }
        }
        else
        {
            newTranslationY = Math.Max(minTranslationY, newTranslationY);
        }

        newTranslationY = Math.Min(maxTranslationY, newTranslationY);

        _sheetContainer.TranslationY = newTranslationY;
        UpdateOverlayOpacity();

        // Dragging event
        var currentRatio = 1 - (newTranslationY / _containerHeight);
        Dragging?.Invoke(this, new BottomSheetDragEventArgs(newTranslationY, direction, _velocity, currentRatio));
    }

    private void HandlePanCompleted()
    {
        DragEnded?.Invoke(this, System.EventArgs.Empty);

        // Fast drag control
        if (Math.Abs(_velocity) > VelocityThreshold)
        {
            if (_velocity > 0)
            {
                SnapToNextLowerState();
            }
            else
            {
                SnapToNextUpperState();
            }
        }
        else
        {
            // Normal snap
            SnapToNearestState();
        }
    }

    #endregion

    #region State & Animation Methods

    private bool IsGestureLocked()
    {
        if (GestureLockMode == GestureLockMode.Locked)
            return true;

        if (LockAtState.HasValue && State == LockAtState.Value)
            return true;

        return false;
    }

    private async void AnimateToState(BottomSheetState newState, BottomSheetState oldState)
    {
        if (_isAnimating) return;
        _isAnimating = true;

        _containerHeight = _rootGrid.Height > 0 ? _rootGrid.Height : 800;
        _sheetHeight = _containerHeight * ExpandedRatio;

        double targetTranslationY;
        double targetOverlayOpacity;

        switch (newState)
        {
            case BottomSheetState.Hidden:
                targetTranslationY = _containerHeight;
                targetOverlayOpacity = 0;
                break;

            case BottomSheetState.Collapsed:
                targetTranslationY = _containerHeight * (1 - CollapsedRatio);
                targetOverlayOpacity = 0.2;
                break;

            case BottomSheetState.HalfExpanded:
                targetTranslationY = _containerHeight * (1 - HalfExpandedRatio);
                targetOverlayOpacity = 0.4;
                break;

            case BottomSheetState.Expanded:
                targetTranslationY = _containerHeight * (1 - ExpandedRatio);
                targetOverlayOpacity = 0.6;
                break;

            default:
                targetTranslationY = _containerHeight;
                targetOverlayOpacity = 0;
                break;
        }

        // Visibility settings
        if (newState != BottomSheetState.Hidden)
        {
            _sheetContainer.IsVisible = true;
            _overlay.IsVisible = true;
            _sheetContainer.HeightRequest = _sheetHeight;

            // Peek content visibility
            if (PeekContent != null)
            {
                _peekContainer.IsVisible = newState == BottomSheetState.Collapsed;
            }
        }

        // Animation
        var animationTasks = new List<Task>
        {
            _sheetContainer.TranslateTo(0, targetTranslationY, AnimationDuration, AnimationEasing),
            _overlay.FadeTo(targetOverlayOpacity, AnimationDuration, AnimationEasing)
        };

        await Task.WhenAll(animationTasks);

        // Bounce effect for expanded state
        if (newState == BottomSheetState.Expanded && EnableBounceEffect && oldState != BottomSheetState.Expanded)
        {
            var bounceUp = targetTranslationY - 10;
            await _sheetContainer.TranslateTo(0, bounceUp, 100, Easing.CubicOut);
            await _sheetContainer.TranslateTo(0, targetTranslationY, 100, Easing.CubicIn);
        }

        _isAnimating = false;

        // Hide if it's closed
        if (newState == BottomSheetState.Hidden)
        {
            _sheetContainer.IsVisible = false;
            _overlay.IsVisible = false;
            Closed?.Invoke(this, System.EventArgs.Empty);
        }
        else if (oldState == BottomSheetState.Hidden)
        {
            Opened?.Invoke(this, System.EventArgs.Empty);
        }

        StateChanged?.Invoke(this, new StateChangedEventArgs(oldState, newState));
    }

    private void SnapToNearestState()
    {
        _containerHeight = _rootGrid.Height > 0 ? _rootGrid.Height : 800;
        var currentRatio = 1 - (_sheetContainer.TranslationY / _containerHeight);

        if (UseSnapPoints && SnapPoints?.Count > 0)
        {
            // Use custom snap points.
            var nearestSnap = SnapPoints.FindNearest(currentRatio);
            var snapIndex = SnapPoints.IndexOf(nearestSnap);

            AnimateToRatio(nearestSnap);
            SnapPointReached?.Invoke(this, new SnapPointReachedEventArgs(nearestSnap, snapIndex));
        }
        else
        {
            // Use the default settings.
            var states = GetAllowedStatesWithRatios();
            var nearest = states.OrderBy(s => Math.Abs(s.ratio - currentRatio)).First();
            State = nearest.state;
        }
    }

    private void SnapToNextUpperState()
    {
        _containerHeight = _rootGrid.Height > 0 ? _rootGrid.Height : 800;
        var currentRatio = 1 - (_sheetContainer.TranslationY / _containerHeight);

        if (UseSnapPoints && SnapPoints?.Count > 0)
        {
            var nextSnap = SnapPoints.FindNext(currentRatio);
            if (nextSnap.HasValue)
            {
                AnimateToRatio(nextSnap.Value);
                SnapPointReached?.Invoke(this, new SnapPointReachedEventArgs(nextSnap.Value, SnapPoints.IndexOf(nextSnap.Value)));
            }
        }
        else
        {
            var states = GetAllowedStatesWithRatios().OrderBy(s => s.ratio).ToList();
            var currentIndex = states.FindIndex(s => Math.Abs(s.ratio - currentRatio) < 0.1);
            if (currentIndex < states.Count - 1)
            {
                State = states[currentIndex + 1].state;
            }
            else
            {
                State = states.Last().state;
            }
        }
    }

    private void SnapToNextLowerState()
    {
        _containerHeight = _rootGrid.Height > 0 ? _rootGrid.Height : 800;
        var currentRatio = 1 - (_sheetContainer.TranslationY / _containerHeight);

        if (UseSnapPoints && SnapPoints?.Count > 0)
        {
            var prevSnap = SnapPoints.FindPrevious(currentRatio);
            if (prevSnap.HasValue)
            {
                AnimateToRatio(prevSnap.Value);
                SnapPointReached?.Invoke(this, new SnapPointReachedEventArgs(prevSnap.Value, SnapPoints.IndexOf(prevSnap.Value)));
            }
            else
            {
                State = BottomSheetState.Hidden;
            }
        }
        else
        {
            var states = GetAllowedStatesWithRatios().OrderByDescending(s => s.ratio).ToList();
            var currentIndex = states.FindIndex(s => Math.Abs(s.ratio - currentRatio) < 0.1);
            if (currentIndex < states.Count - 1)
            {
                State = states[currentIndex + 1].state;
            }
            else
            {
                State = BottomSheetState.Hidden;
            }
        }
    }

    private List<(BottomSheetState state, double ratio)> GetAllowedStatesWithRatios()
    {
        var allStates = new List<(BottomSheetState state, double ratio)>
        {
            (BottomSheetState.Hidden, 0),
            (BottomSheetState.Collapsed, CollapsedRatio),
            (BottomSheetState.HalfExpanded, HalfExpandedRatio),
            (BottomSheetState.Expanded, ExpandedRatio)
        };

        return allStates.Where(s => AllowedStates.Contains(s.state)).ToList();
    }

    private async void AnimateToRatio(double ratio)
    {
        if (_isAnimating) return;
        _isAnimating = true;

        _containerHeight = _rootGrid.Height > 0 ? _rootGrid.Height : 800;
        var targetTranslationY = _containerHeight * (1 - ratio);
        var targetOverlayOpacity = ratio * 0.7;

        await Task.WhenAll(
            _sheetContainer.TranslateTo(0, targetTranslationY, AnimationDuration, AnimationEasing),
            _overlay.FadeTo(targetOverlayOpacity, AnimationDuration, AnimationEasing)
        );

        _isAnimating = false;
    }

    private void UpdateOverlayOpacity()
    {
        _containerHeight = _rootGrid.Height > 0 ? _rootGrid.Height : 800;
        var ratio = 1 - (_sheetContainer.TranslationY / _containerHeight);
        _overlay.Opacity = Math.Clamp(ratio * 0.7, 0, 0.7);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the Bottom Sheet to the specified state.
    /// </summary>
    public void Show(BottomSheetState state = BottomSheetState.HalfExpanded)
    {
        if (AllowedStates.Contains(state))
        {
            State = state;
        }
        else if (AllowedStates.Length > 0)
        {
            State = AllowedStates.Where(s => s != BottomSheetState.Hidden).FirstOrDefault();
        }
    }

    /// <summary>
    /// Closes the bottom sheet.
    /// </summary>
    public void Hide()
    {
        State = BottomSheetState.Hidden;
    }

    /// <summary>
    /// Opens/closes the Bottom Sheet.
    /// </summary>
    public void Toggle(BottomSheetState openState = BottomSheetState.HalfExpanded)
    {
        State = State == BottomSheetState.Hidden ? openState : BottomSheetState.Hidden;
    }

    /// <summary>
    /// It creates an animated transition to the specified ratio value.
    /// </summary>
    /// <param name="ratio">Ratio between 0 and 1</param>
    public void AnimateToPosition(double ratio)
    {
        ratio = Math.Clamp(ratio, 0, 1);
        AnimateToRatio(ratio);
    }

    /// <summary>
    /// The content scrolls to the top.
    /// </summary>
    public async Task ScrollToTopAsync()
    {
        await _contentScrollView.ScrollToAsync(0, 0, true);
    }

    /// <summary>
    /// The content scrolls to the bottom.
    /// </summary>
    public async Task ScrollToBottomAsync()
    {
        await _contentScrollView.ScrollToAsync(0, _contentScrollView.ContentSize.Height, true);
    }

    /// <summary>
    /// Scrolls to the specified element.
    /// </summary>
    public async Task ScrollToElementAsync(Element element, ScrollToPosition position = ScrollToPosition.MakeVisible)
    {
        await _contentScrollView.ScrollToAsync(element, position, true);
    }

    /// <summary>
    /// Temporarily locks the drag.
    /// </summary>
    public void LockGesture()
    {
        GestureLockMode = GestureLockMode.Locked;
    }

    /// <summary>
    /// Unlocks drag lock
    /// </summary>
    public void UnlockGesture()
    {
        GestureLockMode = GestureLockMode.None;
    }

    /// <summary>
    /// Snap point adders
    /// </summary>
    /// <param name="ratio">Ratio between 0 and 1</param>
    public void AddSnapPoint(double ratio)
    {
        ratio = Math.Clamp(ratio, 0, 1);
        if (!SnapPoints.Contains(ratio))
        {
            SnapPoints.Add(ratio);
        }
    }

    /// <summary>
    /// Clears all snap points.
    /// </summary>
    public void ClearSnapPoints()
    {
        SnapPoints.Clear();
    }

    #endregion
}