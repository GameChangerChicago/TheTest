using UnityEngine;
using System.Collections;

public class RoomObjectController : MonoBehaviour
{
    public RoomObject ObjectToMove;
    public Transform TopLeftCornerPos;
    public Vector2 CurrentGridCount;

    private Vector2 _initialTouchPos,
                    _initialObjectPos,
                    _touchDelta;
    private bool _horizontalLock,
                 _verticalLock,
                 _snappingToPosition;
    
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
    }

    private void InputManager()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            _initialTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(ObjectToMove);
            _initialObjectPos = ObjectToMove.transform.position;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            DragHandler();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            ObjectToMove.UpdatePosition();
            _snappingToPosition = true;
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
        CardinalDirections directionToSnap = CardinalDirections.RIGHT;
        if(_horizontalLock)
        {
            if (ObjectToMove.transform.position.x > targetLoc.x)
                directionToSnap = CardinalDirections.RIGHT;
            else
                directionToSnap = CardinalDirections.LEFT;
                
            _horizontalLock = false;
        }
        if(_verticalLock)
        {
            if (ObjectToMove.transform.position.y > targetLoc.y)
                directionToSnap = CardinalDirections.DOWN;
            else
                directionToSnap = CardinalDirections.UP;

            _verticalLock = false;
        }

        float distanceToTravel = Vector2.Distance(ObjectToMove.transform.position, targetLoc);
        distanceToTravel = Mathf.Clamp(distanceToTravel, 0.001f, 0.01f);
        Debug.Log((distanceToTravel * Time.deltaTime));
        switch (directionToSnap)
        {
            case CardinalDirections.UP:
                if (ObjectToMove.transform.position.y < targetLoc.y - 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x, ObjectToMove.transform.position.y + (distanceToTravel * Time.deltaTime), ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.DOWN:
                if (ObjectToMove.transform.position.y > targetLoc.y + 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x, ObjectToMove.transform.position.y - (distanceToTravel * Time.deltaTime), ObjectToMove.transform.position.z);
                else
                {
                    this.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.LEFT:
                if (ObjectToMove.transform.position.x > targetLoc.x + 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x + (distanceToTravel * Time.deltaTime), ObjectToMove.transform.position.y, ObjectToMove.transform.position.z);
                else
                {
                    ObjectToMove.transform.position = targetLoc;
                    _snappingToPosition = false;
                    ObjectToMove = null;
                }
                break;
            case CardinalDirections.RIGHT:
                //Debug.Log("this: " + transform.position.x + " < target: " + (targetLoc.x - 0.1f));
                if (ObjectToMove.transform.position.x < targetLoc.x - 0.1f)
                    ObjectToMove.transform.position = new Vector3(ObjectToMove.transform.position.x - (distanceToTravel * Time.deltaTime), ObjectToMove.transform.position.y, ObjectToMove.transform.position.z);
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
}