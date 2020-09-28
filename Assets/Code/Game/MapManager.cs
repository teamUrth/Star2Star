using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public static GameMap Map;
    public GameObject Chunk;
    public List<Texture> Textures;
    public static GameObject Colliders = null; // 콜라이더들 집합

    public static float ScreenX { get; private set; }
    public static float ScreenY { get; private set; }
    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }

    private static List<Chunk> _chunkPool = new List<Chunk>();
    private static Dictionary<string, GameMap> _maps = new Dictionary<string, GameMap>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        Initialize();
    }

    private void Initialize()
    {
        Colliders = new GameObject { name = "Colliders" };
        Colliders.transform.parent = transform;
        Colliders.transform.position = transform.position;

        BuildMap("Village");
        Map = _maps["Village"];
        SetScreen();
    }

    public void BuildMap(string name)
    {
        if (_maps.ContainsKey(name))
        {
            return;
        }
        GameMap map = CSVReader.LoadMap(name);
        map.BuildMap();
        _maps.Add(name, map);
    }

    public void ChangeMap(Vector3 direction)
    {
        Debug.Log("맵바꾸래" + direction);
        GameManager.isAction = true;
        string name = string.Empty;
        if (direction == Vector3.up)
        {
            name = Map.Neighbor[0];
        }
        else if (direction == Vector3.down)
        {
            name = Map.Neighbor[1];
        }
        else if (direction == Vector3.left)
        {
            name = Map.Neighbor[2];
        }
        else if (direction == Vector3.right)
        {
            name = Map.Neighbor[3];
        }
        BuildMap(name);
        Map = _maps[name];
        GameManager.ScrollMap(direction);
    }

    public void SetScreen()
    {
        ScreenX = Map.X + TileData.ChunkWidth * 0.5f;
        ScreenY = Map.Y - TileData.ChunkHeight * 0.5f;
        ScreenWidth = Map.X + Map.Width - TileData.ChunkWidth * 0.5f;
        ScreenHeight = Map.Y - Map.Height + TileData.ChunkHeight * 0.5f;
        GameManager.isAction = false;
    }

    public void SetScreen(string name)
    {
        Map = _maps[name];
        ScreenX = Map.X + TileData.ChunkWidth * 0.5f;
        ScreenY = Map.Y - TileData.ChunkHeight * 0.5f;
        ScreenWidth = Map.X + Map.Width - TileData.ChunkWidth * 0.5f;
        ScreenHeight = Map.Y - Map.Height + TileData.ChunkHeight * 0.5f;
        GameManager.isAction = false;
    }

    public static void DeleteCollider(int x, int y)
    {
        Debug.Log(x + "랑" + y + "좀 부숴줘");
        Map.Collider[x, y] = 0;
        Map.UpdateCollider();
    }

    public static Texture GetTexture(string name)
    {
        for (var i = 0; i < Instance.Textures.Count; i++)
        {
            if (Instance.Textures[i].name.Equals(name))
            {
                return Instance.Textures[i];
            }
        }
        return null;
    }

    public void SetScreen(float x, float y, float width, float height)
    {
        ScreenX = x + TileData.ChunkWidth * 0.5f;
        ScreenY = y - TileData.ChunkHeight * 0.5f;
        ScreenWidth = x + width - TileData.ChunkWidth * 0.5f;
        ScreenHeight = y - height + TileData.ChunkHeight * 0.5f;
        Debug.Log("SetScreen = x : " + ScreenX + " / y : " + ScreenY + " / width : " + ScreenWidth + " / height : " + ScreenHeight);
    }
    
    /*
    private void BuildMap(string name, Vector3 position)
    {
        _mapCache = CSVReader.LoadMap(name);
        _mapCache.X = (int)position.x;
        _mapCache.Y = (int)position.y;
        _mapCache.BuildMap();
        //Debug.Log("PosX : " + _mapCache.X + " / PosY : " + _mapCache.Y);
        SetScreen(_mapCache.X, _mapCache.Y, _mapCache.Width, _mapCache.Height);
    }*/

    /*
    public void ChangeMap(Vector3 direction)
    {
        _mapCache = CSVReader.LoadMap("Map003");
        //Map이 이전맵
        if (direction == Vector3.up)
        {
            BuildMap("Map003", new Vector3(Map.X, Map.Y + _mapCache.Height, 0));
        }
        else if (direction == Vector3.down)
        {
            BuildMap("Map003", new Vector3(Map.X, Map.Y - Map.Height, 0));
        }
        else if (direction == Vector3.left)
        {
            BuildMap("Map003", new Vector3(Map.X - _mapCache.Width, Map.Y, 0));
        }
        else if (direction == Vector3.right)
        {
            BuildMap("Map003", new Vector3(Map.X + Map.Width, Map.Y, 0));
        }
        GameManager.ScrollMap(direction);
    }*/

    private void CreateNewChunk()
    {
        GameObject chunk = Instantiate(Chunk, Vector3.zero, new Quaternion(0, 0, 0, 0), transform);
        chunk.gameObject.SetActive(false);
        _chunkPool.Add(chunk.GetComponent<Chunk>());
    }

    public Chunk GetChunk()
    {
        if (_chunkPool.Count == 0)
        {
            CreateNewChunk();
        }
        Chunk chunk = _chunkPool[0];
        _chunkPool.RemoveAt(0);
        chunk.gameObject.SetActive(true);
        return chunk;
    }

    public static void BackChunk(Chunk chunk)
    {
        chunk.gameObject.SetActive(false);
        _chunkPool.Add(chunk);
    }
}

public struct TileData
{
    // 화면상으로 보이는 최대 타일개수는 사실 16 * 9임
    public static readonly int ChunkWidth = 16;
    public static readonly int ChunkHeight = 9;

    //메쉬 만들 점들
    public static readonly Vector3[] Vertices = new Vector3[4]
    {
        new Vector3(0f, 0f),
        new Vector3(1f, 0f),
        new Vector3(1f, -1f),
        new Vector3(0f, -1f),
    };

    public static readonly Vector3[] AutoTileVertices = new Vector3[4]
    {
        new Vector3(0f, 0f),
        new Vector3(.5f, 0f),
        new Vector3(.5f, -.5f),
        new Vector3(0f, -.5f),
    };

    public static readonly int[] Triangles = new int[6] //메쉬 만들 삼각형
    {
        0, 1, 3, 2, 3, 1
    };
}