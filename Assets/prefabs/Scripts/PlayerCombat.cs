using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;


    public LayerMask enemyLayers;



    //Attack 1 için
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRange = 0.9f;



    void Update()
    {
        //Attack cooldown için
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack1");

    }


    void AttackOnAnimation()
    {

        //Belirlenen bölgede belirlenen çapta daire oluþturur ve dairenin çarptýðý bütün nesneleri toplar
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Vurulan düþmanlarý tutan listedeki herkese hasar uygulama
        foreach (var enemy in hitEnemies)
        {
            //Vurulan hasar buraya girilecek
        }
    }





    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
}
