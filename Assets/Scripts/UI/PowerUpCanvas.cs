using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpCanvas : Singleton<PowerUpCanvas> {

    public static PowerUpCanvas instance { get; private set; }

    [SerializeField]
    private Transform panelPowerUpTransform;
    [SerializeField]
    private Transform panelHorizontalLayout;
    [SerializeField]
    private GameObject panelModelShowcasePrefab;

    private int lastTweenId = -1;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }
    
    void OnEnable() {
        PlayerLevelManager.Instance.playerLeveledUpEvent.AddListener(OfferPowerUps);
    }
    
    void OnDisable() {
        PlayerLevelManager.Instance.playerLeveledUpEvent.RemoveListener(OfferPowerUps);
    }

    public void OfferPowerUps() {
        List<PowerUp> offeredPowerUps = PowerUpsManager.Instance.GetRandomPowerUps(3);
        if (offeredPowerUps.Count == 0) {
            Debug.Log("There are no more powerups for this vehicule");
            ReturnToStartedGameState();
            return;
        }

        foreach (Transform transform in panelHorizontalLayout) {
            Destroy(transform.gameObject);
        }

        PauseGame();

        foreach (PowerUp offeredPowerUp in offeredPowerUps) {
            GameObject panelModelShowcase = Instantiate(panelModelShowcasePrefab, panelHorizontalLayout);
            TextMeshProUGUI textMeshProUGUI = panelModelShowcase.transform.Find("Text PowerUpName").GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = offeredPowerUp.powerUpData.powerUpName;
            Transform panelRotationTransform = panelModelShowcase.transform.Find("Panel Rotation");
            GameObject model = Instantiate(offeredPowerUp.models[0], panelRotationTransform);
            model.transform.localScale = new Vector3(100, 100, 100);
            model.SetActive(true);
            Button button = panelModelShowcase.GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate {
                Debug.Log("You chose " + offeredPowerUp.powerUpData.powerUpName);
                PowerUpsManager.Instance.AcquirePowerUp(offeredPowerUp);
                ResumeGame();
            });
        }
    }

    private void PauseGame() {
        TweenManager.instance.StopTween(lastTweenId);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomInDefault, null);
        panelPowerUpTransform.localScale = Vector3.zero;
        panelPowerUpTransform.gameObject.SetActive(true);
        
    }

    private void ResumeGame() {
        TweenManager.instance.StopTween(lastTweenId);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomOutDefault, ReturnToStartedGameState);
    }

    public void ReturnToStartedGameState() {
        GameManaging.Instance.RequestGameStateChange(GameState.Started);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PowerUpCanvas))]
class PowerUpsCanvasEditor : Editor {
    public override void OnInspectorGUI() {
        if (GUILayout.Button("GetPowerUpsOffer")) {
            PowerUpCanvas.instance.OfferPowerUps();
        }
        DrawDefaultInspector();
    }
}
#endif
