using System;
using System.Collections.Generic;
using System.Text;

namespace Coriander.SpokenText.Recording
{
    /// <summary>
    /// 
    /// </summary>
    public class Options
    {
        Voice voice;
        Int32 wpm           = 150;
        Int32 volume        = 100;
        Boolean emailMe     = false;
        Boolean isPublic    = false;

        /// <summary>
        /// Gets or sets a value indicating an email should be sent on completion of conversion. 
        /// Defaults to false.
        /// </summary>
        public Boolean EmailMe
        {
            get { return emailMe; }
            set { emailMe = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the resultant file should be public.
        /// Defaults to false.
        /// </summary>
        public Boolean IsPublic
        {
            get { return isPublic; }
            set { isPublic = value; }
        }

        /// <summary>
        /// Gets or sets the volume. Must be between zero and 100. Defaults to 100.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public Int32 Volume
        {
            get { return volume; }
            set 
            {
                if (value < 0 || 100 < value)
                    throw new InvalidOperationException(
                        "Volumn value must be between zero and 100."
                    );
                
                volume = value; 
            }
        }

        /// <summary>
        /// Words per minute reading speed. Defaults to 150.
        /// </summary>
        public Int32 Wpm
        {
            get { return wpm; }
            set { wpm = value; }
        }

        /// <summary>
        /// The voice to use
        /// </summary>
        public Voice Voice
        {
            get { return voice; }
            set { voice = value; }
        }

        /// <summary>
        /// ctor 
        /// </summary>
        /// <param name="voice"></param>
        public Options() : this(Voice.Dave) { }

        /// <summary>
        /// ctor : Voice, Int32, Int32 
        /// </summary>
        /// <param name="voice"></param>
        public Options(Voice voice) 
        {
            this.voice = voice;
        }

        /// <summary>
        /// ctor : Voice, Int32, Int32 
        /// </summary>
        /// <param name="voice"></param>
        public Options(Voice voice, Int32 wpm, Int32 volume) : this(voice)
        {
            this.wpm    = wpm;
            this.volume = volume;
        }
    }
}
