using UnityEngine;
using UnityEngine.UI;

// A script that imitates basic functions of an Android listview
// Contains all functions for adding/removing objects from list
public class ListView : MonoBehaviour {
    public GameObject ListItemPrefab;

    public GameObject AddView() {
        // Used for transform
        VerticalLayoutGroup ListContent = GetComponentInChildren<VerticalLayoutGroup>(); 

        // Create list item and return gameobject (to let user edit text properties, etc.)
        if (ListItemPrefab != null)
            return (GameObject) Instantiate(ListItemPrefab, ListContent.transform);

        return null;
    }

    public void DestroyAllViews() {
        HorizontalLayoutGroup[] childViews = GetComponentsInChildren<HorizontalLayoutGroup>();
        foreach (HorizontalLayoutGroup view in childViews)
            Destroy(view.gameObject);
    }
}
