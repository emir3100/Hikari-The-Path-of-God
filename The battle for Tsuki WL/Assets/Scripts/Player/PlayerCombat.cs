using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public float AttackRate = 2f;
    public LayerMask EnemyLayers;

    public float MaxStamina = 100f;
    public Image Slider;

    public AudioClip swing1;
    public AudioClip swing2;
    public AudioClip shield;

    private float nextAttackTime = 0f;
    private Animator animator;
    private float currentStamina;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentStamina = MaxStamina;
    }

    private void Update()
    {
        Slider.fillAmount = Mathf.Lerp(Slider.fillAmount, currentStamina / 100, 5 * Time.deltaTime);

        if (currentStamina < 100f && currentStamina > 0f && Time.time >= nextAttackTime)
            StartCoroutine("Regenerate");

        if (Time.time >= nextAttackTime && currentStamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / AttackRate;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && currentStamina > 0)
        {
            Defend();
        }
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(3f);
        currentStamina += 10 * Time.deltaTime;
    }

    private void Attack()
    {
        currentStamina -= 10;
        int attack = UnityEngine.Random.Range(1, 3);
        animator.SetTrigger($"Attack{attack}");

        if (attack == 1)
            GameManager.Instance.AudioSource.PlayOneShot(swing1);
        else if (attack == 2)
            GameManager.Instance.AudioSource.PlayOneShot(swing2);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(25);
        }
    }
    
    private void Defend()
    {
        currentStamina -= 5;
        animator.SetTrigger("Defend");
        GameManager.Instance.AudioSource.PlayOneShot(shield);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().DefendBlock();
        }

    }

    private void OnDrawGizmosSelected()
    {
        if(AttackPoint is null)
            return;

        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
