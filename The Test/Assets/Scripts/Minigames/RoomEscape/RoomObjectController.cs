using UnityEngine;
using System.Collections;

public class RoomObjectController : MonoBehaviour
{
    public GameObject ObjectToMove;

    private Vector2 _initialTouchPos,
                    _initialObjectPos,
                    _touchDelta;
    private bool _horizontalLock,
                 _verticalLock;

    void Start()
    {

    }
    
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
        if(Input.GetKey(KeyCode.Mouse0))
        {
            DragHandler();
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            _horizontalLock = false;
            _verticalLock = false;
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
}