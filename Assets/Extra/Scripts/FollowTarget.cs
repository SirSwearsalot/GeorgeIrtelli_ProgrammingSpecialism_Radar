using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform TargetPos;

    // Update is called once per frame
    void Update()
    {
            transform.position = TargetPos.position;
    }
}
