# EVAVEO VR Manager SDK - Unreal Engine

Official Unreal Engine plugin for EVAVEO VR Manager analytics and telemetry.

## Installation

1. Copy the `Plugins/EvaveoSDK` folder to your project's `Plugins` directory
2. Restart Unreal Editor
3. Enable the plugin: `Edit` → `Plugins` → Search "Evaveo" → Enable
4. Restart the editor when prompted

## C++ Usage

### Include the header
```cpp
#include "EvaveoSDK.h"
```

### Initialize in GameMode
```cpp
void AMyGameMode::BeginPlay()
{
    Super::BeginPlay();
    
    // Initialize SDK with your API key
    UEvaveoSDK::Initialize(TEXT("vr_your_api_key_here"));
}
```

### Track custom events
```cpp
void AMyPlayerController::OnLevelComplete()
{
    UEvaveoSDK::TrackEvent(TEXT("level_complete"), TEXT("level_5"));
}
```

## Blueprint Usage

### Initialize SDK
1. In your Level Blueprint or GameMode Blueprint
2. On **Begin Play**, add **Initialize Evaveo SDK** node
3. Enter your API key

### Track Events
1. Add **Track Event** node
2. Set Event Name (e.g., "button_clicked")
3. Set Event Data (optional, e.g., "main_menu")

## API Reference

### Initialize
```cpp
UEvaveoSDK::Initialize(const FString& ApiKey)
```
Initialize the SDK with your API key. Call once at game startup.

### Track Event
```cpp
UEvaveoSDK::TrackEvent(const FString& EventName, const FString& EventData = TEXT(""))
```
Track a custom event with optional data.

### Set Enabled
```cpp
UEvaveoSDK::SetEnabled(bool bEnabled)
```
Enable or disable tracking.

### Set User ID
```cpp
UEvaveoSDK::SetUserId(const FString& UserId)
```
Set a custom user identifier.

### Is Initialized
```cpp
bool UEvaveoSDK::IsInitialized()
```
Check if SDK is initialized.

### Is Enabled
```cpp
bool UEvaveoSDK::IsEnabled()
```
Check if tracking is enabled.

## What's Tracked Automatically

- ✅ **Session Start** - When app launches
- ✅ **Session End** - When app closes
- ✅ **Device Info** - Device model, OS version
- ✅ **App Version** - Build version

## Privacy

Allow users to opt-out:
```cpp
// Disable tracking
UEvaveoSDK::SetEnabled(false);

// Re-enable tracking
UEvaveoSDK::SetEnabled(true);
```

## Support

- 📧 Email: support@evaveo.com
- 📖 Docs: https://github.com/Evaveo/VR-MANAGER-SDK
- 🐛 Issues: https://github.com/Evaveo/VR-MANAGER-SDK/issues

## License

MIT License - See LICENSE file for details
