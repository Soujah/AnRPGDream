using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    #region Singleton

    public static CombatManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Item Generator Found");
        }
        instance = this;
    }
    #endregion

    [SerializeField] private GameObject enemyObject;
    [SerializeField] private EnemyController enemy;

    public GameObject UIManager;
    public GameObject combatUI;

    private GameObject player;

    //combat ui
    public TextMeshProUGUI combatTextBox;
    public TextMeshProUGUI enemyStats;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerDamage;
    public TextMeshProUGUI playerArmor;
    public Image enemyImage;

    public GameObject continueButton;

    public GameObject attackSliderObject;
    public GameObject attackButton;
    public GameObject escapeButton;
    public Slider attackSlider;
    public float sliderMoveTime;
    private float timeToMoveSlider;
    public float minPerfectAttackDefault;
    public float maxPerfectAttackDefault;
    [Range(0,30)]public int perfectAttackAccuracyBonus;
    public float perfectAttackDamageBonus;
    public bool perfectAttack;
    private bool leftToRight;

    //attack phase
    private bool canAttack = true;

    public int groundHeightAttackBonus;

    private float playerGroundHeight;
    private float enemyGroundHeight;

    float maxPerfectAttack = 0.6f;
    float minPerfectAttack = 0.4f;

    public Image attackSliderHandle;

    public Animator combatAnimator;

    public GameObject sliderBackground;
    public Texture2D texture2D;

    public GameObject heightAdvantageIcon;


    //particle Effects
    public ParticleSystem hitParticle;
    public ParticleSystem perfectHitParticle;


    //combat sounds
    public AudioSource SFXSource;
    public AudioClip[] SFXClips;

    //life steal

    public float lifeStealAmount = 0.1f;

    public ParticleSystem playerDamagedParticle;

    void Start()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (enemy != null)
        {
            SliderMover();
        }
    }

    public void SliderMover()
    {
        /*
        if (Time.time > timeToMoveSlider)
        {
            
            timeToMoveSlider = Time.time + sliderMoveTime;
        }*/

        if (leftToRight)
        {
            attackSlider.value = Mathf.Lerp(attackSlider.value, attackSlider.value + 0.1f, enemy.perfectAttackChance * Time.deltaTime);
            if (attackSlider.value >= 1)
            {
                leftToRight = false;
            }

        }
        else
        {
            attackSlider.value = Mathf.Lerp(attackSlider.value, attackSlider.value - 0.1f, enemy.perfectAttackChance * Time.deltaTime);
            if (attackSlider.value <= 0)
            {
                leftToRight = true;
            }

        }
        //change colour of slider background


        //

        //change colour of handle;

        if (attackSlider.value < maxPerfectAttack && attackSlider.value > minPerfectAttack)
        {
            //green
            attackSliderHandle.color = new Color(0, 1, 0);
        } else
        {
            //red
            attackSliderHandle.color = new Color(1, 0, 0);
        }


    }

    public void SetEnemy(GameObject newEnemy)
    {
        enemyObject = newEnemy;
        enemy = newEnemy.GetComponent<EnemyController>();
        //set slider move time
        sliderMoveTime = enemy.perfectAttackChance;
    }

    public void ActivateCombatUI()
    {
        //activate combat ui and set combat ui values
        UIManager.GetComponent<ExitToMenu>().LoadUI(combatUI);

        //set ui variables
        enemyStats.text = CompileEnemyStats(enemy);
        enemyImage.sprite = enemy.sprite;
        playerHealth.text = "Health \n" + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerHealth().ToString();
        playerDamage.text = "Damage \n" + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerDamage().ToString();
        playerArmor.text = "Armor \n" + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerArmor().ToString();
        attackSliderObject.SetActive(true);
        attackButton.SetActive(true);
        escapeButton.SetActive(true);

    }

    public void Attack()
    {
        if (canAttack && enemy != null)
        {
            

            //miss chance
            attackSliderObject.SetActive(false);

            //set attack and escape buttons to be inactive
            attackButton.SetActive(false);
            escapeButton.SetActive(false);

            maxPerfectAttack = 0.6f;
            minPerfectAttack = 0.4f;
            float weaponTypeDamage = 0;

            if (GameManager.instance.player.GetComponent<PlayerManager>().GetWeapon() != null)
            {
                //player has a weapon
                maxPerfectAttack = GameManager.instance.player.GetComponent<PlayerManager>().GetWeapon().maxAccuracy; 
                minPerfectAttack = GameManager.instance.player.GetComponent<PlayerManager>().GetWeapon().minAccuracy;
                weaponTypeDamage = GameManager.instance.player.GetComponent<PlayerManager>().GetWeapon().weaponTypeDamage;
            }         

            if (attackSlider.value < maxPerfectAttack && attackSlider.value > minPerfectAttack)
            {
                Debug.Log("perfect attack");
                //Debug.Log(maxPerfectAttack + " : " + minPerfectAttack);
                perfectAttack = true;
            } else
            {
                //Debug.Log("imperfect attack");
                perfectAttack = false;
            }

            int levelDifference = enemy.enemyLevel - player.GetComponent<PlayerManager>().GetPlayerLevel();
            if (levelDifference <= 0)
            {
                levelDifference = 0;
            }
            bool hitChance = false;

            if (Random.Range(0, levelDifference + 100) > 30 - perfectAttackAccuracyBonus)
            {
                hitChance = true;
            }

            if (hitChance == true)
            {
                //attack the target enemy
                float damage = (perfectAttack ? perfectAttackDamageBonus : 1f);
                float finalDamage = damage * (player.GetComponent<PlayerManager>().GetPlayerDamage());

                if (playerGroundHeight > enemyGroundHeight)
                {
                    finalDamage += groundHeightAttackBonus;
                    Debug.Log("Player height advantage: " + playerGroundHeight + " Enemy Height: " + enemyGroundHeight);
                }

                if (perfectAttack)
                {
                    finalDamage += weaponTypeDamage;
                }

                //Debug.Log("final damage: " + finalDamage);

                enemy.AppendEnemyHealth(-Mathf.RoundToInt(finalDamage));
                combatTextBox.text = "Dealt " + Mathf.RoundToInt(finalDamage) + " to " + enemy.enemyName;
                

                //play particle effects
                if (perfectAttack)
                {
                    perfectHitParticle.Play();
                } else
                {
                    hitParticle.Play();
                }

                //restore player health for perfect attack

                if (perfectAttack)
                {
                    float lifeSteal = finalDamage * lifeStealAmount;
                    player.GetComponent<PlayerManager>().AppendPlayerHealth(Mathf.RoundToInt(lifeSteal));

                    playerHealth.text = "Health \n" + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerHealth().ToString();

                }

                //play attack sound
                //PlaySFX(1);

                if (enemy.enemyHealth <= 0)
                {
                    combatTextBox.text = enemy.enemyName + " was defeated!";
                    if (enemy.boss)
                    {
                        //player gets ownership of world
                        if (!GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaps().Contains(GameManager.instance.currentWorld))
                        {
                            if (GameManager.instance.currentWorld.type != Map.WorldType.Dungeon && GameManager.instance.currentWorld.type != Map.WorldType.DragonLair)
                            {
                                GameManager.instance.player.GetComponent<PlayerManager>().AppendPlayerMaps(true, GameManager.instance.currentWorld);
                            }
                            GameManager.instance.currentWorld.bossDefeated = true;
                            GameManager.instance.currentWorld.fogRevealed = true;
                            //Debug.Log("obtained world: " + GameManager.instance.currentWorld.mapName);

                            if (GameManager.instance.currentWorld.type == Map.WorldType.DragonLair)
                            {
                                GameManager.instance.ReturnHome();
                            }

                        }
                    }
                    combatAnimator.SetTrigger("EnemyDeath");
                    canAttack = false;
                    EnemyDefeated();
                }
                else
                {
                    enemyStats.text = CompileEnemyStats(enemy);
                    combatAnimator.SetTrigger("EnemyDamaged");
                    //play enemy hit sound
                    PlaySFX(0);
                    canAttack = false;
                    StartCoroutine(EnemyAttackWait());
                }
            } else
            {
                combatTextBox.text = "Your Attack Missed";
                combatAnimator.SetTrigger("EnemyEvaded");
                canAttack = false;
                StartCoroutine(EnemyAttackWait());
            }

            

        }

    }
    
    public void Escape()
    {
        //escape the target enemy, greater player level difference to enemy the harder it is to escape
        if (enemy != null && canAttack)
        {
            if (Random.Range(0, 100) > enemy.enemyEvasion && enemy.boss == false)
            {
                attackSliderObject.SetActive(false);
                EnemyEscaped();
            }
            else
            {
                canAttack = false;
                combatTextBox.text = "You failed to escape the " + enemy.enemyName;
                StartCoroutine(EnemyAttackWait());
            }
        }
        


    }

    public string CompileEnemyStats(EnemyController newEnemy)
    {
        string enemyStats = "Lv. " + newEnemy.enemyLevel + " " + newEnemy.enemyName + "\n"
            + /* "A" + newEnemy.GetEnemyDamage() +  " Health " */ "<sprite=1> " + newEnemy.enemyHealth;

        return enemyStats;    
    }

    public void StartEncounter(GameObject attackingEnemy)
    {
        //disable combat window exit button
        continueButton.SetActive(false);
        SetSliderColour();
        //Debug.Log("encounter started");
        GameManager.instance.player.GetComponent<PlayerController>().SetCombatStatus(true);
        continueButton.SetActive(false);
        SetEnemy(attackingEnemy);
        combatTextBox.text = "You encountered a " + enemy.enemyName;
        ActivateCombatUI();
        canAttack = true;

        //give height damage buffs

        foreach (var item in GameManager.instance.currentWorld.groundBlocksPos)
        {
            if (item.x == Mathf.RoundToInt(GameManager.instance.player.transform.position.x) && item.z == Mathf.RoundToInt(GameManager.instance.player.transform.position.z))
            {
                playerGroundHeight = item.y;
                break;
            }
        }

        foreach (var item in GameManager.instance.currentWorld.groundBlocksPos)
        {
            if (item.x == Mathf.RoundToInt(attackingEnemy.transform.position.x) && item.z == Mathf.RoundToInt(attackingEnemy.transform.position.z))
            {
                enemyGroundHeight = item.y;
                break;
            }
        }

        
        heightAdvantageIcon.SetActive(playerGroundHeight > enemyGroundHeight ? true : false);



    }

    public void EnemyAttack()
    {
        bool playerDefeated = false;
        if (enemy != null)
        {
            //miss chance

            int levelDifference = enemy.enemyLevel - player.GetComponent<PlayerManager>().GetPlayerLevel();
            if (levelDifference <= 0)
            {
                levelDifference = 0;
            }
            bool hitChance = false;

            if (Random.Range(0, levelDifference + 100) > 20)
            {
                hitChance = true;
            }

            if (hitChance == true)
            {
                //damage calculation
                float damageCalc = /*(enemy.enemyLevel - player.GetComponent<PlayerManager>().GetPlayerLevel()) + */enemy.enemyDamage;
                float playerArmorValue = player.GetComponent<PlayerManager>().GetPlayerArmor();
                //damageCalc = damageCalc - ((playerArmorValue / 100) * damageCalc);
                damageCalc *= 100 / (100 + playerArmorValue);
                int finalDamageCalc = Mathf.RoundToInt(damageCalc);
                //Debug.Log(finalDamageCalc);
                if (enemyGroundHeight > playerGroundHeight)
                {
                    finalDamageCalc += groundHeightAttackBonus;
                    Debug.Log("Enemy height advantage: " + enemyGroundHeight + " Player Height: " + playerGroundHeight);
                }

                if (finalDamageCalc < 0)
                {
                    finalDamageCalc = 0;
                }

                //Debug.Log("final damage " + finalDamageCalc);
                //Debug.Log("enemy raw damage " + enemy.GetEnemyDamage());

                player.GetComponent<PlayerManager>().AppendPlayerHealth(-finalDamageCalc);

                PlaySFX(2);

                if (player.GetComponent<PlayerManager>().GetPlayerHealth() > 0)
                {
                    combatTextBox.text = enemy.enemyName + " dealt " + finalDamageCalc + " Damage";
                    playerHealth.text = "Health \n" + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerHealth().ToString();
                    combatAnimator.SetTrigger("EnemyAttack");
                    playerDamagedParticle.Play();
                }
                else
                {
                    //player defeated
                    combatTextBox.text = "You were defeated by " + enemy.enemyName;
                    playerHealth.text = "Health \n" + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerHealth().ToString();
                    combatAnimator.SetTrigger("EnemyAttack");
                    playerDamagedParticle.Play();
                    continueButton.SetActive(true);
                    enemy = null;
                    playerDefeated = true;
                    GameManager.instance.Death();
                    GameManager.instance.player.GetComponent<PlayerManager>().Death();
                    GameManager.instance.player.GetComponent<PlayerController>().SetCombatStatus(false);
                }

            } else
            {
                combatTextBox.text = enemy.enemyName + "'s Attack Missed";
                combatAnimator.SetTrigger("EnemyMissed");

            }

            if (playerDefeated == true)
            {
                canAttack = false;
            } else
            {
                canAttack = true;
                attackSliderObject.SetActive(true);
                attackButton.SetActive(true);
                escapeButton.SetActive(true);
            }
            

        }



    }

    public void EnemyDefeated()
    {
        //destroy enemy and reward player
        //UIManager.GetComponent<ExitToMenu>().LoadUI(UIManager.GetComponent<ExitToMenu>().mainMenu);

        //update enemy stats ui
        //reward player
        //Update combat text box with rewards
        if (enemy.boss)
        {
            if (GameManager.instance.currentWorld.type == Map.WorldType.Dungeon)
            {
                DropSelector.instance.BossDrop(enemy.enemyLevel, true, enemy.enemy);
            } else
            {
                DropSelector.instance.BossDrop(enemy.enemyLevel, false, enemy.enemy);
            }
        } else
        {
            if (GameManager.instance.currentWorld.type == Map.WorldType.Dungeon)
            {
                DropSelector.instance.RandomReward(enemy.enemyLevel, true, enemy.enemy);
            } else
            {
                DropSelector.instance.RandomReward(enemy.enemyLevel, false, enemy.enemy);
            }
        }
        enemyStats.text = "Defeated " + enemy.enemyName;

        //add to enemy collection
        /*
        bool addEnemy = true;
        foreach (var item in GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerDefeatedEnemies())
        {
            if (item.GetEnemyName() == enemy.enemyName)
            {
                addEnemy = false;
                break;
            }
        }
        if (addEnemy)
        {
            GameManager.instance.player.GetComponent<PlayerManager>().AppendDefeatedEnemies(true, enemy.enemy);
        }*/

        if (GameManager.instance.currentWorld != null && !enemy.boss)
        {
            //GameManager.instance.currentWorld.activeEnemies--;
            if (enemy.GetComponent<EnemyController>().enemyLevel == 5)
            {
                GameManager.instance.currentWorld.lv5ActiveEnemies--;

            }
            else if (enemy.GetComponent<EnemyController>().enemyLevel == 10)
            {
                GameManager.instance.currentWorld.lv10ActiveEnemies--;
            }
        }
        //enemy.EnemyDeath();

        enemy = null;
        Destroy(enemyObject);

        //continueButton.SetActive(true);
        StartCoroutine(ContinueExecute());
        //GameManager.instance.player.GetComponent<PlayerController>().SetCombatStatus(false);



    }


    public void EnemyEscaped()
    {
        combatTextBox.text = "You escaped the " + enemy.enemyName;
        enemyStats.text = "Escaped " + enemy.enemyName;

        if (enemy.GetComponent<EnemyController>().enemyLevel == 5)
        {
            GameManager.instance.currentWorld.lv5ActiveEnemies--;

        } else if (enemy.GetComponent<EnemyController>().enemyLevel == 10)
        {
            GameManager.instance.currentWorld.lv10ActiveEnemies--;
        }

        continueButton.SetActive(true);

        //enemy.EnemyEscaped();
        enemy = null;
        Destroy(enemyObject);

    }

    IEnumerator EnemyAttackWait()
    {       
        yield return new WaitForSeconds(2);
        EnemyAttack();
    }

    public void Continue()
    {
        UIManager.GetComponent<ExitToMenu>().LoadUI(UIManager.GetComponent<ExitToMenu>().mainMenu);
        GameManager.instance.player.GetComponent<PlayerController>().SetCombatStatus(false);
    }

    IEnumerator ContinueExecute()
    {
        AnimatorClipInfo[] clip = combatAnimator.GetCurrentAnimatorClipInfo(0);
        yield return new WaitForSeconds(clip[0].clip.length);
        continueButton.SetActive(true);
    }

    public void SetSliderColour()
    {

        if (GameManager.instance.player.GetComponent<PlayerManager>().GetWeapon() != null)
        {
            //player has a weapon
            maxPerfectAttack = GameManager.instance.player.GetComponent<PlayerManager>().GetWeapon().maxAccuracy;
            minPerfectAttack = GameManager.instance.player.GetComponent<PlayerManager>().GetWeapon().minAccuracy;
        }      

        int perfectMin = Mathf.RoundToInt(minPerfectAttack * 100);
        int perfectMax = Mathf.RoundToInt(maxPerfectAttack * 100);

        //Debug.Log(perfectMin + " " + perfectMax);

        //texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);        

        Color[] pixels = texture2D.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            for (int ii = 0; ii < pixels.Length; ii++)
            {

                Vector2 pixelPos = new Vector2(i, ii);

                if (pixelPos.x >= perfectMin && pixelPos.x <= perfectMax )
                {
                    texture2D.SetPixel(Mathf.RoundToInt(pixelPos.x), Mathf.RoundToInt(pixelPos.y), Color.green);
                    //Debug.Log("set pixel green: " + pixelPos);
                } else
                {
                    texture2D.SetPixel(Mathf.RoundToInt(pixelPos.x), Mathf.RoundToInt(pixelPos.y), Color.red);
                    //Debug.Log("set pixel red: " + pixelPos);
                }
            }
        }
        texture2D.Apply();
    }


    private void PlaySFX(int index)
    {
        SFXSource.clip = SFXClips[index];
        SFXSource.Play();
    } 
}
