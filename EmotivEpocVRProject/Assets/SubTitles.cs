using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTitles : MonoBehaviour
{
    public DialogueController subController;
    public List<Quest> cinematicaIntroduccionPoly = new List<Quest>();
    public List<Quest> cinematicaPresentacionAlexander = new List<Quest>();
    public List<Quest> cinematicaPasoAlLaboratorioPoly = new List<Quest>();
    public List<Quest> cinematicaConclusionAlexander = new List<Quest>();
    public List<Quest> cinematicaPolyQuiz = new List<Quest>();
    public List<Quest> cinematicaCierre = new List<Quest>();
    public int currentIndex;
    public int cuestionarioIndex;

    public void StartSub()
    {
        switch (cuestionarioIndex)
        {
            case 0:
                subController.StartNewDialogue(cinematicaIntroduccionPoly[currentIndex]);
                break;

            case 1:
                subController.StartNewDialogue(cinematicaPresentacionAlexander[currentIndex]);
                break;

            case 2:
                subController.StartNewDialogue(cinematicaPasoAlLaboratorioPoly[currentIndex]);
                break;

            case 3:
                subController.StartNewDialogue(cinematicaConclusionAlexander[currentIndex]);
                break;

            case 4:
                subController.StartNewDialogue(cinematicaPolyQuiz[currentIndex]);
                break;

            case 5:
                subController.StartNewDialogue(cinematicaCierre[currentIndex]);
                break;


        }
    }

    public void EndSub()
    {
        currentIndex++;
        subController.EndDialogue();
    }

    public void SetCinematicIndex(int cinematic)
    {
        cuestionarioIndex = cinematic;
    }

    public void Finish()
    {
        subController.EndDialogue();
        currentIndex = 0;
        cuestionarioIndex = 0;
    }
}
