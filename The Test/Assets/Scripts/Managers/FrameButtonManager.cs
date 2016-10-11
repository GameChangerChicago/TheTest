using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameButtonManager : MonoBehaviour
{
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