using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private int clipIndex = 0;

    public void StartMusic()
    {
        // 현재 재생 중인 AudioSource 중지
        foreach (AudioSource source in AudioManager.Instance.bgmSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        // 0번째 클립 재생
        AudioManager.Instance.PlayBGM(0, 1f);
        // 출력에는 clipIndex를 사용
        Debug.Log(clipIndex + 1);
    }

    public void ChangeMusic()
    {
        // 현재 재생 중인 AudioSource 중지
        foreach (AudioSource source in AudioManager.Instance.bgmSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        // clipIndex 증가
        clipIndex++;

        // 배열의 끝에 도달하면 0으로 초기화
        if (clipIndex >= AudioManager.Instance.bgmClips.Length)
        {
            clipIndex = 0;
        }
        // 출력에는 clipIndex를 사용
        Debug.Log(clipIndex + 1);
        AudioManager.Instance.PlayBGM(clipIndex, 1f);
    }
}
