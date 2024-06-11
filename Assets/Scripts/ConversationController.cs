using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ConversationController : MonoBehaviour
{
    public VoiceflowController voiceflowController;

    public TextMeshProUGUI outputTextBox;
    public string textPayload = "";
    public GameObject readingItems;
    public GameObject inputItems;

    private bool isWaitingForResponse = false;
    private bool isReadingResponse = false;
    private Queue<string> conversationQueue = new Queue<string>();
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
        if (isReadingResponse) {
            readingItems.SetActive(true);
            inputItems.SetActive(false);
        } else if (isWaitingForResponse) {
            readingItems.SetActive(false);
            inputItems.SetActive(false);
        } else {
            readingItems.SetActive(false);
            inputItems.SetActive(true);
        }
    }

    void SetOutputText(string newText) {
        outputTextBox.text = newText;
    }

    public void EnqueueChatMessage(string newText) {
        isWaitingForResponse = false;
        Debug.Log("EnqueueChatMessage: " + newText);
        Debug.Log("Conversation queue count: " + conversationQueue.Count);
        if (isReadingResponse) {
            conversationQueue.Enqueue(newText);
        } else {
            SetOutputText(newText);
        }
        isReadingResponse = true;
    }

    public void HandleCustomEvent(int value) {
        isWaitingForResponse = false;
        Debug.Log("Custom event: " + value);
    }

    public void ContinueButtonClicked() {
        if (conversationQueue.Count > 0) {
            SetOutputText(conversationQueue.Dequeue());
        } else {
            isReadingResponse = false;
        }
    }
}
