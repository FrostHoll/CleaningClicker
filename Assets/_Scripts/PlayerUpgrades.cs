using UnityEngine;

/// <summary>
/// Отвечает за данные всех улучшений
/// </summary>
public class PlayerUpgrades : MonoBehaviour
{
    /// <summary>
    /// Список всех улучшений
    /// </summary>
    private Upgrade[] _upgrades;

    /// <summary>
    /// Открываем доступ к списку всех улучшений
    /// </summary>
    public Upgrade[] Upgrades => _upgrades;

    /// <summary>
    /// Текущая сумма за клик
    /// </summary>
    private Money _currentPerClick;

    /// <summary>
    /// Текущая сумма по 3-х секундному таймеру
    /// </summary>
    private Money _currentPer3Sec;

    /// <summary>
    /// Текущая сумма по 5-и секундному таймеру
    /// </summary>
    private Money _currentPer5Sec;

    /// <summary>
    /// Текущее количество рабочих в команде
    /// </summary>
    private int _currentHelpers;

    /// <summary>
    /// Из файлов проекта загружаем все шаблоны улучшений и заносим их в список
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
    /// Попытаться сделать улучшение;
    /// Если получилось, говорим пересчитать все суммы
    /// </summary>
    /// <param name="upgrade">Необходимое улучшение</param>
    /// <returns>Получилось или нет</returns>
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
    /// Получить сумму за клик
    /// </summary>
    /// <returns>Сумма за клик</returns>
    public Money GetMoneyPerClick()
    {
        return _currentPerClick;
    }

    /// <summary>
    /// Получить сумму по таймеру
    /// </summary>
    /// <param name="second">Кол-во секунд</param>
    /// <returns>Сумма по таймеру</returns>
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
    /// Получить кол-во рабочих в команде
    /// </summary>
    /// <returns></returns>
    public int GetHelpers()
    {
        return _currentHelpers;
    }

    /// <summary>
    /// Вернуть все суммы к начальным значениям и пересчитать все суммы с учетом текущих улучшений
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
