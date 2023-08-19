using UnityEngine.Device;
using UnityEngine.SceneManagement;
using Zenject;

namespace DeepSlay
{
    public class StartScreenView : UIView
    {
        private LevelConfig _levelConfig;
        
        [Inject]
        private void Construct(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
        }
        
        public void Play()
        {
            _levelConfig.Level = 0;
            SceneManager.LoadScene("MainScene");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}