using UnityEngine.UIElements;

namespace Settings_Menu
{
    public class SettingsMenu : VisualElement
    {


        public SettingsMenu()
        {
            styleSheets.Add(UnityEngine.Resources.Load<StyleSheet>("USS/SettingsMenu"));
            this.Name("settingsMenuContainer");
            this.AddToClassList("settingsMenuContainer");
            this.Unclickable(); //  This is Unclickable

            var smPanel = this.CreateChild("settingsMenuContainer", "settingsMenuPanel")
                .Name("settingsMenuPanel").Unclickable();
            
            var topPanel = smPanel.CreateChild("settingsMenuPanel", "topPanel")
                .Name("topPanel").Unclickable();
            
            topPanel.CreateChild<Label>("topPanel", "topLabel") 
                .Name("topLabel").Unclickable().text = "Settings Menu";

            topPanel.CreateChild<Button>("settingsMenuPanel", "settingsMenuCloseButton")
                .Name("settingsMenuCloseButton").Clickable().text = "Close";

            var vPanel = smPanel.CreateChild("settingsMenuPanel", "volumePanel")
                .Name("volumePanel").Unclickable();

            var mvPanel = vPanel.CreateChild("volumePanel", "masterVolumePanel")
                .Name("masterVolumePanel").Unclickable();
            
            var mvSlider =mvPanel.CreateChild<Slider>("masterVolumeSlider", "masterVolumeSlider")
                .Name("masterVolumeSlider").Clickable();
            mvSlider.lowValue = 0;
            mvSlider.highValue = 10f;
            
            var mvSubPanel = mvPanel.CreateChild("masterVolumePanel", "mvSubPanel")
                .Name("mvSubPanel").Unclickable();
            
            var mvSubPanelLeft = mvSubPanel.CreateChild<Label>("mvSubPanel", "mvSubPanelLeft")
                .Name("mvSubPanelLeft").Unclickable();
            mvSubPanelLeft.text = "-";
            
            var mvSubPanelCenter = mvSubPanel.CreateChild<Label>("mvSubPanel", "mvSubPanelCenter")
                .Name("mvSubPanelCenter").Unclickable();
            mvSubPanelCenter.text = "";
            
            var mvSubPanelRight = mvSubPanel.CreateChild<Label>("mvSubPanel", "mvSubPanelRight")
                .Name("mvSubPanelRight").Unclickable();
            mvSubPanelRight.text = "+";

            var mvLabel = mvPanel.CreateChild<Label>("masterVolumeLabel", "masterVolumeLabel")
                .Name("masterVolumeLabel").Unclickable();
            mvLabel.text = "Master\nVolume";

            var bvPanel = vPanel.CreateChild("volumePanel", "backgroundVolumePanel")
                .Name("backgroundVolumePanel").Unclickable();
            
            var bvSlider = bvPanel.CreateChild<Slider>("backgroundVolumeSlider", "backgroundVolumeSlider")
                .Name("backgroundVolumeSlider").Clickable();
            bvSlider.lowValue = 0;
            bvSlider.highValue = 10f;
            
            var bvSubPanel = bvPanel.CreateChild("backgroundVolumePanel", "bvSubPanel")
                .Name("bvSubPanel").Unclickable();
            
            var bvSubPanelLeft = bvSubPanel.CreateChild<Label>("bvSubPanel", "bvSubPanelLeft")
                .Name("bvSubPanelLeft").Unclickable();
            bvSubPanelLeft.text = "-";
            
            var bvSubPanelCenter = bvSubPanel.CreateChild<Label>("bvSubPanel", "bvSubPanelCenter")
                .Name("bvSubPanelCenter").Unclickable();
            bvSubPanelCenter.text = "";
            
            var bvSubPanelRight = bvSubPanel.CreateChild<Label>("bvSubPanel", "bvSubPanelRight")
                .Name("bvSubPanelRight").Unclickable();
            bvSubPanelRight.text = "+";
            
            var bvLabel = bvPanel.CreateChild<Label>("backgroundVolumeLabel", "backgroundVolumeLabel")
                .Name("backgroundVolumeLabel").Unclickable();
            bvLabel.text = "Background\nVolume";

            var svPanel = vPanel.CreateChild("volumePanel", "sfxVolumePanel")
                .Name("sfxVolumePanel").Unclickable();
            
            var svSlider = svPanel.CreateChild<Slider>("sfxVolumeSlider", "sfxVolumeSlider")
                .Name("sfxVolumeSlider").Clickable();
            svSlider.lowValue = 0;
            svSlider.highValue = 10f;
            
            var svSubPanel = svPanel.CreateChild("sfxVolumePanel", "svSubPanel")
                .Name("svSubPanel").Unclickable();
            
            var svSubPanelLeft = svSubPanel.CreateChild<Label>("svSubPanel", "svSubPanelLeft")
                .Name("svSubPanelLeft").Unclickable();
            svSubPanelLeft.text = "-";
            
            var svSubPanelCenter = svSubPanel.CreateChild<Label>("svSubPanel", "svSubPanelCenter")
                .Name("svSubPanelCenter").Unclickable();
            svSubPanelCenter.text = "";
            
            var svSubPanelRight = svSubPanel.CreateChild<Label>("svSubPanel", "svSubPanelRight")
                .Name("svSubPanelRight").Unclickable();
            svSubPanelRight.text = "+";
            
            var svLabel = svPanel.CreateChild<Label>("sfxVolumeLabel", "sfxVolumeLabel")
                .Name("sfxVolumeLabel").Unclickable();
            svLabel.text = "SFX\nVolume";

        }
    }
}