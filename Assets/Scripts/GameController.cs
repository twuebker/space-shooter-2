using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
	public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
	public GUIText nextWaveText;
	public GUIText upgradeText;
	public GUIText gameStartText;

	public GameObject playerExplosion;

    private bool gameOver;
    private bool restart;

    public int score;
	public int PlayerDeathCount;

    void Start()
    {
		PlayerDeathCount = 0;
        gameOver = false;
        restart = false;
        gameOverText.text = "";
        restartText.text = "";
		nextWaveText.text = "";
		upgradeText.text = "";
        StartCoroutine( SpawnWaves() );
        score = 20;
        UpdateScore();
    }

    void Update()
    {
		hideGameStart();
        if( restart )
        {
            if( Input.GetKeyDown(KeyCode.R) )
            {
                //Application.LoadLevel(Application.loadedLevel);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
		//Upgrade Abfrage
		if (score >= 40 && !gameOver) {
			showUpgradeText();
			if (Input.GetKeyDown (KeyCode.U)) {
				hideUpgradeText ();
				GameObject player1Obj = GameObject.FindWithTag("Player");
				if (player1Obj != null) {
					PlayerController player1 = player1Obj.GetComponent<PlayerController> ();
					player1.increaseFireRate ();
				}
				GameObject player2Obj = GameObject.FindWithTag("Player2");
				if (player2Obj != null) {
					Player2Controller player2 = player2Obj.GetComponent<Player2Controller> ();
					player2.increaseFireRate ();
				}
				AddScore(-40);
			}
		}
		//GameOver bei Score kleiner als 0
		if (score < 0 && gameOver != true) {
			GameOver ();
			resetWaveText();
			GameObject player2Obj = GameObject.FindWithTag("Player2");
			if (player2Obj != null) {
				Player2Controller player2 = player2Obj.GetComponent<Player2Controller> ();
				Instantiate(playerExplosion, player2.transform.position, player2.transform.rotation);
				Destroy(player2.gameObject);
			}
			GameObject player1Obj = GameObject.FindWithTag("Player");
			if (player1Obj != null) {
				PlayerController player1 = player1Obj.GetComponent<PlayerController> ();
				Instantiate(playerExplosion, player1.transform.position, player1.transform.rotation);
				Destroy(player1.gameObject);
			}
		}
    }

    IEnumerator SpawnWaves()
    {
		int counter = 0;
        yield return new WaitForSeconds(startWait);
        while(true)
        {
			resetWaveText ();
            for (int i = 0; i < hazardCount; i++)
            {
				GameObject hazard = hazards[Random.Range (0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);

            }
			counter++;
			if (!gameOver) 
			{
				nextWaveCallText (counter);
			}
            yield return new WaitForSeconds(waveWait);

            if( gameOver )
            {
                restartText.text = "Press 'R' for Restart!";
                restart = true;
                break;
            }
        }
    }

	//score Methoden
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game over!";
		hideUpgradeText();
        gameOver = true;
    }
	//nextWave Text
	public void nextWaveCallText(int wave) {
		nextWaveText.text = "Wave " + wave + " completed!";

	}
	public void resetWaveText() {
		nextWaveText.text = "";
	}

	//upgrade Text 
	public void showUpgradeText() {
		upgradeText.text = "Press U to upgrade!";
	}

	public void hideUpgradeText() {
		upgradeText.text = "";
	}
	public void hideGameStart() {
		gameStartText.text = "";
	}
		
}
