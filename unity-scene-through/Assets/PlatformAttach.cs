using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    public GameObject player;
    public Vector3 size = Vector3.one;

    /*private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject == player)
        {
            player.transform.parent = this.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            player.transform.parent = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject == player)
        {
            player.transform.parent = this.gameObject.transform;
        }
    }*/
    private void OnCollisionEnter(Collision collision)
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
    }
}
