using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SubController : MonoBehaviour
{
    public SpriteRenderer CameraMask;
    public Text Timer;
    public float ForwardAccelerationRate,
                 LateralAccelerationRate,
                 MaxForwardSpeed,
                 MaxLateralSpeed,
                 LeftMoveDistance,
                 RightMoveDistance;
    public int TimerLength;
    private Rigidbody2D _myRigidbody,
                        _cameraRigidbody;

    public SubmarineStates currentSubmarineState
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
                        _currentXTarget = _initialPos.x - LeftMoveDistance;
                        break;
                    case SubmarineStates.MIDDLEROW:
                        _currentXTarget = _initialPos.x;
                        break;
                    case SubmarineStates.RIGHTROW:
                        _currentXTarget = _initialPos.x + RightMoveDistance;
                        break;
                    default:
                        Debug.LogWarning("The only thing I could think of that would cause this to happen is if we add more that three rows in the sub game.");
                        break;
                }
                _lastSubmarineState = _currentSubmarineState;
                _currentSubmarineState = value;
            }
        }
    }
    private SubmarineStates _currentSubmarineState = SubmarineStates.MIDDLEROW;
    private SubmarineStates _lastSubmarineState;
    private CollectablesManager _theCollectablesManager;
    private SpriteRenderer _myRenderer;
    private Vector2 _initialPos,
                    _deltaDistance,
                    _currentMoveDistance,
                    _lastPos;
    private float _currentXTarget,
                  _timer;
    private bool _moving,
                 _movingLateral;
    
    void Start()
    {
        //Setting inital x targets for left, right, and middle positions
        _initialPos = this.transform.position;

        _myRigidbody = GetComponent<Rigidbody2D>();
        _cameraRigidbody = Camera.main.GetComponent<Rigidbody2D>();
        _myRenderer = GetComponentInChildren<SpriteRenderer>();
        _theCollectablesManager = FindObjectOfType<CollectablesManager>();
        _moving = true;
    }
    
    void Update()
    {
        if (_moving)
        {
            MovementHandler();
            RotatationHandler();
            _timer += Time.deltaTime;
            int timerInt = (int)_timer;

            string timerString = (TimerLength - timerInt).ToString();

            if (TimerLength - timerInt > 0)
                Timer.text = timerString;
            else
            {
                Timer.text = "0";
                FadeOut();
            }
        }

        //Disabled for actual builds
        //if (Input.GetKeyDown(KeyCode.Mouse0) && !_moving)
        //{
        //    ToggleMovement();
        //}
    }

    private void MovementHandler()
    {
        if (_lastPos.y != 0)
        {
            _deltaDistance = new Vector2(this.transform.position.x - _lastPos.x, this.transform.position.y - _lastPos.y);
            _lastPos = this.transform.position;
        }
        else
            _lastPos = this.transform.position;

        if(_deltaDistance.y < MaxForwardSpeed)
        {
            _currentMoveDistance.y += ForwardAccelerationRate * Time.deltaTime;
        }

        this.transform.Translate(0, _currentMoveDistance.y, 0);
        Camera.main.transform.Translate(0, _currentMoveDistance.y, 0);

        //This manages the lateral movement of the submarine
        if (this.transform.position.x != _currentXTarget)
        {
            _movingLateral = true;

            if (this.transform.position.x > _currentXTarget + 0.5f)
            {
                if (_deltaDistance.x < MaxLateralSpeed)
                {
                    _currentMoveDistance.x += LateralAccelerationRate * Time.deltaTime;
                }

                this.transform.Translate(-_currentMoveDistance.x, 0, 0);
            }
            else if (this.transform.position.x < _currentXTarget - 0.5f)
            {
                if (_deltaDistance.x < 0.2)
                {
                    _currentMoveDistance.x += LateralAccelerationRate * Time.deltaTime;
                }

                this.transform.Translate(_currentMoveDistance.x, 0, 0);
            }
            else
            {
                _currentMoveDistance.x = 0;

                if (currentSubmarineState == SubmarineStates.LEFTROW)
                    this.transform.position = new Vector2(_initialPos.x - LeftMoveDistance, this.transform.position.y);
                if (currentSubmarineState == SubmarineStates.RIGHTROW)
                    this.transform.position = new Vector2(_initialPos.x + RightMoveDistance, this.transform.position.y);
                if (currentSubmarineState == SubmarineStates.MIDDLEROW)
                    this.transform.position = new Vector2(_initialPos.x, this.transform.position.y);

                _movingLateral = false;
            }
        }
    }

    private void RotatationHandler()
    {
        if (_deltaDistance.x > 0)
        {
            //Rotate Right
            if (_myRenderer.transform.localEulerAngles.z > 330f || _myRenderer.transform.localEulerAngles.z < 31)
                _myRenderer.transform.Rotate(new Vector3(0, 0, -5));
        }
        if (_deltaDistance.x < 0)
        {
            //Rotate Left
            if (_myRenderer.transform.localEulerAngles.z < 30f || _myRenderer.transform.localEulerAngles.z > 329f)
                _myRenderer.transform.Rotate(new Vector3(0, 0, 5));
        }
        if (_deltaDistance.x == 0)
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

        if(!_moving)
        {
            _myRigidbody.velocity = Vector2.zero;
            _cameraRigidbody.velocity = Vector2.zero;
        }
    }

    private void FadeOut()
    {
        if(GameManager.CurrentConvoIndex == 4)
        {
            GameManager.CurrentConvoIndex = 0;
        }
        
        if (CameraMask != null)
        {
            CameraMask.color = new Color(CameraMask.color.r, CameraMask.color.g, CameraMask.color.b, CameraMask.color.a + (2 * Time.deltaTime));
            if (CameraMask.color.a > 0.9f)
                SceneManager.LoadScene("Frame");
        }
        else
            SceneManager.LoadScene("Frame");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Obstacle")
        {
            currentSubmarineState = _lastSubmarineState;
        }

        if(col.tag == "Coin")
        {
            _theCollectablesManager.CoinCollected(col.gameObject);
        }

        if(col.tag == "Gem")
        {
            _theCollectablesManager.GemCollected(col.gameObject);
            _myRigidbody.velocity = Vector2.zero;
            _cameraRigidbody.velocity = Vector2.zero;
        }
    }
}