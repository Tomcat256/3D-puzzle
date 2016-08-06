using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureManager
{
    private string _textureDir = "data";
    private List<string> _textureNames;

    public List<string> TextureNames
    {
        get
        {
            return _textureNames;
        }
    }

    public TextureManager()
    {
        CollectTextureNames();
    }

    public Texture GetTextureByName(string name)
    {
        string filename = name;
        return ReadFromFile(filename);
    }

    public Texture GetRandomTexture()
    {
        string texName = _textureNames[Random.Range(0, _textureNames.Count)];
        return GetTextureByName(texName);
    }

    private Texture ReadFromFile(string filename)
    {
        Texture2D texture = new Texture2D(1, 1);

        FileInfo texFileInfo = new FileInfo(filename);

        byte[] rawImage = File.ReadAllBytes(texFileInfo.FullName);

        texture.LoadImage(rawImage);

        return texture;
    }

    private void CollectTextureNames()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(_textureDir);
        FileInfo[] files = directoryInfo.GetFiles("*.jpg");

        _textureNames = new List<string>();
        foreach (FileInfo file in files)
        {
            _textureNames.Add(file.FullName);
        }
    }
}
