// Copyright EVAVEO 2025. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "EvaveoSDK.generated.h"

/**
 * EVAVEO VR Manager SDK for Unreal Engine
 * 
 * This SDK allows you to integrate analytics and telemetry into your VR applications.
 * Automatically tracks sessions, performance, crashes, and custom events.
 */
UCLASS()
class EVAVEOSDK_API UEvaveoSDK : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    /**
     * Initialize the SDK with your API key
     * Call this once at game startup (e.g., in GameMode BeginPlay)
     * 
     * @param ApiKey Your EVAVEO VR Manager API key
     */
    UFUNCTION(BlueprintCallable, Category = "EVAVEO VR Manager")
    static void Initialize(const FString& ApiKey);

    /**
     * Track a custom event
     * 
     * @param EventName Name of the event (e.g., "level_complete")
     * @param EventData Optional event data (e.g., "level_5")
     */
    UFUNCTION(BlueprintCallable, Category = "EVAVEO VR Manager")
    static void TrackEvent(const FString& EventName, const FString& EventData = TEXT(""));

    /**
     * Enable or disable tracking
     * 
     * @param bEnabled True to enable tracking, false to disable
     */
    UFUNCTION(BlueprintCallable, Category = "EVAVEO VR Manager")
    static void SetEnabled(bool bEnabled);

    /**
     * Set a custom user identifier
     * 
     * @param UserId User ID to associate with this session
     */
    UFUNCTION(BlueprintCallable, Category = "EVAVEO VR Manager")
    static void SetUserId(const FString& UserId);

    /**
     * Check if SDK is initialized
     * 
     * @return True if initialized, false otherwise
     */
    UFUNCTION(BlueprintPure, Category = "EVAVEO VR Manager")
    static bool IsInitialized();

    /**
     * Check if tracking is enabled
     * 
     * @return True if enabled, false otherwise
     */
    UFUNCTION(BlueprintPure, Category = "EVAVEO VR Manager")
    static bool IsEnabled();

private:
    static bool bIsInitialized;
    static bool bIsEnabled;
    static FString ApiKey;
    static FString ApiUrl;
    static FString SessionId;
    static FString UserId;

    static void StartSession();
    static void EndSession();
    static void SendEvent(const FString& EventName, const FString& EventData);
    static void SendHttpRequest(const FString& Endpoint, const FString& JsonData);
};
