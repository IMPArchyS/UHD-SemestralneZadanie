using UnityEngine;
using UnityEngine.SceneManagement;

public enum Type {ASTEROID, PLANET, STAR, VOID}
public enum Owner {NONE, PIRATES, ORDER}
public enum ThreatLevel {NONE, LOW, MODERATE, HIGH}
public enum Income {NONE, LOW, MODERATE, HIGH}

[CreateAssetMenu(menuName = "Sector Data")] 
public class SectorData : ScriptableObject
{
    /// Atributes
    [SerializeField] private Owner owner;
    [SerializeField] private ThreatLevel difficulty;
    [SerializeField] private Income tax;
    [SerializeField] private Type type;
    [SerializeField] private bool revealed;
    /// Called when the inspector updates
    private void onValidate()
    {

    }

    public SectorData newSectorData(SectorData data)
    {
        SectorData newData = ScriptableObject.CreateInstance<SectorData>();
        newData.owner = data.getOwner();
        newData.difficulty = data.getDiff();
        newData.type = data.getType();
        newData.revealed = data.isRevealed();
        newData.tax  = data.getTaxes();
        return newData;
    }

    #region  GETTERS
    public Income getTaxes()
    {
        return this.tax;
    }
    public bool isRevealed()
    {
        return this.revealed;
    }
    public Owner getOwner()
    {
        return this.owner;
    }

    public ThreatLevel getDiff()
    {
        return this.difficulty;
    }

    public Type getType()
    {
        return this.type;
    }
    #endregion

    #region SETTERS
    public void setTaxes(Income i)
    {
        this.tax = i;
    }
    public void setRevealed(bool b)
    {
        this.revealed = b;
    }

    public void setOwner(Owner o)
    {
        this.owner = o;
    }

    public void setDiff(ThreatLevel l)
    {
        this.difficulty = l;
    }

    public void setType(Type t)
    {
        this.type = t;
    }
    #endregion
}
