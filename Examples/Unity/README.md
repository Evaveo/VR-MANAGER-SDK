# 🎮 Unity Example Projects

## 📁 SafetyTraining

### Description
A complete VR safety training application demonstrating full SDK integration.

### Features
- ✅ User identification and metadata
- ✅ Module tracking (start/complete)
- ✅ Mission tracking with scoring
- ✅ Action tracking (grab, use tool, check equipment)
- ✅ Error and hint tracking
- ✅ Performance metrics

### How to Use

1. **Import the SDK**
   ```
   Window → Package Manager → + → Add package from git URL
   https://github.com/Evaveo/VR-MANAGER-SDK-.git#unity
   ```

2. **Add the script to your scene**
   - Create an empty GameObject
   - Add `SafetyTrainingManager.cs` component
   - Configure your API key in the Inspector

3. **Configure User Info**
   - Set `userId`, `userName`, `userDepartment`, `userRole`

4. **Test the simulation**
   - Right-click on the component in Inspector
   - Select "Simulate Training Session"
   - Check the Console for logs

### Integration Points

#### When player grabs an object:
```csharp
SafetyTrainingManager manager = FindObjectOfType<SafetyTrainingManager>();
manager.OnGrabObject("safety_harness");
```

#### When player uses a tool:
```csharp
manager.OnUseTool("carabiner", true); // true = correct usage
```

#### When player checks equipment:
```csharp
manager.OnCheckEquipment("safety_harness", true); // true = correct check
```

#### When player completes a task:
```csharp
manager.OnTaskCompleted("attach_harness");
```

#### When player uses a hint:
```csharp
manager.OnUseHint();
```

#### When mission is complete:
```csharp
manager.CompleteMission();
```

### Expected Analytics

After running this example, you'll see in the VR Manager dashboard:

**Sessions:**
- User: Jean Dupont (Production, Technicien)
- Module: Sécurité en hauteur - Niveau 1

**Module Stats:**
- Score: 85/100
- Duration: ~10 seconds
- Success: ✅

**Mission Stats:**
- Mission: Vérifier l'équipement de sécurité
- Actions: 4 (grab, check, use tool, task)
- Errors: 0
- Hints used: 0

**Actions Tracked:**
1. Grabbed: safety_harness (+10 points)
2. Checked: safety_harness (+30 points)
3. Used tool: carabiner (+20 points)
4. Task completed: attach_harness (+15 points)

---

## 🎯 Next Steps

1. **Customize for your app**: Modify the script to match your training scenarios
2. **Add more missions**: Create multiple missions within a module
3. **Add UI feedback**: Show score and progress to users
4. **Test with real VR**: Deploy to Quest/Pico and test in VR

---

## 📚 Additional Resources

- [Full SDK Documentation](../../README.md)
- [Implementation Guide](../../IMPLEMENTATION_GUIDE.md)
- [API Reference](../../README.md#api-reference)

---

**Need help?** Contact support@evaveo.com
