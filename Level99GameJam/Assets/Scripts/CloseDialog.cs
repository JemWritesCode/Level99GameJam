using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUPERCharacter;

public class CloseDialog : MonoBehaviour
{
    GameObject parentDialog;
    GameObject inputManagerObject;
    InputManager inputManagerComponent;

    private void Start()
    {
        inputManagerObject = GameObject.Find("InputManager");
        inputManagerComponent = inputManagerObject.GetComponent<InputManager>();

        inputManagerComponent.UnlockCursor();
    }

    public void closeThisDialog()
    {
        parentDialog = gameObject.transform.parent.gameObject;
        parentDialog.SetActive(false);
        inputManagerComponent.LockCursor();
    }


    // Jem was testing pausing the game

    //GameObject player;
    //SUPERCharacterAIO characterController;
    //Swimming swimmingComponent;
    //private void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    characterController = player.GetComponent<SUPERCharacterAIO>();
    //    swimmingComponent = player.GetComponent<Swimming>();

    //    showMouseAndStopMovement();
    //}

    //public void showMouseAndStopMovement()
    //{
    //    swimmingComponent.enabled = false;
    //    characterController.enableMovementControl = false;
    //    characterController.enableCameraControl = false;
    //    Cursor.visible = true;
    //    Cursor.lockState = CursorLockMode.None;
    //}

    //public void hideMouseAndResumeMovement()
    //{
    //    swimmingComponent.enabled = true;
    //    characterController.enableMovementControl = true;
    //    characterController.enableCameraControl = true;
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //}
}
