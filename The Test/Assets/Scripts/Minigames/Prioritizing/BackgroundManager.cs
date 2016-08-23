using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    public GameObject Background;
    private GameObject _currentBackground,
                       _lastBackground;
    private SpriteRenderer _currentBackgroundSR,
                           _lastBackgroundSR;

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
        float camTopBackTopDiff = (_currentBackground.transform.position.y + _currentBackgroundSR.bounds.extents.y) -
                                  Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMax, 0)).y,
              camBottomBackBottomDiff = 0;

        if (_lastBackground)
            camBottomBackBottomDiff = (_lastBackground.transform.position.y + _lastBackgroundSR.bounds.extents.y) -
                                      Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.yMin, 0)).y;

        if(camBottomBackBottomDiff < 0 && _lastBackground)
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
}