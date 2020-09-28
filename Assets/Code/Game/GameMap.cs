using System.Collections.Generic;
using UnityEngine;

public enum TileCollider {Default = 0, Water = 4, Event = 8, Wall};

public class GameMap
{
    public string Name { get; set; }
    // 아이디
    public int ID { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string[] Neighbor { get; set; }
    public int Layer { get; set; }
    public string TileName { get; set; }
    public List<string> AutoTiles { get; set; }
    public List<Chunk> Chunks { get; set; }
    public List<GameObject> Colliders { get; }
    public byte[,] Collider { get; set; }
    public int[,,] Tile { get; set; }

    public GameMap()
    {
        AutoTiles = new List<string>();
        Chunks = new List<Chunk>();
        Colliders = new List<GameObject>();
        Neighbor = new string[4];
    }

    public void BuildMap()
    {
        Texture texture = MapManager.GetTexture(TileName);
        var padding = 1 + AutoTiles.Count;
        var rowpadding = padding / (texture.width / GameManager.PPU) + 1;
        rowpadding *= (texture.width / GameManager.PPU);

        for (var l = 0; l < Layer; l++)
        {
            Chunk chunk = MapManager.Instance.GetChunk();
            var divideTile = new int[Width, Height];
            List<int> tagList = new List<int>();
            Dictionary<int, bool[,]> autoTile = new Dictionary<int, bool[,]>();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (Tile[x, y, l] == 0)
                    {
                        continue;
                    }
                    // 해당 타일이 오토타일인지 체크
                    else if (Tile[x, y, l] < padding)
                    {
                        // 포함되어있는지 확인
                        if (tagList.Contains(Tile[x, y, l]))
                        {
                            autoTile[Tile[x, y, l]][x, y] = true;
                            continue;
                        }
                        else
                        {
                            // 없으면 추가
                            tagList.Add(Tile[x, y, l]);
                            autoTile.Add(Tile[x, y, l], new bool[Width, Height]);
                            for (var ay = 0; ay < Height; ay++)
                            {
                                for (var ax = 0; ax < Width; ax++)
                                {
                                    autoTile[Tile[x, y, l]][ax, ay] = false;
                                }
                            }
                            autoTile[Tile[x, y, l]][x, y] = true;
                            continue;
                        }
                    }
                    divideTile[x, y] = Tile[x, y, l];
                }
            }
            chunk.BuildChunk(new Vector3(X, Y, 0), divideTile, texture, l, l, rowpadding); // l < 2 ? 0 : 1
            while (tagList.Count > 0)
            {
                Chunk auto = MapManager.Instance.GetChunk();
                auto.BuildAutotile(new Vector3(X, Y, 0), MapManager.GetTexture(AutoTiles[tagList[0] - 1]), autoTile[tagList[0]],l ,l) ;
                tagList.RemoveAt(0);
                Chunks.Add(auto);
            }
            autoTile.Clear();
            Chunks.Add(chunk);
        }
        BuildCollider();
    }

    public void UnloadMap()
    {
        while (Chunks.Count > 0)
        {
            MapManager.BackChunk(Chunks[0]);
            Chunks.RemoveAt(0);
        }
    }

    // 콜라이더 설정 함수
    private void BuildCollider()
    { 
        List<byte> tagList = new List<byte>();
        // 맵정보에 있는 태그들 불러오기
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                // 포함되어있는지 확인
                if (tagList.Contains(Collider[x, y]))
                {
                    continue;
                }
                // 없으면 추가
                tagList.Add(Collider[x, y]);
            }
        }

        // 태그리스트에 있는 값을 하나씩 읽어온다
        for (var i = 0; i < tagList.Count; i++)
        {
            GameObject collider;
            Transform trans = MapManager.Colliders.transform.Find(tagList[i].ToString());
            if (trans == null)
            {
                collider = new GameObject();
                collider.gameObject.layer = tagList[i];
                collider.transform.parent = MapManager.Colliders.transform;
                // 이것때문에 몇시간동안 좌표계산 계속 고치려고했다 ㅠㅠㅠㅠㅠㅠㅠㅠ
                collider.transform.position = MapManager.Colliders.transform.position;
                // 20200419 02:39분에 해결 완료..
                collider.name = tagList[i].ToString();
            }
            else
            {
                collider = trans.gameObject;
                BoxCollider2D[] boxes = trans.GetComponentsInChildren<BoxCollider2D>();
                for (var j = 0; j < boxes.GetLongLength(0); j++)
                {
                    EventManager.Break(boxes[j]);
                }
            }
            collider.transform.position = new Vector3(X, Y, 0);
            // 한줄씩 잘라서 박스콜라이더를 생성하기 위해 y만
            for (var y = 0; y < Collider.GetLength(1); y++)
            {
                var size = 0;
                for (var x = 0; x < Collider.GetLength(0); x++)
                {
                    if (Collider[x, y] == tagList[i])
                    {
                        size++;
                        if (x == Collider.GetLength(0) - 1) // 한줄이 전부 콜라이더인 경우
                        {
                            AddCollider(collider, x + 1, y, size);
                        }
                    }
                    else
                    {
                        // 태그가 비었는데 사이즈가 있는 경우
                        if (size > 0)
                        {
                            AddCollider(collider, x, y, size);
                            size = 0;
                        }
                    }
                }
            }
        }
        tagList.Clear();
    }

    // 콜라이더 추가 함수
    private void AddCollider(GameObject gameObject, int x, int y, int size)
    {
        BoxCollider2D boxcollider = gameObject.AddComponent<BoxCollider2D>();
        boxcollider.transform.parent = gameObject.transform;
        boxcollider.offset = new Vector2(x - size + (size * 0.5f), -y - 0.5f);
        boxcollider.size = new Vector2(size * 1f, 1f);
    }

    public void UpdateCollider()
    {
        BuildCollider();
    }
}
