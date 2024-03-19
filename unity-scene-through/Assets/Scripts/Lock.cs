using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
     protected bool isLock = false;
    [SerializeField] protected string lockPassword;
    [SerializeField] protected GameObject pivot;
    [SerializeField] protected float angleDoor = 100;
    [SerializeField] protected float doorSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (angleDoor < 0 && doorSpeed > 0) {
            doorSpeed *= -1;
            angleDoor = Mathf.Abs(angleDoor);
        }else if (angleDoor < 0)
        {
            Debug.Log("Error Error Error line: 25");
            return;
        }
        if (isLock && pivot != null && Mathf.Abs(pivot.transform.rotation.eulerAngles.y) <= angleDoor)
        {
            pivot.transform.Rotate(transform.up, doorSpeed*Time.deltaTime);
        }
        else
        {
            isLock = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Interaction>() != null)
        {
           
            other.gameObject.GetComponent<Interaction>().objectInteraction.gameObject.SetActive(true);
            Interaction tempInteraction = other.gameObject.GetComponent<Interaction>();

            if (tempInteraction.objectInteraction != null 
                && tempInteraction.objectInteraction.tag.Equals("Key") 
                && tempInteraction.objectInteraction.GetComponent<Key>() != null 
                && tempInteraction.objectInteraction.GetComponent<Key>().keyPassword.Equals(lockPassword))
            {
                Destroy(other.gameObject.GetComponent<Interaction>().objectInteraction);
                other.gameObject.GetComponent<Interaction>().objectInteraction = null;
                isLock = true;
            }
        }
    }
}
