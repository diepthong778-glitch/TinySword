using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    public AudioClip mainBGM;
    void Awake()
    {
        GlobalSound.Instance?.PlayBGM(mainBGM);
    }
}
