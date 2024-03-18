
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Animator door_aniamtor;
    public bool isOpen = false;
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
            }
            if (isOpen)
            {
                isOpen = true;
                door_aniamtor.Play("Door Open");
            }

        }
    }
    public void CompleteGoal()
    {
        if (door_aniamtor != null)
        {
            if (!isOpen)
            {
                isOpen = true;
                door_aniamtor.Play("Door Open");
            }
        }
        
    }
}
