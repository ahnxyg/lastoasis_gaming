using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class Movement : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도
    public float attackRange = 2.0f; // 공격 범위
    private SkeletonAnimation spineAnimation;

    void Start()
    {
        // Spine 컴포넌트를 찾습니다.
        spineAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        // "red" 태그를 가진 오브젝트를 찾습니다.
        GameObject[] redObjects = GameObject.FindGameObjectsWithTag("RedBuilding");

        // 가장 가까운 "red" 태그를 가진 오브젝트를 찾습니다.
        GameObject closestRedObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject redObject in redObjects)
        {
            
            float distance = Vector3.Distance(transform.position, redObject.transform.position);
            if (distance < closestDistance)
            {
                closestRedObject = redObject;
                closestDistance = distance;
            }
        }

        // 가장 가까운 "red" 태그를 가진 오브젝트를 향해 이동합니다.
        if (closestRedObject != null)
        {
            Vector3 direction = (closestRedObject.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
            spineAnimation.AnimationName = "Side_Walk";
        }
    }
}
