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
        InputManager();
    }

    private void InputManager()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            _initialTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _initialObjectPos = ObjectToMove.transform.position;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            DragHandler();
        }
        else if (_snappingToPosition)
        {
            SnapToPosition();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            _horizontalLock = false;
            _verticalLock = false;
            ObjectToMove.UpdatePosition();
            _snappingToPosition = true;
        }
    }

    private void DragHandler()
    {
        Vector2 currentTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _touchDelta = new Vector2(currentTouchPos.x - _initialTouchPos.x, currentTouchPos.y - _initialTouchPos.y);
        if(!_horizontalLock && !_verticalLock && _touchDelta.x > 0.5f)
        {
            _horizontalLock = true;
        }
        if(!_horizontalLock && !_verticalLock && _touchDelta.y > 0.5f)
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
        Debug.Log(targetLoc);
    }
}