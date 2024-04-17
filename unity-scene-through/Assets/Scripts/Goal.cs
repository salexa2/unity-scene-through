
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Animator door_aniamtor;
    public bool isOpen = false;
    public GameObject[] other = new GameObject[0]; 

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
                projectionToActivate.SetActive(true);
                lightToActivate.SetActive(true);
                isOpen = true;
                bool allTrigger = true;
                if (other.Length != 0)
                {
                    for(int i = 0; i < other.Length; i++)
                    {
                        if (other[i] != null)
                        {
                            if (other[i].GetComponent<Goal>() != null)
                            {
                                if (other[i].GetComponent<Goal>().isOpen == false)
                                {
                                    allTrigger = false;
                                }
                            }
                        }
                    }
                    if (!allTrigger)
                    {
                        return;
                    }
                }
                door_aniamtor.Play("Door Open");
            }

        }
    }
    public void CompleteGoal() { 

        
     
        {

            if (!isOpen)
            {
                //projection should show! 
                isOpen = true;  
                
            }
        }
        
    }
}
