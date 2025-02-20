using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    public bool faceingLeft = true;
    public bool inRange = false;
    public Transform player;
    public float attackRange = 7f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;
    public Animator animator;

    public Transform attackPoint;
    public float attackRadius =1f;
    public LayerMask attackLayer;

    public HealthBarBehavior healthBar;
    public int maxHealth = 5;
    public int health = 0;

    public int bossDamage = 1;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetHealth(health,maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<GameManager>().isGameActive == false)
        {
            return;
        }

        if(health <= 0)
        {
            Die();
        }
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }


        if(inRange == true)
        {
            if (player.position.x > transform.position.x && faceingLeft == true) 
            {
                transform.eulerAngles = new Vector3 (0, -180, 0);
                faceingLeft = false;
            }else if(player.position.x <  transform.position.x && faceingLeft == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                faceingLeft = true;
            }

            if (Vector2.Distance(transform.position, player.position) > retrieveDistance) 
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position,player.position,chaseSpeed * Time.deltaTime);    
            }
            else
            {
                animator.SetBool("Attack1", true);
            }
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            if (hit == false && faceingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                faceingLeft = false;
            }
            else if (hit == false && faceingLeft == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                faceingLeft = true;
            }
        }

        
    }

    public void Attack()
    {
       Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

       if (collInfo == true)
        {
            if(collInfo.gameObject.GetComponent<Player>() != null)
            {
                collInfo.gameObject.GetComponent<Player>().TakeDamage(1 * bossDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(health <= 0)
        {
            return;
        }
        health -= damage;
        animator.SetTrigger("Hurt");
        healthBar.SetHealth(health, maxHealth);
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
           return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if(attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


    void Die()
    {
        Destroy(this.gameObject);
    }



}
