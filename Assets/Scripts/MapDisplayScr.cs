using UnityEngine;

public class MapDisplayScr : MonoBehaviour
{

    #region Public Fields
    public Renderer textureRenderer;
    #endregion
 
    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
}
