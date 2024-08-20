using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int StartHealth = 3;
    private TMP_Text _healthText;
    private int _currentHealth;

    private void Start()
    {
        FillComponents();
        SetHealth(StartHealth);
    }

    private void FillComponents()
    {
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RemoveHealth()
    {
        SetHealth(_currentHealth  -1);
    }

    public void AddHealth(int value)
    {
        SetHealth(_currentHealth + value);
    }

    private void SetHealth(int value)
    {
        _currentHealth = value;
        SetHealthText(value);
    }

    private void SetHealthText(int value)
    {
        _healthText.text = value.ToString();
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void Restart() 
    {
        SetHealth(StartHealth);
    }
}
