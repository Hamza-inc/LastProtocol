using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class itemEqupe : MonoBehaviour
{
    public inventorrySystem system;
    [SerializeField] private TMP_Text BilgiText;
    void Update()
    {
        BilgiText.text = "";
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f))
        {
            if (hit.collider.gameObject.tag == "Item")
            {
                BilgiText.text = "E equpe";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    itemProp prp = hit.collider.gameObject.GetComponent<itemProp>();
                    system.changeInventory(prp, hit.collider.gameObject);
                }
            }
            else if (hit.collider.gameObject.tag == "sellZone")
            {
                
                if (system.bb00SlotIsFull)
                {
                    BilgiText.text = "E to sell";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        system.deleteItem();
                    }
                }

            }
        }
    }
}
