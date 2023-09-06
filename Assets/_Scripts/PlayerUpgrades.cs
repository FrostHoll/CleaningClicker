using UnityEngine;

/// <summary>
/// �������� �� ������ ���� ���������
/// </summary>
public class PlayerUpgrades : MonoBehaviour
{
    /// <summary>
    /// ������ ���� ���������
    /// </summary>
    private Upgrade[] _upgrades;

    /// <summary>
    /// ��������� ������ � ������ ���� ���������
    /// </summary>
    public Upgrade[] Upgrades => _upgrades;

    /// <summary>
    /// ������� ����� �� ����
    /// </summary>
    private Money _currentPerClick;

    /// <summary>
    /// ������� ����� �� 3-� ���������� �������
    /// </summary>
    private Money _currentPer3Sec;

    /// <summary>
    /// ������� ����� �� 5-� ���������� �������
    /// </summary>
    private Money _currentPer5Sec;

    /// <summary>
    /// ������� ���������� ������� � �������
    /// </summary>
    private int _currentHelpers;

    /// <summary>
    /// �� ������ ������� ��������� ��� ������� ��������� � ������� �� � ������
    /// </summary>
    public void LoadUpgrades()
    {
        var datas = Resources.LoadAll<UpgradeData>("Upgrades");
        _upgrades = new Upgrade[datas.Length];
        foreach (var data in datas)
        {
            _upgrades[data.positionInList] = new Upgrade(data);
        }
        _currentPerClick = new Money(1d, 0);
        _currentPer3Sec = new Money(0, 0);
        _currentPer5Sec = new Money(0, 0);
        _currentHelpers = 0;
    }

    /// <summary>
    /// ���������� ������� ���������;
    /// ���� ����������, ������� ����������� ��� �����
    /// </summary>
    /// <param name="upgrade">����������� ���������</param>
    /// <returns>���������� ��� ���</returns>
    public bool TryUpgrade(Upgrade upgrade)
    {
        for (int index = 0; index < _upgrades.Length; index++)
        {
            if (upgrade == _upgrades[index])
            {
                var result = upgrade.TryUpgrade();
                RecalculateStats();
                return result;
            }
        }
        return false;
    }

    /// <summary>
    /// �������� ����� �� ����
    /// </summary>
    /// <returns>����� �� ����</returns>
    public Money GetMoneyPerClick()
    {
        return _currentPerClick;
    }

    /// <summary>
    /// �������� ����� �� �������
    /// </summary>
    /// <param name="second">���-�� ������</param>
    /// <returns>����� �� �������</returns>
    public Money GetMoneyPerSecond(int second)
    {
        switch (second)
        {
            case 3:
                return _currentPer3Sec;
            case 5:
                return _currentPer5Sec;
            default:
                return new Money(0, 0);
        }
    }

    /// <summary>
    /// �������� ���-�� ������� � �������
    /// </summary>
    /// <returns></returns>
    public int GetHelpers()
    {
        return _currentHelpers;
    }

    /// <summary>
    /// ������� ��� ����� � ��������� ��������� � ����������� ��� ����� � ������ ������� ���������
    /// </summary>
    private void RecalculateStats()
    {
        _currentPerClick = new Money(1d, 0);
        _currentPer3Sec = new Money(0, 0);
        _currentPer5Sec = new Money(0, 0);
        _currentHelpers = 0;
        var helpersRaw = new Money(0, 0);
        foreach (var up in _upgrades)
        {
            if (up.Data.upgradeType == UpgradeType.PerClick)
                _currentPerClick += up.CurrentValue;
            if (up.Data.upgradeType == UpgradeType.PassivePer3Sec)
                _currentPer3Sec += up.CurrentValue;
            if (up.Data.upgradeType == UpgradeType.PassivePer5Sec)
                _currentPer5Sec += up.CurrentValue;
            if (up.Data.upgradeType == UpgradeType.Helper)
                helpersRaw += up.CurrentValue;
        }
        _currentHelpers = (int)helpersRaw.Amount;
    }
}
