using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FriendList
{
    public List<string> friends;
}

public class FriendResponseBody
{
    public string statusCode;
    public string body;
}

public class FriendManage : MonoBehaviour
{
    public GameObject bed;
    public GameObject carpet1;
    public GameObject carpet2;
    public GameObject desk;
    public GameObject drawer;
    public GameObject flower_pot;
    public GameObject sofa;
    public GameObject trash_bin;

    [Header("GoPlay -> Off")]
    public GameObject addBtn;
    public GameObject inputField;
    public GameObject scrollView;

    [Header("GoPlay -> On")]
    public GameObject backBtn;

    [Header("Content : Prefab SpawnPos")]
    public GameObject content;

    [Header("Prefab")]
    public GameObject friend;
    public GameObject playerPrefab;

    Button[] deleteButtons;
    Button[] goPlayButtons;
    GameObject playerSpawned;

    private void Start()
    {
        addBtn.GetComponent<Button>().onClick.AddListener(AddFriend);
        backBtn.GetComponent<Button>().onClick.AddListener(BackToFriendList);
        LoadFriend();
        SetButtonListener();
    }

    void SetButtonListener()
    {
        deleteButtons = FindObjectsOfType<Button>().Where(button => button.name == "DeleteBtn").ToArray();
        goPlayButtons = FindObjectsOfType<Button>().Where(button => button.name == "GoPlayBtn").ToArray();

        foreach (Button button in deleteButtons)
        {
            button.onClick.AddListener(() => DeleteFriend(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text, button.transform.parent.gameObject));
        }

        foreach (Button button in goPlayButtons)
        {
            button.onClick.AddListener(() => GoPlayDorm(button.transform.parent.GetChild(0).GetComponent<TMP_Text>().text));
        }
    }

    void AddFriend()
    {
        string friendName;
        if (inputField.GetComponent<TMP_InputField>().text != null)
        {
            friendName = inputField.GetComponent<TMP_InputField>().text;
            inputField.GetComponent<TMP_InputField>().text = null;

            GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
            _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
            SetButtonListener();

            Debug.LogFormat("Friend Added: {0}", friendName);
            StartCoroutine(UpdateFriendListInDynamoDB(friendName, _friend));
        }
    }

    void DeleteFriend(string friendName, GameObject parentObject)
    {
        StartCoroutine(DeleteFriendFromDynamoDB(friendName, parentObject));
    }

    void LoadFriend()
    {
        StartCoroutine(GetFriendListFromDynamoDB());
    }

    void GoPlayDorm(string name)
    {
        Debug.LogFormat("GoPlayDorm: {0}", name);

        addBtn.SetActive(false);
        inputField.SetActive(false);
        scrollView.SetActive(false);
        backBtn.SetActive(true);
        playerSpawned = Instantiate(playerPrefab, new Vector3(0, 0, -1), Quaternion.identity);


        StartCoroutine(LoadFurniturePositionsFromDynamoDB(name));
    }

    void BackToFriendList()
    {
        addBtn.SetActive(true);
        inputField.SetActive(true);
        scrollView.SetActive(true);
        backBtn.SetActive(false);
        Destroy(playerSpawned);
    }

    IEnumerator UpdateFriendListInDynamoDB(string friendName, GameObject _friend)
    {
        string apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/friend";

        var request = new UnityWebRequest(apiGatewayUrl, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        string jsonBody = $"{{\"friendName\":\"{friendName}\", \"userName\":\"{PlayerInfo.playerInfo.nickname}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            FriendResponseBody response = JsonUtility.FromJson<FriendResponseBody>(request.downloadHandler.text);

            if (response.statusCode == "400")
            {
                Debug.Log("User not found, friend not added!");
                Destroy(_friend);
            }
            else
            {
                Debug.Log("Friend list update successful!");
                LoadFriend();
            }
        }
        else
        {
            Debug.Log($"Friend list update failed: {request.error}");
            Debug.Log($"Response Code: {(int)request.responseCode}");
            Debug.Log($"Response: {request.downloadHandler.text}");
            Destroy(_friend);
        }

        request.Dispose();
    }

