using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������� �� ���������� ������ � ������ ����� ������ ���������
/// </summary>
public class ButtonInteractableFader : MonoBehaviour
{
    /// <summary>
    /// ������, ������� ��� ����������
    /// </summary>
    [SerializeField]
    private Button _buttonTarget;

    /// <summary>
    /// ������ ��������, ������� ���� ���������
    /// </summary>
    [SerializeField]
    private GameObject[] _childrenTarget;

    /// <summary>
    /// ������ ����������� ������ ��������, ����� � ��� ������������ ���������
    /// </summary>
    private Color[] _childrenInitColors;

    /// <summary>
    /// ��������� ��������� ������, �������� ��� ���
    /// </summary>
    private bool _currentInteractable = false;

    /// <summary>
    /// ��� ��������� ���������� ����������� ���� ��������, � ����������� �������� ��� ��� �����
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
    /// ��������� ��������� �� ���������� �� ��������� ������,
    /// � ���� ����������, ������ �����
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
    /// �������� � �������� ���� �� ���� ���������� ������, ���� �� �� �����������
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
