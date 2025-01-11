using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class FinishLevel : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject tapToPlayText;
        [SerializeField] private GameObject finalLevelPanel;
        private bool _levelFinished = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand") && !_levelFinished)
            {
               _levelFinished = true; 
                Invoke(nameof(ShowLevelCompletePanel),.2f);
            }
        }

        private void ShowLevelCompletePanel()
        {
            levelCompletePanel.SetActive(true);
            tapToPlayText.SetActive(false);
            Time.timeScale = 0f;
        }

        public void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            {
                levelCompletePanel.SetActive(false);
                tapToPlayText.SetActive(false);
                finalLevelPanel.SetActive(true); 
                Time.timeScale = 0f;
                return;
            }
            
            levelCompletePanel.SetActive(false);
            tapToPlayText.SetActive(true);
            Time.timeScale = 1f; 
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}