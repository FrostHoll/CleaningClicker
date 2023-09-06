using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clicker Smooth ProgressBar Script
/// Author: Sam Davidson
/// </summary>

// Скажешь, что взято у человека сверху

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private float _chipSpeed = 1.5f;

    [SerializeField]
    private Image _fillImage;

    private float _value;

    private float _maxValue;

    private float _fraction = 0f;

    private float _lerpTimer = 0f;

    public float ChipSpeed => MathF.Sqrt(_chipSpeed);

    private void Update()
    {
        float fill = _fillImage.fillAmount;
        if (fill != _fraction)
        {
            _lerpTimer += Time.deltaTime;
            float percent = _lerpTimer / _chipSpeed;
            percent *= percent;
            _fillImage.fillAmount = Mathf.Lerp(fill, _fraction, percent);
        }
    }

    public void UpdateValue(float newValue, float newMaxValue = -1f)
    {
        if (newMaxValue != -1f && newMaxValue != _maxValue)
        {
            _maxValue = newMaxValue;
            _value = Mathf.Clamp(newValue, 0f, _maxValue);
            if (!ApproximateEqual(newValue / newMaxValue, _fraction, 3))
            {
                _fraction = _value / _maxValue;
                _lerpTimer = 0f;
            }
        }
        else
        {
            _value = Mathf.Clamp(newValue, 0f, _maxValue);
            _fraction = _value / _maxValue;
            _lerpTimer = 0f;
        }
    }

    public void Init(float initValue, float maxValue)
    {
        if (_fillImage == null)
            _fillImage = GetComponent<Image>();
        _value = initValue;
        _maxValue = maxValue;
        _fraction = _value / _maxValue;
        _fillImage.fillAmount = _value / _maxValue;
    }

    private bool ApproximateEqual(float a, float b, int precision)
    {
        int a1 = Convert.ToInt32(a * Mathf.Pow(10, precision));
        int b1 = Convert.ToInt32(b * Mathf.Pow(10, precision));
        return a1 == b1;
    }
}
