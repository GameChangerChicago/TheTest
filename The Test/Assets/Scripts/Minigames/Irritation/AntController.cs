using UnityEngine;
using System.Collections;

public class AntController : MonoBehaviour
{
    protected bool eating
    {
        get
        {
            return _eating;
        }
        set
        {
            if(value != _eating)
            {
                _myAnimator.SetBool("Eating", value);
                _eating = value;
            }
        }
    }
    private bool _eating;
    private Animator _myAnimator;
    private Transform _cookie;

    void Start()
    {
        _myAnimator = this.GetComponent<Animator>();
        _cookie = GameObject.Find("Cookie").transform;
    }

    void Update()
    {
        if(!eating)
        {
            MoveTowardCookie();
        }
    }

    private void MoveTowardCookie()
    {
        float goalAngle = Mathf.Rad2Deg * Mathf.Atan((this.transform.position.x - _cookie.position.x) / (_cookie.position.y - this.transform.position.y));

        if (goalAngle < 0)
        {
            goalAngle = 360 + goalAngle;
        }

        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, goalAngle));

        this.transform.Translate(Vector2.up * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Cookie")
        {
            eating = true;
        }
    }
}