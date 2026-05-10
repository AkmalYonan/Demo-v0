using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Ubah menjadi "Sumatra" sesuai nama scene kamu
        SceneManager.LoadScene("Sumatra");
    }
}