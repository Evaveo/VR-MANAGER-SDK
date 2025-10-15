using UnityEngine;
using System.Collections;

namespace EVAVEO_VR_Manager_SDK
{
    public class EvaPerformance
    {
        private EvaNetworking networking;
        private MonoBehaviour coroutineRunner;
        private Coroutine monitoringCoroutine;
        private float sendInterval = 5f;

        private int frameCount = 0;
        private float fpsTimer = 0f;
        private float currentFps = 0f;

        public EvaPerformance(EvaNetworking net)
        {
            networking = net;
        }

        public void StartMonitoring()
        {
            if (coroutineRunner == null)
            {
                var go = new GameObject("EvaPerformanceMonitor");
                coroutineRunner = go.AddComponent<MonoBehaviourHelper>();
                Object.DontDestroyOnLoad(go);
            }

            monitoringCoroutine = coroutineRunner.StartCoroutine(MonitorPerformance());
            Debug.Log("[EVAVEO VR Manager] Performance monitoring started");
        }

        public void StopMonitoring()
        {
            if (monitoringCoroutine != null && coroutineRunner != null)
            {
                coroutineRunner.StopCoroutine(monitoringCoroutine);
                monitoringCoroutine = null;
            }
        }

        private IEnumerator MonitorPerformance()
        {
            while (true)
            {
                yield return new WaitForSeconds(sendInterval);

                // Calculate FPS
                float fps = 1f / Time.deltaTime;
                
                // Get memory usage
                long memoryUsage = System.GC.GetTotalMemory(false);
                
                // Get battery level (0-1)
                float batteryLevel = SystemInfo.batteryLevel;

                // Send to server
                networking.SendPerformanceData(fps, memoryUsage, batteryLevel);
            }
        }

        // Helper MonoBehaviour for coroutines
        private class MonoBehaviourHelper : MonoBehaviour { }
    }
}
