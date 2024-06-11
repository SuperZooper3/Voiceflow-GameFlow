using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ConversationController : MonoBehaviour
{
    public VoiceflowController voiceflowController;
    public FaceController faceController;

    public TextMeshProUGUI outputTextBox;
    public string textPayload = "";
    public GameObject readingItems;
    public GameObject inputItems;

    private bool isWaitingForResponse = false;
    private bool isReadingResponse = false;
    private Queue<Message> conversationQueue = new Queue<Message>();
    private ResponseHandlerPackage responseHandlerPackage = new ResponseHandlerPackage();

    public void SendButtonClicked() {
        Debug.Log("Send button clicked");
        voiceflowController.SendTextVoiceflow(textPayload, responseHandlerPackage);
        isWaitingForResponse = true;
    }

    public void UpdateTextPayload(string newText) {
        textPayload = newText;
    }

    public ConversationController() {
        responseHandlerPackage = new ResponseHandlerPackage{
            textHandler = EnqueueChatMessage,
            faceTalkHandler = HandleFaceTalk,
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
        if (isReadingResponse) {
            Message message = new Message{
                message = newText,
                face = "bad"
            };
            conversationQueue.Enqueue(message);
        } else {
            SetOutputText(newText);
        }
        isReadingResponse = true;
    }

    public void HandleFaceTalk(string newText, string face) {
        isWaitingForResponse = false;
        if (isReadingResponse) {
            Message message = new Message{
                message = newText,
                face = face
            };
            conversationQueue.Enqueue(message);
        } else {
            SetOutputText(newText);
        }
        isReadingResponse = true;
    }

    public void ContinueButtonClicked() {
        if (conversationQueue.Count > 0) {
            Message message = conversationQueue.Dequeue();
            SetOutputText(message.message);
            faceController.SetFace(message.face);
            Debug.Log("Message and face: " + message.message + " | " + message.face);
        } else {
            isReadingResponse = false;
        }
    }

    private class Message {
        public string message;
        public string face;
    }
}
