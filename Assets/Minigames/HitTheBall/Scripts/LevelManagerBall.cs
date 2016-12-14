using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public enum GameStates { Start, Game, GameOver, Win }

public class LevelManagerBall : MonoBehaviour {

    // which player is active
    public bool player1turn;

    public int WinHits; // number of hits needed for the win

    public Text player1text;
    public Text player2text;
    public Text scoreText;
    public Text countdownText;

    public GameObject Player1;
    public GameObject Player2;

    public GameStates state;

    public GameObject ball;

    public int score;

    void Start () {
        score = 0;
        chooseLevel();
        player1text.enabled = true;
        player2text.enabled = false;
        ActivePlayer(Player1, Player2);
        
    }
	
	void Update () {
        CircleCollider2D[] handColliders;

        switch (state)
        {
            case GameStates.Game:
                if (player1turn)
                {
                    player1text.enabled = true;
                    player2text.enabled = false;

                    handColliders = Player1.GetComponentsInChildren<CircleCollider2D>();
                    foreach (CircleCollider2D collider in handColliders)
                    {
                        collider.enabled = true;
                    }

                    handColliders = Player2.GetComponentsInChildren<CircleCollider2D>();
                    foreach (CircleCollider2D collider in handColliders)
                    {
                        collider.enabled = false;
                    }

                    ActivePlayer(Player1, Player2);
                }
                else
                {
                    player1text.enabled = false;
                    player2text.enabled = true;

                    handColliders = Player1.GetComponentsInChildren<CircleCollider2D>();
                    foreach (CircleCollider2D collider in handColliders)
                    {
                        collider.enabled = false;
                    }

                    handColliders = Player2.GetComponentsInChildren<CircleCollider2D>();
                    foreach (CircleCollider2D collider in handColliders)
                    {
                        collider.enabled = true;
                    }

                    ActivePlayer(Player2, Player1);
                }

                // win : get the number of needed hits
                if (score >= WinHits)
                {
                    Win();
                    state = GameStates.Win;
                }
                if (!isBallInGame(ball))
                {
                    
                    GameOver();
                    state = GameStates.GameOver;
                }
                break;
            case GameStates.GameOver:

                break;
            case GameStates.Win:

                break;
        }
    }

    private bool isBallInGame(GameObject ball)
    {
        return (ball.GetComponent<Transform>().position.y >= 0);
    }

    void GameOver()
    {
        MGC.Instance.LoseMinigame();
        Debug.Log("Game is over!");
    }

    void Win()
    {
        MGC.Instance.WinMinigame();
        Debug.Log("You win!");
    }

    private void chooseLevel()
    {
        Debug.Log("Difficulty: " + MGC.Instance.selectedMiniGameDiff);
        switch (MGC.Instance.selectedMiniGameDiff)
        {
            case 0:
                WinHits = 10;
                break;
            case 1:
                WinHits = 15;
                break;
            case 2:
                WinHits = 5000;
                break;
            default:
                WinHits = 10;
                break;
        }
    }

    /// <summary>
    /// Makes deactive player transparent and active visible.
    /// </summary>
    /// <param name="activePlayer">visible player</param>
    /// <param name="deactivePlayer">transparent player</param>
    private void ActivePlayer(GameObject activePlayer, GameObject deactivePlayer)
    {
        foreach (SkinnedMeshRenderer renderer in deactivePlayer.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.material.shader = Shader.Find("Transparent/Diffuse");
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.4f);
        }
        foreach (SkinnedMeshRenderer renderer in activePlayer.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.material.shader = Shader.Find("Standard");
        }
    }
}
