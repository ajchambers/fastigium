using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour, IManager {
    public static DialogueManager dmInstance;
    public static GeneralManager gmInstance;
    public static SceneController scInstance;
    public static SaveManager smInstance;
    public static TimeManager tmInstance;
    public static UIManager umInstance;

    public Animator animator; 

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image iconImage;

    private Queue<string> sentences;

    private void Start() {
        sentences = new Queue<string>();
    }

    public void LookForManagers() {
        gmInstance = FindObjectOfType<GeneralManager>();
        scInstance = FindObjectOfType<SceneController>();
        smInstance = FindObjectOfType<SaveManager>();
        tmInstance = FindObjectOfType<TimeManager>();
        umInstance = FindObjectOfType<UIManager>();
    }

    public void StartDialogue(Dialogue dialogue) {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        if (dialogue.icon != null) {
            iconImage.sprite = dialogue.icon;
            iconImage.enabled = true;
        } else {
            iconImage.enabled = false;
        }

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void EndDialogue() {
        animator.SetBool("IsOpen", false);
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }
}