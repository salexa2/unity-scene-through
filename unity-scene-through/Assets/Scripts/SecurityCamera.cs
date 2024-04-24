using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecurityCamera : MonoBehaviour
{

    public Camera SecCamera;

    [SerializeField] Transform PivotPoint;
    [SerializeField] float DefaultPitch = 20f;
    [SerializeField] float AngleSwept = 60f;
    [SerializeField] float SweepSpeed = 6f;
    [SerializeField] float MaxRotationSpeed = 15f;

    //public Collider playerCollider;
    public float DetectionHalfAngle = 30f;
    public float DetectionRange = 6.5f;
    [SerializeField] float TargetVOffset = 1f;
    public SphereCollider DetectionTrigger;
    public Light DetectionLight;
    public Color Color_NothingDetected = Color.green;
    public Color Color_FullyDetected = Color.red;
    public float DetectionBuildRate = 0.5f;
    public float DetectionDecayRate = 0.5f;
    [SerializeField][Range(0f, 1f)] float SuspicionThreshold = 0.5f;
    public List<string> DetectableTag;
    public LayerMask DetectionLayerMask = ~0;
    public GameObject CurrentlyDetectedTarget { get; private set; }
    public bool HasDetectedTarget { get; private set; } = false;

    [SerializeField] UnityEvent<GameObject> OnDetected = new UnityEvent<GameObject>();

    float CurrentAngle = 0f;
    private float CosDetectionHalfAngle;
    bool SweepClockwise = true;

    class PotentialTarget
    {
        public GameObject LinkedGO;
        public bool InFOV;
        public float DetectionLevel;
        public bool OnDetectedEventSent;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setting up collider and lights
        DetectionLight.color = Color_NothingDetected;
        DetectionLight.range = DetectionRange;
        DetectionLight.spotAngle = DetectionHalfAngle * 2f;
        //DetectionTrigger.radius = 1f;

        // cache detection data
        CosDetectionHalfAngle = Mathf.Cos(Mathf.Deg2Rad * DetectionHalfAngle);
    }

    // Update is called once per frame
    void Update()
    {

        RefreshTargetInfo();

        Quaternion desiredRotation = PivotPoint.transform.rotation;

        // Don't auto-rotate if player in FOV
        if (CurrentlyDetectedTarget != null && AllTargets[CurrentlyDetectedTarget].DetectionLevel >= SuspicionThreshold)
        {
            if (AllTargets[CurrentlyDetectedTarget].InFOV)
            {
                var vecToTarget = (CurrentlyDetectedTarget.transform.position + TargetVOffset * Vector3.up - PivotPoint.transform.position).normalized;
                desiredRotation = Quaternion.LookRotation(vecToTarget, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);
            }
        }
        else
        {
            //Update Angle
            CurrentAngle += SweepSpeed * Time.deltaTime * (SweepClockwise ? 1f : -1f);
            if (Mathf.Abs(CurrentAngle) >= (AngleSwept * 0.5f))
            {
                SweepClockwise = !SweepClockwise;
            }

            // Calculate Rotation
            desiredRotation = PivotPoint.transform.parent.rotation * Quaternion.Euler(0f, CurrentAngle, DefaultPitch);
        }

        PivotPoint.transform.rotation = Quaternion.RotateTowards(PivotPoint.transform.rotation, desiredRotation, MaxRotationSpeed * Time.deltaTime);

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

            // Is target in field of view
            Vector3 vecToTarget = (targetInfo.LinkedGO.transform.position + TargetVOffset * Vector3.up - transform.position).normalized;
            //Vector3 vecToTarget = targetInfo.LinkedGO.transform.position - SecCamera.transform.position;

            if (Vector3.Dot(transform.forward, vecToTarget) >= CosDetectionHalfAngle)
            {
                // Check If We Can See Target
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, vecToTarget, out hitInfo, DetectionRange, DetectionLayerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider.gameObject == targetInfo.LinkedGO)
                    {
                        isVisible = true;
                    }
                }
            }
            targetInfo.InFOV = isVisible;
            //Debug.Log(targetInfo.InFOV);
            //Debug.Log("Hello");
            if (isVisible)
            {
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionBuildRate * Time.deltaTime);

                // notify target was seen
                Debug.Log(targetInfo.DetectionLevel);
                if (targetInfo.DetectionLevel >= 2f && !targetInfo.OnDetectedEventSent)
                {
                    HasDetectedTarget = true;
                    targetInfo.OnDetectedEventSent = true;
                    OnDetected.Invoke(targetInfo.LinkedGO);
                }
            }
            else
            {
                targetInfo.DetectionLevel = Mathf.Clamp01(targetInfo.DetectionLevel + DetectionDecayRate * Time.deltaTime);
            }
            /*if (targetInfo.DetectionLevel < 0.55f) // CHECK
            {
                Debug.Log(targetInfo.DetectionLevel);
                //Debug.Log(targetInfo.OnDetectedEventSent);
            }*/
            if (targetInfo.DetectionLevel >= 0.25f && !targetInfo.OnDetectedEventSent)
            {
                //Debug.Log("Hello");
                HasDetectedTarget = true;
                targetInfo.OnDetectedEventSent = true;
                OnDetected.Invoke(targetInfo.LinkedGO);
            }
            // Found a new more Detected Target?
            if(targetInfo.DetectionLevel > highestDetectionLevel)
            {
                highestDetectionLevel = targetInfo.DetectionLevel;
                CurrentlyDetectedTarget = targetInfo.LinkedGO;
                //Debug.Log("Currently Detected Target is: " + CurrentlyDetectedTarget);
            }
        }
        // update light color
        if(CurrentlyDetectedTarget != null)
        {
            DetectionLight.color = Color.Lerp(Color_NothingDetected, Color_FullyDetected, highestDetectionLevel);
        } else
        {
            DetectionLight.color = Color_NothingDetected;
            if (HasDetectedTarget)
            {
                HasDetectedTarget = false;
            }
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
