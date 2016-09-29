using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireFlyFriendsManager : MonoBehaviour
{
    public GameObject FireflyFriendEasyMode,
                      FireflyFriendHardMode;
    public bool HardMode;
    private GameObject _colliderToSend;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PotentialFirefly")
        {
            GameObject newFF;
            if (!HardMode)
                newFF = (GameObject)Instantiate(FireflyFriendEasyMode, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.identity);
            else
                newFF = (GameObject)Instantiate(FireflyFriendHardMode, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.identity);
            newFF.GetComponent<FireFlyController>().MyInstantiator = (BoxCollider2D)col;
            col.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.name == "FireFlyFriend(Clone)")
        {
            FireFlyController ffController = col.GetComponent<FireFlyController>();
            ffController.MyInstantiator.enabled = true;
            ffController.MyInstantiator.transform.position = col.transform.position;
            GameObject.Destroy(col.gameObject);
        }
    }
}