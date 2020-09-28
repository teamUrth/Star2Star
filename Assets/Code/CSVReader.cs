using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Linq;

public class CSVReader
{
    public static void Test()
    {
        var text = "이문장은 \\ 나누어지는가? \\ 에";
        var texts = text.Split('\\');
        Debug.Log(texts.Length);
        for (var i = 0; i < texts.Length; i++)
        {
            Debug.Log(texts[i]);
        }
    }

    public static List<Dictionary<string, object>> Read(string file)
    {
        // 반환용 리스트를 선언
        var list = new List<Dictionary<string, object>>();
        // 텍스트 에셋으로 
        TextAsset data = Resources.Load(file) as TextAsset;
        var lines = Regex.Split(data.text, "\n");
        // 한줄밖에 없으면 종료
        if (lines.Length <= 1) return list;
        string[] header = lines[0].Split('\\'); // 첫째줄에 컬럼명이 있을 경우에
        for (var i = 0; i < header.Length; i++)
        {
            header[i] = header[i].Trim();
        }
        for (var i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split('\\');
            // 해당 열이 비어있다면 다음으로
            if (values.Length == 0 || values[0] == "") 
            {
                continue;
            }
            // 새 딕셔너리를 생성
            var entry = new Dictionary<string, object>();

            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart('\"').TrimEnd('\"').Replace("\\", "");
                object finalValue = value;
                if (int.TryParse(value, out int n))
                {
                    finalValue = n;
                }
                else if (float.TryParse(value, out float f))
                {
                    finalValue = f;
                }
                entry[header[j]] = finalValue;
            }
            list.Add(entry);
        }
        return list;
    }

    public static void Printer(List<Dictionary<string, object>> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            //list[i].ContainsKey("index")
            Debug.Log(i + "인덱스에 "+ list[i]["index"] + "에" + list[i]["actor"] + "가" + list[i]["message"]);
        }
    }

    public static GameMap LoadMap(string fileName)
    {
        Debug.Log(fileName + "찾는중");
        TextAsset data = Resources.Load(fileName) as TextAsset;
        // 파일 존재할 때
        GameMap map = new GameMap();
        var texts = data.text.Split('\n');
        string[] lineTexts;
        for (var i = 0; i < texts.Length; i++)
        {
            lineTexts = texts[i].Split(',');
            if (string.Compare(lineTexts[0], "Name", true) == 0)
            {
                map.Name = lineTexts[1];
            }
            else if (string.Compare(lineTexts[0], "ID", true) == 0)
            {
                map.ID = Convert.ToInt32(lineTexts[1]);
            }
            else if (string.Compare(lineTexts[0], "Tile", true) == 0)
            {
                map.TileName = lineTexts[1].Trim();
            }
            else if (string.Compare(lineTexts[0], "X", true) == 0)
            {
                map.X = Convert.ToInt32(lineTexts[1]);
            }
            else if (string.Compare(lineTexts[0], "Y", true) == 0)
            {
                map.Y = Convert.ToInt32(lineTexts[1]);
            }
            else if (string.Compare(lineTexts[0], "Width", true) == 0)
            {
                map.Width = Convert.ToInt32(lineTexts[1]);
            }
            else if (string.Compare(lineTexts[0], "Height", true) == 0)
            {
                map.Height = Convert.ToInt32(lineTexts[1]);
            }
            else if (string.Compare(lineTexts[0], "Neighbor", true) == 0)
            {
                for (var l = 0; l < 4; l++)
                {
                    map.Neighbor[l] = lineTexts[l + 1].Trim();
                }
            }
            else if (string.Compare(lineTexts[0], "Layer", true) == 0)
            {
                map.Layer = Convert.ToInt32(lineTexts[1]);
            }
            else if (string.Compare(lineTexts[0], "Data", true) == 0)
            {
                map.Tile = new int[map.Width, map.Height, map.Layer];
                for (var l = 0; l < map.Layer; l++)
                {
                    for (var y = 0; y < map.Height; y++)
                    {
                        for (var x = 0; x < map.Width; x++)
                        {
                            // + 1하는 이유는 Data라는 텍스트가 인덱스 0이기 때문
                            map.Tile[x, y, l] = Convert.ToInt32(lineTexts[x + (y * map.Width) + (l * map.Height * map.Width) + 1]);
                        }
                    }
                }
            }
            else if (string.Compare(lineTexts[0], "Collider", true) == 0)
            {
                map.Collider = new byte[map.Width, map.Height];
                for (var y = 0; y < map.Height; y++)
                {
                    for (var x = 0; x < map.Width; x++)
                    {
                        map.Collider[x, y] = Convert.ToByte(lineTexts[x + (y * map.Width) + 1]);
                    }
                }
            }
            else if (string.Compare(lineTexts[0], "Autotile", true) == 0)
            {
                for (var column = 1; column < lineTexts.Length; column++)
                {
                    if (string.Compare(lineTexts[column], "") == 0)
                    {
                        break;
                    }
                    map.AutoTiles.Add(lineTexts[column].Trim());
                }
            }
        }
        return map;
    }
}