using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за поведение места появления мусора
/// </summary>
public class TrashSpot : MonoBehaviour
{
    /// <summary>
    /// Список всех параметров появления всего возможного мусора
    /// </summary>
    [SerializeField]
    private List<TrashSpotParams> _possibleTrashes;

    /// <summary>
    /// Создаем случайный мусор из списка возможных,
    /// выставляем ему размер и подписываем скрипт BGController на событие,
    /// когда мусор был очищен
    /// </summary>
    /// <param name="parent">Ссылка на текущую локацию, к которой относится это место</param>
    /// <returns>Созданный мусор</returns>
    public Trash SpawnTrash(BGController parent)
    {
        if (_possibleTrashes.Count == 0) return null;
        int index = Random.Range(0, _possibleTrashes.Count);
        var obj = Instantiate(_possibleTrashes[index].prefab, transform);
        obj.Died += parent.OnTrashCleaned;
        obj.transform.localScale = Vector3.one * _possibleTrashes[index].targetSize;
        return obj;
    }
}

/// <summary>
/// Данные шаблона возможного мусора и значение его размера
/// при появлении
/// </summary>
[System.Serializable]
public struct TrashSpotParams
{
    public Trash prefab;
    public float targetSize;
}
