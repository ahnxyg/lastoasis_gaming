using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nukeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 1초 뒤에 현재 게임 오브젝트를 파괴
        StartCoroutine(DestroyAfterDelay(1.0f));
    }

    // 코루틴을 사용하여 지연 후 게임 오브젝트 파괴
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void Update()
    {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        MoveCollider(collider);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        try {
            if (collider.gameObject.CompareTag("Red") && collider.gameObject.name != "Knight(Clone)" && (collider.gameObject.layer == 7 || collider.gameObject.layer == 10))
            {
                MobController temp = collider.gameObject.GetComponent<MobController>();
                if (temp != null)
                {
                    NukePower.instance[0].blueKillCount++;
                    NukePower.instance[0].blueTotalDmg += temp.health;
                    temp.takeHit(99999);
                }
            }
            else
            {
                KnightController temp = collider.GetComponent<KnightController>();
                if (temp != null)
                {
                    temp.takeHit(500);
                }
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }
    }

    private void MoveCollider(Collider2D collider)
    {
        // 이동에 사용할 값을 설정 (예: x좌표에 1.0f를 더함)
        float moveAmount = 0.00001f;

        // 현재 위치에서 moveAmount만큼 이동
        Vector3 newPosition = collider.transform.position;
        newPosition.x += moveAmount;

        // Collider의 위치를 업데이트
        collider.transform.position = newPosition;
    }
}
