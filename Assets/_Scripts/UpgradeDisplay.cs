using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������� �� ����������� �������� � ��������� � ���� � ������ ���������
/// </summary>
public class UpgradeDisplay : MonoBehaviour
{
    /// <summary>
    /// ������ ����������� ������
    /// </summary>
    [SerializeField]
    private Image _icon;

    /// <summary>
    /// ����� �������� ���������
    /// </summary>
    [SerializeField]
    private TMP_Text _upgradeNameText;

    /// <summary>
    /// ����� �������� ���������
    /// </summary>
    [SerializeField]
    private TMP_Text _upgradeDescriptionText;

    /// <summary>
    /// ����� ��������� ���������
    /// </summary>
    [SerializeField]
    private TMP_Text _costText;

    /// <summary>
    /// ������ ��������� ������ ���������
    /// </summary>
    [SerializeField]
    private Button _upgradeButton;

    /// <summary>
    /// ����� �������� ������ ���������
    /// </summary>
    [SerializeField]
    private TMP_Text _levelText;

    /// <summary>
    /// ������ �� ���������, ������� ������������ � ���� ����
    /// </summary>
    private Upgrade _upgrade;

    /// <summary>
    /// ����� �� ������ �������� ������� ���������
    /// </summary>
    public bool ReadyToUpgrade => _upgradeButton.interactable;

    /// <summary>
    /// ��������� ���� ���������� ��������� ��� �����������;
    /// ������������� �� ������� ��������� ������ ���������
    /// � ��������� ������� ����� ������
    /// </summary>
    /// <param name="upgrade">���������</param>
    public void Init(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _icon.sprite = upgrade.Data.icon;
        _upgradeNameText.text = upgrade.Data.upgradeName;
        _upgradeDescriptionText.text = upgrade.Data.upgradeDescription + $" <color=#50b2f4>{upgrade.CurrentValue}</color>";
        _costText.text = upgrade.CurrentCost.ToString();
        _levelText.text = upgrade.CurrentLevel.ToString();
        upgrade.Upgraded += OnUpgraded;
        Player.Instance.MoneyChanged += OnMoneyChanged;
        CheckReady();
    }

    /// <summary>
    /// �������� �������� ������� ���������, ���� ������� �������������,
    /// ����� �� ��� ��� ��������
    /// </summary>
    public void TryUpgrade()
    {
        if (Player.Instance.TryUpgrade(_upgrade))
        {
            CheckReady();
        }
    }

    /// <summary>
    /// ����� ������� ��������� ��� �������, ��������� ��� ������ ������ �������
    /// </summary>
    /// <param name="newLevel">����� �������</param>
    /// <param name="newValue">����� ����� ���������</param>
    /// <param name="newCost">����� ��������� ���������</param>
    private void OnUpgraded(int newLevel, Money newValue, Money newCost)
    {
        _upgradeDescriptionText.text = _upgrade.Data.upgradeDescription + $" <color=#50b2f4>{newValue}</color>";
        _costText.text = newCost.ToString();
        if (newLevel == _upgrade.Data.maxLevel) _levelText.text = "MAX";
        else _levelText.text = newLevel.ToString();
    }

    /// <summary>
    /// ����� ���-�� ����� ������ ����������, ��������� ������� �� �� �� ���������
    /// </summary>
    /// <param name="newMoney">����� ������ ������</param>
    private void OnMoneyChanged(Money newMoney) 
    {
        _upgradeButton.interactable = _upgrade.CurrentCost <= newMoney && _upgrade.CurrentLevel < _upgrade.Data.maxLevel;
    }

    /// <summary>
    /// �������� ������ ���������, ���� ������ ����� �������� �������
    /// </summary>
    private void CheckReady()
    {
        _upgradeButton.interactable = _upgrade.CurrentCost <= Player.Instance.Money 
            && _upgrade.CurrentLevel < _upgrade.Data.maxLevel;
    }
}
