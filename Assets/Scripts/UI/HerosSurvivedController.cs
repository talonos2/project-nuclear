using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HerosSurvivedController : MonoBehaviour
{
    public Image hiro;
    public Image guard;
    public Image squire;
    public Image greenMaid;
    public Image priest;
    public Image cultest;
    public Image redMaid;
    public Image blueMaid;
    public Image trapper;
    public Image blacksmith;
    private float delay = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (GameData.Instance.Douglass == 0) hiro.enabled = false;
        if (GameData.Instance.Sara == 0) blueMaid.enabled = false;
        if (GameData.Instance.McDermit == 0) guard.enabled = false;
        if (GameData.Instance.Todd == 0) cultest.enabled = false;
        if (GameData.Instance.Norma == 0) redMaid.enabled = false;
        if (GameData.Instance.Derringer == 0) blacksmith.enabled = false;
        if (GameData.Instance.Melvardius == 0) priest.enabled = false;
        if (GameData.Instance.Mara == 0) greenMaid.enabled = false;
        if (GameData.Instance.Devon == 0) trapper.enabled = false;
        if (GameData.Instance.Pendleton == 0) squire.enabled = false;
    }

    private void Update()
    {
        if (GameData.Instance.isInDialogue || GameData.Instance.isCutscene) return;
        delay -= Time.deltaTime;

        if (delay < 0) {
            SceneManager.LoadScene("TitleScreen");
        }

    }

}
