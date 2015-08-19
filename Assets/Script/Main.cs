using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class Main : MonoBehaviour
{
    private int screenWidth;
    private int screenHeight;
    private float ratio;

    private ImageFactory imageFactory;

    public RawImage rawImage;

    void Awake()
    {
        imageFactory = ImageFactory.GetInstance();
    }

    // Use this for initialization
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        ratio = (float)screenHeight / (float)screenWidth;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Clicked()
    {
        StartCoroutine(imageFactory.GetImage(ShowImage));
    }


    private void ShowImage(Image i)
    {
        int newWidth;
        int newHeight;

        float x = (float)i.H / (float)i.W;
        if (x > ratio)
        {
            newHeight = screenHeight;
            newWidth = (int)(newHeight / x);
        }
        else
        {
            newWidth = screenWidth;
            newHeight = (int)(newWidth * x);
        }

        rawImage.texture = i.Tex;
        rawImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }
}
