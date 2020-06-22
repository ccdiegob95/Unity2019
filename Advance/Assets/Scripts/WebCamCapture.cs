using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.iOS;
using System.IO;

public class WebCamCapture : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    private WebCamTexture webcam;
    private bool play = false;

   IEnumerator Start()
    {
        findWebCams();

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("webcam found");
             webcam = new WebCamTexture();
            //Renderer render = GetComponent<Renderer>();
            //render.material.mainTexture = webcam;
            rawImage.texture= webcam;
            rawImage.material.mainTexture = webcam;
            webcam.Play();
            play = true;
        }
        else
        {
            Debug.Log("webcam not found");
        }

        findMicrophones();

        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Microphone found");
        }
        else
        {
            Debug.Log("Microphone not found");
        }
    }

    void findWebCams()
    {
        foreach (var device in WebCamTexture.devices)
        {
            Debug.Log("Name: " + device.name);
        }
    }

    void findMicrophones()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    private void LateUpdate() {
        if(Input.GetKeyDown(KeyCode.R))
            Capture();
        if(Input.GetKeyDown(KeyCode.P)){
            play = !play;
            if(play)
                webcam.Pause();
            else
                webcam.Play();
        }

        if(Input.GetKeyDown(KeyCode.Q))
            webcam.Stop();
            
    }

    void Capture(){
        Texture2D photo = new Texture2D(webcam.width, webcam.height);
        photo.SetPixels(webcam.GetPixels());
        photo.Apply();
        //RenderTexture.active = webcam;
 
        byte[] bytes = photo.EncodeToPNG();
        //Destroy(image);
 
        File.WriteAllBytes(Application.dataPath + "/Resources/" + "img.png", bytes);
    }

    private void OnApplicationQuit() {
        webcam.Stop();
    }
}
