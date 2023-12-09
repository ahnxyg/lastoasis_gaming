using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using TMPro;

public class MobController : MonoBehaviour
{
    public static MobController instance;

    [Header("Path Finding")]
    public Transform seeker;
    public Vector2 target;
    //Grid grid;
    //public GameObject GridOwner;
    public Queue<Vector2> wayQueue = new Queue<Vector2>();
    public static bool walkable = true;
    public float moveSpeed;
    public float rotSpeed;
    public float range;
    public bool isWalk;
    public bool isWalking;

    public GameObject dmgtxt;
    public TMP_Text popupText;


    private bool chek = false;
    private bool isAttacking = false;
    public int cost;
    public float speed;
    public float maxHealth;
    public float health;
    public float attackRange;
    public float damage;
    public float attackSpeed;
    public string selfTag;
    public string type;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    private AudioSource theAudio;
    public AudioClip attackAudio;

    private float originalDamage;
    private float defence;

    public bool alive = true;

    Rigidbody2D rigid;
    Collider2D coll;
    WaitForFixedUpdate wait;
    private SkeletonAnimation spineAnimation;

    private float attackCooldown; // 공격 쿨다운 타이머
    private bool canAttack = true;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        seeker = gameObject.transform;
        isWalking = false;
        //grid = GameObject.Find("GridManager").GetComponent<Grid>();

        selfTag = gameObject.tag;
        health = maxHealth;
        if (type == "small")
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else if (type == "medium")
        {
            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        else if (type == "big")
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        firingPoint = gameObject.transform;
        originalDamage = damage;
    }

    void Awake()
    {
        // 격자 생성
        // grid = GameObject.Find("GridManager").GetComponent<Grid>();
        walkable = true;
        spineAnimation = GetComponent<SkeletonAnimation>();
        spineAnimation.timeScale = attackSpeed;
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
        instance = this;
    }

