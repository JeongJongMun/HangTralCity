using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabToInstantiate; // 인스턴스화 할 프리팹

    private GameObject instantiatedObject; // 인스턴스화 된 객체

    // 드래그 시작 시 호출되는 콜백
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 인스턴스화 할 프리팹 로드
        instantiatedObject = Instantiate(prefabToInstantiate);

        // UI 객체를 따라 인스턴스화 된 객체 이동
        Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
        position.z = 0f;
        instantiatedObject.transform.position = position;
    }

    // 드래그 중 호출되는 콜백
    public void OnDrag(PointerEventData eventData)
    {
        // UI 객체를 따라 인스턴스화 된 객체 이동
        Vector3 position = Camera.main.ScreenToWorldPoint(eventData.position);
        position.z = 0f;
        instantiatedObject.transform.position = position;
    }

    // 드래그 종료 시 호출되는 콜백
    public void OnEndDrag(PointerEventData eventData)
    {
        // 인스턴스화 된 객체를 UI 객체와 동일한 위치에 배치
        //instantiatedObject.transform.position = transform.position;
    }
}
