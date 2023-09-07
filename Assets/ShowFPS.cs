using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowFPS : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Updates());
    }

    // Update is called once per frame
    private IEnumerator Updates()
    {
        while (true) {
            textMeshProUGUI.text = (1 / Time.deltaTime).ToString();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
