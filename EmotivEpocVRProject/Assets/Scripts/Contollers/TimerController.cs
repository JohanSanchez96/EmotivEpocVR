using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public bool isCounting;
    float smoothTimeUpdate;
    float currentTime;
    float time;

    [SerializeField] Image eventTimerImage;
    [SerializeField] TMP_Text eventTimerText;

    void Start()
    {
        isCounting = false;
        
    }

    public void EndTimer()
    {
        currentTime = 0;
        eventTimerText.text = string.Empty;
        UIManager.Instance.timer2DPanel.SetActive(false);
        isCounting = false;
    }

    public void StartTimer(float newTime)
    {
        time = newTime;
        currentTime = time;
        smoothTimeUpdate = currentTime;
        isCounting = true;
        UIManager.Instance.timer2DPanel.SetActive(true);
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        SetupGUI();

        while (currentTime > 0 && isCounting)
        {
            currentTime--;

            if (eventTimerText != null)
            {
                eventTimerText.text = currentTime.ToString();
            }
            
            yield return new WaitForSeconds(1f);
        }

        EndTimer();
    }

    void Update()
    {
        if (isCounting)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        smoothTimeUpdate -= Time.unscaledDeltaTime;
        eventTimerImage.fillAmount = smoothTimeUpdate / time;
    }

    void SetupGUI()
    {
        eventTimerImage.fillAmount = 1;
    }
}


