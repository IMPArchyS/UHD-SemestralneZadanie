using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceShipInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI dmgText;

    private void OnEnable()
    {
        PlayerSector p = FindObjectOfType<PlayerSector>();
        this.moneyText.text = p.getPlayerData().dataMoney.ToString();
        this.hpText.text = p.getPlayerData().dataCurrentHp.ToString() + '/' + p.getPlayerData().dataHp.ToString();
        this.speedText.text = p.getPlayerData().dataShipSpeed.ToString();
        this.dmgText.text = p.getPlayerData().dataDamage.ToString();
    }
}
