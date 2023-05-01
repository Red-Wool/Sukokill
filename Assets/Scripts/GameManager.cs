using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameBoard board;
    public ItemGenerator ig;

    public GameRoundDataStore roundData;

    public GameObject startMenu;
    public SpawnPosition[] playerSpawn;

    public TMP_Text playerCountText;
    public Slider playerCount;

    public TMP_Text[] winCountDisplay;

    public GameObject abilitySelectMenu;
    public Image[] abilityPlayerSelect;

    public GameObject gameMenu;
    public GameObject[] abilityPlayerDisplay;
    public Ability ignoreDisplay;
    public BlockPlace[] set;

    public GameObject endMenu;
    public TMP_Text winText;

    private int playerNum = 2;
    private bool playing = false, gameEnd;
    private string endGameText;

    private IEnumerator winAnim;

    private int playerAbilitySelect;

    public void Start()
    {
        //TURBO!
        //Time.timeScale = 3f;

        playerCount.value = roundData.playerNum;
        gameEnd = false;
        for (int i = 0; i < abilityPlayerSelect.Length; i++)
        {
            abilityPlayerSelect[i].sprite = roundData.playerAbility[i].displayImage;
        }

        for (int i = 0; i < winCountDisplay.Length; i++)
        {
            winCountDisplay[i].text = roundData.playerWin[i].ToString();
        }
    }

    public void SliderChange()
    {
        playerCountText.text = playerCount.value + " Players";
        playerNum = (int)playerCount.value;
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        ig.StartGame(board.board);

        roundData.playerNum = playerNum;

        gameMenu.SetActive(true);
        for (int i = 0; i < playerSpawn.Length; i++)
        {
            SpawnPosition p = playerSpawn[i];
            if (i < playerNum)
            {
                board.ReplacePosition(p.pos.x, p.pos.y, p.item);

                p.item.gameObject.SetActive(true);

                abilityPlayerDisplay[i].SetActive(roundData.playerAbility[i] != ignoreDisplay);
                p.item.GetComponent<Player>().ResetPlayer(roundData.playerAbility[i]);
            }
            else
                p.item.gameObject.SetActive(false);

        }

        StartCoroutine(TickWait());
    }

    public IEnumerator TickWait()
    {
        yield return 0;
        playing = true;
    }

    private void Update()
    {
        if (playing)
        {
            int count = 0;
            for (int i = 0; i < playerNum; i++)
            {
                if (playerSpawn[i].item.GetComponent<Player>().canMove)
                    count++;
            }
            //Debug.Log(count + " " + playerNum);
            if (count < 2)
            {
                ig.EndGame();
                gameMenu.SetActive(false);
                playing = false;

                Player p;
                bool tieFlag = true;
                for (int i = 0; i < playerNum; i++)
                {
                    p = playerSpawn[i].item.GetComponent<Player>();
                    if (p.canMove)
                    {
                        tieFlag = false;
                        playerNum = i + 1;
                        roundData.playerWin[i]++;

                        p.canMove = false;
                    }
                }
                
                endGameText = tieFlag ? "Tie!" : "Player " + playerNum + " Wins!";

                winAnim = WinAnim(endGameText);
                StartCoroutine(winAnim);

            }
        }
        else if (gameEnd && Input.GetKeyDown(KeyCode.Space)) {
            StopCoroutine(winAnim);

            winText.text = endGameText;
            endMenu.SetActive(true);
            Time.timeScale = 1f;
        }
        
    }

    public IEnumerator Countdown()
    {

        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator WinAnim(string resultText)
    {
        winText.text = resultText;
        yield return new WaitForSecondsRealtime(.1f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(.5f);
        gameEnd = true;
        Time.timeScale = .3f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(1f);
        endMenu.SetActive(true);


    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetWinCounter()
    {
        for (int i = 0; i < winCountDisplay.Length; i++)
        {
            roundData.playerWin[i] = 0;
            winCountDisplay[i].text = "0";
        }
    }

    public void SelectAbility(int player)
    {
        playerAbilitySelect = player;
        abilitySelectMenu.SetActive(true);
    }

    public void SetAbility(Ability a)
    {
        roundData.playerAbility[playerAbilitySelect] = a;
        abilitySelectMenu.SetActive(false);

        for (int i = 0; i < abilityPlayerSelect.Length; i++)
        {
            abilityPlayerSelect[i].sprite = roundData.playerAbility[i].displayImage;
        }
    }
}

[System.Serializable]
public class SpawnPosition
{
    public GridItem item;
    public Vector2Int pos;
}
