using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static CharacterType CurrentCharacterType;
    public static FrameType CurrentFrame;
    public static int CurrentMapPositionIndex,
                      CurrentConvoIndex;
    
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Prioritizing")
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