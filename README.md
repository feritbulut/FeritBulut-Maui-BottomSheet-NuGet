# 🚀 FeritBulut.Maui.BottomSheet

[![NuGet](https://img.shields.io/nuget/v/FeritBulut.Maui.BottomSheet.svg)](https://www.nuget.org/packages/FeritBulut.Maui.BottomSheet)
[![NuGet Downloads](https://img.shields.io/nuget/dt/FeritBulut.Maui.BottomSheet.svg)](https://www.nuget.org/packages/FeritBulut.Maui.BottomSheet)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Professional, highly customizable Bottom Sheet control for .NET MAUI applications.

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🎯 **Multiple States** | Hidden, Collapsed, HalfExpanded, Expanded |
| 👆 **Draggable** | Smooth drag gestures with velocity detection |
| 📍 **Snap Points** | Custom snap points at any position |
| 👀 **Peek Content** | Spotify-style mini content when collapsed |
| 📌 **Header Support** | Fixed header with scrollable content |
| ⌨️ **Keyboard Aware** | Automatically adjusts when keyboard appears |
| 🔒 **Gesture Lock** | Lock/unlock drag gestures programmatically |
| 🎨 **Fully Customizable** | Colors, corners, shadows, animations |
| 💫 **Bounce Effect** | iOS-style bounce at boundaries |
| 📱 **Cross-platform** | Android, iOS, MacCatalyst, Windows |

---

## 📦 Installation

### NuGet Package Manager
```bash
Install-Package FeritBulut.Maui.BottomSheet
```

### .NET CLI
```bash
dotnet add package FeritBulut.Maui.BottomSheet
```

### Package Reference
```xml
<PackageReference Include="FeritBulut.Maui.BottomSheet" Version="1.0.0" />
```

---

## 🚀 Quick Start

### 1. Register in MauiProgram.cs

```csharp
using FeritBulut.Maui.BottomSheet.Extensions;

public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseBottomSheet(); // Add this line

    return builder.Build();
}
```

### 2. Add namespace in XAML

```xml
xmlns:bottom="clr-namespace:FeritBulut.Maui.BottomSheet.Controls;assembly=FeritBulut.Maui.BottomSheet"
```

### 3. Use the control

```xml
<bottom:BottomSheetView 
    x:Name="myBottomSheet"
    SheetBackgroundColor="White"
    CornerRadius="24"
    HandleColor="Gray"
    CloseOnOverlayTap="True"
    IsDraggable="True"
    EnableBounceEffect="True">
    
    <bottom:BottomSheetView.SheetContent>
        <StackLayout Padding="20">
            <Label Text="Hello Bottom Sheet!" FontSize="24" />
            <Button Text="Close" Clicked="OnCloseClicked" />
        </StackLayout>
    </bottom:BottomSheetView.SheetContent>
    
</bottom:BottomSheetView>
```

### 4. Control from code-behind

```csharp
// Show
myBottomSheet.Show(BottomSheetState.HalfExpanded);

// Hide
myBottomSheet.Hide();

// Toggle
myBottomSheet.Toggle();
```

---

## 📖 Advanced Usage

### Peek Content (Spotify-style)

```xml
<bottom:BottomSheetView x:Name="musicSheet">
    
    <!-- Mini player visible when collapsed -->
    <bottom:BottomSheetView.PeekContent>
        <Grid Padding="15" ColumnDefinitions="50,*,40">
            <Image Source="album.png" />
            <Label Grid.Column="1" Text="Song Name" />
            <Button Grid.Column="2" Text="▶️" />
        </Grid>
    </bottom:BottomSheetView.PeekContent>
    
    <!-- Full content -->
    <bottom:BottomSheetView.SheetContent>
        <!-- Full music player UI -->
    </bottom:BottomSheetView.SheetContent>
    
</bottom:BottomSheetView>
```

### Fixed Header with Scrollable Content

```xml
<bottom:BottomSheetView>
    
    <!-- Fixed header -->
    <bottom:BottomSheetView.HeaderContent>
        <Label Text="My Header" FontSize="20" Padding="20" />
    </bottom:BottomSheetView.HeaderContent>
    
    <!-- Scrollable content -->
    <bottom:BottomSheetView.SheetContent>
        <!-- Your scrollable content -->
    </bottom:BottomSheetView.SheetContent>
    
</bottom:BottomSheetView>
```

### Custom Snap Points

```csharp
// In constructor or initialization
myBottomSheet.UseSnapPoints = true;
myBottomSheet.AddSnapPoint(0.25); // 25%
myBottomSheet.AddSnapPoint(0.50); // 50%
myBottomSheet.AddSnapPoint(0.75); // 75%

// Listen to snap events
myBottomSheet.SnapPointReached += (s, e) =>
{
    Console.WriteLine($"Snapped to: {e.SnapPoint * 100}%");
};
```

### Gesture Locking

```csharp
// Lock completely
myBottomSheet.LockGesture();

// Unlock
myBottomSheet.UnlockGesture();

// Lock specific direction
myBottomSheet.GestureLockMode = GestureLockMode.LockUp;   // Only drag down
myBottomSheet.GestureLockMode = GestureLockMode.LockDown; // Only drag up
```

---

## ⚙️ Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `State` | `BottomSheetState` | `Hidden` | Current state |
| `SheetContent` | `View` | `null` | Main content |
| `HeaderContent` | `View` | `null` | Fixed header |
| `PeekContent` | `View` | `null` | Collapsed preview |
| `CollapsedRatio` | `double` | `0.15` | Collapsed height ratio |
| `HalfExpandedRatio` | `double` | `0.5` | Half expanded ratio |
| `ExpandedRatio` | `double` | `0.92` | Expanded height ratio |
| `SheetBackgroundColor` | `Color` | `White` | Background color |
| `OverlayColor` | `Color` | `#80000000` | Overlay color |
| `HandleColor` | `Color` | `LightGray` | Handle bar color |
| `CornerRadius` | `double` | `20` | Corner radius |
| `IsHandleVisible` | `bool` | `true` | Show handle bar |
| `IsDraggable` | `bool` | `true` | Enable dragging |
| `CloseOnOverlayTap` | `bool` | `true` | Close on overlay tap |
| `EnableBounceEffect` | `bool` | `true` | Bounce at boundaries |
| `EnableContentScroll` | `bool` | `true` | Enable content scrolling |
| `IsKeyboardAware` | `bool` | `true` | Adjust for keyboard |
| `AnimationDuration` | `uint` | `250` | Animation duration (ms) |
| `HasShadow` | `bool` | `true` | Show shadow |

---

## 📡 Events

| Event | Description |
|-------|-------------|
| `StateChanged` | Fired when state changes |
| `Opened` | Fired when sheet opens |
| `Closed` | Fired when sheet closes |
| `Dragging` | Fired during drag |
| `DragStarted` | Fired when drag starts |
| `DragEnded` | Fired when drag ends |
| `SnapPointReached` | Fired when snap point reached |
| `PeekContentTapped` | Fired when peek content tapped |

---

## 🎯 Methods

| Method | Description |
|--------|-------------|
| `Show(state)` | Show with specified state |
| `Hide()` | Hide the sheet |
| `Toggle()` | Toggle visibility |
| `LockGesture()` | Lock all gestures |
| `UnlockGesture()` | Unlock gestures |
| `AddSnapPoint(ratio)` | Add custom snap point |
| `ClearSnapPoints()` | Clear all snap points |
| `ScrollToTopAsync()` | Scroll content to top |
| `ScrollToBottomAsync()` | Scroll content to bottom |

---

## 📱 Platform Support

| Platform | Minimum Version |
|----------|----------------|
| Android | API 21 (5.0) |
| iOS | 15.0 |
| MacCatalyst | 15.0 |
| Windows | 10.0.17763.0 |

---

## 📄 License

This project is licensed under the MIT License

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

---

## 👨‍💻 Author

**Ferit Bulut**

- GitHub: [@feritbulut](https://github.com/feritbulut)

---

## ⭐ Show Your Support

If this project helped you, please give it a ⭐ on GitHub!
