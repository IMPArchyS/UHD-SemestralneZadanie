using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorManager : MonoBehaviour
{
    [SerializeField] private List<Transform> planetSpawns;
    [SerializeField] private List<Transform> asteroidSpawns;
    [SerializeField] private List<Transform> enemySpawns;
    [SerializeField] private GameObject[] planetPrefabs;
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private GameObject enemies;
    [SerializeField] private PlayerData enemyData;
    [SerializeField] private Canvas deathCanvas;
    [SerializeField] private Canvas clearCanvas;

    private bool playOnce;

    private void Awake()
    {
        this.playOnce = false;
        Transform[] allChildObjects = GetComponentsInChildren<Transform>(true);

        foreach (Transform childObject in allChildObjects)
        {
            if (childObject.CompareTag("Planet"))
                planetSpawns.Add(childObject);
            else if (childObject.CompareTag("Asteroid"))
                asteroidSpawns.Add(childObject);
            else if (childObject.CompareTag("Enemy"))
                enemySpawns.Add(childObject);
        }
        this.setCelestialBody();
        this.setEnemies();
    }

    private void LateUpdate()
    {
        GameObject[] e = GameObject.FindGameObjectsWithTag("Pirate");

        if (e.Length == 0)
        {
            Time.timeScale = 0f;
            Entity pp = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
            pp.getData.dataCurrentHp = pp.dataCurrentHP;
            clearCanvas.gameObject.SetActive(true);
            Debug.Log("CLEARED");
        }
        Entity p = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        if (p.dataCurrentHP <= 0)
        {
            if (!this.playOnce)
            {
                SoundManager.instance.playSfx("explode");
                this.playOnce = true;
            }
            Time.timeScale = 0f;
            deathCanvas.gameObject.SetActive(true);
        }
    }

    private void setCelestialBody()
    {
        int randPos = (int)Random.Range(0, 4);
        if (GameManager.currentSectorType.Equals(Type.ASTEROID))
        {
            int randAsteroid = (int)Random.Range(0, 3);
            Instantiate(asteroidPrefabs[randAsteroid], this.asteroidSpawns[randPos].position, enemies.transform.rotation, this.transform);
        }
        if (GameManager.currentSectorType.Equals(Type.PLANET))
        {
            int randPlanet = (int)Random.Range(0, 4);
            Instantiate(planetPrefabs[randPlanet], this.planetSpawns[randPos].position, enemies.transform.rotation, this.transform);
        }
        Debug.Log("Spawned " + GameManager.currentSectorType);
    }

    private void setEnemies()
    {
        bool[] occupied = new bool[this.enemySpawns.Count];
        for (int i = 0; i < GameManager.currentEnemyAmount; i++)
        {
            int randPos = Random.Range(0, GameManager.currentEnemyAmount);
            if (!occupied[randPos])
            {
                Entity e = Instantiate(enemies, this.enemySpawns[randPos].position, enemies.transform.rotation, this.transform).GetComponent<Entity>();
                occupied[randPos] = true;
            }
            else i--;
        }
    }
}
