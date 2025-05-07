using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CristalCharControler : MonoBehaviour
{
    //  public Healthbar healthbar;
    public Animator animator;
    public LayerMask enemyLayers;
    PlayerMovement playerMovement;
    private float runSpeed;
    public float currentHealth;
    public float maxHealth = 200f;

    //AirAttack için
    public float airAttackRate = 2f;
    private float airNextAttackTime = 0f;
    public Transform airAttackPoint1;
    public Transform airAttackPoint2;
    public float airAttack2Range = 0.9f;

    //Attack Damagelarý
    public float normalAttackDamage = 25f;
    private float spinAttackDamage = 15f;
    private float burstAttackDamage = 50f;
    private float specialAttackDamage = 75f;



    //Attack 1 için
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRange = 0.9f;


    //Attack 2 için
    public float attack2Rate = 10f;
    private float nextAttack2Time = 0f;
    public Transform attack2Point;
    public Transform attack2_2Point;
    public Transform attack2_1Point;
    public float attack2Range = 0.9f;
    public float attack2_1Range = 2f;

    //Attack 3 için
    public float attack3Rate = 2f;
    private float nextAttack3Time = 0f;
    public Transform attack3Point;
    public float attack3Range = 0.9f;

    //Special Attack için
    public float SpecialAttackRate = 2f;
    private float SpecialNextAttackTime = 0f;
    public Transform SpecialAttackPoint;
    public float SpecialAttackRange = 0.9f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        runSpeed = playerMovement.runSpeed;
        currentHealth = maxHealth;
        //healthbar.SetHealth(currentHealth, maxHealth);

    }


    void Update()
    {

        //Attack cooldown için
        if (Time.time >= nextAttackTime && !playerMovement.jump)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;


            }
        }
        if (Time.time >= nextAttackTime && playerMovement.jump)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                AirAttack();
                nextAttackTime = Time.time + 1f / attackRate;

            }
        }
        if (Time.time >= nextAttack2Time)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Attack2();
                nextAttack2Time = Time.time + 1f / attack2Rate;


            }
        }
        if (Time.time >= nextAttack3Time)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack3();
                nextAttack3Time = Time.time + 1f / attack3Rate;


            }
        }
        if (Time.time >= SpecialNextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                SpecialAttack();
                SpecialNextAttackTime = Time.time + 1f / SpecialAttackRate;


            }
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack1");
        StartCoroutine(Attack1());
    }
    void AirAttack()
    {
        animator.SetTrigger("Attack1");
    }
    void Attack2()
    {
        animator.SetTrigger("Attack2");
    }
    void Attack3()
    {
        animator.SetTrigger("Attack3");
    }
    void SpecialAttack()
    {
        animator.SetTrigger("SpecialAttack");
    }

    void AirAttackAnimation()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(airAttackPoint1.position, airAttackPoint2.position, enemyLayers);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.GetComponent<enemy1>() != null) { enemy.GetComponent<enemy1>().TakeDamage(normalAttackDamage); }
            /*   if (enemy.GetComponent<enemy2>() != null) { enemy.GetComponent<enemy2>().TakeDamage(normalAttackDamage); }
               if (enemy.GetComponent<enemy3>() != null) { enemy.GetComponent<enemy3>().TakeDamage(normalAttackDamage); }
               if (enemy.GetComponent<enemy4>() != null) { enemy.GetComponent<enemy4>().TakeDamage(normalAttackDamage); }
               if (enemy.GetComponent<enemy5>() != null) { enemy.GetComponent<enemy5>().TakeDamage(normalAttackDamage); }
               if (enemy.GetComponent<FireBall>() != null) { enemy.GetComponent<FireBall>().TakeDamage(normalAttackDamage); }
               if (enemy.GetComponent<Boss>() != null) { enemy.GetComponent<Boss>().TakeDamage(normalAttackDamage); }*/
        }

    }
    void SpecialAttackOnAnimaton()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(SpecialAttackPoint.position, SpecialAttackRange, enemyLayers);
        foreach (var enemy in hitEnemies)
        {

            if (enemy.GetComponent<enemy1>() != null) { enemy.GetComponent<enemy1>().TakeDamage(specialAttackDamage); }
            /*   if (enemy.GetComponent<Boss>() != null) { enemy.GetComponent<Boss>().TakeDamage(specialAttackDamage); }
               if (enemy.GetComponent<enemy2>() != null) { enemy.GetComponent<enemy2>().TakeDamage(specialAttackDamage); }
               if (enemy.GetComponent<enemy3>() != null) { enemy.GetComponent<enemy3>().TakeDamage(specialAttackDamage); }
               if (enemy.GetComponent<enemy4>() != null) { enemy.GetComponent<enemy4>().TakeDamage(specialAttackDamage); }
               if (enemy.GetComponent<enemy5>() != null) { enemy.GetComponent<enemy5>().TakeDamage(specialAttackDamage); }
               if (enemy.GetComponent<FireBall>() != null) { enemy.GetComponent<FireBall>().TakeDamage(specialAttackDamage); }
            */
        }
    }
    void Attack3OnAnimation()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attack3Point.position, attack3Range, enemyLayers);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.GetComponent<enemy1>() != null) { enemy.GetComponent<enemy1>().TakeDamage(burstAttackDamage); }
            /*    if (enemy.GetComponent<Boss>() != null) { enemy.GetComponent<Boss>().TakeDamage(burstAttackDamage); }
                if (enemy.GetComponent<enemy2>() != null) { enemy.GetComponent<enemy2>().TakeDamage(burstAttackDamage); }
                if (enemy.GetComponent<enemy3>() != null) { enemy.GetComponent<enemy3>().TakeDamage(burstAttackDamage); }
                if (enemy.GetComponent<enemy4>() != null) { enemy.GetComponent<enemy4>().TakeDamage(burstAttackDamage); }
                if (enemy.GetComponent<enemy5>() != null) { enemy.GetComponent<enemy5>().TakeDamage(burstAttackDamage); }
                if (enemy.GetComponent<FireBall>() != null) { enemy.GetComponent<FireBall>().TakeDamage(burstAttackDamage); }
            */
        }
    }




    void AttackOnAnimation()
    {
        //Belirlenen bölgede belirlenen çapta daire oluþturur ve dairenin çarptýðý bütün nesneleri toplar
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attack2Point.position, attack2Range, enemyLayers);
        foreach (var enemy in hitEnemies)
        {
            // if (enemy.name == "Enemy1") { enemy.GetComponent<enemy1>().TakeDamage(normalAttackDamage); }
            if (enemy.GetComponent<enemy1>() != null) { enemy.GetComponent<enemy1>().TakeDamage(normalAttackDamage); }
            /*    if (enemy.GetComponent<enemy2>() != null) { enemy.GetComponent<enemy2>().TakeDamage(normalAttackDamage); }
                if (enemy.GetComponent<enemy3>() != null) { enemy.GetComponent<enemy3>().TakeDamage(normalAttackDamage); }
                if (enemy.GetComponent<enemy4>() != null) { enemy.GetComponent<enemy4>().TakeDamage(normalAttackDamage); }
                if (enemy.GetComponent<enemy5>() != null) { enemy.GetComponent<enemy5>().TakeDamage(normalAttackDamage); }
                if (enemy.GetComponent<FireBall>() != null) { enemy.GetComponent<FireBall>().TakeDamage(normalAttackDamage); }
                if (enemy.GetComponent<Boss>() != null) { enemy.GetComponent<Boss>().TakeDamage(normalAttackDamage); }

    */
        }
    }

    void Attack2OnAnimationNormal()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Vurulan düþmanlarý tutan listedeki herkese hasar uygulama
        foreach (var enemy in hitEnemies)
        {
            if (enemy.GetComponent<enemy1>() != null) { enemy.GetComponent<enemy1>().TakeDamage(spinAttackDamage); }
            /*  if (enemy.GetComponent<Boss>() != null) { enemy.GetComponent<Boss>().TakeDamage(spinAttackDamage); }
              if (enemy.GetComponent<enemy2>() != null) { enemy.GetComponent<enemy2>().TakeDamage(spinAttackDamage); }
              if (enemy.GetComponent<enemy3>() != null) { enemy.GetComponent<enemy3>().TakeDamage(spinAttackDamage); }
              if (enemy.GetComponent<enemy4>() != null) { enemy.GetComponent<enemy4>().TakeDamage(spinAttackDamage); }
              if (enemy.GetComponent<enemy5>() != null) { enemy.GetComponent<enemy5>().TakeDamage(spinAttackDamage); }
              if (enemy.GetComponent<FireBall>() != null) { enemy.GetComponent<FireBall>().TakeDamage(spinAttackDamage); }
            */
        }
    }

    void Attack2OnAnimationSpin()
    {

        //Belirlenen bölgede belirlenen çapta daire oluþturur ve dairenin çarptýðý bütün nesneleri toplar
        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(attack2_1Point.position, attack2_2Point.position, enemyLayers);

        //Vurulan düþmanlarý tutan listedeki herkese hasar uygulama
        foreach (var enemy in hitEnemies)
        {
            //Vurulan hasar buraya girilecek
            //   if (enemy.GetComponent<Boss>() != null) { enemy.GetComponent<Boss>().TakeDamage(spinAttackDamage); }
            if (enemy.GetComponent<enemy1>() != null) { enemy.GetComponent<enemy1>().TakeDamage(spinAttackDamage); }
            //   if (enemy.GetComponent<enemy2>() != null) { enemy.GetComponent<enemy2>().TakeDamage(spinAttackDamage); }
            //  if (enemy.GetComponent<enemy3>() != null) { enemy.GetComponent<enemy3>().TakeDamage(spinAttackDamage); }
            //   if (enemy.GetComponent<enemy4>() != null) { enemy.GetComponent<enemy4>().TakeDamage(spinAttackDamage); }
            //   if (enemy.GetComponent<enemy5>() != null) { enemy.GetComponent<enemy5>().TakeDamage(spinAttackDamage); }
            //    if (enemy.GetComponent<FireBall>() != null) { enemy.GetComponent<FireBall>().TakeDamage(spinAttackDamage); }

        }
    }


    IEnumerator Attack1()
    {

        playerMovement.movement = true;
        yield return new WaitForSeconds(0.8f);
        playerMovement.movement = false;
    }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        //    healthbar.SetHealth(currentHealth, maxHealth);
        StartCoroutine(WaitHurt());
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        animator.SetTrigger("Dead");
        playerMovement.runSpeed = 0f;
        this.enabled = true;
        SceneManager.LoadScene("gameoverscene");
        Destroy(gameObject);

    }
    IEnumerator WaitHurt()
    {
        animator.SetTrigger("Hurt");
        playerMovement.runSpeed = 0f;
        yield return new WaitForSeconds(0.4f);
        playerMovement.runSpeed = 25f;
    }
}

