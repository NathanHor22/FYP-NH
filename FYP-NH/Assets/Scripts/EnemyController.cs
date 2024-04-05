using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    public float enemyHealth = 100f;
    //accessing the navmeshagent;
    NavMeshAgent enemyAgent;
    Transform player;
    private Animator animator;
    //Setting up enemy attacks
    bool canAttack = false;
    bool alreadyAttack = false;

    void Start()
    {
        //calls the animator 
        animator = GetComponent<Animator>();
        //declare the player with tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //declare the navmeshagent
        enemyAgent = GetComponent<NavMeshAgent>();
        //setting speed of the enemy following the player
        enemyAgent.speed = 3.5f;
        animator.SetBool("SlimeChasing", false);

    }

    //execute one round of an enemy attack 
    //Attacking
    //Getting hurt
    //Dying
    private void Update() {
        ChasePlayer();
        if(enemyHealth <= 0) {
            StartCoroutine(Die());
        }
    }

    //Chase player method uses navmesh to check if player is in range before attacking
    void ChasePlayer()
    {
        //Calculate if the player is in range
        var dist = Vector3.Distance(player.position, transform.position);
        //if player is in range
        if (dist <= 7)
        {
            animator.SetBool("SlimeChasing", true);
            //find the position of the player
            enemyAgent.SetDestination(player.position);
        }
        else
        {
            //reset animation since enemy is back to idle and nothing is happening to it
            animator.SetBool("SlimeChasing", false);
            animator.SetBool("SlimeAttacking", false);
            animator.SetBool("SlimeGotHurt", false);
            //when player is out of range the enemy stops following it
            transform.LookAt(player);
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            //stops the enemy immediatly after player is out of range instead of using navmesh auto braking feature
            enemyAgent.SetDestination(enemyAgent.transform.position);
            //returns the animation whether from attacking or chasing to false so it returns to idle

        }
    }

    void OnCollisionStay(Collision collide) {
        //maybe put a forloop in here or while loop
        //add a condition where it only registers as an damage to the player after animation is played
        if(collide.gameObject.tag == "Player") {
            Attack();
            Debug.Log("Enemy is attacking");    
        //if collide with weapon, then the enemyhealth reduced
        } else if(collide.gameObject.tag == "Weapon") {
            Debug.Log("Ow ow ow omg omg painful");
            canAttack = false;
            animator.SetBool("SlimeAttacking", false);
            animator.SetBool("SlimeGotHurt", true);
            enemyHealth = enemyHealth - 10;
        }
    }

    //Attacking animation
    void Attack() {
        //Make sure enemy does not move
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(player);
        //if attacking animation has been played one round, then only inflict damage instead of inflicting damage every frame
        //so if animation length is more than 0.833, then only count as a hit
        //if(animator.GetCurrentAnimatorStateInfo(0).IsName("SlimeAttacking") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
        if(!alreadyAttack) {
            alreadyAttack = true;
            canAttack = true;
            animator.SetBool("SlimeAttacking", true);
            animator.SetBool("SlimeChasing", false);
            Invoke(nameof(ResetAttack),2f);
        }
    }


//Possible solution to make each enemy attack at a certain rate
//use enum to control the rate of the enemy attacking and reset it everytime to prevent continuous loop in one single attack
    //sets up the attack process for the enemy
    //if enemy is not already attacking then start attacking player
    //testing phase is done through using debug.log
    //this is input into the collision stay which loops this method

    //method is to reset the enemy back to it's chasing phase and end the attacking animation
    IEnumerator ResetAttack()
    {
        if(alreadyAttack) {
            alreadyAttack = false;
            Debug.Log("Attack reseting");
            animator.SetBool("SlimeChasing", true);
            animator.SetBool("SlimeAttacking", false);
            canAttack = false;
            Debug.Log("Resetting attack");
            yield return new WaitForSeconds(3);
        }

    }

    IEnumerator Die()
    {
        //give time for the animation to play before the enemy disappears
        animator.SetBool("SlimeChasing", false);
        animator.SetBool("SlimeAttacking", false);
        animator.SetBool("SlimeDead", true);
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    IEnumerator OnCompleteAttackAnimation()
    {
        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
            Debug.Log("Enemy is attacking");
        // TODO: Do something when animation did complete
    }
}