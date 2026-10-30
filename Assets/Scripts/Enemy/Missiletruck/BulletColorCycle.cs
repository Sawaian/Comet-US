
using UnityEngine;

public class BulletColorCycle : MonoBehaviour
{
    [Header("Bullet Colors")]
    [SerializeField]
    private Color[] bulletColors =
    {
        new Color(1f, 0.27f, 0.71f),  // Pink
        new Color(0.88f, 0.25f, 0.98f), // Magenta
        new Color(0.60f, 0.33f, 1f), // Violet
        new Color(0.27f, 0.87f, 1f)  // Cyan
    };

    private Renderer bulletRenderer;
    private MaterialPropertyBlock propertyBlock;

    private static int currentVolleyColor = 0;

    private static readonly int BaseColorID =
        Shader.PropertyToID("_BaseColor");

    private static readonly int ColorID =
        Shader.PropertyToID("_Color");

    private void Awake()
    {
        bulletRenderer = GetComponentInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    public void SetVolleyColor(int volleyIndex)
    {
        if (bulletRenderer == null || bulletColors.Length == 0)
            return;

        int colorIndex = Mathf.Abs(volleyIndex) % bulletColors.Length;

        Color selectedColor = bulletColors[colorIndex];

        bulletRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetColor(BaseColorID, selectedColor);
        propertyBlock.SetColor(ColorID, selectedColor);

        bulletRenderer.SetPropertyBlock(propertyBlock);
    }

    public static int NextVolley()
    {
        return currentVolleyColor++;
    }
}
