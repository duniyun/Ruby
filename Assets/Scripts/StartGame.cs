using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 开始选择玩家
/// </summary>
public class StartGame : MonoBehaviour
{
    public Button btnOne;
    public Button btnTwo;
    public Button btnExit;
    private AudioSource m_audio;
    public int selectKey = 1;
    public Text text;

    private void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_audio.Play();
        ColorSelect();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectKey--;
            ColorSelect();
        };
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectKey++;
            ColorSelect();
        };
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            KeySelect();
        };

    }
    //选择的人数
    private void KeySelect()
    {
        switch (selectKey)
        {
            case 1:
                OneGame();
                break;
            case 2:
                TwoGame();
                break;
            case 0:
                ExitGame();
                break;
        }
    }
    //选择的颜色
    private void ColorSelect()
    {
        if (selectKey <= -1) { selectKey = 2; }
        selectKey = selectKey % 3;
        btnOne.GetComponent<Image>().color = new Color(1, 1, 1, 255);
        btnTwo.GetComponent<Image>().color = new Color(1, 1, 1, 255);
        btnExit.GetComponent<Image>().color = new Color(1, 1, 1, 255);
        switch (selectKey)
        {
            case 1:
                btnOne.GetComponent<Image>().color = new Color(0, 210, 20, 255);
                break;
            case 2:
                btnTwo.GetComponent<Image>().color = new Color(0, 210, 20, 255);
                break;
            case 0:
                btnExit.GetComponent<Image>().color = new Color(0, 210, 20, 255);
                break;
        }
    }
    private void OnEnable()
    {
        btnOne.onClick.AddListener(OneGame);
        btnTwo.onClick.AddListener(TwoGame);
        btnExit.onClick.AddListener(ExitGame);
    }
    private void OnDisable()
    {
        btnOne.onClick.RemoveListener(OneGame);
        btnTwo.onClick.RemoveListener(TwoGame);
        btnExit.onClick.RemoveListener(ExitGame);
        //m_audio.Stop();
    }

    private void OneGame()
    {
        SceneManager.LoadScene(1);
    }
    private void TwoGame()
    {
        //SceneManager.LoadScene(1);
        text.gameObject.SetActive(true);
    }

    private void ExitGame()
    {
# if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
