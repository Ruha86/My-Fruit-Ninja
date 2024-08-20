using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public float NormalTimeScale = 1;
    public float SlowMotionTimeScale = 0.7f;
    private float _duration;
    private float _timer;

    public void StartSlow(float duration)
    {
        _duration = duration;
        _timer = 0;
        SetTimeScale(SlowMotionTimeScale);
    }

    public void Restart()
    {
        _duration = 0;
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private void Update()
    {
        MoveTimer();
    }

    private void MoveTimer()
    {
        if (_timer >= _duration)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer >= _duration) 
        {
            SetTimeScale(NormalTimeScale);
        }
    }
}
