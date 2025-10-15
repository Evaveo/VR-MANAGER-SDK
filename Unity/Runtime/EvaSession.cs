using UnityEngine;
using System;

namespace EVAVEO_VR_Manager_SDK
{
    public class EvaSession
    {
        private EvaNetworking networking;
        private string sessionId;
        private float sessionStartTime;
        private bool isActive = false;

        public EvaSession(EvaNetworking net)
        {
            networking = net;
        }

        public void StartSession()
        {
            if (isActive) return;

            sessionId = Guid.NewGuid().ToString();
            sessionStartTime = Time.realtimeSinceStartup;
            isActive = true;

            networking.SendSessionStart(sessionId, Application.productName);
            Debug.Log($"[EVAVEO VR Manager] Session started: {sessionId}");
        }

        public void EndSession()
        {
            if (!isActive) return;

            float duration = Time.realtimeSinceStartup - sessionStartTime;
            networking.SendSessionEnd(sessionId, duration);
            isActive = false;

            Debug.Log($"[EVAVEO VR Manager] Session ended: {duration:F2}s");
        }

        public void PauseSession()
        {
            if (!isActive) return;
            Debug.Log("[EVAVEO VR Manager] Session paused");
        }

        public void ResumeSession()
        {
            if (!isActive) return;
            Debug.Log("[EVAVEO VR Manager] Session resumed");
        }

        public string GetSessionId() => sessionId;
        public bool IsActive() => isActive;
    }
}
