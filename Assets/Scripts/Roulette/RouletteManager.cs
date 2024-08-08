using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouletteManager : MonoBehaviour
{
    /// <summary>
    /// Singleton Pattern
    /// </summary>
    private static RouletteManager instance = null;
    public static RouletteManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    [field:SerializeField]
    public List<RoulettePieceData> roulettePieceDatas { get; private set; }
    public int count { get; private set; }

    [SerializeField]
    private Sprite defaultUserImage;

    [SerializeField]
    private GameObject contents;

    private void Awake()
    {
        SingletonSetup();
    }
    private void Start()
    {
        Setup();
    }
    /// <summary>
    /// �̱��� ó���ϴ� �Լ�
    /// </summary>
    private void SingletonSetup()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Setup()
    {
        // Data Manager�� ����� ���� ���ð��� ������
        count = 0; // TODO#: DataManager���� ���õ� ������ ��ϵ� ���� ����ŭ count ����
        roulettePieceDatas = new List<RoulettePieceData>();
    }
    // �귿�� �� ������ �����͸� �߰�, UI�� �������°� �ƴ� �����ͷ� ������
    // Add ��ư Ŭ�� �� ȣ��Ǵ� �Լ�, �귿�� �� �����͸� ���� �� ����
    public RoulettePieceData AddUserData()
    {
        // �귿 �ǽ� ������ ����
        RoulettePieceData data = new RoulettePieceData();

        // ����Ʈ �̹��� �߰�
        //data.icon = defaultUserImage;
        data.userImage = defaultUserImage.texture;

        // �ε��� �ο�
        data.index = count;

        // ����Ʈ �̸� �ο�
        data.description_name = "User " + (count + 1); 

        // �귿 �ǽ� �����͵��� �����ϴ� ����Ʈ�� �ش� �����͸� �߰�
        roulettePieceDatas.Add(data);

        // �ε��� 1 ����
        count++;

        return data;
    }

    // �Ű������� ���� �ε����� ���� ������ ����
    public void RemoveUserData(int index)
    {
        // �Ű������� ���� index�� ���� index�� ���� ������ ���� 
        roulettePieceDatas.Remove(roulettePieceDatas.Find(data => data.index == index));
    }

    public void UpdateRouletteIndex(int deleteIndex)
    {
        // �����͸� ���� �� �����յ��� �����͸� �ʱ�ȭ �ؾ���
        InfoData[] userDatas = GetInfoDatas(deleteIndex);

        // ������ ���� �ʱ�ȭ
        count = 0;

        // ��� �������� �ε��� �� �缳��
        roulettePieceDatas.ForEach(data =>
        {
            data.index = count;
            count++;
        });
        
        // �� �����͵��� �ε����� �缳�� �� UI�� ������Ʈ
        for (int i = 0; i < userDatas.Length; i++)
        {
            userDatas[i].UpdateIndex(roulettePieceDatas[i].index);
            userDatas[i].UpdateUserName_UI();
        }
    }

    // ������ ���� �����͸� ������ ���� ������ �迭�� ����
    public InfoData[] GetInfoDatas(int deleteIndex)
    {
        // contents�� �پ��ִ� InfoData���� ������ �迭�� ����
        var datas = contents.GetComponentsInChildren<InfoData>();

        // InfoData�� ������ �ε����� ���� �����͸� �����ϰ� �迭 �ٽ� ����
        var newDatas = datas.Where(data => data.index != deleteIndex).ToArray();
        
        return newDatas;
    }

    public RoulettePieceData[] GetInfoDatas()
    {
        var infoDatas = contents.GetComponentsInChildren<InfoData>();

        // �귿 �����͵��� ������ Setup�гο��� ������ ������ ����
        roulettePieceDatas.ForEach(data =>
        {
            data.userImage = infoDatas[data.index].userImage.texture;
            data.description_name = infoDatas[data.index].userName;
        });

        return roulettePieceDatas.ToArray();
    }
}
