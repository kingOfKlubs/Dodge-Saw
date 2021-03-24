using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    string placement = "rewardedVideo";
    string appStoreId = "4061387";
    string googleStoreId = "4061386";

    public bool isTargetAppstore;
    public bool isTestMode;
    public bool hasWatched = false;
    public GameObject AdButton;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        InitializeAd();
    }

    void Update() {
        if (hasWatched) {
            AdButton.SetActive(false);
        }
    }

    public void InitializeAd() {
        if (isTargetAppstore) {
            Advertisement.Initialize(appStoreId,isTestMode);
            return;
        }
        Advertisement.Initialize(googleStoreId, isTestMode);
    }

    public void ShowAd() {
        if (!Advertisement.IsReady(placement)) { return; }
        Advertisement.Show(placement);
    }


    public void OnUnityAdsDidError(string message) {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {

        if(showResult == ShowResult.Finished) {
            //reward the player here
            BankManager.instance.AddMoneyToBank(3);
            GoldManager gm = FindObjectOfType<GoldManager>();
            gm.AddCoins(new Vector2(0, 0), 3);
            AudioManager.instance.Play("TapCollect");
            hasWatched = true;
            Debug.Log("Finished the ad");
        }else if(showResult == ShowResult.Failed) {
            //Well...this wasn't supposed to happen!
        }
    }

    public void OnUnityAdsDidStart(string placementId) {
    }

    public void OnUnityAdsReady(string placementId) {
    }

    private void OnDestroy() {
        Advertisement.RemoveListener(this)
;    }
}
