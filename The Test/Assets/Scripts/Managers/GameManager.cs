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
    public SpriteRenderer BlackFadeMask,
                          WhiteFadeMask;
    public float MiniGameTimer;
    public bool GameOn;

    private MeshRenderer _fireflyLightRenderer;
    private LightController _playerLightController;
    private SpriteRenderer _currentInstructions;
    private bool _removeInstructions;


    void Awake()
    {
        #region DeviceResolution
        if (Camera.main.aspect > 0.74f)
        { //iPad
            CurrentPhoneType = PhoneTypes.IPAD;
        }
        else if (Camera.main.aspect > 0.6665f)
        { //iPhone 4
            CurrentPhoneType = PhoneTypes.IPHONE4;
        }
        else if (Camera.main.aspect > 0.624f)
        { //Android 1
            CurrentPhoneType = PhoneTypes.ANDROID1;
        }
        else if (Camera.main.aspect > 0.5859374f)
        { //Android 2
            CurrentPhoneType = PhoneTypes.ANDROID2;
        }
        else if (Camera.main.aspect > 0.5624f)
        { //iPhone5
            CurrentPhoneType = PhoneTypes.IPHONE5;
        }
        else
        { //Android 3
            CurrentPhoneType = PhoneTypes.ANDROID3;
        }
        #endregion

        //Mason: Temp solution to test transition between phone and game scenes without character select. If you are seeing this, please feel free to remove.
        //Ashlyn: This is ok, just comment stuff like this out before making commits.
        //CurrentCharacterType = CharacterType.Marlon;

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
            if (CurrentCharacterType == CharacterType.Isaac)
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
                else //Incase this level is being started out of context
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

            //ASHLYN: This switch is where all of the transitions out of the minigames happen. They are all watching MiniGameTimer and will call FadeHandler to bring up the next scene. Check FadeHandler for more.
            switch (currentSceneName)
            {
                case "Prioritizing":
                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, false, "TempFrame");
                    }
                    break;
                case "RoomEscape":
                    if (MiniGameTimer > 29)
                    {
                        //ASHLYN: Certain situations require for a new minigame to be loaded rather than the frame. This will be based on which character you've selected and the conversation you had last.
                        if (GameManager.CurrentCharacterType == CharacterType.Isaac && GameManager.CurrentConvoIndex == 2)
                        {
                            FadeHandler(false, false, "FindingFriends");
                        }
                        else if (GameManager.CurrentCharacterType == CharacterType.Felix && GameManager.CurrentConvoIndex == 4)
                        {
                            FadeHandler(false, false, "Irritation");
                        }
                        else
                        {
                            FadeHandler(false, false, "TempFrame");
                        }
                    }
                    break;
                case "Irritation":
                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, true, "TempFrame");
                    }
                    break;
                case "FindingFriends":
                    if (MiniGameTimer > 29)
                    {
                        if (GameManager.CurrentCharacterType == CharacterType.Isaac && GameManager.CurrentConvoIndex == 3)
                        {
                            FadeHandler(false, false, "Irritation");
                        }
                        FadeHandler(false, false, "TempFrame");
                    }
                    break;
                case "PhoneScene":

                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, false, "PhoneScreen");
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
            FadeHandler(true, false, "");
        }
    }

    //ASHLYN: This method takes two paramaters: fadingIn determins whether we're fading in on a scene or out of one. Basically use false for changing scenes. You basically wont need to use this method when fadingIn == true.
    //SceneToLoad is a just a string of the scene you're to load. Long story short: call the method as FadeHandler(false, "NameOfScene");
    public void FadeHandler(bool fadingIn, bool win, string SceneToLoad)
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

                    if (SceneManager.GetActiveScene().name == "FindingFriends")
                    {
                        _fireflyLightRenderer.enabled = true;
                        _playerLightController.enabled = true;
                    }
                }
            }
        }
        else
        {
            if (!win && BlackFadeMask != null)
            {
                BlackFadeMask.color = new Color(BlackFadeMask.color.r, BlackFadeMask.color.g, BlackFadeMask.color.b, BlackFadeMask.color.a + (2 * Time.deltaTime));
                if (BlackFadeMask.color.a > 0.9f)
                    SceneManager.LoadScene(SceneToLoad);
            }
            else if(win && WhiteFadeMask != null)
            {
                WhiteFadeMask.color = new Color(WhiteFadeMask.color.r, WhiteFadeMask.color.g, WhiteFadeMask.color.b, WhiteFadeMask.color.a + (2 * Time.deltaTime));
                if (WhiteFadeMask.color.a > 0.9f)
                    SceneManager.LoadScene(SceneToLoad);
            }
            else
                SceneManager.LoadScene(SceneToLoad);
        }
    }
}