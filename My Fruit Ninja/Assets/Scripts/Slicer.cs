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
    private const float MinSlicingMove = 0.01f; // ����������� �������� ��� ��������, �������� �� �����
    private Collider _sliceTrigger;
    private Camera _mainCamera;
    private Vector3 _direction; // ����������� �������� ������

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
        /* ���� ������ ����� ������ ����, ������� �������, ���� ������ ��������, ������� �� �������
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
        // �������� � ��������� ��������� � ���������� �� value
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
        /*  ����������� ���������� ������� �� ������ � ������� ����������
         *  ������ �������� Z-���������� ���� ������ 0, ����� ���� ���� �� ����� ��������� � �������
         *  ���������� ���������� ������� ����
         */
        Vector3 targetPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = -10;
        return targetPosition;
    }

    private void RefreshDirection(Vector3 targetPosition)
    {
        // ��������� ������ �����������, ������� ��������� �� ���� (������� �������) �� ������� ������� ������
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
        // ������ ���������� ��� ������, �������� �� ���������
        Fruit fruit = other.GetComponent<Fruit>();

        // ���������, �������� �� ������ �������
        if (fruit == null)
        {
            // ���� ������ � �� �����, ������� �� ������
            return;
        }

        // ����� ����� � �������� ����������� � ������ ������� ������� � ���� ����������
        fruit.Slice(_direction, transform.position, SliceForce);

        SlicerComboChecker.IncreaseComboStep();
        int scoreByFruit = 1 * SlicerComboChecker.GetComboMultiplier();

        // �������� ���� ����
        Score.AddScore(1);
        _soundPlayer.PlayOneShot(ScoreSound, ScoreSoundVolume);
    }

    private void CheckBomb(Collider other)
    {
        // ������ ���������� ��� �����, ������� �� ���������
        Bomb bomb = other.GetComponent<Bomb>();

        // ���������, �������� �� ������ ������
        // ����� ����� ����� �������� if (!bomb)
        if (bomb == null)
        {
            // ���� ������ � �� �����, ������� �� ������
            return;
        }

        // ���������� ������� ������ �����
        Destroy(bomb.gameObject);

        // ������ ���� �����
        Health.RemoveHealth();
        CheckHealthEnd(Health.GetCurrentHealth());

        SlicerComboChecker.StopCombo();
        _soundPlayer.PlayOneShot(BombSound, BombSoundVolume);
        FlashAnimation.Play(PlayMode.StopAll);
    }

    private void CheckHealthEnd(int health)
    {
        // ���� ���������� ������ ������ ����
        if (health > 0)
        {
            // ������������ �� ������, ���� ������������
            return;
        }

        // ����� �������� ����� StopGame()
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
        // �������� ����� Stop() �� ������� FruitSpawner
        GameEnder.EndGame();
    }
}
