using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public Transform followTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
        {
            Vector3 nextPosition = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
            transform.position = nextPosition;
        }
    }
}
