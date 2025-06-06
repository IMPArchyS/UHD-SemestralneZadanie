using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sector Map Data")] 
public class SectorMapData : ScriptableObject
{
    /// Atributes
    [Header("Sector Amount")]
    [SerializeField] private int sectorCount;

    [Header("Sector prefab")]
    [SerializeField] private Sector sectorPrefab;
    
    /// Called when the inspector updates
    private void onValidate()
    {

    }

    public Sector getSectorPrefab()
    {
        return this.sectorPrefab;
    }

    public int getSectorCount()
    {
        return this.sectorCount;
    }
}
