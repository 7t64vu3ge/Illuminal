using UnityEngine;
using UnityEngine.Video;
public class tv_commertials : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] commertials;
    private int index = 0;

    void Start()
    {
        videoPlayer.clip = commertials[index];
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        index = (index + 1) % commertials.Length;
        vp.clip = commertials[index];
        vp.Play();
    }
}
