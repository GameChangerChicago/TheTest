using UnityEngine;
using System.Collections;

public class AntController : MonoBehaviour
{
    private Transform Cookie;
    private bool _eating;

    void Start()
    {
        Cookie = GameObject.Find("Cookie").transform;
    }

    void Update()
    {
        if(!_eating)
        {
            MoveTowardCookie();
        }
    }

    private void MoveTowardCookie()
    {
        float goalAngle = Mathf.Atan((Cookie.position.y - this.transform.position.y) / (Cookie.position.x - this.transform.position.x));

        

        this.transform.Translate(Vector2.up * Time.deltaTime);
    }
}