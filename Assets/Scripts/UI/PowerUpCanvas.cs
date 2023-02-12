using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCanvas : MonoBehaviour {

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

    // Start is called before the first frame update
    void Start() {
        PlayerLevelManager.Instance.playerLeveledUpEvent.AddListener(OfferPowerUps);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OfferPowerUps() {
        List<ScriptablePowerUp> scriptablePowerUps = PowerUpsManager.Instance.GetRandomPowerUps();
        if (scriptablePowerUps.Count == 0) {
            Debug.Log("There are no more powerups for this vehicule");
            return;
        }

        foreach (Transform transform in panelHorizontalLayout) {
            Destroy(transform.gameObject);
        }

        PauseGame();

        foreach (ScriptablePowerUp scriptablePowerUp in scriptablePowerUps) {
            GameObject panelModelShowcase = Instantiate(panelModelShowcasePrefab, panelHorizontalLayout);
            TextMeshProUGUI textMeshProUGUI = panelModelShowcase.transform.Find("Text PowerUpName").GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = scriptablePowerUp.powerUpName;
            Transform panelRotationTransform = panelModelShowcase.transform.Find("Panel Rotation");
            GameObject model = Instantiate(scriptablePowerUp.model, panelRotationTransform);
            model.transform.localScale = new Vector3(scriptablePowerUp.modelScaleMultiplier, scriptablePowerUp.modelScaleMultiplier, scriptablePowerUp.modelScaleMultiplier);
            Button button = panelModelShowcase.GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate {
                Debug.Log("You chose " + scriptablePowerUp.powerUpName);
                PowerUpsManager.Instance.ActivatePowerUp(scriptablePowerUp);
                ResumeGame();
            });
        }
    }

    private void PauseGame() {
        TweenManager.instance.StopTween(lastTweenId);
        panelPowerUpTransform.gameObject.SetActive(true);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomInDefault, null);
        Time.timeScale = 0;
    }

    private void ResumeGame() {
        TweenManager.instance.StopTween(lastTweenId);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomOutDefault, DeactivatePanelPowerUps);
        Time.timeScale = 1;
    }

    public void DeactivatePanelPowerUps() {
        panelPowerUpTransform.gameObject.SetActive(false);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PowerUpCanvas))]
class PowerUpsCanvasEditor : Editor {
    public override void OnInspectorGUI() {
        if (GUILayout.Button("GetPowerUpsOffer")) {
            PowerUpCanvas.instance.OfferPowerUps();
        }
    }
}
#endif
