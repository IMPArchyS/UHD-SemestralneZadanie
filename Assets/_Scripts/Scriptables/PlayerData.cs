using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")] 
public class PlayerData : ScriptableObject
{
    [Header("Initial data")]
    [SerializeField] private int initHp;
    [SerializeField] private int initDamage;
    [SerializeField] private int initMoney;
    [SerializeField] private int initShipSpeed;
    [Header("Actual data")]
    [SerializeField] private int hp;
    [SerializeField] private int currentHp;
    [SerializeField] private int damage;
    [SerializeField] private int money;
    [SerializeField] private int shipSpeed;

    #region DATA
    public void initData()
    {
        this.hp = this.initHp;
        this.damage = this.initDamage;
        this.money = this.initMoney;
        this.shipSpeed = this.initShipSpeed;
        this.currentHp = this.hp;
    }
    public int dataCurrentHp
    {
        get { return currentHp; }
        set { currentHp = value; }
    }
    public int dataHp
    {
        get { return hp; }
        set { hp = value; }
    }

    public int dataDamage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int dataMoney
    {
        get { return money; }
        set { money = value; }
    }

    public int dataShipSpeed
    {
        get { return shipSpeed; }
        set { shipSpeed = value; }
    }
    #endregion
}
