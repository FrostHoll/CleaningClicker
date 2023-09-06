using System;

/// <summary>
/// Отвечает за текущие данные этого улучшения
/// </summary>
public class Upgrade
{
    /// <summary>
    /// Ссылка на шаблон с данными улучшения
    /// </summary>
    public UpgradeData Data { get; private set; }

    /// <summary>
    /// Текущий уровень улучшения
    /// </summary>
    public int CurrentLevel { get; private set; }

    /// <summary>
    /// Текущая сумма улучшения
    /// </summary>
    public Money CurrentValue { get; private set; }

    /// <summary>
    /// Текущая стоимость повышения уровня улучшения
    /// </summary>
    public Money CurrentCost { get; private set; }

    /// <summary>
    /// Событие, когда уровень этого улучшения был повышен
    /// </summary>
    public event Action<int, Money, Money> Upgraded;

    /// <summary>
    /// Задаем значения улучшения из шаблона
    /// </summary>
    /// <param name="data">Шаблон улучшения</param>
    public Upgrade(UpgradeData data)
    {
        Data = data;
        CurrentLevel = 0;
        CurrentValue = new Money(0d, 0);
        CurrentCost = new Money(data.initialCostAmount, data.initialCostType);
    }

    /// <summary>
    /// Пытаемся повысить уровень улучшения,
    /// если удачно, говорим пересчитать текущую сумму и стоимость улучшения
    /// и оповещаем всех, что уровень улучшения был повышен
    /// </summary>
    /// <returns>Получилось или нет</returns>
    public bool TryUpgrade()
    {
        if (CurrentLevel == Data.maxLevel) return false;
        CurrentLevel++;
        RecalculateCurrentValue();
        RecalculateCurrentCost();
        Upgraded?.Invoke(CurrentLevel, CurrentValue, CurrentCost);
        return true;
    }

    /// <summary>
    /// Пересчитываем текущую сумму улучшения по коэффициентам из шаблона
    /// </summary>
    private void RecalculateCurrentValue()
    {
        var m = new Money();
        m.Amount = Data.initialValueAmount * CurrentLevel * Data.valueScaleAmount;
        m.Type = (MoneyType)((int)m.Type + (int)Data.initialValueType + (int)(CurrentLevel * (float)Data.valueScaleType));
        CurrentValue = m;
    }

    /// <summary>
    /// Пересчитываем текущую стоимость повышения уровня улучшения по коэффициентам из шаблона
    /// </summary>
    private void RecalculateCurrentCost()
    {
        var m = new Money();
        m.Amount = Data.initialCostAmount * CurrentLevel * Data.costScaleAmount;
        m.Type = (MoneyType)((int)m.Type + (int)Data.initialCostType + (int)(CurrentLevel * Data.costScaleType));
        CurrentCost = m;
    }
}
