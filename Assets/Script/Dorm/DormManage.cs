using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

[System.Serializable]
public class ResponseBody
{
    public string statusCode;
    public string body;
}

[System.Serializable]
public class Body
{
    public Dictionary<string, string> furniture_positions;
}

public class Vector3JsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<Vector3>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JArray obj = JArray.Load(reader);
        List<Vector3> vectors = new List<Vector3>();

        foreach (JObject jObject in obj)
        {
            var x = (float)jObject["x"];
            var y = (float)jObject["y"];
            var z = (float)jObject["z"];
            vectors.Add(new Vector3(x, y, z));
        }

        return vectors;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        List<Vector3> vectors = (List<Vector3>)value;
        JArray array = new JArray();

        foreach (Vector3 vector3 in vectors)
        {
            JObject obj = new JObject
            {
                { "x", vector3.x },
                { "y", vector3.y },
                { "z", vector3.z }
            };
            array.Add(obj);
        }

        serializer.Serialize(writer, array);
    }
}

public static class JsonHelper
{
    public static T[] GetJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

public class DormManage : MonoBehaviour
{
    private string apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/dorm-custom";
    private string apiGatewayUrl2 = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/dorm-custom";
    private Vector2 scroll_amount = new Vector2(0.1f, 0); // 스크롤 양
    public Toggle edit;
    public GameObject save_btn, reset_btn, furniture_list, left_btn, right_btn;
    public GameObject bed, carpet1, carpet2, desk, drawer, flower_pot, sofa, trash_bin;

    private void Start()
    {
        edit.onValueChanged.AddListener(delegate { Edit(); });
        save_btn.GetComponent<Button>().onClick.AddListener(Save);
        left_btn.GetComponent<Button>().onClick.AddListener(ClickLeftBtn);
        right_btn.GetComponent<Button>().onClick.AddListener(ClickRightBtn);
        reset_btn.GetComponent<Button>().onClick.AddListener(Load);

        StartCoroutine(LoadFurniturePositionsFromDynamoDB());
    }

