using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f ;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerData eData;
    [SerializeField] private PlayerData pData;

    public float getSpeed()
    {
        return this.speed;
    }
    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Bullet") || other.gameObject.tag.Equals("EnemyBullet"))
            return;

        if(other.gameObject.tag.Equals("MapBounds"))
        {
            Destroy(gameObject);
            return;
        }
        if(!other.gameObject.tag.Equals("Bullet") && !other.gameObject.tag.Equals("Bounds"))
        {
            if(other.gameObject.tag.Equals("Player") && this.gameObject.tag.Equals("EnemyBullet"))
            {
                Debug.Log("Hit");
                other.GetComponent<Entity>().dataCurrentHP -= eData.dataDamage;
                Destroy(gameObject);
            }
            else if(other.gameObject.tag.Equals("Pirate") && !this.gameObject.tag.Equals("EnemyBullet"))
            {
                Debug.Log("EnemyHit");
                other.GetComponent<Entity>().dataCurrentHP -= pData.dataDamage;
                if(other.GetComponent<Entity>().dataCurrentHP <= 0) {
                    SoundManager.instance.playSfx("explode");
                    Destroy(other.gameObject);
                }
                Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }

    }
}
