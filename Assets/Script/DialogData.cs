using UnityEngine;

// Class baru untuk menyimpan data percabangan
[System.Serializable]
public class ChoiceNode
{
    public int triggerLineIndex;

    [Header("Choice 1")]
    public string choice1Text = "Pilihan 1";
    public int choice1JumpIndex;
    [Tooltip("Kosongkan jika tidak memberikan quest")]
    public QuestData choice1TriggerQuest; // <--- TAMBAHAN BARU

    [Header("Choice 2")]
    public string choice2Text = "Pilihan 2";
    public int choice2JumpIndex;
    [Tooltip("Kosongkan jika tidak memberikan quest")]
    public QuestData choice2TriggerQuest; // <--- TAMBAHAN BARU
}

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog System/Dialog Data")]
public class DialogData : ScriptableObject
{
    [Header("NPC Info")]
    public string npcName;
    public Sprite npcPortrait;

    [Header("Dialog Content")]
    [TextArea(3, 5)]
    public string[] lines;

    [Header("Choice Settings")]
    // Menggunakan array (List) agar 1 percakapan bisa punya banyak percabangan
    public ChoiceNode[] choices;
}