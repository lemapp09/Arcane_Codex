using Settings_Menu;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;
    private VisualElement root;
    private SettingsMenu settingsMenu;
    private VisualElement settingsPanel;
    private Button settingsMenuCloseButton;
    private Slider masterVolumeSlider;
    private Slider backgroundVolumeSlider;
    private Slider sfxVolumeSlider;
    
    private Label masterVolumeLabel;
    private Label backgroundVolumeLabel;
    private Label sfxVolumeLabel;
    
    private bool toggleSettingsMenu = false;
    
    private PlayerInputs.PlayerActions _playerActions;
    
    [SerializeField]
    private AudioMixer audioMixer;
    

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;
        settingsMenu = new SettingsMenu();
        
        _playerActions = new PlayerInputs().Player;
        _playerActions.Enable();
        _playerActions.Settings.performed += SettingsMenuPerformed;
        
        // audioMixer = Resources.Load<AudioMixer>("Audio/BattleGround");
    }

    private void SettingsMenuPerformed(InputAction.CallbackContext obj)
    {
        if (toggleSettingsMenu)
        {
            settingsMenuCloseButton.UnregisterCallback<ClickEvent>(OnSettingsMenuCloseButtonClicked);
            root.Remove(settingsMenu);
            toggleSettingsMenu = false;
            GameManager.Instance.ResumeGame();
        }
        else
        {
            GameManager.Instance.PauseGame();
            root.Add(settingsMenu);
            settingsPanel = root.Q<VisualElement>("settingsMenuPanel");
        
            // Determine if the screen is in portrait or landscape mode
            settingsPanel.style.width = (Screen.height > Screen.width) ?
                new Length(100, LengthUnit.Percent) : new Length(50, LengthUnit.Percent);
            
            // locate Close button and make ir responsive
            settingsMenuCloseButton = root.Q<Button>("settingsMenuCloseButton");
            settingsMenuCloseButton.RegisterCallback<ClickEvent>(OnSettingsMenuCloseButtonClicked);
            toggleSettingsMenu = true;
            
            // Populate sliders with current values and register callbacks
            masterVolumeSlider = root.Q<Slider>("masterVolumeSlider");
            masterVolumeSlider.value = AudioManager.Instance.GetCurrentVolume();
            masterVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnMasterVolumeSliderChanged);
            backgroundVolumeSlider = root.Q<Slider>("backgroundVolumeSlider");
            backgroundVolumeSlider.value = AudioManager.Instance.GetBkgdVolume();
            backgroundVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnBackgroundVolumeSliderChanged);
            sfxVolumeSlider = root.Q<Slider>("sfxVolumeSlider");
            sfxVolumeSlider.value = AudioManager.Instance.GetSFXVolume();
            sfxVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnSFXVolumeSliderChanged);
            
            // Populate Volume Indicators with current values
            masterVolumeLabel = root.Q<Label>("mvSubPanelCenter");
            masterVolumeLabel.text = AudioManager.Instance.GetCurrentVolume().ToString("N1");
            backgroundVolumeLabel = root.Q<Label>("bvSubPanelCenter");
            backgroundVolumeLabel.text = AudioManager.Instance.GetBkgdVolume().ToString("N1");
            sfxVolumeLabel = root.Q<Label>("svSubPanelCenter");
            sfxVolumeLabel.text = AudioManager.Instance.GetSFXVolume().ToString("N1");
        }
    }

    private void OnMasterVolumeSliderChanged(ChangeEvent<float> evt)
    {
        AudioManager.Instance.SetVolume(evt.newValue);
        masterVolumeLabel.text = AudioManager.Instance.GetCurrentVolume().ToString("N1");
    }

    private void OnBackgroundVolumeSliderChanged(ChangeEvent<float> evt)
    {
        AudioManager.Instance.SetBkgdVolume(evt.newValue);
        backgroundVolumeLabel.text = AudioManager.Instance.GetBkgdVolume().ToString("N1");
    }

    private void OnSFXVolumeSliderChanged(ChangeEvent<float> evt)
    {
        AudioManager.Instance.SetSFXVolume(evt.newValue);
        sfxVolumeLabel.text = AudioManager.Instance.GetSFXVolume().ToString("N1");
    }

    private void OnSettingsMenuCloseButtonClicked(ClickEvent evt)
    {
        settingsMenuCloseButton.UnregisterCallback<ClickEvent>(OnSettingsMenuCloseButtonClicked);
        root.Remove(settingsMenu);
        toggleSettingsMenu = false;
        AudioManager.Instance.SetVolume(masterVolumeSlider.value); 
        AudioManager.Instance.SetBkgdVolume(backgroundVolumeSlider.value); 
        AudioManager.Instance.SetSFXVolume(sfxVolumeSlider.value); 
    }
    
    
}
