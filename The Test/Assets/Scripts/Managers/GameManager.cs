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

    public SpriteRenderer FadeMask;
    public int MiniGameTimer;
    public bool GameOn;

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

        CurrentConvoIndex = 1;
        if (SceneManager.GetActiveScene().name == "Prioritizing")
        {
            if (CurrentConvoIndex == 1)
            {
                FindObjectOfType<BackgroundManager>().AddNewSegment("Level 1-1");
            }
            if(CurrentConvoIndex == 2)
            {
                FindObjectOfType<BackgroundManager>().AddNewSegment("Level 2-1");
            }
            if (CurrentConvoIndex == 3)
            {
                FindObjectOfType<BackgroundManager>().AddNewSegment("Level 3-1");
            }
        }
    }

    void Update()
    {
        if(GameOn)
        {
            MiniGameTimer += Mathf.RoundToInt(Time.deltaTime);
            string currentSceneName = SceneManager.GetActiveScene().name;

            switch(currentSceneName)
            {
                case "Prioritizing":
                    if(MiniGameTimer > 29)
                    {
                        FadeHandler(false, "RoomEscape");
                    }
                    break;
                case "RoomEscape":
                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, "Irritation");
                    }
                    break;
                case "Irritation":
                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, "FindingFriends");
                    }
                    break;
                case "FindingFriends":
                    if (MiniGameTimer > 29)
                    {
                        FadeHandler(false, "Frame");
                    }
                    break;
                default:
                    Debug.Log("This scene doesn't have a timer.");
                    break;
            }
        }
    }

    //This is a hackey method we will use to end the game simply for SAT purposes.
    public void HACKENDGAME()
    {
        SceneManager.LoadScene("YouGotTested");
        CurrentConvoIndex = 0;
    }

    private void FadeHandler(bool fadingIn, string SceneToLoad)
    {
        if (fadingIn)
        {
            if (FadeMask != null)
            {
                FadeMask.color = new Color(FadeMask.color.r, FadeMask.color.g, FadeMask.color.b, FadeMask.color.a - (2 * Time.deltaTime));
                if (FadeMask.color.a < 0.1f)
                    FadeMask.color = new Color(FadeMask.color.r, FadeMask.color.g, FadeMask.color.b, 0);
            }
            else
                SceneManager.LoadScene(SceneToLoad);
        }
        else
        {
            if (FadeMask != null)
            {
                FadeMask.color = new Color(FadeMask.color.r, FadeMask.color.g, FadeMask.color.b, FadeMask.color.a + (2 * Time.deltaTime));
                if (FadeMask.color.a > 0.9f)
                    SceneManager.LoadScene(SceneToLoad);
            }
        }
    }
}