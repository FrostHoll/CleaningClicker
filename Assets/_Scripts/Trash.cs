using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �������� �� ��������� ������
/// </summary>
public class Trash : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// ������ �� ������ ��������� ����� ������
    /// </summary>
    [SerializeField]
    private ProgressBar _progressBar;

    /// <summary>
    /// ������ �� ������ ������
    /// </summary>
    [SerializeField]
    private GameObject _icon;

    /// <summary>
    /// ������������ �������� ������
    /// </summary>
    [SerializeField]
    private int _maxHP;

    /// <summary>
    /// ������� �������� ������
    /// </summary>
    private int _hp;

    /// <summary>
    /// �������, ����� �� ����� ������ ��������
    /// </summary>
    private event Action<GameObject> Clicked;

    /// <summary>
    /// �������, ����� ���� ����� ��� ������
    /// </summary>
    public event Action<Trash> Died;

    /// <summary>
    /// ��������� ������� ������ ������
    /// </summary>
    private Vector3 _defaultPosition;

    /// <summary>
    /// � ������ ���������� ��� �������� �������� ������ � �����������
    /// ������ �� ������� �� ����� �� ����� ������
    /// </summary>
    private void Start()
    {       
        _maxHP = 100;
        _hp = _maxHP;
        _progressBar.Init(_hp, _maxHP);
        _progressBar.gameObject.SetActive(false);
        if (Clicked == null) Clicked += Player.Instance.OnTrashClicked;
    }

    /// <summary>
    /// ���������� ������� ������, �������� �������� � ��������� ���,
    /// �������� ��� �� ������ �������� �, ���� �� ��� ������, ������ ��� ������������ � ����� ��������,
    /// ����� ����������� �������� �������
    /// </summary>
    public void Attack()
    {
        _defaultPosition = transform.position;
        _hp -= 10;
        if (!_progressBar.gameObject.activeSelf) _progressBar.gameObject.SetActive(true);
        _progressBar.UpdateValue(_hp);
        Clicked?.Invoke(gameObject);
        if (_hp <= 0) HandleDeath();
        else
        {
            LeanTween.moveX(_icon, _defaultPosition.x + 5f, 0.01f).setOnComplete(() =>
            LeanTween.moveX(_icon, _defaultPosition.x - 5f, 0.02f).setOnComplete(() =>
            LeanTween.moveX(_icon, _defaultPosition.x, 0.01f)));
        }
    }

    /// <summary>
    /// �������� ����, ��� ����� ��� ������, ���������� ���� �� ������� ����� ������ �
    /// ���������� ���
    /// </summary>
    private void HandleDeath()
    {
        Clicked = null;
        Died?.Invoke(this);
        Died = null;
        Destroy(gameObject);
    }

    /// <summary>
    /// ����� �� ������ �������� ������, ������� ���
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Attack();
    }
}
