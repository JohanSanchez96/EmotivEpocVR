using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class QuestGameManager : MonoBehaviour
{
    public DialogueController textController;
    public TimerController timer;
    public GameObject bodyPanel;
    public GameObject resultsPanel;
    public GameObject observationsPanel;
    public GameObject restartPanel;
    public int newQualification;

    [SerializeField] TMP_Text textTitle;
    [SerializeField] TMP_Text textIndicator;
    [SerializeField] List<Quest> questions = new List<Quest>();
    [SerializeField] List<Button> buttons = new List<Button>();
    [SerializeField] string[] positionIndicator;

    [SerializeField] List<Quest> provisionalQuestionsList;
    [SerializeField] List<string> answersProvisionalList = new List<string>();
    List<int> takeList;
    
    [SerializeField] int indexQuestion;
    [SerializeField] AudioSource correctSFX;
    [SerializeField] AudioSource wrongSFX;

    int randomIndex;
    bool startGame;
    bool isAnswered;
    ColorBlock colors;
    Quest currentQuest;

    public bool StartGameState { get => startGame;}

    private void Start()
    {
        GameManager.Instance.questGameManager = this;
    }

    public void StartGame()
    {
        textTitle.text = "Test de Emotiv Epoc +";
        int position = indexQuestion + 1;
        textIndicator.text = "Pregunta #" + position +"/3";
        bodyPanel.SetActive(true);
        provisionalQuestionsList = ShuffleQuestions();
        ChangeStateGame();
        StartCoroutine(GetQuestion());

        for (int i = 0; i < questions.Count; i++)
        {
            questions[i].isAnswerCorrect = false;
        }
    }
    
    void ChangeStateGame()
    {
        startGame = !startGame;
    }

    IEnumerator GetQuestion()
    {
        ResetButtons();
        currentQuest = provisionalQuestionsList[indexQuestion];

        foreach (string answer in currentQuest.answers)
        {
            answersProvisionalList.Add(answer);
        }

        textController.StartNewDialogue(currentQuest);

        yield return new WaitUntil(() => !textController.isPlaying);
        timer.StartTimer(31f);
        RandomButton();
        indexQuestion++;
        
        yield return new WaitUntil(() => !timer.isCounting);

        if (!isAnswered)
        {
            ButtonEvent(false);
        }
    }

    List<Quest> ShuffleQuestions()
    {
        List<Quest> shuffledQuestions;
        shuffledQuestions = questions.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
        return shuffledQuestions;
    }

    void RandomButton()
    {
        takeList = new List<int>(new int[buttons.Count]);
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            randomIndex = UnityEngine.Random.Range(1, buttons.Count + 1);
            while (takeList.Contains(randomIndex))
            {
                randomIndex = UnityEngine.Random.Range(1, buttons.Count + 1);
            }

            takeList[i] = randomIndex;
            string answerEnun = answersProvisionalList[takeList[i] - 1];
            int index = answersProvisionalList.IndexOf(answerEnun);


            SetButtonText(buttons[i], positionIndicator[i], answerEnun);
            AddButtonClickEvent(buttons[i], index);
            SetupButtonColors(buttons[i], index, currentQuest.trueAnswer);
        }
    }

    void SetupButtonColors(Button button, int index, int trueAnswerIndex)
    {
        colors = button.colors;
        colors.highlightedColor = new Color32(36, 96, 214, 255);

        if (index.Equals(trueAnswerIndex))
        {
            colors.pressedColor = new Color32(0, 255, 0, 255);
        }
        else
        {
            colors.pressedColor = new Color32(255, 0, 0, 255);
        }

        button.colors = colors;
    }

    void AddButtonClickEvent(Button button, int index)
    {
        button.onClick.AddListener(() => EventClickButton(index));
    }

    void SetButtonText(Button button, string enumText, string answer)
    {
        button.transform.GetChild(1).transform.GetComponentInChildren<TMP_Text>().text = (enumText + " " + answer);
    }

    void EventClickButton(int indexButton)
    {
        if (indexButton.Equals(currentQuest.trueAnswer))
        {
            ButtonEvent(true);        
        }
        else
        {
            ButtonEvent(false);
        }

        timer.isCounting = false;
        isAnswered = true;
    }

    void ButtonEvent(bool value)
    {
        if (value)
        {
            correctSFX.Play();
            currentQuest.isAnswerCorrect = true;
            bodyPanel.SetActive(false);
            observationsPanel.SetActive(true);
            observationsPanel.GetComponentInChildren<TMP_Text>().color = new Color32(0, 255, 0, 255);
            observationsPanel.GetComponentInChildren<TMP_Text>().text = "Respuesta Correcta";
        }
        else
        {
            wrongSFX.Play();
            bodyPanel.SetActive(false);
            observationsPanel.SetActive(true);
            observationsPanel.GetComponentInChildren<TMP_Text>().color = new Color32(255, 0, 0, 255);
            observationsPanel.GetComponentInChildren<TMP_Text>().text = "Respuesta Incorrrecta, la correcta es: " + answersProvisionalList[currentQuest.trueAnswer];
        }
    }

    void ResetButtons()
    {
        textTitle.text = "";
        isAnswered = false;
        answersProvisionalList.Clear();

        observationsPanel.GetComponentInChildren<TMP_Text>().color = Color.black;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].transform.GetChild(1).transform.GetComponentInChildren<TMP_Text>().text = "";
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].gameObject.SetActive(false);
        }   
    }
    
    public void Validate()
    {
        int position = indexQuestion + 1;
        textIndicator.text = "Pregunta numero: " + position + "/3";

        if (indexQuestion == provisionalQuestionsList.Count)
        {
            bodyPanel.SetActive(false);
            QualityModule();
        }
        else
        {
            bodyPanel.SetActive(true);
            StartCoroutine(GetQuestion());
        }
    }

    void QualityModule()
    {
        ChangeStateGame();
        newQualification = QualificationModule();
        bodyPanel.SetActive(false);
        resultsPanel.SetActive(true);

        if (newQualification > (provisionalQuestionsList.Count / 2))
        {
            GameManager.Instance.endQuiz = true;
            resultsPanel.GetComponentInChildren<TMP_Text>().text = "Aprobado con " + " " + newQualification + " " + "de" + " " + provisionalQuestionsList.Count;
        }
        else
        {
            GameManager.Instance.endQuiz = false;
            resultsPanel.GetComponentInChildren<TMP_Text>().text = "Fallado con " + " " + newQualification + " " + "de" + " " + provisionalQuestionsList.Count;
        }
    }

    public void Retry()
    {
        if (GameManager.Instance.endQuiz)
        {
            End();
        }
        else
        {
            resultsPanel.SetActive(false);
            restartPanel.SetActive(true);
        }
    }

    private int QualificationModule()
    {
        int indexCalificationModule = 0;

        for (int i = 0; i < provisionalQuestionsList.Count; i++)
        {
            if (provisionalQuestionsList[i].isAnswerCorrect)
            {
                indexCalificationModule++;
            }
        }

        return indexCalificationModule;
    }

    public void End()
    {
        UIManager.Instance.quizMenu.SetActive(false);
        Debug.Log("End Quiz");
        textController.EndDialogue();
        ResetButtons();
        answersProvisionalList.Clear();
        provisionalQuestionsList.Clear();
        indexQuestion = 0;
        randomIndex = 0;
        currentQuest = null;
        observationsPanel.SetActive(false);
        bodyPanel.SetActive(false);
        resultsPanel.SetActive(false);
        restartPanel.SetActive(false);  
    }
}
