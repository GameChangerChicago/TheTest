using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireFlyBackgroundManager : MonoBehaviour
{
    public GameObject Background1,
                      Background2;

    private Dictionary<Vector2, int[]> _allBackgrounds = new Dictionary<Vector2, int[]>();
    private List<SpriteRenderer> _allBackgroundSprites = new List<SpriteRenderer>();
    private List<Vector2> _activeBackgrounds = new List<Vector2>();
    private Rigidbody2D _playerRigidbody;
    private Vector2 _initalBackgroundPos;

    void Start()
    {
        _allBackgroundSprites.Add(GameObject.Find("Fireflybackground1").GetComponent<SpriteRenderer>());
        _activeBackgrounds.Add(Vector2.zero);
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _initalBackgroundPos = _allBackgroundSprites[0].transform.position;
    }
    
    void Update()
    {
        BackgroundHandler();
    }

    private void BackgroundHandler()
    {
        if (_allBackgroundSprites.Count > 1)
        {
            for (int i = 0; i < _allBackgroundSprites.Count; i++)
            {
                //The player is moving right, the camera is over this background, and their right edges are close enough to create a new bg to the right
                if (_playerRigidbody.velocity.x > 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x < (_allBackgroundSprites[i].transform.position.x + _allBackgroundSprites[i].bounds.extents.x - 0.1f) &&
                    Vector3.Distance(new Vector2((_allBackgroundSprites[i].transform.position.x + _allBackgroundSprites[i].bounds.extents.x), 0),
                                     new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x, 0)) < 1f)
                {
                    CreateNewBackground(_allBackgroundSprites[i], CardinalDirections.RIGHT);
                }
                //The camera is over this background and their left edges are close enough to create a new bg to the left
                if (_playerRigidbody.velocity.x < 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x > (_allBackgroundSprites[i].transform.position.x - _allBackgroundSprites[i].bounds.extents.x + 0.1f) &&
                    Vector3.Distance(new Vector2((_allBackgroundSprites[i].transform.position.x - _allBackgroundSprites[i].bounds.extents.x), 0),
                                     new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x, 0)) < 1f)
                {
                    CreateNewBackground(_allBackgroundSprites[i], CardinalDirections.LEFT);
                }
                //The camera is over this background and their top edges are close enough to create a new bg to the top
                if (_playerRigidbody.velocity.y > 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y < (_allBackgroundSprites[i].transform.position.y + _allBackgroundSprites[i].bounds.extents.y - 0.1f) &&
                    Vector3.Distance(new Vector2(0, (_allBackgroundSprites[i].transform.position.y + _allBackgroundSprites[i].bounds.extents.y)),
                                     new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y)) < 1f)
                {
                    CreateNewBackground(_allBackgroundSprites[i], CardinalDirections.UP);
                }
                //The camera is over this background and their bottom edges are close enough to create a new bg to the bottom
                if (_playerRigidbody.velocity.y < 0 &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y > (_allBackgroundSprites[i].transform.position.y - _allBackgroundSprites[i].bounds.extents.y + 0.1f) &&
                    Vector3.Distance(new Vector2(0, (_allBackgroundSprites[i].transform.position.y - _allBackgroundSprites[i].bounds.extents.y)),
                                     new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y)) < 1f)
                {
                    CreateNewBackground(_allBackgroundSprites[i], CardinalDirections.DOWN);
                }

                //The player is moving left, the camera is not over this background and the camera's right edge is close enough to the bg's left edge to destroy this background
                if (_playerRigidbody.velocity.x < 0 &&
                    Camera.main.transform.position.x < (_allBackgroundSprites[i].transform.position.x - _allBackgroundSprites[i].bounds.extents.x) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x < _allBackgroundSprites[i].transform.position.x - _allBackgroundSprites[i].bounds.extents.x)
                {
                    DestroyBackground(_allBackgroundSprites[i]);
                    break;
                }
                //The player is moving right, the camera is not over this background and the camera's left edge is close enough to the bg's right edge to destroy this background
                if (_playerRigidbody.velocity.x > 0 &&
                    Camera.main.transform.position.x > (_allBackgroundSprites[i].transform.position.x + _allBackgroundSprites[i].bounds.extents.x) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x > _allBackgroundSprites[i].transform.position.x + _allBackgroundSprites[i].bounds.extents.x)
                {
                    DestroyBackground(_allBackgroundSprites[i]);
                    break;
                }
                //The player is moving down, the camera is not over this background and the camera's top edge is close enough to the bg's bottom edge to destroy this background
                if (_playerRigidbody.velocity.y < 0 &&
                    Camera.main.transform.position.y < (_allBackgroundSprites[i].transform.position.y - _allBackgroundSprites[i].bounds.extents.y) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y < _allBackgroundSprites[i].transform.position.y - _allBackgroundSprites[i].bounds.extents.y)
                {
                    DestroyBackground(_allBackgroundSprites[i]);
                    break;
                }
                //The player is moving up, the camera is not over this background and the camera's bottom edge is close enough to the bg's top edge to destroy this background
                if (_playerRigidbody.velocity.y > 0 &&
                    Camera.main.transform.position.y > (_allBackgroundSprites[i].transform.position.y + _allBackgroundSprites[i].bounds.extents.y) &&
                    Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y > _allBackgroundSprites[i].transform.position.y + _allBackgroundSprites[i].bounds.extents.y)
                {
                    DestroyBackground(_allBackgroundSprites[i]);
                    break;
                }
            }

        }
        else
        {
            if (_playerRigidbody.velocity.x > 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x < (_allBackgroundSprites[0].transform.position.x + _allBackgroundSprites[0].bounds.extents.x - 0.1f) &&
                Vector3.Distance(new Vector2((_allBackgroundSprites[0].transform.position.x + _allBackgroundSprites[0].bounds.extents.x), 0),
                                         new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x, 0)) < 1f)
            {
                CreateNewBackground(_allBackgroundSprites[0], CardinalDirections.RIGHT);
            }
            //The camera is over this background and their left edges are close enough to create a new bg to the left
            if (_playerRigidbody.velocity.x < 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x > (_allBackgroundSprites[0].transform.position.x - _allBackgroundSprites[0].bounds.extents.x + 0.1f) &&
                Vector3.Distance(new Vector2((_allBackgroundSprites[0].transform.position.x - _allBackgroundSprites[0].bounds.extents.x), 0),
                                 new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x, 0)) < 1f)
            {
                CreateNewBackground(_allBackgroundSprites[0], CardinalDirections.LEFT);
            }
            //The camera is over this background and their top edges are close enough to create a new bg to the top
            if (_playerRigidbody.velocity.y > 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y < (_allBackgroundSprites[0].transform.position.y + _allBackgroundSprites[0].bounds.extents.y - 0.1f) &&
                Vector3.Distance(new Vector2(0, (_allBackgroundSprites[0].transform.position.y + _allBackgroundSprites[0].bounds.extents.y)),
                                 new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y)) < 1f)
            {
                CreateNewBackground(_allBackgroundSprites[0], CardinalDirections.UP);
            }
            //The camera is over this background and their bottom edges are close enough to create a new bg to the bottom
            if (_playerRigidbody.velocity.y < 0 &&
                Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y > (_allBackgroundSprites[0].transform.position.y - _allBackgroundSprites[0].bounds.extents.y + 0.1f) &&
                Vector3.Distance(new Vector2(0, (_allBackgroundSprites[0].transform.position.y - _allBackgroundSprites[0].bounds.extents.y)),
                                 new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y)) < 1f)
            {
                CreateNewBackground(_allBackgroundSprites[0], CardinalDirections.DOWN);
            }
        }
    }

    private void CreateNewBackground(SpriteRenderer lastBackground, CardinalDirections direction)
    {
        Vector2 lastBackgroundIndex = new Vector2(Mathf.Round((lastBackground.transform.position.x - _initalBackgroundPos.x) / (lastBackground.bounds.extents.x * 2)),
                                                  Mathf.Round((lastBackground.transform.position.y - _initalBackgroundPos.y) / (lastBackground.bounds.extents.y * 2))),
                newBackgroundIndex;
        GameObject newBackground,
                   backgroundToLoad;

        int backgroundType = Random.Range(1, 3),
            backgroundRotation = 90 * Random.Range(0, 4);

        if (backgroundType < 2)
            backgroundToLoad = Background1;
        else
            backgroundToLoad = Background2;

        switch (direction)
        {
            case CardinalDirections.UP:
                newBackgroundIndex = new Vector2(lastBackgroundIndex.x, lastBackgroundIndex.y + 1);
                if (!_activeBackgrounds.Contains(newBackgroundIndex))
                {
                    if (_allBackgrounds.ContainsKey(newBackgroundIndex))
                    {
                        if (_allBackgrounds[newBackgroundIndex][0] < 2)
                            backgroundToLoad = Background1;
                        else
                            backgroundToLoad = Background2;

                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x, lastBackground.transform.position.y + (lastBackground.bounds.extents.y * 2), 0), Quaternion.AngleAxis(_allBackgrounds[newBackgroundIndex][1], Vector3.forward));
                    }
                    else
                    {
                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x, lastBackground.transform.position.y + (lastBackground.bounds.extents.y * 2), 0), Quaternion.AngleAxis(backgroundRotation, Vector3.forward));
                        _allBackgrounds.Add(newBackgroundIndex, new int[2] { backgroundType, backgroundRotation });
                    }
                    _allBackgroundSprites.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(newBackgroundIndex);
                }
                break;
            case CardinalDirections.DOWN:
                newBackgroundIndex = new Vector2(lastBackgroundIndex.x, lastBackgroundIndex.y - 1);
                if (!_activeBackgrounds.Contains(newBackgroundIndex))
                {
                    if (_allBackgrounds.ContainsKey(newBackgroundIndex))
                    {
                        if (_allBackgrounds[newBackgroundIndex][0] < 2)
                            backgroundToLoad = Background1;
                        else
                            backgroundToLoad = Background2;

                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x, lastBackground.transform.position.y - (lastBackground.bounds.extents.y * 2), 0), Quaternion.AngleAxis(_allBackgrounds[newBackgroundIndex][1], Vector3.forward));
                    }
                    else
                    {
                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x, lastBackground.transform.position.y - (lastBackground.bounds.extents.y * 2), 0), Quaternion.AngleAxis(backgroundRotation, Vector3.forward));
                        _allBackgrounds.Add(newBackgroundIndex, new int[2] { backgroundType, backgroundRotation });
                    }
                    _allBackgroundSprites.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(newBackgroundIndex);
                }
                break;
            case CardinalDirections.LEFT:
                newBackgroundIndex = new Vector2(lastBackgroundIndex.x - 1, lastBackgroundIndex.y);
                if (!_activeBackgrounds.Contains(new Vector2(lastBackgroundIndex.x - 1, lastBackgroundIndex.y)))
                {
                    if (_allBackgrounds.ContainsKey(newBackgroundIndex))
                    {
                        if (_allBackgrounds[newBackgroundIndex][0] < 2)
                            backgroundToLoad = Background1;
                        else
                            backgroundToLoad = Background2;

                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x - (lastBackground.bounds.extents.x * 2), lastBackground.transform.position.y, 0), Quaternion.AngleAxis(_allBackgrounds[newBackgroundIndex][1], Vector3.forward));
                    }
                    else
                    {
                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x - (lastBackground.bounds.extents.x * 2), lastBackground.transform.position.y, 0), Quaternion.AngleAxis(backgroundRotation, Vector3.forward));
                        _allBackgrounds.Add(newBackgroundIndex, new int[2] { backgroundType, backgroundRotation });
                    }
                    _allBackgroundSprites.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(newBackgroundIndex);
                }
                break;
            case CardinalDirections.RIGHT:
                newBackgroundIndex = new Vector2(lastBackgroundIndex.x + 1, lastBackgroundIndex.y);
                if (!_activeBackgrounds.Contains(new Vector2(lastBackgroundIndex.x + 1, lastBackgroundIndex.y)))
                {
                    if (_allBackgrounds.ContainsKey(newBackgroundIndex))
                    {
                        if (_allBackgrounds[newBackgroundIndex][0] < 2)
                            backgroundToLoad = Background1;
                        else
                            backgroundToLoad = Background2;

                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x + (lastBackground.bounds.extents.x * 2), lastBackground.transform.position.y, 0), Quaternion.AngleAxis(_allBackgrounds[newBackgroundIndex][1], Vector3.forward));
                    }
                    else
                    {
                        newBackground = (GameObject)Instantiate(backgroundToLoad, new Vector3(lastBackground.transform.position.x + (lastBackground.bounds.extents.x * 2), lastBackground.transform.position.y, 0), Quaternion.AngleAxis(backgroundRotation, Vector3.forward));
                        _allBackgrounds.Add(newBackgroundIndex, new int[2] { backgroundType, backgroundRotation });
                    }
                    _allBackgroundSprites.Add(newBackground.GetComponent<SpriteRenderer>());
                    _activeBackgrounds.Add(newBackgroundIndex);
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
                                                       
        _activeBackgrounds.Remove(backgroundToDestroyIndex);
        _allBackgroundSprites.Remove(backgroundToDestroy);
        GameObject.Destroy(backgroundToDestroy.gameObject);
    }
}