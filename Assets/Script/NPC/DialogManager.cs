using UnityEngine;
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("UI")]
    public GameObject dialogBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;

    [Header("Choice UI")]
    public GameObject choicePanel;
    private NPC currentNPC;
    private string[] lines;
    private int index;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip typingSound;

    private bool isChoosing;
    private bool isTyping;
    private bool isDialogActive;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!isDialogActive || isChoosing) return;

        // Next dialog
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Jika text masih mengetik
            if (isTyping)
            {
                StopAllCoroutines();

                dialogText.text = lines[index];

                FinishTyping();
            }
            else
            {
                index++;
                DisplayNextLine();
            }
        }
    }

    public void StartDialog(NPC npc, string npcName, string[] npcLines)
    {

        currentNPC = npc;

        isDialogActive = true;

        lines = npcLines;
        index = 0;

        dialogBox.SetActive(true);

        nameText.text = npcName;

        // Lock player movement
        PlayerMovement.instance.canMove = false;

        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        // Dialog selesai
        if (index >= lines.Length)
        {
            EndDialog();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }
    void ShowChoices()
    {
        isChoosing = true;
        choicePanel.SetActive(true);
    }

    public void ChooseYes()
    {
        isChoosing = false;

        choicePanel.SetActive(false);

        dialogText.text = "Terima kasih sudah membantu!";
    }

    public void ChooseNo()
    {
        isChoosing = false;

        choicePanel.SetActive(false);

        dialogText.text = "Baiklah... mungkin lain kali.";
    }
    void FinishTyping()
    {
        isTyping = false;

        // Jika line ini punya choice
        if (currentNPC.hasChoice &&
            index == currentNPC.choiceLineIndex)
        {
            ShowChoices();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        dialogText.text = "";

        foreach (char c in lines[index])
        {
            dialogText.text += c;

            // Play typing sound
            if (c != ' ')
            {
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        FinishTyping();
    }

    public void EndDialog()
    {
        isDialogActive = false;

        dialogBox.SetActive(false);

        // Unlock movement
        PlayerMovement.instance.canMove = true;
    }

    public bool DialogActive()
    {
        return isDialogActive;
    }
}