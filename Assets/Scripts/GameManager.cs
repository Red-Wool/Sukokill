using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameBoard board;
    public Map map;
    public ItemGenerator ig;

    public GameRoundDataStore roundData;

    public GameObject startMenu;
    public SpawnPosition[] playerSpawn;

    public TMP_Text playerCountText;
    public Slider playerCount;

    public TMP_Text[] winCountDisplay;
    public Button playButton;

    public GameObject abilitySelectMenu;
    public Image[] abilityPlayerSelect;
    public Image[] abilityPlayerSelectTwo;

    public GameObject gameMenu;
    public GameObject[] abilityPlayerPrimaryDisplay;
    public GameObject[] abilityPlayerSecondaryDisplay;
    public Image[] abilityFillDisplay;
    public Image[] abilityIconDisplay;
    public PlayerDisplay[] playerDisplays;
    public Ability ignoreDisplay;
    public BlockPlace[] set;

    public GameObject endMenu;
    public TMP_Text winText;
    private int playerNum = 2;
    private bool playing = false, gameEnd; public static bool Playing;
    private string endGameText;
    private bool primary;

    private IEnumerator winAnim;

    private int playerAbilitySelect;

    public void Start()
    {
        //TURBO!
        //Time.timeScale = 3f;

        for (int i = 0; i < roundData.activePlayers.Length; i++)
        {

        }

        for (int i = 0; i < roundData.character.Length; i++)
        {
            SelectCharacter(i, roundData.character[i]);
        }
        

        //playerCount.value = roundData.playerNum;
        gameEnd = false;
        for (int i = 0; i < abilityPlayerSelect.Length; i++)
        {
            abilityPlayerSelect[i].sprite = roundData.playerAbilityPrimary[i].displayImage;
        }
        for (int i = 0; i < abilityPlayerSelectTwo.Length; i++)
        {
            abilityPlayerSelectTwo[i].sprite = roundData.playerAbilitySecondary[i].displayImage;
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
        map = roundData.gameMap;

        board.UpdateMap(map);
        for (int i = 0; i < map.playerSpawn.Length; i++)
        {
            playerSpawn[i].pos = map.playerSpawn[i];
        }

        startMenu.SetActive(false);
        ig.StartGame(board.board);

        //roundData.playerNum = playerNum;

        gameMenu.SetActive(true);
        for (int i = 0; i < playerSpawn.Length; i++)
        {
            SpawnPosition p = playerSpawn[i];
            if (roundData.activePlayers[i])
            {
                board.ReplacePosition(p.pos.x, p.pos.y, p.item);

                p.item.gameObject.SetActive(true);

                abilityPlayerPrimaryDisplay[i].SetActive(roundData.playerAbilityPrimary[i] != ignoreDisplay);
                abilityPlayerSecondaryDisplay[i].SetActive(roundData.playerAbilitySecondary[i] != ignoreDisplay);
                p.item.GetComponent<Player>().ResetPlayer(roundData.character[i], roundData.playerAbilityPrimary[i], roundData.playerAbilitySecondary[i]);
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
        int players = 0;
        for (int i = 0; i < roundData.activePlayers.Length; i++)
        {
            if (roundData.activePlayers[i])
                players++;
        }
        playButton.interactable = players >= 2;

        Playing = playing;
        if (playing)
        {
            int count = 0;
            for (int i = 0; i < playerSpawn.Length; i++)
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
                for (int i = 0; i < playerSpawn.Length; i++)
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

    public void SetMap(Map map)
    {
        roundData.gameMap = map;
    }

    public void SelectAbility(int player)
    {
        playerAbilitySelect = player;
        abilitySelectMenu.SetActive(true);
        primary = true;
    }

    public void SelectAbility(int player, Ability ability)
    {
        playerAbilitySelect = player;
        primary = true;
        SetAbility(ability);
    }

    public void SelectSecondary(int player)
    {
        playerAbilitySelect = player;
        abilitySelectMenu.SetActive(true);
        primary = false;
    }

    public void SelectCharacter(int player, CharacterData data)
    {
        roundData.character[player] = data;

        abilityIconDisplay[player].color = data.baseColor;
        abilityFillDisplay[player].color = data.supportColor;
    }

    public void SetAbility(Ability a)
    {
        if (primary)
        {
            roundData.playerAbilityPrimary[playerAbilitySelect] = a;
            abilitySelectMenu.SetActive(false);

            for (int i = 0; i < abilityPlayerSelect.Length; i++)
            {
                abilityPlayerSelect[i].sprite = roundData.playerAbilityPrimary[i].displayImage;
            }
        }
        else
        {
            roundData.playerAbilitySecondary[playerAbilitySelect] = a;
            abilitySelectMenu.SetActive(false);

            for (int i = 0; i < abilityPlayerSelectTwo.Length; i++)
            {
                abilityPlayerSelectTwo[i].sprite = roundData.playerAbilitySecondary[i].displayImage;
            }
        }
    }
}

[System.Serializable]
public class SpawnPosition
{
    public GridItem item;
    public Vector2Int pos;
}
