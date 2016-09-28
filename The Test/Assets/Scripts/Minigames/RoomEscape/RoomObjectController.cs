using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class RoomObjectController : MonoBehaviour
{
    public RoomObject ObjectToMove;
    public SpriteRenderer CameraMask;
    public Transform TopLeftCornerPos;
    public Vector2 CurrentGridCount;
    public bool GameOver;

    public CardinalDirections ObjectDragDirection
    {
        get
        {
            if(_horizontalLock)
            {
                if(_touchDelta.x > 0)
                {
                    return CardinalDirections.RIGHT;
                }
                else
                {
                    return CardinalDirections.LEFT;
                }
            }
            else if(_verticalLock)
            {
                if (_touchDelta.y > 0)
                {
                    return CardinalDirections.UP;
                }
                else
                {
                    return CardinalDirections.DOWN;
                }
            }
            else
            {
                Debug.LogWarning("Hmm, if neither lock has happened than you should be trying to get this property.");
                return CardinalDirections.DOWN;
            }
        }
    }
    private CardinalDirections _objectDragDirection;

    private List<RoomObject> _roomObjects = new List<RoomObject>();
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
            _theGameManager.FadeHandler(false, true, "TempFrame");//FadeOut();
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
        if (!_horizontalLock && !_verticalLock && Mathf.Abs(_touchDelta.x) > 0.5f)
        {
            _horizontalLock = true;
        }
        if (!_horizontalLock && !_verticalLock && Mathf.Abs(_touchDelta.y) > 0.5f)
        {
            _verticalLock = true;
        }
        
        float dragSpeed = 0;

        if (_horizontalLock)
        {
            dragSpeed = Mathf.Clamp(Time.deltaTime * (Vector2.Distance(new Vector2(ObjectToMove.transform.position.x, 0), new Vector2(_initialObjectPos.x + _touchDelta.x, 0)) * 15), 0.01f, 0.8f);

            if (ObjectToMove.transform.position.x > _initialObjectPos.x + _touchDelta.x)
            {
                dragSpeed *= -1;
            }

            ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x + dragSpeed, ObjectToMove.transform.position.y, ObjectToMove.transform.position.z);
        }
        if (_verticalLock)
        {
            dragSpeed = Mathf.Clamp(Time.deltaTime * (Vector2.Distance(new Vector2(0, ObjectToMove.transform.position.y), new Vector2(0, _initialObjectPos.y + _touchDelta.y)) * 15), 0.01f, 0.8f);

            if (ObjectToMove.transform.position.y > _initialObjectPos.y + _touchDelta.y)
            {
                dragSpeed *= -1;
            }

            ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x, ObjectToMove.transform.position.y + dragSpeed, ObjectToMove.transform.position.z);
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
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x, ObjectToMove.transform.position.y + (distanceToTravel * Time.deltaTime * 20), ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.DOWN:
                if (ObjectToMove.transform.position.y > targetLoc.y + 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x, ObjectToMove.transform.position.y - (distanceToTravel * Time.deltaTime * 20), ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.LEFT:
                if (ObjectToMove.transform.position.x > targetLoc.x + 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x - (distanceToTravel * Time.deltaTime * 20), ObjectToMove.transform.position.y, ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.RIGHT:
                if (ObjectToMove.transform.position.x < targetLoc.x - 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x + (distanceToTravel * Time.deltaTime * 20), ObjectToMove.transform.position.y, ObjectToMove.transform.position.z);
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

    public bool OverlapChecker(RoomObject objectBeingDragged)
    {
        if (_roomObjects.Count == 0)
            _roomObjects.AddRange(FindObjectsOfType<RoomObject>());

        _roomObjects.Remove(objectBeingDragged);
        List<Vector2> objectGridPos = objectBeingDragged.GridPositions;

        foreach (RoomObject ro in _roomObjects)
        {
            for (int i = 0; i < ro.GridPositions.Count; i++)
            {
                for (int j = 0; j < objectGridPos.Count; j++)
                {
                    if (objectGridPos[j] == ro.GridPositions[i])
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}