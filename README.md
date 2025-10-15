# EVAVEO VR Manager SDK

Official SDK for integrating EVAVEO VR Manager analytics and telemetry into your VR applications.

[![Unity](https://img.shields.io/badge/Unity-2020.3+-black.svg)](https://unity.com/)
[![Unreal](https://img.shields.io/badge/Unreal-4.27+-blue.svg)](https://unrealengine.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

## 🎯 What is EVAVEO VR Manager?

EVAVEO VR Manager is a professional VR fleet management platform that allows organizations to:
- Deploy APKs to multiple VR devices
- Manage device fleets
- Track analytics and performance
- Monitor app usage in real-time

## 📦 What Does This SDK Do?

This SDK allows VR app developers to integrate telemetry and analytics into their applications. Once integrated, EVAVEO VR Manager can track:

- ✅ **Session Tracking** - When users launch and close your app
- ✅ **Usage Duration** - How long users engage with your app
- ✅ **Performance Metrics** - FPS, frame drops, memory usage
- ✅ **Crash Detection** - Automatic error and crash reporting
- ✅ **Battery Impact** - Battery drain during sessions
- ✅ **Custom Events** - Track your own custom analytics

## 🚀 Quick Start

### Unity

1. **Install the SDK**
   ```
   Add via Unity Package Manager:
   https://github.com/Evaveo/EVAVEO_VR_Manager_SDK.git#unity
   ```

2. **Initialize in your main script**
   ```csharp
   using EVAVEO_VR_Manager_SDK;
   
   void Start() {
       EVAVEO_VR_Manager.Initialize("your_api_key_here");
   }
   ```

3. **That's it!** The SDK automatically tracks everything.

### Unreal Engine

1. **Clone the repository**
   ```bash
   git clone https://github.com/Evaveo/VR-MANAGER-SDK-.git
   ```

2. **Copy the `Unreal/Plugins/EVAVEO_VR_Manager` folder to your project's `Plugins` directory**

3. **Initialize in your GameMode or PlayerController**
   ```cpp
   #include "EVAVEO_VR_Manager.h"
   
   void AMyGameMode::BeginPlay() {
       Super::BeginPlay();
       UEVAVEO_VR_Manager::Initialize("your_api_key_here");
   }
   ```

## 📖 Documentation

### Getting Your API Key

1. Sign up at [EVAVEO VR Manager](https://vrmanager.evaveo.com)
2. Go to **Settings** → **API Keys**
3. Click **"Generate SDK Key"**
4. Copy the key and use it in your app

### Unity Integration

#### Installation Methods

**Method 1: Unity Package Manager (Recommended)**
1. Open Unity
2. Go to `Window` → `Package Manager`
3. Click `+` → `Add package from git URL`
4. Enter: `https://github.com/Evaveo/VR-MANAGER-SDK-.git#unity`

**Method 2: Manual Installation**
1. Download the latest release
2. Extract to `Assets/Evaveo/`
3. Unity will automatically import

#### Basic Usage

```csharp
using UnityEngine;
using EVAVEO_VR_Manager_SDK;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Initialize SDK with your API key
        EVAVEO_VR_Manager.Initialize("vr_your_api_key_here");
        
        // SDK now automatically tracks:
        // - App launches
        // - Session duration
        // - FPS and performance
        // - Crashes and errors
    }
    
    // Optional: Track custom events
    void OnLevelComplete()
    {
        EVAVEO_VR_Manager.TrackEvent("level_complete", "level_5");
    }
}
```

#### Advanced Configuration

```csharp
// Configure SDK before initialization
EvaveoConfig config = new EvaveoConfig
{
    ApiKey = "vr_your_api_key_here",
    ApiUrl = "https://api.vrmanager.evaveo.com/api/tracking",
    EnableDebugLogs = true,
    TrackFPS = true,
    TrackBattery = true,
    SendInterval = 5.0f // Send data every 5 seconds
};

EVAVEO_VR_Manager.Initialize(config);
```

### Unreal Integration

#### Installation

1. Clone or download this repository
2. Copy `Unreal/Plugins/EVAVEO_VR_Manager` to your project's `Plugins` folder
3. Restart Unreal Editor
4. Enable the plugin: `Edit` → `Plugins` → Search "Evaveo" → Enable

#### Basic Usage

```cpp
// MyGameMode.h
#include "EVAVEO_VR_Manager.h"

UCLASS()
class AMyGameMode : public AGameModeBase
{
    GENERATED_BODY()
    
protected:
    virtual void BeginPlay() override;
};

// MyGameMode.cpp
void AMyGameMode::BeginPlay()
{
    Super::BeginPlay();
    
    // Initialize SDK
    UEVAVEO_VR_Manager::Initialize(TEXT("vr_your_api_key_here"));
}

// Track custom events
void AMyPlayerController::OnLevelComplete()
{
    UEVAVEO_VR_Manager::TrackEvent(TEXT("level_complete"), TEXT("level_5"));
}
```

#### Blueprint Support

The SDK is fully compatible with Blueprints:

1. In your Level Blueprint or GameMode Blueprint
2. On **Begin Play**, call **Initialize Evaveo SDK**
3. Enter your API key
4. Use **Track Event** nodes to log custom events

## 📊 What Data is Tracked?

### Automatic Tracking

The SDK automatically collects:

| Metric | Description | Frequency |
|--------|-------------|-----------|
| **Session Start** | When app launches | Once per launch |
| **Session End** | When app closes | Once per close |
| **Session Duration** | Total time in app | On close |
| **FPS** | Frames per second | Every 5 seconds |
| **Memory Usage** | RAM consumption | Every 5 seconds |
| **Battery Level** | Device battery % | Every 5 seconds |
| **Crashes** | Unhandled exceptions | When they occur |
| **Errors** | Logged errors | When they occur |

### Custom Events

Track your own events:

```csharp
// Unity
EVAVEO_VR_Manager.TrackEvent("button_clicked", "main_menu");
EVAVEO_VR_Manager.TrackEvent("purchase_made", "sword_legendary");
EVAVEO_VR_Manager.TrackEvent("tutorial_completed", "step_3");
```

```cpp
// Unreal
UEVAVEO_VR_Manager::TrackEvent(TEXT("button_clicked"), TEXT("main_menu"));
UEVAVEO_VR_Manager::TrackEvent(TEXT("purchase_made"), TEXT("sword_legendary"));
UEVAVEO_VR_Manager::TrackEvent(TEXT("tutorial_completed"), TEXT("step_3"));
```

## 🔒 Privacy & Security

- ✅ **No personal data collected** - Only app usage metrics
- ✅ **GDPR compliant** - Users can opt-out
- ✅ **Encrypted transmission** - All data sent over HTTPS
- ✅ **API key authentication** - Secure communication
- ✅ **Open source** - Full transparency

### Opt-Out Support

Allow users to disable analytics:

```csharp
// Unity
EVAVEO_VR_Manager.SetEnabled(false); // Disable tracking
EVAVEO_VR_Manager.SetEnabled(true);  // Re-enable tracking
```

```cpp
// Unreal
UEVAVEO_VR_Manager::SetEnabled(false); // Disable tracking
UEVAVEO_VR_Manager::SetEnabled(true);  // Re-enable tracking
```

## 📈 Viewing Analytics

Once integrated, view your app's analytics at:
1. Log in to [EVAVEO VR Manager](https://vrmanager.evaveo.com)
2. Go to **Analytics** → **App Usage**
3. See real-time data:
   - Total sessions
   - Active users
   - Average session duration
   - Crash rate
   - Performance metrics
   - Device distribution

## 🛠️ API Reference

### Unity API

#### `EVAVEO_VR_Manager.Initialize(string apiKey)`
Initialize the SDK with your API key.

#### `EVAVEO_VR_Manager.Initialize(EvaveoConfig config)`
Initialize with custom configuration.

#### `EVAVEO_VR_Manager.TrackEvent(string eventName, string eventData = null)`
Track a custom event.

#### `EVAVEO_VR_Manager.SetEnabled(bool enabled)`
Enable or disable tracking.

#### `EVAVEO_VR_Manager.SetUserId(string userId)`
Set a custom user identifier.

### Unreal API

#### `UEVAVEO_VR_Manager::Initialize(FString ApiKey)`
Initialize the SDK with your API key.

#### `UEVAVEO_VR_Manager::TrackEvent(FString EventName, FString EventData)`
Track a custom event.

#### `UEVAVEO_VR_Manager::SetEnabled(bool bEnabled)`
Enable or disable tracking.

#### `UEVAVEO_VR_Manager::SetUserId(FString UserId)`
Set a custom user identifier.

## 🐛 Troubleshooting

### SDK Not Sending Data

1. **Check API key** - Ensure it's valid and not expired
2. **Check internet connection** - SDK requires network access
3. **Check logs** - Enable debug logs to see what's happening
4. **Check firewall** - Ensure `api.vrmanager.evaveo.com` is not blocked

### Unity Specific

```csharp
// Enable debug logs
EvaveoConfig config = new EvaveoConfig {
    EnableDebugLogs = true
};
EVAVEO_VR_Manager.Initialize(config);

// Check Unity Console for "[EVAVEO VR Manager]" messages
```

### Unreal Specific

```cpp
// Enable debug logs in DefaultEngine.ini
[Core.Log]
LogEvaveo=Verbose

// Check Output Log for "LogEvaveo" messages
```

## 📦 Platform Support

### Unity
- ✅ Unity 2020.3 or newer
- ✅ Android (Meta Quest, Pico, etc.)
- ✅ Windows (PC VR)
- ✅ iOS (Vision Pro)
- ✅ WebGL

### Unreal Engine
- ✅ Unreal Engine 4.27+
- ✅ Unreal Engine 5.0+
- ✅ Android (Meta Quest, Pico, etc.)
- ✅ Windows (PC VR)

## 🤝 Contributing

We welcome contributions! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## 📄 License

This SDK is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## 🆘 Support

- 📧 Email: support@evaveo.com
- 💬 Discord: [Join our community](https://discord.gg/evaveo)
- 📖 Documentation: [docs.vrmanager.evaveo.com](https://docs.vrmanager.evaveo.com)
- 🐛 Issues: [GitHub Issues](https://github.com/Evaveo/VR-MANAGER-SDK-/issues)

## 🎯 Examples

Check out the `Examples/` folder for complete sample projects:
- `Examples/Unity/SimpleVRGame` - Basic Unity VR game with SDK
- `Examples/Unreal/SimpleVRGame` - Basic Unreal VR game with SDK

## 🔄 Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history.

## ⭐ Show Your Support

If you find this SDK useful, please:
- ⭐ Star this repository
- 🐦 Share on social media
- 📝 Write a review

---

Made with ❤️ by [EVAVEO](https://evaveo.com)
