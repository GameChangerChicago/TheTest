using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public GameObject DialogContainer;

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

    private Vector3 _initialDialogPos = Vector3.zero;
    private float _dialogOffset;
    private int _currentDialogPieceIndex;
    private bool _dialogFinished;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!_dialogFinished)
                LoadPieceOfDialog();
            else
                LoadMinigame();

        }
    }

    private void LoadMinigame()
    {
        switch(GameManager.CurrentCharacterType)
        {
            case CharacterType.RED:
            if(currentConvoIndex == 1)
                {
                    SceneManager.LoadScene("Prioritizing");
                }
                break;
            case CharacterType.GREEN:
                break;
            case CharacterType.PURPLE:
                break;
            default:
                Debug.LogWarning("Did you add a new character type? You'll have to update a lot of switch statements, you know...");
                break;
        }
    }

    public void LoadPieceOfDialog()
    {
        if(_initialDialogPos == Vector3.zero)
            _initialDialogPos = DialogContainer.transform.position;

        //Creates a gameobject by pulling the correct dialog prefab from the resources folder
        GameObject pieceOfDialogToLoad = Resources.Load<GameObject>("DialogPieces/" + GameManager.CurrentCharacterType.ToString() + "/" + currentConvoIndex + "/" + _currentDialogPieceIndex);
        bool lastPieceOfDialog = false;
        if(!pieceOfDialogToLoad) //The final piece of dialog will be marked with an 'f' this bit lets us know when we're dealing with the last dialog piece
        {
            pieceOfDialogToLoad = Resources.Load<GameObject>("DialogPieces/" + GameManager.CurrentCharacterType.ToString() + "/" + currentConvoIndex + "/" + _currentDialogPieceIndex + "f");
            lastPieceOfDialog = true;
        }

        //Instatiates the gameobject at the initial position plus the current offset and puts it inside of the DialogContainer
        pieceOfDialogToLoad = (GameObject)Instantiate(pieceOfDialogToLoad, new Vector3(_initialDialogPos.x, _initialDialogPos.y - _dialogOffset, _initialDialogPos.z), Quaternion.identity);
        pieceOfDialogToLoad.transform.parent = DialogContainer.transform;
        //Calculates how much the next dialog box should be offset
        float ammountToIncreaseOffset = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.rect.yMax)), Camera.main.ScreenToWorldPoint(new Vector3(0, pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.rect.yMin))) / 2;
        
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
}