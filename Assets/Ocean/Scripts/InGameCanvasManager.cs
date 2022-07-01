using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InGameCanvasManager : MonoBehaviour
{
    #region Fields

    [SerializeField] GetScripts m_GetScripts;

    [Header("Start Menu Panel")]
    [SerializeField] GameObject StartMenuPanel;
    [SerializeField] GameObject SettingsPanel;

    [SerializeField] RectTransform PlayText;

    [SerializeField] Button PlayButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button MusicButton;
    [SerializeField] Button SoundButton;

    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI GoldUnitText;

    [SerializeField] Sprite MusicEnableSprite;
    [SerializeField] Sprite MusicDisableSprite;
    [SerializeField] Sprite SoundEnableSprite;
    [SerializeField] Sprite SoundDisableSprite;

    [SerializeField] bool MusicCheck;
    [SerializeField] bool SoundCheck;
    [SerializeField] bool PlayTextEnlargeCheck;

    [SerializeField] float PlayTextEnlargeSpeed;

    [Header("InGamePanel Menu Panel")]
    [SerializeField] GameObject InGamePanel;
    [SerializeField] TextMeshProUGUI InGameLevelText;
    [SerializeField] TextMeshProUGUI InGameGoldUnitText;
    public RectTransform InGameTextEnlarge;
    [SerializeField] List<GameObject> InGameText;

    public Image BombPowerRedBar;
    [SerializeField] bool InGameTextEnlargeCheck;
    [SerializeField] bool OpenTextCheck;

    public int textcount = 0;

    [Header("NextLevel Menu Panel")]
    [SerializeField] GameObject NextMenuPanel;
    [SerializeField] RectTransform NextPanelTwoXGoldBar;

    [SerializeField] Button NextButton;
    [SerializeField] Button NextPanelTwoXGoldButton;

    [SerializeField] TextMeshProUGUI NextPanelLevelText;
    [SerializeField] TextMeshProUGUI NextPanelGoldText;
    [SerializeField] TextMeshProUGUI NextPanelTwoXGoldText;

    [SerializeField] RectTransform NextPanelGoldImage;
    [SerializeField] RectTransform NextPanelGoldBarImage;
    [SerializeField] RectTransform NextPanelTwoXGoldImage;
    [SerializeField] RectTransform NextPanelTwoXGoldImagePrefab;
    [SerializeField] List<RectTransform> NextPanelTwoXGoldImages = new List<RectTransform>();

    private bool NextGoAddGold;
    private bool PressedNextButton;
    private bool PressedNextTwoGoldButton;
    private bool OpenNextMenuPanelCheck;
    private float OpenNextMenuPanelTimer;

    [Header("Restart Menu Panel")]
    [SerializeField] GameObject RetryMenuPanel;
    [SerializeField] RectTransform RetryPanelTwoXGoldBar;

    [SerializeField] Button RetryPanelRetryButton;
    [SerializeField] Button RetryPanelTwoXGoldButton;

    [SerializeField] TextMeshProUGUI RetryPanelGoldText;
    [SerializeField] TextMeshProUGUI RetryPanelTwoXGoldText;
    [SerializeField] TextMeshProUGUI RetryPanelLevelText;

    [SerializeField] RectTransform RetryPanelGoldImage;
    [SerializeField] RectTransform RetryPanelGoldBarImage;
    [SerializeField] RectTransform RetryPanelTwoXGoldImage;
    [SerializeField] RectTransform RetryPanelTwoXGoldImagePrefab;
    [SerializeField] List<RectTransform> RetryPanelTwoXGoldImages = new List<RectTransform>();

    private bool GoAddGold;
    private bool PressedRetryButton;
    private bool PressedTwoGoldButton;
    private bool OpenRetryMenuPanelCheck;
    private float OpenRetryMenuPanelTimer;

    #endregion

    int Level;

    void Start()
    {
        Level = m_GetScripts.PlayerData.PlayerUIShowLevel;
        LevelText.text = "LEVEL " + (Level + 1).ToString();
        InGameLevelText.text = "LEVEL " + (Level + 1).ToString();
        NextPanelLevelText.text = "LEVEL " + (Level + 1).ToString();
        RetryPanelLevelText.text = "LEVEL " + (Level + 1).ToString();
        //GoldUnitText.text = m_GetScripts.PlayerData.PlayerGold.ToString();
        //InGameGoldUnitText.text = m_GetScripts.PlayerData.PlayerGold.ToString();

        PlayButton.onClick.AddListener(() => PressPlayButton());
        SettingsButton.onClick.AddListener(() => PressSettingsButton());
        MusicButton.onClick.AddListener(() => PresshMusicButton());
        SoundButton.onClick.AddListener(() => PressSoundButton());

        RetryPanelTwoXGoldButton.onClick.AddListener(() => RetryPanelPressTwoXButton());
        RetryPanelRetryButton.onClick.AddListener(() => PressRetryButton());

        NextPanelTwoXGoldButton.onClick.AddListener(() => NextPanelPressTwoXButton());
        NextButton.onClick.AddListener(() => PressNextButton());
    }

    void Update()
    {
        ChangePlayTextScale(Time.deltaTime / PlayTextEnlargeSpeed);
        ChangeTextScale(Time.deltaTime / PlayTextEnlargeSpeed);

        //if (GoAddGold)
        //{
        //    RetryPanelMoveGold();
        //}

        //if (NextGoAddGold)
        //{
        //    NextPanelMoveGold();
        //}

        if (OpenRetryMenuPanelCheck)
        {
            OpenRetryMenuPanelTimer += Time.deltaTime;
            if (OpenRetryMenuPanelTimer >= 3f)
            {
                //RetryPanelGoldText.text = m_GetScripts.PlayerData.PlayerGold.ToString();
                //RetryPanelTwoXGoldText.text = "100";
                OpenRetryMenuPanel();
                OpenRetryMenuPanelCheck = false;
            }
        }

        if (OpenNextMenuPanelCheck)
        {
            OpenNextMenuPanelTimer += Time.deltaTime;
            if (OpenNextMenuPanelTimer >= 1f)
            {
                //NextPanelGoldText.text = m_GetScripts.PlayerData.PlayerGold.ToString();
                //float gold = 100 * m_GetScripts.GameManager.FinishWalls[m_GetScripts.GameManager.WallNumb].GetComponent<Wall>().Coefficient;
                //NextPanelTwoXGoldText.text = ((int)gold).ToString();
                OpenNextMenuPanel();
                OpenNextMenuPanelCheck = false;
            }
        }
    }

    public void Initialize()
    {
        PlayTextEnlargeCheck = true;
        InGameTextEnlargeCheck = true;

        OpenRetryMenuPanelCheck = false;
        PressedRetryButton = false;
        PressedTwoGoldButton = false;

        OpenNextMenuPanelCheck = false;
        PressedNextButton = false;
        PressedNextTwoGoldButton = false;

        StartMenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    void PressPlayButton()
    {
        StartMenuPanel.SetActive(false);
        InGamePanel.SetActive(true);
        m_GetScripts.GameManager.HasTheGameStarted = true;
    }

    #region NextOrTwoXGoldButtonPress
    void PressNextButton()
    {
        //PlayerPrefs.SetInt("PlayerGold", (int)((100 * m_GetScripts.GameManager.FinishWalls[m_GetScripts.GameManager.WallNumb].GetComponent<Wall>().Coefficient) + m_GetScripts.PlayerData.PlayerGold));

        //if (!PressedNextTwoGoldButton)
        //{
        //    StartCoroutine(NextPanelSpawnGold());
        //}

        //if (!NextPanelTwoXGoldButton.gameObject.activeInHierarchy && PressedNextTwoGoldButton)
        //{
        //    int a = PlayerPrefs.GetInt("PlayerLevel");
        //    SceneManager.LoadScene(a);
        //}
        PlayerPrefs.SetInt("PlayerLevel", PlayerPrefs.GetInt("PlayerLevel") + 1);
        PlayerPrefs.SetInt("PlayerShowLevel", m_GetScripts.PlayerData.PlayerUIShowLevel + 1);

        SceneManager.LoadScene(m_GetScripts.PlayerData.GetLevel(false));

        NextButton.gameObject.SetActive(false);
        NextPanelTwoXGoldButton.gameObject.SetActive(false);
        PressedNextButton = true;
    }

    void OpenNextMenuPanel()
    {
        NextMenuPanel.SetActive(true);
    }

    void NextPanelPressTwoXButton()
    {
        PressedNextTwoGoldButton = true;
        StartCoroutine(NextPanelSpawnGold());
    }

    IEnumerator NextPanelSpawnGold()
    {
        int a = 0;
        NextGoAddGold = true;
        NextPanelTwoXGoldButton.gameObject.SetActive(false);

        while (a != 11)
        {
            a++;
            int b = Random.Range(-200, 100);
            int c = Random.Range(-50, 50);
            RectTransform NewGoldImage = Instantiate(NextPanelTwoXGoldImagePrefab, Vector3.zero, NextPanelTwoXGoldImagePrefab.rotation);
            NewGoldImage.transform.parent = NextPanelTwoXGoldBar.transform;
            NewGoldImage.localScale = new Vector3(1, 1, 1);
            NewGoldImage.transform.localPosition = new Vector3(b, c, NextPanelTwoXGoldImagePrefab.localPosition.z);
            NextPanelTwoXGoldImages.Add(NewGoldImage);
            yield return new WaitForSeconds(0.1f);
        }
    }

    int c = 0;
    void NextPanelMoveGold()
    {
        if (!NextPanelTwoXGoldButton.gameObject.activeInHierarchy && PressedNextButton && PressedNextTwoGoldButton)
        {
            int a = PlayerPrefs.GetInt("PlayerLevel");
            //SceneManager.LoadScene(a);
        }

        NextPanelTwoXGoldImages[c].parent = NextPanelGoldBarImage.transform;
        NextPanelTwoXGoldImages[c].localPosition = Vector3.MoveTowards(NextPanelTwoXGoldImages[c].localPosition, NextPanelGoldImage.transform.localPosition, Time.deltaTime * 3000);
        if (NextPanelTwoXGoldImages[c].localPosition.x == NextPanelGoldImage.transform.localPosition.x)
        {
            c++;
            if (c == 11)
            {
                NextGoAddGold = false;
                if (PressedNextButton)
                {
                    int a = PlayerPrefs.GetInt("PlayerLevel");
                    //SceneManager.LoadScene(a);
                }
            }
        }
    }

    #endregion

    #region RetryOrTwoXGoldButtonPress

    void PressRetryButton()
    {
        //PlayerPrefs.SetInt("PlayerGold", m_GetScripts.PlayerData.PlayerGold + 100);

        //if (!PressedTwoGoldButton)
        //{
        //    StartCoroutine(RetryPanelSpawnGold());
        //}

        //if (!RetryPanelTwoXGoldButton.gameObject.activeInHierarchy && PressedTwoGoldButton)
        //{
        //    int a = PlayerPrefs.GetInt("PlayerLevel");
        //    SceneManager.LoadScene(a);
        //}

        SceneManager.LoadScene(m_GetScripts.PlayerData.GetLevel(true));

        RetryPanelRetryButton.gameObject.SetActive(false);
        RetryPanelTwoXGoldButton.gameObject.SetActive(false);
        PressedRetryButton = true;
    }

    void OpenRetryMenuPanel()
    {
        RetryMenuPanel.SetActive(true);
    }

    void RetryPanelPressTwoXButton()
    {
        PressedTwoGoldButton = true;
        StartCoroutine(RetryPanelSpawnGold());
    }

    IEnumerator RetryPanelSpawnGold()
    {
        int a = 0;
        GoAddGold = true;
        RetryPanelTwoXGoldButton.gameObject.SetActive(false);

        while (a != 11)
        {
            a++;
            int b = Random.Range(-200, 100);
            int c = Random.Range(-50, 50);
            RectTransform NewGoldImage = Instantiate(RetryPanelTwoXGoldImagePrefab, Vector3.zero, RetryPanelTwoXGoldImagePrefab.rotation);
            NewGoldImage.transform.parent = RetryPanelTwoXGoldBar.transform;
            NewGoldImage.localScale = new Vector3(1, 1, 1);
            NewGoldImage.transform.localPosition = new Vector3(b, c, RetryPanelTwoXGoldImagePrefab.localPosition.z);
            RetryPanelTwoXGoldImages.Add(NewGoldImage);
            yield return new WaitForSeconds(0.1f);
        }
    }

    int b = 0;
    void RetryPanelMoveGold()
    {
        if (!RetryPanelTwoXGoldButton.gameObject.activeInHierarchy && PressedRetryButton && PressedTwoGoldButton)
        {
            int a = PlayerPrefs.GetInt("PlayerLevel");
            //SceneManager.LoadScene(a);
        }

        RetryPanelTwoXGoldImages[b].parent = RetryPanelGoldBarImage.transform;
        RetryPanelTwoXGoldImages[b].localPosition = Vector3.MoveTowards(RetryPanelTwoXGoldImages[b].localPosition, RetryPanelGoldImage.transform.localPosition, Time.deltaTime * 3000);
        if (RetryPanelTwoXGoldImages[b].localPosition.x == RetryPanelGoldImage.transform.localPosition.x)
        {
            b++;
            if (b == 11)
            {
                GoAddGold = false;
                if (PressedRetryButton)
                {
                    int a = PlayerPrefs.GetInt("PlayerLevel");
                    //SceneManager.LoadScene(a);
                }
            }
        }
    }

    #endregion

    #region SettingsButton

    void PressSettingsButton()
    {
        if (SettingsPanel.activeInHierarchy)
        {
            SettingsPanel.SetActive(false);
        }
        else
        {
            SettingsPanel.SetActive(true);
        }

        SetMusicOrSoundSprite(MusicCheck, MusicButton, MusicEnableSprite, MusicDisableSprite);
        SetMusicOrSoundSprite(SoundCheck, SoundButton, SoundEnableSprite, SoundDisableSprite);
    }

    void PresshMusicButton()
    {
        ChangeMusicOrSoundSprite(ref MusicCheck, MusicButton, MusicEnableSprite, MusicDisableSprite);
    }

    void PressSoundButton()
    {
        ChangeMusicOrSoundSprite(ref SoundCheck, SoundButton, SoundEnableSprite, SoundDisableSprite);
    }

    void ChangeMusicOrSoundSprite(ref bool howBut, Button howImage, Sprite on, Sprite off)
    {
        howBut = !howBut;
        SetMusicOrSoundSprite(howBut, howImage, on, off);
    }

    void SetMusicOrSoundSprite(bool howBut, Button howImage, Sprite on, Sprite off)
    {
        if (howBut)
        {
            howImage.GetComponent<Image>().sprite = on;
        }
        else
        {
            howImage.GetComponent<Image>().sprite = off;
        }
    }

    #endregion

    void ChangePlayTextScale(float speed)
    {
        if (PlayTextEnlargeCheck)
        {
            PlayText.localScale = new Vector3(PlayText.localScale.x + speed, PlayText.localScale.y + speed, PlayText.localScale.z);

            if (PlayText.localScale.x >= 1.2f)
            {
                PlayTextEnlargeCheck = false;
            }
        }
        else
        {
            PlayText.localScale = new Vector3(PlayText.localScale.x - speed, PlayText.localScale.y - speed, PlayText.localScale.z);

            if (PlayText.localScale.x <= 1f)
            {
                PlayTextEnlargeCheck = true;
            }
        }
    }

    void ChangeTextScale(float speed)
    {
        if (InGameTextEnlargeCheck)
        {
            InGameTextEnlarge.localScale = new Vector3(InGameTextEnlarge.localScale.x + speed, InGameTextEnlarge.localScale.y + speed, InGameTextEnlarge.localScale.z);

            if (InGameTextEnlarge.localScale.x >= 1.2f)
            {
                InGameTextEnlargeCheck = false;
            }
        }
        else
        {
            InGameTextEnlarge.localScale = new Vector3(InGameTextEnlarge.localScale.x - speed, InGameTextEnlarge.localScale.y - speed, InGameTextEnlarge.localScale.z);

            if (InGameTextEnlarge.localScale.x <= 1f)
            {
                InGameTextEnlargeCheck = true;
            }
        }
    }

    public IEnumerator WindSpeedText()
    {
        ChangeText();

        yield return new WaitForSeconds(1f);

        ChangeText();

        yield return new WaitForSeconds(1f);

        ChangeText();
    }

    int t = 0;
    private void ChangeText()
    {
        for (int i = 0; i < InGameText.Count; i++)
        {
            InGameText[i].SetActive(false);
        }

        t = Random.Range(0, InGameText.Count);

        for (int i = 0; i < InGameText.Count; i++)
        {
            if (i == t)
            {
                InGameText[i].SetActive(true);
            }
        }
    }

    public void CanvasGameOver()
    {
        OpenRetryMenuPanelCheck = true;
        InGamePanel.SetActive(false);
        NextMenuPanel.SetActive(false);
    }

    public void CanvasFinishGame()
    {
        OpenNextMenuPanelCheck = true;
        InGamePanel.SetActive(false);
        RetryMenuPanel.SetActive(false);
    }
}