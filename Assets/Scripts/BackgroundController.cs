using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Image selfImage;
    public Texture2D baseCamp;
    public Texture2D gustyCliff;
    public Texture2D darkTemple;
    public Texture2D theSummit;
    public Texture2D deepJungle;

    public void SetBackground(string scene) {
        switch (scene) {
            case "base camp":
                selfImage.sprite = Sprite.Create(baseCamp, new Rect(0, 0, baseCamp.width, baseCamp.height), new Vector2(0.5f, 0.5f));
                break;
            case "gusty cliff":
                selfImage.sprite = Sprite.Create(gustyCliff, new Rect(0, 0, gustyCliff.width, gustyCliff.height), new Vector2(0.5f, 0.5f));
                break;
            case "dark temple":
                selfImage.sprite = Sprite.Create(darkTemple, new Rect(0, 0, darkTemple.width, darkTemple.height), new Vector2(0.5f, 0.5f));
                break;
            case "the summit":
                selfImage.sprite = Sprite.Create(theSummit, new Rect(0, 0, theSummit.width, theSummit.height), new Vector2(0.5f, 0.5f));
                break;
            case "deep jungle":
                selfImage.sprite = Sprite.Create(deepJungle, new Rect(0, 0, deepJungle.width, deepJungle.height), new Vector2(0.5f, 0.5f));
                break;
            default:
                selfImage.sprite = Sprite.Create(baseCamp, new Rect(0, 0, baseCamp.width, baseCamp.height), new Vector2(0.5f, 0.5f));
                break;
        }
    }
}
