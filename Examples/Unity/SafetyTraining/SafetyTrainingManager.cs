using UnityEngine;
using EVAVEO_VR_Manager_SDK;
using System.Collections.Generic;

/// <summary>
/// Example: Safety Training VR Application
/// This demonstrates how to integrate EVAVEO VR Manager SDK into a training app
/// </summary>
public class SafetyTrainingManager : MonoBehaviour
{
    [Header("SDK Configuration")]
    [SerializeField] private string apiKey = "vr_your_api_key_here";
    
    [Header("User Information")]
    [SerializeField] private string userId = "emp_12345";
    [SerializeField] private string userName = "Jean Dupont";
    [SerializeField] private string userDepartment = "Production";
    [SerializeField] private string userRole = "Technicien";
    
    [Header("Module Configuration")]
    [SerializeField] private string moduleId = "safety_height_01";
    [SerializeField] private string moduleName = "Sécurité en hauteur - Niveau 1";
    
    // Current state
    private string currentMissionId;
    private float missionStartTime;
    private int score = 0;
    private int maxScore = 100;
    private int errorsCount = 0;
    private int hintsUsed = 0;
    private List<string> completedTasks = new List<string>();
    
    void Start()
    {
        InitializeSDK();
        StartModule();
    }
    
    /// <summary>
    /// Initialize the EVAVEO VR Manager SDK
    /// </summary>
    void InitializeSDK()
    {
        // Initialize SDK
        EVAVEO_VR_Manager.Initialize(apiKey);
        
        // Set user ID
        EVAVEO_VR_Manager.SetUserId(userId);
        
        // Send user metadata
        EVAVEO_VR_Manager.TrackEvent("user_metadata", new {
            name = userName,
            department = userDepartment,
            role = userRole,
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"[Safety Training] SDK initialized for user: {userName}");
    }
    
    /// <summary>
    /// Start the training module
    /// </summary>
    void StartModule()
    {
        EVAVEO_VR_Manager.TrackEvent("module_started", new {
            moduleId = moduleId,
            moduleName = moduleName,
            difficulty = "intermediate",
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"[Safety Training] Module started: {moduleName}");
        
        // Start first mission
        StartMission("check_equipment", "Vérifier l'équipement de sécurité");
    }
    
    /// <summary>
    /// Start a mission within the module
    /// </summary>
    public void StartMission(string missionId, string missionName)
    {
        currentMissionId = missionId;
        missionStartTime = Time.time;
        score = 0;
        errorsCount = 0;
        hintsUsed = 0;
        completedTasks.Clear();
        
        EVAVEO_VR_Manager.TrackEvent("mission_started", new {
            missionId = missionId,
            missionName = missionName,
            moduleId = moduleId,
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"[Safety Training] Mission started: {missionName}");
    }
    
    /// <summary>
    /// Called when player grabs an object
    /// </summary>
    public void OnGrabObject(string objectName)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "grab_object",
            object_name = objectName,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
        
        // Award points
        AddScore(10);
        
        Debug.Log($"[Safety Training] Grabbed: {objectName}");
    }
    
    /// <summary>
    /// Called when player uses a tool
    /// </summary>
    public void OnUseTool(string toolName, bool correct)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "use_tool",
            tool_name = toolName,
            correct = correct,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
        
        if (correct)
        {
            AddScore(20);
        }
        else
        {
            RecordError("incorrect_tool_usage");
        }
        
        Debug.Log($"[Safety Training] Used tool: {toolName} - {(correct ? "Correct" : "Incorrect")}");
    }
    
    /// <summary>
    /// Called when player checks equipment
    /// </summary>
    public void OnCheckEquipment(string equipmentName, bool correct)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "check_equipment",
            equipment = equipmentName,
            correct = correct,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
        
        if (correct)
        {
            AddScore(30);
            completedTasks.Add($"check_{equipmentName}");
        }
        else
        {
            RecordError("incorrect_check");
        }
        
        Debug.Log($"[Safety Training] Checked equipment: {equipmentName} - {(correct ? "Correct" : "Incorrect")}");
    }
    
    /// <summary>
    /// Called when player completes a task
    /// </summary>
    public void OnTaskCompleted(string taskName)
    {
        float timeSpent = Time.time - missionStartTime;
        
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "task_completed",
            task_name = taskName,
            time_spent = (int)timeSpent,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
        
        completedTasks.Add(taskName);
        AddScore(15);
        
        Debug.Log($"[Safety Training] Task completed: {taskName}");
    }
    
    /// <summary>
    /// Add score points
    /// </summary>
    void AddScore(int points)
    {
        score += points;
        score = Mathf.Min(score, maxScore);
        
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "score_gained",
            points = points,
            total_score = score,
            missionId = currentMissionId
        });
    }
    
    /// <summary>
    /// Record an error
    /// </summary>
    void RecordError(string errorType)
    {
        errorsCount++;
        
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "error",
            error_type = errorType,
            total_errors = errorsCount,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
    }
    
    /// <summary>
    /// Called when player uses a hint
    /// </summary>
    public void OnUseHint()
    {
        hintsUsed++;
        
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "hint_used",
            hints_used = hintsUsed,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"[Safety Training] Hint used (Total: {hintsUsed})");
    }
    
    /// <summary>
    /// Complete the current mission
    /// </summary>
    public void CompleteMission()
    {
        float duration = Time.time - missionStartTime;
        float successRate = (float)score / maxScore * 100f;
        
        EVAVEO_VR_Manager.TrackEvent("mission_completed", new {
            missionId = currentMissionId,
            duration = (int)duration,
            score = score,
            maxScore = maxScore,
            successRate = successRate,
            errorsCount = errorsCount,
            hintsUsed = hintsUsed,
            tasksCompleted = completedTasks.Count,
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"[Safety Training] Mission completed - Score: {score}/{maxScore} ({successRate:F1}%)");
        
        // Check if module is complete
        if (score >= 70) // 70% to pass
        {
            CompleteModule();
        }
        else
        {
            Debug.Log("[Safety Training] Score too low. Please retry.");
        }
    }
    
    /// <summary>
    /// Complete the entire module
    /// </summary>
    void CompleteModule()
    {
        EVAVEO_VR_Manager.TrackEvent("module_completed", new {
            moduleId = moduleId,
            moduleName = moduleName,
            score = score,
            maxScore = maxScore,
            success = score >= 70,
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"[Safety Training] Module completed successfully!");
    }
    
    /// <summary>
    /// Example: Simulate a complete training session
    /// </summary>
    [ContextMenu("Simulate Training Session")]
    public void SimulateTrainingSession()
    {
        StartCoroutine(SimulateSession());
    }
    
    System.Collections.IEnumerator SimulateSession()
    {
        Debug.Log("=== Starting Simulated Training Session ===");
        
        // Grab harness
        yield return new WaitForSeconds(2f);
        OnGrabObject("safety_harness");
        
        // Check harness
        yield return new WaitForSeconds(3f);
        OnCheckEquipment("safety_harness", true);
        
        // Use tool
        yield return new WaitForSeconds(2f);
        OnUseTool("carabiner", true);
        
        // Complete task
        yield return new WaitForSeconds(2f);
        OnTaskCompleted("attach_harness");
        
        // Complete mission
        yield return new WaitForSeconds(1f);
        CompleteMission();
        
        Debug.Log("=== Training Session Completed ===");
    }
}
