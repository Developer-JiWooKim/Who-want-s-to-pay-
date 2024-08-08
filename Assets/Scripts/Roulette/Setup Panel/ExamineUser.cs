using UnityEngine;
using UnityEngine.UI;

public class ExamineUser : MonoBehaviour
{
    [SerializeField]
    private Button completeButton;
    [SerializeField]
    private GameObject roulettePanel;
    [SerializeField]
    private FadeDialogueWindow dialogueWindow;


    private void Start()
    {
        completeButton.onClick.AddListener(() => 
        {
            if (ExaminUserData())
            {
                FailedSFX_Play();
                // ������ �߰��ش޶�� �˾�â ���Դٰ� ������� ���� �ִϸ��̼� or Dialogue â
                dialogueWindow.PlayFadeAnimation();
                return;
            }
            CompleteSFX_Play();
            gameObject.SetActive(false);
            roulettePanel.SetActive(true);
        });
    }
    private bool ExaminUserData()
    {
        return RouletteManager.Instance.roulettePieceDatas.Count <= 1;
    }
    private void FailedSFX_Play()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_SFX(SoundManager.E_SFX_Name.ROULETTE_FAILED_COMPLETE);
        }
    }
    private void CompleteSFX_Play()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_SFX(SoundManager.E_SFX_Name.ROULETTE_COMPLETE_BUTTON_PRESS);
            SoundManager.Instance.Play_BGM(SoundManager.E_BGM_Name.ROULETTE_BOARD);
        }
    }
}
