using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ConversationController : MonoBehaviour
{
    public VoiceflowController voiceflowController;
    public FaceController faceController;
    public BackgroundController backgroundController;

    public TextBoxHandler outputTextBox;
    public TMP_InputField inputField;
    public string textPayload = "";
    public GameObject readingItems;
    public GameObject inputItems;
    public GameObject waitingItems;

    private bool isWaitingForResponse = false;
    private bool isReadingResponse = false;
    private Queue<ConversationEvent> conversationQueue = new Queue<ConversationEvent>();
    private ResponseHandlerPackage responseHandlerPackage = new ResponseHandlerPackage();

    public void SendButtonClicked() {
        Debug.Log("Send button clicked");
        voiceflowController.SendTextVoiceflow(textPayload, responseHandlerPackage);
        isWaitingForResponse = true;
        inputField.text = "";
    }

    public void UpdateTextPayload(string newText) {
        textPayload = newText;
    }

    public ConversationController() {
        responseHandlerPackage = new ResponseHandlerPackage{
            textHandler = EnqueueBasicMessage,
            faceTalkHandler = EnqueueFaceTalk,
            backgroundChangeHandler = EnqueueBackgroundChange
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
            waitingItems.SetActive(false);
        } else if (isWaitingForResponse) {
            readingItems.SetActive(false);
            inputItems.SetActive(false);
            waitingItems.SetActive(true);
        } else {
            readingItems.SetActive(false);
            inputItems.SetActive(true);
            waitingItems.SetActive(false);
        }
    }

    private void EnqueueFirstMessageCheck() {
        if (!isReadingResponse) {
            ContinueButtonClicked();
        }
        isReadingResponse = true;
        isWaitingForResponse = false;
    }

    public void EnqueueBasicMessage(string newText) {
        EnqueueFaceTalk(newText, "normal");
    }

    public void EnqueueFaceTalk(string newText, string face) {
        MessageEvent messageEvent = new MessageEvent {
            messageContent = newText,
            face = face,
            outputTextBox = outputTextBox,
            faceController = faceController
        };
        conversationQueue.Enqueue(messageEvent);
        EnqueueFirstMessageCheck();
    }

    public void EnqueueBackgroundChange(string scene) {
        BackgroundEvent backgroundEvent = new BackgroundEvent {
            scene = scene,
            backgroundController = backgroundController
        };
        conversationQueue.Enqueue(backgroundEvent);
        EnqueueFirstMessageCheck();
    }

    public void ContinueButtonClicked() {
        if (conversationQueue.Count > 0) {
            ConversationEvent nextEvent = conversationQueue.Dequeue();
            nextEvent.Handle();
        } else {
            isReadingResponse = false;
        }
    }

    public abstract class ConversationEvent
    {
        public abstract void Handle();
    }

    public class MessageEvent : ConversationEvent
    {
        public string messageContent { get; set; }
        public string face { get; set; }
        public TextBoxHandler outputTextBox;
        public FaceController faceController;

        public override void Handle()
        {
            outputTextBox.SetTextPayload(messageContent);
            faceController.SetFace(face);
            Debug.Log($"Message: {messageContent}");
        }
    }

    public class BackgroundEvent : ConversationEvent
    {
        public string scene { get; set; }
        public BackgroundController backgroundController;

        public override void Handle()
        {
            backgroundController.SetBackground(scene);
            Debug.Log($"Background: {scene}");
        }
    }
}
