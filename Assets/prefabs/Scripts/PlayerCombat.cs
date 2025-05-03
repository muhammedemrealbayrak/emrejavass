using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;


    public LayerMask enemyLayers;



    //Attack 1 i�in
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRange = 0.9f;



    void Update()
    {
        //Attack cooldown i�in
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

        //Belirlenen b�lgede belirlenen �apta daire olu�turur ve dairenin �arpt��� b�t�n nesneleri toplar
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Vurulan d��manlar� tutan listedeki herkese hasar uygulama
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
