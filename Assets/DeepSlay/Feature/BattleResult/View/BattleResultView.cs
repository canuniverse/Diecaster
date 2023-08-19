using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DeepSlay
{
    public class BattleResultView : UIView
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _win;
        [SerializeField] private GameObject _loose;

        private LevelConfig _levelConfig;

        [Inject]
        private void Construct(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
        }

        public void ShowResult(bool isWin)
        {
            _content.SetActive(true);
            var resultObject = isWin ? _win : _loose;
            resultObject.SetActive(true);
        }

        public void Menu()
        {
            SceneManager.LoadScene("MenuScene");
        }
        
        public void Restart()
        {
            _levelConfig.Level = 0;
            SceneManager.LoadScene("MainScene");
        }

        public void NextLevel()
        {
            _levelConfig.Level++;
            _levelConfig.Level = Mathf.Clamp(_levelConfig.Level, 0, _levelConfig.LevelModels.Count - 1);
            SceneManager.LoadScene("MainScene");
        }
    }
}