using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Power : MonoBehaviour
{
    public GameObject start_position;
    public LineRenderer line_renderer;
    public int bounces = 2;
    bool reflectOnlyMirror = false;
    public float angle = 0;
    public bool killPlayer = true;
    public GameObject player;

    float power_range = 100;
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
        line_renderer.positionCount = 2 + bounces;
        line_renderer.SetPosition(0, start_position.transform.position);
        castRay(start_position.transform.position, transform.forward);
    }
    /*
     * A method to cast a ray from source.
     * p0 is the start position.
     * p1 is the direction
     */
    void castRay(Vector3 p0, Vector3 p1) 
    {
        line_renderer.SetPosition(0, start_position.transform.position);
        
        for (int i = 0; i < bounces+1; i++)
        {
            Ray ray = new Ray(p0, p1);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, power_range, 1))
            {
                //Debug.Log("Hit: " + hit.collider.gameObject.tag.Equals("Player"));
                if (hit.collider.gameObject.tag.Equals("Mirror") || childernNameCheck(hit.collider.gameObject, "Mirror"))
                {
                    Debug.Log("Hit Mirror");
                    reflectOnlyMirror = true;
                    bounces++;
                    line_renderer.positionCount++;
                    p0 = hit.point;
                    p1 = Vector3.Reflect(p1, hit.normal) * Mathf.Cos(hit.collider.gameObject.transform.rotation.y);
                    line_renderer.SetPosition(i + 1, hit.point);
                    if (reflectOnlyMirror)
                    {
                        for (int j = i + 1; j <= bounces + 1; j++)
                        {
                            line_renderer.SetPosition(j, hit.point);
                        }
                    }

                }
                else if (hit.collider.gameObject.tag.Equals("Player") || childernNameCheck(hit.collider.gameObject, "Player") && killPlayer)
                {
                    Debug.Log("Hit: Player");
                    /*
                     * if(hit.collider.gameObject.GetComponent<[Player_script]>() != null){
                     *     hit.collider.gameObject.GetComponent<[Player_script]>().dies;
                     * }
                     */
                }
                else if(hit.transform.gameObject.tag.Equals("Ray") || childernNameCheck(hit.collider.gameObject, "Ray"))
                {
                    Debug.Log("Hit Power");
                    p1 = hit.point;
                }
                
            }
            

        }
        line_renderer.SetPosition(line_renderer.positionCount - 1, power_range * p1);
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
