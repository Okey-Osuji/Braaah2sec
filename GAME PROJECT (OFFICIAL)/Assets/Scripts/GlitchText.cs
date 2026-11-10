using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class GlitchText : MonoBehaviour
{
    [Header("Timing")]
    public float glitchInterval = 1.5f;
    public float glitchDuration = 0.25f;
    public float updateRate = 30f;

    [Header("Distortion")]
    public float horizontalShift = 12f;
    public float verticalShift = 4f;
    public float scaleJitter = 0.3f;
    [Range(0f, 1f)] public float characterChance = 0.4f;

    [Header("Color Split")]
    public Color glitchColorA = new Color(1f, 0f, 0.3f);
    public Color glitchColorB = new Color(0f, 1f, 1f);

    [Header("Font Swapping")]
    public TMP_FontAsset standardFont;
    public TMP_FontAsset[] glitchFonts;
    [Range(0f, 1f)] public float fontSwapChance = 0.5f;
    [Range(0f, 1f)] public float standardFontFlashChance = 0.2f;

    [Header("Word Swapping")]
    public string[] glitchWords;                 // e.g. ERROR, HELP, LOST
    [Range(0f, 1f)] public float wordSwapChance = 0.3f;       // chance per glitch frame to change the word
    [Range(0f, 1f)] public float titleFlashChance = 0.3f;     // when swapping, chance it goes back to the real title

    TMP_Text tmp;
    TMP_FontAsset originalFont;
    string originalText;
    string currentBase;                          // the word currently being glitched
    float nextGlitch, glitchEnd, nextFrame;
    bool glitching;
    Color baseColor;

    const string glitchChars = "!<>-_\\/[]{}=+*^?#%&01";

    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        originalText = tmp.text;
        currentBase = originalText;
        originalFont = standardFont != null ? standardFont : tmp.font;
        tmp.font = originalFont;
        baseColor = tmp.color;
        nextGlitch = Time.time + Random.Range(0.5f, glitchInterval);
    }

    void Update()
    {
        // Start a burst
        if (!glitching && Time.time >= nextGlitch)
        {
            glitching = true;
            glitchEnd = Time.time + glitchDuration * Random.Range(0.5f, 1.5f);
        }

        // End a burst and restore the clean title
        if (glitching && Time.time >= glitchEnd)
        {
            glitching = false;
            nextGlitch = Time.time + Random.Range(glitchInterval * 0.3f, glitchInterval * 1.7f);
            currentBase = originalText;
            tmp.font = originalFont;
            tmp.text = originalText;
            tmp.color = baseColor;
            tmp.ForceMeshUpdate();
            return;
        }

        if (!glitching || Time.time < nextFrame) return;
        nextFrame = Time.time + 1f / updateRate;

        ApplyGlitch();
    }

    void ApplyGlitch()
    {
        // Swap font
        if (glitchFonts != null && glitchFonts.Length > 0 && Random.value < fontSwapChance)
        {
            tmp.font = Random.value < standardFontFlashChance
                ? originalFont
                : glitchFonts[Random.Range(0, glitchFonts.Length)];
        }

        // Swap the word itself (or flash back to the real title)
        if (glitchWords != null && glitchWords.Length > 0 && Random.value < wordSwapChance)
        {
            currentBase = Random.value < titleFlashChance
                ? originalText
                : glitchWords[Random.Range(0, glitchWords.Length)];
        }

        // Swap some characters for symbols
        char[] chars = currentBase.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            if (chars[i] != ' ' && Random.value < 0.08f)
                chars[i] = glitchChars[Random.Range(0, glitchChars.Length)];
        tmp.text = new string(chars);
        tmp.ForceMeshUpdate();

        TMP_TextInfo info = tmp.textInfo;

        // Horizontal slice that shifts a band of the text together
        float sliceCenter = Random.Range(-20f, 20f);
        float sliceHeight = Random.Range(5f, 15f);
        float sliceOffset = Random.Range(-horizontalShift, horizontalShift) * 2f;

        for (int i = 0; i < info.characterCount; i++)
        {
            TMP_CharacterInfo c = info.characterInfo[i];
            if (!c.isVisible) continue;

            int mat = c.materialReferenceIndex;
            int vi = c.vertexIndex;
            Vector3[] verts = info.meshInfo[mat].vertices;
            Color32[] cols = info.meshInfo[mat].colors32;

            bool hit = Random.value < characterChance;
            Vector3 center = (verts[vi] + verts[vi + 2]) / 2f;

            Vector3 offset = Vector3.zero;
            float scaleX = 1f, scaleY = 1f;
            if (hit)
            {
                offset = new Vector3(
                    Random.Range(-horizontalShift, horizontalShift),
                    Random.Range(-verticalShift, verticalShift), 0);
                scaleX = 1f + Random.Range(-scaleJitter, scaleJitter);
                scaleY = 1f + Random.Range(-scaleJitter, scaleJitter);
            }

            if (Mathf.Abs(center.y - sliceCenter) < sliceHeight)
                offset.x += sliceOffset;

            for (int j = 0; j < 4; j++)
            {
                Vector3 v = verts[vi + j] - center;
                v.x *= scaleX;
                v.y *= scaleY;
                verts[vi + j] = center + v + offset;

                if (hit)
                    cols[vi + j] = Random.value > 0.5f ? glitchColorA : glitchColorB;
            }
        }

        // Push the modified mesh back
        for (int i = 0; i < info.meshInfo.Length; i++)
        {
            info.meshInfo[i].mesh.vertices = info.meshInfo[i].vertices;
            info.meshInfo[i].mesh.colors32 = info.meshInfo[i].colors32;
            tmp.UpdateGeometry(info.meshInfo[i].mesh, i);
        }
    }

    // Call from other scripts (e.g. a button hover) to glitch on demand
    public void TriggerGlitch() => nextGlitch = Time.time;

    // Use this if you change the title text at runtime
    public void SetText(string newText)
    {
        originalText = newText;
        currentBase = newText;
        tmp.text = newText;
    }
}