using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBeh : MonoBehaviour {

    public float DMG;
    public float health = 100f;
    public float cooldown;
    public int layerHit;
    public Slider healthSlider;
    public GameObject gameover;
    public GameObject gameManager;
    bool isImunne;


    void Start () {
        healthSlider.value = health;
	}
	
	// Update is called once per frame
	void Update () {
        healthSlider.transform.position = transform.position + new Vector3 (0f,0f,2f);
	    if (healthSlider.value <= 0f)
        {
            if (this.gameObject.layer == 14 )
            {
                print("mrtav");
                gameover.SetActive(true);
            }
            healthSlider.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            if (this.gameObject.layer == 12) {
                gameManager.GetComponent<GameManager>().InstantiateEnemy();
            }
        }
	}

    public void TakeDamage(float damage)
    {
        healthSlider.value -= damage;
    }

    void OnTriggerStay (Collider coll)
    {
        if (coll.gameObject.layer == layerHit && !isImunne)
        {
            TakeDamage(DMG);
            isImunne = true;
            Invoke("CanTakeDMG", cooldown);
        } 
    }

    void CanTakeDMG()
    {
        isImunne = false;
    }
}
