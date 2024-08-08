using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton Pattern
    /// </summary>
    private static GameManager instance = null;
    public static GameManager Instance
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

    [SerializeField]
    private List<LoadingWindowBase> loadingWindows = new List<LoadingWindowBase>();

    public enum SceneName
    {
        Robby,
        Roulette,
        Slotmachine,
    }
    public SceneName sceneName = SceneName.Robby;

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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Setup()
    {
        Application.targetFrameRate = 60;
        loadingWindows = transform.gameObject.GetComponentsInChildren<LoadingWindowBase>().ToList();
    }
    
    /// <summary>
    /// �񵿱� Scene ��ȯ �ڷ�ƾ �Լ�(�񵿱� Scene ��ȯ �� �ε�ȭ�鵵 ǥ�����ִ� �ڷ�ƾ �Լ�)
    /// </summary>
    private IEnumerator AsyncLoadScene()
    {
        // �񵿱� �� ��ȯ ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncLoad.allowSceneActivation = false;

        // �̵��Ϸ��� ���� ���� �̸��� ���� LoadingWindowBase�� ���� ������Ʈ�� ã��
        var loadingwindow = loadingWindows.Find((loading) => loading.sceneName == sceneName);
        // �ش� ������Ʈ�� Ȱ��ȭ
        loadingwindow.loadingProgress.SetActive(true);
        // �ش� ������Ʈ�� �ʱ�ȭ
        loadingwindow.ResetPercent();

        int progressPercentage = 0;
        float time = 0;
        float progress;

        loadingwindow.percentText.text = "0%";

        // ���� �ε� ���൵�� ǥ��
        while (!asyncLoad.isDone)
        {
            progress = asyncLoad.progress;
            progressPercentage = Mathf.RoundToInt(progress * 100f);
            loadingwindow.percentText.text = progressPercentage.ToString() + "%";

            loadingwindow.percentSlider.value = progress;

            time += Time.deltaTime;
            // �ּ� 2�ʰ� �ɶ����� ��ٸ�
            if (time > 2f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            // 2�ʰ� �帣�� �ε� ���൵ �ؽ�Ʈ�� 100%��, ��ȯ�� ���� ������� ���
            if(asyncLoad.allowSceneActivation == true)
            {
                loadingwindow.percentText.text = "100%";
                loadingwindow.EndLoadingAndStartBGM();
            }
            yield return null;
        }
        // �ε��� �������� �ε�ȭ���� ��Ȱ��ȭ
        loadingwindow.loadingProgress.SetActive(false);
        yield return null;
    }
    private IEnumerator ChangeSceneAction()
    {
        // �� ��ȯ ���� �� �ϵ� ������ �����
        
        // ���̵� �ƿ� �Ϸ� �� �񵿱� �ε� ����
        yield return StartCoroutine(AsyncLoadScene());
    }
    public void ChangeScene(SceneName newScene)
    {
        if (sceneName != newScene)
        {
            sceneName = newScene;
        }
        // ���� �κ�� �̵��ϴ°Ÿ� �ε��� �ҷ����� �ʰ� �ٷ� �̵� -> �ε��ؾ��� �����Ͱ� ���� ���� ���� �ʿ�� �ش� if�� ����� �ε�ȭ���� ���� ����� ��
        if (newScene == SceneName.Robby)
        {
            SceneManager.LoadScene(sceneName.ToString());
            SoundManager.Instance.Play_BGM(SoundManager.E_BGM_Name.ROBBY);
            return;
        }

        StartCoroutine(ChangeSceneAction());
    }
    
    public void AwakeAction(Action action = null)
    {
        if (action != null)
        {
            action();
        }
        else
        {
            Debug.Log("GameManager.AwakeAction() : action is null!");
        }
    }
}
