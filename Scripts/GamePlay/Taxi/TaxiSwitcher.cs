using UnityEngine;

public sealed class TaxiCarSwitcher : MonoBehaviour
{
    [SerializeField] private Transform modelRoot;

    public void SwitchTo(GameObject carPrefab)
    {
        if (modelRoot == null || carPrefab == null)
            return;

        for (int i = modelRoot.childCount - 1; i >= 0; i--)
            Destroy(modelRoot.GetChild(i).gameObject);

        GameObject instance = Instantiate(carPrefab, modelRoot);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = Vector3.one;
    }
}