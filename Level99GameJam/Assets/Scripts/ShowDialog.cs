using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialog : MonoBehaviour
{
    GameObject inputManagerObject;
    InputManager inputManagerComponent;
    [SerializeField] GameObject dialogToShow;

    public void showDialog()
    {
        dialogToShow.SetActive(true);

        inputManagerObject = GameObject.Find("InputManager");
        inputManagerComponent = inputManagerObject.GetComponent<InputManager>();
        inputManagerComponent.UnlockCursor();
    }

}
