using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpTextScript : MonoBehaviour
{

    public Text pickUpText;
    public GameObject background;
    public GameObject player;

    private bool pickUpAllowed;

    // Start is called before the first frame update
    void Start()
    {
        pickUpText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - transform.position).sqrMagnitude <= 5f) {
            EnableText();
        } else {
            DisableText();
        }
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Success in Picking Up Item");
        }
    }

    private void EnableText()
    {
        pickUpText.gameObject.SetActive(true);
        pickUpText.text = "Press \'E\' to\nPick Up the " + transform.name;
        background.gameObject.SetActive(true);
        pickUpAllowed = true;
    }

    private void DisableText()
    {
        pickUpText.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        pickUpAllowed = false;
    }
}
