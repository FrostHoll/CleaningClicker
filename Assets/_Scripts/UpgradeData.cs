using UnityEngine;

/// <summary>
/// Шаблон данных улучшения
/// </summary>
[CreateAssetMenu(menuName = "Upgrade Data", fileName = "new UpgradeData")]
public class UpgradeData : ScriptableObject
{
    /// <summary>
    /// Позиция улучшения в списке
    /// </summary>
    public int positionInList;

    /// <summary>
    /// Ссылка на иконку улучшения
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// Название улучшения
    /// </summary>
    public string upgradeName;

    /// <summary>
    /// Описание улучшения
    /// </summary>
    public string upgradeDescription;

    /// <summary>
    /// Вид улучшения
    /// </summary>
    public UpgradeType upgradeType;

    /// <summary>
    /// Максимальный уровень улучшения
    /// </summary>
    public int maxLevel;

    /// <summary>
    /// Изначальная сумма улучшения
    /// </summary>
    public double initialValueAmount;

    /// <summary>
    /// Разряд изначальной суммы улучшения
    /// </summary>
    public MoneyType initialValueType;

    /// <summary>
    /// Изначальная стоимость улучшения
    /// </summary>
    public double initialCostAmount;

    /// <summary>
    /// Разряд изначальной стоимости улучшения
    /// </summary>
    public MoneyType initialCostType;

    /// <summary>
    /// Коэффициент увеличения суммы улучшения
    /// </summary>
    public double valueScaleAmount;

    /// <summary>
    /// Коэффициент увеличения разряда суммы улучшения
    /// </summary>
    public double valueScaleType;

    /// <summary>
    /// Коэффициент увеличения стоимости улучшения
    /// </summary>
    public double costScaleAmount;

    /// <summary>
    /// Коэффициент увеличения разряда стоимости улучшения
    /// </summary>
    public double costScaleType;
}
