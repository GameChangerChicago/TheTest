using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireFlyBackgroundManager : MonoBehaviour
{
    public GameObject Background1,
                      Background2;
    private List<SpriteRenderer> _allBackgrounds;
    private Vector2 _originalCenterPoint;
    
    void Start()
    {
        _originalCenterPoint = this.transform.position;
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
                //The camera is over this background and their right edges are close enough to create a new bg to the right
                if(Camera.main.transform.position.x < (_allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x) &&
                   Vector3.Distance(new Vector2((_allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x), 0),
                                    new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x, 0)) < 0.01f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.RIGHT);
                }
                //The camera is over this background and their left edges are close enough to create a new bg to the left
                if (Camera.main.transform.position.x > (_allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x) &&
                    Vector3.Distance(new Vector2((_allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x), 0),
                                     new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x, 0)) < 0.01f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.LEFT);
                }
                //The camera is over this background and their top edges are close enough to create a new bg to the top
                if (Camera.main.transform.position.y < (_allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y) &&
                    Vector3.Distance(new Vector2(0, (_allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y)),
                                     new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y)) < 0.01f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.UP);
                }
                //The camera is over this background and their bottom edges are close enough to create a new bg to the bottom
                if (Camera.main.transform.position.y > (_allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y) &&
                    Vector3.Distance(new Vector2(0, (_allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y)),
                                     new Vector2(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y)) < 0.01f)
                {
                    CreateNewBackground(_allBackgrounds[i], CardinalDirections.DOWN);
                }

                //The camera is not over this background and the camera's right edge is close enough to the bg's left edge to destroy this background
                if (Camera.main.transform.position.x > (_allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x) &&
                   Vector3.Distance(new Vector2((_allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x), 0),
                                    new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x, 0)) < 0.01f)
                {
                    DestroyBackground(_allBackgrounds[i]);
                }
                //The camera is not over this background and the camera's left edge is close enough to the bg's right edge to destroy this background
                if (Camera.main.transform.position.x < (_allBackgrounds[i].transform.position.x - _allBackgrounds[i].bounds.extents.x) &&
                    Vector3.Distance(new Vector2((_allBackgrounds[i].transform.position.x + _allBackgrounds[i].bounds.extents.x), 0),
                                     new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x, 0)) < 0.01f)
                {
                    DestroyBackground(_allBackgrounds[i]);
                }
                //The camera is not over this background and the camera's top edge is close enough to the bg's bottom edge to destroy this background
                if (Camera.main.transform.position.y > (_allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y) &&
                    Vector3.Distance(new Vector2(0, (_allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y)),
                                     new Vector3(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y)) < 0.01f)
                {
                    DestroyBackground(_allBackgrounds[i]);
                }
                //The camera is not over this background and the camera's bottom edge is close enough to the bg's top edge to destroy this background
                if (Camera.main.transform.position.y < (_allBackgrounds[i].transform.position.y - _allBackgrounds[i].bounds.extents.y) &&
                    Vector3.Distance(new Vector2(0, (_allBackgrounds[i].transform.position.y + _allBackgrounds[i].bounds.extents.y)),
                                     new Vector3(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y)) < 0.01f)
                {
                    DestroyBackground(_allBackgrounds[i]);
                }
            }
        }
        else
        {
            float camTopBackTopDiff = Vector3.Distance(new Vector2(0, (_allBackgrounds[0].transform.position.y + _allBackgrounds[0].bounds.extents.y)),
                                                       new Vector3(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y)),
                  camBottomBackBottomDiff = Vector3.Distance(new Vector2(0, (_allBackgrounds[0].transform.position.y - _allBackgrounds[0].bounds.extents.y)),
                                                             new Vector3(0, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y)),
                  camLeftBackLeftDiff = Vector3.Distance(new Vector2((_allBackgrounds[0].transform.position.x - _allBackgrounds[0].bounds.extents.x), 0),
                                                         new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMin, 0, 0)).x, 0)),
                  camRightBackRightDiff = Vector3.Distance(new Vector2((_allBackgrounds[0].transform.position.x + _allBackgrounds[0].bounds.extents.x), 0),
                                                           new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0)).x, 0));

            if (camTopBackTopDiff < 0.01f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.UP);
            }
            if (camBottomBackBottomDiff < 0.01f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.DOWN);
            }
            if (camLeftBackLeftDiff < 0.01f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.LEFT);
            }
            if (camRightBackRightDiff < 0.01f)
            {
                CreateNewBackground(_allBackgrounds[0], CardinalDirections.RIGHT);
            }
        }
    }

    private void CreateNewBackground(SpriteRenderer lastBackground, CardinalDirections direction)
    {
        switch(direction)
        {
            case CardinalDirections.UP:
                GameObject newBackground = (GameObject)Instantiate(Background1, new Vector3(0, lastBackground.transform.position.y + (lastBackground.bounds.extents.y * 2), 0), Quaternion.identity);
                _allBackgrounds.Add(newBackground.GetComponent<SpriteRenderer>());
                break;
            case CardinalDirections.DOWN:
                Instantiate(Background1, new Vector3(0, lastBackground.transform.position.y + (lastBackground.bounds.extents.y * 2), 0), Quaternion.identity);
                break;
            case CardinalDirections.LEFT:
                Instantiate(Background1, new Vector3(0, lastBackground.transform.position.y + (lastBackground.bounds.extents.y * 2), 0), Quaternion.identity);
                break;
            case CardinalDirections.RIGHT:
                Instantiate(Background1, new Vector3(0, lastBackground.transform.position.y + (lastBackground.bounds.extents.y * 2), 0), Quaternion.identity);
                break;
            default:
                break;
        }
    }

    private void DestroyBackground(SpriteRenderer backgroundToDestroy)
    {
        _allBackgrounds.Remove(backgroundToDestroy);
        GameObject.Destroy(backgroundToDestroy);
    }
}