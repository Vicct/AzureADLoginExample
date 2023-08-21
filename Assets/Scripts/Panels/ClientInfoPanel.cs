using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public InputField firstName, lastName;
    public LocationPanel locationPanel;

    public void OnEnable()
    {
        caseNumberText.text = "" + UIManager.Instance.activeCase.caseID;
    }
    public void ProcessInfo()
    {
        if (string.IsNullOrEmpty(firstName.text) || string.IsNullOrEmpty(lastName.text))
        {
            Debug.Log("Either the forst or last name is empty and we cannot continue");
        }
        else
        {
            UIManager.Instance.activeCase.name = firstName.text + " " + lastName.text;
            locationPanel.gameObject.SetActive(true);
        }
    }
}
