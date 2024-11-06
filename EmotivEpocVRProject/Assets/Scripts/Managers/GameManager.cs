using System.Collections;
using Unity.VRTemplate;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public VideoTimeScrubControl videoPlayerController;
    public TimeLineController timeLineController;
    public QuestGameManager questGameManager;
    public GameObject timeLineHolder;
    public SelectPosition poly;
    public SelectPosition alexander;
    public bool startTraining;
    public bool startQuiz;
    public bool endQuiz;
    public bool restartQuiz;

    public int polyPositionIndex;
    public int alexanderPositionIndex;


    IEnumerator ao;

    public void OnEnable()
    {
        ao = null;
        ao = StartTraining();
        StartCoroutine(ao);
    }

    public void SetCharacter(SelectPosition character)
    {
        if (character.character == Character.Alexander)
        {
            alexander = character;
        }
        else if (character.character == Character.Poly)
        {
            poly = character;
        }
    }

    public void FindTimeLineHolder()
    {
        timeLineHolder = GameObject.FindWithTag("TimeLineHolder");
        timeLineController.SetCinematics(timeLineHolder);
        Debug.Log("Found it");
    }
    public IEnumerator Test()
    {
        FindTimeLineHolder();
        yield return new WaitUntil(() => timeLineHolder != null);
    }

    public void RestartTraining(bool validation)
    {
        restartQuiz = validation;
    }

    public IEnumerator StartTraining()
    {
        alexanderPositionIndex = 0;
        polyPositionIndex = 0; 
        FindTimeLineHolder();
        yield return new WaitUntil(() => timeLineHolder != null);

        yield return new WaitForSeconds(0.2f);
        startTraining = true;

        yield return new WaitUntil(() => startTraining);
        Debug.Log("Start Cinematic Introduction");
        timeLineController.Play(0);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(0));
        Debug.Log("End Cinematic Introduction");

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Start Cinematic History");
        timeLineController.Play(1);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(1));
        Debug.Log("End Cinematic History");

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Start Video #01");
        videoPlayerController.SetVideo(0);
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.CallVideoPlayer();
        yield return new WaitUntil(() => !UIManager.Instance.videoPlayer2DPanel.activeInHierarchy);
        Debug.Log("End Video #01");

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Start Cinematic Bridge #01");
        timeLineController.Play(2);
        startTraining = false;
        yield return new WaitUntil(() => !timeLineController.StatePlayable(2));
        Debug.Log("End Cinematic Bridge #01");

        yield return new WaitForSeconds(1);
        Debug.Log("Start Load new Scene");
        ScenesManager.Instance.LoadLevel("Laboratorio");
        yield return new WaitForSeconds(2f);

        FindTimeLineHolder();
        yield return new WaitUntil(() => timeLineHolder != null);
        Debug.Log("End Load new Scene ");

        yield return new WaitForSeconds(0.2f);
        Debug.Log("Start Cinematic Bridge #02");
        timeLineController.Play(0);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(0));
        Debug.Log("End Cinematic Bridge #02");

        yield return new WaitForSeconds(0.2f);
        Debug.Log("Start Video #02");
        videoPlayerController.SetVideo(1);
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.CallVideoPlayer();
        yield return new WaitUntil(() => !UIManager.Instance.videoPlayer2DPanel.activeInHierarchy);
        Debug.Log("End Video #02");

        yield return new WaitForSeconds(0.2f);
        Debug.Log("Start Cinematic Bridge #03");
        timeLineController.Play(1);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(1));
        Debug.Log("End Cinematic Bridge #03");

        yield return new WaitForSeconds(0.2f);
        Debug.Log("Start Video #03");
        videoPlayerController.SetVideo(2);
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.CallVideoPlayer();
        yield return new WaitUntil(() => !UIManager.Instance.videoPlayer2DPanel.activeInHierarchy);
        Debug.Log("End Video #03");

        yield return new WaitForSeconds(0.2f);
        Debug.Log("Start Cinematic Bridge #04");
        timeLineController.Play(2);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(2));
        Debug.Log("End Cinematic Bridge #04");

        alexanderPositionIndex = 1;
        polyPositionIndex = 1;

        yield return new WaitForSeconds(1);
        Debug.Log("Start Load new Scene");
        ScenesManager.Instance.LoadLevel("Auditorio");
        yield return new WaitForSeconds(1f);

        FindTimeLineHolder();
        yield return new WaitUntil(() => timeLineHolder != null);
        Debug.Log("End Load new Scene ");

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Start Cinematic Conclution");
        timeLineController.Play(3);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(3));
        Debug.Log("End Cinematic Conclution");
        
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Start Cinematic Final Test");
        timeLineController.Play(4);
        yield return new WaitUntil(() => !timeLineController.StatePlayable(4));
        Debug.Log("End Cinematic Final Test");

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Start Quiz");
        startQuiz = true;

        while (startQuiz)
        {
            UIManager.Instance.quizMenu.SetActive(true);
            yield return new WaitForEndOfFrame();
            questGameManager.StartGame();
            yield return new WaitUntil(() => !questGameManager.StartGameState);
            Debug.Log("End Quiz");

            yield return new WaitUntil(() => !UIManager.Instance.quizMenu.activeInHierarchy);
            if (endQuiz)
            {
                yield return new WaitForSeconds(0.5f);
                Debug.Log("Start Cinematic End Training");
                timeLineController.Play(5);
                yield return new WaitUntil(() => !timeLineController.StatePlayable(5));
                
                Debug.Log("End Cinematic End Training");
                startQuiz = false;
                endQuiz = false;
                restartQuiz = false;
                yield return new WaitForSeconds(0.5f);
                
                ScenesManager.Instance.ResetLevel("Auditorio");
                UIManager.Instance.mainMenuPanel.SetActive(true);
            }
            else
            {
                if (restartQuiz)
                {
                    yield return new WaitForSeconds(0.5f);
                    Debug.Log("Retry");
                    questGameManager.End();
                    startQuiz = true;
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                    Debug.Log("Restart");
                    startQuiz = false;
                    ScenesManager.Instance.ResetLevel("Auditorio");
                    yield return new WaitForSeconds(0.2f);
                    ScenesManager.Instance.LoadLevel("Auditorio");
                }
            }
        }
        
        this.gameObject.SetActive(false);
    }
}
