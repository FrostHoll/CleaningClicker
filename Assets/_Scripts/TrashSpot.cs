using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �� ��������� ����� ��������� ������
/// </summary>
public class TrashSpot : MonoBehaviour
{
    /// <summary>
    /// ������ ���� ���������� ��������� ����� ���������� ������
    /// </summary>
    [SerializeField]
    private List<TrashSpotParams> _possibleTrashes;

    /// <summary>
    /// ������� ��������� ����� �� ������ ���������,
    /// ���������� ��� ������ � ����������� ������ BGController �� �������,
    /// ����� ����� ��� ������
    /// </summary>
    /// <param name="parent">������ �� ������� �������, � ������� ��������� ��� �����</param>
    /// <returns>��������� �����</returns>
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
/// ������ ������� ���������� ������ � �������� ��� �������
/// ��� ���������
/// </summary>
[System.Serializable]
public struct TrashSpotParams
{
    public Trash prefab;
    public float targetSize;
}
