using UnityEngine;
using UnityEngine.UI;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private Roulette roulette;
    [SerializeField]
    private Button buttonSpin;

    [SerializeField]
    private GameObject resultPanel;
    [SerializeField]
    private GameObject roulettePanel;

    private void Awake()
    {
        buttonSpin.onClick.AddListener(() =>
        {
            SpinStartSFX_Play();

            // �귿�� ȸ���ϴ� ������ ��ư �ٽ� ���� �� ���� ����
            buttonSpin.interactable = false;

            // ȸ�� ����, ȸ�� ������ �ٽ� ��ư ���� �� �ְ� �ٲٴ� Action�Լ� ����
            roulette.Spin(EndOfSpin);
        });
    }
    private void SpinStartSFX_Play()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_SFX(SoundManager.E_SFX_Name.ROULETTE_SPIN_START_BUTTON_PRESS);
        }
    }
    private void ResultBGM_Play()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_BGM(SoundManager.E_BGM_Name.ROULETTE_RESULT);
        }
    }

    private void EndOfSpin(RoulettePieceData selectedData)
    {
        ResultBGM_Play();

        buttonSpin.interactable = true;
      
        // ���� ���� �� �귿 �г� ��Ȱ��ȭ
        roulettePanel.SetActive(false);
        // ��� â Ȱ��ȭ
        resultPanel.SetActive(true);
    }


}
