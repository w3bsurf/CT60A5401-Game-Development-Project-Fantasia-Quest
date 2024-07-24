using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for treasure chests
public class TreasureChest : MonoBehaviour, IInteractable
{
    [SerializeField] private Equipment equipment;
    [SerializeField] private ScriptableObject interactionSuccess;
    [SerializeField] private ScriptableObject interactionFail;
    [SerializeField] private AudioSource itemGet;
    [SerializeField] private int chestID;

    public Vector2Int CurrentCell => Game.Manager.Map.Grid.GetCell2D(this.gameObject);

    public ScriptableObject Interaction => interactionSuccess;
    public ScriptableObject InteractionFail => interactionFail;

    private void Start()
    {
        Equipment treasure = Instantiate(equipment);

        Vector2Int currentCell = Game.Manager.Map.Grid.GetCell2D(this.gameObject);
        transform.position = Game.Manager.Map.Grid.GetCellCenter2D(currentCell);
        Game.Manager.Map.OccupiedCells.Add(currentCell, this);
    }

    // Function for interacting with a treasure chest. Check interaction type and runs approriate interaction.
    public void Interact()
    {
        if (!Game.Manager.OpenedTreasureChestIDs.Contains(chestID) && interactionSuccess is Dialogue dialogue)
        {
            Game.Manager.StartDialogue(dialogue);
            Game.Manager.Party.Inventory.AddEquipment(equipment);
            itemGet.Play();
            Game.Manager.OpenedTreasureChestIDs.Add(chestID);
        }
        else if (Game.Manager.OpenedTreasureChestIDs.Contains(chestID) && interactionFail is Dialogue dialogueFail)
        {
            Game.Manager.StartDialogue(dialogueFail);
        }
    }
}
