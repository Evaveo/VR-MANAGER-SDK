using UnityEngine;
using System;

namespace EVAVEO_VR_Manager_SDK
{
    /// <summary>
    /// Main SDK class for EVAVEO VR Manager analytics integration
    /// </summary>
    public class EVAVEO_VR_Manager : MonoBehaviour
    {
        private static EVAVEO_VR_Manager instance;
        private static string apiKey;
        private static string apiUrl = "https://api.vrmanager.evaveo.com/api/tracking";
        private static bool isEnabled = true;
        
        private EvaSession session;
        private EvaPerformance performance;
        private EvaCrashHandler crashHandler;
        private EvaNetworking networking;

        /// <summary>
        /// Initialize the SDK with your API key
        /// </summary>
        /// <param name="key">Your EVAVEO VR Manager API key</param>
        public static void Initialize(string key)
        {
            if (instance != null)
            {
                Debug.LogWarning("[EVAVEO VR Manager] SDK already initialized");
                return;
            }

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("[EVAVEO VR Manager] API key cannot be empty");
                return;
            }

            apiKey = key;

            // Create SDK GameObject
            GameObject sdkObject = new GameObject("EVAVEO_VR_Manager");
            instance = sdkObject.AddComponent<EVAVEO_VR_Manager>();
            DontDestroyOnLoad(sdkObject);

            Debug.Log("[EVAVEO VR Manager] SDK Initialized successfully");
        }

        /// <summary>
        /// Initialize the SDK with custom configuration
        /// </summary>
        /// <param name="config">Configuration object</param>
        public static void Initialize(EvaveoConfig config)
        {
            if (config == null)
            {
                Debug.LogError("[EVAVEO VR Manager] Config cannot be null");
                return;
            }

            apiUrl = config.ApiUrl ?? apiUrl;
            Initialize(config.ApiKey);
        }

        void Awake()
        {
            // Initialize components
            networking = new EvaNetworking(apiUrl, apiKey);
            session = new EvaSession(networking);
            performance = new EvaPerformance(networking);
            crashHandler = new EvaCrashHandler(networking);

            // Start tracking
            if (isEnabled)
            {
                session.StartSession();
                performance.StartMonitoring();
                crashHandler.RegisterHandlers();
            }
        }

        void OnApplicationQuit()
        {
            if (isEnabled && session != null)
            {
                session.EndSession();
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (!isEnabled || session == null) return;

            if (pauseStatus)
            {
                session.PauseSession();
            }
            else
            {
                session.ResumeSession();
            }
        }

        /// <summary>
        /// Track a custom event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="eventData">Optional event data</param>
        public static void TrackEvent(string eventName, string eventData = null)
        {
            if (instance != null && isEnabled)
            {
                instance.networking.SendEvent(eventName, eventData);
            }
        }

        /// <summary>
        /// Enable or disable tracking
        /// </summary>
        /// <param name="enabled">True to enable, false to disable</param>
        public static void SetEnabled(bool enabled)
        {
            isEnabled = enabled;
            Debug.Log($"[EVAVEO VR Manager] Tracking {(enabled ? "enabled" : "disabled")}");
        }

        /// <summary>
        /// Set a custom user identifier
        /// </summary>
        /// <param name="userId">User ID</param>
        public static void SetUserId(string userId)
        {
            if (instance != null)
            {
                instance.networking.SetUserId(userId);
            }
        }

        /// <summary>
        /// Check if SDK is initialized
        /// </summary>
        public static bool IsInitialized => instance != null;

        /// <summary>
        /// Check if tracking is enabled
        /// </summary>
        public static bool IsEnabled => isEnabled;
    }

    /// <summary>
    /// Configuration class for SDK initialization
    /// </summary>
    [Serializable]
    public class EvaveoConfig
    {
        public string ApiKey;
        public string ApiUrl = "https://api.vrmanager.evaveo.com/api/tracking";
        public bool EnableDebugLogs = false;
        public bool TrackFPS = true;
        public bool TrackBattery = true;
        public float SendInterval = 5.0f;
    }
}
