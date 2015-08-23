using UnityEngine;
using System.Collections;

using BestHTTP;
using Newtonsoft.Json;

public class AppGoogleConfig
{
    public int min_times { get; set; }
    public int max_times { get; set; }
}

public class AppConfig
{
    public AppGoogleConfig googleconfig { get; set; }
}


public class ConfigManager : MonoBehaviour
{
    public bool IsConfiged
    {
        get;
        private set;
    }

    private int clickTimes;

    public AppConfig config;
    private static ConfigManager instance = null;

    public static ConfigManager GetInstance()
    {
        if(instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<ConfigManager>();
        }

        return instance;
    }

    private void Awake()
    {
        IsConfiged = false;
        clickTimes = 0;
    }


    public IEnumerator GetConfig(System.Action<bool> callback, System.Action<bool> callback2, System.Action<string> errorCallback)
    {
        HTTPRequest req = new HTTPRequest(new System.Uri("http://one.digitnode.com/config/"));
        req.Send();
        yield return StartCoroutine(req);

        if (req.Response == null || !req.Response.IsSuccess)
        {
            errorCallback("Get Config Error");
            yield break;
        }

        string data = req.Response.DataAsText;
        config = JsonConvert.DeserializeObject<AppConfig>(data);
        IsConfiged = true;

        GoogleAds.GetInstance();

        callback(false);
        callback2(true);
        yield return null;
    }

    public void Clicked()
    {
        clickTimes++;
        if(clickTimes >= Random.Range(config.googleconfig.min_times, config.googleconfig.max_times))
        {
            clickTimes = 0;
            GoogleMobileAd.StartInterstitialAd();
        }
    }
}
