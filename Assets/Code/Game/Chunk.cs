using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private Mesh _mesh;
    private MeshRenderer _meshRenderer;

    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();
    private List<Vector2> _uvs = new List<Vector2>();

    private int _tileInTextureX; // 텍스쳐의 가로가 몇개의 타일인지 저장하는 변수
    private int _tileInTextureY; // 텍스쳐의 세로가 몇개의 타일인지 저장하는 변수
    private float _tileSizeX;
    private float _tileSizeY;

    private int _frame;
    private int _currentFrame;
    private int _padding;
    private bool[,] _tiles;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // 텍스쳐를 업데이트 하는 메소드
    private void UpdateTexture(Texture texture)
    {
        _meshRenderer.material.SetTexture(1, texture);
        _tileInTextureX = texture.width / GameManager.PPU;
        _tileInTextureY = texture.height / GameManager.PPU;
        _tileSizeX = 1f / _tileInTextureX;
        _tileSizeY = 1f / _tileInTextureY;
    }

    // 오토타일의 텍스쳐를 업데이트 하는 코루틴
    private IEnumerator UpdateFrame()
    {
        WaitForSeconds wait = new WaitForSeconds(.5f);
        Vector2 offset = new Vector2();
        while (true)
        {
            // 프레임 
            if (++_currentFrame >= _frame)
            {
                _currentFrame = 0;
            }
            offset.x = _tileSizeX * 3 * _currentFrame;
            _meshRenderer.material.SetTextureOffset(1, offset);
            //AutoTile(_tiles);
            //UpdateMesh();

            yield return wait;
        }
    }

    public void BuildAutotile(Vector3 position, Texture texture, bool[,] tiles, int layer, int order)
    {
        _tiles = tiles;
        transform.position = position;
        UpdateTexture(texture);
        _frame = _tileInTextureX / 3;
        _currentFrame = 0;

        AutoTile(_tiles);
        UpdateMesh();
        if (layer < 3)
        {
            _meshRenderer.sortingLayerName = "Background"; // myMeshRenderer.sortingLayerID =
        }
        else// if (layer == 1)
        {
            _meshRenderer.sortingLayerName = "Front";
        }
        _meshRenderer.sortingOrder = order;
        if (_frame > 1)
        {
            StartCoroutine(UpdateFrame());
        }
    }
    
    public void BuildAutotile(Vector3 position, Texture texture, bool[,] tiles)
    {
        _tiles = tiles;
        transform.position = position;
        UpdateTexture(texture);
        _frame = _tileInTextureX / 3;
        _currentFrame = 0;

        AutoTile(_tiles);
        UpdateMesh();
    }

    public void BuildChunk(Vector3 position, int[,] tiles, Texture texture, int layer, int order, int padding)
    {
        transform.position = position;
        UpdateTexture(texture);
        _padding = padding;
        for (var y = 0; y < tiles.GetLength(1); y++)
        {
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                if (tiles[x, y] != 0)
                {
                    AddTile(new Vector3(x, -y), tiles[x, y]); //데카르트 좌표계이기때문에 y를 거꾸로 더해주어야 제대로 나옴
                }
            }
        }
        if (layer < 3)
        {
            _meshRenderer.sortingLayerName = "Background"; // myMeshRenderer.sortingLayerID =
        }
        else// if (layer == 1)
        {
            _meshRenderer.sortingLayerName = "Front";
        }
        _meshRenderer.sortingOrder = order;
        UpdateMesh();
    }

    private void AutoTile(bool[,] tiles)
    {
        var signX = 0;
        var signY = 0;
        bool[] tile = new bool[5];
        for (var y = 0; y < tiles.GetLength(1); y++)
        {
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                if (!tiles[x, y])
                {
                    continue;
                }
                for (var i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        signX = signY = 1;
                    }
                    else if (i == 1)
                    {
                        signY = 1;
                        signX = -1;
                    }
                    else if (i == 2)
                    {
                        signX = 1;
                        signY = -1;
                    }
                    else if (i == 3)
                    {
                        signX = signY = -1;
                    }

                    if ((x - signX < 0 || x - signX >= tiles.GetLength(0)) || ((y - signY < 0 || y - signY >= tiles.GetLength(1))))
                    {
                        tile[0] = true;
                    }
                    else
                    {
                        tile[0] = tiles[x - signX, y - signY];
                    }
                    if (x - signX < 0 || x - signX >= tiles.GetLength(0))
                    {
                        tile[2] = true;
                    }
                    else
                    {
                        tile[2] = tiles[x - signX, y];
                    }
                    if (y - signY < 0 || y - signY >= tiles.GetLength(1))
                    {
                        tile[1] = true;
                    }
                    else
                    {
                        tile[1] = tiles[x, y - signY];
                    }
                    if (x + signX < 0 || x + signX >= tiles.GetLength(0))
                    {
                        tile[3] = true;
                    }
                    else
                    {
                        tile[3] = tiles[x + signX, y];
                    }
                    if (y + signY < 0 || y + signY >= tiles.GetLength(1))
                    {
                        tile[4] = true;
                    }
                    else
                    {
                        tile[4] = tiles[x, y + signY];
                    }

                    int bit = 0;
                    int textureIndex;
                    for (var bitLoop = 0; bitLoop < 5; bitLoop++)
                    {
                        bit = bit | (tile[bitLoop] ? 1 << bitLoop : 0);
                    }

                    switch (bit)
                    {
                        case 2:
                        case 3:
                        case 10:
                        case 11:
                            textureIndex = 10;
                            break;
                        case 4:
                        case 5:
                        case 20:
                        case 21:
                            textureIndex = 6;
                            break;
                        case 6:
                        case 14:
                        case 22:
                        case 30:
                            textureIndex = 3;
                            break;
                        case 7:
                            textureIndex = 12;
                            break;
                        case 12:
                        case 13:
                        case 28:
                        case 29:
                            textureIndex = 5;
                            break;
                        case 15:
                            textureIndex = 11;
                            break;
                        case 18:
                        case 19:
                        case 26:
                        case 27:
                            textureIndex = 7;
                            break;
                        case 23:
                            textureIndex = 9;
                            break;
                        case 31:
                            textureIndex = 8;
                            break;
                        default:
                            textureIndex = 4;
                            break;
                    }

                    if (textureIndex != 3)
                    {
                        int offsetX;
                        int offsetY;
                        offsetY = (textureIndex - 1) / 3;
                        offsetX = textureIndex - 1 - (offsetY * 3);

                        if (signX == -1)
                        {
                            if (offsetX == 0)
                            {
                                offsetX = 2;
                            }
                            else if (offsetX == 2)
                            {
                                offsetX = 0;
                            }
                        }
                        if (signY == -1)
                        {
                            if (offsetY == 1)
                            {
                                offsetY = 3;
                            }
                            else if (offsetY == 3)
                            {
                                offsetY = 1;
                            }
                        }
                        textureIndex = (offsetY * 3) + offsetX + 1;
                    }
                    AddAutoTile(new Vector3(x - (signX - 1) * .25f, -y + (signY - 1) * .25f), textureIndex, 1, new Vector3(-(signX - 1) * _tileSizeX / 4, (signY + 1) * _tileSizeY / 4, 0));
                }
            }
        }
    }

    private void AddAutoTile(Vector3 position, int id, int frame, Vector3 offset)
    { 
        for (var i = 0; i < 6; i++)
        {
            _triangles.Add(TileData.Triangles[i] + _vertices.Count);
        }
        for (var i = 0; i < 4; i++)
        {
            _vertices.Add(TileData.AutoTileVertices[i] + position);
        }
        AddAutoTileTexture(id, frame, offset);
    }

    private void AddAutoTileTexture(int id, int frame, Vector3 offset)
    {
        int tileFrame = _tileInTextureX / 3;
        // 0부터 시작되므로 1부터 시작하게
        id -= 1;

        float y = id / 3; // 이때 정수형으로 저장됨 소숫점데이터 손실
        float x = id - (y * 3);
        x *= _tileSizeX;
        y *= _tileSizeY;
        y = 1f - y - _tileSizeY;
        x += _tileSizeX * (_currentFrame * 3);

        x += offset.x;
        y += offset.y;
        _uvs.Add(new Vector2(x, y + _tileSizeY / 2));
        _uvs.Add(new Vector2(x + _tileSizeX / 2, y + _tileSizeY / 2));
        _uvs.Add(new Vector2(x + _tileSizeX / 2, y));
        _uvs.Add(new Vector2(x, y));
    }

    private void AddTile(Vector3 position, int id)
    {
        for (int i = 0; i < 6; i++)
        {
            _triangles.Add(TileData.Triangles[i] + _vertices.Count);
        }
        for (int i = 0; i < 4; i++)
        {
            _vertices.Add(TileData.Vertices[i] + position);
        }
        AddTexture(id);
    }

    private void AddTexture(int id)
    {
        // 0부터 시작되므로 1부터 시작하게
        id -= _padding;
        
        float y = id / _tileInTextureX; // 이때 정수형으로 저장됨 소숫점데이터 손실
        float x = id - (y * _tileInTextureX);
        x *= _tileSizeX;
        y *= _tileSizeY;
        y = 1f - y - _tileSizeY;

        _uvs.Add(new Vector2(x, y + _tileSizeY));
        _uvs.Add(new Vector2(x + _tileSizeX, y + _tileSizeY));
        _uvs.Add(new Vector2(x + _tileSizeX, y));
        _uvs.Add(new Vector2(x, y));
    }

    private void UpdateMesh()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _mesh.Clear();
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.RecalculateNormals();

        // 다 그린 리스트 초기화
        _vertices.Clear();
        _triangles.Clear();
        _uvs.Clear();
    }
}