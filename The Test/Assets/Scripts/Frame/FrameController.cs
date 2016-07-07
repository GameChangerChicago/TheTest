using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameController : MonoBehaviour
{
    public Image[] PlayerPortraits;

    private GameManager _theGameManager;
    private float _portraitChangeTimer;
    private bool _changingPortraits,
                 _movingLeftPortrait;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
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
                _theGameManager.CurrentCharacterType = CharacterType.RED;
            else if (PlayerPortraits[1].name == "GreenPlayer")
                _theGameManager.CurrentCharacterType = CharacterType.GREEN;
            else if (PlayerPortraits[1].name == "PurplePlayer")
                _theGameManager.CurrentCharacterType = CharacterType.PURPLE;
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
}