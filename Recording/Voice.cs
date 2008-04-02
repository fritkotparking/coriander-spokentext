using System;
using System.Collections.Generic;
using System.Text;

namespace Coriander.SpokenText.Recording
{
    public struct Voice
    {
        readonly String value;
        public static readonly Voice Dave       = "dave";
        public static readonly Voice Avery      = "avery";
        public static readonly Voice Bob        = "bob";
        public static readonly Voice Pierre     = "pierre";
        public static readonly Voice Matthias   = "matthias";

        /// <summary>
        /// 
        /// </summary>
        public String Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// ctor : String
        /// </summary>
        /// <param name="value"></param>
        public Voice(String value)
        {
            this.value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Voice (String value)
        {
            return new Voice(value); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator String(Voice voice)
        {
            return voice.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value != null ? Value : String.Empty; 
        }
    }
}
