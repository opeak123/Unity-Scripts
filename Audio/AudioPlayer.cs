using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private int clipIndex = 0;

    public void StartMusic()
    {
        // ���� ��� ���� AudioSource ����
        foreach (AudioSource source in AudioManager.Instance.bgmSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        // 0��° Ŭ�� ���
        AudioManager.Instance.PlayBGM(0, 1f);
        // ��¿��� clipIndex�� ���
        Debug.Log(clipIndex + 1);
    }

    public void ChangeMusic()
    {
        // ���� ��� ���� AudioSource ����
        foreach (AudioSource source in AudioManager.Instance.bgmSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        // clipIndex ����
        clipIndex++;

        // �迭�� ���� �����ϸ� 0���� �ʱ�ȭ
        if (clipIndex >= AudioManager.Instance.bgmClips.Length)
        {
            clipIndex = 0;
        }
        // ��¿��� clipIndex�� ���
        Debug.Log(clipIndex + 1);
        AudioManager.Instance.PlayBGM(clipIndex, 1f);
    }
}
