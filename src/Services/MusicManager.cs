using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace MysticChronicles.Services
{
    public enum MusicTrack
    {
        MainMenu,
        Exploration,
        Battle,
        Victory,
        GameOver
    }

    public static class MusicManager
    {
        private static MediaPlayer mediaPlayer;
        private static MusicTrack? currentTrack;

        static MusicManager()
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.IsLoopingEnabled = true;
            mediaPlayer.Volume = 0.5; // 50% volume
        }

        public static async void PlayMusic(MusicTrack track)
        {
            // Don't restart if already playing this track
            if (currentTrack == track && mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
            {
                return;
            }

            currentTrack = track;

            try
            {
                string filename = null;

                switch (track)
                {
                    case MusicTrack.MainMenu:
                        filename = "MainMenu.mp3";
                        break;
                    case MusicTrack.Exploration:
                        filename = "Exploration.mp3";
                        break;
                    case MusicTrack.Battle:
                        filename = "Battle.mp3";
                        break;
                    case MusicTrack.Victory:
                        filename = "Victory.mp3";
                        break;
                    case MusicTrack.GameOver:
                        filename = "GameOver.mp3";
                        break;
                }

                if (filename == null)
                {
                    return;
                }

                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\Music");
                var file = await folder.GetFileAsync(filename);

                mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);

                // Victory and GameOver should not loop
                mediaPlayer.IsLoopingEnabled = track != MusicTrack.Victory && track != MusicTrack.GameOver;

                mediaPlayer.Play();
            }
            catch (Exception)
            {
                // Music file not found, continue silently
            }
        }

        public static void StopMusic()
        {
            mediaPlayer.Pause();
            currentTrack = null;
        }

        public static void SetVolume(double volume)
        {
            mediaPlayer.Volume = Math.Clamp(volume, 0.0, 1.0);
        }

        public static void Cleanup()
        {
            mediaPlayer?.Dispose();
        }
    }
}
