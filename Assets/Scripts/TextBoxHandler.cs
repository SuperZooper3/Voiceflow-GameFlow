using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextBoxHandler : MonoBehaviour
{
    public string textPayload = "";
    public TextMeshProUGUI textMeshProUGUI;
    public int framesBetweenCharacters = 3;
    private int currentTextCharacter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (currentTextCharacter < textPayload.Length) {
            if (Time.frameCount % framesBetweenCharacters == 0) {
                textMeshProUGUI.text += textPayload[currentTextCharacter];
                currentTextCharacter++;
            }
        }
    }

    public void SetTextPayload(string newText) {
        textMeshProUGUI.text = "";
        currentTextCharacter = 0;
        textPayload = newText;
    }
}
