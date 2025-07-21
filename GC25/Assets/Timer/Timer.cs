using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [SerializeField] private float _elapsedTime = 0f;
    private bool _isRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        LoadTimerData();
        _isRunning = true;
    }

    public void StopTimer() => _isRunning = false;

    public float GetCurrentTime() => _elapsedTime;

    public bool IsRunning() => _isRunning;

    private void LoadTimerData()
    {
        GameData gameData = SaveGame.LoadGameData();
        if (gameData == null) return;

        float savedTime = gameData.time;
        if (savedTime > 0) _elapsedTime = savedTime;
    }
}
