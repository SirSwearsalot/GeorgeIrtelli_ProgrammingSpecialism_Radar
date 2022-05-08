using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarManager : MonoBehaviour
{

    [SerializeField] Transform Player;

    [Space]
    Camera RadarCam;
    [SerializeField] float RadarResolution;
    GameObject[] RadarIcons;
    [SerializeField] GameObject TemplateBlip;

    RadarContact[] Contacts;

    // Start is called before the first frame update
    void Start()
    {
        RadarCam = GetComponent<Camera>();

        CreateIcons();
    }

    void CreateIcons()
    {
        Contacts = FindObjectsOfType(typeof(RadarContact)) as RadarContact[];

        RadarIcons = new GameObject[Contacts.Length];

        for (int i = 0; i < Contacts.Length; i++)
            RadarIcons[i] = Instantiate(TemplateBlip, transform.position, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        RadarCam.orthographicSize = RadarResolution;


        for (int i = 0; i < Contacts.Length; i++)
        {

            if (Contacts[i] != null)
            {
                RadarIcons[i].GetComponent<Image>().sprite = Contacts[i].RadarSprite;

                RadarIcons[i].transform.position = Contacts[i].transform.position;
                RadarIcons[i].transform.eulerAngles = new Vector3(90, 0, -Contacts[i].transform.eulerAngles.y);
                RadarIcons[i].transform.localScale = Vector3.one * 0.01f * Contacts[i].IconSize;

                if (Vector3.Distance(RadarIcons[i].transform.position, Player.position) > RadarResolution * 2)
                {
                    RadarIcons[i].GetComponent<Image>().color = new Color(Contacts[i].SpriteColor.r,
                                                                          Contacts[i].SpriteColor.g,
                                                                          Contacts[i].SpriteColor.b,
                                                                          0);
                }
                else
                    RadarIcons[i].GetComponent<Image>().color = new Color(Contacts[i].SpriteColor.r,
                                                                          Contacts[i].SpriteColor.g,
                                                                          Contacts[i].SpriteColor.b,
                                                                          1);

                if (Vector3.Distance(RadarIcons[i].transform.position, Player.position) > RadarResolution * 0.85f)
                {
                    Vector3 dir = (RadarIcons[i].transform.position - Player.position).normalized;
                    RadarIcons[i].transform.position = Player.position + dir * (RadarResolution * 0.85f);
                }
            }
            else if (RadarIcons[i] != null)
                Destroy(RadarIcons[i]);
        }
    }
}
