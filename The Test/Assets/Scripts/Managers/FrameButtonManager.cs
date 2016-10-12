using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameButtonManager : MonoBehaviour
{
    protected bool mapIconActive
    {
        get
        {
            return _mapIconActive;
        }
        set
        {
            if (_mapIconActive = !value)
            {
                if (value)
                {
                    _mapImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _mapImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 40);
                }
                _mapButton.enabled = value;
                _mapIconActive = value;
            }
        }
    }
    private bool _mapIconActive;

    protected bool gameIconActive
    {
        get
        {
            return _gameIconActive;
        }
        set
        {
            if (_gameIconActive = !value)
            {
                if (value)
                {
                    _gameImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _gameImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 40);
                }
                _gameButton.enabled = value;
                _gameIconActive = value;
            }
        }
    }
    private bool _gameIconActive;

    protected bool messageIconActive
    {
        get
        {
            return _messageIconActive;
        }
        set
        {
            if (_messageIconActive = !value)
            {
                if (value)
                {
                    _messageImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _messageImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 40);
                }
                _messageButton.enabled = value;
                _messageIconActive = value;
            }
        }
    }
    private bool _messageIconActive;

    protected bool contactsIconActive
    {
        get
        {
            return _contactsIconActive;
        }
        set
        {
            if (_contactsIconActive = !value)
            {
                if (value)
                {
                    _contactsImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _contactsImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 40);
                }
                _contactsButton.enabled = value;
                _contactsIconActive = value;
            }
        }
    }
    private bool _contactsIconActive;
    
    private Button _mapButton,
                   _gameButton,
                   _messageButton,
                   _contactsButton;
    private Image _mapImage,
                  _gameImage,
                  _messageImage,
                  _contactsImage;
     
    void Start()
    {
        GameObject MapIcon = GameObject.Find("MapIcon"),
                   GameIcon = GameObject.Find("GameIcon"),
                   MessageIcon = GameObject.Find("MessageIcon"),
                   ContactsIcon = GameObject.Find("ContactsIcon");

        _mapButton = MapIcon.GetComponent<Button>();
        _gameButton = GameIcon.GetComponent<Button>();
        _messageButton = MessageIcon.GetComponent<Button>();
        _contactsButton = ContactsIcon.GetComponent<Button>();

        _mapImage = MapIcon.GetComponent<Image>();
        _gameImage = GameIcon.GetComponent<Image>();
        _messageImage = MessageIcon.GetComponent<Image>();
        _contactsImage = ContactsIcon.GetComponent<Image>();
    }
}