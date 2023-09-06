using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �� ����� ������� � ����������� ��������� ����������
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// ������� ������ �� ������������ ���������, ����� ������� ���� ����������
    /// </summary>
    public static GameController Instance = null;

    /// <summary>
    /// � ����� ������� ������������� ��� �������
    /// </summary>
    [SerializeField]
    private GameObject _backgroundParent;

    /// <summary>
    /// ������������ ������, ����������� � ���������� �������
    /// </summary>
    [SerializeField]
    private CanvasGroup _bgClearedTextCanvasGroup;

    /// <summary>
    /// ������ ������� �� ������
    /// </summary>
    [SerializeField]
    private GameObject _workersDisplay;

    /// <summary>
    /// ������ ������ �� ������
    /// </summary>
    [SerializeField]
    private GameObject _factoriesDisplay;

    /// <summary>
    /// ������ ������� ������ ������� �� ������
    /// </summary>
    [SerializeField]
    private GameObject _helpersDisplay;

    /// <summary>
    /// ������ ���� ����������� �������
    /// </summary>
    private static List<BGController> s_BGPrefabs;

    /// <summary>
    /// ������� �������
    /// </summary>
    private BGController _currentBG = null;

    /// <summary>
    /// ��������� ������� (����� ��� �������� �� �����)
    /// </summary>
    private BGController _nextBG = null;

    /// <summary>
    /// ��������� ������ � ������ �������
    /// </summary>
    public GameObject WorkersDisplay => _workersDisplay;

    /// <summary>
    /// ��������� ������ � ������ ������
    /// </summary>
    public GameObject FactoriesDisplay => _factoriesDisplay;

    /// <summary>
    /// ��������� ������ � ������ ������� �������
    /// </summary>
    public GameObject HelpersDisplay => _helpersDisplay;

    /// <summary>
    /// ��� ��������� ������ ������ �� ���� ������ � ��������� ��� ������� �� ������ �������
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
    /// ����� ��������� ���� ������� ��������� ������,
    /// � ������������� �� �������, ����� ����� ������� ����� ���������
    /// </summary>
    private void Start()
    {
        SpawnNextBG();
        Player.Instance.Upgraded += OnPlayerUpgraded;
    }

    /// <summary>
    /// ����� ����� ���-�� �������, �������� ������ ��� ���������, ������� � ������ ������ ����
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
    /// ������� ������� ������� ������� ��������� �����
    /// </summary>
    /// <param name="times">������� ��� ���������</param>
    public void AttackRandomTargets(int times)
    {
        if (_currentBG != null)
            StartCoroutine(_currentBG.AttackRandomTargets(times));
    }

    /// <summary>
    /// ����� ������� �������, ��������� ����� � ����������� � � ��������� ���������
    /// ��������� �������
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
    /// ��������� ��������� ��������� ������� ����� �����������
    /// � � ������� �������� ������ �� ��������
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
