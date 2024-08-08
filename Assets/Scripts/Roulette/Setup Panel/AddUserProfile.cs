using UnityEngine;

public class AddUserProfile : MonoBehaviour
{
    public GameObject userProfilePrefab;
    public Transform contentsTransform;
    private void AddButtonPressSFX()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_SFX(SoundManager.E_SFX_Name.ROULETTE_ADD_BUTTON_PRESS);
        }
    }
    // ���� �Ŵ����� �����͸� �߰�, ���� �������� UI�߰�
    public void Add()
    {
        // �߰� ��ư ȿ���� ���
        AddButtonPressSFX();

        // �귿 �Ŵ����� ���ο� ���� �������� �߰�
        RoulettePieceData userData = RouletteManager.Instance.AddUserData();

        // ������ �������� ����
        GameObject prefab = Instantiate(userProfilePrefab);

        // �ش� �����ʿ� �ο��� �ε���
        int index = userData.index;

        // �ش� �����տ� ����Ʈ �̸��ο�
        //prefab.GetComponent<RoulettePiece>().Setup(userData, index);
        prefab.GetComponent<InfoData>().Setup(userData.userImage, userData.description_name, index);

        // ��ũ�Ѻ信 ���̴� �۾�
        prefab.transform.SetParent(contentsTransform, false);
    }
    
}