    void Update() //여기에 움직임 구현
    {

        if (health <= 0) return;
        GameObject closestRedObject = FindClosestRedObject();

        damage = originalDamage + UpgradeManager.instance.dmgUp;
        defence = UpgradeManager.instance.defUp;
        if (closestRedObject == this.gameObject)
        {

            return;
        }
        if (closestRedObject == null)
        {

            return;
        }
        float closestObjectPositionX = closestRedObject.transform.position.x;
        float myPositionX = transform.position.x;

        if (gameObject.tag == "Blue")
        {
            if (closestObjectPositionX < myPositionX)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (gameObject.tag == "Red")
        {
            if (closestObjectPositionX < myPositionX)
            {

                gameObject.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (closestRedObject != null)
        {

            float distance = Vector3.Distance(transform.position, closestRedObject.transform.position);
            //closestRedObject = FindClosestRedObject();
            if (distance <= attackRange && closestRedObject.tag != gameObject.tag)
            {
                chek = true;
                //isWalking = false;
                // 공격 범위 내에 있을 때는 attack 함수 실행
                if (canAttack)
                {
                    if (gameObject)
                    {
                        attack(closestRedObject);
                    }
                }
            }
            else
            {
                // 공격 범위 밖에 있을 때는 걷기 함수 실행
                //walk(closestRedObject);

                if (closestRedObject != null && !isAttacking)
                {
                    Vector2 startPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                    if (gameObject.tag == "Blue")
                    {
                        target = new Vector2(closestRedObject.transform.position.x - 2f, closestRedObject.transform.position.y);
                    }
                    else
                    {
                        target = new Vector2(closestRedObject.transform.position.x + 2f, closestRedObject.transform.position.y);
                    }
                    seeker = gameObject.transform;
                    StartFindPath(seeker.position, target);
                }
            }
        }
    }


    private void attack(GameObject closestRedObject)
    {
        isAttacking = true;
        if (closestRedObject != null)
        {

            spineAnimation.AnimationState.SetAnimation(0, "Side_Attack", false);
            theAudio.clip = attackAudio;

            if (closestRedObject.name == "RedSilo" || closestRedObject.name == "BlueSilo" || closestRedObject.name == "RedNexus" || closestRedObject.name == "BlueNexus")
            {
                Building building = closestRedObject.GetComponent<Building>();
                if (building != null)
                {
                    canAttack = false;
                    StartCoroutine(AttackIntervalBuilding(building));

                }

                //StartCoroutine(WaitAndExecuteCode());
            }
            else if (closestRedObject.name == "Wizard(Clone)")
            {
                WizardController wiz = closestRedObject.GetComponent<WizardController>();
                if (wiz != null)
                {
                    canAttack = false;
                    StartCoroutine(AttackIntervalWiz(wiz));
                }
            }
            else if (closestRedObject.name == "Knight(Clone)")
            {
                KnightController kni = closestRedObject.GetComponent<KnightController>();
                if (kni != null)
                {
                    canAttack = false;
                    StartCoroutine(AttackIntervalKnight(kni));
                }
            }
            else if (closestRedObject.layer == LayerMask.NameToLayer("Monster") || closestRedObject.layer == LayerMask.NameToLayer("RangeMob"))
            {
                MobController currentMob = closestRedObject.GetComponent<MobController>();
                if (currentMob != null)
                {
                    canAttack = false;
                    StartCoroutine(AttackInterval(currentMob));
                }
                //StartCoroutine(WaitAndExecuteCode());
            }
        }
    }
    IEnumerator AttackInterval(MobController mob)
    {
        //Debug.Log(gameObject.name);
        yield return new WaitForSeconds((1 / attackSpeed) * 0.7f);
        if (mob == null)
        {
            canAttack = true;
            isAttacking = false;
            yield break;
        }
        theAudio.Play();
        if (type == "big" && mob.type == "small")
        {
            if (gameObject.layer == 10)
            {
                GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
                bulletObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                bulletObj.tag = gameObject.tag;
                Bullet bulletScript = bulletObj.GetComponent<Bullet>();
                GameObject target = FindClosestRedObject();
                try
                {
                    Vector3 pos = target.GetComponent<MobController>().transform.position;

                    Vector3 directionToTarget = (pos - firingPoint.position).normalized;

                    bulletScript.SetTarget(target.transform, damage * 1.5f);
                }
                catch (NullReferenceException)
                {
                    Destroy(bulletObj);
                }
            }
            else
            {
                mob.takeHit(damage * 1.5f);
            }
        }
        else if (type == "big" && mob.type == "medium")
        {
            if (gameObject.layer == 10)
            {
                GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
                bulletObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                bulletObj.tag = gameObject.tag;
                Bullet bulletScript = bulletObj.GetComponent<Bullet>();
                GameObject target = FindClosestRedObject();
                try
                {
                    Vector3 pos = target.GetComponent<MobController>().transform.position;

                    Vector3 directionToTarget = (pos - firingPoint.position).normalized;

                    bulletScript.SetTarget(target.transform, damage * 1.5f);
                }
                catch (NullReferenceException)
                {
                    canAttack = true;
                    isAttacking = false;
                    Destroy(bulletObj);
                    yield break;
                }
                catch
                {
                    canAttack = true;
                    isAttacking = false;
                    yield break;
                }

            }
            else
            {
                mob.takeHit(damage * 1.3f);
            }
        }
        else if (type == "medium" && mob.type == "small")
        {
            if (gameObject.layer == 10)
            {
                GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
                bulletObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                bulletObj.tag = gameObject.tag;
                Bullet bulletScript = bulletObj.GetComponent<Bullet>();
                GameObject target = FindClosestRedObject();
                try
                {
                    Vector3 pos = target.GetComponent<MobController>().transform.position;

                    Vector3 directionToTarget = (pos - firingPoint.position).normalized;

                    bulletScript.SetTarget(target.transform, damage * 1.5f);
                }
                catch (NullReferenceException)
                {
                    Destroy(bulletObj);
                }
            }
            else
            {
                mob.takeHit(damage * 1.5f);
            }
        }
        else
        {
            if (gameObject.layer == 10)
            {
                GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
                bulletObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                bulletObj.tag = gameObject.tag;
                Bullet bulletScript = bulletObj.GetComponent<Bullet>();
                GameObject target = FindClosestRedObject();
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > attackRange)
                {
                    canAttack = true;
                    isAttacking = false;
                    yield break;
                }
                try
                {
                    Vector3 pos = target.GetComponent<MobController>().transform.position;

                    Vector3 directionToTarget = (pos - firingPoint.position).normalized;
                    if (target == null)
                    {
                        canAttack = true;
                        isAttacking = false;
                        yield break;
                    }
                    bulletScript.SetTarget(target.transform, damage);
                }
                catch (NullReferenceException)
                {
                    Destroy(bulletObj);
                    //Debug.Log("TTTTTTTT");
                    canAttack = true;
                    isAttacking = false;
                    yield break;
                }
            }
            else
            {
                mob.takeHit(damage);
            }

        }

        if (gameObject.tag == "Blue")
        {
            try
            {
                GameManager.instance.totalDmg[gameObject.name] += (int)damage;
                DmgShow.instance.UpdateText();
            }
            catch
            {
                canAttack = true;
                isAttacking = false;
                yield break;
            }

        }


        yield return new WaitForSeconds((1f / attackSpeed) * 0.3f);

        canAttack = true;
        isAttacking = false;
    }

    IEnumerator AttackIntervalBuilding(Building build)
    {
        yield return new WaitForSeconds((1 / attackSpeed) * 0.7f);
        if (build == null)
        {
            canAttack = true;
            isAttacking = false;
            yield break;
        }
        theAudio.Play();
        if (gameObject.layer == 10)
        {

            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            bulletObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            bulletObj.tag = gameObject.tag;
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            //Debug.Log(bulletObj.transform.position);
            GameObject target = FindClosestRedObject();
            try
            {
                Vector3 pos = target.GetComponent<Building>().transform.position;

                Vector3 directionToTarget = (pos - firingPoint.position).normalized;

                bulletScript.SetTarget(target.transform, damage);
            }
            catch (NullReferenceException)
            {
                //Debug.Log("b");
                Destroy(bulletObj);
            }

        }
        else build.TakeDamage(damage);

        if (gameObject.tag == "Blue" && GameManager.instance.currentSceneName == "MainGamePlay")
        {
            try
            {
                GameManager.instance.totalDmg[gameObject.name] += (int)damage;
                //Debug.Log(GameManager.instance.totalDmg[gameObject.name]);
                DmgShow.instance.UpdateText();
            }
            catch
            {
                GameManager.instance.totalDmg.Add(gameObject.name, (int)damage);
                //Debug.Log("this " + gameObject.name);
                DmgShow.instance.UpdateText();
            }
        }

        yield return new WaitForSeconds((1 / attackSpeed) * 0.3f);
        canAttack = true;
        isAttacking = false;
    }

    IEnumerator AttackIntervalWiz(WizardController wizard)
    {
        yield return new WaitForSeconds((1 / attackSpeed) * 0.7f);
        if (wizard == null)
        {
            canAttack = true;
            isAttacking = false;
            yield break;
        }
        theAudio.Play();
        if (gameObject.layer == 10)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            bulletObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            bulletObj.tag = gameObject.tag;
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            GameObject target = FindClosestRedObject();
            try
            {
                Vector3 pos = target.GetComponent<WizardController>().transform.position;

                Vector3 directionToTarget = (pos - firingPoint.position).normalized;

                bulletScript.SetTarget(target.transform, damage);
            }
            catch (NullReferenceException)
            {
                Destroy(bulletObj);
            }
        }
        else wizard.takeHit(damage);

        if (gameObject.tag == "Blue")
        {

            try
            {
                GameManager.instance.totalDmg[gameObject.name] += (int)damage;
                Debug.Log(GameManager.instance.totalDmg[gameObject.name]);
                DmgShow.instance.UpdateText();
            }
            catch
            {
                GameManager.instance.totalDmg.Add(gameObject.name, (int)damage);
                Debug.Log("this " + gameObject.name);
                DmgShow.instance.UpdateText();
            }
        }
        yield return new WaitForSeconds((1 / attackSpeed) * 0.3f);
        canAttack = true;
        isAttacking = false;
    }

    IEnumerator AttackIntervalKnight(KnightController knight)
    {
        yield return new WaitForSeconds((1 / attackSpeed) * 0.7f);
        if (knight == null)
        {
            canAttack = true;
            isWalk = true;
            isWalking = true;
            isAttacking = false;
            yield break;
        }
        theAudio.Play();
        if (gameObject.layer == 10)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            bulletObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            bulletObj.tag = gameObject.tag;
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            GameObject target = FindClosestRedObject();
            try
            {
                Vector3 pos = target.GetComponent<KnightController>().transform.position;

                Vector3 directionToTarget = (pos - firingPoint.position).normalized;

                bulletScript.SetTarget(target.transform, damage);
            }
            catch (NullReferenceException)
            {
                Destroy(bulletObj);
            }
        }
        else knight.takeHit(damage);

        if (gameObject.tag == "Blue" && GameManager.instance.currentSceneName == "MainGamePlay")
        {
            try
            {
                GameManager.instance.totalDmg[gameObject.name] += (int)damage;
                Debug.Log(GameManager.instance.totalDmg[gameObject.name]);
                DmgShow.instance.UpdateText();
            }
            catch
            {
                GameManager.instance.totalDmg.Add(gameObject.name, (int)damage);
                Debug.Log("this " + gameObject.name);
                DmgShow.instance.UpdateText();
            }
        }
        yield return new WaitForSeconds((1 / attackSpeed) * 0.3f);
        canAttack = true;
        isAttacking = false;
    }
    // 아래 두 코루틴은 idle animation 중간에 집어넣어서 자연스럽게 보이기 위함. 공속을 모두 1로 통일할꺼면 안써도됨 (Side_attack 플레이 시간이 1초)
    private IEnumerator WaitAndExecuteCode()
    {
        yield return new WaitForSeconds(1.0f);
        //canAttack = true;
        attackCooldown = 1f / attackSpeed;
        StartCoroutine(AttackCooldownTimer());
    }

