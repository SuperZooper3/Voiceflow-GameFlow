using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using Newtonsoft.Json;
using TMPro;

public class VoiceflowController : MonoBehaviour
{
    public string DM_API_KEY = "YOUR KEY GOES HERE";
    public string userID = "test-user-unity";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InteractVoiceflow(string payloadJson, ResponseHandlerPackage responseHandlerPackage) {
        StartCoroutine(InteractVoiceflowWorker(payloadJson, responseHandlerPackage));
    }

    IEnumerator InteractVoiceflowWorker(string payloadJson, ResponseHandlerPackage responseHandlerPackage)
    {
        string url = $"https://general-runtime.voiceflow.com/state/user/{userID}/interact";
        using (UnityWebRequest webRequest = new UnityWebRequest(url,"POST"))
        {
            webRequest.SetRequestHeader("Authorization", DM_API_KEY);
            webRequest.SetRequestHeader("content-type", "application/json");
            webRequest.SetRequestHeader("accept", "application/json");
            webRequest.SetRequestHeader("versionID","production");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(payloadJson);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            Debug.Log(payloadJson);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Voiceflow interact error: {webRequest.error} - {webRequest.downloadHandler.text}");
                yield break;
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }

            // wrap the response in a wrapper class so that it can be deserialized correctly
            string wrappedResponse = "{\"items\":" + webRequest.downloadHandler.text + "}";

            GenericResponseWrapper genericResponses = JsonConvert.DeserializeObject<GenericResponseWrapper>(wrappedResponse);
            foreach (var item in genericResponses.items)
            {
                Debug.Log(item.type);
                switch (item.type)
                {
                    case "text":
                        TextResponsePayload textPayload = item.payload.ToObject<TextResponsePayload>();
                        responseHandlerPackage.textHandler(textPayload.message);
                        break;
                    case "custom":
                        CustomResponsePayload customPayload = item.payload.ToObject<CustomResponsePayload>();
                        responseHandlerPackage.customHandler(customPayload.value);
                        break;
                    default:
                        Debug.LogWarning("Unknown type: " + item.type);
                        break;
                }
            }
        }
    }

    public void LaunchVoiceflow(ResponseHandlerPackage responseHandlerPackage) {
        InterractPayload payload = new InterractPayload {
            action = new InterractAction()
        };

        string payloadJson = JsonConvert.SerializeObject(payload);
        InteractVoiceflow(payloadJson, responseHandlerPackage);
    }

    public void SendTextVoiceflow(string text, ResponseHandlerPackage responseHandlerPackage) {
        InterractPayload payload = new InterractPayload {
            action = new InterractAction()
        };
        payload.action.type = "text";
        payload.action.payload = text;

        string payloadJson = JsonConvert.SerializeObject(payload);
        InteractVoiceflow(payloadJson, responseHandlerPackage);
    }
}

[System.Serializable]
public class InterractPayload {
    public InterractAction action;
}

[System.Serializable]
public class InterractAction {
    public string type = "launch";
    public string payload = "";
}

public class GenericResponseWrapper
{
    public List<GenericResponseItem> items;
}

public class GenericResponseItem
{
    public string type;
    public Newtonsoft.Json.Linq.JToken payload;
}

public abstract class ResponsePayload{
}

public class TextResponsePayload : ResponsePayload {
    public string message;
}

public class CustomResponsePayload : ResponsePayload {
    public int value;
}

public class ResponseHandlerPackage {
    public Action<string> textHandler;
    public Action<int> customHandler;
}