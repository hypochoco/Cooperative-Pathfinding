using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RSelectable
public class RSelectable : MonoBehaviour {
    
    #region RSelectable Variables

    // Selectable References
    [SerializeField] private Transform _t;
    [SerializeField] private GameObject _selectIconPrefab;

    // Selectable Variables
    private bool _selected;
    private GameObject _selectIcon;

    #endregion

    #region Unity Functions

    // Initialize select 
    private void Start() {

        // Create select icon
        _selectIcon = Instantiate(_selectIconPrefab, 
            _t.position + Vector3.up * 0.25f, Quaternion.identity);

        // Parent select icon
        _selectIcon.transform.parent = _t;

        // Hide select icon
        _selectIcon.SetActive(false);

    }

    #endregion

    #region RSelectable Functions

    // Toogle the selection
    public void ToggleSelect() {
        if (_selected) {
            Deselect();
        } else {
            Select();
        }
    }

    // Deselection indication
    public void Deselect() {

        // Set _selected
        _selected = false;

        // Hide selection icon
        _selectIcon.SetActive(false);

    }

    // Selection indication
    public void Select() {

        // Set _selected
        _selected = true;

        // Show select icon
        _selectIcon.SetActive(true);

    }

    #endregion

}
