using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public GameObject DialogContainer;
    public Transform BottomPoint,
                     LeftPoint,
                     RightPoint;

    protected int currentConvoIndex
    {
        get
        {
            return GameManager.CurrentConvoIndex;
        }
        set
        {
            GameManager.CurrentConvoIndex = value;
        }
    }

    private Vector3 _initialDialogPos = Vector3.zero,
                    _initialTopPointPos;//This field will only be needed for this SAT dialog test thing
    private float _dialogOffset;
    private int _currentDialogPieceIndex;
    private bool _dialogFinished,
                 _dialogActive;

    void Start()
    {
        _initialTopPointPos = DialogContainer.transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && _dialogActive)
        {
            if (!_dialogFinished)
                LoadPieceOfDialog();
            else
            {
                _dialogActive = false;
                ClearDialogContainer();
                _dialogFinished = false;
                LoadMinigame();
            }
        }
    }

    private void LoadMinigame()
    {
        switch (GameManager.CurrentCharacterType)
        {
            case CharacterType.Felix:
                if (currentConvoIndex <= 3)
                {
                    SceneManager.LoadScene("Prioritizing");
                }
                else if(currentConvoIndex <= 5)
                {
                    SceneManager.LoadScene("RoomEscape");
                }
                else if(currentConvoIndex == 6)
                {
                    SceneManager.LoadScene("Irritation");
                }
                else if(currentConvoIndex == 7)
                {
                    GameManager.CharacterSelected = false;
                    SceneManager.LoadScene("TempFrame");
                }
                break;
            case CharacterType.Isaac:
                if(currentConvoIndex <= 2)
                {
                    SceneManager.LoadScene("RoomEscape");
                }
                else if(currentConvoIndex == 3)
                {
                    SceneManager.LoadScene("FindingFriends");
                }
                else if(currentConvoIndex == 4)
                {
                    SceneManager.LoadScene("Irritation");
                }
                else if(currentConvoIndex == 5)
                {
                    GameManager.CharacterSelected = false;
                    SceneManager.LoadScene("TempFrame");
                }
                break;
            case CharacterType.Marlon:
                if (currentConvoIndex <= 2)
                {
                    SceneManager.LoadScene("RoomEscape");
                }
                else if(currentConvoIndex <= 4)
                {
                    SceneManager.LoadScene("FindingFriends");
                }
                else if(currentConvoIndex < 5)
                {
                    SceneManager.LoadScene("Prioritizing");
                }
                break;
            default:
                Debug.LogWarning("Did you add a new character type? You'll have to update a lot of switch statements, you know...");
                break;
        }
    }

    public void LoadPieceOfDialog()
    {
        if (_initialDialogPos == Vector3.zero)
        {
            _initialDialogPos = DialogContainer.transform.position;
            _dialogActive = true;
        }

        //Creates a gameobject by pulling the correct dialog prefab from the resources folder
        GameObject pieceOfDialogToLoad = Resources.Load<GameObject>("Prefabs/DialogPieces/" + GameManager.CurrentCharacterType.ToString() + "/" + currentConvoIndex + "/" + _currentDialogPieceIndex);
        Debug.Log("Prefabs/DialogPieces/" + GameManager.CurrentCharacterType.ToString() + "/" + currentConvoIndex + "/" + _currentDialogPieceIndex);
        bool lastPieceOfDialog = false;
        if(!pieceOfDialogToLoad) //The final piece of dialog will be marked with an 'f' this bit lets us know when we're dealing with the last dialog piece
        {
            
            pieceOfDialogToLoad = Resources.Load<GameObject>("Prefabs/DialogPieces/" + GameManager.CurrentCharacterType.ToString() + "/" + currentConvoIndex + "/" + _currentDialogPieceIndex + "f");
            lastPieceOfDialog = true;
        }

        //Instatiates the gameobject at the initial position plus the current offset and puts it inside of the DialogContainer
        if (pieceOfDialogToLoad)
            _dialogOffset += (pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.bounds.max.y / 2f) / 0.7f;
        else
        {
            Debug.Log("GameOver");
            return;
        }

        if (pieceOfDialogToLoad.tag == "LeftSideDialog")
            pieceOfDialogToLoad = (GameObject)Instantiate(pieceOfDialogToLoad, new Vector3(LeftPoint.position.x + ((pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.bounds.max.x / 2) / 0.7f), _initialDialogPos.y - _dialogOffset, _initialDialogPos.z), Quaternion.identity);
        else if (pieceOfDialogToLoad.tag == "RightSideDialog")
            pieceOfDialogToLoad = (GameObject)Instantiate(pieceOfDialogToLoad, new Vector3(RightPoint.position.x - ((pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.bounds.max.x / 2) / 0.7f), _initialDialogPos.y - _dialogOffset, _initialDialogPos.z), Quaternion.identity);
        else
            Debug.LogWarning("You need to tag all dialog pieces with either LeftSideDialog or RightSideDialog.");

        pieceOfDialogToLoad.transform.parent = DialogContainer.transform;

        if(pieceOfDialogToLoad.transform.position.y - ((pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.bounds.max.y / 2f) / 0.7f) < BottomPoint.position.y)
        {
            float containerOffset = Vector2.Distance(new Vector2(0, pieceOfDialogToLoad.transform.position.y - ((pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.bounds.max.y / 2f) / 0.7f)), new Vector2(0,BottomPoint.position.y));
            DialogContainer.transform.position = new Vector3(DialogContainer.transform.position.x, DialogContainer.transform.position.y + containerOffset, pieceOfDialogToLoad.transform.position.z);
            _dialogOffset -= containerOffset;
        }

        //Calculates how much the next dialog box should be offset
        float ammountToIncreaseOffset = (pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.bounds.max.y / 2f) / 0.7f;//Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.rect.yMax)), Camera.main.ScreenToWorldPoint(new Vector3(0, pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.rect.yMin))) / 2;
        
        //This ticks forward the index of currenct convo unless it's the last dialog piece in which case...
        if(!lastPieceOfDialog)
        {
            _currentDialogPieceIndex++;
            _dialogOffset += ammountToIncreaseOffset;// + 0.1f;
        }
        else //We let the dialog manager know the dialog is finished and tick forward the index of which convo you're on.
        {
            _dialogFinished = true;
            currentConvoIndex++;
        }
    }

    //This method is only needed for this SAT dialog test thing.
    private void ClearDialogContainer()
    {
        _currentDialogPieceIndex++;
        for (int i = 0; i < _currentDialogPieceIndex; i++)
        {
            GameObject.Destroy(DialogContainer.transform.GetChild(i + 2).gameObject);
        }
        _currentDialogPieceIndex = 0;
        DialogContainer.transform.position = _initialTopPointPos;
        _dialogOffset = 0;
    }
}