using UnityEngine;


//Youtube: https://www.youtube.com/watch?v=iasDPyC0QOg&t=315s
public class WaterBounce : MonoBehaviour
{
    public float underWaterDrag = 3f; 

    public float underWaterAngularDrag = 1f;

    public float airDrag = 0f;

    public float airAngularDrag = 0.05f;

    public float floatingPower = 15f;

    public float waterHeight = 0f;

    Rigidbody m_Rigidbody;
    public GameObject water;
    public Vector3 start_position;
    public Quaternion start_rotation;
    bool underwater;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        
        start_position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(start_position.x, transform.position.y, start_position.z);
        transform.rotation = Quaternion.identity;
    }
    private void FixedUpdate()
    {
        float difference = transform.position.y - water.transform.position.y;

        if (difference < 0)
        {
            
            m_Rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), transform.position, ForceMode.Force);
            if(!underwater)
            {
                underwater = true;
                SwitchState(true);
            }
        }
        else if(underwater)
        {
            underwater = false;
            SwitchState(false);
        }
    }
    void SwitchState(bool underwater)
    {
        if(underwater)
        {
            m_Rigidbody.drag = underWaterDrag;
            m_Rigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            m_Rigidbody.drag = airDrag;
            m_Rigidbody.angularDrag = airAngularDrag;
        }
    }
}
