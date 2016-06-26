using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    int EnemyID = 0;
    public GameObject Player,Enemy, SpawnPlace ;
    public GameObject sliderEnemy;
    public GameObject winScreen;

	void Start () {
        //Enemy = GameObject.Find("Prefabs\Seeker");
        Invoke("InstantiateEnemy", 1f);

    }
	
	
	void Update () {
	     
	}

   public  void InstantiateEnemy()
    {
        GameObject enemySpawn = Instantiate(Enemy, SpawnPlace.transform.position, Quaternion.identity) as GameObject;
        switch (EnemyID) {

            case 0:
        

        enemySpawn.GetComponent<Unit>().target = Player.transform;
        enemySpawn.GetComponent<HealthBeh>().healthSlider = sliderEnemy.GetComponent<Slider>();
                enemySpawn.GetComponent<HealthBeh>().gameManager = this.gameObject;
                enemySpawn.SetActive(true);
                sliderEnemy.SetActive(true);
        EnemyID ++;
        break;

            case 1:
             

                enemySpawn.GetComponent<Unit>().target = Player.transform;
                enemySpawn.GetComponent<HealthBeh>().healthSlider = sliderEnemy.GetComponent<Slider>();
                enemySpawn.GetComponent<HealthBeh>().gameManager = this.gameObject;
                sliderEnemy.SetActive(true);
                enemySpawn.GetComponent<HealthBeh>().DMG = 30f;
                enemySpawn.GetComponent<HealthBeh>().health = 150f;
                EnemyID++;
                break;
            case 2:


                enemySpawn.GetComponent<Unit>().target = Player.transform;
                enemySpawn.GetComponent<HealthBeh>().healthSlider = sliderEnemy.GetComponent<Slider>();
                enemySpawn.GetComponent<HealthBeh>().gameManager = this.gameObject;

                sliderEnemy.SetActive(true);
                enemySpawn.GetComponent<HealthBeh>().DMG = 50f;
                enemySpawn.GetComponent<HealthBeh>().health = 300f;
                EnemyID++;
                break;

            case 3:
                enemySpawn.SetActive(false);
                winScreen.SetActive(true);

                break;

        }
    } 
}
