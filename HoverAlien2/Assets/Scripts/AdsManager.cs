using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public string adId;
    string placement = "rewardedVideo";
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
#if UNITY_IOS
{
        adId = "3897128";
}
#elif UNITY_ANDROID
{
        adId = "3897129";
}
#else
{
        adId = "3897128";
}
#endif

        
        Advertisement.Initialize(adId, false);
    }

    // Update is called once per frame
    public void ShowRetryAd()
    {
        StartCoroutine("showRetryAd");
    }

    IEnumerator showRetryAd()
    {
        while (!Advertisement.IsReady(placement))
        {
            yield return null;
        }

        Advertisement.Show(placement);
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            gameController.AdCompleted();
        }
        
        else if (showResult == ShowResult.Skipped)
        {
            gameController.GameOver();
        }

        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning ("The ad did not finish due to an error.");
            gameController.GameOver();
        }
    }

    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, show the ad:
       // if (placementId == myPlacementId) {
        //    Advertisement.Show (myPlacementId);
        //}
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    }
}
