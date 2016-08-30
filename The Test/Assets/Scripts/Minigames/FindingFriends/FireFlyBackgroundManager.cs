using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireFlyBackgroundManager : MonoBehaviour
{
    public GameObject Background1,
                      Background2;

    private List<SpriteRenderer> _allBackgrounds = new List<SpriteRenderer>();
    private List<Vector2> _activeBackgrounds = new List<Vector2>();
    private Rigidbody2D _playerRigidbody;
    private Vector2 _initalBackgroundPos;

    void Start()
    {
        _allBackgrounds.Add(GameObject.Find("Fireflybackground1").GetComponent<SpriteRenderer>());
        _activeBackgrounds.Add(Vector2.zero);
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _initalBackgroundPos = _allBackgrounds[0].transform.position;
    }
    
    void Update()
    {
        BackgroundHandler();
    }

    private void BackgroundHandler()
    {
        if (_allBackgrounds.Count > 1)
        {
            for (int i = 0; i < _allBackgrounds.Count; i++)
            {
                //The player is moving right, the camera is over this background, and their right edges are close enough to create a new bg to the right
                if (_playerRigidbody.velocity.x > 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x < (_allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x - 0.1f) &&
                    Vector3.Distance(new Vector2((_allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x), 0),
                                     new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x, 0)) < 1f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.RIGHT);
                }
                //The camera is over this background and their left edges are close enough to create a new bg to the left
                if (_playerRigidbody.velocity.x < 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x > (_allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x + 0.1f) &&
                    Vector3.Distance(new Vector2((_allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x), 0),
                                     new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x, 0)) < 1f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.LEFT);
                }
                //The camera is over this background and their top edges are close enough to create a new bg to the top
                if (_playerRigidbody.velocity.y > 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y < (_allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y - 0.1f) &&
                    Vector3.Distance(new Vector2(0, (_allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y)),
                                     new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y)) < 1f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.UP);
                }
                //The camera is over this background and their bottom edges are close enough to create a new bg to the bottom
                if (_playerRigidbody.velocity.y < 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y > (_allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y + 0.1f) &&
                    Vector3.Distance(new Vector2(0, (_allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y)),
                                     new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y)) < 1f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.DOWN);
                }

                //The player is moving left, the camera is not over this background and the camera's right edge is close enough to the bg's left edge to destroy this background
                if (_playerRigidbody.velocity.x < 0 &&
                    Camera.main.transform.position.x < (_allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x < _allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x)
                {
                    DestroyBackground(_allBackgrounds[i]);
                    break;
                }
                //The player is moving right, the camera is not over this background and the camera's left edge is close enough to the bg's right edge to destroy this background
                if (_playerRigidbody.velocity.x > 0 &&
                    Camera.main.transform.position.x > (_allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x > _allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x)
                {
                    DestroyBackground(_allBackgrounds[i]);
                    break;
                }
                //The player is moving down, the camera is not over this background and the camera's top edge is close enough to the bg's bottom edge to destroy this background
                if (_playerRigidbody.velocity.y < 0 &&
                    Camera.main.transform.position.y < (_allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y < _allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y)
                {
                    DestroyBackground(_allBackgrounds[i]);
                    break;
                }
                //The player is moving up, the camera is not over this background and the camera's bottom edge is close enough to the bg's top edge to destroy this background
                if (_playerRigidbody.velocity.y > 0 &&
                    Camera.main.transform.position.y > (_allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y > _allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y)
                {
                    DestroyBackground(_allBackgrounds[i]);
                    break;
                }
            }

        }
        else
        {
            if (_playerRigidbody.velocity.x > 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x < (_allBackgrounds[0].transform.position.x + _allBackgrounds[0].bounds.extents.x - 0.1f) &&
                Vector3.Distance(new Vector2((_allBackgrounds[0].transform.position.x + _allBackgrounds[0].bounds.extents.x), 0),
                                         new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x, 0)) < 1f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.RIGHT);
            }
            //The camera is over this background and their left edges are close enough to create a new bg to the left
            if (_playerRigidbody.velocity.x < 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x > (_allBackgrounds[0].transform.position.x - _allBackgrounds[0].bounds.extents.x + 0.1f) &&
                Vector3.Distance(new Vector2((_allBackgrounds[0].transform.position.x - _allBackgrounds[0].bounds.extents.x), 0),
                                 new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x, 0)) < 1f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.LEFT);
            }
            //The camera is over this background and their top edges are close enough to create a new bg to the top
            if (_playerRigidbody.velocity.y > 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y < (_allBackgrounds[0].transform.position.y + _allBackgrounds[0].bounds.extents.y - 0.1f) &&
                Vector3.Distance(new Vector2(0, (_allBackgrounds[0].transform.position.y + _allBackgrounds[0].bounds.extents.y)),
                                 new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y)) < 1f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.UP);
            }
            //The camera is over this background and their bottom edges are close enough to create a new bg to the bottom
            if (_playerRigidbody.velocity.y < 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y > (_allBackgrounds[0].transform.position.y - _allBackgrounds[0].bounds.extents.y + 0.1f) &&
                Vector3.Distance(new Vector2(0, (_allBackgrounds[0].transform.position.y - _allBackgrounds[0].bounds.extents.y)),
                                 new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y)) < 1f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.DOWN);
            }
        }
    }

    private void CreateNewBackground(SpriteRenderer lastBackground, CardinalDirections direction)
    {
        Vector2 lastBackgroundIndex = new Vector2(Vector2.Distance(new Vector2(0, lastBackground.transform.position.x), new Vector2(0, _initalBackgroundPos.x)) / (lastBackground.bounds.extents.x * 2),
                                          Vector2.Distance(new Vector2(lastBackground.transform.position.y, 0), new Vector2(_initalBackgroundPos.y, 0)) / (lastBackground.bounds.extents.y * 2));
        GameObject newBackground;

        switch (direction)
        {
            case CardinalDirections.UP:
                if (!_activeBackgrounds.Contains(new Vector2(lastBackgroundIndex.x, lastBackgroundIndex.y + 1)))
                {
                    newBackground = (GameObject)Instantiate(Background1, new Vector3(lastBackground.transform.position.x, lastBackground.transform.position.y + (lastBackground.bounds.extents.y * 2), 0), Quaternion.identity);
                    _allBackgrounds.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(new Vector2(lastBackgroundIndex.x, lastBackgroundIndex.y + 1));
                }
                break;
            case CardinalDirections.DOWN:
                if (!_activeBackgrounds.Contains(new Vector2(lastBackgroundIndex.x, lastBackgroundIndex.y - 1)))
                {
                    newBackground = (GameObject)Instantiate(Background1, new Vector3(lastBackground.transform.position.x, lastBackground.transform.position.y - (lastBackground.bounds.extents.y * 2), 0), Quaternion.identity);
                    _allBackgrounds.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(new Vector2(lastBackgroundIndex.x, lastBackgroundIndex.y - 1));
                    Debug.Log(new Vector2(lastBackgroundIndex.x, lastBackgroundIndex.y - 1));
                }
                break;
            case CardinalDirections.LEFT:
                if (!_activeBackgrounds.Contains(new Vector2(lastBackgroundIndex.x - 1, lastBackgroundIndex.y)))
                {
                    newBackground = (GameObject)Instantiate(Background1, new Vector3(lastBackground.transform.position.x - (lastBackground.bounds.extents.x * 2), lastBackground.transform.position.y, 0), Quaternion.identity);
                    _allBackgrounds.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(new Vector2(lastBackgroundIndex.x - 1, lastBackgroundIndex.y));
                }
                break;
            case CardinalDirections.RIGHT:
                if (!_activeBackgrounds.Contains(new Vector2(lastBackgroundIndex.x + 1, lastBackgroundIndex.y)))
                {
                    newBackground = (GameObject)Instantiate(Background1, new Vector3(lastBackground.transform.position.x + (lastBackground.bounds.extents.x * 2), lastBackground.transform.position.y, 0), Quaternion.identity);
                    _allBackgrounds.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(new Vector2(lastBackgroundIndex.x + 1, lastBackgroundIndex.y));
                }
                break;
            default:
                break;
        }
    }

    private void DestroyBackground(SpriteRenderer backgroundToDestroy)
    {
        Vector2 backgroundToDestroyIndex = new Vector2(Mathf.Round((backgroundToDestroy.transform.position.x - _initalBackgroundPos.x) / (backgroundToDestroy.bounds.extents.x * 2)),
                                                       Mathf.Round((backgroundToDestroy.transform.position.y - _initalBackgroundPos.y) / (backgroundToDestroy.bounds.extents.y * 2)));
                                                       
        _activeBackgrounds.Remove(new Vector2(backgroundToDestroyIndex.x, backgroundToDestroyIndex.y));
        _allBackgrounds.Remove(backgroundToDestroy);
        GameObject.Destroy(backgroundToDestroy.gameObject);
    }
}