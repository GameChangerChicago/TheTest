using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomObject : MonoBehaviour
{
    public Transform[] PositionCheckers;
    public List<Vector2> GridPositions = new List<Vector2>();
    private RoomObjectController _myController;
    private BoxCollider2D _myCollider;
    private Vector2 _singleSquareDimensions;

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

        GridPositions.Clear();
        for (int i = 0; i < PositionCheckers.Length; i++)
        {
            GridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                           Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
        }

        if (_myController.OverlapChecker(this))
        {
            GridPositions.Clear();
            for (int i = 0; i < PositionCheckers.Length; i++)
            {
                switch (_myController.ObjectDragDirection)
                {
                    case CardinalDirections.UP:
                        GridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y - _singleSquareDimensions.y) / -_singleSquareDimensions.y)));
                        break;
                    case CardinalDirections.DOWN:
                        GridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y + _singleSquareDimensions.y) / -_singleSquareDimensions.y)));
                        break;
                    case CardinalDirections.LEFT:
                        GridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x + _singleSquareDimensions.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
                        break;
                    case CardinalDirections.RIGHT:
                        GridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x - _singleSquareDimensions.x) / _singleSquareDimensions.x),
                                                       Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
                        break;
                    default:
                        Debug.LogWarning("How is this possible. There are only four cadinal directions...");
                        break;
                }
            }
        }
    }

    public Vector2 GetTargetPos()
    {
        return new Vector2((_myController.TopLeftCornerPos.position.x + (_singleSquareDimensions.x * GridPositions[0].x)) + (this.transform.position.x - PositionCheckers[0].position.x),
                           (_myController.TopLeftCornerPos.position.y - (_singleSquareDimensions.y * GridPositions[0].y)) + (this.transform.position.y - PositionCheckers[0].position.y));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(PlayerObject && col.tag == "RoomGoal")
        {
            _myController.GameOver = true;
            GameManager.MessageIconActive = true;
            if(GameManager.CurrentCharacterType == CharacterType.Marlon && GameManager.CurrentConvoIndex == 2)
            {
                GameManager.MapIconActive = true;
            }
        }
        else if((!PlayerObject && col.tag != "RoomGoal") || (PlayerObject && col.tag != "Door"))
        {
            //Debug.Log(this.name + " on Trigger enter collided: " + _collided);
            if (this.gameObject == _myController.ObjectToMove.gameObject)
                _myController.LoseControl();
        }
    }
}