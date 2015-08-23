using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Main : MonoBehaviour
{
    private int screenWidth;
    private int screenHeight;
    private float ratio;

    private ConfigManager configManager;
    private ImageFactory imageFactory;

    private RectTransform btnClick;
    private Button btn;
    private RawImage rawImage;
    private Canvas uiLoading;
    private Canvas uiError;


    void Awake()
    {
        btnClick = GameObject.Find("TouchIcon").GetComponent<RectTransform>();
        btn = GameObject.Find("MainPanel").GetComponent<Button>();

        rawImage = GameObject.Find("MainImage").GetComponent<RawImage>();
        uiLoading = GameObject.Find("UILoading").GetComponent<Canvas>();
        uiError = GameObject.Find("UIError").GetComponent<Canvas>();

        uiError.gameObject.SetActive(false);

        imageFactory = ImageFactory.GetInstance();
        configManager = ConfigManager.GetInstance();
        StartGetConfig();
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

    private void ToggleLoadingUI(bool display)
    {
        if (display)
        {
            uiLoading.gameObject.SetActive(true);
            btn.interactable = false;
            uiError.gameObject.SetActive(false);
        }
        else
        {
            uiLoading.gameObject.SetActive(false);
            btn.interactable = true;
        }

        ToggleClickIcon(false);
    }

    private void ToggleClickIcon(bool display)
    {
        btnClick.gameObject.SetActive(display);
    }

    private void StartGetConfig()
    {
        ToggleLoadingUI(true);
        StartCoroutine(configManager.GetConfig(ToggleLoadingUI, ToggleClickIcon, ShowErrorUI));
    }


    public void Clicked()
    {
        ToggleLoadingUI(true);
        if(configManager.IsConfiged)
        {
            StartCoroutine(imageFactory.GetImage(ShowImage, ShowErrorUI));
            configManager.Clicked();
        }
        else
        {
            StartGetConfig();
        }
    }


    private void ShowImage(Image i)
    {
        ToggleLoadingUI(false);
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

    private void ShowErrorUI(string message)
    {
        Debug.LogError(message);
        ToggleLoadingUI(false);
        uiError.gameObject.SetActive(true);
    }
}
