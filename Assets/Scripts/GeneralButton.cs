using UnityEngine;

public class GeneralButton : MonoBehaviour
{
    public void playSound(AudioClip buttonClickSound)
    {
        GlobalSound.Instance?.PlaySound(buttonClickSound);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void muteSound(bool isMuted)
    {
        GlobalSound.Instance?.SetMute(isMuted);
    }
}
