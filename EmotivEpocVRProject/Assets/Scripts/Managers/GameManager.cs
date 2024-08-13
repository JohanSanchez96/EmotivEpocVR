using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public VideoTimeScrubControl videoPlayerController;
    public TimeLineController timeLineController;
    public GameObject timeLineHolder;
        
    public bool startTraining;
    public bool startQuiz;
    private bool restartTraining;
    public bool endQuiz;
    public bool restartQuiz;
    public void Start()
    {
        StartCoroutine(StartTraining());
    }

    public void FindTimeLineHolder()
    {
        timeLineHolder = GameObject.FindWithTag("TimeLineHolder");
        timeLineController.SetCinematics(timeLineHolder);
    } 

    public IEnumerator StartTraining()
    {
        FindTimeLineHolder();
        yield return new WaitUntil(() => timeLineHolder != null);
        Debug.Log("Found it");

        yield return new WaitForSeconds(2f);
        startTraining = true;

        yield return new WaitUntil(() => startTraining);
        Debug.Log("Start Cinematic Introduction");
        timeLineController.Play(0);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(0));
        Debug.Log("End Cinematic Introduction");

        Debug.Log("Start Cinematic History");
        timeLineController.Play(1);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(1));
        Debug.Log("End Cinematic History");

        Debug.Log("Start Video #01");
        Debug.Log("End Video #01");


        Debug.Log("Start Cinematic Bridge #01");
        timeLineController.Play(2);
        startTraining = false;
        yield return new WaitUntil(() => !timeLineController.StatePlayable(2));
        Debug.Log("End Cinematic Bridge #01");

        //Load Scene
        Debug.Log("Start Load new Scene");
        ScenesManager.Instance.LoadLevel("Lab");

        yield return new WaitForSeconds(5f);
        Debug.Log("End Load new Scene ");

        FindTimeLineHolder();
        yield return new WaitUntil(() => timeLineHolder != null);
        Debug.Log("Found it");

        yield return new WaitForSeconds(2f);
        startTraining = true;

        yield return new WaitUntil(() => startTraining);
        Debug.Log("Start Cinematic Bridge #02");
        timeLineController.Play(0);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(0));
        Debug.Log("End Cinematic Bridge #02");

        Debug.Log("Start Video #02");
        Debug.Log("End Video #02");

        Debug.Log("Start Cinematic Bridge #03");
        timeLineController.Play(1);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(1));
        Debug.Log("End Cinematic Bridge #03");

        Debug.Log("Start Video #03");
        Debug.Log("End Video #03");

        Debug.Log("Start Cinematic Bridge #04");
        timeLineController.Play(2);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(2));
        Debug.Log("End Cinematic Bridge #04");

        //Load Scene
        Debug.Log("Start Load new Scene");
        ScenesManager.Instance.LoadLevel("Auditorium");

        yield return new WaitForSeconds(5f);
        Debug.Log("End Load new Scene ");

        FindTimeLineHolder();
        yield return new WaitUntil(() => timeLineHolder != null);
        Debug.Log("Found it");

        Debug.Log("Start Cinematic Conclution");
        timeLineController.Play(3);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(3));
        Debug.Log("End Cinematic Conclution");

        Debug.Log("Start Cinematic Conclution");
        timeLineController.Play(3);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(3));
        Debug.Log("End Cinematic Conclution");

        Debug.Log("Start Cinematic Final Test");
        timeLineController.Play(4);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(4));
        Debug.Log("End Cinematic Final Test");

        Debug.Log("Start Quiz");
        startQuiz = true;
        Debug.Log("End Quiz");

        endQuiz = true;
        while (startQuiz)
        {
            //Iniciar Quiz
            //Presentando Quiz
            //Terminar Quiz

            if (endQuiz)
            {
                Debug.Log("Start Cinematic End Training");
                timeLineController.Play(4);
                yield return new WaitUntil(() => !timeLineController.StatePlayable(4));
                Debug.Log("End Cinematic End Training");

                //Reiniciar sistema al menu principal

                startQuiz = false;
                restartTraining = false;
                endQuiz = false;
            }
            else
            {
                //Iniciar pantalla de si quiere repetir quiz o todo el tutorial
                // pantalla de confirmacion
                if (restartQuiz)
                {
                    startQuiz = true;
                    restartTraining = false;

                }
                else
                {
                    startQuiz = false;
                    restartTraining = true;
                }
            }
        }

        if (restartTraining)
        {
            //recargar todo desde el incio
        }
        else
        {
            //Llevar al menu principal
        }

    }
}