    private IEnumerator AttackCooldownTimer()
    {
        spineAnimation.AnimationName = "Side_Idle";
        yield return new WaitForSeconds(0);

        canAttack = true;
    }
    private IEnumerator Death()
    {
        gameObject.tag = "Untagged";
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    private GameObject FindClosestRedObject()
    {
        GameObject[] redObjects = null;

        if (selfTag == "Blue")
        {

            redObjects = GameObject.FindGameObjectsWithTag("Red").Where(obj => obj.layer != 12).ToArray();

        }
        else
        {
            redObjects = GameObject.FindGameObjectsWithTag("Blue").Where(obj => obj.layer != 12).ToArray();
        }

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
        if (closestRedObject == this) return null;
        else return closestRedObject;
    }
    /*     private void walk(GameObject closestRedObject)
           {

               // 가장 가까운 "red" 태그를 가진 오브젝트를 향해 이동합니다.
               if (closestRedObject != null)
               {
                   Vector3 direction = (closestRedObject.transform.position - transform.position).normalized;
                   transform.Translate(direction * speed * Time.deltaTime);
                   spineAnimation.AnimationName = "Side_Walk";
               }
           }*/


    private void walk(GameObject closestRedObject)
    {

        if (closestRedObject != null)
        {
            Vector3 targetPosition = closestRedObject.transform.position;
            Vector3 direction = (targetPosition - transform.position).normalized;


            float separationDistance = 1.0f;
            Vector3 separation = Vector3.zero;

            Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, separationDistance);
            int neighborCount = 0;

            foreach (var neighbor in neighbors)
            {
                if (neighbor.gameObject != gameObject && neighbor.gameObject.CompareTag(gameObject.tag))
                {
                    Vector3 awayFromNeighbor = transform.position - neighbor.transform.position;
                    separation += awayFromNeighbor.normalized / awayFromNeighbor.magnitude;
                    neighborCount++;
                }
            }

            if (neighborCount > 0)
            {
                separation /= neighborCount;
            }


            direction += separation.normalized;
            direction.Normalize();

            Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, -11, 8);

            transform.position = newPosition;
            spineAnimation.AnimationName = "Side_Walk";
        }
    }



    public void takeHit(float damage)
    {
        try
        {
            if (health - (damage - defence) <= 0)
            {
                health -= (damage - defence);
                speed = 0;
                popupText.text = (damage - defence).ToString();
                Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, 0);
                Instantiate(dmgtxt, pos, Quaternion.identity);

                spineAnimation.AnimationState.SetAnimation(0, "Side_Death", false);

                //GameManager.instance.gameMoney += 3;

                StartCoroutine(Death());
            }
            else
            {
                popupText.text = (damage - defence).ToString();
                Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, 0);
                Instantiate(dmgtxt, pos, Quaternion.identity);

                health -= (damage - defence);
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("NULL REF EXC");
            return;
        }
        catch (MissingReferenceException)
        {
            Debug.Log("Missing REF EXC");
            return;
        }
        if (health <= 0)
        {
            try
            {
                //Destroy(gameObject);
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }
    }

    private void OnEnable()
    {
        coll.enabled = true;
        rigid.simulated = true;
        health = maxHealth;
    }

    private void Init(SpawnData data)
    {
        speed = data.speed;
        maxHealth = data.maxHealth;
        health = data.health;
        attackRange = data.attackRange;
        cost = data.cost;
        damage = data.damage;
    }
    //path find
    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        StopAllCoroutines();
        //grid = GameObject.Find("GridManager").GetComponent<Grid>();
        target = targetPos;
        StartCoroutine(FindPath(startPos, targetPos));
    }

    // 길찾기 로직.
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        // start, target의 좌표를 grid로 분할한 좌표로 지정.
        Node startNode = Grid.instance.NodeFromWorldPoint(startPos);
        Node targetNode = Grid.instance.NodeFromWorldPoint(targetPos);

        // target에 도착했는지 확인하는 변수.
        bool pathSuccess = false;

        if (!startNode.walkable)
        {
            //Debug.Log("Unwalkable StartNode 입니다.");


        }
        //Debug.Log(targetNode.walkable);
        if (!targetNode.walkable)
        {
            Node closestWalkableNode = FindClosestWalkableNode(targetNode);

            if (closestWalkableNode != null)
            {
                targetNode = closestWalkableNode;
            }
            else
            {

            }
        }
        // walkable한 targetNode인 경우 길찾기 시작.
        if (targetNode.walkable)
        {
            // openSet, closedSet 생성.
            // closedSet은 이미 계산 고려한 노드들.
            // openSet은 계산할 가치가 있는 노드들.
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            // closedSet에서 가장 최저의 F를 가지는 노드를 빼낸다. 
            while (openSet.Count > 0)
            {
                // currentNode를 계산 후 openSet에서 빼야 한다.
                Node currentNode = openSet[0];
                // 모든 openSet에 대해, current보다 f값이 작거나, h(휴리스틱)값이 작으면 그것을 current로 지정.
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                // openSet에서 current를 뺀 후, closedSet에 추가.
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // 방금 들어온 노드가 목적지 인 경우
                if (currentNode == targetNode)
                {
                    // seeker가 위치한 지점이 target이 아닌 경우
                    if (pathSuccess == false)
                    {
                        // wayQueue에 PATH를 넣어준다.
                        PushWay(RetracePath(startNode, targetNode));
                    }
                    pathSuccess = true;
                    break;
                }

                // current의 상하좌우 노드들에 대하여 g,h cost를 고려한다.
                foreach (Node neighbour in Grid.instance.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;
                    // F cost 생성.
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // 이웃으로 가는 F cost가 이웃의 G보다 짧거나, 방문해볼 Openset에 그 값이 없다면,
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        // openSet에 추가.
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }


        // 길을 찾았을 경우(계산 다 끝난경우) 이동시킴.
        if (pathSuccess == true)
        {
            // 이동중이라는 변수 ON
            isWalking = true;

            // wayQueue를 따라 이동시킨다.
            while (wayQueue.Count > 0 && isWalking)
            {

                var dir = this.wayQueue.First() - (Vector2)this.seeker.transform.position;
                Vector2 direction = (wayQueue.First() - (Vector2)transform.position).normalized;
                if (gameObject.tag == "Red")
                {
                    direction.x = -direction.x;
                }
                transform.Translate(direction * speed * Time.deltaTime);
                spineAnimation.AnimationName = "Side_Walk";

                GameObject closestRedObject = FindClosestRedObject();
                if (closestRedObject != null)
                {

                    float distance = Vector3.Distance(transform.position, closestRedObject.transform.position);
                    closestRedObject = FindClosestRedObject();
                    if (distance <= attackRange && closestRedObject.tag != gameObject.tag)
                    {
                        isWalking = false;
                        //wayQueue.Clear();
                        StopAllCoroutines();

                    }

                    //gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized * moveSpeed * 5 * Time.deltaTime;
                    if (Vector2.Distance((Vector2)seeker.position, wayQueue.First()) <= 1.05f)
                    {
                        //Debug.Log("HEll");
                        wayQueue.Dequeue();
                    }
                    yield return new WaitForSeconds(0.02f);
                }
                // 이동중이라는 변수 OFF
                isWalking = false;
            }
        }

        // WayQueue에 새로운 PATH를 넣어준다.
        void PushWay(Vector2[] array)
        {
            wayQueue.Clear();
            foreach (Vector2 item in array)
            {

                wayQueue.Enqueue(item);
            }
        }

        // 현재 큐에 거꾸로 저장되어있으므로, 역순으로 wayQueue를 뒤집어준다. 
        Vector2[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            // Grid의 path에 찾은 길을 등록한다.
            Grid.instance.path = path;
            Vector2[] wayPoints = SimplifyPath(path);
            return wayPoints;
        }

        // Node에서 Vector 정보만 빼낸다.
        Vector2[] SimplifyPath(List<Node> path)
        {
            List<Vector2> wayPoints = new List<Vector2>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 0; i < path.Count; i++)
            {
                wayPoints.Add(path[i].worldPosition);
            }
            return wayPoints.ToArray();
        }

        // custom g cost 또는 휴리스틱 추정치를 계산하는 함수.
        // 매개변수로 들어오는 값에 따라 기능이 바뀝니다.
        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
            // 대각선 - 14, 상하좌우 - 10.
            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        Node FindClosestWalkableNode(Node currentNode)
        {
            // Get all the neighbors of the current node.
            List<Node> neighbors = Grid.instance.GetNeighbours(currentNode);

            Node closestWalkableNode = null;
            float closestDistance = float.MaxValue;

            foreach (Node neighbor in neighbors)
            {
                if (neighbor.walkable)
                {
                    float distance = Vector2.Distance(currentNode.worldPosition, neighbor.worldPosition);
                    if (distance < closestDistance)
                    {
                        closestWalkableNode = neighbor;
                        closestDistance = distance;
                    }
                }
            }

            return closestWalkableNode;
        }

    }
}