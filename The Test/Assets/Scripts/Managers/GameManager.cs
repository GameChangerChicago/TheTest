using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static CharacterType CurrentCharacterType;
    public static FrameType CurrentFrame;
    public static int CurrentMapPositionIndex;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && SceneManager.GetActiveScene().name == "Prioritizing")
        {
            SceneManager.LoadScene("Frame");
        }
    }
}