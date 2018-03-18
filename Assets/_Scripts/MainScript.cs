using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {

    [SerializeField]
    private GameObject[] UnitPrefabs;
    [SerializeField]
    private GameObject ProjectilePrefab;
    [SerializeField]
    private GameObject HealthbarPrefab;

    private List<Unit> TeamA;
    private List<Unit> TeamB;

    private List<Healthbar> Healthbars;

    void Awake()
    {
        TeamA = new List<Unit>();
        TeamB = new List<Unit>();
        Healthbars = new List<Healthbar>();
    }

	void Start ()
    {
        spawnTeams();	
	}
	

	void Update ()
    {
        removeDeadFromTeams();
        moveTeams();
        updateHealthbars();
    }


    //method called on Start() and after using restart button. Fills teams with units and sets their positions; 5 Greens, 5 Blues, 5 Reds and 2 Yellows each team. Adds healthbars.
    private void spawnTeams()
    {
        for (int i = 0; i < 5; i++)
        {
            TeamA.Add(new UnitGreen(UnitPrefabs[2]));
            TeamA[i].UnitGO.transform.position = posBasedOnTeam(true);
        }
        for (int i = 0; i < 5; i++)
        {
            TeamA.Add(new UnitBlue(UnitPrefabs[0]));
            TeamA[i + 5].UnitGO.transform.position = posBasedOnTeam(true);
        }
        for (int i = 0; i < 5; i++)
        {
            TeamA.Add(new UnitRed(UnitPrefabs[4]));
            TeamA[i + 10].UnitGO.transform.position = posBasedOnTeam(true);
        }
        for (int i = 0; i < 2; i++)
        {
            TeamA.Add(new UnitYellow(UnitPrefabs[6], ProjectilePrefab));
            TeamA[i+15].UnitGO.transform.position = posBasedOnTeam(true);
        }

        foreach (Unit unit in TeamA)
        {
            Healthbars.Add(new Healthbar(unit, HealthbarPrefab));
        }
        for (int i = 0; i < 5; i++)
        {
            TeamB.Add(new UnitGreen(UnitPrefabs[3]));
            TeamB[i].UnitGO.transform.position = posBasedOnTeam(false);
        }
        for (int i = 0; i < 5; i++)
        {
            TeamB.Add(new UnitBlue(UnitPrefabs[1]));
            TeamB[i + 5].UnitGO.transform.position = posBasedOnTeam(false);
        }
        for (int i = 0; i < 5; i++)
        {
            TeamB.Add(new UnitRed(UnitPrefabs[5]));
            TeamB[i + 10].UnitGO.transform.position = posBasedOnTeam(false);
        }
        for (int i = 0; i <2; i++)
        {
            TeamB.Add(new UnitYellow(UnitPrefabs[7], ProjectilePrefab));
            TeamB[i+15 ].UnitGO.transform.position = posBasedOnTeam(false);
        }

        foreach (Unit unit in TeamB)
        {
            Healthbars.Add(new Healthbar(unit, HealthbarPrefab));
        }

    }



    //removes dead units from teams
    private void removeDeadFromTeams()
    {
        List<Unit> remainingA = new List<Unit>();
        List<Unit> remainingB = new List<Unit>();
        foreach (Unit unit in TeamA)
        {
            if (unit.Health > 0) remainingA.Add(unit); else Destroy(unit.UnitGO);
        }
        TeamA = remainingA;


        foreach (Unit unit in TeamB)
        {
            if (unit.Health > 0) remainingB.Add(unit); else Destroy(unit.UnitGO);
        }
        TeamB = remainingB;
    }


    //checks if units has target (finds one if needed). Updates all Units in each team.
    private void moveTeams()
    {
        foreach (Unit unit in TeamA)
        {
            if (unit.Target == null)
            {
                unit.setTarget(findEnemy(unit, TeamB));

            }
            unit.Update();
        }
        foreach (Unit unit in TeamB)
        {
            if (unit.Target == null)
            {
                unit.setTarget(findEnemy(unit, TeamA));

            }
            unit.Update();
        }
    }


    //updates healthbars
    private void updateHealthbars()
    {
        foreach (Healthbar hb in Healthbars)
        {
            hb.Update();
        }
    }


    //called upon selecting restart button. Clears both teams and fills them with new units
    private void restartLevel()
    {
        foreach(Healthbar hb in Healthbars)
        {
            Destroy(hb.healthbarBG);
        }
        Healthbars.Clear();


        foreach(Unit unit in TeamA)
        {
            Destroy(unit.UnitGO);
        }
        TeamA.Clear();
        foreach (Unit unit in TeamB)
        {
            Destroy(unit.UnitGO);
        }
        TeamB.Clear();

        spawnTeams();
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 20;

        if (GUI.Button(new Rect(Screen.width - 200, 25, 150, 40), "RESTART")) restartLevel();

        checkIfOver();
    }

    //displays text when fight is over
    private void checkIfOver()
    {
        if (TeamA.Count==0 && TeamB.Count==0)
        {
            GUI.Label(new Rect(Screen.width/2-50,Screen.height/2-50, 150, 40),"DRAW");
        }
        else if (TeamA.Count == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 150, 40), "Team B won!");
        }
        else if (TeamB.Count == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 150, 40), "Team A won!");
        }
    }

    //method for finding target. Ranged units attack most damaged enemy (or closest if few have same % of health remaining
    private Unit findEnemy(Unit unit, List<Unit> Enemies)
    {
        Unit enemy = null;
        float curDist = Mathf.Infinity;
        float curHealth = Mathf.Infinity;
        foreach (Unit potEnemy in Enemies)
        {
            if (unit is UnitYellow)
            {
                float healthLost = potEnemy.Health / potEnemy.maxHealth;
                if (healthLost < curHealth)
                {
                    enemy = potEnemy;
                    curHealth = healthLost;
                }
                if (healthLost == curHealth)
                {
                    float dist = Vector3.Distance(unit.UnitGO.transform.position, potEnemy.UnitGO.transform.position);
                    if (potEnemy.Health > 0 && dist < curDist)
                    {
                        enemy = potEnemy;
                        curDist = dist;
                    }
                }
            }

            else
            {
                float dist = Vector3.Distance(unit.UnitGO.transform.position, potEnemy.UnitGO.transform.position);
                if (potEnemy.Health > 0 && dist < curDist)
                {
                    enemy = potEnemy;
                    curDist = dist;
                }
            }

        }
        return enemy;
    }


    //helper method for setting position of new units
    private Vector3 posBasedOnTeam(bool teamA)
    { 
        return new Vector3((teamA) ? Random.Range(-8, -4) : Random.Range(4, 8), Random.Range(-4, 4), 0);
    }
}
