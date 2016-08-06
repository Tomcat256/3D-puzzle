using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureManager
{
    private string _textureDir = "data";
    private List<string> _textureNames;

    public const string RANDOM_TEXTURE_NAME = "RandomTexture";

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

    public Texture GetRandomTexture()
    {
        string texName = _textureNames[Random.Range(0, _textureNames.Count)];
        return GetTexture(texName);
    }

    public Texture GetTexture(string textureName)
    {
        if(textureName == RANDOM_TEXTURE_NAME)
        {
            return GetRandomTexture();
        }

        Texture2D texture = new Texture2D(1, 1);

        byte[] rawImage = LoadRawTextureImage(textureName);

        texture.LoadImage(rawImage);

        return texture;
    }

    public byte[] LoadRawTextureImage(string textureName)
    {
        string filename = TextureNameToFilename(textureName);

        FileInfo texFileInfo = new FileInfo(filename);

        byte[] rawImage = File.ReadAllBytes(texFileInfo.FullName);

        return rawImage;
    }

    private string TextureNameToFilename(string textureName)
    {
        return textureName;
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
