using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за смену локаций и отображение элементов интерфейса
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// Создаем ссылку на единственный экземпляр, чтобы удобнее было обращаться
    /// </summary>
    public static GameController Instance = null;

    /// <summary>
    /// К этому объекту прикрепляются все локации
    /// </summary>
    [SerializeField]
    private GameObject _backgroundParent;

    /// <summary>
    /// Прозрачность текста, извещающего о пройденной локации
    /// </summary>
    [SerializeField]
    private CanvasGroup _bgClearedTextCanvasGroup;

    /// <summary>
    /// Значок рабочих на экране
    /// </summary>
    [SerializeField]
    private GameObject _workersDisplay;

    /// <summary>
    /// Значок фабрик на экране
    /// </summary>
    [SerializeField]
    private GameObject _factoriesDisplay;

    /// <summary>
    /// Значок команды личных рабочих на экране
    /// </summary>
    [SerializeField]
    private GameObject _helpersDisplay;

    /// <summary>
    /// Список всех загруженных локаций
    /// </summary>
    private static List<BGController> s_BGPrefabs;

    /// <summary>
    /// Текущая локация
    /// </summary>
    private BGController _currentBG = null;

    /// <summary>
    /// Следующая локация (нужно при анимации их смены)
    /// </summary>
    private BGController _nextBG = null;

    /// <summary>
    /// Открываем доступ к значку рабочих
    /// </summary>
    public GameObject WorkersDisplay => _workersDisplay;

    /// <summary>
    /// Открываем доступ к значку фабрик
    /// </summary>
    public GameObject FactoriesDisplay => _factoriesDisplay;

    /// <summary>
    /// Открываем доступ к значку команды рабочих
    /// </summary>
    public GameObject HelpersDisplay => _helpersDisplay;

    /// <summary>
    /// При появлении задаем ссылку на этот скрипт и загружаем все локации из файлов проекта
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        if (s_BGPrefabs == null)
        {
            s_BGPrefabs = new List<BGController>();
            s_BGPrefabs.AddRange(Resources.LoadAll<BGController>("Prefabs/BGs"));
        }
    }

    /// <summary>
    /// После прогрузки всех локаций загружаем первую,
    /// и подписываемся на событие, когда игрок улучшил любое улучшение
    /// </summary>
    private void Start()
    {
        SpawnNextBG();
        Player.Instance.Upgraded += OnPlayerUpgraded;
    }

    /// <summary>
    /// Когда игрок что-то улучшил, включаем значки тех улучшений, которые у игрока сейчас есть
    /// </summary>
    private void OnPlayerUpgraded()
    {
        if (Player.Instance.HasWorkers)
        {
            if (!_workersDisplay.activeSelf)
                _workersDisplay.SetActive(true);
        }
        if (Player.Instance.HasFactories)
        {
            if (!_factoriesDisplay.activeSelf)
                _factoriesDisplay.SetActive(true);
        }
        if (Player.Instance.HasHelpers)
        {
            if (!_helpersDisplay.activeSelf)
                _helpersDisplay.SetActive(true);
        }
    }

    /// <summary>
    /// Говорим текущей локации очищать случайный мусор
    /// </summary>
    /// <param name="times">Сколько раз атаковать</param>
    public void AttackRandomTargets(int times)
    {
        if (_currentBG != null)
            StartCoroutine(_currentBG.AttackRandomTargets(times));
    }

    /// <summary>
    /// Когда локация очищена, проявляем текст с оповещением и с задержкой загружаем
    /// следующую локацию
    /// </summary>
    public void OnBGCleared()
    {
        LeanTween.alphaCanvas(_bgClearedTextCanvasGroup, 1f, 0.2f).setOnComplete(() =>
        {
            Invoke(nameof(SpawnNextBG), 0.5f);
            LeanTween.alphaCanvas(_bgClearedTextCanvasGroup, 0f, 0.8f).setDelay(0.5f);
        });
    }

    /// <summary>
    /// Загружаем следующую случайную локацию среди загруженных
    /// и с помощью анимации плавно их сдвигаем
    /// </summary>
    public void SpawnNextBG()
    {
        int index = Random.Range(0, s_BGPrefabs.Count);
        _nextBG = Instantiate(s_BGPrefabs[index], _backgroundParent.transform);
        if (_currentBG != null)
        {
            LeanTween.moveX(_currentBG.gameObject, -1920f, 1f).setDestroyOnComplete(true);
        }
        LeanTween.moveX(_nextBG.gameObject, 0f, 1f).setOnComplete(() =>
        {
            _currentBG = _nextBG;
            _currentBG.BGCleared += OnBGCleared;
            _nextBG = null;
        });
    }
}
