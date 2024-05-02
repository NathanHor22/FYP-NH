using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damageAmount = 2;
    //if the weapon collides with the enemy then the weapon deals damage to the enemy
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy") {
            other.GetComponent<Insect>().TakeDamge(damageAmount);
        }
    }
}   
