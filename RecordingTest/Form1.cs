using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using System.IO;
using System.Reflection;

namespace RecordingTest
{
    public partial class Form1 : Form
    {
        const string VIDEO_URL1 = "rtsp://192.168.0.91/axis-media/media.amp";//
        const string VIDEO_URL2 = "rtsp://admin:admin@192.168.1.168/defaultPrimary?streamtype=u";
        const string VIDEO_URL3 = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";
        readonly LibVLC _libvlc;

        public Form1()
        {
            InitializeComponent();
            // Record in a file "record.ts" located in the bin folder next to the app
            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var destination1 = Path.Combine(currentDirectory, "record.ts");
            var destination2 = Path.Combine(currentDirectory, "record2.ts");
            var destination3 = Path.Combine(currentDirectory, "record3.ts");

            // this will load the native libvlc library (if needed, depending on the platform). 
            Core.Initialize();

            // instanciate the main libvlc object
            _libvlc = new LibVLC();

            // Redirect log output to the console
            _libvlc.Log += (sender, e) => Console.WriteLine($"[{e.Level}] {e.Module}:{e.Message}");

            // Create new media with HLS link
            Media media = new Media(_libvlc, VIDEO_URL1, FromType.FromLocation);
            Media media2 = new Media(_libvlc, VIDEO_URL2, FromType.FromLocation);
            Media media3 = new Media(_libvlc, VIDEO_URL3, FromType.FromLocation);

            // Define stream output options. 
            // In this case stream to a file with the given path and play locally the stream while streaming it.
            media.AddOption(":sout=#transcode{scodec=none}:duplicate{dst=display,dst=std{access=file,mux=ts,dst='" + destination1 + "'}}");
            media.AddOption(":no-sout-all:sout-keep");
            media2.AddOption(":sout=#transcode{scodec=none}:duplicate{dst=display,dst=std{access=file,mux=ts,dst='" + destination2 + "'}}");
            media2.AddOption(":no-sout-all:sout-keep");
            media3.AddOption(":sout=#transcode{scodec=none}:duplicate{dst=display,dst=std{access=file,mux=ts,dst='" + destination3 + "'}}");
            media3.AddOption(":no-sout-all:sout-keep");

            // Start recording
            videoView1.MediaPlayer = new MediaPlayer(_libvlc);
            videoView1.MediaPlayer.Play(media);
            videoView2.MediaPlayer = new MediaPlayer(_libvlc);
            videoView2.MediaPlayer.Play(media2);
            videoView3.MediaPlayer = new MediaPlayer(_libvlc);
            videoView3.MediaPlayer.Play(media3);
        }
    }
}
