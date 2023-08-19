using UnityEngine.Device;
using UnityEngine.SceneManagement;

namespace DeepSlay
{
    public class StartScreenView : UIView
    {
        public void Play()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}