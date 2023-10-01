using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic instance; 
    [SerializeField] private TargetController target;
    [SerializeField] private Transform spawnPoint;
    List<IPlayer> players;

    private bool isMoving = false;  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        players = new List<IPlayer>();
    }

    public static GameLogic Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameLogic = new GameObject("GameLogic");
                instance = gameLogic.AddComponent<GameLogic>();
            }
            return instance;
        }
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            GeneratePlayer();

        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if (Physics.Raycast(ray, out hit))
            {
                if (!isMoving)
                {
                    // Spawn a new sphere at the hit point
                    if (target == null)
                    {
                        GameObject spherePrefab = Resources.Load<GameObject>("Target");
                        target = Instantiate(spherePrefab, hit.point, Quaternion.identity).GetComponent<TargetController>();
                        //player.LookTarget(target.transform.position);
                    }
                    else
                    {
                        target.Place(hit.point);
                        target.Activate();
                    }
                    isMoving = true;
                    
                }
                else
                {
                    //player.FollowLookAt(target.transform.position);
                    target.Place(hit.point);  
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        { 
            isMoving = false; 
            UpdateAllTargetPosition(target.transform.position);
        }
    }

    private void GeneratePlayer()
    { 
        GameObject player = Resources.Load<GameObject>("Player");
        var playerController = Instantiate(player, spawnPoint.transform.position, Quaternion.identity).GetComponent<IPlayer>();
        players.Add(playerController);
    }

    private void UpdateAllTargetPosition(Vector3 targetPosition) {
        foreach (var player in players)
        {
            Vector3 targetPos = new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z);
            player.SetTargetPosition(targetPos);
        }
    }
}