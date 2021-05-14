using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetSpawner : MonoBehaviour
{
    public string[] TargetsToSpawn;
    [SerializeField] public GameObject[] TargetPrefabs;
    public float Delay;
    private int SizeNum;
    [HideInInspector]public int layerorder = 1;
    public GameObject[] Targets;
    public List<GameObject> ActiveTargets;

    public static float deadCount;
    public static float TotalCount;
    public static float score;
    public GameObject Coin;

    public static TargetSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        deadCount = 0;
        TotalCount = 0;



        Targets = new GameObject[TargetsToSpawn.Length];
        PrepareTargets();
        TotalCount = GetTotalCount();
    }
    public void StartGame()
    {
        StartCoroutine(SpawnTargets());
        UIManager.Instance.StartGame();
    }

    float GetTotalCount()
    {
        float count=0;
        for (int i = 0; i < TargetsToSpawn.Length; i++)
        {
            string[] s = TargetsToSpawn[i].Split('-');
            string size = s[1];
            if (size == "S")
            {
                count+=1;
            }
            else if (size == "M")
            {
                count += 3;
            }
            else if (size == "L")
            {
                count += 7;
            }
            else if (size == "XL")
            {
                count += 15;
            }
        }
        return count;
    }

    IEnumerator SpawnTargets()
    {
        for (int i = 0; i < Targets.Length; i++)
        {
            yield return new WaitForSeconds(Delay);
            if (ActiveTargets.Count <= 2)
            {
                Targets[i].SetActive(true);
                ActiveTargets.Add(Targets[i]);
            }
            else
            {
                i--;
            }
        }
    }

    void PrepareTargets()
    {
        for (int i = 0; i < TargetsToSpawn.Length; i++)
        {
            string[] s = TargetsToSpawn[i].Split('-');
            string life = s[0];
            string size = s[1];
            if (size == "S")
            {
                SizeNum = 0;
            } else if (size == "M")
            {
                SizeNum = 1;
            } else if (size == "L")
            {
                SizeNum = 2;
            } else if (size == "XL")
            {
                SizeNum = 3;
            }
            Targets[i] = Instantiate(TargetPrefabs[SizeNum], transform);
            Targets[i].GetComponent<Target>().health =int.Parse(life);
            Targets[i].GetComponent<SpriteRenderer>().sortingOrder = layerorder;
            layerorder += 1;
            Targets[i].GetComponent<Target>().textHealth.GetComponent<MeshRenderer>().sortingOrder=layerorder;
            layerorder += 1;
            Targets[i].SetActive(false);
        }
    }
}
