using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Power : MonoBehaviour
{
    public GameObject start_position;
    public LineRenderer line_renderer;
    public int bounces = 2;
    bool reflectOnlyMirror = false;
    public float angle = 0;
    public bool killPlayer = true;
    public GameObject player;
    public Vector3 direciton; 
    float power_range = 100;
    public GameObject power_Light = null;
    public GameObject[] lightPoint = new GameObject[0];
    public GameObject[] raycreate = new GameObject[0];


    GameObject tmp_goal_object = null;
    struct RayData
    {
        bool kill;
        bool interected;
        

    }

    void Start()
    {
        line_renderer = GetComponent<LineRenderer>();
        line_renderer.SetPosition(0, start_position.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if(raycreate != null)
        {
            for (int i = 0; i < raycreate.Length; i++)
            {
                Destroy(raycreate[i]);
                raycreate[i] = null;
            }
        }
        
        line_renderer.positionCount = 2 + bounces;
        line_renderer.SetPosition(0, start_position.transform.position);
        direciton = transform.forward;
        castRay(start_position.transform.position, direciton);
    }
    /*
     * A method to cast a ray from source.
     * p0 is the start position.
     * p1 is the direction
     */
    public void castRay(Vector3 p0, Vector3 p1) 
    {
        line_renderer.SetPosition(0, start_position.transform.position);
        Ray ray = new Ray(p0, p1);
        RaycastHit hit;
        for (int i = 0; i < bounces+1; i++)
        {
            ray = new Ray(p0, p1);
            if (Physics.Raycast(ray, out hit, power_range, 1))
            {
                
                if (hit.collider.gameObject.tag.Equals("Mirror") || childernNameCheck(hit.collider.gameObject, "Mirror"))
                {
                    //Debug.Log("Hit Mirror");
                    reflectOnlyMirror = true;
                    bounces++;
                    line_renderer.positionCount++;
                    p0 = hit.point;
                    p1 = Vector3.Reflect(p1, hit.normal);//* Mathf.Cos(hit.collider.gameObject.transform.rotation.y);
                    line_renderer.SetPosition(i + 1, hit.point);
                    if (reflectOnlyMirror)
                    {
                        for (int j = i + 1; j <= bounces + 1; j++)
                        {
                            line_renderer.SetPosition(j, hit.point);
                        }
                    }
                    if(hit.collider.gameObject.GetComponent<Mirror>() != null)
                    {
                        raycreate = hit.collider.gameObject.GetComponent<Mirror>().NewCast(this.gameObject);
                    }
                }
                else if (hit.collider.gameObject.tag.Equals("Player") || childernNameCheck(hit.collider.gameObject, "Player") && killPlayer)
                {
                    //Debug.Log("Hit: Player");
                    /*
                     * if(hit.collider.gameObject.GetComponent<[Player_script]>() != null){
                     *     hit.collider.gameObject.GetComponent<[Player_script]>().dies;
                     * }
                     */
                      
                }
                else if(hit.transform.gameObject.tag.Equals("Ray") || childernNameCheck(hit.collider.gameObject, "Ray"))
                {

                }
                else if(hit.transform.gameObject.tag.Equals("Goal") || childernNameCheck(hit.collider.gameObject, "Goal"))
                {
                    Debug.Log("Play video");
                    Debug.Log("Door Open");
                    if(hit.collider.gameObject.GetComponent<Goal>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Goal>().CompleteGoal();
                        tmp_goal_object = hit.collider.gameObject;
                        tmp_goal_object.GetComponent<Goal>().isOpen = false;
                    }
                }
                
            }
            

        }
        ray = new Ray(p0, p1);
        if (Physics.Raycast(ray, out hit, power_range))
        {
            line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
            if (hit.transform.gameObject.tag.Equals("Goal") || childernNameCheck(hit.collider.gameObject, "Goal")){
                if (tmp_goal_object != null)
                {
                    if (tmp_goal_object.GetComponent<Goal>() != null)
                    {
                        tmp_goal_object.GetComponent<Goal>().isOpen = true;
                    }
                }
            }
            else
            {
                if (tmp_goal_object != null)
                {
                    if (tmp_goal_object.GetComponent<Goal>() != null)
                    {
                        tmp_goal_object.GetComponent<Goal>().isOpen = false;
                    }
                }
            }
        }
        else
        {
            line_renderer.SetPosition(line_renderer.positionCount - 1, power_range * p1);
            if(tmp_goal_object != null)
            {
                if(tmp_goal_object.GetComponent<Goal>() != null)
                {
                    tmp_goal_object.GetComponent<Goal>().isOpen = false;
                }
            }
        }
        for (int j = 0; j < lightPoint.Length; j++){
            Destroy(lightPoint[j]);
        }
        lightPoint = new GameObject[line_renderer.positionCount];
        for (int j = 0; j < lightPoint.Length; j++)
        {
            GameObject tmp_light = Instantiate(power_Light);
            tmp_light.transform.position = line_renderer.GetPosition(j);
            lightPoint[j] = tmp_light;
        }
        bounces =  0;
    }
    bool childernNameCheck(GameObject obj, string name)
    {
        if(obj.GetComponent<GameObject>() == null) { return false; }
        GameObject[] tmp_child = obj.GetComponentsInChildren<GameObject>();
        for(int i = 0; i <  tmp_child.Length; i++)
        {
            if (tmp_child[i].tag.Equals(name))
            {
                return true;
            }
        }
        return false;
    }
}
