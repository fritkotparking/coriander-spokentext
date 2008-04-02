using System;
using System.Collections.Generic;
using System.Text;
using Rest;
using Rest.Transport;
using System.IO;
using System.Net;
using System.Diagnostics;
using Rest.Http;
using Coriander.SpokenText.Recording;

namespace Coriander.SpokenText
{
    /// <summary>
    /// 
    /// </summary>
    public class SpokenTextClient
    {
        const String ServiceUrl = "http://www.spokentext.net/s_record_file_eng.php";
        const String RecordServiceUrl = "http://www.spokentext.net/s_record_file.php";
        const String RecordByEnteredTextServiceUrl = "http://www.spokentext.net/s_record_entered_text_eng.php";
        const String LoginUrl = "http://www.spokentext.net/login_eng.php";

        /// <summary>
        /// Creates RestCommand for POST operations. Most operations share a bunch of RestParameters.
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        static RestCommand CreatePostCommand(
            String url, 
            Options options,
            CookieCollection cookies
        ) 
        {
            RestCommand command               = new RestCommand(url, RestTransport.Post);
            command.Transport.HttpContentType = HttpContentType.MultipartFormData;

            command.Parameters.Add("voice",         options.Voice);
            command.Parameters.Add("wpm",           options.Wpm.ToString());
            command.Parameters.Add("volume",        options.Volume.ToString());
            command.Parameters.Add("emailMe",       options.EmailMe ? "1" : "0");
            command.Parameters.Add("public",        options.IsPublic ? "1" : "0");

            command.Parameters.Add("submit",        "Record");
            command.Parameters.Add("specialWords",  String.Empty);

            command.Cookies.Add(cookies); 

            return command;
        }

        /// <summary>
        /// Records the supplied file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="options"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static Byte[] Record(
            String filename, 
            Options options, 
            CookieCollection cookies
        )
        {
            if (null == filename)
                throw new ArgumentNullException(filename);

            return Record(File.ReadAllBytes(filename), options, cookies); 
        }

        /// <summary>
        /// Records the supplied file
        /// </summary>
        /// <remarks>See: http://www.spokentext.net/s_record_file.php?.</remarks>
        /// <param name="file"></param>
        /// <param name="options"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static Byte[] Record(
            Byte[] file, 
            Options options, 
            CookieCollection cookies
        ) 
        {
            if (null == file)
                throw new ArgumentNullException("file");

            if (file.Length == 0)
                throw new ArgumentException("Unexpected empty file.");

            if (null == cookies)
                throw new ArgumentNullException("sessionId");

            RestCommand command = CreatePostCommand(
                RecordServiceUrl, 
                options, 
                cookies
            );

            command.Data.Add("uploadFile", file);

            HttpWebResponse response = command.Execute();
             
            // (2) TODO: Fetch back the MP3. This takes time anyway.
            return new Byte[1] { 0x0 };
        }

        /// <summary>
        /// Records the supplied text
        /// </summary>
        /// <remarks>
        /// For some reason this records URL encoded text, it is not decoded at the other end for some reason.
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="options"></param>
        /// <param name="cookies"></param>
        public static void RecordText(
            String name, 
            String text,
            Options options,
            CookieCollection cookies
        ) 
        { 
            if (null == name || null == text)
                throw new ArgumentNullException(
                    null == name ? "name" : "text"
                );

            RestCommand command = CreatePostCommand(
                RecordByEnteredTextServiceUrl, 
                options,
                cookies
            );

            command.Parameters.Add("recordingNameEnteredText", name);  
            command.Parameters.Add("text", text);

            // Ignore redirect, just takes us to view recordings.
            command.AllowAutoRedirect = false; 

            command.Execute();
        }


        # region * Login *

        /// <summary>
        /// Logs the supplied user and and returns session information as a set of cookies.
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public static CookieCollection Login(NetworkCredential credential)
        {
            RestCommand command = new RestCommand(LoginUrl, RestTransport.Post); 
            command.Transport.HttpContentType = HttpContentType.MultipartFormData;
            command.Parameters.Add("username",      credential.UserName);
            command.Parameters.Add("password",      credential.Password);
            command.Parameters.Add("chk_remember",  "on");
            command.Parameters.Add("Submit",        "Login");

            command.AllowAutoRedirect = false;

            HttpWebResponse response = command.Execute();

            foreach (String header in response.Headers)
            {
                Debug.WriteLine(header + ", type: " + header.GetType().Name); 
            }

            Debug.Assert(
                response.Cookies["authchallenge"] != null, 
                "Expected response to include authchallenge cookie."
            ); 

            return response.Cookies;
        }

        # endregion
    }
}
