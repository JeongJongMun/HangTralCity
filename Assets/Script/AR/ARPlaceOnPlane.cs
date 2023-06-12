using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    public MeshRenderer arDefaultPlane;
    public ParticleSystem arDefaultPointCloud;
    GameObject spawnObject;

    void Update()
    {
        //UpdateCenterObject();
        PlaceObjectByTouch();
    }
    void PlaceObjectByTouch()
    {
        // 터치가 하나라도 일어났다면
        if (Input.touchCount > 0)
        {
            // 여러 터치 중 가장 먼저 터지가 일어난 터치를 반환
            Touch touch = Input.GetTouch(0); 
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                // spawnObject가 없다면 생성
                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                    spawnObject.GetComponent<MeshRenderer>().receiveShadows = false;
                }
                // spawnObject가 있다면 위치만 바꿔줌
                else
                {
                    spawnObject.transform.position = hitPose.position;
                    spawnObject.transform.rotation = hitPose.rotation;
                }
                // 투명도를 0으로 설정
                SetTransparencyValue(0f);
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
    void SetTransparencyValue(float value)
    {
        // ArDefaultPlane의 Material 배열 가져오기
        Material[] materials = arDefaultPlane.materials;

        foreach (Material material in materials)
        {
            // 기존에 있는 Material의 투명도를 0으로 설정
            Color materialColor = material.color;
            materialColor.a = value;
            material.color = materialColor;
        }
        // ArDefaultPointCloud의 Point 투명도를 0으로 설정
        Color pointColor = arDefaultPointCloud.startColor;
        pointColor.a = value;
        arDefaultPointCloud.startColor = pointColor;
    }
}
