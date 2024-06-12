using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ConversationController : MonoBehaviour
{
    public VoiceflowController voiceflowController;
    public FaceController faceController;
    public SpeakingController speakingController;
    public BackgroundController backgroundController;
    public InventoryController inventoryController;

    public TextBoxHandler outputTextBox;
    public TMP_InputField inputField;
    public string textPayload = "";
    public GameObject readingItems;
    public GameObject inputItems;
    public GameObject waitingItems;
    public Button[] itemButtons;

    private bool isWaitingForResponse = true;
    private bool isReadingResponse = false;
    private Queue<ConversationEvent> conversationQueue = new Queue<ConversationEvent>();
    private ResponseHandlerPackage responseHandlerPackage = new ResponseHandlerPackage();

    public void SendButtonClicked() {
        Debug.Log("Send button clicked");
        voiceflowController.SendTextVoiceflow(textPayload, responseHandlerPackage);
        isWaitingForResponse = true;
        inputField.text = "";
    }

    public void GiveItemClicked(int slot) {
        string item = inventoryController.inventory[slot];
        if (item != "") {
            voiceflowController.SendTextVoiceflow($"*The climber gifted you their {item}*", responseHandlerPackage);
            isWaitingForResponse = true;
        }
        inventoryController.RemoveItem(slot);
    }

    public void UpdateTextPayload(string newText) {
        textPayload = newText;
    }

    public ConversationController() {
        responseHandlerPackage = new ResponseHandlerPackage{
            textHandler = EnqueueBasicMessage,
            faceTalkHandler = EnqueueFaceTalk,
            spokenFaceTalkHandler = EnqueueSpokenMessage,
            backgroundChangeHandler = EnqueueBackgroundChange,
            itemGiftHandler = EnqueueItemGift
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
            InterractibleItemButtons(false);
        } else if (isWaitingForResponse) {
            readingItems.SetActive(false);
            inputItems.SetActive(false);
            waitingItems.SetActive(true);
            InterractibleItemButtons(false);
        } else {
            readingItems.SetActive(false);
            inputItems.SetActive(true);
            waitingItems.SetActive(false);
            InterractibleItemButtons(true);
        }
    }

    private void InterractibleItemButtons(bool interactible) {
        foreach (Button button in itemButtons) {
            button.interactable = interactible;
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

    public void EnqueueSpokenMessage(string newText, string face, string base64AudioData) {
        SpokenEvent spokenEvent = new SpokenEvent {
            messageContent = newText,
            face = face,
            base64AudioData = base64AudioData,
            outputTextBox = outputTextBox,
            faceController = faceController,
            speakingController = speakingController
        };
        conversationQueue.Enqueue(spokenEvent);
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

    public void EnqueueItemGift(string item) {
        ItemGiftEvent itemGiftEvent = new ItemGiftEvent {
            item = item,
            inventoryController = inventoryController
        };
        conversationQueue.Enqueue(itemGiftEvent);
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

    public class SpokenEvent : ConversationEvent
    {
        public string messageContent { get; set; }
        public string face { get; set; }
        public string base64AudioData { get; set; }
        
        public TextBoxHandler outputTextBox;
        public FaceController faceController;
        public SpeakingController speakingController;

        public override void Handle()
        {
            outputTextBox.SetTextPayload(messageContent);
            faceController.SetFace(face);
            speakingController.PlayBase64Audio(base64AudioData);
            Debug.Log($"Spoken: {messageContent}");
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

    public class ItemGiftEvent : ConversationEvent
    {
        public string item { get; set; }
        public InventoryController inventoryController;

        public override void Handle()
        {
            inventoryController.AddItem(item);
            Debug.Log($"Item: {item}");
        }
    }
}
