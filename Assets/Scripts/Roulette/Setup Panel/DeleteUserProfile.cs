using UnityEngine;

public class DeleteUserProfile : MonoBehaviour
{
    private GameObject userProfile;

    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        userProfile = gameObject.transform.parent.gameObject;
    }
    private void DeleteSFX_Play()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_SFX(SoundManager.E_SFX_Name.ROULETTE_DELETE_BUTTON_PRESS);
        }
        
    }
    public void Delete()
    {
        if (userProfile == null)
        {
            Debug.Log("null");

            return;
        }

        // ���� ȿ���� ���
        DeleteSFX_Play();

        // �귿 �Ŵ����� ������ ����Ʈ���� ���� �ε����� ���� ������ ����
        RouletteManager.Instance.RemoveUserData(userProfile.gameObject.GetComponent<InfoData>().index);
        RouletteManager.Instance.UpdateRouletteIndex(userProfile.gameObject.GetComponent<InfoData>().index);

        // �� ������ ����
        Destroy(userProfile);
    }
}
