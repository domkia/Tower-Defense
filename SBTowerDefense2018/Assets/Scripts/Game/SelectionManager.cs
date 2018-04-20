using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public string selectedLayer = "Outline";
    private ISelectable selected;
    private SelectionPostEffect selectionEffect;

    private void Start()
    {
        selectionEffect = Camera.main.GetComponent<SelectionPostEffect>();
        selectionEffect.enabled = false;
    }

    private void Select(ISelectable selected)
    {
        if(this.selected != selected)
            Deselect();
        //TODO: move this nonsense ---------------------
        if (selected is ResourceInteractable)
        {
            ResourceInteractable resource = selected as ResourceInteractable;
            resource.OnCancelled += DeselectResource;
            resource.OnCompleted += DeselectResource;
        }
        else if (selected is TowerInteractable)
        {
            TowerInteractable tower = selected as TowerInteractable;
            HexGrid.Instance.displayHexTiles.ShowTiles(tower.GetComponent<Tower>().TilesInRange);
        }
        else if (selected is Enemy)
            (selected as Enemy).OnDeath += DeselectEnemy;
        //-------------------------------

        this.selected = selected;
        selectionEffect.SetColor(selected.SelectionColor);
        SetChildrenLayer(selectedLayer);
        selectionEffect.enabled = true;
    }

    private void DeselectEnemy(Enemy enemy)
    {
        enemy.OnDeath -= DeselectEnemy;
        Deselect();
    }

    private void DeselectResource(IInteractable interactable)
    {
        interactable.OnCancelled -= DeselectResource;
        interactable.OnCompleted -= DeselectResource;
        Deselect();
    }

    private void Deselect()
    {
        if (selected == null)
            return;
        //if(!(selected is TowerInteractable))
        HexGrid.Instance.displayHexTiles.HideTiles();
        SetChildrenLayer("Default");
        selected = null;
    }

    private void SetChildrenLayer(string layer)
    {
        // Bug (at the end of the game?)
        GameObject go = (selected as MonoBehaviour).gameObject;

        foreach(MeshRenderer mr in go.GetComponentsInChildren<MeshRenderer>())
            if(mr.gameObject != go)
                mr.gameObject.layer = LayerMask.NameToLayer(layer);
    }

    private void Update()
    {
        if (selected == null)
            if(selectionEffect.enabled == true)
                selectionEffect.enabled = false;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                ISelectable s = hit.collider.GetComponent<ISelectable>();
                if (s != null)
                    Select(s);
                else
                    Deselect();
            }
        }
    }
}
