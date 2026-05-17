using UnityEngine;

namespace Kinnly
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Kinnly Item Settings")]
        public Item Item; // Data item (ScriptableObject Kinnly)
        public int Amount = 1;

        [Header("UI Interaction")]
        public GameObject interactHint; // Tarik UI "Press B" ke sini

        private bool playerInRange;
        private PlayerInventory cachedInventory;

        void Update()
        {
            if (interactHint != null)
            {
                interactHint.SetActive(playerInRange);
            }

            // Jika player di dekatnya dan menekan tombol B
            if (playerInRange && Input.GetKeyDown(KeyCode.B) && cachedInventory != null)
            {
                Pickup();
            }
        }

        void Pickup()
        {
            cachedInventory.AddItem(Item, Amount);
            Debug.Log($"Berhasil mengambil {Amount} {Item.name}!");

            // FIX MASALAH 1: Matikan paksa UI sebelum objek ranting ini hancur
            if (interactHint != null)
            {
                interactHint.SetActive(false);
            }

            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerInRange = true;
                cachedInventory = collision.GetComponent<PlayerInventory>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerInRange = false;
                cachedInventory = null;
            }
        }
    }
}