    private IEnumerator LoadFurniturePositionsFromDynamoDB()
    {
        // Create the request payload
        Dictionary<string, object> payloadData = new Dictionary<string, object>
        {
            { "nickname", PlayerInfo.playerInfo.nickname }
        };
        Dictionary<string, object> payload = new Dictionary<string, object>
        {
            { "data", payloadData }
        };

        string payloadJson = JsonConvert.SerializeObject(payload);
        Debug.Log("Payload JSON: " + payloadJson);

        // Send the request to the API Gateway
        UnityWebRequest request = new UnityWebRequest(apiGatewayUrl2, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(payloadJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error loading furniture positions: " + request.error);
            yield break;
        }


        Debug.Log("Furniture positions loaded successfully: " + request.downloadHandler.text);

        ResponseBody responseBody = JsonConvert.DeserializeObject<ResponseBody>(request.downloadHandler.text);

        if (responseBody.body == null)
        {
            Debug.LogError("Body is null in the response");
            yield break;
        }


        Body body = JsonConvert.DeserializeObject<Body>(responseBody.body);

        if (body.furniture_positions == null)
        {
            Debug.LogError("Furniture positions not found in the response body");
            yield break;
        }

        foreach (KeyValuePair<string, string> furniturePosition in body.furniture_positions)
        {
            string furnitureName = furniturePosition.Key;
            JArray positionsArray = JArray.Parse(furniturePosition.Value);
            List<Vector3> positionsList = positionsArray.ToObject<List<Vector3>>(new JsonSerializer { Converters = { new Vector3JsonConverter() } });
            PlayerInfo.playerInfo.funiturePos[furnitureName] = positionsList;
        }
        request.Dispose(); // 메모리 누수 방지를 위해 추가

        Load(); // Now, apply the positions to the scene


    }

    private IEnumerator SaveFurniturePositionsToDynamoDB()
    {
        // Convert furniture positions to JSON format
        Dictionary<string, string> furniturePositionsJson = new Dictionary<string, string>();

        foreach (var pair in PlayerInfo.playerInfo.funiturePos)
        {
            string listString = JsonConvert.SerializeObject(pair.Value, new Vector3JsonConverter());
            furniturePositionsJson.Add(pair.Key, listString);
        }

        // Create the request payload
        Dictionary<string, object> payloadData = new Dictionary<string, object>
        {
            { "nickname", PlayerInfo.playerInfo.nickname },
            { "furniture_positions", furniturePositionsJson }
        };
        Dictionary<string, object> payload = new Dictionary<string, object>
        {
            { "data", payloadData }
        };


        string payloadJson = JsonConvert.SerializeObject(payload);
        Debug.Log("Payload JSON: " + payloadJson);


        // Send the request to the API Gateway
        UnityWebRequest request = new UnityWebRequest(apiGatewayUrl, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(payloadJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error saving furniture positions: " + request.error);
        }
        else
        {
            Debug.Log("Furniture positions saved successfully: " + request.downloadHandler.text);
        }

        request.Dispose();  // 메모리 누수 방지를 위해 추가
    }

    private void Edit()
    {
        if (edit.isOn)
        {
            save_btn.SetActive(true);
            reset_btn.SetActive(true);
            furniture_list.SetActive(true);
            left_btn.SetActive(true);
            right_btn.SetActive(true);
        }
        else
        {
            save_btn.SetActive(false);
            reset_btn.SetActive(false);
            furniture_list.SetActive(false);
            left_btn.SetActive(false);
            right_btn.SetActive(false);
        }

    }
    private void Save() // 방 커스터마이징 정보 저장
    {
        // 씬의 모든 오브젝트 가져오기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // 기존 PlayerInfo의 기숙사 커스터마이징 정보 삭제
        PlayerInfo.playerInfo.funiturePos.Clear();

        // 모든 오브젝트를 순환하며 태그를 비교
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag == "Custom_Dorm")
            {
                // 원하는 태그가 있는 오브젝트의 이름과 포지션 가져오기
                string name = obj.name;
                Vector3 pos = obj.transform.position;

                // PlayerInfo에 기숙사 커스터마이징 정보 저장
                if (PlayerInfo.playerInfo.funiturePos.ContainsKey(name))
                {
                    PlayerInfo.playerInfo.funiturePos[name].Add(pos); // 기존 key가 있다면 list에 추가
                }
                else // key가 없다면 list를 만들어서 추가
                {
                    List<Vector3> list = new List<Vector3> { pos };
                    PlayerInfo.playerInfo.funiturePos.Add(name, list);
                }
            }
        }
        StartCoroutine(SaveFurniturePositionsToDynamoDB());

        // 확인용 출력   
        foreach (var kvp in PlayerInfo.playerInfo.funiturePos)
        {
            Debug.Log("Key = " + kvp.Key);
            foreach (var pos in kvp.Value)
            {
                Debug.Log("Position: " + pos);
            }
        }
    }
    private void Load() // 방 커스터마이징 정보 불러오기
    {
        // 씬의 모든 오브젝트 가져오기
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // 모든 오브젝트를 순환하며 태그를 비교하여 기존 방 커스터마이징 정보 삭제
        foreach (GameObject obj in allObjects) if (obj.tag == "Custom_Dorm") Destroy(obj);

        // PlayerInfo에 저장된 방 커스터마이징 정보 불러오기
        string to_remove = "(Clone)";

        foreach (var kvp in PlayerInfo.playerInfo.funiturePos)
        {
            if (kvp.Key.Replace(to_remove, "") == "Bed") foreach (Vector3 pos in kvp.Value) Instantiate(bed, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Carpet1") foreach (Vector3 pos in kvp.Value) Instantiate(carpet1, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Carpet2") foreach (Vector3 pos in kvp.Value) Instantiate(carpet2, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Desk") foreach (Vector3 pos in kvp.Value) Instantiate(desk, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Drawer") foreach (Vector3 pos in kvp.Value) Instantiate(drawer, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "FlowerPot") foreach (Vector3 pos in kvp.Value) Instantiate(flower_pot, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "Sofa") foreach (Vector3 pos in kvp.Value) Instantiate(sofa, pos, Quaternion.identity);
            else if (kvp.Key.Replace(to_remove, "") == "TrashBin") foreach (Vector3 pos in kvp.Value) Instantiate(trash_bin, pos, Quaternion.identity);
            Debug.Log("Key = " + kvp.Key + ", Value : " + kvp.Value);
        }
    }
    private void ClickLeftBtn() // 스크롤 바 왼쪽으로 이동
    {
        furniture_list.GetComponent<ScrollRect>().normalizedPosition -= scroll_amount;
    }
    private void ClickRightBtn() // 스크롤 바 오른쪽으로 이동
    {
        furniture_list.GetComponent<ScrollRect>().normalizedPosition += scroll_amount;
    }
}