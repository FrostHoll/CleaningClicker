using System;

/// <summary>
/// �������� �� ������� ������ ����� ���������
/// </summary>
public class Upgrade
{
    /// <summary>
    /// ������ �� ������ � ������� ���������
    /// </summary>
    public UpgradeData Data { get; private set; }

    /// <summary>
    /// ������� ������� ���������
    /// </summary>
    public int CurrentLevel { get; private set; }

    /// <summary>
    /// ������� ����� ���������
    /// </summary>
    public Money CurrentValue { get; private set; }

    /// <summary>
    /// ������� ��������� ��������� ������ ���������
    /// </summary>
    public Money CurrentCost { get; private set; }

    /// <summary>
    /// �������, ����� ������� ����� ��������� ��� �������
    /// </summary>
    public event Action<int, Money, Money> Upgraded;

    /// <summary>
    /// ������ �������� ��������� �� �������
    /// </summary>
    /// <param name="data">������ ���������</param>
    public Upgrade(UpgradeData data)
    {
        Data = data;
        CurrentLevel = 0;
        CurrentValue = new Money(0d, 0);
        CurrentCost = new Money(data.initialCostAmount, data.initialCostType);
    }

    /// <summary>
    /// �������� �������� ������� ���������,
    /// ���� ������, ������� ����������� ������� ����� � ��������� ���������
    /// � ��������� ����, ��� ������� ��������� ��� �������
    /// </summary>
    /// <returns>���������� ��� ���</returns>
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
    /// ������������� ������� ����� ��������� �� ������������� �� �������
    /// </summary>
    private void RecalculateCurrentValue()
    {
        var m = new Money();
        m.Amount = Data.initialValueAmount * CurrentLevel * Data.valueScaleAmount;
        m.Type = (MoneyType)((int)m.Type + (int)Data.initialValueType + (int)(CurrentLevel * (float)Data.valueScaleType));
        CurrentValue = m;
    }

    /// <summary>
    /// ������������� ������� ��������� ��������� ������ ��������� �� ������������� �� �������
    /// </summary>
    private void RecalculateCurrentCost()
    {
        var m = new Money();
        m.Amount = Data.initialCostAmount * CurrentLevel * Data.costScaleAmount;
        m.Type = (MoneyType)((int)m.Type + (int)Data.initialCostType + (int)(CurrentLevel * Data.costScaleType));
        CurrentCost = m;
    }
}
