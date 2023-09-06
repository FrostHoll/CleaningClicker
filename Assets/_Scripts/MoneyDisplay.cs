using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Отвечает за отрисовку кол-ва денег справа в углу
/// </summary>
public class MoneyDisplay : MonoBehaviour
{
    /// <summary>
    /// Текст, в который будем заносить деньги
    /// </summary>
    [SerializeField]
    private TMP_Text _moneyText;

    /// <summary>
    /// При появлении подписываемся на обновление кол-ва денег у игрока
    /// и сразу же выставляем текущие деньги
    /// </summary>
    private void Start()
    {
        Player.Instance.MoneyChanged += SetMoneyText;
        SetMoneyText(Player.Instance.Money);
    }

    /// <summary>
    /// Выставить текущие деньги
    /// </summary>
    /// <param name="money">Текущие деньги</param>
    private void SetMoneyText(Money money)
    {
        _moneyText.text = money.ToString();
    }
}
