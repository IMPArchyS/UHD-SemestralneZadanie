using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonExit : MonoBehaviour
{
    public void exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
