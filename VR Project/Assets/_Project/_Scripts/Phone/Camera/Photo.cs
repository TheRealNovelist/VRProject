using System.IO;
using UnityEngine;

[System.Serializable]
public class Photo
{
    public string name;
    public Texture2D texture;
        
    private string path;

    public Photo(string path, int width, int height)
    {
        this.path = path;
            
        byte[] bytes = File.ReadAllBytes(path);
        name = Path.GetFileNameWithoutExtension(path);
            
        texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        texture.Apply();
    }
        
    public Photo(RenderTexture texture, string directoryPath, string name)
    {
        this.texture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = texture;
        this.texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        this.texture.Apply();
        
        byte[] bytes = this.texture.EncodeToPNG();

        string tempName = name;
        path = directoryPath + "/" + tempName + ".png";
        
        int version = 1;
        while (File.Exists(path))
        {
            tempName = name + $"_{version}";
            path = directoryPath + "/" + tempName +".png";
            version++;
        }
        
        this.name = tempName;
        Debug.Log($"Created new image: {tempName} + at {path}");
        File.WriteAllBytes(path, bytes);
    }
        
    //TODO: free up memory if image gone
        
    public void DeleteImage()
    {
        File.Delete(path);
    }
}