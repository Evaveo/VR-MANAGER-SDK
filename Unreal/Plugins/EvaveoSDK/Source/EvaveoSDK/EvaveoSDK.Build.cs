// Copyright EVAVEO 2025. All Rights Reserved.

using UnrealBuildTool;

public class EvaveoSDK : ModuleRules
{
    public EvaveoSDK(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(
            new string[]
            {
                "Core",
                "CoreUObject",
                "Engine",
                "Http",
                "Json",
                "JsonUtilities"
            }
        );

        PrivateDependencyModuleNames.AddRange(
            new string[]
            {
                // Add private dependencies here
            }
        );
    }
}
