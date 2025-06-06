using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum State {BEGIN, PLAYERMOVE, PLAYERACTION, ACTION, POSTACTION, END}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Type currentSectorType;
    public static int currentEnemyAmount;
    public long turn;
    public TextMeshProUGUI turnText;
    public Button nextTurnText;
    public Button shopText;
    public Button shipText;
    public Canvas battleCanvas;
    public Canvas winCanvas;
    public State gameState;
    public PlayerData pirateData;
    public GameObject eventSystem;
    public Canvas deathCanvas;
    [SerializeField] private List<GameObject> sceneObjects;
    private void Start()
    {
        Time.timeScale = 1f;
    }
    private void Awake()
    {
        currentSectorType = Type.VOID;
        currentEnemyAmount = 0;
        this.turn = 0;
        this.pirateData.initData();
        this.increaseTurnAmount();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        sceneObjects.AddRange(allObjects);
        if(instance == null)
            instance = this;
        this.battleCanvas.gameObject.SetActive(false);
        this.gameState = State.PLAYERMOVE;
    }

    public void deactivateScene()
    {   
        Camera.main.GetComponent<AudioListener>().enabled = false;
        eventSystem.GetComponent<EventSystem>().enabled = false;
        sceneObjects.ForEach(obj => { if (obj.CompareTag("MainCamera")) return;; obj.SetActive(false); });
        GameObject canvas = sceneObjects.FirstOrDefault(obj => obj.CompareTag("CanvasManager"));
        GameObject soundMNG = sceneObjects.FirstOrDefault(obj => obj.CompareTag("SoundManager"));
        canvas.SetActive(true);
        soundMNG.SetActive(true);
        soundMNG.transform.GetChild(0).gameObject.SetActive(true);
        soundMNG.transform.GetChild(1).gameObject.SetActive(true);
        Debug.Log("Deactivating scene");
    }

    public async void activateScene()
    {
        sceneObjects.ForEach(obj => obj.SetActive(true));
        Camera.main.GetComponent<AudioListener>().enabled = true;
        await Task.Delay(50);
        eventSystem.GetComponent<EventSystem>().enabled = true;
        FindObjectOfType<HoverManager>().hideTip();
        Debug.Log("Activating scene");
    }

    private void increaseTurnAmount()
    {
        this.turn++;
        this.turnText.text = "TURN " + this.turn;
    }

    public void nextTurn()
    {
        if(this.gameState.Equals(State.POSTACTION) || this.gameState.Equals(State.PLAYERMOVE))
        {   
            shopText.interactable = true;
            shipText.interactable = true;
            SoundManager.instance.playSfx("nextTurn");
            PlayerSector p = FindObjectOfType<PlayerSector>();
            p.revealNeighbours(p.getCurrentSector());
            SectorMap.instance.incomeEvent.Invoke();
            this.gameState = State.PLAYERMOVE;
            if(p.getPlayerData().dataCurrentHp < p.getPlayerData().dataHp)
            {
                p.getPlayerData().dataCurrentHp += 15;
                if (p.getPlayerData().dataCurrentHp > p.getPlayerData().dataHp)
                    p.getPlayerData().dataCurrentHp = p.getPlayerData().dataCurrentHp % p.getPlayerData().dataHp;
            }
            this.increaseTurnAmount();
            Debug.Log("Enemy power grows");
            this.pirateData.dataDamage += 1;
            this.pirateData.dataCurrentHp += 5;
        }
    }

    public void playerAction()
    {
        this.gameState = State.PLAYERACTION;
        this.battleCanvas.gameObject.SetActive(true);
        nextTurnText.interactable = false;
        shopText.interactable = false;
        shipText.interactable = false;
    }

    public void clearSector()
    {
        PlayerSector p = FindObjectOfType<PlayerSector>();
        currentSectorType = p.getCurrentSector().getData().getType();
        currentEnemyAmount = p.getCurrentSector().getEnemyAmount();
        Debug.LogWarning(currentEnemyAmount);
        this.gameState = State.ACTION;
    }

    public void autoClear()
    {
        PlayerSector p = FindObjectOfType<PlayerSector>();
        p.getPlayerData().dataCurrentHp -= 35;
        this.captureSector();
    }

    public void captureSector()
    {
        Time.timeScale = 1f;
        this.gameState = State.POSTACTION;
        nextTurnText.interactable = true;
        shopText.interactable = false;
        shipText.interactable = false;
        PlayerSector p = FindObjectOfType<PlayerSector>();
        if(p.getPlayerData().dataCurrentHp <= 0)
            Debug.Log("PlayerDead");
            
        if(!p.getCurrentSector().getData().getType().Equals(Type.VOID))
            p.getCurrentSector().getData().setOwner(Owner.ORDER);   
        p.getCurrentSector().setColor();
        p.getCurrentSector().getData().setDiff(ThreatLevel.NONE);
        p.getCurrentSector().setIncomeText();
        SectorMap sm = FindObjectOfType<SectorMap>();
        bool noPiratesSectorsLeft = !sm.getSectorList().Any(sector => sector.getData().getOwner().Equals(Owner.PIRATES));
        bool noEnemiesLeft = sm.getSectorList().All(sector => sector.getData().getDiff().Equals(ThreatLevel.NONE));
        if(p.getPlayerData().dataCurrentHp <= 0)
        {
            Time.timeScale = 0f;
            deathCanvas.gameObject.SetActive(true);
        }
        if(noEnemiesLeft && noEnemiesLeft)
            this.endGame();
    }
    public void endGame()
    {
        this.gameState = State.END;        
        Time.timeScale = 0f;
        this.winCanvas.gameObject.SetActive(true);
    }
}
