using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueController : MonoBehaviour
{
    Queue<string> sentences;
    Quest newQuestion;
    public bool isPlaying;

    [SerializeField] TMP_Text textBox;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartNewDialogue(Quest question)
    {
        newQuestion = question;
        StartDialogue();
        isPlaying = true;
    }

    void StartDialogue()
    {
        sentences.Clear();

        foreach (string sentence in newQuestion.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextDialogue();
    }

    void DisplayNextDialogue()
    {
        string sentence = sentences.Dequeue();

        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        textBox.text = "";

        foreach(char letter in sentence.ToCharArray()) 
        { 
            textBox.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        isPlaying = false;
    }

    public void EndDialogue()
    {
        sentences.Clear();
        newQuestion = null;
    }
}