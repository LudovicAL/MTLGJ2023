using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartingCanvas : MonoBehaviour {

    [SerializeField]
    private int countDownDurationInSeconds;
    private TextMeshProUGUI countDownText;
    private int countDownId = 0;
    private float countDownStartingTime;

    // Start is called before the first frame update
    void Start() {
        countDownText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnEnable() {
        if (countDownText != null) {
            countDownStartingTime = Time.realtimeSinceStartup;
            StartCoroutine(CountDown(countDownId));
        }
    }

    private void OnDisable() {
        countDownId++;
    }


    private IEnumerator CountDown(int id) {
        while (id == countDownId && (Time.realtimeSinceStartup - countDownStartingTime) < countDownDurationInSeconds) {
            countDownText.text = (Mathf.CeilToInt(countDownDurationInSeconds - (Time.realtimeSinceStartup - countDownStartingTime))).ToString();
            yield return null;
        }
        if (id == countDownId) {
            GameManaging.Instance.RequestGameStateChange(GameState.Started);
        }
    }

}
