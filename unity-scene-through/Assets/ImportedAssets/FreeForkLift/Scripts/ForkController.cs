using UnityEngine;
using System.Collections;

public class ForkController : MonoBehaviour {

    public Transform fork; 
    public Transform mast;
    public float speedTranslate; //Platform travel speed
    public Vector3 maxY; //The maximum height of the platform
    //public Vector3 maxYBox;
    public Vector3 minY; //The minimum height of the platform
    public Vector3 maxYmast; //The maximum height of the mast
    public Vector3 minYmast; //The minimum height of the mast
    public GameObject player;
    public GameObject box;
    public AudioSource audio;

    private bool mastMoveTrue = false; //Activate or deactivate the movement of the mast
    private bool moveBox = false; // Stop box from moving
    private bool goingUp = false; // Stop forklift from going up

    private float yInitialPosition; // Box initial y position

    void Start()
    {
        yInitialPosition = box.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate () { 

        //Debug.Log(mastMoveTrue);
        if(fork.transform.localPosition.y >= maxYmast.y && fork.transform.localPosition.y < maxY.y)
        {
            mastMoveTrue = true;
        }
        else
        {
            mastMoveTrue = false;

        }

        /*if((player.transform.position - transform.position).sqrMagnitude <= 50f && goingDown == false)
        {
            goingUp = true;
        }*/

        if (fork.transform.localPosition.y <= maxYmast.y)
        {
            mastMoveTrue = false;
        }

        /*if(range == true)
        {
            Debug.Log("Why not working");
            minY.y -= 10f;
            fork.transform.localPosition = Vector3.MoveTowards(fork.transform.localPosition, minY, speedTranslate * Time.deltaTime);
            fork.transform.localPosition = Vector3.MoveTowards(fork.transform.localPosition, maxY, speedTranslate * Time.deltaTime);
            if (mastMoveTrue)
            {
                mast.transform.localPosition = Vector3.MoveTowards(mast.transform.localPosition, maxYmast, speedTranslate * Time.deltaTime);
            }
        }*/

        if ((player.transform.position - transform.position).sqrMagnitude <= 5f)
        {
            goingUp = true;

        }

        if(goingUp == true)
        {
            fork.transform.localPosition = Vector3.MoveTowards(fork.transform.localPosition, maxY, speedTranslate * Time.deltaTime);
            if (mastMoveTrue)
            {
                mast.transform.localPosition = Vector3.MoveTowards(mast.transform.localPosition, maxYmast, speedTranslate * Time.deltaTime);
                audio.Play();
            }
            if (moveBox == false)
            {
                box.transform.Translate(Vector3.up * speedTranslate * Time.deltaTime);
                //audio.Play();
            }
            if (box.transform.position.y >= maxY.y + yInitialPosition)
            {
                moveBox = true;
                //audio.Pause();
            }
        }

        /*if (Input.GetKey(KeyCode.PageUp))
        {
           //fork.Translate(Vector3.up * speedTranslate * Time.deltaTime);
            fork.transform.localPosition = Vector3.MoveTowards(fork.transform.localPosition, maxY, speedTranslate * Time.deltaTime);
            if(mastMoveTrue)
            {
                mast.transform.localPosition = Vector3.MoveTowards(mast.transform.localPosition, maxYmast, speedTranslate * Time.deltaTime);
            }
          
        }*/
        /*if (Input.GetKey(KeyCode.PageDown))
        {
            fork.transform.localPosition = Vector3.MoveTowards(fork.transform.localPosition, minY, speedTranslate * Time.deltaTime);

            if (mastMoveTrue)
            {
                mast.transform.localPosition = Vector3.MoveTowards(mast.transform.localPosition, minYmast, speedTranslate * Time.deltaTime);

            }

        }*/

    }
}
