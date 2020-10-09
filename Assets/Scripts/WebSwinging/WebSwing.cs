using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WebSwing : MonoBehaviour
{
    public float range = 100f;
    public float forceMag = 100f;
    public ForceMode forceMode;
    public LayerMask mask;
    public QueryTriggerInteraction query;

    private Transform anchorTransform;

    private bool ropeAttached=false;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ropeAttached)
        {
            Vector3 dir = (anchorTransform.position - gameObject.transform.position).normalized;

            rb.AddForce(dir * forceMag, forceMode);
        }
    }

    public void FireWeb(Vector3 origin , Vector3 direction)
    {
        Ray ray = new Ray(origin, direction);

        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, range,mask,query))
        {
            anchorTransform = new GameObject("Anchor").transform;
            anchorTransform.position = hitInfo.point;
            anchorTransform.parent = hitInfo.transform;

            ropeAttached = true;
        }
    }

    public void DetachWeb()
    {
        Destroy(this.anchorTransform.gameObject);
        ropeAttached = false;
    }

    //void Swing()
    //{
    //    rb.velocity = rb.velocity + Physics.gravity * Time.deltaTime;
    //    rb.position = rb.position + rb.velocity * Time.deltaTime;
    //}
}
