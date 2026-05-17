using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Dialog Data")]
    public DialogData defaultDialog; // Awal mula kasih quest
    public DialogData questProgressDialog; // Ranting belum cukup 10
    public DialogData questCompleteDialog; // Ranting pas 10 dan diserahkan
    public DialogData afterQuestDialog; // Ngobrol santai setelah quest beres

    [Header("Quest Settings")]
    public QuestData relatedQuest;
    public string requiredItemName = "Ranting"; // Sesuaikan dengan nama Item Kinnly
    public int requiredItemAmount = 10;

    [Header("UI Interaction")]
    public GameObject interactHint; // UI "Press F"

    private bool playerInRange;

    void Update()
    {
        // Hilangkan "Press F" jika dialog sedang aktif
        if (DialogManager.instance != null && DialogManager.instance.DialogActive())
        {
            interactHint.SetActive(false);
            return;
        }

        interactHint.SetActive(playerInRange);

        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    void Interact()
    {
        DialogData dialogToPlay = defaultDialog;

        // 1. Cek apakah quest sudah pernah tamat?
        if (QuestManager.instance.completedQuests.Contains(relatedQuest))
        {
            dialogToPlay = afterQuestDialog != null ? afterQuestDialog : questCompleteDialog;
        }
        // 2. Cek apakah quest sedang berjalan?
        else if (QuestManager.instance.IsQuestActive(relatedQuest))
        {
            // Tambahkan '(true)' agar Unity mencari item di dalam UI Inventory yang sedang di-disable/tutup
            Kinnly.InventoryItem[] allInventoryItems = FindObjectsOfType<Kinnly.InventoryItem>(true);
            int currentRantingCount = 0;

            // Hitung total ranting yang dimiliki player
            foreach (Kinnly.InventoryItem invItem in allInventoryItems)
            {
                if (invItem.Item != null && invItem.Item.name == requiredItemName)
                {
                    currentRantingCount += invItem.Amount;
                }
            }

            // 3. Evaluasi jumlah ranting
            if (currentRantingCount >= requiredItemAmount)
            {
                // A. Potong item dari MAIN INVENTORY Kinnly
                int amountToRemove = requiredItemAmount;
                foreach (Kinnly.InventoryItem invItem in allInventoryItems)
                {
                    if (invItem.Item != null && invItem.Item.name == requiredItemName)
                    {
                        if (invItem.Amount >= amountToRemove)
                        {
                            invItem.RemoveAmount(amountToRemove);
                            break;
                        }
                        else
                        {
                            amountToRemove -= invItem.Amount;
                            invItem.RemoveAmount(invItem.Amount);
                        }
                    }
                }

                // B. FIX: Potong item dari TOOLBAR Kinnly secara instan
                Kinnly.ToolbarItem[] allToolbarItems = FindObjectsOfType<Kinnly.ToolbarItem>(true);
                int toolbarAmountToRemove = requiredItemAmount;
                foreach (Kinnly.ToolbarItem tItem in allToolbarItems)
                {
                    if (tItem.assignedItem != null && tItem.assignedItem.name == requiredItemName)
                    {
                        if (tItem.Amount >= toolbarAmountToRemove)
                        {
                            tItem.ReduceAmount(toolbarAmountToRemove);
                            break;
                        }
                        else
                        {
                            toolbarAmountToRemove -= tItem.Amount;
                            tItem.ReduceAmount(tItem.Amount);
                        }
                    }
                }

                // C. Selesaikan Quest
                QuestManager.instance.CompleteQuest(relatedQuest);
                dialogToPlay = questCompleteDialog;
            }
            else // <--- INI BAGIAN YANG HILANG SEBELUMNYA!
            {
                // Jika ranting belum mencapai requiredItemAmount (misal: 10)
                dialogToPlay = questProgressDialog;
            }
        }

        // 4. Mulai Obrolan
        DialogManager.instance.StartDialog(dialogToPlay);
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