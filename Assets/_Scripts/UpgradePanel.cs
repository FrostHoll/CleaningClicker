using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �� ��������� �������� ���������
/// </summary>
public class UpgradePanel : MonoBehaviour
{
    /// <summary>
    /// ������ �� ���� ��������
    /// </summary>
    [SerializeField]
    private GameObject _panel;

    /// <summary>
    /// ������ �� ���������, ������� �������� ��� ����������/���������� ������
    /// </summary>
    [SerializeField]
    private GameObject _arrow;

    /// <summary>
    /// ������ �� �������� ���������� ������
    /// </summary>
    [SerializeField]
    private GameObject _blackScreen;

    /// <summary>
    /// � ����� ������� �������� � ���� ������������� ��� ���� ���������
    /// </summary>
    [SerializeField]
    private Transform _scrollContent;

    /// <summary>
    /// ������ ���� ���������
    /// </summary>
    [SerializeField]
    private UpgradeDisplay _upgradeDisplayPrefab;

    /// <summary>
    /// ������ �� ��������, ������������ ����������� �������� ������� ���� �� ������ ���������
    /// </summary>
    [SerializeField]
    private GameObject _upgradeAvaliableIcon;

    /// <summary>
    /// ������ ���� ���� ���������
    /// </summary>
    private List<UpgradeDisplay> _upgradeDisplays = new List<UpgradeDisplay>();

    /// <summary>
    /// ������ �� ������������ �������� ���������� ������
    /// </summary>
    private CanvasGroup _blackScreenCanvasGroup;

    /// <summary>
    /// ������� �� ������ ��������
    /// </summary>
    private bool _isOpened = false;

    /// <summary>
    /// ��� ��������� �������� ������ �� ������������ �������� ����������
    /// � ������� ��� ���� ���������
    /// </summary>
    private void Start()
    {
        _blackScreenCanvasGroup = _blackScreen.GetComponent<CanvasGroup>();
        SetupUpgrades();
    }

    /// <summary>
    /// ��������� ����� �� �������� ������� ���� �� ������ ��������� 
    /// � �������� ������, ���� �����
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
    /// � ������� �������� ��������/��������� ���������� � ���������/��������� ������,
    /// ������ ���������
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
    /// �� ������ �������� ������ ���� ���������,
    /// ������� ���� � ���������� � ��� ���������
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
