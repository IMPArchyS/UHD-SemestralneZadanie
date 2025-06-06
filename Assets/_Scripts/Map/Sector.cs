using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Sector : MonoBehaviour
{
    [SerializeField] private List<NeighbourInfo> neighbourInfos;
    [SerializeField] private SectorData data;
    [SerializeField] private Sprite[] sprites;
    private int passiveIncomeAmount;
    private int enemyAmount;
    private void Awake()
    {
        this.neighbourInfos = new List<NeighbourInfo>();
        SectorMap.instance.incomeEvent.AddListener(evalIncome);
        for (int i = 0; i < 6; i++)
            this.neighbourInfos.Add(new NeighbourInfo());
    }
    #region GAMELOGIC
    public void lookForNeighbours(Vector3 currentPosition, float xOffset, float yOffset)
    {
        Vector3 neighborPosition = currentPosition;
        for (int i = 0; i < 6; i++)
        {
            neighborPosition = currentPosition;
            switch (i)
            {
                case 0: // Top
                    neighborPosition += new Vector3(0f, yOffset);
                    break;
                case 1: // Top Right
                    neighborPosition += new Vector3(xOffset, yOffset / 2f);
                    break;
                case 2: // Bottom Right
                    neighborPosition += new Vector3(xOffset, -yOffset / 2f);
                    break;
                case 3: // Bottom
                    neighborPosition += new Vector3(0f, -yOffset);
                    break;
                case 4: // Bottom Left
                    neighborPosition += new Vector3(-xOffset, -yOffset / 2f);
                    break;
                case 5: // Top Left
                    neighborPosition += new Vector3(-xOffset, yOffset / 2f);
                    break;
                default:
                    break;
            }
            Vector2 raycastOrigin = neighborPosition;
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down,0.001f);
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Sector")
                {
                    this.neighbourInfos[i].setOccupied(true);
                    this.neighbourInfos[i].setPosition(neighborPosition); 
                    this.neighbourInfos[i].setSector(hit.collider.gameObject.GetComponent<Sector>());
                }
            }
        }
    }

    public void setPassiveIncome()
    {
        switch (this.data.getTaxes())
        {
            case Income.NONE:
            this.passiveIncomeAmount = 0;
            break;

            case Income.LOW:
            this.passiveIncomeAmount = Random.Range(1,3);
            break;

            case Income.MODERATE:
            this.passiveIncomeAmount = Random.Range(3,6);
            break;

            case Income.HIGH:
            this.passiveIncomeAmount = Random.Range(6,9);
            break;

            default:
            break;
        }
    }
    public int getEnemyAmount()
    {
        return this.enemyAmount;
    }
    public void evalDifficulty()
    {
        switch (this.data.getDiff())
        {
            case ThreatLevel.NONE:
            this.enemyAmount = 0;
            break;

            case ThreatLevel.LOW:
            this.enemyAmount = Random.Range(3,6);
            break;

            case ThreatLevel.MODERATE:
            this.enemyAmount = Random.Range(6,10);
            break;

            case ThreatLevel.HIGH:
            this.enemyAmount = Random.Range(10,15);
            break;

            default:
            break;
        }
    }

    public void evalIncome()
    {
        PlayerSector p = FindObjectOfType<PlayerSector>();
        TextMeshProUGUI t = GetComponentInChildren<TextMeshProUGUI>();

        if(this.data.getOwner().Equals(Owner.ORDER))
        {
            t.GetComponent<Animator>().Play("SectorIncome",-1,0f);
            p.getPlayerData().dataMoney += this.passiveIncomeAmount;
        }
    }

    #endregion

    #region GETTERSETTERS   
    public void setData(SectorData sd)
    {
        this.data = sd;
        this.data = this.data.newSectorData(this.data);
    }
    public SectorData getData()
    {
        return this.data;
    }

    public NeighbourInfo atIndexInfo(int index)
    {
        return this.neighbourInfos[index];
    }

    public List<NeighbourInfo> getNeighbours()
    {
        return this.neighbourInfos;
    }
    #endregion

    #region DEBUG
    public void setColor()
    {
        SpriteRenderer s = this.transform.GetChild(this.transform.childCount - 1).GetComponent<SpriteRenderer>();
        SpriteRenderer sO = GetComponent<SpriteRenderer>();
        if(this.data.isRevealed())
        {
            s.color = new Vector4(1f, 1f, 1f, 1f);
            if(this.data.getOwner().Equals(Owner.ORDER))
                sO.color = new Vector4(0f, 1f, 0.1f, 1f);
            else if(this.data.getOwner().Equals(Owner.PIRATES))
                sO.color = new Vector4(1f, 0f, 0.1f, 1f);
            else 
                sO.color = new Vector4(0.4f, 0.4f, 0.4f, 1f);   

            switch(this.data.getType())
            {
                case Type.ASTEROID:
                    int randomAsteroid = Random.Range(0,3);
                    s.sprite = sprites[randomAsteroid];
                break;

                case Type.PLANET:
                    int randomPlanet = Random.Range(3,7);
                    s.sprite = sprites[randomPlanet];
                break;

                case Type.VOID:
                    s.color = new Vector4(.7f, .7f, .7f, 0f);
                break;

                case Type.STAR:
                    s.sprite = sprites[7];
                break;
            }
        } 
        else 
        {
            sO.color = new Vector4(0.4f, 0.4f, 0.4f, 1f);
            s.color = new Vector4(.2f, .2f, .2f, 1f);
        }
        this.setIncomeText();
    }

    public void setIncomeText()
    {
        TextMeshProUGUI t = GetComponentInChildren<TextMeshProUGUI>();
        if(!this.data.isRevealed()) 
        {
            t.text = default;
            return;
        }
        if(!this.data.getType().Equals(Type.VOID) || !this.data.getType().Equals(Type.STAR))
            t.text = "+" + this.passiveIncomeAmount;
        else
            t.text = default;
    }
    #endregion
}
