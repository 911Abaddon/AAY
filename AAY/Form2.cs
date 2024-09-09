using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Dsp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AAY
{
    public partial class Form2 : Form
    {
        private List<string> trackList = new List<string>();
       
        private bool isEchoOn = false;
        private bool isFlangerOn = false;
        private bool isReverbOn = false;
        private bool isFilterOn = false;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private WaveOutEvent outputDeviceA;
        private WaveOutEvent outputDeviceB;
        private AudioFileReader audioFileA;
        private AudioFileReader audioFileB;
        float volumeA = 0.5f;  // Default to 50%
        float volumeB = 0.5f;  // Default to 50%
        float mixLevel = 0.5f; // Default to 50% 
        private WaveOutEvent deckAPlayer;
        private WaveOutEvent deckBPlayer;
        private AudioFileReader deckAReader;
        private AudioFileReader deckBReader;

        public Form2()
        {
            InitializeComponent();
            InitializeTimers();
            PopulateTrackList();
            LoadTracksIntoListBox();

            // Set the initial volume for Deck A to 50%
            SetVolumeForDeckA(0.5f); // Initial volume at 50%

            // Initialize the trackbar to start at 50%
            trackBarCrossfader1.Minimum = 0;  // Ensure minimum is set to 0
            trackBarCrossfader1.Maximum = 100;  // Ensure maximum is set to 100
            trackBarCrossfader1.Value = 50;  // Start the slider at 50%

            // Add event handler for volume change
            trackBarCrossfader1.Scroll += new EventHandler(trackBarCrossfader1_Scroll_1);
        }


        private void InitializeTimers()
        {
            // Timer initialization
        }

        private void LoadTracksIntoListBox()
        {
            string trackFolder = Path.Combine(Application.StartupPath, "Tracks");
            if (Directory.Exists(trackFolder))
            {
                string[] musicFiles = Directory.GetFiles(trackFolder, "*.mp3");
                foreach (string file in musicFiles)
                {
                    listBoxTracks.Items.Add(Path.GetFileName(file));
                }
            }
            else
            {
                MessageBox.Show("Tracks folder not found. Please create a 'Tracks' folder in the application directory and add some MP3 files.");
            }
        }

        private void InitializePlayers()
        {
            deckAReader = new AudioFileReader("path_to_deckA_track.mp3");
            deckBReader = new AudioFileReader("path_to_deckB_track.mp3");

            deckAPlayer = new WaveOutEvent();
            deckBPlayer = new WaveOutEvent();

            deckAPlayer.Init(deckAReader);
            deckBPlayer.Init(deckBReader);

            deckAPlayer.Play();
            deckBPlayer.Play();
        }

        private void SetVolumeForDeckA(float volume)
        {
            if (deckAReader != null)
            {
                // Set the volume (0.0 for mute, 1.0 for full volume)
                deckAReader.Volume = volume;

                // Debugging: Print out the volume level to check
                Console.WriteLine($"Volume for Deck A set to: {volume * 100}%");
            }
            else
            {
                Console.WriteLine("Deck A Reader is null!");
            }
        }


        private void SetVolumeForDeckB(float volume)
        {
            if (deckBReader != null)
            {
                deckBReader.Volume = volume;  // Set volume for Deck B
            }
        }

        private void LoadAndPlayAudio(string filePath)
        {
            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }

            audioFile = new AudioFileReader(filePath);
            outputDevice = new WaveOutEvent();
            ApplyEffects();
        }

        private void PopulateTrackList()
        {
            // Populate the track list logic
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Form load event
        }

        private void ButtonPlayA_Click(object sender, EventArgs e)
        {
            if (listBoxTracks.SelectedItem == null)
            {
                MessageBox.Show("Please select a track first.");
                return;
            }

            string selectedTrack = listBoxTracks.SelectedItem.ToString();
            string trackPath = Path.Combine(Application.StartupPath, "Tracks", selectedTrack);

            if (File.Exists(trackPath))
            {
                if (audioFileA != null && outputDeviceA != null && outputDeviceA.PlaybackState == PlaybackState.Playing)
                {
                    outputDeviceA.Pause();
                    ButtonPlayA.Text = "▶";
                }
                else
                {
                    LoadAndPlayAudioForDeckA(trackPath);
                }
            }
            else
            {
                MessageBox.Show("Selected track file not found.");
            }
        }

        private void LoadAndPlayAudioForDeckA(string filePath)
        {
            if (outputDeviceA != null)
            {
                outputDeviceA.Stop();
                outputDeviceA.Dispose();
            }

            if (audioFileA != null)
            {
                audioFileA.Dispose();
            }

            audioFileA = new AudioFileReader(filePath);
            outputDeviceA = new WaveOutEvent();
            outputDeviceA.Init(audioFileA);
            outputDeviceA.Play();
            ButtonPlayA.Text = "⏸";
        }

        private void LoadAndPlayAudioForDeckB(string filePath)
        {
            if (outputDeviceB != null)
            {
                outputDeviceB.Stop();
                outputDeviceB.Dispose();
            }

            if (audioFileB != null)
            {
                audioFileB.Dispose();
            }

            audioFileB = new AudioFileReader(filePath);
            outputDeviceB = new WaveOutEvent();
            outputDeviceB.Init(audioFileB);
            outputDeviceB.Play();
            ButtonPlayB.Text = "⏸";
        }

        private void LoadAudioForDeckA(string filePath)
        {
            if (outputDeviceA != null)
            {
                outputDeviceA.Stop();
                outputDeviceA.Dispose();
                outputDeviceA = null;
            }

            if (audioFileA != null)
            {
                audioFileA.Dispose();
            }

            audioFileA = new AudioFileReader(filePath);
            ButtonPlayA.Text = "▶";
        }

        private void LoadAudioForDeckB(string filePath)
        {
            if (outputDeviceB != null)
            {
                outputDeviceB.Stop();
                outputDeviceB.Dispose();
                outputDeviceB = null;
            }

            if (audioFileB != null)
            {
                audioFileB.Dispose();
            }

            audioFileB = new AudioFileReader(filePath);
            ButtonPlayB.Text = "▶";
        }

        private void trackBarCrossfader1_Scroll(object sender, EventArgs e)
        {
            System.Windows.Forms.TrackBar trackBar = (System.Windows.Forms.TrackBar)sender;
            AdjustVolume(trackBar, deck1);
        }

        private void trackBarCrossfader2_Scroll_1(object sender, EventArgs e)
        {
            int sliderValue = trackBarCrossfader2.Value;
            volumeB = sliderValue / 100.0f;
            SetVolumeForDeckB(volumeB);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
         
        }


        private void AdjustVolume(System.Windows.Forms.TrackBar trackBar, System.Windows.Forms.Panel deck)
        {
            int volume = trackBar.Value;
            deck.BackColor = Color.FromArgb(volume, volume, volume);
        }


        private void buttonEffect_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button effectButton = (System.Windows.Forms.Button)sender;
            effectButton.BackColor = effectButton.BackColor == Color.Red ? Color.FromArgb(24, 24, 24) : Color.Red;
        }

        private void buttonEffect1_Click(object sender, EventArgs e)
        {
            isEchoOn = !isEchoOn;
            buttonEffect1.BackColor = isEchoOn ? Color.LightBlue : SystemColors.Control;
            ApplyEffects();
        }

        private void buttonEffect2_Click(object sender, EventArgs e)
        {
            isFlangerOn = !isFlangerOn;
            buttonEffect2.BackColor = isFlangerOn ? Color.LightBlue : SystemColors.Control;
            ApplyEffects();
        }

        private void buttonEffect3_Click(object sender, EventArgs e)
        {
            isReverbOn = !isReverbOn;
            buttonEffect3.BackColor = isReverbOn ? Color.LightBlue : SystemColors.Control;
            ApplyEffects();
        }

        private void buttonEffect4_Click(object sender, EventArgs e)
        {
            isFilterOn = !isFilterOn;
            buttonEffect4.BackColor = isFilterOn ? Color.LightBlue : SystemColors.Control;
            ApplyEffects();
        }

        private void ApplyEffects()
        {
            if (audioFile == null) return;

            ISampleProvider sampleProvider = audioFile;

            if (isEchoOn)
            {
                sampleProvider = new PassThroughSampleProvider(sampleProvider);
            }

            if (isFlangerOn)
            {
                sampleProvider = new PassThroughSampleProvider(sampleProvider);
            }

            if (isReverbOn)
            {
                sampleProvider = new PassThroughSampleProvider(sampleProvider);
            }

            if (isFilterOn)
            {
                sampleProvider = new PassThroughSampleProvider(sampleProvider);
            }

            if (outputDevice != null)
            {
                outputDevice.Stop();
                outputDevice.Init(sampleProvider);
                outputDevice.Play();
            }
        }

        public class PassThroughSampleProvider : ISampleProvider
        {
            private readonly ISampleProvider source;

            public PassThroughSampleProvider(ISampleProvider source)
            {
                this.source = source;
            }

            public WaveFormat WaveFormat => source.WaveFormat;

            public int Read(float[] buffer, int offset, int count)
            {
                return source.Read(buffer, offset, count);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            outputDeviceA?.Dispose();
            outputDeviceB?.Dispose();
            audioFileA?.Dispose();
            audioFileB?.Dispose();
            base.OnFormClosing(e);
        }

        private void ButtonPlayB_Click_1(object sender, EventArgs e)
        {
            if (listBoxTracks.SelectedItem == null)
            {
                MessageBox.Show("Please select a track first.");
                return;
            }

            string selectedTrack = listBoxTracks.SelectedItem.ToString();
            string trackPath = Path.Combine(Application.StartupPath, "Tracks", selectedTrack);

            if (File.Exists(trackPath))
            {
                if (audioFileB != null && outputDeviceB != null && outputDeviceB.PlaybackState == PlaybackState.Playing)
                {
                    outputDeviceB.Pause();
                    ButtonPlayB.Text = "▶";
                }
                else
                {
                    LoadAndPlayAudioForDeckB(trackPath);
                }
            }
            else
            {
                MessageBox.Show("Selected track file not found.");
            }
        }

        // Add these methods in Form2.cs
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Leave empty or add functionality if needed
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Leave empty or add functionality if needed
        }

        private void deck1_Paint(object sender, PaintEventArgs e)
        {
            // Leave empty or add functionality if needed
        }

        private void trackBarCrossfader1_Scroll_1(object sender, EventArgs e)
        {
            // Get slider value (range from 0 to 100)
            int sliderValue = trackBarCrossfader1.Value;

            // Convert the slider value to a float between 0.0 and 1.0 for volume control
            float volumeA = sliderValue / 100.0f;

            // Set the volume for Deck A (will be 0 if sliderValue is 0)
            SetVolumeForDeckA(volumeA);
        }


        private void MuteDeckA()
        {
            if (outputDeviceA != null && outputDeviceA.PlaybackState == PlaybackState.Playing)
            {
                // Pause the audio to simulate mute
                outputDeviceA.Pause();
                Console.WriteLine("Deck A is muted");
            }
        }

        private void UnmuteDeckA(float volume)
        {
            if (deckAReader != null && outputDeviceA != null)
            {
                // Set volume and resume playback if paused
                deckAReader.Volume = volume;
                if (outputDeviceA.PlaybackState == PlaybackState.Paused)
                {
                    outputDeviceA.Play();  // Resume playing when unmuting
                    Console.WriteLine($"Resuming Deck A with Volume: {volume}");
                }
            }
        }
        // This function would actually change the volume for Deck A's audio stream


        private void button1_Click(object sender, EventArgs e)
        {
            // Loop for 1/4 beat
            StartLoop(0.25f);  // 1/4 beat loop
        }

        private void listBoxTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Leave empty or add functionality for listBox item selection changed event
        }

        private void buttonLoop2_Click(object sender, EventArgs e)
        {
            // Loop for 1/2 beat
            StartLoop(0.5f);  // 1/2 beat loop
        }

        private void buttonLoop3_Click(object sender, EventArgs e)
        {
            // Loop for 1 beat
            StartLoop(1.0f);  // 1 beat loop
        }

        private void buttonLoop4_Click(object sender, EventArgs e)
        {
            // Loop for 2 beats
            StartLoop(2.0f);  // 2 beat loop
        }

        private void StartLoop(float beats)
        {
            if (audioFileA != null)
            {
                // Calculate the duration of one beat based on the track's BPM
                float bpm = 128f; // Example BPM (this should be dynamically calculated or obtained)
                float secondsPerBeat = 60f / bpm;

                // Loop length in seconds
                float loopLength = beats * secondsPerBeat;

                // Get the current playback position
                float currentPosition = (float)audioFileA.CurrentTime.TotalSeconds;

                // Define the loop end time
                float loopEnd = currentPosition + loopLength;

                // Start looping
                LoopSection(currentPosition, loopEnd);
            }
        }


        private void LoopSection(float loopStart, float loopEnd)
        {
            if (outputDeviceA != null && audioFileA != null)
            {
                outputDeviceA.PlaybackStopped += (s, e) =>
                {
                    if (audioFileA.CurrentTime.TotalSeconds >= loopEnd)
                    {
                        // Loop back to the start of the loop
                        audioFileA.CurrentTime = TimeSpan.FromSeconds(loopStart);
                        outputDeviceA.Play();
                    }
                };

                // Set the playback position to the loop start and play
                audioFileA.CurrentTime = TimeSpan.FromSeconds(loopStart);
                outputDeviceA.Play();
            }
        }

        private void PlayEffect(string filePath, int durationInSeconds = 5)
        {
            // Check if a sound is already playing for this effect and stop it before restarting
            if (activePlayers.ContainsKey(filePath))
            {
                activePlayers[filePath].Stop();
                activePlayers[filePath].Dispose();
                activePlayers.Remove(filePath);
            }

            if (File.Exists(filePath))
            {
                var outputDevice = new WaveOutEvent();
                var audioFile = new AudioFileReader(filePath);

                outputDevice.Init(audioFile);
                outputDevice.Play();

                // Keep track of the currently playing sound
                activePlayers[filePath] = outputDevice;

                // Set a timer to stop the sound after a specified duration (e.g., 5 seconds)
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = durationInSeconds * 1000;  // Convert seconds to milliseconds
                timer.Tick += (s, e) =>
                {
                    outputDevice.Stop();
                    outputDevice.Dispose();
                    audioFile.Dispose();
                    activePlayers.Remove(filePath);
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();

                // Dispose the resources when playback is manually stopped or finished
                outputDevice.PlaybackStopped += (s, e) =>
                {
                    if (activePlayers.ContainsKey(filePath))
                    {
                        activePlayers.Remove(filePath);
                    }
                    outputDevice.Dispose();
                    audioFile.Dispose();
                };
            }
            else
            {
                Console.WriteLine($"File {filePath} not found.");
            }
        }

        private Dictionary<string, WaveOutEvent> activePlayers = new Dictionary<string, WaveOutEvent>();


        private void button1_Click_1(object sender, EventArgs e)
        {
            PlayEffect("Effects/siren.mp3");  // Plays a siren sound effect when button1 is clicked
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlayEffect("Effects/airhorn.mp3", 5);  // Plays an airhorn sound effect when button2 is clicked
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PlayEffect("Effects/laser.mp3", 5);  // Plays a laser sound effect when button3 is clicked
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PlayEffect("Effects/explosion.mp3", 5);  // Plays an explosion sound effect when button4 is clicked
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PlayEffect("Effects/horn.mp3", 5);  // Plays an explosion sound effect when button4 is clicked
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PlayEffect("Effects/crowd_cheer.mp3", 5);  // Plays an explosion sound effect when button4 is clicked
        }

        private void button7_Click(object sender, EventArgs e)
        {
            PlayEffect("Effects/drumroll.mp3", 5);  // Plays an explosion sound effect when button4 is clicked
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PlayEffect("Effects/scratch.mp3", 5);  // Plays an explosion sound effect when button4 is clicked
        }
    }
}
