using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaselineHitsplat : Hitsplat
{
    public HitsplatType hitsplatType = HitsplatType.OLD;
    // Start is called before the first frame update
    void Start()
    {
    }

    public TMP_FontAsset[] eleMaterials;
    public Sprite[] eleSlashes;
    public SpriteRenderer slashSprite;
    public SpriteRenderer critSprite;
    public SpriteRenderer critItemSprite;
    public SpriteRenderer dodgeSprite;

    protected override void CreateGraphics()
    {
        switch (hitsplatType)
        {
            case HitsplatType.OLD:
                text1.SetText("" + physicalDamage);
                if (elementalDamage > 0)
                {
                    text2.SetText("" + elementalDamage);
                }
                else
                {
                    text2.SetText("");
                }
                text2.color = (type.EleColor());
                break;
            case HitsplatType.SIDE_BY_SIDE:
                if (elementalDamage > 0)
                {
                    text1.SetText("" + physicalDamage+ " + <#"+ ColorUtility.ToHtmlStringRGB(type.EleColor())+"> "+elementalDamage + " "+type.EleName());
                    text2.SetText(effective ? "Effective!" : "");
                    text2.color = type.EleColor();
                }
                else
                {
                    text1.SetText("" + physicalDamage);
                    text2.SetText("");
                }
                break;
            case HitsplatType.ELE_ICONS:
                if (elementalDamage > 0)
                {
                    text1.SetText("" + physicalDamage + " + <#" + ColorUtility.ToHtmlStringRGB(type.EleColor()) + "> " + elementalDamage + " " + type.TempEleIconString());
                    text2.SetText(effective ? "Effective!" : "");
                    text2.color = type.EleColor();
                }
                else
                {
                    text1.SetText("" + physicalDamage);
                    text2.SetText("");
                }
                break;
            case HitsplatType.DERRICK_STYLE:
                if (elementalDamage > 0)
                {
                    slashSprite.enabled = true;
                    text1.SetText("" + physicalDamage);
                    text2.font = eleMaterials[(int)type];
                    text2.SetText("" + elementalDamage);
                    slashSprite.sprite = eleSlashes[(int)type];
                    if (effective)
                    {
                        critSprite.enabled = true;
                    }
                    if (crit)
                    {
                        critItemSprite.enabled = true;
                    }
                    if (dodge)
                    {
                        dodgeSprite.enabled = true;
                    }
                }
                else
                {
                    text1.SetText("" + physicalDamage);
                    text2.SetText("");
                    slashSprite.enabled = false;
                }
                break;

        }
    }

    public enum HitsplatType { OLD, SIDE_BY_SIDE, ELE_ICONS, DERRICK_STYLE }
}
