using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDance : MonoBehaviour
{
    public void ExitDanceing()
    {
        ChangeModel.instance.isDance = false;
        ChangeModel.instance.isLightOn = false;
        ChangeModel.instance.CancelInvoke("SwitchLight");
        ChangeModel.instance.lights[ChangeModel.instance.currentLightIndex].gameObject.SetActive(false);
    }
}
