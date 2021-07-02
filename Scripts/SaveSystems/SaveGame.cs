using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class SaveGame : MonoBehaviour
{


    #region Singleton

    public static SaveGame instance;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one Item Generator Found");
        }
        instance = this;
    }
    #endregion

    public static string mainDirectory = "/SaveData/";
    public static string playerFile = "Player.txt";

    public float autoSaveTime = 60f;

    public ParticleSystem saveParticle;
    public GameObject saveText;
    public float saveTextTime = 3f;

    private void Start()
    {
        LoadGame();
        //InvokeRepeating("Save", 2f, autoSaveTime);
    }


    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
            Debug.Log(Application.persistentDataPath + mainDirectory);
        } 
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Loading save");
            LoadGame();
        }*/
    }

    

    //SAVING THE GAME

    public void Save()
    {
        PlayerManager player = GameManager.instance.player.GetComponent<PlayerManager>();
        string dir = Application.persistentDataPath + mainDirectory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        SavePlayer(dir, player);

    }

    public void SavePlayer(string directory, PlayerManager newPM)
    {
        PlayerData PM = new PlayerData();

        PM.playerHealth = newPM.playerHealth;
        PM.playerMaxHealth = newPM.playerMaxHealth;
        PM.playerInventory = newPM.playerInventory;
        PM.playerEquippedItems = newPM.playerEquippedItems;
        PM.playerWallet = newPM.playerWallet;
        PM.playerLevel = newPM.playerLevel;
        PM.playerDamage = newPM.playerDamage;
        PM.playerArmor = newPM.playerArmor;
        PM.playerExperience = newPM.playerExperience;
        PM.equippedWeapon = newPM.equippedWeapon;
        PM.worldShards = newPM.worldShards;
        PM.ownedMaps = newPM.ownedMaps;
        PM.defeatedEnemies = newPM.defeatedEnemies;
        PM.defaultEnemyUnlocks = newPM.defaultEnemyUnlocks;
        PM.basePlayerDamage = newPM.basePlayerDamage;
        PM.equippedWeaponItem = newPM.equippedWeaponItem;

        string json = JsonUtility.ToJson(PM);

        File.WriteAllText(directory + playerFile, json);
        StartCoroutine(SaveText());
    }

    IEnumerator SaveText()
    {
        saveText.SetActive(true);
        yield return new WaitForSeconds(saveTextTime);
        saveText.SetActive(false);
    }

    public void SaveAtlas()
    {

    }

    //LOADING THE SAVE

    public void LoadGame()
    {
        LoadPlayer();
        
    }

    public void LoadPlayer()
    {
        string fullPath = Application.persistentDataPath + mainDirectory + playerFile;
        PlayerData newPM = new PlayerData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            newPM = JsonUtility.FromJson<PlayerData>(json);

            var PM = GameManager.instance.player.GetComponent<PlayerManager>();

            PM.playerHealth = newPM.playerHealth;
            PM.playerMaxHealth = newPM.playerMaxHealth;
            PM.playerInventory = newPM.playerInventory;
            PM.playerEquippedItems = newPM.playerEquippedItems;
            PM.playerWallet = newPM.playerWallet;
            PM.playerLevel = newPM.playerLevel;
            PM.playerDamage = newPM.playerDamage;
            PM.playerArmor = newPM.playerArmor;
            PM.playerExperience = newPM.playerExperience;
            PM.equippedWeapon = newPM.equippedWeapon;
            PM.worldShards = newPM.worldShards;
            PM.ownedMaps = newPM.ownedMaps;
            PM.defeatedEnemies = newPM.defeatedEnemies;
            PM.defaultEnemyUnlocks = newPM.defaultEnemyUnlocks;
            PM.basePlayerDamage = newPM.basePlayerDamage;
            PM.equippedWeaponItem = newPM.equippedWeaponItem;
        } else
        {
            Debug.Log("Player Save does not exist!");
        }

        



    }

    public void LoadAtlas()
    {

    }

    
    
}
