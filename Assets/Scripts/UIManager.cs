using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The UI Manager is NULL");
            }
            return _instance;
        }
    }

    public Case activeCase { get; private set; }
    public ClientInfoPanel clientInfoPanel;
    public GameObject borderPanel;

    private void Awake()
    {
        _instance = this;
    }

    public int caseNumberID = 00000000; //vc
    public void CreateNewCase()
    {
        activeCase = new Case();

        //generate a caseID
        //between 0 and 999
        //assign it to the active case ID
        int activeCaseNumber = caseNumberID;
        caseNumberID = caseNumberID + 1;
        activeCase.caseID = "CASE NUMBER: " + caseNumberID;

        clientInfoPanel.gameObject.SetActive(true);
        borderPanel.SetActive(true);
    }

}

