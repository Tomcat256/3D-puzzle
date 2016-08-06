using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject TogglePrefab;
    public GameObject ParentList;

	// Use this for initialization
	void Start () {
        CreateTextureToggles();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateTextureToggles()
    {
        TextureManager tm = new TextureManager();
        foreach (string texName in tm.TextureNames)
        {
            GameObject toggle = (GameObject)Instantiate(TogglePrefab);
            toggle.transform.SetParent(ParentList.transform);
            toggle.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
            toggle.GetComponent<ImageListSelector>().TextureName = texName;
            Image img = toggle.transform.Find("Image").gameObject.GetComponent<Image>();
            img.material = new Material(img.material);
            img.material.mainTexture = tm.GetTexture(texName);
        }
    }

    public void onPlayButtonClicked()
    {
        IEnumerator<Toggle> togglesEnumerator = GetComponent<ToggleGroup>().ActiveToggles().GetEnumerator();

        if(togglesEnumerator.MoveNext())
        {
            GlobalContext.Instance.TextureName = togglesEnumerator.Current.GetComponent<ImageListSelector>().TextureName;
        }
        else
        {
            GlobalContext.Instance.TextureName = TextureManager.RANDOM_TEXTURE_NAME;
        }

        GlobalContext.Instance.ColumnsCount = 5;
        GlobalContext.Instance.RowsCount = 5;
        SceneManager.LoadScene("MainScene");
    }
}
