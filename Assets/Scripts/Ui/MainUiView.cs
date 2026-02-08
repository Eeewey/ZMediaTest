using UnityEngine;
using UnityEngine.UI;

public class MainUiView : MonoBehaviour
{
    [SerializeField] private Button rebuildButton;
    [SerializeField] private Button startMoveButton;

    private MainUiViewModel viewModel;

    private void Start()
    {
        viewModel = new MainUiViewModel();

        viewModel.DestroyAllUnits();

        rebuildButton.onClick.RemoveAllListeners();
        rebuildButton.onClick.AddListener(MainMenuClick);

        startMoveButton.onClick.RemoveAllListeners();
        startMoveButton.onClick.AddListener(StartClick);

        WinConditionSystem.OnGameOver += MainMenuClick;
    }

    private void MainMenuClick()
    {
        viewModel.DestroyAllUnits();

        startMoveButton.gameObject.SetActive(true);
    }

    private void StartClick()
    {
        viewModel.RebuildFormation();

        startMoveButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        rebuildButton.onClick.RemoveAllListeners();
        startMoveButton.onClick.RemoveAllListeners();

        WinConditionSystem.OnGameOver -= MainMenuClick;
    }
}
