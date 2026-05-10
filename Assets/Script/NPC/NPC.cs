using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    [TextArea(3, 10)]
    public string[] dialogLines;

    public GameObject interactHint; // Tarik objek InteractHint ke sini
    private bool playerInRange;

    [Header("Choice Dialog")]
    public bool hasChoice;
    public int choiceLineIndex = 0;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !DialogManager.instance.DialogActive())
        {
            // Saat ngobrol dimulai, sembunyikan petunjuk F
            interactHint.SetActive(false);
            DialogManager.instance.StartDialog(this, npcName, dialogLines);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            // Pastikan variabel ini tidak null sebelum diaktifkan
            if (interactHint != null)
            {
                interactHint.SetActive(true);
                Debug.Log("UI Press F harusnya muncul sekarang!");
                Debug.Log(interactHint.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            // Sembunyikan tulisan Press F
            if (interactHint != null) interactHint.SetActive(false);
            DialogManager.instance.EndDialog();
        }
    }
}