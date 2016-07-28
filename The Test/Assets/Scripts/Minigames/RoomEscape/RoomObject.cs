﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomObject : MonoBehaviour
{
    public Transform[] PositionCheckers;
    private RoomObjectController _myController;
    private List<Vector2> _gridPositions = new List<Vector2>();
    private Vector2 _singleSquareDimensions;

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
        if(_singleSquareDimensions == Vector2.zero)
        {
            SpriteRenderer Floor = GameObject.Find("Floor").GetComponent<SpriteRenderer>();

            _singleSquareDimensions = new Vector2((Floor.sprite.bounds.max.x - Floor.sprite.bounds.min.x) / _myController.CurrentGridCount.x, (Floor.sprite.bounds.max.y - Floor.sprite.bounds.min.y) / _myController.CurrentGridCount.y);
        }
        _gridPositions.Clear();

        for (int i = 0; i < PositionCheckers.Length; i++)
        {
            _gridPositions.Add(new Vector2(Mathf.RoundToInt((PositionCheckers[i].position.x - _myController.TopLeftCornerPos.position.x) / _singleSquareDimensions.x),
                                            Mathf.RoundToInt((PositionCheckers[i].position.y - _myController.TopLeftCornerPos.position.y) / -_singleSquareDimensions.y)));
        }
    }

    public Vector2 GetTargetPos()
    {
        Debug.Log(_myController.TopLeftCornerPos.position.y + " - " + "(" + _singleSquareDimensions.y + " * " + _gridPositions[0].y + ")");
        return new Vector2((_myController.TopLeftCornerPos.position.x + (_singleSquareDimensions.x * _gridPositions[0].x)) + (this.transform.position.x - PositionCheckers[0].position.x),
                           (_myController.TopLeftCornerPos.position.y - (_singleSquareDimensions.y * _gridPositions[0].y)) + (this.transform.position.y - PositionCheckers[0].position.y));
    }
}