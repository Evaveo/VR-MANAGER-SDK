// Copyright EVAVEO 2025. All Rights Reserved.

#include "EvaveoSDK.h"
#include "Http.h"
#include "Json.h"
#include "JsonUtilities.h"
#include "Misc/Guid.h"

// Static member initialization
bool UEvaveoSDK::bIsInitialized = false;
bool UEvaveoSDK::bIsEnabled = true;
FString UEvaveoSDK::ApiKey = TEXT("");
FString UEvaveoSDK::ApiUrl = TEXT("https://api.vrmanager.evaveo.com/api/tracking");
FString UEvaveoSDK::SessionId = TEXT("");
FString UEvaveoSDK::UserId = TEXT("");

void UEvaveoSDK::Initialize(const FString& InApiKey)
{
    if (bIsInitialized)
    {
        UE_LOG(LogTemp, Warning, TEXT("[EVAVEO VR Manager] SDK already initialized"));
        return;
    }

    if (InApiKey.IsEmpty())
    {
        UE_LOG(LogTemp, Error, TEXT("[EVAVEO VR Manager] API key cannot be empty"));
        return;
    }

    ApiKey = InApiKey;
    bIsInitialized = true;

    UE_LOG(LogTemp, Log, TEXT("[EVAVEO VR Manager] SDK Initialized successfully"));

    // Start session
    if (bIsEnabled)
    {
        StartSession();
    }
}

void UEvaveoSDK::TrackEvent(const FString& EventName, const FString& EventData)
{
    if (!bIsInitialized || !bIsEnabled)
    {
        return;
    }

    SendEvent(EventName, EventData);
}

void UEvaveoSDK::SetEnabled(bool bEnabled)
{
    bIsEnabled = bEnabled;
    UE_LOG(LogTemp, Log, TEXT("[EVAVEO VR Manager] Tracking %s"), bEnabled ? TEXT("enabled") : TEXT("disabled"));
}

void UEvaveoSDK::SetUserId(const FString& InUserId)
{
    UserId = InUserId;
}

bool UEvaveoSDK::IsInitialized()
{
    return bIsInitialized;
}

bool UEvaveoSDK::IsEnabled()
{
    return bIsEnabled;
}

void UEvaveoSDK::StartSession()
{
    // Generate session ID
    SessionId = FGuid::NewGuid().ToString();

    // Prepare session data
    TSharedPtr<FJsonObject> JsonObject = MakeShareable(new FJsonObject);
    JsonObject->SetStringField(TEXT("sessionId"), SessionId);
    JsonObject->SetStringField(TEXT("deviceModel"), FPlatformMisc::GetDeviceMakeAndModel());
    JsonObject->SetStringField(TEXT("osVersion"), FPlatformMisc::GetOSVersion());
    JsonObject->SetStringField(TEXT("appVersion"), FApp::GetBuildVersion());
    JsonObject->SetStringField(TEXT("startTime"), FDateTime::UtcNow().ToIso8601());

    if (!UserId.IsEmpty())
    {
        JsonObject->SetStringField(TEXT("userId"), UserId);
    }

    // Convert to JSON string
    FString JsonString;
    TSharedRef<TJsonWriter<>> Writer = TJsonWriterFactory<>::Create(&JsonString);
    FJsonSerializer::Serialize(JsonObject.ToSharedRef(), Writer);

    // Send to server
    SendHttpRequest(TEXT("/app/launch"), JsonString);

    UE_LOG(LogTemp, Log, TEXT("[EVAVEO VR Manager] Session started: %s"), *SessionId);
}

void UEvaveoSDK::EndSession()
{
    if (SessionId.IsEmpty())
    {
        return;
    }

    // Prepare end session data
    TSharedPtr<FJsonObject> JsonObject = MakeShareable(new FJsonObject);
    JsonObject->SetStringField(TEXT("sessionId"), SessionId);
    JsonObject->SetStringField(TEXT("endTime"), FDateTime::UtcNow().ToIso8601());

    // Convert to JSON string
    FString JsonString;
    TSharedRef<TJsonWriter<>> Writer = TJsonWriterFactory<>::Create(&JsonString);
    FJsonSerializer::Serialize(JsonObject.ToSharedRef(), Writer);

    // Send to server
    SendHttpRequest(TEXT("/app/close"), JsonString);

    UE_LOG(LogTemp, Log, TEXT("[EVAVEO VR Manager] Session ended"));
}

void UEvaveoSDK::SendEvent(const FString& EventName, const FString& EventData)
{
    // Prepare event data
    TSharedPtr<FJsonObject> JsonObject = MakeShareable(new FJsonObject);
    JsonObject->SetStringField(TEXT("eventName"), EventName);
    JsonObject->SetStringField(TEXT("eventData"), EventData);
    JsonObject->SetStringField(TEXT("sessionId"), SessionId);
    JsonObject->SetStringField(TEXT("timestamp"), FDateTime::UtcNow().ToIso8601());

    // Convert to JSON string
    FString JsonString;
    TSharedRef<TJsonWriter<>> Writer = TJsonWriterFactory<>::Create(&JsonString);
    FJsonSerializer::Serialize(JsonObject.ToSharedRef(), Writer);

    // Send to server
    SendHttpRequest(TEXT("/event"), JsonString);
}

void UEvaveoSDK::SendHttpRequest(const FString& Endpoint, const FString& JsonData)
{
    if (!bIsInitialized)
    {
        return;
    }

    // Create HTTP request
    TSharedRef<IHttpRequest, ESPMode::ThreadSafe> HttpRequest = FHttpModule::Get().CreateRequest();
    HttpRequest->SetURL(ApiUrl + Endpoint);
    HttpRequest->SetVerb(TEXT("POST"));
    HttpRequest->SetHeader(TEXT("Content-Type"), TEXT("application/json"));
    HttpRequest->SetHeader(TEXT("X-API-Key"), ApiKey);
    HttpRequest->SetContentAsString(JsonData);

    // Set response callback
    HttpRequest->OnProcessRequestComplete().BindLambda([](FHttpRequestPtr Request, FHttpResponsePtr Response, bool bSuccess)
    {
        if (bSuccess && Response.IsValid())
        {
            UE_LOG(LogTemp, Verbose, TEXT("[EVAVEO VR Manager] Data sent successfully"));
        }
        else
        {
            UE_LOG(LogTemp, Warning, TEXT("[EVAVEO VR Manager] Failed to send data"));
        }
    });

    // Send request
    HttpRequest->ProcessRequest();
}
