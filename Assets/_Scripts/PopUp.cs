using TMPro;
using UnityEngine;

/// <summary>
/// Отвечает за анимацию всплывающего текста
/// </summary>
public class PopUp : MonoBehaviour
{
    /// <summary>
    /// Шаблон всплывающего текста
    /// </summary>
    public static PopUp PopUpPrefab = null;

    /// <summary>
    /// Ссылка на текст, куда будем помещать сообщение
    /// </summary>
    [SerializeField]
    private TMP_Text _text;

    /// <summary>
    /// Ссылка на степень прозрачности текста
    /// </summary>
    [SerializeField]
    private CanvasGroup _canvasGroup;

    /// <summary>
    /// Время исчезновения текста
    /// </summary>
    [SerializeField]
    private float _fadeTime = 1f;

    /// <summary>
    /// Отобразить текст, сместить его в случайном направлении и
    /// начать анимацию исчезновения
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
