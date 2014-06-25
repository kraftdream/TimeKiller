using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {
	public float time = 1f;	
	public float delay = 1f;			
	public int maxEnemyCount = 5;
	public GameObject[] enemies;
	public Camera mainCamera;

	private const int SPRITE_SHIFT_W = 178;
	private const int SPRITE_SHIFT_H = 200;
	private GameObject[] createdEnemies;
	private GameObject player;
	
	void Start ()
	{
		// Start calling the Spawn function repeatedly after a delay .
		InvokeRepeating("CreateEnemy", delay, time);
		createdEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void CreateEnemy ()
	{
		if (player == null)
						return;
		// Instantiate a random enemy.
		if (getEnemyCount () > maxEnemyCount)
						return;
		int width = Random.Range(SPRITE_SHIFT_W, Screen.width - SPRITE_SHIFT_W);
		int height = Random.Range(SPRITE_SHIFT_H, Screen.height - SPRITE_SHIFT_H);
		Vector3 randomPosition = mainCamera.ScreenToWorldPoint(new Vector3 (width, height));
		randomPosition.z = 0;
		enemies[0].transform.position = randomPosition;
		if (!IsOverlapping (enemies [0]))
				Instantiate (enemies [0], randomPosition, transform.rotation);
	}

	int getEnemyCount()
	{
		return (createdEnemies = GameObject.FindGameObjectsWithTag("Enemy")).Length;
	}

	bool IsOverlapping(GameObject newEnemy)
	{
		foreach (GameObject enemy in createdEnemies) {
			if(enemy.renderer.bounds.Intersects(newEnemy.renderer.bounds) || enemy.renderer.bounds.Intersects(player.renderer.bounds))		
				return true;
		}
		return false;
	}


}
