using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour
{
    public int insectHP = 10;
    public Animator animator;
    public void TakeDamge(int damageAmount) {
        insectHP -= damageAmount;
        if(insectHP < 0) 
        {
            animator.SetTrigger("die");
            //Death animation is played here
        }
        else 
        {
            animator.SetTrigger("getHit");
            //Get hit animation is played here
        }
    }
}
