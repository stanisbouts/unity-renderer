using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IDropdownComponentView
{
    /// <summary>
    /// Event that will be triggered when the selection of any option changes.
    /// </summary>
    event Action<bool, string> OnOptionSelectionChanged;

    /// <summary>
    /// Set the dropdown as multiselect or not.
    /// </summary>
    bool isMultiselect { get; set; }

    /// <summary>
    /// Open the options list.
    /// </summary>
    void Open();

    /// <summary>
    /// Closes the options list
    /// </summary>
    void Close();

    /// <summary>
    /// Set the dropdown title.
    /// </summary>
    /// <param name="newText">New title.</param>
    void SetTitle(string newText);

    /// <summary>
    /// Set the available options of the dropdown.
    /// </summary>
    /// <param name="options">List of options..</param>
    void SetOptions(List<ToggleComponentModel> options);

    /// <summary>
    /// Get an option of the dropdown.
    /// </summary>
    /// <param name="index">Index of the list of options.</param>
    /// <returns>A specific option toggle.</returns>
    IToggleComponentView GetOption(int index);

    /// <summary>
    /// Get all the options of the dropdown.
    /// </summary>
    /// <returns>The list of options.</returns>
    List<IToggleComponentView> GetAllOptions();

    /// <summary>
    /// Filter options using a text.
    /// </summary>
    /// <param name="filterText">Text used to filter.</param>
    void FilterOptions(string filterText);

    /// <summary>
    /// Select/unselect all the available options (if multiselect is activated).
    /// </summary>
    void SetSelectAll(bool isSelected);

    /// <summary>
    /// Set the search bar place holder text.
    /// </summary>
    /// <param name="newText">New text.</param>
    void SetSearchPlaceHolderText(string newText);

    /// <summary>
    /// Show/Hide the loading panel.
    /// </summary>
    /// <param name="isActive">Tru for showing it.</param>
    void SetLoadingActive(bool isActive);

    /// <summary>
    /// Show/Hide the "Select All" option (only for multiselect configuration).
    /// </summary>
    /// <param name="isActive"></param>
    void SetSelectAllOptionActive(bool isActive);
}
public class DropdownComponentView : BaseComponentView, IDropdownComponentView, IComponentModelConfig
{
    internal const string SELECT_ALL_OPTION_ID = "select_all";
    internal const string SELECT_ALL_OPTION_TEXT = "Select All";

    [Header("Prefab References")]
    [SerializeField] internal Button button;
    [SerializeField] internal TMP_Text title;
    [SerializeField] internal SearchBarComponentView searchBar;
    [SerializeField] internal GameObject optionsPanel;
    [SerializeField] internal GameObject loadingPanel;
    [SerializeField] internal GridContainerComponentView availableOptions;
    [SerializeField] internal UIHelper_ClickBlocker blocker;

    [Header("Resources")]
    [SerializeField] internal ToggleComponentView togglePrefab;

    [Header("Configuration")]
    [SerializeField] internal DropdownComponentModel model;

    public event Action<bool, string> OnOptionSelectionChanged;

    internal ToggleComponentView selectAllOptionComponent;

    public bool isMultiselect 
    {
        get => model.isMultiselect;
        set => model.isMultiselect = value;
    }

    internal bool isOpen = false;
    internal List<ToggleComponentModel> originalOptions;

    public override void Awake()
    {
        base.Awake();

        RefreshControl();
        Close();

        blocker.OnClicked += Close;
        button.onClick.AddListener(() => ToggleOptionsList());
        searchBar.OnSearchText += FilterOptions;
    }

    public void Configure(BaseComponentModel newModel)
    {
        model = (DropdownComponentModel)newModel;
        RefreshControl();
    }

    public override void RefreshControl()
    {
        if (model == null)
            return;

        isMultiselect = model.isMultiselect;
        SetTitle(model.title);
        SetOptions(model.options);
        SetSearchPlaceHolderText(model.searchPlaceHolderText);
    }

    public void Open()
    {
        optionsPanel.SetActive(true);
        isOpen = true;
        blocker.Activate();
    }

    public void Close()
    {
        optionsPanel.SetActive(false);
        isOpen = false;
        searchBar.ClearSearch();
        blocker.Deactivate();
    }

    public void SetTitle(string newText)
    {
        model.title = newText;

        if (title == null)
            return;

        title.text = newText;
    }

    public void SetOptions(List<ToggleComponentModel> options)
    {
        model.options = options;
        originalOptions = options;

        RemoveAllInstantiatedOptions();

        if (options.Count > 0)
            CreateSelectAllOption();

        for (int i = 0; i < options.Count; i++)
        {
            CreateOption(options[i], $"Option_{i}");
        }

        UpdateSelectAllOptionStatus();
        SetSelectAllOptionActive(model.showSelectAllOption);
    }

