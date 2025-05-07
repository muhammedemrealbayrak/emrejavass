using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Dusman1AI : MonoBehaviour
{
    private Vector2 point;
    private Animator animator;
    private Transform playerPos;
    private bool playerFound = false;
    private Transform currentPosition;
    public float speed = 1f;
    private Rigidbody2D rb;
    private float lookDistance = 4f;
    private bool followPlayer = false;
    private bool isFacingRight = true; // Default value to true assuming the enemy starts facing right
    private float flipValue;
    private float attackRange = 1.6f;
    private float attackCooldown = 1f; // Cooldown time between attacks
    private float timeSinceLastAttack = 0f; // Time since the last attack
    public Transform attackPoint;
    public LayerMask playerLayer;
    private CharacterControler CharacterControler;
    private float damage = 15f;
    private float findPlayerRange = 10f;
    private float flipThreshold = 0.1f; // Threshold to prevent flipping when very close to the player

    void Start()
    {
        CharacterControler = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControler>();
        playerPos = CharacterControler.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.isKinematic = false;
    }

    void Update()
    {
        FindPlayer();
        if (followPlayer)
        {
            FollowPlayer();
            FlipIfNeeded();
            AttackIfNeeded();
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    void FindPlayer()
    {
        Vector3 targetPos = playerPos.position- transform.position;
        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < findPlayerRange)
        {
            followPlayer = true;
            Debug.Log("Player found within range.");
        }
        else
        {
            followPlayer = false; // Stop following if the player is out of range
        }
    }

    void FollowPlayer()
    {
        Vector2 target = new Vector2(playerPos.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        flipValue = target.x - rb.position.x; // Update flipValue to use rb.position.x
    }

    void FlipIfNeeded()
    {
        if (Mathf.Abs(flipValue) > flipThreshold) // Only flip if the absolute value of flipValue is greater than the threshold
        {
            if (flipValue < 0 && isFacingRight)
            {
                Flip();
            }
            else if (flipValue > 0 && !isFacingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    void AttackIfNeeded()
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            if (Vector2.Distance(playerPos.position, rb.position) <= attackRange)
            {
                animator.SetTrigger("Attack");
                StartCoroutine(StopForAttack());
                timeSinceLastAttack = 0f; // Reset the timer after an attack
            }
            else
            {
                animator.ResetTrigger("Attack");
            }
        }
    }

    IEnumerator StopForAttack()
    {
        speed = 0f;
        yield return new WaitForSeconds(0.5f); // Duration of the attack
        speed = 1f;
    }

    void AttackAnimation()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, 1f, playerLayer);
        foreach (var player in hitPlayers)
        {
            if (player.name == "MetalChar")
            {
                player.GetComponent<Metal_Char_Combat>().takeDamage(damage);
            }
                else if (player.name == "Cristal_Char")
                {
                    player.GetComponent<CristalCharControler>().takeDamage(damage);
                }
              /*  else if (player.name == "Water_Priestess(Clone)")
                {
                    player.GetComponent<Water_Priestess_Combat>().takeDamage(damage);
                }
                else if (player.name == "Wind_Hashashin(Clone)")
                {
                    player.GetComponent<Wind_Hashashin_Combat>().takeDamage(damage);
                }
            }*/
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, 1f);
    }
}
