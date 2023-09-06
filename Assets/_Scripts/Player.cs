using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// �������� �� ��� ������ ������
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// ������ �� ������������ ��������� ����� �������
    /// </summary>
    public static Player Instance;

    /// <summary>
    /// �������, ����� ���������� ����� ������ ����������
    /// </summary>
    public event Action<Money> MoneyChanged;

    /// <summary>
    /// �������, ����� ����� ���-�� �������
    /// </summary>
    public event Action Upgraded;

    /// <summary>
    /// ������� ���-�� ����� ������
    /// </summary>
    private Money _money = new Money();

    /// <summary>
    /// ������ �� ��� ������ ��������� ������
    /// </summary>
    private PlayerUpgrades _playerUpgrades;

    /// <summary>
    /// �������� �� ����� ������ � �������
    /// </summary>
    public bool HasWorkers => !_playerUpgrades.GetMoneyPerSecond(3).IsZero;

    /// <summary>
    /// �������� �� ����� ������ � ������
    /// </summary>
    public bool HasFactories => !_playerUpgrades.GetMoneyPerSecond(5).IsZero;

    /// <summary>
    /// ���� �� � ������ ������ ������� �������
    /// </summary>
    public bool HasHelpers => _playerUpgrades.GetHelpers() != 0;

    /// <summary>
    /// ��������� ������ � ������� ������, � ���� ��� ���������� ����������,
    /// �������� ����, ��� ��� ����������
    /// </summary>
    public Money Money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            MoneyChanged?.Invoke(_money);
        }
    }

    /// <summary>
    /// ��������� ������ � ���� ������������ ����������
    /// </summary>
    /// <returns>��� ���������</returns>
    public Upgrade[] GetUpgrades()
    {
        return _playerUpgrades.Upgrades;
    }

    /// <summary>
    /// ����������� �������� ������� ���� ��� ����� ���������;
    /// ���� ����������, ��������� ��������� � �������� ����, ��� ����� ���-�� �������
    /// </summary>
    /// <param name="upgrade">����������� ���������</param>
    /// <returns>���������� ��� ���</returns>
    public bool TryUpgrade(Upgrade upgrade)
    {
        if (upgrade.CurrentCost <= Money && upgrade.CurrentLevel < upgrade.Data.maxLevel)
        {
            var cost = upgrade.CurrentCost;
            if (_playerUpgrades.TryUpgrade(upgrade))
            {
                Money -= cost;
                Upgraded?.Invoke();
                return true;
            }

        }
        return false;
    }

    /// <summary>
    /// ����� ����� ������� �� ������, �������� � �������������� ������� ����� �� ����
    /// � ������� ����������� ����� � ���������� ������
    /// </summary>
    /// <param name="target">������, ����� �������� ���������� ����������� �����</param>
    public void OnTrashClicked(GameObject target)
    {
        var m = _playerUpgrades.GetMoneyPerClick();
        Instantiate(PopUp.PopUpPrefab, target.transform).Show("+" + m.ToString());
        Money += m;
    }

    /// <summary>
    /// ��� ��������� ��������� ������ ������������ ������,
    /// � ����� ������������� ������ �� ��������� ����� �������,
    /// ��������� ������� �� ���������� ��������� � ������ �������,
    /// �������� ������ �� ������ �� ����� ����������� � ������� ��� ��������� ��
    /// </summary>
    private void Awake()
    {
        if (PopUp.PopUpPrefab == null) PopUp.PopUpPrefab = Resources.Load<PopUp>("Prefabs/PopUp");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _playerUpgrades = GetComponent<PlayerUpgrades>();
            _playerUpgrades.LoadUpgrades();
            StartCoroutine(PassivePerSecond(3, GameController.Instance.WorkersDisplay));
            StartCoroutine(PassivePerSecond(5, GameController.Instance.FactoriesDisplay));
            StartCoroutine(HelpersCoroutine(GameController.Instance.HelpersDisplay));
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// �� ������ ������ ����� �������� ���� ����� (���������)
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Money += new Money(100d, 0);
        if (Input.GetKeyDown(KeyCode.W))
            Money += new Money(100d, 1);
        if (Input.GetKeyDown(KeyCode.E))
            Money += new Money(100d, 2);
        if (Input.GetKeyDown(KeyCode.R))
            Money += new Money(100d, 3);
        if (Input.GetKeyDown(KeyCode.T))
            Money += new Money(100d, 6);
    }

    /// <summary>
    /// ������ �� ���������� ��������� � ����������� �� ���������� ������
    /// </summary>
    /// <param name="seconds">����� �������</param>
    /// <param name="targetDisplay">������, ����� �������� ������� ����������� �����</param>
    private IEnumerator PassivePerSecond(int seconds, GameObject targetDisplay)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            var money = _playerUpgrades.GetMoneyPerSecond(seconds);
            if (!money.IsZero)
            {
                Money += money;
                Instantiate(PopUp.PopUpPrefab, targetDisplay.transform).Show("+" + money.ToString());
            }
        }
    }

    /// <summary>
    /// ������ ������� �������
    /// </summary>
    /// <param name="targetDisplay">������ ������� �������</param>
    private IEnumerator HelpersCoroutine(GameObject targetDisplay)
    {
        while(true) 
        {
            yield return new WaitForSeconds(8);
            var count = _playerUpgrades.GetHelpers();
            if (count != 0)
            {
                GameController.Instance.AttackRandomTargets(count);
                Instantiate(PopUp.PopUpPrefab, targetDisplay.transform).Show(count.ToString());
            }
        }
    }
}
