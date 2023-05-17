using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    GameObject spawnObject;

    void Update()
    {
        UpdateCenterObject();
        //PlaceObjectByTouch();
    }
    void PlaceObjectByTouch()
    {
        // 터치가 하나라도 일어났다면
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // 만약 4개의 손가락으로 터치를 한다면 가장 먼저 터지가 일어난 터치를 반환
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                // spawnObject가 없다면 생성
                if (!spawnObject) spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                // spawnObject가 있다면 위치만 바꿔줌
                else
                {
                    spawnObject.transform.position = hitPose.position;
                    spawnObject.transform.rotation = hitPose.rotation;
                }

            }
        }
    }
    void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); // 스크린의 중앙
        List<ARRaycastHit> hits = new List<ARRaycastHit>(); // ray가 부딛힌 사물들
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes); // ray를 쏜다 (스크린의 중앙에, hits에 부딛힌 사물을 저장, 평면을 추적)
        
        // 평면이 인식되었고, ray가 사물과 부딛혔을때
        if (hits.Count > 0)
        {
            Pose placementPose = hits[0].pose; // ray가 부딛힌 사물들 중 가장 먼저 부딛힌 사물
            placeObject.SetActive(true);
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }
}
