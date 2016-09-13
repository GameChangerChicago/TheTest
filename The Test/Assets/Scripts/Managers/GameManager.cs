using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static PhoneTypes CurrentPhoneType;
    public static CharacterType CurrentCharacterType;
    public static FrameType CurrentFrame;
    public static int CurrentMapPositionIndex,
                      CurrentConvoIndex;
    public static bool CharacterSelected;

    public GameObject CollectDiamond,
                      GetBone,
                      ProtectCookie,
                      FindFF;
    public SpriteRenderer FadeMask;
    public float MiniGameTimer;
    public bool GameOn;

    private MeshRenderer _fireflyLightRenderer;
    private LightController _playerLightController;
    private SpriteRenderer _currentInstructions;
    private bool _removeInstructions;

    void Awake()
    {
        #region DeviceResolution
        if (Camera.main.aspect > 0.74f) //iPad
        {
            CurrentPhoneType = PhoneTypes.IPAD;
        }
        else if (Camera.main.aspect > 0.6665f) //iPhone 4
        {
            CurrentPhoneType = PhoneTypes.IPHONE4;
        }
        else if (Camera.main.aspect > 0.624f) //Android 1
        {
            CurrentPhoneType = PhoneTypes.ANDROID1;
        }
        else if (Camera.main.aspect > 0.5859374f) //Android 2
        {
            CurrentPhoneType = PhoneTypes.ANDROID2;
        }
        else if (Camera.main.aspect > 0.5624f) //iPhone5
        {
            CurrentPhoneType = PhoneTypes.IPHONE5;
        }
        else //Android 3
        {
            CurrentPhoneType = PhoneTypes.ANDROID3;
        }
        #endregion

        if (SceneManager.GetActiveScene().name == "Prioritizing")
        {
            GameObject instructions = Instantiate(CollectDiamond, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
            _currentInstructions = instructions.GetComponent<SpriteRenderer>();

            if (CurrentCharacterType == CharacterType.Felix)
            {
                if (CurrentConvoIndex == 1)
                {
                    FindObjectOfType<BackgroundManager>().AddNewSegment("Level 1-1");
                }
                if (CurrentConvoIndex == 2)
                {
                    FindObjectOfType<BackgroundManager>().AddNewSegment("Level 2-1");
                }
                if (CurrentConvoIndex == 3)
                {
                    FindObjectOfType<BackgroundManager>().AddNewSegment("Level 3-1");
                }
            }
            if (CurrentCharacterType == CharacterType.Marlon)
            {
                if (CurrentConvoIndex == 5)
                {
                    FindObjectOfType<BackgroundManager>().AddNewSegment("Level 1-1");
                }
                else if (CurrentConvoIndex == 6)
                {
                    FindObjectOfType<BackgroundManager>().AddNewSegment("Level 3-1");
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "RoomEscape")
        {
            GameObject instructions = Instantiate(GetBone, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
            _currentInstructions = instructions.GetComponent<SpriteRenderer>();
            GameObject currentRELevel = null;

            if (CurrentCharacterType == CharacterType.Marlon)
            {
                if (CurrentConvoIndex == 0)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 2);
                else if (CurrentConvoIndex == 1)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 3);
                else if (CurrentConvoIndex == 2)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 1);
            }
            if(CurrentCharacterType == CharacterType.Isaac)
            {
                if (CurrentConvoIndex == 0)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 3);
                else if (CurrentConvoIndex == 1)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 4);
                else if (CurrentConvoIndex == 2)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 1);
            }
            else
            {
                if (CurrentConvoIndex == 4)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 5);
                else if (CurrentConvoIndex == 5)
                    currentRELevel = Resources.Load<GameObject>("Prefabs/Minigames/RoomEscape/Level " + 1);
            }

            Instantiate(currentRELevel, currentRELevel.transform.position, Quaternion.identity);
        }
        if (SceneManager.GetActiveScene().name == "Irritation")
        {
            GameObject instructions = Instantiate(ProtectCookie, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
            _currentInstructions = instructions.GetComponent<SpriteRenderer>();
        }
        if (SceneManager.GetActiveScene().name == "FindingFriends")
        {
            _fireflyLightRenderer = GameObject.Find("Player").GetComponentInChildren<MeshRenderer>();
            _playerLightController = _fireflyLightRenderer.gameObject.GetComponent<LightController>();
            _fireflyLightRenderer.enabled = false;
            _playerLightController.enabled = false;
            GameObject instructions = Instantiate(FindFF, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
            _currentInstructions = instructions.GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TempFrame")
        {
            if (CharacterSelected)
            {
                GameObject.Find("SimpleFrame").SetActive(false);
            }
            else
            {
                CurrentConvoIndex = 0;
            }
        }
    }

    void Update()
    {
        if (GameOn)
        {
            MiniGameTimer += Time.deltaTime;
            string currentSceneName = SceneManager.GetActiveScene().name;

            switch (currentSceneName)
            {
                case "Prioritizing":
                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, "TempFrame");
                    }
                    break;
                case "RoomEscape":
                    if (MiniGameTimer > 29)
                    {
                        if (GameManager.CurrentCharacterType == CharacterType.Isaac && GameManager.CurrentConvoIndex == 2)
                        {
                            FadeHandler(false, "FindingFriends");
                        }
                        else if (GameManager.CurrentCharacterType == CharacterType.Felix && GameManager.CurrentConvoIndex == 4)
                        {
                            FadeHandler(false, "Irritation");
                        }
                        else
                        {
                            FadeHandler(false, "TempFrame");
                        }
                    }
                    break;
                case "Irritation":
                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, "TempFrame");
                    }
                    break;
                case "FindingFriends":
                    if (MiniGameTimer > 29)
                    {
                        if(GameManager.CurrentCharacterType == CharacterType.Isaac && GameManager.CurrentConvoIndex == 3)
                        {
                            FadeHandler(false, "Irritation");
                        }
                        FadeHandler(false, "TempFrame");
                    }
                    break;
                default:
                    Debug.Log("This scene doesn't have a timer.");
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && _currentInstructions)
        {
            _removeInstructions = true;
        }

        if (_removeInstructions)
        {
            FadeHandler(true, "");
        }
    }

    public void FadeHandler(bool fadingIn, string SceneToLoad)
    {
        if (fadingIn)
        {
            if (_currentInstructions != null)
            {
                _currentInstructions.color = new Color(_currentInstructions.color.r, _currentInstructions.color.g, _currentInstructions.color.b, _currentInstructions.color.a - (2 * Time.deltaTime));
                if (_currentInstructions.color.a < 0.1f)
                {
                    _currentInstructions.color = new Color(_currentInstructions.color.r, _currentInstructions.color.g, _currentInstructions.color.b, 0);
                    _currentInstructions = null;
                    _removeInstructions = false;
                    GameOn = true;

                    if(SceneManager.GetActiveScene().name == "FindingFriends")
                    {
                        _fireflyLightRenderer.enabled = true;
                        _playerLightController.enabled = true;
                    }
                }
            }
        }
        else
        {
            if (FadeMask != null)
            {
                FadeMask.color = new Color(FadeMask.color.r, FadeMask.color.g, FadeMask.color.b, FadeMask.color.a + (2 * Time.deltaTime));
                if (FadeMask.color.a > 0.9f)
                    SceneManager.LoadScene(SceneToLoad);
            }
            else
                SceneManager.LoadScene(SceneToLoad);
        }
    }
}