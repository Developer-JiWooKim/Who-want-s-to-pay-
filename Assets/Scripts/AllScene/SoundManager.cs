using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Singleton Pattern
    /// </summary>
    private static SoundManager instance = null;
    public static SoundManager Instance
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
    public enum E_SFX_Name
    {
        ROBBY_BUTTON_PRESS,
        ROULETTE_BACK_BUTTON_PRESS,
        ROULETTE_ADD_BUTTON_PRESS,
        ROULETTE_ADD_IMAGE_BUTTON_PRESS,
        ROULETTE_DELETE_BUTTON_PRESS,
        ROULETTE_COMPLETE_BUTTON_PRESS,
        ROULETTE_FAILED_COMPLETE,
        ROULETTE_SPIN_START_BUTTON_PRESS,
        ROULETTE_SPINNING,
        ROULETTE_SETTING_BUTTON_PRESS,
    }

    public enum E_BGM_Name
    {
        ROBBY,
        LOADING_ROULETTE,
        LOADING_SLOTMACHINE,
        ROULETTE_SETUP,
        ROULETTE_BOARD,
        ROULETTE_RESULT,
        SLOTMACHINE,
    }

    [SerializeField]
    private AudioClip[]     bgm                     = null;
    [SerializeField]
    private AudioClip[]     sfx                     = null;

    [SerializeField]
    private AudioSource     bgm_Player              = null;
    [SerializeField]
    private AudioSource[]   sfx_Player              = null;

    private const float     __DEFAULT_VOLUME_VALUE  = 0.7f;

    private void Awake()
    {
        SingletonSetup();
    }
    private void Start()
    {
        Setup();
    }
    private void SingletonSetup()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// !!__����__!!
    /* ���� �������� �ʿ� ����
     * �� ������Ʈ������ ȿ������ ���� �ʾƼ� ���� �Ŵ������� ���� �ε� �ÿ� ��� ȿ������ �ε��� ������ ����
     * ������ ������Ʈ�� Ŀ���� ���� ���� ȿ������ ���ܹ����� ������ ����� �ε��ÿ� ���� �ð��� �ҿ��
     * �׷��Ƿ� ���߿��� ������ ȿ������ ������ ���� ��ũ��Ʈ�� ����� ���� �Ŵ����� �� ��ȯ�ÿ� �ش� ��ũ��Ʈ�� �ε��ؼ� ����� �� �ְ� �ٲ�ߵ�
     * �׷��Ƿ� �� Setup�� public���� �ٲٰ� �� ��ȯ�� ������ �ش� ������ ��ũ��Ʈ�� ã�Ƽ� �ε��ϴ� �ڵ带 Setup�� �ۼ��ϸ� ��
     */
    private void Setup()
    {
        // ���� �÷��̾� ����, Ÿ��Ʋ BGM���
        {
            bgm_Player.playOnAwake = true;
            bgm_Player.loop = true;
            Play_BGM(E_BGM_Name.ROBBY);
            SetDefaultVolume();
        }
    }
    /// <summary>
    /// ���� ũ�⸦ __DEFAULT_VOLUME_VALUE(.5f)�� �����ϴ� �Լ�
    /// </summary>
    private void SetDefaultVolume()
    {
        SetVolume_BGM(__DEFAULT_VOLUME_VALUE);
        SetVolume_SFX(1);
    }
    public void Play_BGM(E_BGM_Name bgm_Name)
    {
        bgm_Player.clip = bgm[(int)bgm_Name];
        bgm_Player.Play();
    }
    public void SetVolume_BGM(float _volume)
    {
        bgm_Player.volume = _volume;
    }
    public void SetVolume_SFX(float _volume)
    {
        for (int i = 0; i < sfx_Player.Length; i++)
        {
            sfx_Player[i].volume = _volume;
        }
    }
    public void Stop_BGM()
    {
        bgm_Player.Stop();
    }
    public void Play_SFX(E_SFX_Name sfx_Name)
    {
        for (int j = 0; j < sfx_Player.Length; j++)
        {
            // SFX �÷��̾� �� ��� ������ ���� AudioSource�� �߰��ϸ�
            if (!sfx_Player[j].isPlaying)
            {
                sfx_Player[j].clip = sfx[(int)sfx_Name];
                sfx_Player[j].Play();
                return;
            }
        }
        Debug.Log("All SFX Player is Playing!!");
    }
}
