using UnityEngine;
using System.Collections;

public class RoomObject : MonoBehaviour
{
    private RoomObjectController _myController;

    void Start()
    {
        _myController = FindObjectOfType<RoomObjectController>();
    }

    void OnMouseDown()
    {
        _myController.ObjectToMove = this.gameObject;
    }
}