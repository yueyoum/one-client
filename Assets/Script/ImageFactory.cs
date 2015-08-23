using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;
using BestHTTP;

public class Image
{
    public string Url { get; set; }
    public int W { get; set; }
    public int H { get; set; }
    public Texture2D Tex { get; set; }
}

public class ImageList
{
    public List<Image> Images { get; set; }
}



public class ImageFactory : MonoBehaviour
{

    private ImageList imageList;

    private static ImageFactory instance = null;
    public static ImageFactory GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<ImageFactory>();
        }

        return instance;
    }

    void Awake()
    {
        imageList = new ImageList();
        imageList.Images = new List<Image>();

        HTTPManager.ConnectTimeout = System.TimeSpan.FromSeconds(10);
        HTTPManager.RequestTimeout = System.TimeSpan.FromSeconds(20);
    }


    public IEnumerator GetImage(System.Action<Image> callback, System.Action<string> errorCallback)
    {
        if (imageList.Images.Count == 0)
        {
            // no images, get new
            yield return StartCoroutine(GetImages(errorCallback));
        }

        if (imageList.Images.Count == 0)
        {
            yield break;
        }

        int index = Random.Range(0, imageList.Images.Count - 1);
        Image i = imageList.Images[index];

        
        HTTPRequest req = new HTTPRequest(new System.Uri(i.Url));
        req.Send();
        yield return StartCoroutine(req);

        if(req.Response==null || !req.Response.IsSuccess)
        {
            errorCallback("Get Image Error");
            yield break;
        }

        imageList.Images.RemoveAt(index);

        i.Tex = req.Response.DataAsTexture2D;
        callback(i);
        yield return null;
    }


    private IEnumerator GetImages(System.Action<string> errorCallback)
    {
        HTTPRequest req = new HTTPRequest(new System.Uri("http://one.digitnode.com/images/"));
        req.Send();
        yield return StartCoroutine(req);

        if(req.Response==null || !req.Response.IsSuccess)
        {
            errorCallback("Get DigitNode Error");
            yield break;
        }

        string data = req.Response.DataAsText;
        imageList = JsonConvert.DeserializeObject<ImageList>(data);
        yield return null;
    }
}
