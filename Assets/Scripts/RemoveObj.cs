using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObj : MonoBehaviour
{

    [SerializeField] Transform orientation;
    [SerializeField] LayerMask TargetMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, orientation.forward, out hit, 100, TargetMask))
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
