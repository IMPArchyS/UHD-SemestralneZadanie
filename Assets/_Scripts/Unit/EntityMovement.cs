using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EntityMovement : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool player;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private Camera cam;
    [SerializeField] private float snapRadius = 6f;
    [SerializeField] private LayerMask overlapLayer;
    [SerializeField] private float distanceToPlayer;
    private Vector2 movement;
    private bool snapped;
    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(this.player)
        {

            this.movement.x = Input.GetAxisRaw("Horizontal");
            this.movement.y = Input.GetAxisRaw("Vertical");

            Vector2 mousePos = cam.ScreenToWorldPoint(new Vector3( Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z ));
            this.followMouse(mousePos);
        }
        else
            this.snapToTarget();
    }

    private void FixedUpdate()
    {
        if(this.player)
            this.movePlayer();
        else
        {
            this.moveAI();
        }
    }
    public bool isSnapped()
    {
        return this.snapped;
    }
    private void followMouse(Vector2 pos)
    {
        Vector3 lookDir = pos - this.rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void movePlayer()
    {
        this.movement.Normalize();
        Vector2 targetVelocity = this.movement * this.entity.dataSpeed;
        Vector2 currentVelocity = this.rb.velocity;
        if (this.movement.magnitude > 0)
        {
            Vector2 acceleratedVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            rb.velocity = Vector2.ClampMagnitude(acceleratedVelocity, this.entity.dataSpeed);
        }
        else
        {
            Vector2 deceleratedVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rb.velocity = deceleratedVelocity; 
        }
    }

    private void moveAI()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Player");
        if (this.snapped)
        {   
            Vector2 direction = targetObject.transform.position - transform.position;
            direction.Normalize();
            Vector2 targetVelocity = direction * this.entity.dataSpeed;
            Vector2 currentVelocity = this.rb.velocity;
            if (Vector2.Distance(targetObject.transform.position, this.transform.position) > this.distanceToPlayer)
            {
                if (direction.magnitude > 0)
                {
                    Vector2 acceleratedVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
                    rb.velocity = Vector2.ClampMagnitude(acceleratedVelocity, this.entity.dataSpeed);
                }
                else
                {
                    Vector2 deceleratedVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
                    rb.velocity = deceleratedVelocity; 
                }
            }
        }
    }

    private void snapToTarget()
    {
        Collider2D[] snapColliders = Physics2D.OverlapCircleAll(transform.position, this.snapRadius, this.overlapLayer);
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (snapColliders.Any(collider => collider.CompareTag("Player")))
        {
            Vector3 directionToTarget = p.transform.position - this.transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            this.transform.rotation = targetRotation;
            this.snapped = true;
        }
        else
            this.snapped = false;
    }
}
