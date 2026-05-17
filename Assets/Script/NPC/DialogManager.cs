using UnityEngine;
using UnityEngine.UI; // Ditambahkan untuk akses Image Portrait
using TMPro;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("UI")]
    public GameObject dialogBox;
    public Image portraitImage; // Tambahan untuk Portrait System
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    [Header("UI Systems")]
    public GameObject inventorySystemUI;

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;


    [Header("Choice UI")]
    public GameObject choicePanel;
    public TextMeshProUGUI choice1TextUI; // Masukkan Text anak Tombol 1 ke sini di Editor
    public TextMeshProUGUI choice2TextUI; // Masukkan Text anak Tombol 2 ke sini di Editor

    private ChoiceNode currentChoiceNode; // Menyimpan data choice yang sedang aktif

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip typingSound;

    // State
    private bool isChoosing;
    private bool isTyping;
    private bool isDialogActive;

    // Data (Refactored to ScriptableObject)
    private DialogData currentDialog;
    private int index = 0;

    void Awake() { if (instance == null) instance = this; }

    void Update()
    {
        if (!isDialogActive || isChoosing) return;

        // Next dialog
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Jika text masih mengetik (Skip System)
            if (isTyping)
            {
                StopAllCoroutines();
                dialogText.text = currentDialog.lines[index]; // Langsung tampil semua
                FinishTyping(); // Centralized Logic
            }
            else
            {
                index++;
                DisplayNextLine();
            }
        }
    }

    public void StartDialog(DialogData data)
    {
        isDialogActive = true;

        // Simpan data ScriptableObject
        currentDialog = data;
        index = 0;

        dialogBox.SetActive(true);

        // Set UI dari ScriptableObject
        nameText.text = currentDialog.npcName;

        // Portrait Logic
        if (portraitImage != null && currentDialog.npcPortrait != null)
        {
            portraitImage.sprite = currentDialog.npcPortrait;
            portraitImage.gameObject.SetActive(true);
        }
        else if (portraitImage != null)
        {
            portraitImage.gameObject.SetActive(false); // Sembunyikan jika NPC tak punya portrait
        }

        // Lock player movement
        if (PlayerMovement.instance != null)
        {
            PlayerMovement.instance.canMove = false;
        }

        if (inventorySystemUI != null)
            inventorySystemUI.SetActive(false);

        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        // Dialog selesai
        if (index >= currentDialog.lines.Length)
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
        // Ubah teks tombol secara dinamis dari ScriptableObject
        choice1TextUI.text = currentChoiceNode.choice1Text;
        choice2TextUI.text = currentChoiceNode.choice2Text;

        choicePanel.SetActive(true);
    }

    // Ubah nama method dan logikanya
    public void Choose1()
    {
        // Cek apakah pilihan ini memberikan quest
        if (currentChoiceNode.choice1TriggerQuest != null)
        {
            QuestManager.instance.AcceptQuest(currentChoiceNode.choice1TriggerQuest);
        }
        ExecuteChoice(currentChoiceNode.choice1JumpIndex);
    }

    public void Choose2()
    {
        // Cek apakah pilihan ini memberikan quest
        if (currentChoiceNode.choice2TriggerQuest != null)
        {
            QuestManager.instance.AcceptQuest(currentChoiceNode.choice2TriggerQuest);
        }
        ExecuteChoice(currentChoiceNode.choice2JumpIndex);
    }

    private void ExecuteChoice(int jumpIndex)
    {
        isChoosing = false;
        choicePanel.SetActive(false);

        if (jumpIndex == -1)
        {
            EndDialog(); // Jika -1, langsung tutup dialog (seperti Exit)
        }
        else
        {
            index = jumpIndex; // Set index dialog ke index tujuan
            DisplayNextLine(); // Ketikkan teks tujuan tersebut
        }
    }

    void FinishTyping()
    {
        isTyping = false;

        // Cek apakah di Line (index) saat ini ada percabangan (Choice)
        foreach (ChoiceNode node in currentDialog.choices)
        {
            if (index == node.triggerLineIndex)
            {
                currentChoiceNode = node;
                ShowChoices();
                break; // Stop looping jika sudah ketemu
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in currentDialog.lines[index])
        {
            dialogText.text += c;

            // Play typing sound (abaikan spasi agar suaranya lebih natural)
            if (c != ' ' && audioSource != null && typingSound != null)
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
        choicePanel.SetActive(false); // Safety: Pastikan choice tertutup

        if (inventorySystemUI != null)
            inventorySystemUI.SetActive(true);
        // Unlock movement
        if (PlayerMovement.instance != null)
        {
            PlayerMovement.instance.canMove = true;
        }

    }

    public bool DialogActive()
    {
        return isDialogActive;
    }
}