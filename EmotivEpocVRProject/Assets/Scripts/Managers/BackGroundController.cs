using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Video;

public class BackGroundController : MonoBehaviour 
{
    public List<VideoPlayer> videoPlayers = new List<VideoPlayer>();
    public List<Material> videoSkyboxes = new List<Material>();
    public List<Material> sceneSkyboxes = new List<Material>();
    public int currentIndex;
    public bool isLoading;

    VideoPlayer currentVideoPlayer;

    [SerializeField] float tiempoPasado;
    [SerializeField] float duration;
    public Color32 color;
    public Color32 color2;
    private bool fade;

    // Start Logos videos
    void Awake()
    {
        RenderSettings.skybox = sceneSkyboxes[0];
        //currentVideoPlayer = videoPlayers[0];
        //StartCoroutine(GameManager.Instance.StartLogos());
    }
    public void CallChangeVideo(int index,float tartTime)
    {
        isLoading = true;
        StartCoroutine(ChangeNextVideo(index, tartTime)); 
    }

    public void CallChangeImagen(int index) 
    {
        isLoading = true;
        StartCoroutine(ChangeNextImage(index));
    }

    public void CallIntroduction()
    {
        //StartCoroutine(GameManager.Instance.StartIntroductionFromMultimedia());
    }

    //  Change Video Skybox
    IEnumerator ChangeNextVideo(int index, float startTime)
    {
     //   currentVideoPlayer.gameObject.SetActive(false);
        Material newSkybox = videoSkyboxes[index];
 
        yield return new WaitForSeconds(0.1f);
        RenderSettings.skybox = newSkybox;
       //videoPlayers[index].gameObject.SetActive(true);
      //  currentVideoPlayer = videoPlayers[index];

        yield return new WaitForSeconds(startTime);
        //videoPlayers[index].Play();
        UIManager.Instance.CallVideoPlayer(UIManager.Instance.videoPlayer2DPanel.transform.GetChild(0).transform.GetChild(7).gameObject);
        UIManager.Instance.videoPlayer2DPanel.SetActive(true);
        
        currentIndex = index;
        Debug.Log("Start "+ videoPlayers[index].name);
    }

    // Change Image Skybox
    IEnumerator ChangeNextImage(int index)
    {
        Material currentSkybox = RenderSettings.skybox;
        StartCoroutine(FadeSkyboxColor(currentSkybox, color2, 1f));

        Material newSkybox = sceneSkyboxes[index];
        newSkybox.SetColor("_Tint", color2);
        
        yield return new WaitUntil(()=> !fade);

        RenderSettings.skybox = newSkybox;
        StartCoroutine(FadeSkyboxColor(newSkybox,color, 1f));

        yield return new WaitUntil(()=> !fade);
        isLoading = false;
        currentSkybox.SetColor("_Tint", color);
    }

    IEnumerator FadeSkyboxColor(Material skyboxMaterial, Color32 targetColor, float duration)
    {
        fade = true;
        float elapsedTime = 0f;
        Color32 startColor = skyboxMaterial.GetColor("_Tint");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            skyboxMaterial.SetColor("_Tint", Color32.Lerp(startColor, targetColor, t));
            yield return null;
        }

        fade = false;

    }

    public void RestartNewVideoPlayer(VideoPlayer videoPlayer)
    {
        videoPlayer.Stop();
        videoPlayer.time = 0.0;

        RenderTexture renderTexture;
        renderTexture = videoPlayer.targetTexture;
        renderTexture.Release();
        renderTexture.Create();
        videoPlayer.targetTexture = renderTexture;
    }
}
