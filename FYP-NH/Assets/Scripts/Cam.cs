using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Objectfader _fader;

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null) 
        {
            //Check direction of camera
            Vector3 dir = player.transform.position - transform. position;
            Ray ray = new Ray(transform.position, dir);
            //Raycast casted
            RaycastHit hit;
            //Check physics to see if player hit the wall
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider == null)
                {
                    return;
                }
                if (hit.collider.gameObject == player)
                {
                    //nothing is in front of player
                    if(_fader != null)
                    {
                        _fader.DoFade = false;
                    }
                }
                else
                {
                    _fader = hit.collider.gameObject.GetComponent<Objectfader>();
                    if(_fader != null)
                    {
                        _fader.DoFade = true;
                    }
                }
            }
        }
    }
}
