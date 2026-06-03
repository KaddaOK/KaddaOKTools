# Kadda OK Tools

A desktop application to speed up, streamline, and simplify the process of creating custom karaoke videos.

 - **Create precise per-syllable timing**, manually or automatically, using the method that works best for you:  
   
     <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/3de9b823-3abc-4e11-83c2-5d99e7189554" width="500"/>

   - **(Recommended) Tap to time manually** - tap the right-arrow key [`→`] on your keyboard as you listen to mark the start of each syllable in your prepared list of lyrics.  
   <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/81bddf39-478f-4380-9cd7-c4cee142e55d" width="600" />  
   An advantage over other karaoke timing software is that you can seek to any point in both the recording and the syllable list, so you can repeat something you're not happy with without losing anything before or after it.  
   <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/3c4b5823-4a04-412d-ab69-b8b76d4df131" width="600" />  
   You can also tap the down-arrow key [`↓`] while recording to stop the current syllable wherever you want without automatically starting the next one.  
   (The one current disadvantage over other karaoke timing software is no speed control - recording speed is 100% only. Personally, with seek forward and back and easy lossless re-do of any point, I don't find that I need to change the speed.)

   - **Import a `.ctm` file from [NeMo Forced Aligner](https://github.com/NVIDIA/NeMo/blob/main/tools/nemo_forced_aligner/README.md)** - NFA is pretty great at timing known lyrics onto vocal stems, tending to give better results than speech recognition that only takes the "known" lyrics under advisement as possible phrases.  (Works best with a clean vocal stem; results may be imperfect if the separation is noisy or the singer is difficult to understand.)

   - **Automatic Azure Speech recognition** - create timing automatically from a vocal-isolated audio file and the text of the lyrics, using free cloud-powered speech recognition.  (The original but least accurate option, still available for anyone for whom it's useful.)     
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/8b1e0c15-c267-43a3-b8b6-a25d7f8ebfdf" width="350"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/8ab3548d-ac7e-47c6-b833-4c17c5a79904" width="350"/>



 - Add, edit, and delete lines and syllables with easy-to-use controls, or even import a YouTube Movie Maker or Karaoke Builder Studio file to take advantage of time- or tedium-saving editing features not available in those apps.  
<img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/eedf4b4f-b434-4598-b8b8-3ad8eb87b338" width="700" />   
<img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/b295bf49-8aa5-4d94-9bb3-eb121320f17c" width="400" />  

    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/9546f8e9-3261-41a3-8b50-14ec31975f8e" width="250"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/32004c0d-c9da-404f-88ca-ae403cd9511f" width="250"/>
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/5a7213e1-0508-4f2e-a553-60af9767e861" width="250"/>



 - Export to YTMM or KBS to generate the final product, accelerating your karaoke creation workflow with templates and project generation features like automatic progress bars.  Or export to a `.json` file for [AutoSubs](https://autosubs.app/), the free [DaVinci Resolve](https://www.blackmagicdesign.com/products/davinciresolve) plugin — ideal for making lyric videos where words pop in one-by-one as they're sung.  (AutoSubs highlights one whole word at a time rather than a progress-bar-style highlight, so it's not currently well-suited for traditional karaoke, but it's a great fit for word-by-word lyric videos that would otherwise be very tedious to make manually.)

    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/7dc0a69c-8961-496d-bd70-e3d2ae2d79fa" width="350"/> ➤➤➤
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/65ade6d6-af49-4fda-af8c-201dd410266d" width="400"/>
    
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/9cf54209-0a2f-4264-8ba9-21cd30c41f79" width="350"/> ➤➤➤
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/ac56d509-3547-4627-8046-5da9675384c0" width="400"/>

    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/6d912e1a-1c73-43fa-9ea2-42de87ce37c0" width="350"/> ➤➤➤
    <img src="https://github.com/KaddaOK/KaddaOKTools/assets/151568451/1d42f310-81f0-4de7-9bcc-aca4a7b690b3" width="350"/>


    

## Get Started

Kadda OK Tools currently mainly targets YTMM and KBS, which are both Windows-only apps, and as such is only available for win-x64.  (Support for Mac OSX, and import/export to MidiCo and/or Kanto Syncro, are possible future features.)

Please find the latest win-x64 release [in the releases section](https://github.com/KaddaOK/KaddaOKTools/releases).  No installation should be required, just unzip and run.

## Known Issues:

 - The caret and word highlighting in the Edit Line Timing Dialog is sometimes not in proper visual sync when playing back.  This doesn't seem to affect recording, only playback.  Your mileage may vary.
   
 - When importing from then re-exporting to KBS, the following information will currently be **_lost_**:
   
   - Page display and removal modes (Line by Line vs Clear Screen vs Fade vs etc.)
   - Style settings (shadow, bold, etc.)
   - Palette indexes other than the default (0 is screen and border, 1-4 for style 01, 5-8 for style 02, 9-12 for style 03, 4/8/12/14 for style 04)
   - If you split any lines, the page breaks will be reset to ignorant 4-line pages, and you will need to hit the "Reset" button to correct line and page display/removal times.
     
 - Output colors selected in the export tab are not applied if you start with an import from YTMM.  This does not affect when you start from scratch nor when you import from KBS.
   
 - If you use the Clear All button and then import, the waveform phrase map will not be drawn correctly when the app first jumps to the Edit tab.  Resizing the window will correct it (or just restart the app when you want to work on another song).
   
 - When you import, only one audio file is specified, so the app will try to guess which one it is and find others in the same folder, without telling you it's doing that or asking you if the ones it found were the right ones. Then it whisks you off to the Edit tab immediately and you'd need to go back to the Audio tab to change them.

 
## Some Major Roadmap Items (not necessarily in any order):
 - Design/UX cleanup
 - Ability to access the new tap-to-time editing experience after completing it or with any other type of start
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
     - Mac OSX, MidiCo and Kanto Syncro support, eventually, maybe?

## Need Help?

If you discover a bug (don't be shy, there are many right now I'm sure!), and it's not mentioned in the above couple sections, please [create a bug report](https://github.com/KaddaOK/KaddaOKTools/issues).

Otherwise, for questions or comments, give me a shout in the [#kadda-ok](https://discord.com/channels/918644502128885760/1055115584007835668) channel on the diveBar discord.
  
## Contribute

Pull requests are welcome.
