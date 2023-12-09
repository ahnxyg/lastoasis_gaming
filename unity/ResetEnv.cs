using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetEnv : MonoBehaviour
{
    private string resourcesPath = "Assets/Resources/";
    private string filePath = "Temp/Environment";
    private string extension = "prefab";

    private bool saved = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!this.saved)
        //{
        //    PrefabUtilityExtension.CreateNewPrefab(this.gameObject, $"{this.resourcesPath}{this.filePath}.{this.extension}");
        //    this.saved = true;
        //}
    }
    public static void CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);

        // 필드 복사
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            try
            {
                field.SetValue(copy, field.GetValue(original));
            }
            catch
            {

            }
        }

        // 프로퍼티 복사
        System.Reflection.PropertyInfo[] properties = type.GetProperties();
        foreach (System.Reflection.PropertyInfo property in properties)
        {
            if (property.CanWrite && property.CanRead)
            {
                try
                {
                    property.SetValue(copy, property.GetValue(original, null), null);
                }
                catch
                {
                    // 예외 발생시 무시
                }
            }
        }
    }
    public void EnvReset()
    {
        GameObject prefab = Resources.Load<GameObject>(this.filePath);
        GameObject env = Instantiate(prefab);
        if (prefab != null)
        {
            CopyComponentsFromPrefab(env, gameObject);
        }
    }

    private void CopyComponentsFromPrefab(GameObject prefab, GameObject targetGameObject)
    {
        foreach (Component prefabComponent in prefab.GetComponents<Component>())
        {
            // 기존 컴포넌트 제거
            Component existingComponent = targetGameObject.GetComponent(prefabComponent.GetType());
            if (existingComponent != null)
            {
                if (existingComponent.GetType().Name != "Transform")
                {
                    Destroy(existingComponent);
                }
                else
                { 
                }
            }

            // 새 컴포넌트 복사
            CopyComponent(prefabComponent, targetGameObject);
        }

        // 자식 오브젝트에 대해서도 동일한 작업 수행
        for (int i = 0; i < prefab.transform.childCount; i++)
        {
            Transform prefabChild = prefab.transform.GetChild(i);
            Transform targetChild = targetGameObject.transform.Find(prefabChild.name);

            if (targetChild != null)
            {
                CopyComponentsFromPrefab(prefabChild.gameObject, targetChild.gameObject);
            }
        }
    }
}

public static class PrefabUtilityExtension
{
    public static void CreateNewPrefab(GameObject gameObject, string path)
    {
        //PrefabUtility.SaveAsPrefabAsset(gameObject, path);
    }
}

interface EnvComponents
{
    public static EnvComponents Instance { get; set; }
}