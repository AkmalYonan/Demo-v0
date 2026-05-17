using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Kinnly
{
    public class ToolbarItem : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TMP_Text amountText;

        int amount;

        // --- TAMBAHAN BARU: Agar data bisa dibaca & dimodifikasi dari script NPC ---
        public Item assignedItem { get; private set; }
        public int Amount => amount;

        public void SetItem(Item item, int amount)
        {
            assignedItem = item; // Simpan referensi item saat di-set
            image.sprite = item.image;
            this.amount = amount;
            UpdateUI();
        }

        // --- TAMBAHAN BARU: Fungsi untuk mengurangi jumlah di Toolbar secara langsung ---
        public void ReduceAmount(int removeAmount)
        {
            this.amount -= removeAmount;
            UpdateUI();
        }
        // --------------------------------------------------------------------------

        void UpdateUI()
        {
            if (amount <= 1)
            {
                amountText.gameObject.SetActive(false);
            }
            else
            {
                amountText.gameObject.SetActive(true);
                amountText.text = amount.ToString();
            }

            if (amount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}