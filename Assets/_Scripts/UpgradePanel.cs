using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за поведение панельки улучшений
/// </summary>
public class UpgradePanel : MonoBehaviour
{
    /// <summary>
    /// Ссылка на саму панельку
    /// </summary>
    [SerializeField]
    private GameObject _panel;

    /// <summary>
    /// Ссылка на стрелочку, которая крутится при открывании/закрывании панели
    /// </summary>
    [SerializeField]
    private GameObject _arrow;

    /// <summary>
    /// Ссылка на картинку затемнения экрана
    /// </summary>
    [SerializeField]
    private GameObject _blackScreen;

    /// <summary>
    /// К этому объекту крепятся и сами расставляются все окна улучшений
    /// </summary>
    [SerializeField]
    private Transform _scrollContent;

    /// <summary>
    /// Шаблон окна улучшения
    /// </summary>
    [SerializeField]
    private UpgradeDisplay _upgradeDisplayPrefab;

    /// <summary>
    /// Ссылка на картинку, обозначающую возможность повысить уровень хотя бы одного улучшения
    /// </summary>
    [SerializeField]
    private GameObject _upgradeAvaliableIcon;

    /// <summary>
    /// Список всех окон улучшений
    /// </summary>
    private List<UpgradeDisplay> _upgradeDisplays = new List<UpgradeDisplay>();

    /// <summary>
    /// Ссылка на прозрачность картинки затемнения экрана
    /// </summary>
    private CanvasGroup _blackScreenCanvasGroup;

    /// <summary>
    /// Открыта ли сейчас панелька
    /// </summary>
    private bool _isOpened = false;

    /// <summary>
    /// При появлении получаем ссылку на прозрачность картинки затемнения
    /// и создаем все окна улучшений
    /// </summary>
    private void Start()
    {
        _blackScreenCanvasGroup = _blackScreen.GetComponent<CanvasGroup>();
        SetupUpgrades();
    }

    /// <summary>
    /// Проверяем можно ли повысить уровень хотя бы одному улучшению 
    /// и включаем значок, если можно
    /// </summary>
    private void Update()
    {
        bool upgradeAvaliable = false;
        foreach (var ud in _upgradeDisplays)
        {
            if (ud.ReadyToUpgrade)
            {
                upgradeAvaliable = true;
                break;
            }
        }
        _upgradeAvaliableIcon.SetActive(upgradeAvaliable);
    }

    /// <summary>
    /// С помощью анимаций включаем/выключаем затемнение и выдвигаем/задвигаем панель,
    /// крутим стрелочку
    /// </summary>
    public void TooglePanel()
    {
        if (!_isOpened) 
        {
            _isOpened = true;
            _blackScreen.SetActive(true);
            LeanTween.alphaCanvas(_blackScreenCanvasGroup, 1f, 0.3f);
            LeanTween.rotateZ(_arrow, 180f, 0.2f);
            LeanTween.moveX(_panel, -_panel.transform.position.x, 0.5f).setEaseOutQuad();
        }
        else
        {
            _isOpened = false;           
            LeanTween.alphaCanvas(_blackScreenCanvasGroup, 0f, 0.5f).setOnComplete(() => _blackScreen.SetActive(false));
            LeanTween.rotateZ(_arrow, 0f, 0.2f);
            LeanTween.moveX(_panel, -_panel.transform.position.x, 0.5f).setEaseOutQuad();
        }
    }

    /// <summary>
    /// От игрока получаем список всех улучшений,
    /// создаем окна и закидываем в них улучшения
    /// </summary>
    private void SetupUpgrades()
    {
        var upgrades = Player.Instance.GetUpgrades();
        foreach (var up in upgrades)
        {
            var obj = Instantiate(_upgradeDisplayPrefab, _scrollContent);
            obj.Init(up);
            _upgradeDisplays.Add(obj);
        }
    }
}
