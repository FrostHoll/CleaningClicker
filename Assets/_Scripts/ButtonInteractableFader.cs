using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за затемнение текста и иконок когда кнопка выключена
/// </summary>
public class ButtonInteractableFader : MonoBehaviour
{
    /// <summary>
    /// Кнопка, которая нас интересует
    /// </summary>
    [SerializeField]
    private Button _buttonTarget;

    /// <summary>
    /// Список объектов, которые надо затемнять
    /// </summary>
    [SerializeField]
    private GameObject[] _childrenTarget;

    /// <summary>
    /// Список изначальных цветов объектов, чтобы к ним впоследствии вернуться
    /// </summary>
    private Color[] _childrenInitColors;

    /// <summary>
    /// Последнее состояние кнопки, включена или нет
    /// </summary>
    private bool _currentInteractable = false;

    /// <summary>
    /// При появлении запоминаем изначальный цвет объектов, в зависимости картинка это или текст
    /// </summary>
    private void Start()
    {
        _childrenInitColors = new Color[_childrenTarget.Length];
        for (int i = 0; i < _childrenTarget.Length; i++)
        {
            if (_childrenTarget[i].TryGetComponent<Image>(out var im))
            {
                _childrenInitColors[i] = im.color;
            }
            else if (_childrenTarget[i].TryGetComponent<TMP_Text>(out var txt))
            {
                _childrenInitColors[i] = txt.color;
            }
        }
        UpdateColors();
    }

    /// <summary>
    /// Постоянно проверяем не изменилось ли состояние кнопки,
    /// и если изменилось, меняем цвета
    /// </summary>
    private void Update()
    {
        if (_currentInteractable != _buttonTarget.interactable) 
        {
            _currentInteractable = _buttonTarget.interactable;
            UpdateColors();
        }
    }

    /// <summary>
    /// Заменяем у объектов цвет на цвет затемнения кнопки, либо на их изначальный
    /// </summary>
    private void UpdateColors()
    {
        for (int i = 0; i < _childrenTarget.Length; i++)
        {
            if (_childrenTarget[i].TryGetComponent<Image>(out var im))
            {
                if (!_currentInteractable)
                    im.color = _buttonTarget.colors.disabledColor;
                else
                    im.color = _childrenInitColors[i];
            }
            else if (_childrenTarget[i].TryGetComponent<TMP_Text>(out var txt))
            {
                if (!_currentInteractable)
                    txt.color = _buttonTarget.colors.disabledColor;
                else
                    txt.color = _childrenInitColors[i];
            }
        }
    }
}
