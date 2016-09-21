using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomObject : MonoBehaviour
{
    public Transform[] PositionCheckers;
    private RoomObjectController _myController;
    private List<Vector2> _gridPositions = new List<Vector2>();
    private Vector2 _singleSquareDimensions;

    public bool PlayerObject;

    void Start()
    {
        _myController = FindObjectOfType<RoomObjectController>();
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
        _gridPositions.Clear();

        //BoxCollider2D myBoxCollider = this.GetComponent<BoxCollider2D>();
        //myBoxCollider.
        //if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("RoomObject", "Impassable")))
        //{
        //if(myBoxCollider.bounds.min.x)
        //    Debug.Log("sup");
        //}

        for (int i = 0; i < PositionCheckers.Length; i++)
        {
            _gridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                           Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
        }
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
            _myController.LoseControl();
        }
    }
}