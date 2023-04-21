using System.IO;
using UnityEngine;

[System.Serializable]
public class PhoneImage
{
    public string name;
    public Texture2D image;
        
    private string path;

    public PhoneImage(string path, int width, int height)
    {
        this.path = path;
            
        byte[] bytes = File.ReadAllBytes(path);
        name = Path.GetFileNameWithoutExtension(path);
            
        image = new Texture2D(width, height);
        image.LoadImage(bytes);
        image.Apply();
    }
        
    public PhoneImage(RenderTexture texture, string directoryPath, string name)
    {
        image = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = texture;
        image.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        image.Apply();
        
        byte[] bytes = image.EncodeToPNG();

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
        Debug.Log("Created new image: " + tempName);
        File.WriteAllBytes(path, bytes);
    }
        
    //TODO: free up memory if image gone
        
    public void DeleteImage()
    {
        File.Delete(path);
    }
}