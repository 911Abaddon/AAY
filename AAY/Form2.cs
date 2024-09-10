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
        private int bpmDeckA = 128; // Default BPM
        private int bpmDeckB = 128; // Default BPM
        private System.Windows.Forms.Timer timerBpmA;
        private System.Windows.Forms.Timer timerBpmB;
        private bool isLooping = false;
        private float loopStart;
        private float loopEnd;
        private System.Windows.Forms.Timer loopTimer;
        private System.Windows.Forms.Button activeLoopButton = null;







        public Form2()
        {
            InitializeComponent();
            InitializeTimers();
            PopulateTrackList();
            LoadTracksIntoListBox();
            InitializeLoopTimer(); // Add this line

            // Set the initial volume for Deck A to 50%
            volumeA = 0.5f;
            SetVolumeForDeckA(volumeA);

            // Set the initial volume for Deck B to 50%
            volumeB = 0.5f;
            SetVolumeForDeckB(volumeB);

            // Initialize the trackbar to start at 50%
            trackBarCrossfader1.Minimum = 0;
            trackBarCrossfader1.Maximum = 100;
            trackBarCrossfader1.Value = 50;

            trackBarCrossfader2.Minimum = 0;
            trackBarCrossfader2.Maximum = 100;
            trackBarCrossfader2.Value = 50;

            // Initialize the mix slider to start in the middle
            trackBarCrossfader.Minimum = 0;
            trackBarCrossfader.Maximum = 100;
            trackBarCrossfader.Value = 50;
            mixLevel = 0.5f;

            // Add event handlers for volume changes
            trackBarCrossfader1.Scroll += new EventHandler(trackBarCrossfader1_Scroll_1);
            trackBarCrossfader2.Scroll += new EventHandler(trackBarCrossfader2_Scroll_1);
            trackBarCrossfader.Scroll += new EventHandler(trackBar1_Scroll);

            UpdateBpmLabelA();
            UpdateBpmLabelB();

            timerBpmA = new System.Windows.Forms.Timer();
            timerBpmA.Interval = 2000; // Update every 2 seconds
            timerBpmA.Tick += TimerBpmA_Tick;

            timerBpmB = new System.Windows.Forms.Timer();
            timerBpmB.Interval = 2000; // Update every 2 seconds
            timerBpmB.Tick += TimerBpmB_Tick;

        }


        private void TimerBpmA_Tick(object sender, EventArgs e)
        {
            if (outputDeviceA != null && outputDeviceA.PlaybackState == PlaybackState.Playing)
            {
                Random rnd = new Random();
                int change = rnd.Next(-2, 3); // Random change between -2 and 2
                bpmDeckA = Math.Max(60, Math.Min(200, bpmDeckA + change)); // Keep BPM between 60 and 200
                UpdateBpmLabelA();
            }
        }

        private void TimerBpmB_Tick(object sender, EventArgs e)
        {
            if (outputDeviceB != null && outputDeviceB.PlaybackState == PlaybackState.Playing)
            {
                Random rnd = new Random();
                int change = rnd.Next(-2, 3); // Random change between -2 and 2
                bpmDeckB = Math.Max(60, Math.Min(200, bpmDeckB + change)); // Keep BPM between 60 and 200
                UpdateBpmLabelB();
            }
        }


        private void InitializeLoopTimer()
        {
            loopTimer = new System.Windows.Forms.Timer();
            loopTimer.Interval = 10; // Check every 10ms
            loopTimer.Tick += LoopTimer_Tick;
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

        private int DetectBPM(string filePath)
        {
            using (var audioFile = new AudioFileReader(filePath))
            {
                // Read the first 30 seconds of the track
                int sampleRate = audioFile.WaveFormat.SampleRate;
                int channels = audioFile.WaveFormat.Channels;
                int bytesPerSample = audioFile.WaveFormat.BitsPerSample / 8;
                int bufferSize = sampleRate * channels * bytesPerSample * 30; // 30 seconds
                byte[] buffer = new byte[bufferSize];
                audioFile.Read(buffer, 0, buffer.Length);

                // Convert byte array to float array
                float[] samples = new float[bufferSize / bytesPerSample];
                for (int i = 0; i < samples.Length; i++)
                {
                    samples[i] = BitConverter.ToSingle(buffer, i * bytesPerSample);
                }

                // Perform FFT
                Complex[] fftResults = new Complex[samples.Length];
                for (int i = 0; i < samples.Length; i++)
                {
                    fftResults[i].X = samples[i];
                }
                FastFourierTransform.FFT(true, (int)Math.Log(fftResults.Length, 2.0), fftResults);

                // Find the dominant frequency
                int maxIndex = 0;
                double maxMagnitude = 0;
                for (int i = 0; i < fftResults.Length / 2; i++)
                {
                    double magnitude = Math.Sqrt(fftResults[i].X * fftResults[i].X + fftResults[i].Y * fftResults[i].Y);
                    if (magnitude > maxMagnitude)
                    {
                        maxMagnitude = magnitude;
                        maxIndex = i;
                    }
                }

                // Calculate BPM
                double dominantFrequency = maxIndex * sampleRate / fftResults.Length;
                int estimatedBPM = (int)Math.Round(dominantFrequency * 60);

                // Ensure BPM is within a reasonable range (e.g., 60-200 BPM)
                return Math.Max(60, Math.Min(200, estimatedBPM));
            }
        }

        private void SetVolumeForDeckA(float volume)
        {
            if (audioFileA != null)
            {
                audioFileA.Volume = volume;
                Console.WriteLine($"Volume for Deck A set to: {volume * 100}%");
            }
            else
            {
                Console.WriteLine("Deck A audio file is null!");
            }
        }

        private void UpdateMix()
        {
            // Calculate volumes based on mix level
            float volumeAdjustedA = volumeA * (1 - mixLevel);
            float volumeAdjustedB = volumeB * mixLevel;

            // Apply the adjusted volumes
            SetVolumeForDeckA(volumeAdjustedA);
            SetVolumeForDeckB(volumeAdjustedB);

            Console.WriteLine($"Mix updated: A={volumeAdjustedA * 100}%, B={volumeAdjustedB * 100}%");
        }


        private void SetVolumeForDeckB(float volume)
        {
            if (audioFileB != null)
            {
                audioFileB.Volume = volume;
                Console.WriteLine($"Volume for Deck B set to: {volume * 100}%");
            }
            else
            {
                Console.WriteLine("Deck B audio file is null!");
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
                    StopBpmTimerA();
                    StopLoopEnhanced(); // Stop the loop when pausing
                }
                else
                {
                    LoadAndPlayAudioForDeckA(trackPath);
                    timerBpmA.Start();
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
            SetVolumeForDeckA(volumeA); // Set initial volume
            outputDeviceA.Play();
            ButtonPlayA.Text = "⏸";

            // Generate a random initial BPM between 120 and 140
            Random rnd = new Random();
            bpmDeckA = rnd.Next(120, 141);
            UpdateBpmLabelA();
            // Start the BPM update timer
            timerBpmA.Start();
        }
        private void UpdateBpmLabelA()
        {
            label2.Text = $"{bpmDeckA} BPM";
        }

        private void UpdateBpmLabelB()
        {
            label3.Text = $"{bpmDeckB} BPM";
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
            SetVolumeForDeckB(volumeB); // Set initial volume
            outputDeviceB.Play();
            ButtonPlayB.Text = "⏸";

            Random rnd = new Random();
            bpmDeckB = rnd.Next(120, 141);
            UpdateBpmLabelB();
            // Start the BPM update timer
            timerBpmB.Start();
        }

        private void StopBpmTimerA()
        {
            timerBpmA.Stop();
        }

        private void StopBpmTimerB()
        {
            timerBpmB.Stop();
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
            UpdateMix();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            mixLevel = trackBarCrossfader.Value / 100.0f;
            UpdateMix();
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
            loopTimer?.Dispose();
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
                    StopBpmTimerB();
                    StopLoopEnhanced(); // Stop the loop when pausing

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
            int sliderValue = trackBarCrossfader1.Value;
            volumeA = sliderValue / 100.0f;
            UpdateMix();
        }

        private void MuteDeckA()
        {
            if (outputDeviceA != null && outputDeviceA.PlaybackState == PlaybackState.Playing)
            {
                outputDeviceA.Pause();
                Console.WriteLine("Deck A is muted");
            }
        }

        private void UnmuteDeckA(float volume)
        {
            if (audioFileA != null && outputDeviceA != null)
            {
                audioFileA.Volume = volume;
                if (outputDeviceA.PlaybackState == PlaybackState.Paused)
                {
                    outputDeviceA.Play();
                    Console.WriteLine($"Resuming Deck A with Volume: {volume}");
                }
            }
        }
        // This function would actually change the volume for Deck A's audio stream


        private void button1_Click(object sender, EventArgs e)
        {
            // Loop for 1/4 beat
            ToggleLoop(0.25f, (System.Windows.Forms.Button)sender);  // 1/4 beat loop
        }

        private void listBoxTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Leave empty or add functionality for listBox item selection changed event
        }

        private void buttonLoop2_Click(object sender, EventArgs e)
        {
            ToggleLoop(0.5f, (System.Windows.Forms.Button)sender);
        }
        private void buttonLoop3_Click(object sender, EventArgs e)
        {
            ToggleLoop(1.0f, (System.Windows.Forms.Button)sender);
        }

        private void buttonLoop4_Click(object sender, EventArgs e)
        {
            ToggleLoop(2.0f, (System.Windows.Forms.Button)sender);
        }


        private void ToggleLoop(float beats, System.Windows.Forms.Button clickedButton)
        {
            if (audioFileA == null || outputDeviceA == null)
            {
                MessageBox.Show("Please load and play a track before using the loop function.", "No Audio Loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isLooping && clickedButton == activeLoopButton)
            {
                StopLoopEnhanced();
            }
            else
            {
                StartLoopEnhanced(beats, clickedButton);
            }
        }


        private void StartLoopEnhanced(float beats, System.Windows.Forms.Button clickedButton)
        {
            try
            {
                if (audioFileA == null || outputDeviceA == null)
                {
                    Console.WriteLine("Error: Audio not initialized");
                    return;
                }

                if (loopTimer == null)
                {
                    Console.WriteLine("Error: loopTimer is null");
                    InitializeLoopTimer();
                }

                if (isLooping)
                {
                    StopLoopEnhanced();
                }

                float bpm = bpmDeckA;
                float secondsPerBeat = 60f / bpm;
                float loopLength = beats * secondsPerBeat;

                loopStart = (float)audioFileA.CurrentTime.TotalSeconds;
                loopEnd = loopStart + loopLength;

                isLooping = true;
                loopTimer.Start();

                // Highlight the active loop button
                HighlightLoopButton(clickedButton);

                Console.WriteLine($"Loop started: Start={loopStart}, End={loopEnd}, BPM={bpm}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StartLoopEnhanced: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }


        private void StopLoopEnhanced()
        {
            isLooping = false;
            loopTimer.Stop();
            UnhighlightAllLoopButtons();
            activeLoopButton = null;
            Console.WriteLine("Loop stopped");
        }

        private void LoopTimer_Tick(object sender, EventArgs e)
        {
            if (!isLooping || audioFileA == null || outputDeviceA == null) return;

            if (audioFileA.CurrentTime.TotalSeconds >= loopEnd)
            {
                audioFileA.CurrentTime = TimeSpan.FromSeconds(loopStart);
                if (outputDeviceA.PlaybackState != PlaybackState.Playing)
                {
                    outputDeviceA.Play();
                }
            }
        }

        private void HighlightLoopButton(System.Windows.Forms.Button button)
        {
            UnhighlightAllLoopButtons();
            button.BackColor = Color.LightBlue;
            activeLoopButton = button;
        }

        private void UnhighlightAllLoopButtons()
        {
            buttonLoop2.BackColor = SystemColors.Control;
            buttonLoop3.BackColor = SystemColors.Control;
            buttonLoop4.BackColor = SystemColors.Control;
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

        private void panelLoops_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
