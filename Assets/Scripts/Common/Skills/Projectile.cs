using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed;
	private Vector3 dir;
	private Skill skill;
	private Unit owner;
	private Unit target;

	private float distance;
	private float travel;
	public int enemyPenetration;
	private Grid grid;
	/// <summary>
	/// Metodo para inicializar las variables del proyectil
	/// </summary>
	/// <param name="_owner">Owner.</param>
	/// <param name="_dir">Dir.</param>
	/// <param name="_enemyPenetration">Enemy penetration. Numero de enemigos a los que puede penetrar y dañar</param>
	/// <param name="_distance">Distance.</param>
	/// <param name="_skill">Skill. Habilidad que ha generado la bala</param>
	public void Ini(Unit _owner, Vector3 _dir,int _enemyPenetration,float _distance, Skill _skill)
	{
		owner = _owner;
		dir = _dir;
		skill = _skill;
		enemyPenetration = _enemyPenetration;
		//distance = Vector2.Distance(owner.thisTransform.position,owner.target.thisTransform.position);
		distance = _distance;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		angle += 90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		target = owner.target;
		travel = 0;
		grid = GameObject.Find("GameManager/PathFinder").GetComponent<Grid>();
	}
	// Update is called once per frame
	void Update () {
		//Si ya ha muerto el objetivo se elimina la bala
		/*if (target == null || !target.thisGameObject.activeInHierarchy)
			Destroy(gameObject);*/
		transform.position += dir * speed * Time.deltaTime;
		travel += speed * Time.deltaTime;
		Unit[] nearEnemies = grid.GetEnemiesArea (transform.position, 0.2f);
		if (nearEnemies != null && nearEnemies.Length > 0) {
			//Debug.Log ("Creep cerca");
			for (int i = 0; i < nearEnemies.Length && enemyPenetration > 0; i++) {
				skill.Attack (nearEnemies [i], owner);
				enemyPenetration--;
			}
			if (enemyPenetration <= 0) {
				Destroy (gameObject);
			}
		}

		if (travel >= distance) {
			//skill.Attack(owner);
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		skill.Attack (other.GetComponent<Unit> (), owner);
		enemyPenetration--;
		if (enemyPenetration <= 0)
			Destroy (gameObject);
	}
}
