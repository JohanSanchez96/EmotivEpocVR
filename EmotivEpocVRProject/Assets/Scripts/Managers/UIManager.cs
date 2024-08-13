using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public AudioMixer audioMixer;
    public GameObject mainMenuPanel;
    public GameObject interactionPanel;
    public GameObject videoPlayer2DPanel;
    public GameObject optionsMenuPanel;
    public GameObject resultGraphicPanel;

    GameObject currentUI;
    GameObject newUI;

    public void SetNewUIMenu(GameObject newUIObject)
    {
        newUI = newUIObject;
        StartCoroutine(ChangeUIMenuByImage());
    }

    public void SetCurrentUIMenu(GameObject currentUIObject)
    {
        currentUI = currentUIObject;
    }

    public void CallVideoPlayer(GameObject videoPLayer)
    {
        StartCoroutine(StartVideoPlayer(videoPLayer));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator StartVideoPlayer(GameObject videoPlayer)
    {
        yield return new WaitUntil(() => videoPlayer.activeInHierarchy);
        videoPlayer.GetComponent<VideoTimeScrubControl>().First();
    }

    IEnumerator ChangeUIMenuByImage()
    {
        currentUI.SetActive(false);
        yield return new WaitForSeconds(2f);
        newUI.SetActive(true);
    }
}
