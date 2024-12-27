using UnityEditor;
using UnityEngine;

public class SpriteDefaults : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        TextureImporter textImp = (TextureImporter)assetImporter;
        textImp.filterMode = FilterMode.Point;
        textImp.spritePixelsPerUnit = 16;
        textImp.textureCompression = TextureImporterCompression.Uncompressed;
    }
}
