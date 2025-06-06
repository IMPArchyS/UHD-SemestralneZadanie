using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private Canvas soundMenu;
    [SerializeField] private bool inOther = false;

    private bool inMenu = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(soundMenu.gameObject);
            Destroy(pauseMenu.gameObject);
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(!SceneManager.GetActiveScene().name.Equals("MainMenu") && !this.inOther)
        {
            if(Input.GetKeyDown(KeyCode.Escape) && this.inMenu == false)
            {
                this.callPauseMenu();
            }
            else if(Input.GetKeyDown(KeyCode.Escape) && this.inMenu == true && !soundMenu.gameObject.activeInHierarchy)
            {
                this.resumeGame();
            }
            else if(Input.GetKeyDown(KeyCode.Escape) && this.inMenu == true && soundMenu.gameObject.activeInHierarchy)
            {
                pauseMenu.gameObject.SetActive(true);
                soundMenu.gameObject.SetActive(false);
            }
        }
    }
    public void setInOtherMenu(bool b)
    {
        this.inOther = b;
    }
    public bool getInOtherMenu()
    {
        return this.inOther;
    }
    public bool getInMenu()
    {
        return this.inMenu;
    }
    public void setTimeFlow(float t)
    {
        Time.timeScale = t;
    }

    public void reloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void callPauseMenu()
    {
        soundMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        this.inMenu = true;
    }

    public void exitToMainMenu()
    {
        pauseMenu.gameObject.SetActive(false);
        soundMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Destroy(soundMenu.gameObject);
        Destroy(pauseMenu.gameObject);
        Destroy(GameManager.instance.gameObject);
        Destroy(gameObject);
    }
    public void resumeGame()
    {
        Time.timeScale = 1f;
        this.inMenu = false;
        pauseMenu.gameObject.SetActive(false);
    }

    public void callSoundMenu()
    {
        soundMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
    }
    public Canvas getPauseMenu()
    {
        return pauseMenu;
    }

    public Canvas getSoundMenu()
    {
        return soundMenu;
    }

    public void setOtherMenusDelayed(bool b)
    {
        StartCoroutine(this.delaySet(b));
    }

    public void setOtherMenusInstant(bool b)
    {
        this.inOther = b;
    }

    private IEnumerator delaySet(bool b)
    {
        yield return new WaitForSeconds(0.1f);
        this.inOther = b;
    }

    public bool isInOtherMenu()
    {
        return this.inOther;
    }
}
