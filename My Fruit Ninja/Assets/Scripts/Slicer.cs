using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    public Score Score;
    public Health Health;
    public GameEnder GameEnder;
    public SlicerComboChecker SlicerComboChecker;
    public SlowMotion SlowMotion;

    public float SliceForce = 65;
    private const float MinSlicingMove = 0.01f; // минимальное значение для проверки, двикался ли резак
    private Collider _sliceTrigger;
    private Camera _mainCamera;
    private Vector3 _direction; // Направление движения резака

    public AudioClip ScoreSound;
    public float ScoreSoundVolume = 0.45f;

    public AudioClip BombSound;
    public float BombSoundVolume = 0.3f;

    public AudioClip BonusSound;
    public float BonusSoundVolume = 0.7f;

    private AudioSource _soundPlayer;

    public Animation FlashAnimation;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _soundPlayer = GetComponentInChildren<AudioSource>();
        _sliceTrigger = GetComponent<Collider>();
        _mainCamera = Camera.main;
        SetSlicing(false);
    }

    private void Update()
    {
        Slicing();
    }

    private void Slicing()
    {
        /* Если нажата левая кнопка мыши, слайсер активен, если кнопка отпущена, слайсер не активен
         */
        if (Input.GetMouseButton(0))
        {
            RefreshSlicing();
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            SetSlicing(false);
        }
    }

    private void SetSlicing(bool value)
    {
        // Включаем и выключаем коллайдер в звисимости от value
        _sliceTrigger.enabled = value;
    }

    private void RefreshSlicing()
    {
        Vector3 targetPosition = GetTargetPosition();
        RefreshDirection(targetPosition);
        MoveSlicer(targetPosition);
        bool isSlicing = CheckMoreThenMove(_direction);
        SetSlicing(isSlicing);
    }

    private Vector3 GetTargetPosition()
    {
        /*  Преобразуем координаты курсора на экране в мировые координаты
         *  Задаем значение Z-координаты цели равным 0, чтобы цель была на одной плоскости с резаком
         *  Возвращаем полученную позицию цели
         */
        Vector3 targetPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = -10;
        return targetPosition;
    }

    private void RefreshDirection(Vector3 targetPosition)
    {
        // Вычисляем вектор направления, который указывает на цель (позицию курсора) от текущей позиции резака
        _direction = targetPosition - transform.position;
    }

    private void MoveSlicer(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private bool CheckMoreThenMove(Vector3 direction)
    {
        float slicingSpeed = direction.magnitude / Time.deltaTime;
        return slicingSpeed >= MinSlicingMove;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckFriut(other);
        CheckBomb(other);
        CheckHeart(other);
        CheckSandClocks(other);
    }

    private void CheckFriut(Collider other)
    {
        // Создаём переменную для фрукта, которого мы коснулись
        Fruit fruit = other.GetComponent<Fruit>();

        // Проверяем, является ли объект фруктом
        if (fruit == null)
        {
            // Если объект — не фрукт, выходим из метода
            return;
        }

        // Режем фрукт в заданном направлении с учётом позиции курсора и силы разрезания
        fruit.Slice(_direction, transform.position, SliceForce);

        SlicerComboChecker.IncreaseComboStep();
        int scoreByFruit = 1 * SlicerComboChecker.GetComboMultiplier();

        // Получаем одно очко
        Score.AddScore(1);
        _soundPlayer.PlayOneShot(ScoreSound, ScoreSoundVolume);
    }

    private void CheckBomb(Collider other)
    {
        // Создаём переменную для бомбы, которой мы коснулись
        Bomb bomb = other.GetComponent<Bomb>();

        // Проверяем, является ли объект бомбой
        // Здесь также можно написать if (!bomb)
        if (bomb == null)
        {
            // Если объект — не бомба, выходим из метода
            return;
        }

        // Уничтожаем игровой объект бомбы
        Destroy(bomb.gameObject);

        // Теряем одну жизнь
        Health.RemoveHealth();
        CheckHealthEnd(Health.GetCurrentHealth());

        SlicerComboChecker.StopCombo();
        _soundPlayer.PlayOneShot(BombSound, BombSoundVolume);
        FlashAnimation.Play(PlayMode.StopAll);
    }

    private void CheckHealthEnd(int health)
    {
        // Если количество жизней больше нуля
        if (health > 0)
        {
            // Возвращаемся из метода, игра продолжается
            return;
        }

        // Иначе вызываем метод StopGame()
        StopGame();
    }

    private void CheckHeart(Collider other)
    {
        Heart heart = other.GetComponentInParent<Heart>();

        if (heart == null)
        {
            return;
        }

        int healthForHeart = heart.HealthForHeart;
        heart.ShowSliceParticles();

        Destroy(heart.gameObject);

        SlicerComboChecker.IncreaseComboStep();
        Health.AddHealth(healthForHeart);
        _soundPlayer.PlayOneShot(BonusSound, BonusSoundVolume);
    }

    private void CheckSandClocks(Collider other)
    {
        SandClocks sandClocks = other.GetComponent<SandClocks>();

        if (sandClocks == null)
        {
            return;
        }

        float slowDuration = sandClocks.SlowDuration;
        sandClocks.ShowSliceParticles();

        Destroy(sandClocks.gameObject);

        SlicerComboChecker.IncreaseComboStep();
        SlowMotion.StartSlow(slowDuration);
        _soundPlayer.PlayOneShot(BonusSound, BonusSoundVolume);
    }

    private void StopGame()
    {
        // Вызываем метод Stop() из скрипта FruitSpawner
        GameEnder.EndGame();
    }
}
