using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class YouGotTestedScript : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SceneManager.LoadScene("Frame");
            GameManager.CurrentConvoIndex = 0;
        }
    }
}