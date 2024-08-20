using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject FruitPrefab1;
    public GameObject FruitPrefab2;
    public GameObject FruitPrefab3;
    public GameObject FruitPrefab4;
    public GameObject FruitPrefab5;
    public GameObject BombPrefab;
    public DifficultyChanger DifficultyChanger;
    public GameObject HeartPrefab;
    public GameObject SandClockPrefab;
    private Collider _spawnZone;

    public float MinDelay = 0.2f;
    public float MaxDelay = 0.9f;
    public float AngleRangeZ = 20;
    public float LifeTime = 7f;
    public float MinForce = 15f;
    public float MaxForce = 25f;
    private float _currentDelay = 0;
    private bool _isActive = true;
    public float FruitWeight = 1f;
    public float MinBombWeight = 0.1f;
    public float MaxBombWeight = 0.25f;
    public float HeartWeight = 0.02f;
    public float SandClocksWeight = 0.04f;


    private void Start()
    {
        FillComponents();
        SetNewDelay();
    }

    private void FillComponents()
    {
        _spawnZone = GetComponent<Collider>();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // генерируем случайную точку в пределах зоны появления фруктов
        Vector3 pos;
        pos.x = Random.Range(_spawnZone.bounds.min.x, _spawnZone.bounds.max.x);
        pos.y = Random.Range(_spawnZone.bounds.min.y, _spawnZone.bounds.max.y);
        pos.z = Random.Range(_spawnZone.bounds.min.z, _spawnZone.bounds.max.z);
        return pos;
    }

    private void SetNewDelay()
    {
        _currentDelay = DifficultyChanger.CalculateRandomSpawnDelay(MinDelay, MaxDelay);
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        MoveDelay();
    }

    private void MoveDelay()
    {
        _currentDelay -= Time.deltaTime;

        if (_currentDelay < 0)
        {
            GameObject prefab = GetPrefabByWeights();
            SpawnObject(prefab);
            
            SetNewDelay();
        }
    }

    private void SpawnObject(GameObject prefab)
    {
        // Генерируем случайное начальное положение в зоне появления
        Vector3 startPosition = GetRandomSpawnPosition();

        // Генерируем случайное значение для угла вращения по оси Z
        Quaternion startRotation = Quaternion.Euler(0f, 0f, Random.Range(-AngleRangeZ, AngleRangeZ));

        // Создаём новый объект с заданным начальным положением
        GameObject newObject = Instantiate(prefab, startPosition, startRotation);

        // Удаляем объект через указанное время
        Destroy(newObject, LifeTime);

        // Добавляем силу броска
        AddForce(newObject);
    }

    private GameObject GetRandomFruitPrefab()
    {
        // Генерируем случайное число в пределах от 1 до 5 для выбора префаба фрукта
        int r = Random.Range(1, 6);

        if (r == 1)
        {
            return FruitPrefab1;
        }
        if (r == 2)
        {
            return FruitPrefab2;
        }
        if (r == 3)
        {
            return FruitPrefab3;
        }
        if (r == 4)
        {
            return FruitPrefab4;
        }
        else
        {
            return FruitPrefab5;
        }
    }

    public void AddForce(GameObject fruit)
    {
        /* Генерируем случайное значение силы в указаных пределах
           Бросаем фрукт в направлении вверх с указанной силой
        */
        float force = Random.Range(MinForce, MaxForce);
        fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);
    }

    public void Stop() 
    {
        _isActive = false;
    }

    public void Restart()
    {
        _isActive = true;
        SetNewDelay();
    }

    private GameObject GetPrefabByWeights() 
    {
        float bombWeight = DifficultyChanger.CalculateBombChance(MinBombWeight, MaxBombWeight);

        float totalWeight = FruitWeight + bombWeight + HeartWeight + SandClocksWeight;

        float random = Random.Range(0, totalWeight);

        if (random <= bombWeight)
        {
            return BombPrefab;
        }

        random -=bombWeight;

        if (random <= HeartWeight)
        {
            return HeartPrefab;
        }
        random -= HeartWeight;

        if (random <= SandClocksWeight)
        {
            return SandClockPrefab;
        }

        random -= SandClocksWeight;

        return GetRandomFruitPrefab();
    }

    private void SpawnObjects(GameObject prefab)
    {
        Vector3 startPosition = GetRandomSpawnPosition();

        Quaternion startRotation = Quaternion.Euler(0f, 0f, Random.Range(-AngleRangeZ, AngleRangeZ));

        GameObject newObject = Instantiate(prefab, startPosition, startRotation);

        Destroy(newObject, LifeTime);

        AddForce(newObject);
    }
}
