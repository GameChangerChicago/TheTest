using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    public GameObject Background;
    private GameObject _currentBackground,
                       _lastBackground,
                       _currentSegment,
                       _lastSegment;
    private SpriteRenderer _currentBackgroundSR,
                           _lastBackgroundSR;
    private string _currentSegmentName;
    private float _currentTopSegPos = -999,
                  _lastTopSegPos = -999;
    private bool _noMoreSegments;

    void Start()
    {
        _currentBackground = (GameObject)Instantiate(Background, this.transform.position, Quaternion.identity);
        _currentBackgroundSR = _currentBackground.GetComponent<SpriteRenderer>();

        switch(GameManager.CurrentPhoneType)
        {
            case PhoneTypes.ANDROID1:
                _currentBackground.transform.localScale = new Vector3(2.08f, 2.08f, 1);
                break;
            case PhoneTypes.ANDROID2:
                _currentBackground.transform.localScale = new Vector3(1.95f, 1.95f, 1);
                break;
            case PhoneTypes.ANDROID3:
                _currentBackground.transform.localScale = new Vector3(1.87f, 1.87f, 1);
                break;
            case PhoneTypes.IPHONE4:
                _currentBackground.transform.localScale = new Vector3(2.2f, 2.2f, 1);
                break;
            case PhoneTypes.IPHONE5:
                _currentBackground.transform.localScale = new Vector3(1.87f, 1.87f, 1);
                break;
            case PhoneTypes.IPAD:
                _currentBackground.transform.localScale = new Vector3(2.5f, 2.5f, 1);
                break;
            default:
                break;
        }
    }
    
    void Update()
    {
        BackgroundHandlder();
        GameSegmentHandler();
    }

    private void BackgroundHandlder()
    {
        float camTopBackTopDiff = (_currentBackground.transform.position.y + _currentBackgroundSR.bounds.extents.y) -
                                  Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y,
              camBottomBackBottomDiff = 0;

        if (_lastBackground)
            camBottomBackBottomDiff = (_lastBackground.transform.position.y + _lastBackgroundSR.bounds.extents.y) -
                                      Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y;

        if (camBottomBackBottomDiff < 0 && _lastBackground)
        {
            //Delete old background
            GameObject.Destroy(_lastBackground);
        }

        if (camTopBackTopDiff < 0)
        {
            //Spawn new background
            _lastBackground = _currentBackground;
            _lastBackgroundSR = _lastBackground.GetComponent<SpriteRenderer>();
            _currentBackground = (GameObject)Instantiate(Background, new Vector2(_lastBackground.transform.position.x, _lastBackground.transform.position.y + (_currentBackgroundSR.bounds.max.y - _currentBackgroundSR.bounds.min.y)), Quaternion.identity);
            _currentBackgroundSR = _currentBackground.GetComponent<SpriteRenderer>();
            _currentBackground.transform.localScale = new Vector3(_lastBackground.transform.localScale.x, -_lastBackground.transform.localScale.y, _lastBackground.transform.localScale.z);
        }
    }

    private void GameSegmentHandler()
    {
        float camTopSegTopDiff = _currentTopSegPos - Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y,
              camBottomSegTopDiff = 0;

        if (_lastSegment)
        {
            camBottomSegTopDiff = _lastTopSegPos - Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y;
        }

        if(camBottomSegTopDiff < 0 && _lastSegment)
        {
            GameManager.Destroy(_lastSegment);
        }

        if(camTopSegTopDiff < 0 && !_noMoreSegments)
        {
            int currentSegIndex = (int)char.GetNumericValue(_currentSegmentName[2]);
            currentSegIndex++;
            _currentSegmentName = "" + (char)_currentSegmentName[0] + (char)_currentSegmentName[1] + currentSegIndex;
            AddNewSegment("Level " + _currentSegmentName);
        }
    }

    public void AddNewSegment(string segmentName)
    {
        float newSegPosition;

        if (_currentSegment)
        {
            _lastSegment = _currentSegment;
            _lastTopSegPos = _currentTopSegPos;
        }

        Debug.Log(segmentName);
        _currentSegment = Resources.Load<GameObject>("Prefabs/Minigames/Prioritizing/" + segmentName);

        if (_currentSegment)
        {
            _currentSegment = (GameObject)Instantiate(_currentSegment);
            _currentSegmentName = "" + (char)_currentSegment.name[6] + (char)_currentSegment.name[7] + (char)_currentSegment.name[8];

            foreach (SpriteRenderer sr in _currentSegment.GetComponentsInChildren<SpriteRenderer>())
            {
                if (sr.transform.position.y + sr.bounds.extents.y > _currentTopSegPos)
                {
                    _currentTopSegPos = sr.transform.position.y + sr.bounds.extents.y;
                }
            }
        }
        else
        {
            _noMoreSegments = true;
        }
    }
}