    IEnumerator GetFriendListFromDynamoDB()
    {
        string apiGatewayUrl2 = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/";

        var request = new UnityWebRequest(apiGatewayUrl2, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        string jsonBody = $"{{\"userName\":\"{PlayerInfo.playerInfo.nickname}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        try
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Get friend list successful!");
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Response: {request.downloadHandler.text}");

                FriendResponseBody response = JsonUtility.FromJson<FriendResponseBody>(jsonResponse);

                FriendList friendList = JsonUtility.FromJson<FriendList>(response.body);
                Debug.Log(friendList.friends.Count);

                foreach (Transform child in content.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (var friendName in friendList.friends)
                {
                    Debug.Log("Friend: " + friendName);

                    GameObject _friend = Instantiate(friend, new Vector2(0, 0), Quaternion.identity, content.transform);
                    _friend.transform.GetChild(0).GetComponent<TMP_Text>().text = friendName;
                }
                SetButtonListener();
            }
            else
            {
                Debug.Log($"Get friend list failed: {request.error}");
                Debug.Log($"Response Code: {(int)request.responseCode}");
                Debug.Log($"Response: {request.downloadHandler.text}");
            }
        }
        finally
        {
            request.Dispose();
        }
    }

    IEnumerator DeleteFriendFromDynamoDB(string friendName, GameObject parentObject)
    {
        string apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/frienddelete";

        var request = new UnityWebRequest(apiGatewayUrl, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();

        string jsonBody = $"{{\"friendName\":\"{friendName}\", \"userName\":\"{PlayerInfo.playerInfo.nickname}\"}}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        try
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Friend deletion successful!");
                Destroy(parentObject);
                LoadFriend();
            }
            else
            {
                Debug.Log($"Friend deletion failed: {request.error}");
                Debug.Log($"Response Code: {(int)request.responseCode}");
                Debug.Log($"Response: {request.downloadHandler.text}");
            }
        }
        finally
        {
            request.Dispose();
        }
    }

    IEnumerator LoadFurniturePositionsFromDynamoDB(string friendName)
    {
        string apiGatewayUrl = "https://q4xm6p11e1.execute-api.ap-northeast-2.amazonaws.com/test1/dorm-custom";

        // Create the request payload
        Dictionary<string, object> payloadData = new Dictionary<string, object>
        {
            { "data", new Dictionary<string, string> { { "nickname", friendName } } }
        };
        string payloadJson = JsonConvert.SerializeObject(payloadData);

        // Send the request to the API Gateway
        UnityWebRequest request = new UnityWebRequest(apiGatewayUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(payloadJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            FriendResponseBody response = JsonConvert.DeserializeObject<FriendResponseBody>(request.downloadHandler.text);
            Debug.Log(response.statusCode);
            Debug.Log(response.body);


            // Check if the request was successful
            if (response.statusCode == "200")
            {
                // Get the furniture positions from the response body
                Body body = JsonConvert.DeserializeObject<Body>(response.body);
                Dictionary<string, string> furniturePositions = body.furniture_positions;

                // 씬의 모든 오브젝트 가져오기
                GameObject[] allObjects = FindObjectsOfType<GameObject>();

                // 모든 오브젝트를 순환하며 태그를 비교하여 기존 방 커스터마이징 정보 삭제
                foreach (GameObject obj in allObjects) if (obj.tag == "Custom_Dorm") Destroy(obj);

                // Apply the furniture positions to the panel
                foreach (KeyValuePair<string, string> furniturePosition in furniturePositions)
                {
                    string furnitureName = furniturePosition.Key;
                    Debug.LogFormat("FurnitureName:{0}", furnitureName);
                    JArray positionsArrayRaw = JArray.Parse(furniturePosition.Value);
                    Vector3[] positionsArray = positionsArrayRaw.ToObject<Vector3[]>();

                    List<Vector3> positionsList = new List<Vector3>(positionsArray);
                    PlayerInfo.playerInfo.funiturePos[furnitureName] = positionsList;

                    GameObject furniturePrefab = GetFurniturePrefab(furnitureName);
                    if (furniturePrefab != null)
                    {
                        InstantiateFurniture(furniturePrefab, positionsList);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to load friend's furniture positions: " + response.body);
            }
        }
        else
        {
            Debug.LogError("Error loading friend's furniture positions: " + request.error);
        }

        request.Dispose(); // Prevent memory leak
    }


    void InstantiateFurniture(GameObject prefab, List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    GameObject GetFurniturePrefab(string furnitureName)
    {
        GameObject prefab = null;

        switch (furnitureName)
        {
            case "Bed(Clone)":
                prefab = bed;
                break;
            case "Carpet1(Clone)":
                prefab = carpet1;
                break;
            case "Carpet2(Clone)":
                prefab = carpet2;
                break;
            case "Desk(Clone)":
                prefab = desk;
                break;
            case "Drawer(Clone)":
                prefab = drawer;
                break;
            case "FlowerPot(Clone)":
                prefab = flower_pot;
                break;
            case "Sofa(Clone)":
                prefab = sofa;
                break;
            case "TrashBin(Clone)":
                prefab = trash_bin;
                break;
            default:
                Debug.LogWarning("Prefab not found for furniture: " + furnitureName);
                break;
        }

        return prefab;
    }

}
