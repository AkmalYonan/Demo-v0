using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest Data")]
public class QuestData : ScriptableObject
{
    [Header("Quest Info")]
    public string questID; // ID unik, misal: "Q001_FindSword"
    public string questTitle;

    [TextArea(3, 5)]
    public string description;

    // Nanti kita bisa tambahkan Gold, Exp, atau Item Reward di sini
}