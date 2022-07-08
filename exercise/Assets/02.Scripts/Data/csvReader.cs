using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using UnityEngine;

public class csvReader : MonoBehaviour
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";         // 헤더에 맞는 스플릿 조건식
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";                               // 라인에 맞는 스플릿 조건식

    string itemCsvPath = "csvDir/item_csv";                                                                     // csv 경로
    public static List<List<object>> itemData = new List<List<object>>();                        // csv - item
    public static Dictionary<int, int> itemIndexPairs = new Dictionary<int, int>();               // csv 인덱스 매치
    public static Dictionary<string, int> itemHeaderPairs = new Dictionary<string, int>();   // csv 헤더 - 속성 값 매치

    string MonsterCsvPath = "csvDir/monster_csv";                                                                     // csv 경로
    public static List<List<object>> monsterData = new List<List<object>>();                        // csv - item
    public static Dictionary<int, int> monsterIndexPairs = new Dictionary<int, int>();               // csv 인덱스 매치
    public static Dictionary<string, int> monsterHeaderPairs = new Dictionary<string, int>();   // csv 헤더 - 속성 값 매치

    private void Awake()
    {
        csvInit(itemCsvPath, itemData, itemIndexPairs, itemHeaderPairs);
        csvInit(MonsterCsvPath, monsterData, monsterIndexPairs, monsterHeaderPairs);
    }

    private void Start()
    {

        // 예제 출력 형식 1
        //var a = csvReader.itemData[csvReader.itemIndexPairs[5020]][csvReader.itemHeaderPairs["itemName"]];
        //Debug.Log(a);

        // 예제 출력 형식 2
        //         var a = csvReader.itemData[3][csvReader.itemHeaderPairs["itemName"]];
        //         Debug.Log(a);

        // 예제 출력 형식 3
        //         var a = csvReader.itemData[csvReader.itemIndexPairs[5020]][2]];
        //         Debug.Log(a);

        // 예제 출력 형식 4
        //         var a = csvReader.itemData[3]][2]];
        //         Debug.Log(a);
        //var b = csvReader.monsterData[csvReader.monsterIndexPairs[2010]][csvReader.monsterHeaderPairs["monsterName"]];
        //Debug.Log(b);
    }

    public void csvInit(string path, List<List<object>> dataList, Dictionary<int, int> Ipairs, Dictionary<string, int> Hpairs)
    {
        TextAsset data = Resources.Load(path) as TextAsset;                         // TextAsset 생성
        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);                          // 스플릿
        makeIndexPairs(ref Ipairs, lines);                                                                      // 키 값 초기화 1
        makeHeaderPairs(ref Hpairs, lines);                                                                   // 키 값 초기화 2
        makeDataList(ref dataList, lines);                                                                         // 데이터 값 생성
    }

    public void makeIndexPairs(ref Dictionary<int, int> emptyIndex, string[] lines)
    {
        for (int i = 1; i < lines.Length - 1; i++)                                                   // 라인 전체길이 만큼 순회
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);                                  // 인덱스 값 획득 (예) (1000,1, 1, 1, img)

            if (values[0] != null)
            {
                emptyIndex.Add(Int32.Parse(values[0]), i - 1);                                 // 페어 키 : 인덱스 - 번호
            }
        }
    }


    public void makeHeaderPairs(ref Dictionary<string, int> emptyList, string[] lines)
    {
        string[] header = Regex.Split(lines[0], SPLIT_RE);                                // 헤더 조회

        for (int i = 0; i < header.Length; i++)                                                // 라인 전체길이 만큼 순회
        {
            emptyList.Add(header[i], i);                                                // 페어 키 : 헤더 이름 - 인덱스
        }
    }

    public void makeDataList(ref List<List<object>> emptyList, string[] lines)
    {
        int headerLength = (Regex.Split(lines[0], SPLIT_RE)).Length;                 // 헤더의 총 갯수

        for (int i = 1; i < lines.Length - 1; i++)                                                // 라인 전체길이 만큼 순회
        {
            var templist = new List<object>();                                    //  임시 객체 생성
            string[] values = Regex.Split(lines[i], SPLIT_RE);                                // 인덱스 [값] 획득   (예시 데이터 ::: 1000,1, 1, 1, img)


            for (int header = 0; header < headerLength; header++)                  // 인덱스 [값] 안에서 [헤더 길이] 만큼 반복
            {
                string value = values[header];          // [값]을 [String]으로 받아옴
                object finalValue = value;                 // [값]을 [objcet]으로 박싱             

                int intNum;                 // out 으로 받아올 [값]
                float floatNum;           // out 으로 받아올 [값]
                Sprite spriteImage;     // out 으로 받아올 [값]

                // [값]을 저장 하기 전 형 변환을 마친 후 박싱 작업

                if (header == headerLength - 1) { finalValue = Resources.Load<Sprite>(value); }
                else if (int.TryParse(value, out intNum)) finalValue = intNum;
                else if (float.TryParse(value, out floatNum)) finalValue = floatNum;

                templist.Add(finalValue);       // index 행에 대한 [값] 리스트
            }
            emptyList.Add(templist);               // 최종 데이터 리스트
        }
    }
}