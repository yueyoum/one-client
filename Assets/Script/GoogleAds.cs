using UnityEngine;
using System.Collections;

public class GoogleAds
{

    private static GoogleAds instance = null;
    private GoogleAds()
    {
        GoogleMobileAd.Init();
        GoogleMobileAd.SetBannersUnitID("ca-app-pub-7468307217611497/1999630365", "", "");
        GoogleMobileAd.SetInterstisialsUnitID("ca-app-pub-7468307217611497/5313023562", "", "");
        GoogleMobileAd.SetGender(GoogleGender.Male);
        //GoogleMobileAd.AddKeyword("girl");
        //GoogleMobileAd.AddKeyword("sexy");
        //GoogleMobileAd.AddKeyword("big");
        //GoogleMobileAd.AddKeyword("hot");
        //GoogleMobileAd.AddKeyword("ass");
        GoogleMobileAd.CreateAdBanner(TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
    }

    public static GoogleAds GetInstance()
    {
        if (instance == null)
        {
            instance = new GoogleAds();
        }

        return instance;
    }


    public void ShowInterstitialAd()
    {
        GoogleMobileAd.StartInterstitialAd();
    }
}
