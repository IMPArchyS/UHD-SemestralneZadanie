using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSector : MonoBehaviour
{
    [SerializeField] private Sector currentSector;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int speed;
    [SerializeField] private PlayerData data;

    private void Awake()
    {
        this.data.initData();
        this.rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(GameManager.instance.gameState.Equals(State.PLAYERMOVE))
        {
            if(!CanvasManager.instance.getInMenu() && !CanvasManager.instance.getInOtherMenu())
            {
                if(Input.GetMouseButtonUp(1))
                    this.getSectorInfo(false);
                if(Input.GetMouseButtonUp(2))
                    this.getSectorInfo(true);

                if(Input.GetMouseButtonUp(0))
                    this.targetSector();
            }
        }
    }

    #region DATA
    public PlayerData getPlayerData()
    {
        return this.data;
    }

    public void setCurrentSector(Sector s)
    {
        this.currentSector = s;
    }

    public Sector getCurrentSector()
    {
        return this.currentSector;
    }
    #endregion

    #region LOGIC
    public void revealNeighbours(Sector s)
    {
        foreach (NeighbourInfo n in s.getNeighbours())
        {
            if(n.isOccupied())
            {
                n.getSector().getData().setRevealed(true);
                n.getSector().setColor();
            }
        }
    }

    private void targetSector()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if(rayHit.collider != null)
        {   
            Sector s = rayHit.collider.gameObject.GetComponent<Sector>();
            bool inRange = false;
            // check if its in the neigbourhood
            foreach (NeighbourInfo nb in this.currentSector.getNeighbours())
            {
                if(nb.isOccupied())
                {
                    if(nb.getSector().Equals(s))
                    {
                        inRange = true;
                        break;
                    }
                    else
                        inRange = false;
                }
            }
            // if not check if its revealed
            if(s.getData().isRevealed() && inRange == false)
                inRange = true;

            // if in range check if its not a star or the current sector
            if(inRange && !CanvasManager.instance.isInOtherMenu())
            {
                if(s.Equals(this.currentSector))
                    Debug.LogWarning("Current Sector");
                else if(s.getData().getType().Equals(Type.STAR))
                    Debug.LogWarning("Star Sector");
                else if(this.currentSector.transform.position.Equals(this.rb.position))
                    StartCoroutine(this.moveToSector(s));
                else
                    Debug.Log("ship is still moving");
            }
            else
                Debug.Log("out of range");
        }
    }

    private IEnumerator moveToSector(Sector s) 
    {
        Debug.Log("moving to Sector: " + s.getData().getType());
        
        HoverManager.onMouseLoseFocus();
        

        Vector3 directionToTarget = s.transform.position - this.transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        this.transform.rotation = targetRotation;
        while(!this.rb.position.Equals(s.transform.position))
        {
            this.rb.position = Vector2.MoveTowards(this.rb.position, s.transform.position, this.speed * Time.deltaTime);
            yield return null;
        }
        this.currentSector = s;
        this.transform.rotation = Quaternion.identity;

        if(!s.getData().getDiff().Equals(ThreatLevel.NONE))
            GameManager.instance.playerAction();
    }
    #endregion

    #region DEBUG
    private void getSectorInfo(bool reveal)
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if(rayHit.collider != null)
        {   
            Sector s = rayHit.collider.gameObject.GetComponent<Sector>();
            Debug.Log("----------------");
            if(s.Equals(this.currentSector))
                Debug.Log("Current Sector");
            else if(s.getData().getType().Equals(Type.STAR))
                Debug.Log("Star Sector");

            if(reveal)
            {
                s.getData().setRevealed(true);
                s.setColor();
            }
            Debug.Log("position: " + s.transform.position);
            Debug.Log("revealed: " + s.getData().isRevealed());
            Debug.Log("threat lvl: " + s.getData().getDiff());
            Debug.Log("owner: " + s.getData().getOwner());
            Debug.Log("type: " + s.getData().getType());
            Debug.Log("income: " + s.getData().getTaxes());
            Debug.Log("----------------");
        }
    }
    #endregion
}
