using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityShoot : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private bool player;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireSpeed;
    private float fireTimer;
    private void Awake()
    {
        this.fireTimer = 0f;
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        if(this.player)
        {
            if(Input.GetButton("Fire1") && this.fireTimer >= this.fireSpeed)
            {
                this.shootPlayer();
                this.fireTimer = 0f;
            }
        }
        else 
        {
            if (this.fireTimer >= this.fireSpeed)
            {
                this.shootAI();
                this.fireTimer = 0f;
            }
        } 
    }

    private void shootPlayer()
    {
        this.shoot();
    }

    private void shootAI()
    {
        if (GetComponent<EntityMovement>().isSnapped())
        {
            this.shoot();
        }
    }

    private void shoot()
    {
        if(this.player) 
            SoundManager.instance.playSfx("shoot1");
        else
            SoundManager.instance.playSfx("shoot2");
        Bullet bullet = Instantiate(this.bulletPrefab, this.shootPoint.position, this.shootPoint.rotation, this.transform).GetComponent<Bullet>();
        bullet.GetComponent<Rigidbody2D>().AddForce(shootPoint.up * bullet.getSpeed(), ForceMode2D.Impulse);
    }
}
