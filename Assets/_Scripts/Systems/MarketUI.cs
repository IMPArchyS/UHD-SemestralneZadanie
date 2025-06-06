using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MarketUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dmgPay;
    [SerializeField] private TextMeshProUGUI speedPay;
    [SerializeField] private TextMeshProUGUI hpPay;
    [SerializeField] private TextMeshProUGUI piratesPay;
    [SerializeField] private TextMeshProUGUI playerMoney;
    [SerializeField] private int dmgAmount;
    [SerializeField] private int speedAmount;
    [SerializeField] private int hpAmount;
    [SerializeField] private int dmgCost;
    [SerializeField] private int speedCost;
    [SerializeField] private int hpCost;
    [SerializeField] private int piratesCost;
    [SerializeField] private PlayerData pirateData;

    private PlayerSector p;
    private void Awake()
    {
        p = FindObjectOfType<PlayerSector>();
        this.playerMoney.text = "on ship: " + p.getPlayerData().dataMoney.ToString();
        this.dmgPay.text = this.dmgCost.ToString();
        this.speedPay.text = this.speedCost.ToString();
        this.hpPay.text = this.hpCost.ToString();
        this.piratesPay.text = this.piratesCost.ToString();
    }

    private void OnEnable()
    {
        this.checkMoney();
        this.checkAvailable();
    }
    public void buyUpgrade(string upgradeName)
    {
        SoundManager.instance.playSfx("buy");
        switch (upgradeName)
        {
            case "DMG":
                if(p.getPlayerData().dataMoney >= this.dmgCost)
                {
                    this.dmgAmount--;
                    p.getPlayerData().dataDamage += 2;
                    p.getPlayerData().dataMoney -= this.dmgCost;
                    this.dmgCost += this.dmgCost / 2;
                    this.dmgPay.text = this.dmgCost.ToString();
                }
            break;

            case "SPEED":
                if(p.getPlayerData().dataMoney >= this.speedCost)
                {
                    this.speedAmount--;
                    p.getPlayerData().dataShipSpeed += 1;
                    p.getPlayerData().dataMoney -= this.speedCost;
                    this.speedCost += this.speedCost / 2;
                    this.speedPay.text = this.speedCost.ToString();
                }
            break;

            case "HP":
                if(p.getPlayerData().dataMoney >= this.hpCost)
                {
                    this.hpAmount--;
                    p.getPlayerData().dataHp += 25;
                    p.getPlayerData().dataMoney -= this.hpCost;
                    this.hpCost += this.hpCost / 2;
                    this.hpPay.text = this.hpCost.ToString();
                }
            break;

            case "PIRATES":
                if(p.getPlayerData().dataMoney >= this.piratesCost)
                {
                    p.getPlayerData().dataMoney -= this.piratesCost;
                    this.piratesCost += 15;
                    this.pirateData.dataDamage -= 2;
                    this.pirateData.dataCurrentHp -= 10;
                    this.piratesPay.text = this.piratesCost.ToString();
                }
            break;

            default:
            break;
        }
        this.checkMoney();
        this.checkAvailable();
    }

    private void checkMoney()
    {
        this.playerMoney.text = "on ship: " + p.getPlayerData().dataMoney.ToString();

        if(p.getPlayerData().dataMoney < this.dmgCost)
            this.dmgPay.GetComponent<Button>().interactable = false;
        else
            this.dmgPay.GetComponent<Button>().interactable = true;

        if(p.getPlayerData().dataMoney < this.speedCost)
            this.speedPay.GetComponent<Button>().interactable = false;
        else
            this.speedPay.GetComponent<Button>().interactable = true;

        if(p.getPlayerData().dataMoney < this.hpCost)
            this.hpPay.GetComponent<Button>().interactable = false;
        else
            this.hpPay.GetComponent<Button>().interactable = true;
        
        if(p.getPlayerData().dataMoney < this.piratesCost)
            this.piratesPay.GetComponent<Button>().interactable = false;
        else
            this.piratesPay.GetComponent<Button>().interactable = true;
    }
    private void checkAvailable()
    {

        if(this.dmgAmount == 0)
        {
            this.dmgPay.GetComponent<Button>().interactable = false;
            this.dmgPay.text = "X";
        }
        if(this.speedAmount == 0)
        {
            this.speedPay.GetComponent<Button>().interactable = false;
            this.speedPay.text = "X";
        }
        if(this.hpAmount == 0)
        {
            this.hpPay.GetComponent<Button>().interactable = false;
            this.hpPay.text = "X";
        }
        if(this.pirateData.dataCurrentHp == this.pirateData.dataHp )
        {
            this.piratesPay.GetComponent<Button>().interactable = false;
            this.piratesPay.text = "X";
        }
        else
            this.piratesPay.text = this.piratesCost.ToString();
    }
}
