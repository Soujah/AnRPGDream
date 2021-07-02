using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool inCombat = false;
    public Vector3 currentPosition;
    //Vector3 newPosition = new Vector3();
    public Transform playerMovePoint;
    public float moveSpeed;

    public Joystick joyStick;

    public Transform HologramContainer;
    public Transform player3D;

    public GameObject WorldView3D;

    public GameObject movementParticles;
    public Vector3 particleOffset;

    void Start()
    {
        currentPosition = gameObject.transform.position;
        playerMovePoint.transform.parent = null;
    }

    void Update()
    {
        if (GameManager.instance.mapGenerationFinished == false || GameManager.instance.GetComponent<GameManager>().hologramMode == true)
        {
            return;
        }
        if (!inCombat && GameManager.instance.playerTeleporting == false && !WorldView3DActive())
        {
            MovementInput();
            MobileMovementInput();

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //interacting with world
            //GameManager.instance.GetCurrentWorld().GetComponent<MapManager>().InteractWithMapPosition(transform.position);
            PlayerInteract();
        }
    }

    public void PlayerInteract()
    {
        GameManager.instance.InteractWithWorldObject(transform.position);

    }

    public void MovementInput()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerMovePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerMovePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                float inputHor = Input.GetAxisRaw("Horizontal");

                if (WithinMapContraints(playerMovePoint.position + new Vector3(inputHor, 0, 0)))
                {
                    playerMovePoint.position += new Vector3(inputHor, 0f, 0f);
                }
                Instantiate(movementParticles, transform.position + particleOffset, Quaternion.Euler(new Vector3(90, 0, 0)));

            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                float inputVert = Input.GetAxisRaw("Vertical");

                if (WithinMapContraints(playerMovePoint.position + new Vector3(0,0,inputVert)))
                {
                    playerMovePoint.position += new Vector3(0f, 0f, inputVert);
                }
                Instantiate(movementParticles, transform.position + particleOffset, Quaternion.Euler(new Vector3(90, 0, 0)));
            }
            currentPosition = playerMovePoint.position;
            //player3D.position = playerMovePoint.position;
        }
        
        /* OLD MOVEMENT SYSTEM
        if (Input.GetKeyDown(KeyCode.W))
        {
            newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            newPosition = new Vector3(currentPosition.x - 1, currentPosition.y, currentPosition.z);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newPosition = new Vector3(currentPosition.x + 1, currentPosition.y, currentPosition.z);
        }

        //Debug.Log(newPosition);

        if (newPosition.x <= GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1 && newPosition.x >= 0 && newPosition.z <= GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1 && newPosition.z >= 0)
        {
            currentPosition = newPosition;
            currentPosition.y = 6;
            gameObject.transform.position = currentPosition;
        }*/
     
    }


    public void MobileMovementInput()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerMovePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerMovePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(joyStick.Horizontal) == 1f)
            {
                float inputHor = joyStick.Horizontal;

                if (WithinMapContraints(playerMovePoint.position + new Vector3(inputHor, 0, 0)))
                {
                    playerMovePoint.position += new Vector3(inputHor, 0f, 0f);
                }
            }
            else if (Mathf.Abs(joyStick.Vertical) == 1f)
            {
                float inputVert = joyStick.Vertical;

                if (WithinMapContraints(playerMovePoint.position + new Vector3(0, 0, inputVert)))
                {
                    playerMovePoint.position += new Vector3(0f, 0f, inputVert);
                }
            }
            currentPosition = playerMovePoint.position;
            //player3D.position = playerMovePoint.position;

        }
    }

    public bool GetCombatStatus()
    {
        return inCombat;
    }

    public void SetCombatStatus(bool combatStatus)
    {
        inCombat = combatStatus;
    }

    public bool WithinMapContraints(Vector3 pos)
    {
        if (pos.x <= GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1 && pos.x >= 0 && pos.z <= GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1 && pos.z >= 0)
        {
            return true;
        } else
        {
            return false;
        }

    }


    public bool WorldView3DActive()
    {
        return WorldView3D.activeSelf;
    }

}
