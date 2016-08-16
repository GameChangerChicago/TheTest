using UnityEngine;
using System.Collections;

public class FireFlyController : MonoBehaviour
{
    private GameObject _playerFireFly;
    private Rigidbody2D _myRigidbody;
    private bool _following;
    
    void Start()
    {
        _playerFireFly = GameObject.Find("Player");
        _myRigidbody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_following)
        {
            FollowMovement();
        }
        else
        {
            IdleMovement();
        }
    }

    private void IdleMovement()
    {
        
    }

    private void FollowMovement()
    {
        
    }
}