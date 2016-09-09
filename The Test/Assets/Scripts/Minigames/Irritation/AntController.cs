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
    protected bool dragging
    {
        get
        {
            return _dragging;
        }
        set
        {
            if (value != _dragging)
            {
                if (value)
                {
                    _myRigidBody.drag = 0;
                }
                else
                {
                    _myRigidBody.AddForce(_deltaPosition);
                }

                _dragging = value;
            }
        }
    }
    private bool _dragging;

    private GameManager _theGameManager;
    private Animator _myAnimator;
    private Rigidbody2D _myRigidBody;
    private CookieManager _theCookieManager;
    private Transform _cookie;
    private Vector2 _lastPosition,
                    _deltaPosition;
    private bool _instructionsDone;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
        _myAnimator = this.GetComponent<Animator>();
        _myRigidBody = this.GetComponent<Rigidbody2D>();
        _theCookieManager = FindObjectOfType<CookieManager>();
        _cookie = _theCookieManager.transform;
    }

    void Update()
    {
        eating = Physics2D.OverlapPoint(this.transform.position, LayerMask.GetMask("Cookie"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            dragging = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Ant")) == this.GetComponent<Collider2D>();
            _instructionsDone = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            dragging = false;
        }

        if(!dragging && _myRigidBody.drag < 5)
        {
            _myRigidBody.drag += Time.deltaTime * 5;
        }

        if (_theGameManager.GameOn)
        {
            if (dragging)
            {
                Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.transform.position = new Vector3(currentPosition.x, currentPosition.y, this.transform.position.z);
                _deltaPosition = new Vector2((currentPosition.x - _lastPosition.x) * Time.deltaTime, (currentPosition.y - _lastPosition.y) * Time.deltaTime);
                _lastPosition = currentPosition;
            }
            else if (!eating)
            {
                MoveTowardCookie();
            }
            else
            {
                EatingHandler();
            }
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

    //void OnMouseDown()
    //{
    //    _dragging = true;
    //}

    //void OnMouseUp()
    //{
    //    _dragging = false;
    //}
}