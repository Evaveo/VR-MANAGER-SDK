using UnityEngine;
using System;

namespace EVAVEO_VR_Manager_SDK
{
    public class EvaCrashHandler
    {
        private EvaNetworking networking;

        public EvaCrashHandler(EvaNetworking net)
        {
            networking = net;
        }

        public void RegisterHandlers()
        {
            Application.logMessageReceived += HandleLog;
            Debug.Log("[EVAVEO VR Manager] Crash handler registered");
        }

        public void UnregisterHandlers()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception || type == LogType.Error)
            {
                networking.SendEvent("error", new
                {
                    message = logString,
                    stackTrace = stackTrace,
                    type = type.ToString()
                });

                Debug.LogWarning($"[EVAVEO VR Manager] Error logged: {logString}");
            }
        }
    }
}
