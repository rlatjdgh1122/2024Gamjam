using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ETCClass
{
    public float radius;
    public Transform objPos;
    public SpawnObstacle obj;
}
[System.Serializable]
public class Setting
{
    public PlanetEnum type;
    [Header("위치 설정")]
    public Transform spawnPivot;
    public int radius;
    public int length;
    [Header("스폰 설정")]
    public int asteroidCount;
    public List<SpawnObstacle> asteroidLists = new();
    public int trashCount;
    public List<SpawnObstacle> trashLists = new();
    public List<ETCClass> etcLists = new(); //예) 비행기, 블랙홀 등
}
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public List<Setting> settings = new(); //행성들마다 생성
    public Dictionary<PlanetEnum, Setting> planetListDic = new(); //저장
    public Dictionary<PlanetEnum, List<SpawnObstacle>> dummyObjDic = new();
    private Dictionary<PlanetEnum, List<ETCClass>> etcObjDic = new(); //이전 스폰된 오브젝트를 지워줌

    private PlanetEnum curType = PlanetEnum.Neptune;
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
        foreach (PlanetEnum type in Enum.GetValues(typeof(PlanetEnum)))
        {
            if (idx > 5) continue;
            planetListDic.Add(type, settings[idx++]);
            dummyObjDic.Add(type, new List<SpawnObstacle>());
            etcObjDic.Add(type, new List<ETCClass>());
        }

        Init();
    }
    public void Init()
    {
        Despawn(PlanetEnum.Neptune);
        Spawn(PlanetEnum.Neptune);
    }
    public void Spawn(PlanetEnum planet)
    {
        if ((int)planet + 2 <= (int)PlanetEnum.Neptune)
        {
            Despawn(planet + 2);
        }

        curType = planet;

        var setting = planetListDic[planet];
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
           new Vector3(0, 0, Random.Range(-length, length));

            obj.Spawn(randomPos);

            dummyObjDic[planet].Add(obj);
        }
        for (int i = 0; i < trashCount; i++)
        {
            var name = trashList[Random.Range(0, trashList.Count)].name; //돌
            var obj = PoolManager.Instance.Pop(name) as SpawnObstacle;

            var randomPos = setting.spawnPivot.position + Random.insideUnitSphere * radius +
           new Vector3(0, 0, Random.Range(-length, length));

            obj.Spawn(randomPos);

            dummyObjDic[planet].Add(obj);
        }

        for (int i = 0; i < etcList.Count; i++)
        {
            var item = etcList[i];
            var name = item.obj.name;
            var pos = item.objPos.position + Random.insideUnitSphere * item.radius;

            var obj = PoolManager.Instance.Pop(name) as SpawnObstacle;
            obj.Spawn(pos);

            etcObjDic[planet].Add(etcList[i]);
        }
    }
    public void Despawn(PlanetEnum plent)
    {
        if (dummyObjDic[plent].Count <= 0) return;

        foreach (var value in dummyObjDic[plent])
            PoolManager.Instance.Push(value);

        dummyObjDic[plent].Clear();

        if (etcObjDic[plent].Count <= 0) return;

        foreach (var obj in etcObjDic[plent])
        {
            PoolManager.Instance.Push(obj.obj);
        }
        etcObjDic[plent].Clear();
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (planetListDic.Count <= 0) return;

        var setting = planetListDic[curType];
        var pos = setting.spawnPivot;
        int radius = setting.radius;
        int length = setting.length;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos.position, new Vector3(radius, length, radius));

    }
#endif
}
