using UnityEngine;
using System.Collections;

public class PlayerFireFlyController : MonoBehaviour
{
    private Rigidbody2D _playerRigidbody;
    private Vector2 _targetPosition;
    private bool _atTarget,
                 _colliding,
                 _hasControl = true;

    void Start()
    {
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _targetPosition = this.transform.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !_colliding)
        {
            _hasControl = true;
        }

        if (!_atTarget)
        {
            if (_hasControl)
                PlayerMovement();
            else
            {
                _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x / 1.1f, _playerRigidbody.velocity.y / 1.1f);
            }
        }
        else if (Vector2.Distance(_playerRigidbody.position, _targetPosition) > 0.5f)
        {
            _atTarget = false;
        }
    }

    private void PlayerMovement()
    {
        _playerRigidbody.velocity = new Vector2((_targetPosition.x - _playerRigidbody.position.x) * 25 * Time.deltaTime, (_targetPosition.y - _playerRigidbody.position.y) * 25 * Time.deltaTime);

        if (Vector2.Distance(_playerRigidbody.position, _targetPosition) < 0.5f)
        {
            _atTarget = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Impassable")
        {
            _playerRigidbody.velocity = new Vector2(-_playerRigidbody.velocity.x * 1.5f, -_playerRigidbody.velocity.y * 1.5f);
            _colliding = true;
            _hasControl = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _colliding = false;
    }
}