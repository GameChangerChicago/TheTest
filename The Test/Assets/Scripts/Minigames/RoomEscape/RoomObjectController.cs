using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoomObjectController : MonoBehaviour
{
    public RoomObject ObjectToMove;
    public SpriteRenderer CameraMask;
    public Transform TopLeftCornerPos;
    public Vector2 CurrentGridCount;
    public bool GameOver;

    private GameManager _theGameManager;
    private Vector2 _initialTouchPos,
                    _initialObjectPos,
                    _touchDelta;
    private CardinalDirections _directionToSnap = CardinalDirections.RIGHT;
    private bool _horizontalLock,
                 _verticalLock,
                 _snappingToPosition;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (_snappingToPosition)
        {
            SnapToPosition();
        }
        else if (ObjectToMove)
        {
            InputManager();
        }

        //ASHLYN: This method ends the game when you collect the bone.
        if (GameOver)
        {
            _theGameManager.FadeHandler(false, "TempFrame");//FadeOut();
        }
    }

    private void InputManager()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            _initialTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _initialObjectPos = ObjectToMove.transform.position;
            if (ObjectToMove.PlayerObject)
                ObjectToMove.GetComponent<Animator>().SetBool("Walking", true);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            DragHandler();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            LoseControl();

            if (ObjectToMove.PlayerObject)
                ObjectToMove.GetComponent<Animator>().SetBool("Walking", false);
        }
    }

    private void DragHandler()
    {
        Vector2 currentTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _touchDelta = new Vector2(currentTouchPos.x - _initialTouchPos.x, currentTouchPos.y - _initialTouchPos.y);
        if(!_horizontalLock && !_verticalLock && Mathf.Abs(_touchDelta.x) > 0.5f)
        {
            _horizontalLock = true;
        }
        if(!_horizontalLock && !_verticalLock && Mathf.Abs(_touchDelta.y) > 0.5f)
        {
            _verticalLock = true;
        }

        if(_horizontalLock)
        {
            ObjectToMove.transform.position = new Vector3(_initialObjectPos.x + _touchDelta.x, _initialObjectPos.y, ObjectToMove.transform.position.z);
        }
        if(_verticalLock)
        {
            ObjectToMove.transform.position = new Vector3(_initialObjectPos.x, _initialObjectPos.y + _touchDelta.y, ObjectToMove.transform.position.z);
        }
    }

    private void SnapToPosition()
    {
        Vector2 targetLoc = ObjectToMove.GetTargetPos();

        if(_horizontalLock)
        {
            if (ObjectToMove.transform.position.x < targetLoc.x)
                _directionToSnap = CardinalDirections.RIGHT;
            else
                _directionToSnap = CardinalDirections.LEFT;
                
            _horizontalLock = false;
        }
        if(_verticalLock)
        {
            if (ObjectToMove.transform.position.y > targetLoc.y)
                _directionToSnap = CardinalDirections.DOWN;
            else
                _directionToSnap = CardinalDirections.UP;

            _verticalLock = false;
        }

        float distanceToTravel = Vector2.Distance(ObjectToMove.transform.position, targetLoc);

        switch (_directionToSnap)
        {
            case CardinalDirections.UP:
                if (ObjectToMove.transform.position.y < targetLoc.y - 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x, ObjectToMove.transform.position.y + (distanceToTravel * Time.deltaTime * 10), ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.DOWN:
                if (ObjectToMove.transform.position.y > targetLoc.y + 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x, ObjectToMove.transform.position.y - (distanceToTravel * Time.deltaTime * 10), ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.LEFT:
                if (ObjectToMove.transform.position.x > targetLoc.x + 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x - (distanceToTravel * Time.deltaTime * 10), ObjectToMove.transform.position.y, ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.RIGHT:
                if (ObjectToMove.transform.position.x < targetLoc.x - 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x + (distanceToTravel * Time.deltaTime * 10), ObjectToMove.transform.position.y, ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            default:
                Debug.Log("Did you add diagonals?");
                break;
        }
    }

    public void LoseControl()
    {
        if (ObjectToMove)
            ObjectToMove.UpdatePosition();

        _snappingToPosition = true;

        if (ObjectToMove.PlayerObject)
            ObjectToMove.GetComponent<Animator>().SetBool("Walking", false);
    }

    //HACK: THIS SHOULD BE HANDLED BY THE GAMEMANAGER!!!
    private void FadeOut()
    {
        if (CameraMask != null)
        {
            CameraMask.color = new Color(CameraMask.color.r, CameraMask.color.g, CameraMask.color.b, CameraMask.color.a + (2 * Time.deltaTime));
            if (CameraMask.color.a > 0.9f)
                SceneManager.LoadScene("TempFrame");
        }
        else
            SceneManager.LoadScene("TempFrame");
    }
}