using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControlV1 : MonoBehaviour {

    public GameObject[] goals;
	UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    Vector3 lastGoal;
    

	// Use this for initialization
	void Start () {
 //       DontDestroyOnLoad(gameObject);
//		goalLocations = GameObject.FindGameObjectsWithTag("goal");

		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", true);
        PickGoalLocation();
	}

    void PickGoalLocation()
    {
        lastGoal = agent.destination;
        //GameObject go = GameEnvironment.Singleton.GetRandomGoal();
        GameObject go = GetRandomGoal();
        agent.SetDestination(go.transform.position);

       agent.SetDestination(goals[Random.Range(0, goals.Length)].transform.position);
    }


    // Update is called once per frame
    void Update () {
        if (agent.remainingDistance < 1) //At the goal
        {
            PickGoalLocation();

        }
//        foreach (GameObject go in GameEnvironment.Singleton.Obstacles)
//        {
//            float distance = Vector3.Distance(go.transform.position, this.transform.position);
//            if (distance < 5 && Random.Range(0,100) < 5)
//            {
//                agent.SetDestination(lastGoal);
//            }
//            else if(distance < 1)
//            {
////                GameEnvironment.Singleton.RemoveObstacles(go);
//                break;
//            }
//        }
    }

    private GameObject GetRandomGoal()
    {
        int index = Random.Range(0, goals.Length);
        return goals[index];
    }
}
