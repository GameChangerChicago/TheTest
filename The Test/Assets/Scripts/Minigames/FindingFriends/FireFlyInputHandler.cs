using UnityEngine;
using System.Collections;

public class FireFlyInputHandler : MonoBehaviour
{
    private Rigidbody2D _playerRigidbody;
    private Vector2 _targetPosition;
    private bool _atTarget;
    
    void Start()
    {
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _targetPosition = this.transform.position;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (!_atTarget)
        {
            PlayerMovement();
        }
        else if(Vector2.Distance(_playerRigidbody.position, _targetPosition) > 0.5f)
        {
            _atTarget = false;
        }
    }

    private void PlayerMovement()
    {
        _playerRigidbody.velocity = new Vector2((_targetPosition.x - _playerRigidbody.position.x) * 25 * Time.deltaTime, (_targetPosition.y - _playerRigidbody.position.y) * 25 * Time.deltaTime);
        
        if(Vector2.Distance(_playerRigidbody.position, _targetPosition) < 0.5f)
        {
            _atTarget = true;
        }
    }
}