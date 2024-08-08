using UnityEngine;
[System.Serializable]
public class RoulettePieceData
{
    public Texture userImage;         // �귿 ������ �̹���, ���������� �ҷ����� ����
    public string description_name;     // �귿 ������ �̸� TODO#: description �����

    // 3���� ������ ���� Ȯ���� 100 60 40�̸� ����Ȯ���� ���� 200. 100 / 200 = 50%, 60 / 200 = 30%, 40 / 200 = 20%
    [Range(1, 100)]
    public int chance = 100;            // �� ���� Ȯ��

    // [HideInInspector]
    public int index;                   // ����
    [HideInInspector]
    public int weight;                  // ����ġ
}