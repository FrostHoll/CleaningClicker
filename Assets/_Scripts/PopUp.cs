using TMPro;
using UnityEngine;

/// <summary>
/// �������� �� �������� ������������ ������
/// </summary>
public class PopUp : MonoBehaviour
{
    /// <summary>
    /// ������ ������������ ������
    /// </summary>
    public static PopUp PopUpPrefab = null;

    /// <summary>
    /// ������ �� �����, ���� ����� �������� ���������
    /// </summary>
    [SerializeField]
    private TMP_Text _text;

    /// <summary>
    /// ������ �� ������� ������������ ������
    /// </summary>
    [SerializeField]
    private CanvasGroup _canvasGroup;

    /// <summary>
    /// ����� ������������ ������
    /// </summary>
    [SerializeField]
    private float _fadeTime = 1f;

    /// <summary>
    /// ���������� �����, �������� ��� � ��������� ����������� �
    /// ������ �������� ������������
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text)
    {
        Vector3 offset = Random.onUnitSphere * 30f;
        transform.position += offset;
        _text.text = text;
        LeanTween.moveY(_text.gameObject, transform.position.y + 30f, _fadeTime);
        LeanTween.alphaCanvas(_canvasGroup, 0f, _fadeTime).setDestroyOnComplete(true);
    }
}
