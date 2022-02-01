using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 100;
    public Slider Slider;
    public Transform AttackPoint;
    public float AttackDistance = 2f;
    public float FollowDistance = 10f;
    public float AttackRange = 1f;
    public float MoveSpeed = 5f;
    public float AttackRate = 2f;
    public LayerMask PlayerLayer;

    private int currentHealth;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private float nextAttackTime = 0f;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerHealth = CharacterController2D.Instance.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    private void Update()
    {

        Slider.value = Mathf.Lerp(Slider.value, currentHealth, 5 * Time.deltaTime);


        float distanceToPlayer = Vector2.Distance(transform.position, CharacterController2D.Instance.transform.position);

        if(distanceToPlayer < FollowDistance && distanceToPlayer > AttackDistance)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer < AttackDistance)
        {
            if(Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / AttackRate;
            }
        }
        else
        {
            StopChase();
        }



        animator.SetFloat("Speed", rigidbody.velocity.magnitude);
    }

    private void Attack()
    {
        if (playerHealth.isDead)
            return;

        StopChase();
        animator.SetTrigger("Attack");

            
        
    }

    public void CanAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, PlayerLayer);

        foreach (var player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(15);
            Debug.Log("player has been hit");
        }
    }

    private void StopChase()
    {
        rigidbody.velocity = Vector3.zero;
    }

    private void ChasePlayer()
    {
        if(transform.position.x < CharacterController2D.Instance.transform.position.x)
        {
            rigidbody.velocity = new Vector2(MoveSpeed, 0);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(transform.position.x > CharacterController2D.Instance.transform.position.x)
        {
            rigidbody.velocity = new Vector2(-MoveSpeed, 0);
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        GetHit();

        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        animator.SetBool("IsDead", true);
        Debug.Log("enemy dead");

        this.enabled = false;
        Destroy(Slider.transform.parent.gameObject);
        GetComponent<Collider2D>().enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (AttackPoint is null)
            return;

        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    public void GetHit()
    {
        rigidbody.AddForce(new Vector2(CharacterController2D.Instance.m_FacingRight ? CharacterController2D.Instance.m_JumpForce * 2 : -CharacterController2D.Instance.m_JumpForce * 2, CharacterController2D.Instance.m_JumpForce / 2));
    }
}
