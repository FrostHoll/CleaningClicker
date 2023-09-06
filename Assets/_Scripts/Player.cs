using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Отвечает за все данные игрока
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// Ссылка на единственный экземпляр этого скрипта
    /// </summary>
    public static Player Instance;

    /// <summary>
    /// Событие, когда количество денег игрока изменилось
    /// </summary>
    public event Action<Money> MoneyChanged;

    /// <summary>
    /// Событие, когда игрок что-то улучшил
    /// </summary>
    public event Action Upgraded;

    /// <summary>
    /// Текущее кол-во денег игрока
    /// </summary>
    private Money _money = new Money();

    /// <summary>
    /// Ссылка на все данные улучшений игрока
    /// </summary>
    private PlayerUpgrades _playerUpgrades;

    /// <summary>
    /// Получает ли игрок деньги с рабочих
    /// </summary>
    public bool HasWorkers => !_playerUpgrades.GetMoneyPerSecond(3).IsZero;

    /// <summary>
    /// Получает ли игрок деньги с фабрик
    /// </summary>
    public bool HasFactories => !_playerUpgrades.GetMoneyPerSecond(5).IsZero;

    /// <summary>
    /// Есть ли у игрока личная команда рабочих
    /// </summary>
    public bool HasHelpers => _playerUpgrades.GetHelpers() != 0;

    /// <summary>
    /// Открываем доступ к деньгам игрока, и если это количество изменилось,
    /// сообщаем всем, что оно изменилось
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
    /// Открывает доступ к всем существующим улучшениям
    /// </summary>
    /// <returns>Все улучшения</returns>
    public Upgrade[] GetUpgrades()
    {
        return _playerUpgrades.Upgrades;
    }

    /// <summary>
    /// Попробовать повысить уровень тому или иному улучшению;
    /// Если получилось, списываем стоимость и сообщаем всем, что игрок что-то улучшил
    /// </summary>
    /// <param name="upgrade">Необходимое улучшение</param>
    /// <returns>Получилось или нет</returns>
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
    /// Когда игрок кликнул по мусору, получаем и приплюсовываем текущую сумму за клик
    /// и создаем всплывающий текст с полученной суммой
    /// </summary>
    /// <param name="target">Объект, возле которого отобразить всплывающий текст</param>
    public void OnTrashClicked(GameObject target)
    {
        var m = _playerUpgrades.GetMoneyPerClick();
        Instantiate(PopUp.PopUpPrefab, target.transform).Show("+" + m.ToString());
        Money += m;
    }

    /// <summary>
    /// При появлении загружаем шаблон всплывающего текста,
    /// а также устанавливаем ссылку на экземпляр этого скрипта,
    /// запускаем таймеры по пассивному заработку и команд рабочих,
    /// получаем ссылку на скрипт со всеми улучшениями и говорим ему загрузить их
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
    /// По нужной кнопке сразу получаем кучу денег (читерство)
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
    /// Таймер по пассивному заработку в зависимости от количества секунд
    /// </summary>
    /// <param name="seconds">Время таймера</param>
    /// <param name="targetDisplay">Значок, возле которого создаем всплывающий текст</param>
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
    /// Таймер команды рабочих
    /// </summary>
    /// <param name="targetDisplay">Значок команды рабочих</param>
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
