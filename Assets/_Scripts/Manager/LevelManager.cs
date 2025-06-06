using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private Canvas loadingMenu;
    [SerializeField] private Image loadingBar;
    [SerializeField] private Canvas battleTab; 
    private float target;    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    public async void loadSector(bool unload)
    {
        await Task.Delay(100);
        Debug.LogWarning("Loaded Sector");
        await Task.Delay(1000);
        if(!unload)
            GameManager.instance.deactivateScene();
        else
        {
            GameManager.instance.activateScene();
            GameManager.instance.captureSector();
        }

        this.loadingMenu.gameObject.SetActive(true);
        this.loadingBar.fillAmount = 0;
        target = 0;
        AsyncOperation scene;
        if(!unload)
            scene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Additive);
        else
            scene = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("SectorScene"));

        scene.allowSceneActivation = false;
        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);
        target = 1;
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        if(!unload)
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        Time.timeScale = 1f;
        this.battleTab.gameObject.SetActive(false);
        this.loadingMenu.gameObject.SetActive(false);
    }
}
