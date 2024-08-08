using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadGallery : MonoBehaviour
{
    public RawImage img;

    public void OnClickImageLoad()
    {
        AddImageButtonPress();
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo selected = new FileInfo(file);

            // �뷮 ���� 5õ�� ����Ʈ ���� ũ�� ����
            if (selected.Length > 50000000)
            {
                return;
            }
            if (!string.IsNullOrEmpty(file))
            {
                // �����ϸ� �ҷ�����
                StartCoroutine(LoadImage(file));
            }
        });
    }
    private void AddImageButtonPress()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play_SFX(SoundManager.E_SFX_Name.ROULETTE_ADD_IMAGE_BUTTON_PRESS);
        }
    }
    private IEnumerator LoadImage(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path).Split('.')[0];
        string savePath = Application.persistentDataPath + "/Image";

        if (!Directory.Exists(savePath)) 
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllBytes(savePath + filename + ".png", fileData);
        var temp = File.ReadAllBytes(savePath + filename + ".png");

        Texture2D texture2D = new Texture2D(0, 0);
        texture2D.LoadImage(temp);

        img.texture = texture2D;

        yield return null;
    }
}
