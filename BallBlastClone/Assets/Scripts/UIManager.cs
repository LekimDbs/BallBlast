using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject GameoverPanel;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Percentage;
    public Slider Slider;
    public Button RestartButton;
    public TextMeshProUGUI CurrentScoreText;
    public GameObject SliderPos;
    public GameObject ScoreBoard;
    public GameObject PercPosition;
    public GameObject NextLvl;
    public GameObject WonPanel;
    public TextMeshProUGUI WonText;

    public TextMeshProUGUI CurrentLevelText;
    public TextMeshProUGUI NextLevelText;

    public static UIManager Instance;
    public float SliderPercent;

    public static bool Won;
    //Colors
    public Color[] BuyMenuColors;
    public Color[] LineColors;

    public Image Box;
    public Image Line;

    public GameObject FireSpeedPanel;
    public TextMeshProUGUI FireSpeed;
    public TextMeshProUGUI FireSpeedCost;
    public GameObject FirePowerPanel;
    public TextMeshProUGUI FirePower;
    public TextMeshProUGUI FirePowerCost;
    public GameObject CoinsPowerPanel;
    public TextMeshProUGUI CoinsPower;
    public TextMeshProUGUI CoinsPowerCost;
    public Animator CameraAnimator;
    public Animator BuyMenuAnimator;
    public TextMeshProUGUI CoinsText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Won = false;
        SetCurrentScore(TargetSpawner.score);
        SetLevelText();
        UpdateFireSpeedText();
        UpdateCoinsText();
    }
    public void StartGame()
    {
        CameraAnimator.SetBool("Started",true);
        BuyMenuAnimator.SetBool("Started",true);
    }
    public void UpdateCoinsText()
    {
        CoinsText.text = BuyManager.Coins.ToString();
    }

    public void UpdateFireSpeedText()
    {
        FireSpeed.text = BuyManager.FireSpeed.ToString()+" BPS";
        FireSpeedCost.text = BuyManager.FireSpeedCost.ToString();
    }
    public void UpdateFirePowerText()
    {
        FirePower.text = BuyManager.FirePower*100 +" %";
        FirePowerCost.text = BuyManager.FirePowerCost.ToString();
    }
    public void UpdateCoinsPowerText()
    {
        CoinsPower.text = BuyManager.CoinsPower*100+" %";
        CoinsPowerCost.text = BuyManager.CoinsPowerCost.ToString();
    }
    public void UpgradeFireSpeed()
    {
        
        BuyManager.Instance.UpgradeFireSpeed();
        UpdateFireSpeedText();
    }
    public void UpgradeFirePower()
    {
        BuyManager.Instance.UpgradeFirePower();
        UpdateFirePowerText();
    }
    public void UpgradeCoinsPower()
    {
        BuyManager.Instance.UpgradeCoinsPower();
        UpdateCoinsPowerText();
    }
    public void OpenFireSpeedPanel()
    {
        FireSpeedPanel.SetActive(true);
        FirePowerPanel.SetActive(false);
        CoinsPowerPanel.SetActive(false);
        Box.color = BuyMenuColors[0];
        Line.color = LineColors[0];
        UpdateFireSpeedText();

    }
    public void OpenFirePowerPanel()
    {
        FireSpeedPanel.SetActive(false);
        FirePowerPanel.SetActive(true);
        CoinsPowerPanel.SetActive(false);
        Box.color = BuyMenuColors[1];
        Line.color = LineColors[1];
        UpdateFirePowerText();
    }
    public void OpenCoinsPowerPanel()
    {
        FireSpeedPanel.SetActive(false);
        FirePowerPanel.SetActive(false);
        CoinsPowerPanel.SetActive(true);
        Box.color = BuyMenuColors[2];
        Line.color = LineColors[2];
        UpdateCoinsPowerText();
    }
    public void SetLevelText()
    {
        CurrentLevelText.text = SceneManager.GetActiveScene().buildIndex+1+"";
        NextLevelText.text = SceneManager.GetActiveScene().buildIndex+2+"";
    }

    public void GameOver()
    {
        GameoverPanel.GetComponent<Animator>().SetBool("isDead",true);
        StartCoroutine(SetRestartButtonOn());
    }

    IEnumerator SetRestartButtonOn()
    {
        yield return new WaitForSeconds(2f);
        RestartButton.gameObject.SetActive(true);
    }
    public void SetPercentage(float percent)
    {
        Percentage.text = Mathf.Floor(percent)+"% completed";
        
    }
    private void Update()
    {
        Slider.value = Mathf.Lerp(Slider.value, SliderPercent, Time.deltaTime*2f);
        if (Won)
        {
            ScoreBoard.transform.position = Vector2.MoveTowards(ScoreBoard.transform.position,PercPosition.transform.position,20f*Time.deltaTime);
        }
    }
    public void SetScore(float score)
    {
        Score.text = score.ToString();
    }
    public void Restart()
    {
        TargetSpawner.score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetCurrentScore(float points)
    {
        CurrentScoreText.text = points.ToString();
        CurrentScoreText.gameObject.GetComponent<Animator>().Play("Scale");
    }
    public void GameWon()
    {
        WonPanel.GetComponent<Animator>().SetBool("isWon", true);
        Won = true;
        int lvl = SceneManager.GetActiveScene().buildIndex + 1;
        WonText.text = "Level "+lvl+" completed";
        StartCoroutine(SetNextLevel());
    }
    IEnumerator SetNextLevel()
    {
        
        yield return new WaitForSeconds(2f);
        NextLvl.SetActive(true);
    }

    public void NextLevel()
    {
        DestroyInstance();
        TargetSpawner.Instance = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void DestroyInstance()
    {
        Instance = null;
    }
}
