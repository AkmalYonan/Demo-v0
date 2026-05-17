using UnityEngine;
using UnityEngine.EventSystems;

namespace Kinnly
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] PlayerInventory playerInventory;

        void Start()
        {
            // Auto-Assign: Mencari Player secara otomatis jika lupa diisi di Inspector
            if (playerInventory == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    playerInventory = playerObj.GetComponent<PlayerInventory>();
                }
                else
                {
                    Debug.LogWarning("InventorySlot gagal menemukan GameObject dengan tag 'Player'.");
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Safety check agar tidak NullReferenceException
            if (playerInventory != null)
            {
                playerInventory.CurrentlyHoveredInventorySlot = this.gameObject;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (playerInventory != null)
            {
                playerInventory.CurrentlyHoveredInventorySlot = null;
            }
        }
    }
}