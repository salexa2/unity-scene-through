using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{

    public Camera SecCamera;
    public float DetectionHalfAngle = 30f;
    public float DetectionRange = 20f;
    public SphereCollider DetectionTrigger;
    public Light DetectionLight;
    public Color Color_NothingDetected = Color.green;
    public Color Color_FullyDetected = Color.red;
    float DetectionBuildRate = 0.5f;
    float DetectionDecayRate = 0.5f;
    public List<string> DetectableTag;
    public LayerMask DetectionLayerMask = ~0;
    public GameObject CurrentlyDetectedTarget { get; private set; }

    private float CosDetectionHalfAngle;

    class PotentialTarget
    {
        public GameObject LinkedGO;
        public bool InFOV;
        public float DetectionLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setting up collider and lights
        DetectionLight.color = Color_NothingDetected;
        DetectionLight.range = DetectionRange;
        DetectionLight.spotAngle = DetectionHalfAngle * 2f;
        DetectionTrigger.radius = DetectionRange;

        // cache detection data
        CosDetectionHalfAngle = Mathf.Cos(Mathf.Deg2Rad * DetectionHalfAngle);
    }

    // Update is called once per frame
    void Update()
    {
        RefreshTargetInfo();

    }

    Dictionary<GameObject, PotentialTarget> AllTargets = new Dictionary<GameObject, PotentialTarget>();

    void RefreshTargetInfo()
    {

        float highestDetectionLevel = 0f;
        CurrentlyDetectedTarget = null;

        foreach (var target in AllTargets)
        {
            var targetInfo = target.Value;

            bool isVisible = false;

            // It is target in field of view
            Vector3 vecToTarget = targetInfo.LinkedGO.transform.position - transform.position;
            //Vector3 vecToTarget = targetInfo.LinkedGO.transform.position - SecCamera.transform.position;
            /*if (Vector3.Dot(SecCamera.transform.forward, vecToTarget.normalized) >= CosDetectionHalfAngle)
            {
                // Check If We Can See Target
                RaycastHit hitInfo;
                if (Physics.Raycast(SecCamera.transform.position, SecCamera.transform.forward, out hitInfo, DetectionRange, DetectionLayerMask, QueryTriggerInteraction.Ignore))
                {
                    if(hitInfo.collider.gameObject == targetInfo.LinkedGO)
                    {
                        isVisible = true;
                    }
                }
            }*/

            if (Vector3.Dot(transform.forward, vecToTarget.normalized) >= CosDetectionHalfAngle)
            {
                // Check If We Can See Target
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, DetectionRange, DetectionLayerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider.gameObject == targetInfo.LinkedGO)
                    {
                        isVisible = true;
                    }
                }
            }

            targetInfo.InFOV = isVisible;
            if (isVisible)
            {
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionBuildRate * Time.deltaTime);
            } else
            {
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionDecayRate * Time.deltaTime);
            }
            // Found a new more Detected Target?
            if(targetInfo.DetectionLevel > highestDetectionLevel)
            {
                highestDetectionLevel = targetInfo.DetectionLevel;
                CurrentlyDetectedTarget = targetInfo.LinkedGO;
                Debug.Log("Currently Detected Target is: " + CurrentlyDetectedTarget);
            }
        }
        // update light color
        if(CurrentlyDetectedTarget != null)
        {
            DetectionLight.color = Color.Lerp(Color_NothingDetected, Color_FullyDetected, highestDetectionLevel);
        } else
        {
            DetectionLight.color = Color_NothingDetected;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Skip if not supported
        if (!DetectableTag.Contains(other.tag))
        {
            return;
        }
        // ADD TO TARGET LIST
        AllTargets[other.gameObject] = new PotentialTarget() { LinkedGO = other.gameObject };
    }

    void OnTriggerExit(Collider other)
    {
        if (!DetectableTag.Contains(other.tag))
        {
            return;
        }
        // Remove from target list
        AllTargets.Remove(other.gameObject);
    }
}
