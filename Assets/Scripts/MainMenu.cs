using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class SizeUIPreset
{
    public uint ColumnsCount;
    public uint RowssCount;

    public SizeUIPreset(uint rowsCount, uint columnsCount)
    {
        RowssCount = rowsCount;
        ColumnsCount = columnsCount;
    }

    public string Caption
    {
        get
        {
            return System.String.Format("{0}x{1}", RowssCount, ColumnsCount);
        }
    }
}


public class MainMenu : MonoBehaviour {

    public GameObject TogglePrefab;
    public GameObject ParentList;
    public GameObject SizeDropdown;

    public Vector2[] AvailableSizes;

    private List<SizeUIPreset> _dropdownPresets = new List<SizeUIPreset>();

    // Use this for initialization
	void Start () {
        CreateTextureToggles();
        SetupSizeDropdown();
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

    private void SetupSizeDropdown()
    {
        foreach(Vector2 size in AvailableSizes)
        {
            SizeUIPreset preset = new SizeUIPreset((uint)size.x, (uint)size.y);
            SizeDropdown.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(preset.Caption));
            _dropdownPresets.Add(preset);
        }

        SizeDropdown.GetComponent<Dropdown>().value = 0;
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

        SizeUIPreset selectedSize = _dropdownPresets[SizeDropdown.GetComponent<Dropdown>().value];

        GlobalContext.Instance.ColumnsCount = selectedSize.ColumnsCount;
        GlobalContext.Instance.RowsCount = selectedSize.RowssCount;
        SceneManager.LoadScene("MainScene");
    }
}
