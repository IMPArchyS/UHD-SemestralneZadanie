using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    [SerializeField] private int speed;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int damage;
    private void Awake()
    {
        this.speed = this.data.dataShipSpeed;
        this.maxHP = this.data.dataHp;
        this.currentHP = this.data.dataCurrentHp;
        this.damage = this.data.dataDamage;
    }

    public int dataSpeed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int dataMaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    public int dataCurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    public int dataDamage
    {
        get { return damage; }
        set { damage = value; }
    }

    public PlayerData getData
    {
        get { return this.data; }
    }
}
