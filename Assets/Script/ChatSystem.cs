using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    public Queue<string> sentences;
    public string currentSentence;
    public TextMeshPro text;
    public GameObject quad;
    private GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player(Clone)");
    }
    private void Update()
    {
        transform.position = player.transform.position + new Vector3(0,3.5f,0);
    }

    public void Ondialogue(string[] lines, Transform chatPoint)
    {
        transform.position = chatPoint.position;
        sentences = new Queue<string>();
        sentences.Clear();
        foreach (var line in lines)
        {
            sentences.Enqueue(line);
        }
        StartCoroutine(DialogueFlow(chatPoint));
    }

    IEnumerator DialogueFlow(Transform chatPoint)
    {
        yield return null;
        while (sentences.Count > 0)
        {
            currentSentence = sentences.Dequeue();
            text.text = currentSentence;

            // 말풍선 크기 설정
            float x = text.preferredWidth;
            x = (x > 3) ? 3 : x + 0.3f;
            quad.transform.localScale = new Vector2(x, text.preferredHeight + 0.3f);

            transform.position = new Vector2(chatPoint.position.x, chatPoint.position.y + text.preferredHeight / 2);
            yield return new WaitForSeconds(3);
        }
        Destroy(gameObject);
    }
}
