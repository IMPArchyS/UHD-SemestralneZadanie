using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SectorMap : MonoBehaviour
{
    public static SectorMap instance;
    public UnityEvent incomeEvent = new UnityEvent();
    [SerializeField] private SectorMapData data;
    [SerializeField] private SectorData[] sectorDataTypes;
    [SerializeField] private PlayerSector player;
    private List<Sector> sectors;

    private void Awake()
    {
        instance = this;
        float targetXScale = 1f; 
        float targetYScale = 1.2f; 

        float scaleX = targetXScale; 
        float scaleY = targetYScale;  

        if (data.getSectorCount() != 15)
        {
            float ratio = targetYScale / targetXScale ;
            float adjustedYScale = Mathf.Sqrt(data.getSectorCount()) * ratio;
            scaleX *= Mathf.Sqrt(data.getSectorCount()) / 2.5f;
            scaleY = adjustedYScale / 2.5f;
        }

        Vector3 scale = new Vector3(scaleX, scaleY, 1f);
        this.transform.localScale = scale;
        this.drawHexGrid();
    }

    private void drawHexGrid()
    {
        this.sectors = new List<Sector>();

        Sector g = Instantiate(data.getSectorPrefab(),Vector2.zero,Quaternion.identity).GetComponent<Sector>();
        this.initialiseCurrentSector(true,g);
        this.sectors.Add(g);
        SpriteRenderer spriteRenderer = g.GetComponent<SpriteRenderer>();

        Vector2 currentHexagonPosition = g.transform.position; 
        float hexagonSize = Mathf.Max(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y); 

        float xOffset = hexagonSize * Mathf.Sqrt(3f) / 2f;
        float yOffset = hexagonSize;

        while(!sectors.Count.Equals(this.data.getSectorCount()))
        {
            Sector currentSector = this.getRandomSector();
            currentHexagonPosition = currentSector.transform.position;

            for (int direction = 0; direction < 6; direction++)
            {
                Vector2 neighboringHexagonPosition = currentHexagonPosition;

                if(sectors.Count.Equals(this.data.getSectorCount()))
                    break;

                while(currentSector.atIndexInfo(direction).isOccupied())
                {
                    direction++;
                    if(direction >= 6)
                        break;
                }
                
                if(direction >= 6)
                    break;

                neighboringHexagonPosition = this.getDirection(direction,neighboringHexagonPosition,yOffset,xOffset);
                this.sectors.Add(Instantiate(data.getSectorPrefab(),neighboringHexagonPosition,Quaternion.identity).GetComponent<Sector>());
                this.initialiseCurrentSector(false,this.sectors.Last());
                this.checkSectors(xOffset,yOffset);
            }
        }
        while(true)
        {
            int randomHome = Random.Range(0,this.sectors.Count);
            if(!this.sectors[randomHome].getData().getType().Equals(Type.STAR) && !this.sectors[randomHome].getData().getType().Equals(Type.VOID))
            {
                this.sectors[randomHome].getData().setDiff(ThreatLevel.NONE);
                this.sectors[randomHome].getData().setRevealed(true);
                this.sectors[randomHome].getData().setOwner(Owner.ORDER);
                this.sectors[randomHome].setColor();
                PlayerSector p = Instantiate(player,this.sectors[randomHome].transform.position,Quaternion.identity).GetComponent<PlayerSector>(); 
                p.setCurrentSector(this.sectors[randomHome]);
                p.revealNeighbours(p.getCurrentSector());
                break;
            }
        }
    }

    private Vector2 getDirection(int direction, Vector2 neighboringHexagonPosition, float yOffset, float xOffset)
    {
        switch (direction)
        {
            case 0: // Top
                neighboringHexagonPosition += new Vector2(0f, yOffset);
                break;
            case 1: // Top Right
                neighboringHexagonPosition += new Vector2(xOffset, yOffset / 2f);
                break;
            case 2: // Bottom Right
                neighboringHexagonPosition += new Vector2(xOffset, -yOffset / 2f);
                break;
            case 3: // Bottom
                neighboringHexagonPosition += new Vector2(0f, -yOffset);
                break;
            case 4: // Bottom Left
                neighboringHexagonPosition += new Vector2(-xOffset, -yOffset / 2f);
                break;
            case 5: // Top Left
                neighboringHexagonPosition += new Vector2(-xOffset, yOffset / 2f);
                break;
            default:
                break;
        }
        return neighboringHexagonPosition;
    }
    private void checkSectors(float xOffset, float yOffset)
    {
        foreach (Sector s in this.sectors)
        {
            s.lookForNeighbours(s.transform.position,xOffset,yOffset);
        }
    }
    private bool availableSector(Sector s)
    {
        return s.getNeighbours().Any(item => item.isOccupied() == false) ? true : false;
    }

    private Sector getRandomSector()
    {
        int randIndex = Random.Range(0,this.sectors.Count);
        while(!this.availableSector(this.sectors[randIndex]))
        {
            randIndex = Random.Range(0,this.sectors.Count);
        }
        return this.sectors[randIndex];
    }

    public void initialiseCurrentSector(bool isCenter, Sector s)
    {
        if(isCenter)  // STAR
        {
            s.setData(sectorDataTypes[9]);
            s.setColor();
            return;
        }  

        int rand = (int)Random.Range(1, 101);
        int randDiff = (int)Random.Range(0,3);

        if (rand <= 40) // VOID
        {
            if(randDiff==0) s.setData(sectorDataTypes[6]);
            else if(randDiff==1) s.setData(sectorDataTypes[7]);
            else s.setData(sectorDataTypes[8]);
        }
        else if (rand <= 75) // ASTEROID
        {
            if(randDiff==0) s.setData(sectorDataTypes[0]);
            else if(randDiff==1) s.setData(sectorDataTypes[1]);
            else s.setData(sectorDataTypes[2]);
        }
        else // PLANET
        {
            if(randDiff==0) s.setData(sectorDataTypes[3]);
            else if(randDiff==1) s.setData(sectorDataTypes[4]);
            else s.setData(sectorDataTypes[5]);
        }
        s.setColor();
        s.setPassiveIncome();
        s.evalDifficulty();
    }

    public List<Sector> getSectorList()
    {
        return this.sectors;
    }
}
