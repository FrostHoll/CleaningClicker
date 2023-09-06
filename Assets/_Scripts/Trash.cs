using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ќтвечает за поведение мусора
/// </summary>
public class Trash : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// —сылка на панель прогресса жизни мусора
    /// </summary>
    [SerializeField]
    private ProgressBar _progressBar;

    /// <summary>
    /// —сылка на иконку мусора
    /// </summary>
    [SerializeField]
    private GameObject _icon;

    /// <summary>
    /// ћаксимальное здоровье мусора
    /// </summary>
    [SerializeField]
    private int _maxHP;

    /// <summary>
    /// “екущее здоровье мусора
    /// </summary>
    private int _hp;

    /// <summary>
    /// —обытие, когда по этому мусору кликнули
    /// </summary>
    private event Action<GameObject> Clicked;

    /// <summary>
    /// —обытие, когда этот мусор был очищен
    /// </summary>
    public event Action<Trash> Died;

    /// <summary>
    /// Ќачальна€ позици€ иконки мусора
    /// </summary>
    private Vector3 _defaultPosition;

    /// <summary>
    /// ¬ начале выставл€ем все значени€ здоровь€ мусора и подписываем
    /// игрока на событие по клику по этому мусору
    /// </summary>
    private void Start()
    {       
        _maxHP = 100;
        _hp = _maxHP;
        _progressBar.Init(_hp, _maxHP);
        _progressBar.gameObject.SetActive(false);
        if (Clicked == null) Clicked += Player.Instance.OnTrashClicked;
    }

    /// <summary>
    /// «апоминаем позицию иконки, отнимаем здоровье и обновл€ем его,
    /// —ообщаем что по мусору кликнули и, если он был очищен, просим его позаботитьс€ о своем удалении,
    /// иначе проигрываем анимацию очистки
    /// </summary>
    public void Attack()
    {
        _defaultPosition = transform.position;
        _hp -= 10;
        if (!_progressBar.gameObject.activeSelf) _progressBar.gameObject.SetActive(true);
        _progressBar.UpdateValue(_hp);
        Clicked?.Invoke(gameObject);
        if (_hp <= 0) HandleDeath();
        else
        {
            LeanTween.moveX(_icon, _defaultPosition.x + 5f, 0.01f).setOnComplete(() =>
            LeanTween.moveX(_icon, _defaultPosition.x - 5f, 0.02f).setOnComplete(() =>
            LeanTween.moveX(_icon, _defaultPosition.x, 0.01f)));
        }
    }

    /// <summary>
    /// —ообщаем всем, что мусор был очищен, отписываем всех от событий этого мусора и
    /// уничтожаем его
    /// </summary>
    private void HandleDeath()
    {
        Clicked = null;
        Died?.Invoke(this);
        Died = null;
        Destroy(gameObject);
    }

    /// <summary>
    ///  огда по мусору кликнули мышкой, очищаем его
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Attack();
    }
}
