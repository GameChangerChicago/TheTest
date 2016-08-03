using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameController : MonoBehaviour
{
    public Image[] PlayerPortraits;
    public GameObject CharacterSelectionFrame,
                      DialogFrame,
                      MapFrame;
    public Transform RedCameraPosition,
                     GreenCameraPosition,
                     PurpleCameraPosistion;
    public Image FullCharacterImage,
                 MapPreview,
                 MapFramePortrait;
    //Plus the Character name and bio which will eventually need to be images.

    //Temporary Public images for setting which character will be show and their map.
    //In the future these will be pulled from a resource file but we'll hold off doing that until we have real art assets.
    public Sprite RedFullBody,
                  GreenFullBody,
                  PurpleFullBody,
                  RedPortrait,
                  GreenPortrait,
                  PurplePortrait,
                  RedMap,
                  GreenMap,
                  PurpleMap;
    //Delete the above

    protected CharacterType currentCharacterType
    {
        get
        {
            return _currentCharacterType;
        }
        set
        {
            switch (value)
            {
                case CharacterType.Felix:
                    FullCharacterImage.sprite = RedFullBody;
                    MapPreview.sprite = RedMap;
                    MapFramePortrait.sprite = RedPortrait;
                    break;
                case CharacterType.Isaac:
                    FullCharacterImage.sprite = GreenFullBody;
                    //MapPreview.sprite = GreenMap;
                    MapFramePortrait.sprite = GreenPortrait;
                    break;
                case CharacterType.Marlon:
                    FullCharacterImage.sprite = PurpleFullBody;
                    //MapPreview.sprite = PurpleMap;
                    MapFramePortrait.sprite = PurplePortrait;
                    break;
                default:
                    Debug.LogWarning("I thought we only had three characters. Did something change?");
                    break;
            }
            _currentCharacterType = value;
        }
    }
    private CharacterType _currentCharacterType;

    private GameManager _theGameManager;
    private MapMovementManager _mapMovementManager;
    private DialogManager _dialogManager;
    private float _portraitChangeTimer;
    private bool _changingPortraits,
                 _movingLeftPortrait;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
        _mapMovementManager = GetComponent<MapMovementManager>();
        _dialogManager = GetComponent<DialogManager>();

        switch (GameManager.CurrentFrame)
        {
            case FrameType.CHARACTERSELECTION:
                CharacterSelectionFrame.SetActive(true);
                MapFrame.SetActive(false);
                break;
            case FrameType.MAPFOLLOW:
                CharacterSelectionFrame.SetActive(false);
                MapFrame.SetActive(true);
                break;
            default:
                Debug.LogWarning("Did you add a new frame type?");
                break;
        }
    }

    void Update()
    {
        if (_changingPortraits)
        {
            PortraitHandler();
        }
    }

    private void PortraitHandler()
    {
        if(_movingLeftPortrait)
        {
            RectTransform leftPortraitRect = PlayerPortraits[0].rectTransform,
                          midPortraitRect = PlayerPortraits[1].rectTransform;

            PlayerPortraits[0].rectTransform.anchorMin = new Vector2(leftPortraitRect.anchorMin.x + (0.33f / (0.5f / Time.deltaTime)), leftPortraitRect.anchorMin.y);
            PlayerPortraits[0].rectTransform.anchorMax = new Vector2(leftPortraitRect.anchorMax.x + (0.4f / (0.5f / Time.deltaTime)), leftPortraitRect.anchorMax.y + (0.05f / (0.5f / Time.deltaTime)));

            PlayerPortraits[1].rectTransform.anchorMin = new Vector2(midPortraitRect.anchorMin.x - (0.33f / (0.5f / Time.deltaTime)), midPortraitRect.anchorMin.y);
            PlayerPortraits[1].rectTransform.anchorMax = new Vector2(midPortraitRect.anchorMax.x - (0.4f / (0.5f / Time.deltaTime)), midPortraitRect.anchorMax.y - (0.05f / (0.5f / Time.deltaTime)));
        }
        else
        {
            RectTransform rightPortraitRect = PlayerPortraits[2].rectTransform,
                          midPortraitRect = PlayerPortraits[1].rectTransform;

            PlayerPortraits[2].rectTransform.anchorMin = new Vector2(rightPortraitRect.anchorMin.x - (0.4f / (0.5f / Time.deltaTime)), rightPortraitRect.anchorMin.y);
            PlayerPortraits[2].rectTransform.anchorMax = new Vector2(rightPortraitRect.anchorMax.x - (0.33f / (0.5f / Time.deltaTime)), rightPortraitRect.anchorMax.y + (0.05f / (0.5f / Time.deltaTime)));

            PlayerPortraits[1].rectTransform.anchorMin = new Vector2(midPortraitRect.anchorMin.x + (0.4f / (0.5f / Time.deltaTime)), midPortraitRect.anchorMin.y);
            PlayerPortraits[1].rectTransform.anchorMax = new Vector2(midPortraitRect.anchorMax.x + (0.33f / (0.5f / Time.deltaTime)), midPortraitRect.anchorMax.y - (0.05f / (0.5f / Time.deltaTime)));
        }

        _portraitChangeTimer += Time.deltaTime;
        if(_portraitChangeTimer > 0.5f)
        {
            if(_movingLeftPortrait)
            {
                Image placeHolder = PlayerPortraits[0];
                PlayerPortraits[0] = PlayerPortraits[1];
                PlayerPortraits[1] = placeHolder;
            }
            else
            {
                Image placeHolder = PlayerPortraits[2];
                PlayerPortraits[2] = PlayerPortraits[1];
                PlayerPortraits[1] = placeHolder;
            }
            _changingPortraits = false;
            _portraitChangeTimer = 0;

            //Setting portraits to their default values, keeping things clean ;)
            PlayerPortraits[0].rectTransform.anchorMin = new Vector2(0.02f, 0);
            PlayerPortraits[0].rectTransform.anchorMax = new Vector2(0.25f, 0.15f);
            PlayerPortraits[1].rectTransform.anchorMin = new Vector2(0.35f, 0);
            PlayerPortraits[1].rectTransform.anchorMax = new Vector2(0.65f, 0.2f);
            PlayerPortraits[2].rectTransform.anchorMin = new Vector2(0.75f, 0);
            PlayerPortraits[2].rectTransform.anchorMax = new Vector2(0.98f, 0.15f);

            //Tell the game manager which character is the selected char
            if (PlayerPortraits[1].name == "RedPlayer")
            {
                currentCharacterType = CharacterType.Felix;
                GameManager.CurrentCharacterType = CharacterType.Felix;
            }
            else if (PlayerPortraits[1].name == "GreenPlayer")
            {
                currentCharacterType = CharacterType.Isaac;
                GameManager.CurrentCharacterType = CharacterType.Isaac;
            }
            else if (PlayerPortraits[1].name == "PurplePlayer")
            {
                currentCharacterType = CharacterType.Marlon;
                GameManager.CurrentCharacterType = CharacterType.Marlon;
            }
        }
    }

    public void MovePortraitToFront(bool left)
    {
        if (!_changingPortraits)
        {
            _movingLeftPortrait = left;
            _changingPortraits = true;
        }
    }

    public void ChangeFrame(int frameNumber)
    {
        switch (frameNumber)
        {
            case 1:
                CharacterSelectionFrame.SetActive(true);
                DialogFrame.SetActive(false);
                MapFrame.SetActive(false);
                GameManager.CurrentFrame = FrameType.CHARACTERSELECTION;
                break;
            case 2:
                CharacterSelectionFrame.SetActive(false);
                DialogFrame.SetActive(false);
                MapFrame.SetActive(true);
                Camera.main.transform.position = RedCameraPosition.position;
                StartCoroutine(_mapMovementManager.StartMoving(0.5f));
                GameManager.CurrentFrame = FrameType.MAPFOLLOW;
                break;
            case 3:
                CharacterSelectionFrame.SetActive(false);
                DialogFrame.SetActive(true);
                MapFrame.SetActive(false);
                _dialogManager.LoadPieceOfDialog();
                GameManager.CurrentFrame = FrameType.DIALOG;
                break;
            default:
                Debug.LogWarning("Hmm, it doesn't seem like we have something set up for a number " + frameNumber + ". How many frames to we have?");
                break;
        }
    }
}