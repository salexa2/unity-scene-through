using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementPlatform : MonoBehaviour
{
    public GameObject[] movementPlatformPosition = new GameObject[2];
    public Transform CurrentPlatformTarget = null;
    public Vector3 CurrentPlatformGoals;
    public int CurrentPlatformTargetedIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (movementPlatformPosition == null)
        {
            return;
        }
        CurrentPlatformTarget = movementPlatformPosition[0].transform;
        CurrentPlatformGoals = new Vector3();
        PlatformGoal();

    }

    // Update is called once per frame
    void Update()
    {
        if (movementPlatformPosition == null)
        {
            return;
        }
        PlatformGoal();
        Vector3 target_position = new Vector3();
        if (this.transform.position.y != CurrentPlatformTarget.position.y && CurrentPlatformGoals.y != 0)
        {
            if (CurrentPlatformTarget.position.y - this.transform.position.y > 0)
            {
                target_position.y = 1;
            }
            else if (CurrentPlatformTarget.position.y - this.transform.position.y < 0)
            {
                target_position.y = -1;
            }
            else
            {
                target_position.y = 0;
            }

        }
        if (this.transform.position.z != CurrentPlatformTarget.position.z && CurrentPlatformGoals.z != 0)
        {
            if (CurrentPlatformTarget.position.z - this.transform.position.z > 0)
            {
                target_position.z = 1;
            }
            else if (CurrentPlatformTarget.position.z - this.transform.position.z < 0)
            {
                target_position.z = -1;
            }
            else
            {
                target_position.z = 0;
            }
        }
        if (this.transform.position.x != CurrentPlatformTarget.position.x && CurrentPlatformGoals.x != 0)
        {
            if (CurrentPlatformTarget.position.x - this.transform.position.x > 0)
            {
                target_position.x = 1;
            }
            else if (CurrentPlatformTarget.position.x - this.transform.position.x < 0)
            {
                target_position.x = -1;
            }
            else
            {
                target_position.x = 0;
            }
        }
        this.gameObject.transform.position = this.gameObject.transform.position + target_position;
    }
    void PlatformGoal()
    {
        if (this.transform.position.y != CurrentPlatformTarget.position.y)
        {
            if (CurrentPlatformTarget.position.y - this.transform.position.y > 0)
            {
                CurrentPlatformGoals.y = 1;
            }
            else if (CurrentPlatformTarget.position.y - this.transform.position.y < 0)
            {
                CurrentPlatformGoals.y = -1;
            }
            else
            {
                CurrentPlatformGoals.y = 0;
            }

        }
        if (this.transform.position.z != CurrentPlatformTarget.position.z)
        {
            if (CurrentPlatformTarget.position.z - this.transform.position.z > 0)
            {
                CurrentPlatformGoals.z = 1;
            }
            else if (CurrentPlatformTarget.position.z - this.transform.position.z < 0)
            {
                CurrentPlatformGoals.z = -1;
            }
            else
            {
                CurrentPlatformGoals.z = 0;
            }
        }
        if (this.transform.position.x != CurrentPlatformTarget.position.x)
        {
            if (CurrentPlatformTarget.position.x - this.transform.position.x > 0)
            {
                CurrentPlatformGoals.x = 1;
            }
            else if (CurrentPlatformTarget.position.x - this.transform.position.x < 0)
            {
                CurrentPlatformGoals.x = -1;
            }
            else
            {
                CurrentPlatformGoals.x = 0;
            }
        }

    } 
}

