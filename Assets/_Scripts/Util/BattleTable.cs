using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BattleTable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText; 
    [SerializeField] private TextMeshProUGUI ThreatText; 
    [SerializeField] private TextMeshProUGUI OwnerText; 
    [SerializeField] private TextMeshProUGUI IncomeText;  
    private PlayerSector p;
    private void OnEnable()
    {
        this.p = FindObjectOfType<PlayerSector>();
        this.typeText.text = "Type: " + p.getCurrentSector().getData().getType();
        this.ThreatText.text = "Threat level: " + p.getCurrentSector().getData().getDiff();
        this.OwnerText.text = "Owner: " + p.getCurrentSector().getData().getOwner();
        this.IncomeText.text = "Income: " + p.getCurrentSector().getData().getTaxes();
    }
}
