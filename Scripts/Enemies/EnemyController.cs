using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{

    public Enemy enemy;
    //private float timeTillNextMove = 0;
    [SerializeField] private bool canMove = true;
    private Vector3 targetPos;
    public TextMesh enemyInfoText;

    //scaled enemy variables;
    public string enemyName = "";
    public int enemyLevel = 0;
    public int enemyDamage = 0;
    public int enemyEvasion = 0;
    public int enemyHealth = 0;
    public float enemyMoveTime = 0;
    public int enemyID = 0;
    public Sprite sprite;
    public bool boss;
    public float moveSpeed = 0;
    public float perfectAttackChance = 0;

    public GameObject movementParticle;
    public Vector3 particleOffset;

    private void Start()
    {
        targetPos = transform.position;
    }

    void Update()
    {
        /*
        if (transform.position == GameManager.instance.player.transform.position)
        {
            AttackPlayer();
        }*/
       
        /*
        if (new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z)) 
            == new Vector3(Mathf.RoundToInt(GameManager.instance.player.transform.position.x),
            Mathf.RoundToInt(GameManager.instance.player.transform.position.y),
            Mathf.RoundToInt(GameManager.instance.player.transform.position.z)))
        {
            AttackPlayer();
        }*/

        if (Vector3.Distance(gameObject.transform.position, GameManager.instance.player.transform.position) <= 1 && GameManager.instance.playerTeleporting == false)
        {
            AttackPlayer();
        }

        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, enemy.GetEnemyMoveSpeed() * Time.deltaTime);
            //Instantiate(movementParticle, transform.position + particleOffset, Quaternion.Euler(new Vector3(90, 0, 0)));
        }

        /*
        if (Vector3.Distance(transform.position, targetPos) == 0f)
        {

            RandomMove();
        }*/

        if (transform.position == targetPos)
        {
            RandomMove();
        }

        /*
        if (Time.time >= timeTillNextMove && canMove && enemy.GetEnemyMoveTime() != 0)
        {
            transform.position = RandomMove();
            
            timeTillNextMove = Time.time + enemy.GetEnemyMoveTime();
        }*/
    }

    public void SetEnemy(Enemy newEnemy)
    {
        enemy = newEnemy;
        gameObject.GetComponent<SpriteRenderer>().sprite = newEnemy.GetSprite();
        //get scaled enemy and set variables

        enemyName = newEnemy.GetEnemyName();
        enemyLevel = newEnemy.GetEnemyLevel();
        enemyDamage = newEnemy.GetEnemyDamage();
        enemyEvasion = newEnemy.GetEnemyEvasion();
        enemyHealth = newEnemy.GetEnemyHealth();
        enemyMoveTime = newEnemy.GetEnemyMoveTime();
        enemyID = newEnemy.GetEnemyID();
        sprite = newEnemy.GetSprite();
        boss = newEnemy.IsBoss();
        moveSpeed = newEnemy.GetEnemyMoveSpeed();
        perfectAttackChance = newEnemy.GetPerfectAttackChance();
    }

    public void SetEnemyInfo()
    {
        enemyLevel = enemy.GetEnemyLevel();
        enemyDamage = enemy.GetEnemyDamage();
        enemyHealth = enemy.GetEnemyHealth();
        enemyInfoText.text = "Lv. " + enemyLevel + " " + enemyName;

    }

    public Vector3 RandomMove()
    {
        Vector3 newPos = new Vector3();

        //move in a random direction

        int randomAxis = Random.Range(0,4);
        if (randomAxis == 0)
        {
            newPos = new Vector3(transform.position.x + 1 , transform.position.y, transform.position.z);
        } else if (randomAxis == 1)
        {
            newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        } else if (randomAxis == 2)
        {
            newPos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        else if (randomAxis == 3)
        {
            newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }

        //check if new pos is on the map
        if (newPos.x > GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1 || newPos.z > GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1 || newPos.x < 0 || newPos.z < 0)
        {
            return transform.position;
        }

        if (GameManager.instance.GetOccupiedPositions().Contains(newPos))
        {
            return transform.position;
        }
        newPos.y = 6;
        GameManager.instance.AppendOccipiedPositions(true, newPos);
        GameManager.instance.AppendOccipiedPositions(false, transform.position);
        targetPos = newPos;
        return newPos;
    }

    public void AttackPlayer()
    {
        //only attack player if the player is not in combat
        if (!GameManager.instance.player.GetComponent<PlayerController>().GetCombatStatus())
        {
            canMove = false;
            GameManager.instance.GetComponent<CombatManager>().StartEncounter(gameObject);
        }
    }

    /*
    public Enemy GetEnemy()
    {
        return enemy;
    }*/

    public void AppendEnemyHealth(int amount)
    {
        enemyHealth += amount;
    }
}
