using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

public class SpeakingController : MonoBehaviour
{
    public AudioSource audioSource;
    private string outputFileName = "tempSpeakingOutput.mp3";

    void Start()
    {
 
    }

    public void PlayBase64Audio(string base64AudioData)
    {
        StartCoroutine(PlayBase64AudioWorkhorse(base64AudioData));
    }

    public IEnumerator PlayBase64AudioWorkhorse(string base64AudioData)
    {
        byte[] audioBytes = Convert.FromBase64String(base64AudioData);

        // Write audio bytes to a mp3
        string directoryPath = Path.Combine(Application.dataPath, "TempAudio");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string outputFilePath = Path.Combine(directoryPath, outputFileName);

        File.WriteAllBytes(outputFilePath, audioBytes);

        // Set the audio clip to the output file, through a file streaming
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + outputFilePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.Play();
            }
        }
        yield return null;
    }
}