    public void FilterOptions(string filterText)
    {
        if (filterText == string.Empty)
        {
            SetOptions(originalOptions);
            return;
        }

        List<ToggleComponentModel> newFilteredOptions = new List<ToggleComponentModel>();
        foreach (ToggleComponentModel option in originalOptions)
        {
            if (option.text.ToLower().Contains(filterText.ToLower()))
                newFilteredOptions.Add(option);
        }

        model.options = newFilteredOptions;

        RemoveAllInstantiatedOptions();

        for (int i = 0; i < newFilteredOptions.Count; i++)
        {
            CreateOption(newFilteredOptions[i], $"FilteredOption_{i}");
        }
    }

    public IToggleComponentView GetOption(int index)
    {
        if (index >= availableOptions.GetItems().Count - 1)
            return null;

        return availableOptions.GetItems()[index + 1] as IToggleComponentView;
    }

    public List<IToggleComponentView> GetAllOptions() 
    {
        return availableOptions
            .GetItems()
            .Select(x => x as IToggleComponentView)
            .Where(x => x.id != SELECT_ALL_OPTION_ID)
            .ToList();
    }

    public void SetSelectAll(bool isSelected)
    {
        List<IToggleComponentView> allOptions = GetAllOptions();
        foreach (IToggleComponentView option in allOptions)
        {
            option.isOn = isSelected;
        }

        foreach (ToggleComponentModel option in originalOptions)
        {
            option.isOn = isSelected;
        }
    }

    public void SetSearchPlaceHolderText(string newText)
    {
        model.searchPlaceHolderText = newText;

        if (searchBar == null)
            return;

        searchBar.SetPlaceHolderText(newText);
    }

    public void SetLoadingActive(bool isActive)
    {
        if (loadingPanel == null)
            return;

        loadingPanel.SetActive(isActive);
    }

    public void SetSelectAllOptionActive(bool isActive)
    {
        model.showSelectAllOption = isActive;

        if (!isMultiselect || selectAllOptionComponent == null)
            return;

        selectAllOptionComponent.gameObject.SetActive(isActive);
    }

    public override void Dispose()
    {
        base.Dispose();

        blocker.OnClicked -= Close;
        button.onClick.RemoveAllListeners();
        searchBar.OnSearchText -= FilterOptions;
    }

    internal void ToggleOptionsList()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    internal void CreateSelectAllOption()
    {
        CreateOption(
            new ToggleComponentModel
            {
                id = SELECT_ALL_OPTION_ID,
                text = SELECT_ALL_OPTION_TEXT,
                isOn = false,
            },
            $"Option_{SELECT_ALL_OPTION_ID}");
    }

    internal void CreateOption(ToggleComponentModel newOptionModel, string name)
    {
        if (togglePrefab == null)
            return;

        ToggleComponentView newGO = Instantiate(togglePrefab);
        newGO.gameObject.SetActive(true);
        newGO.Configure(newOptionModel);
        availableOptions.AddItem(newGO);
        newGO.name = name;

        if (newOptionModel.id == SELECT_ALL_OPTION_ID)
        {
            selectAllOptionComponent = newGO;
            newGO.gameObject.SetActive(isMultiselect);
        }

        newGO.OnSelectedChanged += OnOptionSelected;
    }

    internal void OnOptionSelected(bool isOn, string optionId)
    {
        if (optionId != SELECT_ALL_OPTION_ID)
        {
            OnOptionSelectionChanged?.Invoke(isOn, optionId);

            if (isOn && !isMultiselect)
            {
                List<IToggleComponentView> allOptions = GetAllOptions();
                foreach (IToggleComponentView option in allOptions)
                {
                    if (option.id != optionId)
                        option.isOn = false;
                }
            }

            foreach (ToggleComponentModel option in originalOptions)
            {
                if (optionId == option.id)
                    option.isOn = isOn;
            }

            UpdateSelectAllOptionStatus();
        }
        else
        {
            SetSelectAll(isOn);
        }
    }

    internal void UpdateSelectAllOptionStatus()
    {
        if (!isMultiselect)
            return;

        List<IToggleComponentView> allOptions = GetAllOptions();

        if (selectAllOptionComponent != null)
        {
            selectAllOptionComponent.OnSelectedChanged -= OnOptionSelected;
            selectAllOptionComponent.isOn = allOptions.Count > 0 && allOptions.All(x => x.isOn);
            selectAllOptionComponent.OnSelectedChanged += OnOptionSelected;
        }
    }

    internal void RemoveAllInstantiatedOptions()
    {
        availableOptions.RemoveItems();
    }

    [ContextMenu("Mock Options")]
    public void MockOptions()
    {
        List<ToggleComponentModel> test = new List<ToggleComponentModel>();
        for (int i = 0; i < 20; i++)
        {
            test.Add(new ToggleComponentModel
            {
                id = i.ToString(),
                isOn = false,
                text = "Option" + i.ToString()
            });
        }

        isMultiselect = false;
        SetOptions(test);
    }

    [ContextMenu("Mock Options (Multi-selection)")]
    public void MockOptions_MultiSelection()
    {
        List<ToggleComponentModel> test = new List<ToggleComponentModel>();
        for (int i = 0; i < 20; i++)
        {
            test.Add(new ToggleComponentModel
            {
                id = i.ToString(),
                isOn = false,
                text = "Option" + i.ToString()
            });
        }

        isMultiselect = true;
        SetOptions(test);
    }
}
