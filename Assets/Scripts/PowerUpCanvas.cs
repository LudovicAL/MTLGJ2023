using System.Collections;
using System.Collections.Generic;
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

        PauseGame();

        foreach (ScriptablePowerUp scriptablePowerUp in scriptablePowerUps) {
            GameObject panelModelShowcase = Instantiate(panelModelShowcasePrefab, panelHorizontalLayout);
            Transform panelRotationTransform = panelModelShowcase.transform.Find("Panel Rotation");
            GameObject model = Instantiate(scriptablePowerUp.model, panelRotationTransform);
            model.transform.localScale = new Vector3(50, 50, 50);
            Button button = panelModelShowcase.GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate {
                Debug.Log("You chose " + scriptablePowerUp.powerUpName);
                powerUpsManager.ActivatePowerUp(scriptablePowerUp);
                ResumeGame();
            });
        }
    }

    public void PauseGame() {
        TweenManager.instance.StopTween(lastTweenId);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomInDefault, null);
    }

    public void ResumeGame() {
        TweenManager.instance.StopTween(lastTweenId);
        lastTweenId = TweenManager.instance.TweenScale(panelPowerUpTransform, TweenManager.instance.tweenZoomOutDefault, null);
    }
}
