using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas optionsMenu;
    [SerializeField] private Canvas loadingMenu;
    [SerializeField] private Image loadingBar;
    
    private float target;    


    public void exit()
    {
        StartCoroutine(this.waitForAnimationDisable(null));
        Application.Quit();
        Debug.Log("Quit");
    }

    public async void play()
    {
        await Task.Delay(1000);
        this.mainMenu.gameObject.SetActive(false);
        this.loadingMenu.gameObject.SetActive(true);
        this.loadingBar.fillAmount = 0;
        target = 0;
        var scene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
        scene.allowSceneActivation = false;
        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);
        target = 1;
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        this.loadingMenu.gameObject.SetActive(false); 
    }

    public void disableDelayed(GameObject g)
    {
        StartCoroutine(this.waitForAnimationDisable(g));
    }
    public void enableDelayed(GameObject g)
    {
        StartCoroutine(this.waitForAnimationEnable(g));
    }

    private IEnumerator waitForAnimationEnable(GameObject g)
    {
        yield return new WaitForSeconds(1f);
        if(g != null)
            g.SetActive(true);
    }

    private IEnumerator waitForAnimationDisable(GameObject g)
    {
        yield return new WaitForSeconds(1.01f);
        if(g != null)
            g.SetActive(false);
    }

    private void Update()
    {
        if(loadingBar != null)
            loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, target, Time.deltaTime);
    }
}
