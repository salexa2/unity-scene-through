using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    public GameObject player;

    private void FixedUpdate()
    {
        if(player != null)
        {
            //player.transform.position -= player.transform.parent.localPosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")){
           // other.transform.parent.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            //other.transform.parent.SetParent(null);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.tag.Equals("Player"))
        {
            other.transform.parent.SetParent(transform);
            player = other.gameObject;
        }*/
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("It parent");
            player = collision.gameObject;
        }
        if (player != null)
        {
            player.transform.parent = gameObject.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if(player != null)
            {
                player.transform.parent = null;
                player = null;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (player != null)
        {
            player.transform.parent = gameObject.transform;
        }
    }*/
}
