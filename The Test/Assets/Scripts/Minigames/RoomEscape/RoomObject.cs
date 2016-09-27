using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomObject : MonoBehaviour
{
    public Transform[] PositionCheckers;
    private RoomObjectController _myController;
    private BoxCollider2D _myCollider;
    private List<Vector2> _gridPositions = new List<Vector2>();
    private Vector2 _singleSquareDimensions;
    private bool _collided;

    public bool PlayerObject;

    void Start()
    {
        _myController = FindObjectOfType<RoomObjectController>();
        _myCollider = this.GetComponent<BoxCollider2D>();
        UpdatePosition();
    }

    void OnMouseDown()
    {
        _myController.ObjectToMove = this;
    }

    public void UpdatePosition()
    {
        if (_singleSquareDimensions == Vector2.zero)
        {
            SpriteRenderer Floor = GameObject.Find("Grid").GetComponent<SpriteRenderer>();

            _singleSquareDimensions = new Vector2((Floor.bounds.max.x - Floor.bounds.min.x) / _myController.CurrentGridCount.x, (Floor.bounds.max.y - Floor.bounds.min.y) / _myController.CurrentGridCount.y);
        }

        //Debug.Log(this.name + " update pos check collided: " + _collided);
        if (_myCollider.IsTouchingLayers(LayerMask.GetMask("RoomObject")) && !_collided)
        {
            _gridPositions.Clear();
            for (int i = 0; i < PositionCheckers.Length; i++)
            {
                switch (_myController.ObjectDragDirection)
                {
                    case CardinalDirections.UP:
                        _gridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y - _singleSquareDimensions.y) / -_singleSquareDimensions.y)));
                        break;
                    case CardinalDirections.DOWN:
                        Debug.Log("It works!");
                        _gridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y + _singleSquareDimensions.y) / -_singleSquareDimensions.y)));
                        break;
                    case CardinalDirections.LEFT:
                        _gridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
                        break;
                    case CardinalDirections.RIGHT:
                        _gridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
                        break;
                    default:
                        Debug.LogWarning("How is this possible. There are only four cadinal directions...");
                        break;
                }
            }
        }
        else
        {
            _gridPositions.Clear();
            for (int i = 0; i < PositionCheckers.Length; i++)
            {
                _gridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                               Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
            }
        }
        _collided = false;
        //Debug.Log(this.name + " finish update pos collided: " + _collided);
    }

    public Vector2 GetTargetPos()
    {
        return new Vector2((_myController.TopLeftCornerPos.position.x + (_singleSquareDimensions.x * _gridPositions[0].x)) + (this.transform.position.x - PositionCheckers[0].position.x),
                           (_myController.TopLeftCornerPos.position.y - (_singleSquareDimensions.y * _gridPositions[0].y)) + (this.transform.position.y - PositionCheckers[0].position.y));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(PlayerObject && col.tag == "RoomGoal")
        {
            _myController.GameOver = true;
        }
        else if((!PlayerObject && col.tag != "RoomGoal") || (PlayerObject && col.tag != "Door"))
        {
            _collided = true;
            //Debug.Log(this.name + " on Trigger enter collided: " + _collided);
            if (this.gameObject == _myController.ObjectToMove.gameObject)
                _myController.LoseControl();
        }
    }
}