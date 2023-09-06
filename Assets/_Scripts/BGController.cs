using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������; �������� �� ���������� ���� ��� ��������� ������
/// </summary>
public class BGController : MonoBehaviour
{
    /// <summary>
    /// ������ ���� ��� ��������� ������
    /// </summary>
    [SerializeField]
    private List<TrashSpot> _trashSpots;

    /// <summary>
    /// ������ ������������ ������ �� �������
    /// </summary>
    private List<Trash> _currentTrash = new List<Trash>();

    /// <summary>
    /// �������, ������������, ��� ������� �������
    /// </summary>
    public event Action BGCleared;

    /// <summary>
    /// ��� ��������� ������� ������� �����
    /// </summary>
    private void Start()
    {
        SpawnTrash();
    }

    /// <summary>
    /// ��������� ��������� ����� �� �������
    /// </summary>
    /// <param name="times">������� ��� ���������</param>
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
    /// ����� ����� ������, ������ ��� � ������ �������, � �� ���� ��� ���������, 
    /// �������� ����, ��� ������� ��������
    /// </summary>
    /// <param name="trash">�����, ������� ��� ������</param>
    public void OnTrashCleaned(Trash trash)
    {
        _currentTrash.Remove(trash);
        if (_currentTrash.Count == 0) BGCleared?.Invoke();
    }

    /// <summary>
    /// ������� �� ������ ������ �����
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
