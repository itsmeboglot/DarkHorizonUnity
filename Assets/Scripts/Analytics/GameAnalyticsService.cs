using GameAnalyticsSDK;
using UnityEngine;

namespace Analytics
{
  public class GameAnalyticsService : MonoBehaviour, IGameAnalyticsATTListener
  {
    private void Start()
    {
      if (Application.platform == RuntimePlatform.IPhonePlayer)
        GameAnalytics.RequestTrackingAuthorization(this);
      else
        GameAnalytics.Initialize();
    }

    public void GameAnalyticsATTListenerNotDetermined() =>
      GameAnalytics.Initialize();

    public void GameAnalyticsATTListenerRestricted() =>
      GameAnalytics.Initialize();

    public void GameAnalyticsATTListenerDenied() =>
      GameAnalytics.Initialize();

    public void GameAnalyticsATTListenerAuthorized() =>
      GameAnalytics.Initialize();

    // GameAnalytics.NewResourceEvent(GA_Resource.GAResourceFlowType.GAResourceFlowTypeSource, “Gems”, 400, “IAP”, “Coins400”);
    public void NewResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId) =>
      GameAnalytics.NewResourceEvent(flowType, currency, amount, itemType, itemId);

    public void NewProgressionEvent(GAProgressionStatus status, string progression, int amount) =>
      GameAnalytics.NewProgressionEvent(status, progression, amount);

    public void NewDesignEvent(string eventName, float eventValue) =>
      GameAnalytics.NewDesignEvent(eventName, eventValue);

    public void NewErrorEvent(GAErrorSeverity severity, string message) => 
      GameAnalytics.NewErrorEvent(severity, message);
  }
}