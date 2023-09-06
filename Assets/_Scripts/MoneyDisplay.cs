using System;
using TMPro;
using UnityEngine;

/// <summary>
/// �������� �� ��������� ���-�� ����� ������ � ����
/// </summary>
public class MoneyDisplay : MonoBehaviour
{
    /// <summary>
    /// �����, � ������� ����� �������� ������
    /// </summary>
    [SerializeField]
    private TMP_Text _moneyText;

    /// <summary>
    /// ��� ��������� ������������� �� ���������� ���-�� ����� � ������
    /// � ����� �� ���������� ������� ������
    /// </summary>
    private void Start()
    {
        Player.Instance.MoneyChanged += SetMoneyText;
        SetMoneyText(Player.Instance.Money);
    }

    /// <summary>
    /// ��������� ������� ������
    /// </summary>
    /// <param name="money">������� ������</param>
    private void SetMoneyText(Money money)
    {
        _moneyText.text = money.ToString();
    }
}
