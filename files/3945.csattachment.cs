/// Word Wrapping method.
/// Copyright 2006, James M. Curran
/// May be used freely.

using System;
using System.Text;

namespace Curran.Utils
{
    /// <summary>
    /// Class to hold Wrap function.
    /// </summary>
    public class WordWrap
    {
        #region Public Methods
        /// <summary>
        /// Wraps the specified original text.
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <returns></returns>
        public static string Wrap(string originalText, int maxWidth)
        {
            return Wrap(originalText, maxWidth, "");
        } /* Wrap */

        /// <summary>
        /// Wraps the specified original text, and prepends the 
        /// specified prefix to each line
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="preFix">The prefix.</param>
        /// <returns></returns>
        public static string Wrap(string originalText, int maxWidth, string preFix)
        {
            // Convert exist CRs into line feeds.
            // (We'll respect existing LFs as forced line breaks)
            originalText = originalText.Replace("\r\n", "\n");
            originalText = originalText.Replace("\r", "\n");
            // Remove all existing tabs.
            originalText = originalText.Replace("\t", " ");

            string[] textParagraphs = originalText.Split('\n');
            StringBuilder wrappedBlock = new StringBuilder();

            for (int i = 0; i < textParagraphs.Length; i++)
            {
                string line = textParagraphs[i];

                // In the code below:
                // begin is the character position of the first char of the current line.
                // end is the position of the last possible character in the current line.
                // a & b will be the actual beginning position & length of the current line.

                int begin = 0;
                int end = maxWidth;
                int a = 0;
                int b = 0;

                while (end < line.Length)
                {
                    // First, we look for the last space before the max size.
                    int pos = line.LastIndexOf(' ', end);

                    if (pos < begin)
                    {   // If there is no space in that range, we just take all of it.
                        a = begin;
                        b = maxWidth;
                        begin = begin + maxWidth + 1;
                    }
                    else
                    {   // otherwise, we take everything between begin & that space.
                        a = begin;
                        b = pos - begin;
                        begin = pos + 1;
                    }

                    // Here's where we build the text block. The second Append is
                    // the important line.  It grabs the range of character from the 
                    // middle of the line string.
                    wrappedBlock.Append(preFix);
                    wrappedBlock.Append(line, a, b);
                    wrappedBlock.Append(Environment.NewLine);

                    // Finally, we reset end to our new max position.
                    end = begin + maxWidth;

                }
                // Don't forget the final line.
                wrappedBlock.Append(preFix);
                wrappedBlock.Append(line.Substring(begin));
                wrappedBlock.Append(Environment.NewLine);
            }

//          wrappedBlock.Length-= Environment.NewLine.Length;		// remove final \r\n
            return wrappedBlock.ToString();
        }
        #endregion
    } /* WordWrap */
}
