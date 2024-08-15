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
    public GameObject videoPlayer;
    public GameObject quizMenu;
    public GameObject timer2DPanel;

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

    public void CallVideoPlayer()
    {
        StartCoroutine(StartVideoPlayer());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator StartVideoPlayer()
    {
        videoPlayer.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => videoPlayer.activeInHierarchy);
        GameManager.Instance.videoPlayerController.First();
    }

    IEnumerator ChangeUIMenuByImage()
    {
        currentUI.SetActive(false);
        yield return new WaitForSeconds(2f);
        newUI.SetActive(true);
    }
}
