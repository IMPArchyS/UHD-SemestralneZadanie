using UnityEngine;

[System.Serializable]
public class NeighbourInfo
{
    [SerializeField] private bool occupied;
    [SerializeField] private Vector2 position;
    [SerializeField] private Sector sector;

    public NeighbourInfo()
    {
        this.occupied = false;
        this.position = new Vector2(0,0);
    }
    #region GETTERSETTER
    public void setOccupied(bool b)
    {
        this.occupied = b;
    }
    public void setPosition(Vector2 v)
    {
        this.position = v;
    }

    public void setSector(Sector s)
    {
        this.sector = s;
    }

    public bool isOccupied()
    {
        return this.occupied;
    }

    public Vector2 getPosition()
    {
        return this.position;
    }

    public Sector getSector()
    {
        return this.sector;
    }
    #endregion
}
