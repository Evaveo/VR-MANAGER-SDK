using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EVAVEO_VR_Manager_SDK
{
    public class EvaNetworking
    {
        private string apiUrl;
        private string apiKey;
        private string userId;
        private Queue<EventData> eventQueue = new Queue<EventData>();
        private bool isSending = false;

        public EvaNetworking(string url, string key)
        {
            apiUrl = url;
            apiKey = key;
        }

        public void SetUserId(string id)
        {
            userId = id;
        }

        public void SendEvent(string eventName, object eventData = null)
        {
            var data = new EventData
            {
                apiKey = apiKey,
                deviceId = SystemInfo.deviceUniqueIdentifier,
                userId = userId,
                eventType = "custom",
                eventName = eventName,
                eventData = eventData,
                timestamp = DateTime.UtcNow.ToString("o")
            };

            eventQueue.Enqueue(data);
            TrySendQueue();
        }

        public void SendSessionStart(string sessionId, string appName)
        {
            var data = new EventData
            {
                apiKey = apiKey,
                deviceId = SystemInfo.deviceUniqueIdentifier,
                userId = userId,
                sessionId = sessionId,
                appName = appName,
                eventType = "session_start",
                eventName = "session_start",
                timestamp = DateTime.UtcNow.ToString("o")
            };

            eventQueue.Enqueue(data);
            TrySendQueue();
        }

        public void SendSessionEnd(string sessionId, float duration)
        {
            var data = new EventData
            {
                apiKey = apiKey,
                deviceId = SystemInfo.deviceUniqueIdentifier,
                userId = userId,
                sessionId = sessionId,
                eventType = "session_end",
                eventName = "session_end",
                sessionDuration = (int)duration,
                timestamp = DateTime.UtcNow.ToString("o")
            };

            eventQueue.Enqueue(data);
            TrySendQueue();
        }

        public void SendPerformanceData(float fps, long memoryUsage, float batteryLevel)
        {
            var data = new EventData
            {
                apiKey = apiKey,
                deviceId = SystemInfo.deviceUniqueIdentifier,
                userId = userId,
                eventType = "performance",
                eventName = "performance_snapshot",
                fps = fps,
                memoryUsage = memoryUsage,
                batteryLevel = batteryLevel,
                timestamp = DateTime.UtcNow.ToString("o")
            };

            eventQueue.Enqueue(data);
            TrySendQueue();
        }

        private void TrySendQueue()
        {
            if (isSending || eventQueue.Count == 0) return;

            isSending = true;
            var data = eventQueue.Dequeue();
            SendToServer(data);
        }

        private void SendToServer(EventData data)
        {
            string json = JsonUtility.ToJson(data);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            UnityWebRequest request = new UnityWebRequest(apiUrl + "/sdk/event", "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            operation.completed += (op) =>
            {
                isSending = false;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("[EVAVEO VR Manager] Event sent successfully");
                }
                else
                {
                    Debug.LogWarning($"[EVAVEO VR Manager] Failed to send event: {request.error}");
                    // Re-queue on failure
                    eventQueue.Enqueue(data);
                }

                request.Dispose();
                TrySendQueue(); // Try next in queue
            };
        }

        [Serializable]
        private class EventData
        {
            public string apiKey;
            public string deviceId;
            public string userId;
            public string appId;
            public string appName;
            public string sessionId;
            public string eventType;
            public string eventName;
            public object eventData;
            public int? sessionDuration;
            public string moduleId;
            public string moduleName;
            public string missionId;
            public string missionName;
            public int? score;
            public int? maxScore;
            public float? successRate;
            public int? duration;
            public int? errorsCount;
            public int? hintsUsed;
            public float? fps;
            public long? memoryUsage;
            public float? batteryLevel;
            public string timestamp;
        }
    }
}
