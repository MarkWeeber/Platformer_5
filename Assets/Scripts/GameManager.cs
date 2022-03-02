using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace Platformer.Inputs
{
    public class GameManager : MonoBehaviour
    {
        private int collectiblesTotalCounter = 0;
        public int enemySlayCounter = 0;
        public int secretsFoundCounter = 0;
        private int goldCointCounter = 0;
        public int GoldCointCounter
        {
            get{ return goldCointCounter; }
            set{ 
                    collectiblesTotalCounter += value - goldCointCounter;
                    goldCointCounter = value;
                    UpdateCollectiblesUICointer();
                }
        }
        private int blueGemCounter = 0;        
        public int BlueGemCounter
        {
            get{ return blueGemCounter; }
            set{ 
                    collectiblesTotalCounter += value - blueGemCounter;
                    blueGemCounter = value;
                    UpdateCollectiblesUICointer();
                }
        }

        [SerializeField] private int targetFPS = 60;
        [SerializeField] private Text coinsCountTextSuccess = null;
        [SerializeField] private Text enemySlayCountTextSuccess = null;
        [SerializeField] private Text secretsCountsTextSuccess = null;
        [SerializeField] private Text coinsCountTextGameOver = null;
        [SerializeField] private Text enemySlayCountTextGameOver = null;
        [SerializeField] private Text secretsCountsTextGameOver = null;
        [SerializeField] private Text collectibleGoldCoinText = null;
        [SerializeField] private Text collectibleBlueGemText = null;
        [SerializeField] private CanvasRenderer inGameMenu = null;
        [SerializeField] private CanvasRenderer creditsMenu = null;
        [SerializeField] private CanvasRenderer gameOverMenu = null;
        [SerializeField] private Text fpsCounter = null;
        [SerializeField] private CanvasRenderer[] levelButtonsExceptForIntroLevel = null;
        [SerializeField] private CanvasRenderer announcerPanel = null;
        [SerializeField] private Text announcerText = null;
        [SerializeField] private float announceTime = 6f;
        private ProgressData progressData = null;
        private int currentSceneIndex = 0;
        private int totalAmountOfScenes = 0;
        private int totalAmountOfSecrets, totalAmountOfCollectibles, totalAmountOfEnemies = 0;

        private void Start()
        {
            //Debug.Log(Application.persistentDataPath);
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            totalAmountOfScenes = SceneManager.sceneCountInBuildSettings;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFPS;
            #if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = targetFPS;
            #endif
            if(fpsCounter != null)
            {
                InvokeRepeating("UpdateFPS", 1, 1);
            }
            // managing save system
            progressData = SaveSystem.LoadProgress();
            // on main menu
            if(currentSceneIndex == 0)
            {
                if(progressData != null)
                {
                    int index = 0;
                    while(index < progressData.levels.Length)
                    {
                        levelButtonsExceptForIntroLevel[index].gameObject.SetActive(progressData.levels[index]);
                        index++;
                    }
                }
            }
            // new level - save current level, the 1st index is for intro level
            if(currentSceneIndex > 1)
            {
                // save already exists
                if(progressData != null)
                {
                    progressData.levels[currentSceneIndex - 2] = true; // one for main menu level another for intro level
                    SaveSystem.SaveProgress(progressData);
                }
                // new save
                else
                {
                    SaveSystem.SaveProgressNew(totalAmountOfScenes -2, currentSceneIndex - 2); // one for main menu level another for intro level
                }
            }
            Time.timeScale = 1;
            // getting total ammount of enemies, secrets and collectibles
            Component[] _amount = FindObjectsOfType<Collectible>();
            foreach (Collectible _item in _amount)
            {
                if(_item.collectibleType == Collectible.CollectibleType.Secret)
                {
                    totalAmountOfSecrets++;
                }
                else
                {
                    totalAmountOfCollectibles++;
                }
            }
            _amount = FindObjectsOfType<EnemySimpleAI>();
            totalAmountOfEnemies = _amount.Length;
            _amount = null;
        }

        private void UpdateFPS()
        {
            fpsCounter.text = "FPS " + (int)(1f / Time.unscaledDeltaTime);
        }
        public void GoToMainMenu()
        {
            Time.timeScale = 1;
            ResetCounters();
            SceneManager.LoadScene(0);
        }

        public void GoToSpecificLevel(int buildIndex = -1)
        {
            Time.timeScale = 1;
            ResetCounters();
            // if user did not specify any buildIndex then just load current scene
            if (buildIndex == -1)
                buildIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(buildIndex);
        }

        public void RestartLevel()
        {
            Time.timeScale = 1;
            ResetCounters();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            if (totalAmountOfScenes == currentSceneIndex + 1)
            {
                SceneManager.LoadScene(0); // if this is the last scene
            }
            else
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
        }

        public void PutOnPause()
        {
            Time.timeScale = 0;
        }

        public void Resume()
        {
            Time.timeScale = 1;
        }

        public void LevelComplete()
        {
            coinsCountTextSuccess.text = collectiblesTotalCounter.ToString() + " / " + totalAmountOfCollectibles.ToString();
            enemySlayCountTextSuccess.text = enemySlayCounter.ToString() + " / " + totalAmountOfEnemies.ToString();
            secretsCountsTextSuccess.text = secretsFoundCounter.ToString() + " / " + totalAmountOfSecrets.ToString();
            Time.timeScale = 0;
            inGameMenu.gameObject.SetActive(false);
            creditsMenu.gameObject.SetActive(true);
        }

        public void GameOver()
        {
            coinsCountTextGameOver.text = collectiblesTotalCounter.ToString() + " / " + totalAmountOfCollectibles.ToString();
            enemySlayCountTextGameOver.text = enemySlayCounter.ToString() + " / " + totalAmountOfEnemies.ToString();
            secretsCountsTextGameOver.text = secretsFoundCounter.ToString() + " / " + totalAmountOfSecrets.ToString();
            Time.timeScale = 0;
            inGameMenu.gameObject.SetActive(false);
            gameOverMenu.gameObject.SetActive(true);
        }

        private void ResetCounters()
        {
            collectiblesTotalCounter = 0;
            enemySlayCounter = 0;
            secretsFoundCounter = 0;
            goldCointCounter = 0;
            blueGemCounter = 0;
        }

        public void UpdateCollectiblesUICointer()
        {
            collectibleGoldCoinText.text = goldCointCounter.ToString();
            collectibleBlueGemText.text = blueGemCounter.ToString();
        }

        public void AnnounceMessage(string text)
        {
            announcerPanel.gameObject.SetActive(true);
            announcerText.text = text;
            Invoke(nameof(ResetAnnouncer), announceTime);
        }

        private void ResetAnnouncer()
        {
            announcerPanel.gameObject.SetActive(false);
            announcerText.text = "";
        }
    }
}