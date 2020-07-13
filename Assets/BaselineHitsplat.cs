using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaselineHitsplat : Hitsplat
{
    public HitsplatType hitsplatType = HitsplatType.OLD;
    // Start is called before the first frame update
    void Start()
    {
    }

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
                }
                else
                {
                    text1.SetText("" + physicalDamage);
                }
                break;

        }
    }

    public enum HitsplatType { OLD, SIDE_BY_SIDE }
}
