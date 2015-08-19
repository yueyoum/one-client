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
    }


    public IEnumerator GetImage(System.Action<Image> callback)
    {
        Debug.Log("GetImage");
        if (imageList.Images.Count == 0)
        {
            // no images, get new
            Debug.Log("No Image, Get New");
            yield return StartCoroutine(GetImages());
        }

        int index = Random.Range(0, imageList.Images.Count - 1);
        Image i = imageList.Images[index];
        imageList.Images.RemoveAt(index);
        
        HTTPRequest req = new HTTPRequest(new System.Uri(i.Url));
        req.Send();
        Debug.Log("Send to get image");
        yield return StartCoroutine(req);

        if(req.Response==null || !req.Response.IsSuccess)
        {
            Debug.Log("image content req error");
            yield break;
        }

        i.Tex = req.Response.DataAsTexture2D;
        callback(i);

        Debug.Log("Done");
    }


    private IEnumerator GetImages()
    {
        HTTPRequest req = new HTTPRequest(new System.Uri("http://one.digitnode.com/images/"));
        req.Send();
        yield return StartCoroutine(req);

        if(!req.Response.IsSuccess)
        {
            Debug.Log("res not success!");
            yield break;
        }

        string data = req.Response.DataAsText;
        imageList = JsonConvert.DeserializeObject<ImageList>(data);
        Debug.Log("Get Done");
        yield return null;
    }
}
