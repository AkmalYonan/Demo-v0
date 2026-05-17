using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [Header("Quest Lists")]
    public List<QuestData> activeQuests = new List<QuestData>();
    public List<QuestData> completedQuests = new List<QuestData>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); // Keamanan tambahan untuk Singleton
    }

    // Fungsi untuk menerima Quest baru
    public void AcceptQuest(QuestData newQuest)
    {
        if (!activeQuests.Contains(newQuest) && !completedQuests.Contains(newQuest))
        {
            activeQuests.Add(newQuest);
            Debug.Log("Quest Accepted: " + newQuest.questTitle);
            // Todo: Nanti bisa panggil UI Pop-up "New Quest!" di sini
        }
    }

    // Fungsi untuk mengecek apakah quest sedang aktif
    public bool IsQuestActive(QuestData quest)
    {
        return activeQuests.Contains(quest);
    }

    // Fungsi untuk menyelesaikan Quest
    public void CompleteQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            Debug.Log("Quest Completed: " + quest.questTitle);
            // Todo: Berikan reward ke player di sini nanti
        }
    }
}