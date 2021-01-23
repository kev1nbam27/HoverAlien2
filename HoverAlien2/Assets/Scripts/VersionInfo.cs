using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VersionInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerSettings.bundleVersion.ToString()+ " (" +PlayerSettings.iOS.buildNumber.ToString()+ ")";
    }
}
