using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceController : MonoBehaviour
{
    public Image selfImage;
    public Texture2D normalFace;
    public Texture2D sadFace;
    public Texture2D angryFace;
    public Texture2D distractedFace;
    public Texture2D panicFace;
    public Texture2D surprisedFace;
    public Texture2D upsetFace;
    public Texture2D phoneFace;
    public Texture2D badFace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFace(string face) {
        switch (face) {
            case "normal":
                selfImage.sprite = Sprite.Create(normalFace, new Rect(0, 0, normalFace.width, normalFace.height), new Vector2(0.5f, 0.5f));
                break;
            case "sad":
                selfImage.sprite = Sprite.Create(sadFace, new Rect(0, 0, sadFace.width, sadFace.height), new Vector2(0.5f, 0.5f));
                break;
            case "angry":
                selfImage.sprite = Sprite.Create(angryFace, new Rect(0, 0, angryFace.width, angryFace.height), new Vector2(0.5f, 0.5f));
                break;
            case "distracted":
                selfImage.sprite = Sprite.Create(distractedFace, new Rect(0, 0, distractedFace.width, distractedFace.height), new Vector2(0.5f, 0.5f));
                break;
            case "panic":
                selfImage.sprite = Sprite.Create(panicFace, new Rect(0, 0, panicFace.width, panicFace.height), new Vector2(0.5f, 0.5f));
                break;
            case "surprised":
                selfImage.sprite = Sprite.Create(surprisedFace, new Rect(0, 0, surprisedFace.width, surprisedFace.height), new Vector2(0.5f, 0.5f));
                break;
            case "upset":
                selfImage.sprite = Sprite.Create(upsetFace, new Rect(0, 0, upsetFace.width, upsetFace.height), new Vector2(0.5f, 0.5f));
                break;
            case "phone":
                selfImage.sprite = Sprite.Create(phoneFace, new Rect(0, 0, phoneFace.width, phoneFace.height), new Vector2(0.5f, 0.5f));
                break;
            default:
                selfImage.sprite = Sprite.Create(badFace, new Rect(0, 0, badFace.width, badFace.height), new Vector2(0.5f, 0.5f));
                break;
        }
    }
}
