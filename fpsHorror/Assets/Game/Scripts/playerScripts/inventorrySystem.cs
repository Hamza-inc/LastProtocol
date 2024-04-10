using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventorrySystem : MonoBehaviour
{
    [SerializeField] private Image[] slotImage;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject selectLayer;
    public int slt = 1;
    public bool bb00SlotIsFull;


    private Sprite defaultSlot;
    private GameObject _player;
    private GameObject alinanObje = null;
    private List<SlotProps> Slots = new List<SlotProps>();
    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Slots.Add(new SlotProps(i));
        }

        defaultSlot = slotImage[slt].sprite;

        selectLayer.transform.parent = slotImage[slt].transform.parent;
        selectLayer.GetComponent<Image>().rectTransform.position = slotImage[slt].rectTransform.position;


        _player = GameObject.FindGameObjectWithTag("Player");
    
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            slt++;
            if (slt > Slots.Count-1)
            {
                slt = 0;
            }

            if (alinanObje != null)
            {
                GameObject sil = alinanObje;
                alinanObje = null;
                sil.SetActive(false);
                sil = null;
            }

            selectLayer.transform.parent = slotImage[slt].transform.parent;
            selectLayer.GetComponent<Image>().rectTransform.position = slotImage[slt].rectTransform.position;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            slt--;
            if (slt < 0)
            {
                slt = Slots.Count-1;
            }

            if (alinanObje != null)
            {
                GameObject sil = alinanObje;
                alinanObje = null;
                sil.SetActive(false);
                sil = null;
            }

            selectLayer.transform.parent = slotImage[slt].transform.parent;
            selectLayer.GetComponent<Image>().rectTransform.position = slotImage[slt].rectTransform.position;
        }


        
        
        
        if (Slots[slt].isFull)
        {
            bb00SlotIsFull = true;
            if (alinanObje == null)
            {
                alinanObje = Slots[slt].Item;
            }
            else
            {
                alinanObje.SetActive(true);
            }



            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameObject gm = alinanObje;
                alinanObje = null;
                gm.SetActive(true);
                gm.transform.parent = null;
                gm.transform.rotation = _player.transform.rotation;
                gm.GetComponent<ýtemGravity>().enabled = true;
                gm.GetComponent<ýtemGravity>().isGround = false;
                gm.GetComponent<ýtemGravity>().hitted = false;
                Slots[slt].deleteVaraible();
                changeSlots();
            }
        }
        else
        {
            bb00SlotIsFull = false;
            if (alinanObje != null)
            {
                GameObject sil = alinanObje;
                alinanObje = null;
                sil.SetActive(false);
                sil = null;
            }
            
        }
    }

    public void changeInventory(itemProp itmprp, GameObject itm)
    {
        for (int i = 0; i < slotImage.Length; i++)
        {
            if (!Slots[i].isFull)
            {
                Slots[i].changeVaraible(itmprp.ItemName, itmprp.ItemExplanation, itmprp.ItemPicture, itmprp.ItemTotal, itm);
                itm.GetComponent<ýtemGravity>().enabled = false;
                itm.transform.parent = hand.transform;
                itm.transform.position = hand.transform.position;
                itm.transform.rotation = hand.transform.rotation;
                itm.SetActive(false);
                break;
            }
        }

        changeSlots();
    }



    public void deleteItem()
    {
        if (alinanObje != null)
        {
            GameObject gm = alinanObje;
            alinanObje = null;
            gm.SetActive(true);
            gm.transform.parent = null;
            gm.transform.rotation = _player.transform.rotation;
            gm.GetComponent<ýtemGravity>().enabled = true;
            gm.GetComponent<ýtemGravity>().isGround = false;
            gm.GetComponent<ýtemGravity>().hitted = false;
            Slots[slt].deleteVaraible();
            changeSlots();
        }
        
    }




    private void changeSlots()
    {
        for (int i = 0; i < slotImage.Length; i++)
        {
            if (Slots[i].isFull == true)
            {
                slotImage[i].sprite = Slots[i].ItemPicture;
            }
            else
            {
                slotImage[i].sprite = defaultSlot;
            }
        }
    }










    // Sýnýf



    class SlotProps
    {
        public int slotID { get;}
        public bool isFull { get; set; }
        public string ItemName { get; set; }
        public string ItemExplanation { get; set; }
        public Sprite ItemPicture { get; set; }
        public float ItemTotal { get; set; }
        public GameObject Item { get; set; }

        public SlotProps(int xxid)
        {
            slotID = xxid;
            isFull = false;
            ItemName = string.Empty;
            ItemExplanation = string.Empty;
            ItemPicture = null;
            ItemTotal = 0;
            Item = null;
        }

        public void changeVaraible(string xxName, string xxExpl, Sprite xxSptire, float xxTotal, GameObject xxItem)
        {
            isFull = true;
            ItemName = xxName;
            ItemExplanation = xxExpl;
            ItemPicture = xxSptire;
            ItemTotal = xxTotal;
            Item = xxItem;
        }

        public void deleteVaraible()
        {
            isFull = false;
            ItemName = string.Empty;
            ItemExplanation = string.Empty;
            ItemPicture = null;
            ItemTotal = 0;
            Item = null;
        }
    }
}
