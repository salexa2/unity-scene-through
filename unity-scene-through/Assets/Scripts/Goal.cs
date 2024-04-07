
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Animator door_aniamtor;
    public bool isOpen = false;

    [SerializeField]
    public GameObject projectionToActivate;
    [SerializeField]
    public GameObject lightToActivate;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (door_aniamtor != null)
        {
            if (!isOpen)
            {
                door_aniamtor.Play("Door_Idle");
                projectionToActivate.SetActive(false);
                lightToActivate.SetActive(false);
            }
            if (isOpen)
            {
                isOpen = true;
                door_aniamtor.Play("Door Open");
            }

        }
    }
    public void CompleteGoal() { 

        
     
        {

            if (!isOpen)
            {
                //projection should show! 
               

                if (door_aniamtor != null)
                isOpen = true;  
                projectionToActivate.SetActive(true);
                lightToActivate.SetActive(true);
                door_aniamtor.Play("Door Open");
            }
        }
        
    }
}
