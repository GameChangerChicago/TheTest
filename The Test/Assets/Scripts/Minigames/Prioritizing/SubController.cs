using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SubController : MonoBehaviour
{
    public BoxCollider2D[] TouchColliders;
    public SpriteRenderer CameraMask;
    public Text Timer;
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
                _lastSubmarineState = _currentSubmarineState;
                _currentSubmarineState = value;
            }
        }
    }
    private SubmarineStates _currentSubmarineState = SubmarineStates.MIDDLEROW;
    private SubmarineStates _lastSubmarineState;
    private CollectablesManager _theCollectablesManager;
    private SpriteRenderer _myRenderer;
    private Vector2 _initialPos;
    private float _currentXTarget,
                  _timer;
    private bool _moving;
    
    void Start()
    {
        //Setting inital x targets for left, right, and middle positions
        _initialPos = this.transform.position;

        _myRigidbody = GetComponent<Rigidbody2D>();
        _cameraRigidbody = Camera.main.GetComponent<Rigidbody2D>();
        _myRenderer = GetComponentInChildren<SpriteRenderer>();
        _theCollectablesManager = FindObjectOfType<CollectablesManager>();
    }
    
    void Update()
    {
        if (_moving)
        {
            MovementHandler();
            RotatationHandler();
            _timer += Time.deltaTime;
            int timerInt = (int)_timer;

            string timerString = (5 - timerInt).ToString();

            if (5 - timerInt > 0)
                Timer.text = timerString;
            else
            {
                Timer.text = "0";
                FadeOut();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !_moving)
        {
            ToggleMovement();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            TouchHandler();
        }
    }

    private void TouchHandler()
    {
        for(int i = 0; i < TouchColliders.Length; i++)
        {
            if(TouchColliders[i].OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                switch (i)
                {
                    case 0:
                        currentSubmarineState = SubmarineStates.LEFTROW;
                        break;
                    case 1:
                        currentSubmarineState = SubmarineStates.MIDDLEROW;
                        break;
                    case 2:
                        currentSubmarineState = SubmarineStates.RIGHTROW;
                        break;
                    default:
                        break;
                }
            }
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

        if(!_moving)
        {
            _myRigidbody.velocity = Vector2.zero;
            _cameraRigidbody.velocity = Vector2.zero;
        }
    }

    private void FadeOut()
    {
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