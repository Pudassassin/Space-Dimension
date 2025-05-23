using UnityEngine;
using UnityEngine.UI;

public class GameplayUIUpdater : MonoBehaviour
{
    public Text hpText;
    public Text timerText;
    public Text scoreText;

    private MainLogic mainLogic;

    void Start()
    {
        mainLogic = FindObjectOfType<MainLogic>();
    }

    void Update()
    {
        if (mainLogic == null) return;

        int hp = Mathf.Max(mainLogic.GetHP(), 0);
        hpText.text = $"HP\n{hp}";

        float time = Mathf.Max(mainLogic.GetTimeRemaining(), 0);
        timerText.text = $"Time\n{time:F1}s";

        scoreText.text = $"Score\n{mainLogic.GetScore()}";
    }
}
