using UnityEngine;
using System.Collections;

public class FireFlyController : MonoBehaviour
{
    private Rigidbody2D _myRigidbody,
                        _playerRigidbody;
    private LightController _myLight,
                            _playerLight;
    private Vector2 _randomModifier;
    private bool _following;
    
    void Start()
    {
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _playerLight = _playerRigidbody.GetComponentInChildren<LightController>();
        _myRigidbody = this.GetComponent<Rigidbody2D>();
        _myLight = this.GetComponentInChildren<LightController>();
        UpdateMyRandomValue();
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
        _myRigidbody.velocity = new Vector2(_randomModifier.x * Time.deltaTime, _randomModifier.y * Time.deltaTime);
    }

    private void FollowMovement()
    {
        _myRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x + _randomModifier.x, _playerRigidbody.velocity.y + _randomModifier.y);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "Player")
        {
            _following = true;
            _playerLight.GainedAFriend();
            StartCoroutine(_myLight.FadeAway());
            this.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void UpdateMyRandomValue()
    {
        if (_following && Vector2.Distance(this.transform.position, _playerRigidbody.position) > 1)
        {
            if (this.transform.position.x > _playerRigidbody.position.x)
            {
                //Top Right Quad
                if (this.transform.position.y > _playerRigidbody.position.y)
                {
                    _randomModifier = new Vector2(Random.Range(-0.5f, -0.1f), Random.Range(-0.5f, -0.1f));
                }
                else //Bottom Right Quad
                {
                    _randomModifier = new Vector2(Random.Range(-0.5f, -0.1f), Random.Range(0.1f, 0.5f));
                }
            }
            else
            {
                //Top Left Quad
                if (this.transform.position.y > _playerRigidbody.position.y)
                {
                    _randomModifier = new Vector2(Random.Range(0.1f, 0.5f), Random.Range(-0.5f, -0.1f));
                }
                else //Bottom Left Quad
                {
                    _randomModifier = new Vector2(Random.Range(0.1f, 0.5f), Random.Range(0.1f, 0.5f));
                }
            }
        }
        else
        {
            _randomModifier = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        }
        Invoke("UpdateMyRandomValue", Random.Range(0.1f, 1f));
    }
}