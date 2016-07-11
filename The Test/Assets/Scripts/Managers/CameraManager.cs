using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public Transform TargetToFollow;

    void Update()
    {
        if(TargetToFollow)
        {
            MatchTargetMovement();
        }
    }

    private void MatchTargetMovement()
    {
        this.transform.position = new Vector3(TargetToFollow.position.x, TargetToFollow.position.y + 1.9f, this.transform.position.z);
    }
}