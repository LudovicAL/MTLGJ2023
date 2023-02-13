using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : Singleton<UIControls> {

    [Header("Vehicle selection")]
    [SerializeField]
    private GameObject deloreanPrefab;
    [SerializeField]
    private GameObject coupePrefab;
    [SerializeField]
    private GameObject buggyPrefab;
    [SerializeField]
    private GameObject jeepPrefab;
    [SerializeField]
    private GameObject busPrefab;
    [SerializeField]
    private GameObject truckPrefab;

    [Header("UI Panels")]
    [SerializeField]
    private List<GameObject> panelList;

    // Start is called before the first frame update
    void Start() {
        GameManager.Instance.gameStateChangedEvent.AddListener(GameStateChanged);
        GameStateChanged();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void GameStateChanged() {
        switch(GameManager.Instance.currentState) {
            case GameState.Menu:
                ActivatePanel("Panel Menu");
                break;
            case GameState.Starting:
                ActivatePanel("Panel Starting");
                break;
            case GameState.Started:
                ActivatePanel("Panel Started");
                break;
            case GameState.PowerUp:
                ActivatePanel("Panel PowerUp");
                break;
            case GameState.Paused:
                ActivatePanel("Panel Paused");
                break;
            case GameState.Ended:
                ActivatePanel("Panel Ended");
                break;
            default:
                Debug.Log("The broadcasted GameState is not recognized.");
                break;
        }
    }
    
    public void SelectVehicle(string chosenVehicle) {
        switch (chosenVehicle) {
            case "Delorean":
                StartGame(deloreanPrefab);
                break;
            case "Coupe":
                StartGame(coupePrefab);
                break;
            case "Buggy":
                StartGame(buggyPrefab);
                break;
            case "Jeep":
                StartGame(jeepPrefab);
                break;
            case "Bus":
                StartGame(busPrefab);
                break;
            case "Truck":
                StartGame(truckPrefab);
                break;
            default:
                Debug.Log("The chosen vehicle is not recognized.");
                break;
        }
    }

    private void StartGame(GameObject chosenVehiclePrefab) {
        VehicleSpawner.Instance.SpawnVehicle(chosenVehiclePrefab);
        GameManager.Instance.RequestGameStateChange(GameState.Starting);
    }

    private void ActivatePanel(string panelName) {
        foreach (GameObject panel in panelList) {
            panel.SetActive((panel.name == panelName) ? true : false);
        }
    }
}
