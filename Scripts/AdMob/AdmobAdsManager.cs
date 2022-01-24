using UnityEngine;

public class AdmobAdsManager : MonoBehaviour
{
    private AdmobAds admobAds;

    private void Awake()
    {
        admobAds = AdmobAds.instance;

    }

    public void ShowRewardedAd()
    {
        admobAds.ShowRewardedAd();
    }

}
