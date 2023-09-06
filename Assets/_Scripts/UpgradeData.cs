using UnityEngine;

/// <summary>
/// ������ ������ ���������
/// </summary>
[CreateAssetMenu(menuName = "Upgrade Data", fileName = "new UpgradeData")]
public class UpgradeData : ScriptableObject
{
    /// <summary>
    /// ������� ��������� � ������
    /// </summary>
    public int positionInList;

    /// <summary>
    /// ������ �� ������ ���������
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// �������� ���������
    /// </summary>
    public string upgradeName;

    /// <summary>
    /// �������� ���������
    /// </summary>
    public string upgradeDescription;

    /// <summary>
    /// ��� ���������
    /// </summary>
    public UpgradeType upgradeType;

    /// <summary>
    /// ������������ ������� ���������
    /// </summary>
    public int maxLevel;

    /// <summary>
    /// ����������� ����� ���������
    /// </summary>
    public double initialValueAmount;

    /// <summary>
    /// ������ ����������� ����� ���������
    /// </summary>
    public MoneyType initialValueType;

    /// <summary>
    /// ����������� ��������� ���������
    /// </summary>
    public double initialCostAmount;

    /// <summary>
    /// ������ ����������� ��������� ���������
    /// </summary>
    public MoneyType initialCostType;

    /// <summary>
    /// ����������� ���������� ����� ���������
    /// </summary>
    public double valueScaleAmount;

    /// <summary>
    /// ����������� ���������� ������� ����� ���������
    /// </summary>
    public double valueScaleType;

    /// <summary>
    /// ����������� ���������� ��������� ���������
    /// </summary>
    public double costScaleAmount;

    /// <summary>
    /// ����������� ���������� ������� ��������� ���������
    /// </summary>
    public double costScaleType;
}
