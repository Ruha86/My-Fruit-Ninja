using UnityEngine;
using TMPro;

public class SlicerComboChecker : MonoBehaviour
{
    public GameObject ComboMultiplierRootGO;
    public float ComboIncreaseInterval = 1.1f;
    public int ComboMultiplierIncreaseStep = 3;
    private TMP_Text _comboMultiplierText;
    private float _comboTimer;
    private int _comboStep;
    private int _comboMultiplier;

    private Animation _comboMultiplierAnimation;
    private int _prevComboMultiplier;

    public void IncreaseComboStep()
    {
        SetComboStep(_comboStep + 1);
    }

    public int GetComboMultiplier()
    {
        return _comboMultiplier;
    }

    public void StopCombo() 
    {
        SetComboStep(0);
    }

    private void Start()
    {
        FillComponents();
        DropComboTimer();
        CalculateComboMultiplier(0);
    }

    private void FillComponents() 
    {
        _comboMultiplierText = ComboMultiplierRootGO.GetComponentInChildren<TMP_Text>();
        _comboMultiplierAnimation = ComboMultiplierRootGO.GetComponent<Animation>();
    }

    private void SetComboStep(int value) 
    {
        _comboStep = value;
        CalculateComboMultiplier(value);
        DropComboTimer();
    }

    private void DropComboTimer() 
    {
        _comboTimer = 0;
    }

    private void CalculateComboMultiplier(int comboStep) 
    {
        _comboMultiplier = 1 + comboStep / ComboMultiplierIncreaseStep;
        SetComboMultiplierText(_comboMultiplier);
        SetComboMultiplierShow(_comboMultiplier);
        ComboMultiplierIncreaseAnimation();
    }

    private void SetComboMultiplierText(int value) 
    {
        _comboMultiplierText.text = $"x{value}";
    }

    private void SetComboMultiplierShow(int value) 
    {
        bool needShow = value > 1;
        ComboMultiplierRootGO.SetActive(needShow);
    }

    private void Update()
    {
        MoveTimer();
    }

    private void MoveTimer() 
    {
        _comboTimer += Time.deltaTime;

        if (_comboTimer >= ComboIncreaseInterval)
        {
            StopCombo();
        }
    }

    private void ComboMultiplierIncreaseAnimation() 
    {
        if (_comboMultiplier > _prevComboMultiplier)
        {
            _comboMultiplierAnimation.Play(PlayMode.StopAll);
        }

        _prevComboMultiplier = _comboMultiplier;
    }
}
