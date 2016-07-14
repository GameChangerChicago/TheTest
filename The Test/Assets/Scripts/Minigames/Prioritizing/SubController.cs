﻿using UnityEngine;
using System.Collections;

public class SubController : MonoBehaviour
{           
    public float ForwardAcceleration,
                 LateralAcceleration,
                 MaxForwardSpeed,
                 MaxLateralSpeed;
    private Rigidbody2D _myRigidbody,
                        _cameraRigidbody;

    protected SubmarineStates currentSubmarineState
    {
        get
        {
            return _currentSubmarineState;
        }
        set
        {
            if (value != _currentSubmarineState)
            {
                switch (value)
                {
                    case SubmarineStates.LEFTROW:
                        _currentXTarget = _initialPos.x - 3.25f;
                        break;
                    case SubmarineStates.MIDDLEROW:
                        _currentXTarget = _initialPos.x;
                        break;
                    case SubmarineStates.RIGHTROW:
                        _currentXTarget = _initialPos.x + 3.25f;
                        break;
                    default:
                        Debug.LogWarning("The only thing I could think of that would cause this to happen is if we add more that three rows in the sub game.");
                        break;
                }
                _currentSubmarineState = value;
            }
        }
    }
    private SubmarineStates _currentSubmarineState = SubmarineStates.MIDDLEROW;
    private SpriteRenderer _myRenderer;
    private Vector2 _initialPos;
    private float _currentXTarget;
    private bool _moving;
    
    void Start()
    {
        //Setting inital x targets for left, right, and middle positions
        _initialPos = this.transform.position;

        _myRigidbody = GetComponent<Rigidbody2D>();
        _cameraRigidbody = Camera.main.GetComponent<Rigidbody2D>();
        _myRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        if (_moving)
        {
            MovementHandler();
            RotatationHandler();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMovement();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSubmarineState = SubmarineStates.LEFTROW;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSubmarineState = SubmarineStates.MIDDLEROW;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSubmarineState = SubmarineStates.RIGHTROW;
        }

    }

    private void MovementHandler()
    {
        //This keeps the constant forward speed of the sub
        if (_myRigidbody.velocity.y < MaxForwardSpeed)
        {
            _myRigidbody.AddForce(new Vector2(0, ForwardAcceleration));
            _cameraRigidbody.AddForce(new Vector2(0, ForwardAcceleration));
        }

        //This manages the lateral movement of the submarine
        if (this.transform.position.x > _currentXTarget + 0.1f)
        {
            if (_myRigidbody.velocity.x > -MaxLateralSpeed)
                _myRigidbody.AddForce(new Vector2(-LateralAcceleration, 0));
        }
        else if(this.transform.position.x < _currentXTarget - 0.1f)
        {
            if (_myRigidbody.velocity.x < MaxLateralSpeed)
                _myRigidbody.AddForce(new Vector2(LateralAcceleration, 0));
        }
        else
        {
            _myRigidbody.velocity = new Vector2(0, _myRigidbody.velocity.y);
            _cameraRigidbody.velocity = new Vector2(0, _myRigidbody.velocity.y);

            if (currentSubmarineState == SubmarineStates.LEFTROW)
                this.transform.position = new Vector2(_initialPos.x - 3.25f, this.transform.position.y);
            if (currentSubmarineState == SubmarineStates.RIGHTROW)
                this.transform.position = new Vector2(_initialPos.x + 3.25f, this.transform.position.y);
            if (currentSubmarineState == SubmarineStates.MIDDLEROW)
                this.transform.position = new Vector2(_initialPos.x, this.transform.position.y);
        }
    }

    private void RotatationHandler()
    {
        Debug.Log(_myRenderer.transform.localEulerAngles.z);
        
        if (_myRigidbody.velocity.x > 0)
        {
            //Rotate Right
            if (_myRenderer.transform.localEulerAngles.z > 330f || _myRenderer.transform.localEulerAngles.z < 31)
                _myRenderer.transform.Rotate(new Vector3(0, 0, -5));
        }
        if (_myRigidbody.velocity.x < 0)
        {
            //Rotate Left
            if (_myRenderer.transform.localEulerAngles.z < 30f || _myRenderer.transform.localEulerAngles.z > 329f)
                _myRenderer.transform.Rotate(new Vector3(0, 0, 5));
        }
        if (_myRigidbody.velocity.x == 0)
        {
            //Even out
            if (_myRenderer.transform.localEulerAngles.z > 300 && _myRenderer.transform.localEulerAngles.z < 359)
                _myRenderer.transform.Rotate(new Vector3(0, 0, 5));
            else if (_myRenderer.transform.localEulerAngles.z < 100 && _myRenderer.transform.localEulerAngles.z > 1)
                _myRenderer.transform.Rotate(new Vector3(0, 0, -5));
            else
                _myRenderer.transform.localRotation = Quaternion.identity;
        }
    }

    public void ToggleMovement()
    {
        _moving = !_moving;
    }

    private void CollectaleHandler(Animator theAnimator, bool isCoin)
    {
        //theAnimator.SetBool("Animate", true);

        if (isCoin)
        {
            
        }
        else
        {
            
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Obstacle")
        {
            if(_myRigidbody.velocity.x > 0)
            {
                if (currentSubmarineState == SubmarineStates.MIDDLEROW)
                    currentSubmarineState = SubmarineStates.LEFTROW;
                if (currentSubmarineState == SubmarineStates.RIGHTROW)
                    currentSubmarineState = SubmarineStates.MIDDLEROW;
            }
            else if(_myRigidbody.velocity.x < 0)
            {
                if (currentSubmarineState == SubmarineStates.MIDDLEROW)
                    currentSubmarineState = SubmarineStates.RIGHTROW;
                if (currentSubmarineState == SubmarineStates.LEFTROW)
                    currentSubmarineState = SubmarineStates.MIDDLEROW;
            }
            else
            {
                currentSubmarineState = SubmarineStates.MIDDLEROW;
            }
        }

        if(col.tag == "Coin")
        {
            CollectaleHandler(col.GetComponent<Animator>(), true);
        }

        if(col.tag == "Gem")
        {
            CollectaleHandler(col.GetComponent<Animator>(), false);
        }
    }
}