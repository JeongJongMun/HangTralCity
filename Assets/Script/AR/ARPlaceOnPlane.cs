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
        // ��ġ�� �ϳ��� �Ͼ�ٸ�
        if (Input.touchCount > 0)
        {
            // ���� ��ġ �� ���� ���� ������ �Ͼ ��ġ�� ��ȯ
            Touch touch = Input.GetTouch(0); 
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                // spawnObject�� ���ٸ� ����
                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                    spawnObject.GetComponent<MeshRenderer>().receiveShadows = false;
                }
                // spawnObject�� �ִٸ� ��ġ�� �ٲ���
                else
                {
                    spawnObject.transform.position = hitPose.position;
                    spawnObject.transform.rotation = hitPose.rotation;
                }
                // ������ 0���� ����
                SetTransparencyValue(0f);
            }
        }
    }
    void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); // ��ũ���� �߾�
        List<ARRaycastHit> hits = new List<ARRaycastHit>(); // ray�� �ε��� �繰��
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes); // ray�� ��� (��ũ���� �߾ӿ�, hits�� �ε��� �繰�� ����, ����� ����)
        
        // ����� �νĵǾ���, ray�� �繰�� �ε�������
        if (hits.Count > 0)
        {
            Pose placementPose = hits[0].pose; // ray�� �ε��� �繰�� �� ���� ���� �ε��� �繰
            placeObject.SetActive(true);
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }
    void SetTransparencyValue(float value)
    {
        // ArDefaultPlane�� Material �迭 ��������
        Material[] materials = arDefaultPlane.materials;

        foreach (Material material in materials)
        {
            // ������ �ִ� Material�� ������ 0���� ����
            Color materialColor = material.color;
            materialColor.a = value;
            material.color = materialColor;
        }
        // ArDefaultPointCloud�� Point ������ 0���� ����
        Color pointColor = arDefaultPointCloud.startColor;
        pointColor.a = value;
        arDefaultPointCloud.startColor = pointColor;
    }
}
