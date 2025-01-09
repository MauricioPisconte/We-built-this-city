using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PeopleCounterManager : MonoBehaviour
{
    public static PeopleCounterManager Instance;
    [SerializeField] private int peopleCount;
    [SerializeField] private GameObject[] numeros;
    [SerializeField] private float digitSpacing = 1f;
    [SerializeField] private Transform digitsParent;

    private List<GameObject> activeDigits = new List<GameObject>();

    [Header("Visual Effects")]
    [SerializeField] private GameObject personFBX;
    [SerializeField] private Image panel;

    [SerializeField] private float pushDuration = 0.5f;
    [SerializeField] private float explotionDashTime = 0.2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetQuanity();
    }

    public void CountingPeople(int pc)
    {
        peopleCount += pc;
        peopleCount = Mathf.Clamp(peopleCount, 0, 10000);
        SetQuanity();
    }

    private void SetQuanity()
    {
        foreach (Transform digits in digitsParent)
        {
            Destroy(digits.gameObject);
        }

        string peopleCountStr = peopleCount.ToString();

        for (int i = 0; i < peopleCountStr.Length; i++)
        {
            int digit = int.Parse(peopleCountStr[i].ToString());
            GameObject digitInstance = Instantiate(numeros[digit], digitsParent.position, digitsParent.rotation, digitsParent);

            Vector3 position = new Vector3(0, 0, i * digitSpacing);
            digitInstance.transform.localPosition = position;
        }

        MakePushPerson();
    }

    public void MakePushPerson()
    {
        StartCoroutine(PushNPress());
    }

    private IEnumerator PushNPress()
    {
        Vector3 originalScale = personFBX.transform.localScale;
        Vector3 compressedScale = new Vector3(originalScale.x, originalScale.y * 0.9f, originalScale.z);

        float elapsedTime = 0f;

        while (elapsedTime < pushDuration / 2)
        {
            personFBX.transform.localScale = Vector3.Lerp(originalScale, compressedScale, elapsedTime / (pushDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        personFBX.transform.localScale = compressedScale;
        elapsedTime = 0f;

        while (elapsedTime < pushDuration / 2)
        {
            personFBX.transform.localScale = Vector3.Lerp(compressedScale, originalScale, elapsedTime / (pushDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        personFBX.transform.localScale = originalScale;
    }

    public void AlterZoom()
    {
        StartCoroutine(ZoomCoroutine());
    }

    private IEnumerator ZoomCoroutine()
    {
        Color originalColor = panel.color;

        originalColor.a = 0f;
        panel.color = originalColor;

        float elapsedTime = 0f;

        while (elapsedTime < explotionDashTime / 2)
        {
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / (explotionDashTime / 2));
            panel.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        elapsedTime = 0f;

        while (elapsedTime < explotionDashTime / 2)
        {
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / (explotionDashTime / 2));
            panel.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
