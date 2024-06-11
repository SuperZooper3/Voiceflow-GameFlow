using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ConversationController : MonoBehaviour
{
    public VoiceflowController voiceflowController;

    public TextMeshProUGUI outputTextBox;
    public Queue<string> conversationQueue = new Queue<string>();
    public string textPayload = "Hello";

    private ResponseHandlerPackage responseHandlerPackage = new ResponseHandlerPackage();

    public void SendButtonClicked() {
        Debug.Log("Send button clicked");
        voiceflowController.SendTextVoiceflow(textPayload, responseHandlerPackage);
    }

    public void UpdateTextPayload(string newText) {
        textPayload = newText;
    }

    public ConversationController() {
        responseHandlerPackage = new ResponseHandlerPackage{
            textHandler = EnqueueChatMessage,
            customHandler = HandleCustomEvent,
        };
    }


    // Start is called before the first frame update
    void Start()
    {
        voiceflowController.LaunchVoiceflow(responseHandlerPackage);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void EnqueueChatMessage(string newText) {
        outputTextBox.text += newText + "\n";
        // conversationQueue.Enqueue(newText);
    }

    public void HandleCustomEvent(int value) {
        Debug.Log("Custom event: " + value);
    }


}
