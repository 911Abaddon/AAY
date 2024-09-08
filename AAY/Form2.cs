using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAY
{
    public partial class Form2 : Form
    {
        private List<string> trackList = new List<string>();
        private bool isPlayingA = false;
        private bool isPlayingB = false;
        private Timer timerA;
        private Timer timerB;

        public Form2()
        {
            InitializeComponent();
            InitializeTimers();
            PopulateTrackList();
        }

        private void InitializeTimers()
        {
            timerA = new Timer() { Interval = 1000 };
            timerB = new Timer() { Interval = 1000 };
            timerA.Tick += (s, e) => SimulatePlayback(deck1, labelDeckA, ButtonPlayA);
            timerB.Tick += (s, e) => SimulatePlayback(deck2, labelDeckB, ButtonPlayB);
        }

        private void PopulateTrackList()
        {
            trackList.AddRange(new[] { "Track 1", "Track 2", "Track 3", "Track 4", "Track 5" });
            listBoxTracks.Items.AddRange(trackList.ToArray());
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ButtonPlayA.Click += ButtonPlayA_Click;
            ButtonPlayB.Click += ButtonPlayB_Click;
            listBoxTracks.SelectedIndexChanged += listBoxTracks_SelectedIndexChanged;

            buttonEffect1.Click += buttonEffect_Click;
            buttonEffect2.Click += buttonEffect_Click;
            buttonEffect3.Click += buttonEffect_Click;
            buttonEffect4.Click += buttonEffect_Click;

            trackBarCrossfader1.Scroll += trackBarCrossfader1_Scroll;
            trackBarCrossfader2.Scroll += trackBarCrossfader2_Scroll;
            trackBarCrossfader.Scroll += trackBar1_Scroll;
        }

        private void ButtonPlayA_Click(object sender, EventArgs e)
        {
            TogglePlayPause(ref isPlayingA, ButtonPlayA, timerA);
        }

        private void ButtonPlayB_Click(object sender, EventArgs e)
        {
            TogglePlayPause(ref isPlayingB, ButtonPlayB, timerB);
        }

        private void TogglePlayPause(ref bool isPlaying, Button playButton, Timer timer)
        {
            isPlaying = !isPlaying;
            playButton.Text = isPlaying ? "⏸" : "▶";
            if (isPlaying)
                timer.Start();
            else
                timer.Stop();
        }

        private void SimulatePlayback(Panel deck, Label deckLabel, Button playButton)
        {
            deck.BackColor = Color.FromArgb(new Random().Next(256), new Random().Next(256), new Random().Next(256));
            deckLabel.Text = $"Playing: {listBoxTracks.SelectedItem ?? "No track selected"}";
        }

        private void listBoxTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTracks.SelectedItem != null)
            {
                string selectedTrack = listBoxTracks.SelectedItem.ToString();
                if (isPlayingA)
                    labelDeckA.Text = $"Playing: {selectedTrack}";
                else if (isPlayingB)
                    labelDeckB.Text = $"Playing: {selectedTrack}";
            }
        }

        private void trackBarCrossfader1_Scroll(object sender, EventArgs e)
        {
            AdjustVolume(trackBarCrossfader1, deck1);
        }

        private void trackBarCrossfader2_Scroll(object sender, EventArgs e)
        {
            AdjustVolume(trackBarCrossfader2, deck2);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // Implement crossfader logic here
            int crossfaderValue = trackBarCrossfader.Value;
            // You can adjust the volume of both decks based on the crossfader value
            // For example:
            // deck1Volume = 100 - crossfaderValue;
            // deck2Volume = crossfaderValue;
        }

        private void AdjustVolume(TrackBar trackBar, Panel deck)
        {
            int volume = trackBar.Value;
            deck.BackColor = Color.FromArgb(volume, volume, volume);
        }

        private void buttonEffect_Click(object sender, EventArgs e)
        {
            Button effectButton = (Button)sender;
            effectButton.BackColor = effectButton.BackColor == Color.Red ? Color.FromArgb(24, 24, 24) : Color.Red;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Implementation for panelMixer paint event
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Implementation for panelEffects paint event
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Implementation for buttonLoop1 click event
        }
    }
}