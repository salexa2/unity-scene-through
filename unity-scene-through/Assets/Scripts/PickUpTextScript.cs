using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpTextScript : MonoBehaviour
{

    //public Text pickUpText;
    //public GameObject background;
    //public GameObject player;
    public GameObject eKey;

    private bool pickUpAllowed;

    // Start is called before the first frame update
    void Start()
    {
        //pickUpAllowed = true;
        //pickUpText.gameObject.SetActive(false);
        //background.gameObject.SetActive(false);
        //eKey.gameObject.SetActive(false);
        DisableText();
    }

    // Update is called once per frame
    void Update()
    {
        /*if ((player.transform.position - transform.position).sqrMagnitude <= 1f & pickUpAllowed == true) {
            EnableText();
        } else {
            DisableText();
        }*/
        if (pickUpAllowed)
        {
            EnableText();
        }
        if (pickUpAllowed == false)
        {
            DisableText();
        }
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            DisableText();
            pickUpAllowed = false;
        }
        if (pickUpAllowed == false && Input.GetKeyDown(KeyCode.Q))
        {
            pickUpAllowed = true;
        }
    }

    private void EnableText()
    {
        //pickUpText.gameObject.SetActive(true);
        //pickUpText.text = "Press        to Interact";
        //background.gameObject.SetActive(true);
        eKey.gameObject.SetActive(true);
        pickUpAllowed = true;
    }

    private void DisableText()
    {
        //pickUpText.gameObject.SetActive(false);
        //background.gameObject.SetActive(false);
        eKey.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mirror"))
        {
            pickUpAllowed = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mirror"))
        {
            pickUpAllowed = false;
        }
    }
}
