using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingGIF : MonoBehaviour
{
    public float loopInterval = 0.2f;
    public List<Sprite> loadingSprites;

    private UnityEngine.UI.Image image;
    private int index;
    private int total;
    private float t;


    // Use this for initialization
    void Start()
    {
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        index = 0;
        total = loadingSprites.Count;
        t = loopInterval;

    }

    // Update is called once per frame
    void Update()
    {
        if(t >= loopInterval)
        {
            image.sprite = loadingSprites[index];
            t = 0f;
            index++;
            
            if(index>=total)
            {
                index = 0;
            }
        }

        t += Time.deltaTime;
    }
}
