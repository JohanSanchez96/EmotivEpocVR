using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VRTemplate;
using System.Collections;

/// <summary>
/// Controls the steps in the in coaching card.
/// </summary>
public class StepManager : MonoBehaviour
{
    [Serializable]
    class Step
    {
        [SerializeField]
        public GameObject stepObject;

        [SerializeField]
        public string buttonText;
    }
    

    [SerializeField]
    TextMeshProUGUI m_StepButtonTextField;

    [SerializeField]
    List<Step> m_StepList = new List<Step>();

    public int m_CurrentStepIndex = 0;
    public void Restart()
    {
        m_CurrentStepIndex = 0;
    }

    public void StartNextStep()
    {
        if (m_CurrentStepIndex == m_StepList.Count - 1)
        {
            //GameManager.Instance.finalTestController.StartFinalTest();
        }
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = (m_CurrentStepIndex + 1) % m_StepList.Count;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
           // m_StepButtonTextField.text = LanguageManager.Instance.GetStringValue(m_StepList[m_CurrentStepIndex].buttonText);
    }
}
