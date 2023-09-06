using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за отображение сведений о улучшении в окне в списке улучшений
/// </summary>
public class UpgradeDisplay : MonoBehaviour
{
    /// <summary>
    /// Ссылка изображение иконки
    /// </summary>
    [SerializeField]
    private Image _icon;

    /// <summary>
    /// Текст названия улучшения
    /// </summary>
    [SerializeField]
    private TMP_Text _upgradeNameText;

    /// <summary>
    /// Текст описания улучшения
    /// </summary>
    [SerializeField]
    private TMP_Text _upgradeDescriptionText;

    /// <summary>
    /// Текст стоимости улучшения
    /// </summary>
    [SerializeField]
    private TMP_Text _costText;

    /// <summary>
    /// Кнопка повышения уровня улучшения
    /// </summary>
    [SerializeField]
    private Button _upgradeButton;

    /// <summary>
    /// Текст текущего уровня улучшения
    /// </summary>
    [SerializeField]
    private TMP_Text _levelText;

    /// <summary>
    /// Ссылка на улучшение, которое отображается в этом окне
    /// </summary>
    private Upgrade _upgrade;

    /// <summary>
    /// Можно ли сейчас повысить уровень улучшения
    /// </summary>
    public bool ReadyToUpgrade => _upgradeButton.interactable;

    /// <summary>
    /// Установка всех параметров улучшения для отображения;
    /// Подписываемся на события повышения уровня улучшения
    /// и изменения текущих денег игрока
    /// </summary>
    /// <param name="upgrade">Улучшение</param>
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
    /// Пытаемся повысить уровень улучшения, если удалось перепроверяем,
    /// можно ли еще раз улучшить
    /// </summary>
    public void TryUpgrade()
    {
        if (Player.Instance.TryUpgrade(_upgrade))
        {
            CheckReady();
        }
    }

    /// <summary>
    /// Когда уровень улучшения был повышен, обновляем все тексты новыми данными
    /// </summary>
    /// <param name="newLevel">Новый уровень</param>
    /// <param name="newValue">Новая сумма улучшения</param>
    /// <param name="newCost">Новая стоимость улучшения</param>
    private void OnUpgraded(int newLevel, Money newValue, Money newCost)
    {
        _upgradeDescriptionText.text = _upgrade.Data.upgradeDescription + $" <color=#50b2f4>{newValue}</color>";
        _costText.text = newCost.ToString();
        if (newLevel == _upgrade.Data.maxLevel) _levelText.text = "MAX";
        else _levelText.text = newLevel.ToString();
    }

    /// <summary>
    /// Когда кол-во денег игрока изменилось, проверяем хватает ли их на улучшение
    /// </summary>
    /// <param name="newMoney">Новые деньги игрока</param>
    private void OnMoneyChanged(Money newMoney) 
    {
        _upgradeButton.interactable = _upgrade.CurrentCost <= newMoney && _upgrade.CurrentLevel < _upgrade.Data.maxLevel;
    }

    /// <summary>
    /// Включаем кнопку улучшения, если сейчас можно повысить уровень
    /// </summary>
    private void CheckReady()
    {
        _upgradeButton.interactable = _upgrade.CurrentCost <= Player.Instance.Money 
            && _upgrade.CurrentLevel < _upgrade.Data.maxLevel;
    }
}
