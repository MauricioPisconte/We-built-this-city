using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrabableBuilding : MonoBehaviour
{
    private Rigidbody rb;
    private Grabbable grabbable;
    private HandGrabInteractable handGrabInteractable;
    private GrabInteractable grabInteractable;

    [Header("Raycast Settings")]
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private Color rayColor = Color.green;
    [SerializeField] private string populationTerrainName = "BED_EffectMesh";
    //private TextMeshProUGUI texto;

    [Header("Building properties")]
    [SerializeField] private string buildDestroyCollisionTag;
    [SerializeField] private int peopleCharge;
    [SerializeField] private GameObject grabable;

    [Header("Raycast Timer")]
    [SerializeField] private float hitTimeThreshold = 3f;
    private float timeOnHit = 0f;
    public bool hasActionBeenPerformed = false;

    [Header("Raycast Offset")]
    [SerializeField] private float yOffset = 0f;

    [Header("Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject explosionParticle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = transform.GetChild(0).GetComponent<Grabbable>();
        handGrabInteractable = transform.GetChild(0).GetComponent<HandGrabInteractable>();
        grabInteractable = transform.GetChild(0).GetComponent<GrabInteractable>();
        grabable = transform.GetChild(0).gameObject;
        hasActionBeenPerformed = false;
        audioSource = GetComponent<AudioSource>();
        //texto = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        grabbable.InjectOptionalRigidbody(rb);
        grabbable.InjectOptionalTargetTransform(this.transform);
        handGrabInteractable.InjectRigidbody(rb);
        grabInteractable.InjectRigidbody(rb);
    }

    private void Update()
    {
        if (!hasActionBeenPerformed)
        {
            Ray ray = new Ray(transform.position + new Vector3(0, yOffset, 0), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                if (hit.collider.name == populationTerrainName)
                {
                    timeOnHit += Time.deltaTime;

                    if (timeOnHit >= hitTimeThreshold)
                    {
                        PeopleCounterManager.Instance.CountingPeople(peopleCharge);
                        hasActionBeenPerformed = true;
                        audioSource.Play();
                        grabable.SetActive(false);
                    }
                }
                else
                {
                    timeOnHit = 0f;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        Gizmos.DrawLine(transform.position + new Vector3(0, yOffset, 0), transform.position + new Vector3(0, yOffset, 0) + Vector3.down * rayLength);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag(buildDestroyCollisionTag))
        {
            GrabableBuilding go = col.gameObject.GetComponent<GrabableBuilding>();
            int deletePeople = go.hasActionBeenPerformed ? go.ReturnPeopleCharge() : 0;
            Instantiate(explosionParticle, go.transform.position, Quaternion.identity);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            PeopleCounterManager.Instance.CountingPeople(-deletePeople);
            PeopleCounterManager.Instance.AlterZoom();

            Destroy(go.gameObject);
            Destroy(gameObject);
        }
    }

    public int ReturnPeopleCharge()
    {
        return peopleCharge;
    }
}
