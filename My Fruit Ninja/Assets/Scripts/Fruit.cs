using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject Whole;
    public GameObject Sliced;
    public Rigidbody TopPartRigidbody;
    public Rigidbody BottomPartRigidbody;

    private Rigidbody _mainRigidbody;
    private Collider _sliceTrigger;

    private void Start()
    {
        FillComponents();
    }

    private void FillComponents()
    {
        _mainRigidbody = GetComponent<Rigidbody>();

        _sliceTrigger = GetComponent<Collider>();
    }

    public void Slice(Vector3 direction, Vector3 position, float force) 
    {
        /*  
         *  Вызываем метод, который меняет состояние фрукта с целого на разрезанное
         *  Вызываем метод, который поворачивает половинки фрукта в заданном направлении
         *  Вызываем метод, который добавляет силу броска к верхней и нижней части разрезанного фрукта
         */
        SetSliced();
        RotateBySliceDirection(direction);
        AddForce(TopPartRigidbody, direction, position, force);
        AddForce(BottomPartRigidbody, direction, position, force);
    }

    private void SetSliced()
    {
        /*
         * Делаем целый фрукт неактивным
         * Делаем половинки фрукта активными
         * Отключаем коллайдер для разрезания
         */
        Whole.SetActive(false);
        Sliced.SetActive(true);
        _sliceTrigger.enabled = false;
    }

    private void RotateBySliceDirection(Vector3 direction)
    {
        /*
         * Вычислем угол поворота из направления отрезания
         * Применяем поворот к полловинкам фрукта
         */
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void AddForce(Rigidbody sliceRigidbody, Vector3 direction, Vector3 position, float force)
    {
        /*
         * Копируем линейную и угловую скорость целого фрукта
         * Прикладываем силу в заданных направлении и позиции
         */
        sliceRigidbody.velocity = _mainRigidbody.velocity;
        sliceRigidbody.angularVelocity = _mainRigidbody.angularVelocity;

        sliceRigidbody.AddForceAtPosition(direction * force, position);
    }
}
