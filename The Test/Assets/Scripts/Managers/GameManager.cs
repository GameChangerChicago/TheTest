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

        //CurrentConvoIndex = 1;
        if (SceneManager.GetActiveScene().name == "Prioritizing")
        {
            if(CurrentConvoIndex == 1)
            {
                GameObject LevelToLoad = Resources.Load<GameObject>("Prefabs/Minigames/Prioritizing/Level 1");
                Instantiate(LevelToLoad);
            }
            if(CurrentConvoIndex == 2)
            {
                GameObject LevelToLoad = Resources.Load<GameObject>("Prefabs/Minigames/Prioritizing/Level 2");
                Instantiate(LevelToLoad);
            }
            if (CurrentConvoIndex == 3)
            {
                GameObject LevelToLoad = Resources.Load<GameObject>("Prefabs/Minigames/Prioritizing/Level 3");
                Instantiate(LevelToLoad);
            }
        }
    }

    //This is a hackey method we will use to end the game simply for SAT purposes.
    public void HACKENDGAME()
    {
        SceneManager.LoadScene("YouGotTested");
        CurrentConvoIndex = 0;
    }
}