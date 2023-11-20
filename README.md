# Kadda OK Tools

A desktop application to speed up, streamline, and simplify the process of creating custom karaoke videos.

 - **_Automatically_** create karaoke word timing from a vocal-isolated audio file and the text of the lyrics, using the magic of free cloud-powered speech recognition.
    
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/e2580711-a1f0-4bed-ab7f-6c50bbfa17cb" width="170"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/3f1aeb61-17e6-46a0-9b79-251bd83c21ff" width="170"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/8b1e0c15-c267-43a3-b8b6-a25d7f8ebfdf" width="170"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/8ab3548d-ac7e-47c6-b833-4c17c5a79904" width="170"/>



 - Add, edit, and delete lines and syllables with easy-to-use controls, or even import a YouTube Movie Maker or Karaoke Builder Studio file to take advantage of time- or tedium-saving editing features not available in those apps.

    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/2df82625-4008-435c-9530-2c660eddf9a0" width="170"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/9546f8e9-3261-41a3-8b50-14ec31975f8e" width="170"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/32004c0d-c9da-404f-88ca-ae403cd9511f" width="170"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/5a7213e1-0508-4f2e-a553-60af9767e861" width="170"/>



 - Export to YTMM or KBS to generate the final product, accelerating your karaoke creation workflow with templates and project generation features like automatic progress bars.

    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/7dc0a69c-8961-496d-bd70-e3d2ae2d79fa" width="170"/> ➤➤➤
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/65ade6d6-af49-4fda-af8c-201dd410266d" width="170"/>
    
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/9cf54209-0a2f-4264-8ba9-21cd30c41f79" width="170"/> ➤➤➤
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/ac56d509-3547-4627-8046-5da9675384c0" width="170"/>

    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/6d912e1a-1c73-43fa-9ea2-42de87ce37c0" width="170"/> ➤➤➤
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/1d42f310-81f0-4de7-9bcc-aca4a7b690b3" width="170"/>


    

## Get Started

Kadda OK Tools currently targets YTMM and KBS, which are both Windows-only apps, and as such is only available for win-x64.  (Support for Mac OSX, and import/export to MidiCo and/or Kanto Syncro, are possible future features.)

Please find the latest win-x64 release [in the releases section](https://github.com/KaddaOK/KaddaOKTools/releases).  No installation should be required, just unzip and run.

## Known Issues:

 - The caret and word highlighting in the Edit Line Timing Dialog is sometimes not in proper visual sync when playing back.  This doesn't seem to affect recording, only playback.  Your mileage may vary.
   
 - When importing from then re-exporting to KBS, the following information will currently be **_lost_**:
   
   - Page display and removal (Line by Line vs Clear Screen vs Fade vs etc.)
   - Style settings (shadow, bold, etc.)
   - Palette indexes other than the default (0 is screen and border, 1-4 for style 01, 5-8 for style 02, 9-12 for style 03, 4/8/12/14 for style 04)
   - If you split any lines, the page breaks will be reset to ignorant 4-line pages, and you will need to hit the "Reset" button to correct line and page display/removal times.
     
 - Output colors selected in the export tab are not applied if you start with an import from YTMM.  This does not affect when you start from scratch nor when you import from KBS.
   
 - If you use the Clear All button and then import, the waveform phrase map will not be drawn correctly when the app first jumps to the Edit tab.  Resizing the window will correct it (or just restart the app when you want to work on another song).
   
 - When you import, only one audio file is specified, so the app will try to guess which one it is and find others in the same folder, without telling you it's doing that or asking you if the ones it found were the right ones. Then it whisks you off to the Edit tab immediately and you'd need to go back to the Audio tab to change them.

 
## Some Major Roadmap Items (not necessarily in any order):
 - Design/UX cleanup
 - Cleaning up and opening source code
 - Narrowing and Edit tabs:
   - Allow toggle of which audio track to show in waveform map
   - Hover highlighting and/or click-to-scroll connection between waveform map and phrase content
 - Edit Line Timing Dialog:
   - Option to Save and Go to Next, rather than closing it and reopening it for each line
   - Split, edit, and delete syllables in here, rather than having to close it to make text changes
   - Speed control for recording and playback 
 - Export Tab:
   - Font selection 
   - Ability to check/reselect audio files
 - Misc.:
     - Audition the phrase timespan on the Narrow tab
     - Option to transform text to all-caps
     - Page/paragraph grouping and options
     - Full manual timing process from scratch (Edit Line Timing Dialog continuously for the whole song)
     - Mac OSX, MidiCo and Kanto Syncro support?

## Need Help?

If you discover a bug (don't be shy, there are many right now I'm sure!), and it's not mentioned in the above couple sections, please [create a bug report](https://github.com/KaddaOK/KaddaOKTools/issues).

Otherwise, for questions or comments, give me a shout in the (#kadda-ok)(https://discord.com/channels/918644502128885760/1055115584007835668) channel on the diveBar discord.
  
## Contribute

Once the code has stabilized and gone through a cleanup pass (think, like, tidying up your house because guests are coming over), the source will be open in this repository, and pull requests will be welcome.  Thank you for your patience!
