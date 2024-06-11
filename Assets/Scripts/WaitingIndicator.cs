using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingIndicator : MonoBehaviour
{
    public TextMeshProUGUI dotsTextBox;
    public int framesBetweenDots = 30;
    public int numberOfDots = 3;
    private string currentDots = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.frameCount % framesBetweenDots == 0) {
            currentDots += ".";
            if (currentDots.Length > numberOfDots) {
                currentDots = "";
            }
            dotsTextBox.text = currentDots;
        }
    }
}
