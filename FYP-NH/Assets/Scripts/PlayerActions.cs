using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro UseText;
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;
    
    public AudioSource doorSound;
    public void OnUse() {
        if(Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if(door.IsOpen)
                {
                    door.Close();
                    doorSound.Play();
                }
                else
                {
                    door.Open(transform.position);
                    doorSound.Play();
                }
            }
        }
    }
    private void Update()
    {
        if(Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers)
        && hit.collider.TryGetComponent<Door>(out Door door))
        {
            // if (door.IsOpen)
            // {
            //     UseText.SetText("Close \"E\"/ \"B\"");
            // }
            // else
            // {
            //     UseText.SetText("Open \"E\"/ \"B\"");
            // }
            UseText.gameObject.SetActive(true);
            UseText.transform.position = hit.point - (hit.point - Camera.position).normalized * 0.01f;
            UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);  
        }
        else 
        {
            UseText.gameObject.SetActive(false);
        }
    }
}

