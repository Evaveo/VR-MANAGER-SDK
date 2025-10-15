# 🚀 EVAVEO VR Manager SDK - Guide d'implémentation complet

## 📋 Table des matières
1. [Installation](#installation)
2. [Configuration de base](#configuration-de-base)
3. [Tracking des sessions](#tracking-des-sessions)
4. [Tracking des modules](#tracking-des-modules)
5. [Tracking des missions](#tracking-des-missions)
6. [Tracking des actions](#tracking-des-actions)
7. [Exemples complets](#exemples-complets)
8. [API Reference](#api-reference)

---

## 📦 Installation

### Unity
```bash
# Via Unity Package Manager
Window → Package Manager → + → Add package from git URL
https://github.com/Evaveo/VR-MANAGER-SDK-.git#unity
```

---

## ⚙️ Configuration de base

### 1. Initialiser le SDK

```csharp
using UnityEngine;
using EVAVEO_VR_Manager_SDK;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Initialiser avec votre API key
        EVAVEO_VR_Manager.Initialize("vr_your_api_key_here");
        
        // Identifier l'utilisateur
        EVAVEO_VR_Manager.SetUserId("employee_12345");
        
        Debug.Log("SDK initialized!");
    }
}
```

---

## 👤 Tracking des sessions

### Démarrage automatique
Le SDK démarre automatiquement une session quand l'app lance.

### Métadonnées utilisateur
```csharp
void Start()
{
    EVAVEO_VR_Manager.Initialize("vr_api_key");
    
    // Ajouter des métadonnées utilisateur
    EVAVEO_VR_Manager.SetUserId("emp_12345");
    EVAVEO_VR_Manager.TrackEvent("user_metadata", new {
        name = "Jean Dupont",
        department = "Production",
        role = "Technicien",
        level = "Senior"
    });
}
```

---

## 📚 Tracking des modules

### Démarrer un module
```csharp
public class ModuleManager : MonoBehaviour
{
    private string currentModuleId;
    private float moduleStartTime;
    
    public void StartModule(string moduleId, string moduleName)
    {
        currentModuleId = moduleId;
        moduleStartTime = Time.time;
        
        EVAVEO_VR_Manager.TrackEvent("module_started", new {
            moduleId = moduleId,
            moduleName = moduleName,
            difficulty = "intermediate",
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"Module started: {moduleName}");
    }
    
    public void CompleteModule(int score, int maxScore)
    {
        float duration = Time.time - moduleStartTime;
        float successRate = (float)score / maxScore * 100f;
        
        EVAVEO_VR_Manager.TrackEvent("module_completed", new {
            moduleId = currentModuleId,
            duration = (int)duration,
            score = score,
            maxScore = maxScore,
            successRate = successRate,
            success = score >= (maxScore * 0.7f), // 70% pour réussir
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"Module completed with score: {score}/{maxScore}");
    }
}
```

---

## 🎯 Tracking des missions

### Structure complète
```csharp
public class MissionManager : MonoBehaviour
{
    [System.Serializable]
    public class MissionStats
    {
        public string missionId;
        public string missionName;
        public string moduleId;
        public float startTime;
        public int score;
        public int maxScore;
        public int errorsCount;
        public int hintsUsed;
        public List<string> actionsPerformed;
    }
    
    private MissionStats currentMission;
    
    public void StartMission(string missionId, string missionName, string moduleId)
    {
        currentMission = new MissionStats
        {
            missionId = missionId,
            missionName = missionName,
            moduleId = moduleId,
            startTime = Time.time,
            score = 0,
            maxScore = 100,
            errorsCount = 0,
            hintsUsed = 0,
            actionsPerformed = new List<string>()
        };
        
        EVAVEO_VR_Manager.TrackEvent("mission_started", new {
            missionId = missionId,
            missionName = missionName,
            moduleId = moduleId,
            timestamp = System.DateTime.UtcNow
        });
    }
    
    public void CompleteMission()
    {
        float duration = Time.time - currentMission.startTime;
        float successRate = (float)currentMission.score / currentMission.maxScore * 100f;
        
        EVAVEO_VR_Manager.TrackEvent("mission_completed", new {
            missionId = currentMission.missionId,
            missionName = currentMission.missionName,
            moduleId = currentMission.moduleId,
            duration = (int)duration,
            score = currentMission.score,
            maxScore = currentMission.maxScore,
            successRate = successRate,
            errorsCount = currentMission.errorsCount,
            hintsUsed = currentMission.hintsUsed,
            actionsCount = currentMission.actionsPerformed.Count,
            timestamp = System.DateTime.UtcNow
        });
        
        Debug.Log($"Mission completed: {currentMission.missionName}");
    }
    
    public void AddScore(int points)
    {
        currentMission.score += points;
        
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "score_gained",
            points = points,
            total_score = currentMission.score,
            missionId = currentMission.missionId
        });
    }
    
    public void RecordError(string errorType)
    {
        currentMission.errorsCount++;
        
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "error",
            error_type = errorType,
            missionId = currentMission.missionId,
            timestamp = System.DateTime.UtcNow
        });
    }
    
    public void UseHint()
    {
        currentMission.hintsUsed++;
        
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "hint_used",
            hints_used = currentMission.hintsUsed,
            missionId = currentMission.missionId
        });
    }
}
```

---

## 🎮 Tracking des actions

### Actions de base
```csharp
public class PlayerActions : MonoBehaviour
{
    private string currentMissionId;
    
    // Grab object
    public void OnGrabObject(string objectName)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "grab_object",
            object_name = objectName,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
    }
    
    // Use tool
    public void OnUseTool(string toolName, bool success)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "use_tool",
            tool_name = toolName,
            success = success,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
    }
    
    // Interact with object
    public void OnInteract(string objectName, string interactionType)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "interact",
            object_name = objectName,
            interaction_type = interactionType, // "press", "pull", "rotate", etc.
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
    }
    
    // Complete task
    public void OnTaskCompleted(string taskName, float timeSpent)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "task_completed",
            task_name = taskName,
            time_spent = (int)timeSpent,
            missionId = currentMissionId,
            timestamp = System.DateTime.UtcNow
        });
    }
}
```

---

## 📊 Exemple complet : Formation Sécurité

```csharp
using UnityEngine;
using EVAVEO_VR_Manager_SDK;

public class SafetyTrainingManager : MonoBehaviour
{
    private string currentModuleId = "safety_height_01";
    private string currentMissionId;
    private float missionStartTime;
    private int score = 0;
    private int errorsCount = 0;
    
    void Start()
    {
        // 1. Initialiser le SDK
        EVAVEO_VR_Manager.Initialize("vr_your_api_key");
        
        // 2. Identifier l'utilisateur
        EVAVEO_VR_Manager.SetUserId("emp_12345");
        EVAVEO_VR_Manager.TrackEvent("user_metadata", new {
            name = "Jean Dupont",
            department = "Production",
            role = "Technicien"
        });
        
        // 3. Démarrer le module
        StartModule();
    }
    
    void StartModule()
    {
        EVAVEO_VR_Manager.TrackEvent("module_started", new {
            moduleId = currentModuleId,
            moduleName = "Sécurité en hauteur - Niveau 1",
            difficulty = "intermediate"
        });
        
        // Démarrer la première mission
        StartMission("check_equipment", "Vérifier l'équipement de sécurité");
    }
    
    void StartMission(string missionId, string missionName)
    {
        currentMissionId = missionId;
        missionStartTime = Time.time;
        score = 0;
        errorsCount = 0;
        
        EVAVEO_VR_Manager.TrackEvent("mission_started", new {
            missionId = missionId,
            missionName = missionName,
            moduleId = currentModuleId
        });
    }
    
    // Appelé quand le joueur attrape le harnais
    public void OnGrabHarness()
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "grab_object",
            object_name = "safety_harness",
            missionId = currentMissionId
        });
        
        score += 10;
    }
    
    // Appelé quand le joueur vérifie le harnais
    public void OnCheckHarness(bool correct)
    {
        EVAVEO_VR_Manager.TrackEvent("user_action", new {
            action_type = "check_equipment",
            equipment = "safety_harness",
            correct = correct,
            missionId = currentMissionId
        });
        
        if (correct)
        {
            score += 20;
        }
        else
        {
            errorsCount++;
            EVAVEO_VR_Manager.TrackEvent("user_action", new {
                action_type = "error",
                error_type = "incorrect_check",
                missionId = currentMissionId
            });
        }
    }
    
    // Appelé quand la mission est terminée
    public void CompleteMission()
    {
        float duration = Time.time - missionStartTime;
        
        EVAVEO_VR_Manager.TrackEvent("mission_completed", new {
            missionId = currentMissionId,
            duration = (int)duration,
            score = score,
            maxScore = 100,
            successRate = (float)score / 100f * 100f,
            errorsCount = errorsCount,
            timestamp = System.DateTime.UtcNow
        });
        
        // Passer à la mission suivante ou terminer le module
        if (score >= 70)
        {
            CompleteModule();
        }
    }
    
    void CompleteModule()
    {
        EVAVEO_VR_Manager.TrackEvent("module_completed", new {
            moduleId = currentModuleId,
            score = score,
            maxScore = 100,
            success = score >= 70
        });
        
        Debug.Log("Module terminé avec succès!");
    }
}
```

---

## 📖 API Reference

### Initialisation
```csharp
EVAVEO_VR_Manager.Initialize(string apiKey)
EVAVEO_VR_Manager.SetUserId(string userId)
EVAVEO_VR_Manager.SetEnabled(bool enabled)
```

### Events
```csharp
EVAVEO_VR_Manager.TrackEvent(string eventName, object eventData)
```

### Types d'events supportés
- `session_start` - Démarrage de session
- `session_end` - Fin de session
- `module_started` - Module démarré
- `module_completed` - Module terminé
- `mission_started` - Mission démarrée
- `mission_completed` - Mission terminée
- `user_action` - Action utilisateur
- `performance` - Données de performance
- `error` - Erreur
- `custom` - Event personnalisé

---

## 🎯 Bonnes pratiques

1. **Toujours identifier l'utilisateur** au démarrage
2. **Tracker les modules et missions** pour avoir une vue d'ensemble
3. **Enregistrer les actions importantes** (pas toutes les actions)
4. **Inclure des timestamps** pour l'analyse temporelle
5. **Ajouter du contexte** (moduleId, missionId) aux actions
6. **Gérer les erreurs** pour améliorer la formation

---

## 📊 Visualisation dans le Dashboard

Une fois intégré, vous verrez dans le dashboard :
- 📈 Sessions par utilisateur
- 🎯 Taux de réussite par module
- ⏱️ Temps moyen par mission
- 🏆 Scores et progression
- 📉 Erreurs communes
- 👥 Utilisateurs les plus actifs

---

**Made with ❤️ by EVAVEO**
