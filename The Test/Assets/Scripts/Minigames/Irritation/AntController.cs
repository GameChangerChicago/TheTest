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
    private CookieManager _theCookieManager;
    private Transform _cookie;
    private bool _dragging;

    void Start()
    {
        _myAnimator = this.GetComponent<Animator>();
        _theCookieManager = FindObjectOfType<CookieManager>();
        _cookie = _theCookieManager.transform;
    }

    void Update()
    {
        eating = Physics2D.OverlapPoint(this.transform.position, 512);

        if(_dragging)
        {
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = new Vector3(currentPosition.x, currentPosition.y, this.transform.position.z);
        }
        else if(!eating)
        {
            MoveTowardCookie();
        }
        else
        {
            EatingHandler();
        }
    }

    private void MoveTowardCookie()
    {
        float goalAngle = Mathf.Rad2Deg * Mathf.Atan((this.transform.position.x - _cookie.position.x) / (_cookie.position.y - this.transform.position.y));

        if (_cookie.position.y < this.transform.position.y)
        {
            if (goalAngle > 180)
            {
                goalAngle -= 180;
            }
            else
            {
                goalAngle += 180;
            }
        }

        if (goalAngle < 0)
        {
            goalAngle = 360 + goalAngle;
        }

        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, goalAngle));

        this.transform.Translate(Vector2.up * Time.deltaTime);
    }

    private void EatingHandler()
    {
        _theCookieManager.PercentEaten += Time.deltaTime;
    }

    void OnMouseDown()
    {
        _dragging = true;
    }

    void OnMouseUp()
    {
        _dragging = false;
    }
}