using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
// using Newtonsoft.Json; 

public class VoiceflowController : MonoBehaviour
{
    public string DM_API_KEY = "YOUR KEY GOES HERE";
    public string userID = "test-user-unity";
    public string textPayload = "Hello";
    // Start is called before the first frame update
    void Start()
    {
        LaunchVoiceflow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InteractVoiceflow(string payloadJson) {
        StartCoroutine(InteractVoiceflowWorker(payloadJson));
    }

    IEnumerator InteractVoiceflowWorker(string payloadJson)
    {
        string url = $"https://general-runtime.voiceflow.com/state/user/{userID}/interact";
        using (UnityWebRequest webRequest = new UnityWebRequest(url,"POST"))
        {
            webRequest.SetRequestHeader("Authorization", DM_API_KEY);
            webRequest.SetRequestHeader("content-type", "application/json");
            webRequest.SetRequestHeader("accept", "application/json");

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(payloadJson);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            Debug.Log(payloadJson);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }

            // wrap the response in a wrapper class so that it can be deserialized correctly
            string wrappedResponse = "{\"items\":" + webRequest.downloadHandler.text + "}";

            GenericResponseWrapper genericResponses = JsonUtility.FromJson<GenericResponseWrapper>(wrappedResponse);
            Debug.Log(genericResponses.items[0].type);
        }
    }


    void LaunchVoiceflow() {
        InterractPayload payload = new InterractPayload {
            action = new InterractAction()
        };

        string payloadJson = JsonUtility.ToJson(payload);
        InteractVoiceflow(payloadJson);
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

[System.Serializable]
public class GenericResponseWrapper
{
    public List<GenericResponseItem> items;
}

[System.Serializable]
public class GenericResponseItem
{
    public string type;
}