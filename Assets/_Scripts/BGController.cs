using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ћокаци€; ќтвечает за размещение мест дл€ по€влени€ мусора
/// </summary>
public class BGController : MonoBehaviour
{
    /// <summary>
    /// —писок мест дл€ по€влени€ мусора
    /// </summary>
    [SerializeField]
    private List<TrashSpot> _trashSpots;

    /// <summary>
    /// —писок по€вившегос€ мусора на локации
    /// </summary>
    private List<Trash> _currentTrash = new List<Trash>();

    /// <summary>
    /// —обытие, показывающее, что локаци€ очищена
    /// </summary>
    public event Action BGCleared;

    /// <summary>
    /// ѕри по€влении локации создать мусор
    /// </summary>
    private void Start()
    {
        SpawnTrash();
    }

    /// <summary>
    /// јтаковать случайный мусор на локации
    /// </summary>
    /// <param name="times">—колько раз атаковать</param>
    public IEnumerator AttackRandomTargets(int times)
    {
        while (times > 0)
        {
            yield return new WaitForSeconds(0.1f);
            if (_currentTrash.Count == 0) break;
            int index = UnityEngine.Random.Range(0, _currentTrash.Count);
            if (_currentTrash[index] != null)
                _currentTrash[index].Attack();
            times--;
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    ///  огда мусор очищен, убрать его и списка текущих, и он если был последним, 
    /// сообщить всем, что локаци€ зачищена
    /// </summary>
    /// <param name="trash">ћусор, который был очищен</param>
    public void OnTrashCleaned(Trash trash)
    {
        _currentTrash.Remove(trash);
        if (_currentTrash.Count == 0) BGCleared?.Invoke();
    }

    /// <summary>
    /// —оздать на нужных местах мусор
    /// </summary>
    public void SpawnTrash()
    {
        foreach (var spot in _trashSpots)
        {
            var trash = spot.SpawnTrash(this);
            if (trash != null) _currentTrash.Add(trash);
        }
    }
}
