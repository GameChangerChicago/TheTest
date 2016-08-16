using UnityEngine;
using System.Collections;

public class FireFlyInputHandler : MonoBehaviour
{
    private GameObject _playerFireFly;
    
    void Start()
    {
        _playerFireFly = GameObject.Find("Player");
    }   

    void OnMouseDown()
    {
        
    }
}