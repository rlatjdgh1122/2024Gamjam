using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PlanetType
{
    Mars, //수성
    Jupiter, //목성
    Saturn, //토성
    Uranus, //천왕성
    Neptune //해왕성
}
[System.Serializable]
public class Setting
{
    public PlanetType type;
    [Header("위치 설정")]
    public Transform spawnPivot;
    public int radius;
    public int length;
    [Header("스폰 설정")]
    public int asteroidCount;
    public List<SpawnObstacle> asteroidLists = new();
    public int trashCount;
    public List<SpawnObstacle> trashLists = new();
    public List<GameObject> etcLists = new(); //예) 비행기, 블랙홀 등
}
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public List<Setting> settings = new(); //행성들마다 생성
    public Dictionary<PlanetType, Setting> planetLists = new(); //저장
    private List<SpawnObstacle> dummyObjs = new(); //이전 스폰된 오브젝트를 지워줌
    private List<GameObject> etcObjs = new(); //이전 스폰된 오브젝트를 지워줌

    private PlanetType curType = PlanetType.Neptune;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SpawnManager is not NULL");
        }

        Instance = this;
    }
    private void Start()
    {
        int idx = 0;
        foreach (PlanetType type in Enum.GetValues(typeof(PlanetType)))
        {
            planetLists.Add(type, settings[idx++]);
        }

        Init();
    }
    private void Update()  
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn(PlanetType.Neptune);
        }
    }
    public void Init()
    {
        Despawn();
        Spawn(PlanetType.Neptune);
    }
    public void Spawn(PlanetType planet)
    {
        Despawn(); //확인용

        curType = planet;

        var setting = planetLists[planet];
        var radius = setting.radius;
        var length = setting.length;

        int asteroidCount = setting.asteroidCount;
        int trashCount = setting.trashCount;

        var asteroidList = setting.asteroidLists;
        var trashList = setting.trashLists;
        var etcList = setting.etcLists;

        for (int i = 0; i < asteroidCount; i++)
        {
            var name = asteroidList[Random.Range(0, asteroidList.Count)].name; //돌
            var obj = PoolManager.Instance.Pop(name) as SpawnObstacle;

            var randomPos = setting.spawnPivot.position + Random.insideUnitSphere * radius +
           new Vector3(0, Random.Range(-length, length), 0);

            obj.Spawn(randomPos);

            dummyObjs.Add(obj);
        }
        for (int i = 0; i < trashCount; i++)
        {
            var name = trashList[Random.Range(0, trashList.Count)].name; //돌
            var obj = PoolManager.Instance.Pop(name) as SpawnObstacle;

            var randomPos = setting.spawnPivot.position + Random.insideUnitSphere * radius +
           new Vector3(0, Random.Range(-length, length), 0);

            obj.Spawn(randomPos);

            dummyObjs.Add(obj);
        }

        for (int i = 0; i < etcList.Count; i++)
        {
            etcList[i].gameObject.SetActive(true);
            etcObjs.Add(etcList[i]);
        }
    }
    public void Despawn()
    {
        if (dummyObjs.Count <= 0) return;

        foreach (var obj in dummyObjs)
        {
            PoolManager.Instance.Push(obj);
        }
        dummyObjs.Clear();

        if (etcObjs.Count <= 0) return;

        foreach (var obj in etcObjs)
        {
            Destroy(obj);
        }
        etcObjs.Clear();
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (planetLists.Count <= 0) return;

        var setting = planetLists[curType];
        var pos = setting.spawnPivot;
        int radius = setting.radius;
        int length = setting.length;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos.position, new Vector3(radius, length, radius));

    }
#endif
}
