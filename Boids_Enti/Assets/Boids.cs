using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{

    [SerializeField]
    GameObject agentPrefab;

    [SerializeField]
    int numBoids = 10;

    Agent[] agents;

    [SerializeField]
    float agentRadius = 10.0f;

    [SerializeField]
    float separationWeight = 1.0f, cohesionWeight = 1.0f, alignmentWeight = 1.0f;


    private void Awake()
    {
        List<Agent> agentlist = new List<Agent>();

        for(int i = 0; i<numBoids; i++)
        {
            Vector3 position = Vector3.up * Random.Range(0, 10)
                + Vector3.right * Random.Range(0, 10) + Vector3.forward * Random.Range(0, 10);

            Agent agent = Instantiate(agentPrefab, position, Quaternion.identity).GetComponent<Agent>();
            agent.radius = agentRadius;

            agentlist.Add(agent);
        }

        agents = agentlist.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Agent a in agents)
        {
            a.neightbours.Clear();

            a.velocity = Vector3.zero;
            a.checkNeightbours();

            calculateSeparation(a);
            calculateAlignment(a);
            calculateCohesion(a);

            a.updateAgent();
            
         
        }
    }


    void calculateSeparation(Agent a)
    {
        float distance;

        foreach (Agent n in a.neightbours)
        {
            distance = Vector3.Distance(a.transform.position, n.transform.position);
            distance /= agentRadius;
            distance = 1 - distance; 

            a.addForce(distance * (a.transform.position - n.transform.position) * separationWeight, Agent.DEBUGforceType.SEPARATION);
        }
    }

    void calculateCohesion(Agent a)
    {
        Vector3 centralPosition = new Vector3();

        foreach (Agent n in a.neightbours)
        {
            centralPosition += n.transform.position;
        }

        centralPosition += a.transform.position;
        centralPosition /= a.neightbours.Count + 1;

        a.addForce((centralPosition - a.transform.position) * cohesionWeight, Agent.DEBUGforceType.COHESION);
    }

    void calculateAlignment(Agent a)
    {
        Vector3 directionVector = new Vector3();

        foreach (Agent n in a.neightbours)
        {
            directionVector += n.velocity;
        }

        directionVector += a.velocity;
        directionVector /= a.neightbours.Count + 1;


        a.addForce(directionVector, Agent.DEBUGforceType.ALIGNMENT);
    }
}
