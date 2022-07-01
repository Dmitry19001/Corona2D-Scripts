using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    [SerializeField]
    private float SpawnTime = 3.0f;
    [SerializeField]
    private GameObject Citizen;
    private bool Spawning = true;

    [SerializeField]
    private int amount = 25;

    //[SerializeField]
    //private GameObject citizenList;

    private int num = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawning)
        {
            float extend = Random.Range(0.0f, 3.0f);
            if (amount >0)
            {
                Invoke("SpawnNewCitizen", SpawnTime + extend);
                amount--;
            }
            Spawning = false;
        }
        if (num > 100) { Spawning=false; }
    }
    //spawn new citizen
    void SpawnNewCitizen()
    {
        float movelittle = (Random.Range(-0.5f, 0.5f));
        var citizen = Instantiate(Citizen, new Vector2(gameObject.transform.position.x + movelittle, gameObject.transform.position.y + movelittle), Quaternion.identity);
        //citizen.transform.SetParent(citizenList.transform.GetChild(0).transform);
        Spawning = true;
    }
}
