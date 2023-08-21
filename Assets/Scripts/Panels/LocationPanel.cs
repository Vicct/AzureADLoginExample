using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationPanel : MonoBehaviour, IPanel
{
    public RawImage map;
    public InputField mapNotes;
    public Text caseNumberText;
    public ClientInfoPanel ClientInfoPanel;
    public string apiKey;
    public float xCord, yCord;
    public int mapSize;
    public int zoom = 18;
    public string url = "https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/";


    public void OnEnable()
    {
        ClientInfoPanel.gameObject.SetActive(false);
        caseNumberText.text = "" + UIManager.Instance.activeCase.caseID;
        //Download static map
        //https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/47.645523,-122.139059/18?mapSize=500,500&pp=47.645523,-122.139059;66&mapLayer=Basemap,Buildings&key=AtQKq4Syx7z6cISYSCRkOJ5sun-prQ1Voyd9XWhPTmcKl3GyF3OoQ6krQ3B7Kqkq
        url = url + xCord + "," + yCord + "/" + zoom + "?" + "mapSize=" + mapSize + "," + mapSize + "&pp=" + xCord + "," + yCord + ";66&mapLayer=Basemap,Buildings&key=" + apiKey;

    }


    public void ProcessInfo()
    {

    }

}
