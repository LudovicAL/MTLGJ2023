using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OfferPowerUps(PowerUpsManager powerUpsManager) {
        List<ScriptablePowerUp> scriptablePowerUps = powerUpsManager.GetRandomPowerUps();
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
                powerUpsManager.ActivatePowerUp(scriptablePowerUp);
                ResumeGame();
            });
        }
    }

    private void PauseGame() {
        TweenManager.instance.StopTween(lastTweenId);
        panelPowerUpTransform.gameObject.SetActive(true);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomInDefault, null);
    }

    private void ResumeGame() {
        TweenManager.instance.StopTween(lastTweenId);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomOutDefault, DeactivatePanelPowerUps);
    }

    public void DeactivatePanelPowerUps() {
        panelPowerUpTransform.gameObject.SetActive(false);
    }
}
