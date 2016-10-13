using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameButtonManager : MonoBehaviour
{
    public GameObject MapIcon,
                      GameIcon,
                      MessageIcon,
                      ContactsIcon;

    //ASHLYN: All the magic happens in these properties,
    //If it's being set to true then it enables the button component and makes the button fully opaque
    //Conversely if it's being set to false it disables the button component and makes the button partially transparent
    //In most cases you wont be setting these values directly but rather you'll be changing the correspoding static field in GameManager
    //The only time you'll need to set these properties directly is if you're changing them while in the frame
    public bool MapIconActive
    {
        get
        {
            return GameManager.MapIconActive;
        }
        set
        {
            if (_mapIconActive == !value)
            {
                if (value)
                {
                    _mapImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _mapImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 0.157f);
                }
                _mapButton.enabled = value;
                _mapIconActive = value;
                GameManager.MapIconActive = value;
            }
        }
    }
    private bool _mapIconActive = true;

    //ASHLYN: All of these work the same way for each icon button
    public bool GameIconActive
    {
        get
        {
            return GameManager.GameIconActive;
        }
        set
        {
            if (_gameIconActive == !value)
            {
                if (value)
                {
                    _gameImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _gameImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 0.157f);
                }
                _gameButton.enabled = value;
                _gameIconActive = value;
                GameManager.GameIconActive = value;
            }
        }
    }
    private bool _gameIconActive = true;

    public bool MessageIconActive
    {
        get
        {
            return GameManager.MessageIconActive;
        }
        set
        {
            if (_messageIconActive == !value)
            {
                if (value)
                {
                    _messageImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _messageImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 0.157f);
                }
                _messageButton.enabled = value;
                _messageIconActive = value;
                GameManager.MessageIconActive = value;
            }
        }
    }
    private bool _messageIconActive = true;

    public bool ContactsIconActive
    {
        get
        {
            return GameManager.ContactsIconActive;
        }
        set
        {
            if (_contactsIconActive == !value)
            {
                if (value)
                {
                    _contactsImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
                }
                else
                {
                    _contactsImage.color = new Color(_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 0.157f);
                }
                _contactsButton.enabled = value;
                _contactsIconActive = value;
                GameManager.ContactsIconActive = value;
            }
        }
    }
    private bool _contactsIconActive = true;
    
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
        _mapButton = MapIcon.GetComponent<Button>();
        _gameButton = GameIcon.GetComponent<Button>();
        _messageButton = MessageIcon.GetComponent<Button>();
        _contactsButton = ContactsIcon.GetComponent<Button>();

        _mapImage = MapIcon.GetComponent<Image>();
        _gameImage = GameIcon.GetComponent<Image>();
        _messageImage = MessageIcon.GetComponent<Image>();
        _contactsImage = ContactsIcon.GetComponent<Image>();

        ButtonSetup();
    }

    private void ButtonSetup()
    {
        //ASHLYN: This bit makes sure that our properties are in sync with the static values from the GameManager
        //This only happens on start so if you don't run start(basically if you're already in the frame scene)
        MapIconActive = GameManager.MapIconActive;
        GameIconActive = GameManager.GameIconActive;
        MessageIconActive = GameManager.MessageIconActive;
        ContactsIconActive = GameManager.ContactsIconActive;

        //Here you'll be able to set which icons should be active on start based on character and current convo index.
        //Convo index increments at the end of the message convo if you're curious about that
        switch (GameManager.CurrentCharacterType)
        {
            case CharacterType.Marlon:
                if(GameManager.CurrentConvoIndex == 0)
                {
                    MapIconActive = false;
                    GameIconActive = true;
                    MessageIconActive = true;
                    ContactsIconActive = true;
                }
                if(GameManager.CurrentConvoIndex == 2)
                {
                    MapIconActive = false;
                    GameIconActive = false;
                }
                break;
            case CharacterType.Isaac:
                break;
            case CharacterType.Felix:
                break;
            default:
                break;
        }
    }
}