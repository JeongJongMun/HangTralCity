using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabToInstantiate; // �ν��Ͻ�ȭ �� ������

    private GameObject instantiatedObject; // �ν��Ͻ�ȭ �� ��ü

    // �巡�� ���� �� ȣ��Ǵ� �ݹ�
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �ν��Ͻ�ȭ �� ������ �ε�
        instantiatedObject = Instantiate(prefabToInstantiate);

        // UI ��ü�� ���� �ν��Ͻ�ȭ �� ��ü �̵�
        Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
        position.z = 0f;
        instantiatedObject.transform.position = position;
    }

    // �巡�� �� ȣ��Ǵ� �ݹ�
    public void OnDrag(PointerEventData eventData)
    {
        // UI ��ü�� ���� �ν��Ͻ�ȭ �� ��ü �̵�
        Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
        position.z = 0f;
        instantiatedObject.transform.position = position;
    }

    // �巡�� ���� �� ȣ��Ǵ� �ݹ�
    public void OnEndDrag(PointerEventData eventData)
    {
        // �ν��Ͻ�ȭ �� ��ü�� UI ��ü�� ������ ��ġ�� ��ġ
        //instantiatedObject.transform.position = transform.position;
    }
}
